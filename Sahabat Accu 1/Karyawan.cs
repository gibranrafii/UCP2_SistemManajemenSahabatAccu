using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions; 
using System.IO;
using NPOI.SS.UserModel; 
using NPOI.XSSF.UserModel; 

namespace Sahabat_Accu_1
{
    public partial class Karyawan : Form
    {
        private string connectionString = "Data Source=LAPTOP-QT79LBKA\\GIBRANRAFI;Initial Catalog=SistemManajemenSahabatAccu;Integrated Security=True;Pooling=False";
        private Form1 parentForm;

        public Karyawan(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        // Mengubah ShowError agar bisa menerima string title juga, dan detail Exception
        private void ShowError(string message, string title = "Terjadi Kesalahan", Exception ex = null)
        {
            string fullMessage = message;
            string finalTitle = title; // Gunakan variabel terpisah untuk title

            // Tangani error SQL spesifik dengan pesan yang lebih user-friendly
            if (ex is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627: // Primary Key violation (misal: id_karyawan sudah ada)
                        fullMessage = "ID Karyawan sudah ada. Masukkan ID lain atau gunakan fitur update.";
                        finalTitle = "Peringatan Input Duplikat";
                        break;
                    case 547: // Constraint violation (misal: CHECK constraint atau Foreign Key)
                        if (sqlEx.Message.Contains("CHECK constraint 'CK__Karyawan__id_ka__")) // Validasi format ID Karyawan
                        {
                            fullMessage = "Format ID Karyawan tidak sesuai. Harus 'KR' diikuti 3 angka (contoh: KR001).";
                        }
                        else if (sqlEx.Message.Contains("CHECK constraint 'CK__Karyawan__no_hp__")) // Validasi format No HP
                        {
                            fullMessage = "Format No HP tidak sesuai. Harus diawali '08' dan panjang antara 10-13 digit.";
                        }
                        else if (sqlEx.Message.Contains("FK__Detail_La__id_ka__")) // Foreign Key dari Detail_Layanan ke Karyawan
                        {
                            fullMessage = "Karyawan ini tidak dapat dihapus karena masih digunakan dalam Detail Layanan atau tabel lain. Harap periksa tabel terkait.";
                        }
                        else
                        {
                            fullMessage = $"Terjadi pelanggaran constraint: {sqlEx.Message}";
                        }
                        finalTitle = "Kesalahan Validasi Data Database";
                        break;
                    case 515: // Cannot insert NULL into NOT NULL column (misal: nama tidak diisi)
                        if (sqlEx.Message.Contains("nama"))
                        {
                            fullMessage = "Nama karyawan tidak boleh kosong.";
                        }
                        else if (sqlEx.Message.Contains("alamat"))
                        {
                            fullMessage = "Alamat karyawan tidak boleh kosong."; // Harusnya dicegah oleh validasi client-side baru
                        }
                        else
                        {
                            fullMessage = "Ada kolom yang wajib diisi namun kosong. Mohon periksa kembali input Anda.";
                        }
                        finalTitle = "Kesalahan Input Data";
                        break;
                    case 8152: // String or binary data would be truncated (misal: nama terlalu panjang)
                        fullMessage = "Ada input yang terlalu panjang untuk kolom database. Mohon periksa kembali panjang data.";
                        finalTitle = "Kesalahan Panjang Data";
                        break;
                    default:
                        // Untuk error SQL lainnya, tampilkan detail error untuk debugging
                        fullMessage += $"\n\nDetail Error Database:\nSQL Error Code: {sqlEx.Number}";
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            fullMessage += $"\n  Line: {error.LineNumber}, Msg: {error.Message}";
                        }
                        finalTitle = "Kesalahan Database Umum";
                        break;
                }
            }
            else if (ex != null)
            {
                // Untuk non-SQL exception, tampilkan pesan umum
                fullMessage += $"\n\nDetail Error Umum:\n{ex.Message}";
                finalTitle = "Kesalahan Aplikasi Umum";
            }

            MessageBox.Show(fullMessage, finalTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    
        private string GetSqlErrorMessage(SqlException sqlEx)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Kode Error: {sqlEx.Number}");
            if (!string.IsNullOrEmpty(sqlEx.Procedure))
            {
                sb.AppendLine($"Prosedur/Objek: {sqlEx.Procedure}");
            }
            if (sqlEx.Errors.Count > 0)
            {
                sb.AppendLine("Detail Kesalahan SQL:");
                foreach (SqlError error in sqlEx.Errors)
                {
                    sb.AppendLine($"  Baris: {error.LineNumber}, Nomor: {error.Number}, Pesan: {error.Message}");
                }
            }
            return sb.ToString();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT id_karyawan, nama, no_hp, alamat FROM Karyawan", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvKaryawan.DataSource = dt;
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError("Gagal memuat data karyawan dari database.", "Kesalahan Memuat Data", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data karyawan.", "Kesalahan Umum", ex);
            }
        }

        private void ClearForm()
        {
            txtIdKaryawan.Clear();
            txtNama.Clear();
            txtHp.Clear();
            txtAlamat.Clear();
            txtIdKaryawan.Focus();
        }

        private void Karyawan_Load(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtIdKaryawan.Text))
            {
                MessageBox.Show("ID Karyawan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdKaryawan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("Nama wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNama.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtHp.Text))
            {
                MessageBox.Show("No HP wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHp.Focus();
                return;
            }
            //  Tambahan Validasi untuk Alamat (mencegah string kosong) 
            if (string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                MessageBox.Show("Alamat wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlamat.Focus();
                return;
            }
            // 

            // Validasi format ID Karyawan
            if (!Regex.IsMatch(txtIdKaryawan.Text.Trim(), "^KR[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Karyawan tidak sesuai. Harus 'KR' diikuti 3 angka (contoh: KR001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdKaryawan.Focus();
                return;
            }

            // Validasi No HP
            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai. Harus diawali '08' dan panjang antara 10-13 digit.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHp.Focus();
                return;
            }
            // --- Akhir Validasi Input Client-Side ---

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("INSERT_Karyawan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_karyawan", txtIdKaryawan.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim()); // Mengirim string, bukan DBNull.Value
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data karyawan berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Gagal menambahkan data karyawan ke database.", "Kesalahan Penambahan Data Karyawan", sqlEx);
                    // Fokuskan control setelah ShowError dipanggil
                    if (sqlEx.Number == 2627 || (sqlEx.Number == 547 && sqlEx.Message.Contains("CK__Karyawan__id_ka__")))
                    {
                        txtIdKaryawan.Focus();
                    }
                    else if (sqlEx.Number == 515)
                    {
                        if (sqlEx.Message.Contains("'nama'")) txtNama.Focus();
                        else if (sqlEx.Message.Contains("'no_hp'")) txtHp.Focus();
                        else if (sqlEx.Message.Contains("'alamat'")) txtAlamat.Focus(); // Fokus ke alamat jika error dari DB
                    }
                    else if (sqlEx.Number == 8152)
                    {
                        txtNama.Focus();
                    }
                    else if (sqlEx.Number == 547 && sqlEx.Message.Contains("CK__Karyawan__no_hp__"))
                    {
                        txtHp.Focus();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menambahkan data karyawan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtIdKaryawan.Text))
            {
                MessageBox.Show("Pilih data karyawan yang ingin diperbarui terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("Nama wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNama.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtHp.Text))
            {
                MessageBox.Show("No HP wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHp.Focus();
                return;
            }
            //  Tambahan Validasi untuk Alamat (mencegah string kosong)
            if (string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                MessageBox.Show("Alamat wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlamat.Focus();
                return;
            }
            //

            // Validasi No HP
            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai. Harus diawali '08' dan panjang antara 10-13 digit.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHp.Focus();
                return;
            }
            // --- Akhir Validasi Input Client-Side ---

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("UPDATE_Karyawan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_karyawan", txtIdKaryawan.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim()); 
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data karyawan berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Gagal memperbarui data karyawan di database.", "Kesalahan Pembaruan Data Karyawan", sqlEx);
                    // Fokuskan control setelah ShowError dipanggil
                    if (sqlEx.Number == 515)
                    {
                        if (sqlEx.Message.Contains("'nama'")) txtNama.Focus();
                        else if (sqlEx.Message.Contains("'no_hp'")) txtHp.Focus();
                        else if (sqlEx.Message.Contains("'alamat'")) txtAlamat.Focus(); 
                    }
                    else if (sqlEx.Number == 8152)
                    {
                        txtNama.Focus();
                    }
                    else if (sqlEx.Number == 547 && sqlEx.Message.Contains("CK__Karyawan__no_hp__"))
                    {
                        txtHp.Focus();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat memperbarui data karyawan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdKaryawan.Text))
            {
                MessageBox.Show("Pilih data karyawan yang ingin dihapus terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Apakah Anda yakin ingin menghapus data karyawan dengan ID: {txtIdKaryawan.Text}?\n\nJika karyawan ini digunakan dalam Detail Layanan, penghapusan mungkin gagal.", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("DELETE_Karyawan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_karyawan", txtIdKaryawan.Text.Trim());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data karyawan berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Gagal menghapus data karyawan dari database.", "Kesalahan Penghapusan Data Karyawan", sqlEx);
                    // Fokuskan control jika terjadi error Foreign Key
                    if (sqlEx.Number == 547 && sqlEx.Message.Contains("FK__Detail_La__id_ka__"))
                    {
                        txtIdKaryawan.Focus();
                    }
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menghapus data karyawan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }

        private void dgvKaryawan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKaryawan.Rows[e.RowIndex];
                txtIdKaryawan.Text = row.Cells["id_karyawan"].Value.ToString();
                txtNama.Text = row.Cells["nama"].Value.ToString();
                txtHp.Text = row.Cells["no_hp"].Value.ToString();
                txtAlamat.Text = row.Cells["alamat"].Value.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.Show();
            this.Close();
        }

        private void btnInputData_Click(object sender, EventArgs e)
        {
            using (var openFile = new OpenFileDialog())
            {
                openFile.Filter = "Excel Workbooks|*.xlsx;*.xls|All Files|*.*";
                if (openFile.ShowDialog() == DialogResult.OK)
                    PreviewData(openFile.FileName);
            }
        }

        private void PreviewData(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;

                    if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    else if (filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Untuk file .xls, Anda juga perlu menginstal NPOI.HSSF.UserModel via NuGet dan menggunakan HSSFWorkbook.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Format file tidak didukung. Harap pilih file .xls atau .xlsx.", "Format Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    IFormulaEvaluator evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                    ISheet sheet = workbook.GetSheetAt(0);
                    DataTable dt = new DataTable();

                    // Header kolom
                    IRow headerRow = sheet.GetRow(0);
                    if (headerRow != null)
                    {
                        foreach (var cell in headerRow.Cells)
                        {
                            string colName = cell.ToString();
                            if (dt.Columns.Contains(colName))
                            {
                                int i = 1;
                                while (dt.Columns.Contains(colName + "_" + i))
                                {
                                    i++;
                                }
                                colName = colName + "_" + i;
                            }
                            dt.Columns.Add(colName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("File Excel kosong atau tidak memiliki header.", "Data Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    for (int i = (headerRow == null ? 0 : 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow dataRow = sheet.GetRow(i);
                        if (dataRow == null) continue;
                        DataRow newRow = dt.NewRow();

                        for (int cellIndex = 0; cellIndex < dt.Columns.Count; cellIndex++)
                        {
                            ICell cell = dataRow.GetCell(cellIndex);
                            if (cell != null)
                            {

                                switch (cell.CellType)
                                {
                                    case CellType.String:
                                        newRow[cellIndex] = cell.StringCellValue;
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            newRow[cellIndex] = cell.DateCellValue;
                                        }
                                        else
                                        {
                                            newRow[cellIndex] = cell.NumericCellValue;
                                        }
                                        break;
                                    case CellType.Boolean:
                                        newRow[cellIndex] = cell.BooleanCellValue;
                                        break;
                                    case CellType.Formula:
                                        try
                                        {
                                            CellValue cellValue = evaluator.Evaluate(cell);
                                            switch (cellValue.CellType)
                                            {
                                                case CellType.String:
                                                    newRow[cellIndex] = cellValue.StringValue;
                                                    break;
                                                case CellType.Numeric:
                                                    newRow[cellIndex] = cellValue.NumberValue;
                                                    break;
                                                case CellType.Boolean:
                                                    newRow[cellIndex] = cellValue.BooleanValue;
                                                    break;
                                                case CellType.Error:
                                                    newRow[cellIndex] = cellValue.ErrorValue.ToString();
                                                    break;
                                                default:
                                                    newRow[cellIndex] = cell.ToString();
                                                    break;
                                            }
                                        }
                                        catch (Exception exFormula)
                                        {

                                            newRow[cellIndex] = $"ERROR_FORMULA: {exFormula.Message}";
                                        }
                                        break;
                                    case CellType.Blank:
                                        newRow[cellIndex] = DBNull.Value;
                                        break;
                                    default:
                                        newRow[cellIndex] = cell.ToString();
                                        break;
                                }
                            }
                            else
                            {
                                newRow[cellIndex] = DBNull.Value;
                            }
                        }
                        dt.Rows.Add(newRow);
                    }

                    this.Hide();
                    PreviewData previewForm = new PreviewData(dt, this);
                    previewForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error reading the Excel file: " + ex.Message, "Error Membaca Excel", ex);
            }
        }
    }
}
