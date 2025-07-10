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
        Koneksi kn = new Koneksi();

        private Form1 parentForm;

        public Karyawan(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void ShowError(string message, string title = "Terjadi Kesalahan", Exception ex = null)
        {
            string fullMessage = message;
            string finalTitle = title;

            if (ex is SqlException sqlEx)
            {
                if (sqlEx.State >= 16)
                {
                    fullMessage = sqlEx.Message;
                    finalTitle = "Kesalahan Proses";
                }
                else if (sqlEx.Number == 2627)
                {
                    fullMessage = "ID Karyawan yang Anda masukkan sudah ada.";
                    finalTitle = "Peringatan Input Duplikat";
                }
                else if (sqlEx.Number == 547)
                {
                    fullMessage = "Karyawan ini tidak dapat dihapus karena masih digunakan dalam Detail Layanan.";
                    finalTitle = "Kesalahan Referensi Data";
                }
                else
                {
                    fullMessage += $"\n\nDetail Error Database (Kode: {sqlEx.Number}):\n{sqlEx.Message}";
                    finalTitle = "Kesalahan Database Umum";
                }
            }
            else if (ex != null)
            {
                fullMessage += $"\n\nDetail Error Umum:\n{ex.Message}";
            }

            MessageBox.Show(fullMessage, finalTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Karyawan_Load(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT id_karyawan, nama, no_hp, alamat FROM Karyawan", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvKaryawan.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data karyawan.", "Kesalahan Memuat Data", ex);
            }
        }

        private void ClearForm()
        {
            txtIdKaryawan.Clear();
            txtNama.Clear();
            txtHp.Clear();
            txtAlamat.Clear();
            txtIdKaryawan.Focus();
            txtIdKaryawan.Enabled = true;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtIdKaryawan.Text) || !txtIdKaryawan.Enabled)
            {
                MessageBox.Show("Semua kolom wajib diisi. Perhatikan Id wajib diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtHp.Text) || string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                MessageBox.Show("Semua kolom wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(txtIdKaryawan.Text.Trim(), "^KR[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Karyawan tidak sesuai. Harus 'KR' diikuti 3 angka (contoh: KR001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai. Harus diawali '08' dan panjang antara 10-13 digit.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    // Tidak perlu mengelola SqlTransaction di C#
                    SqlCommand cmd = new SqlCommand("INSERT_Karyawan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_karyawan", txtIdKaryawan.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data karyawan berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menambahkan data karyawan.", "Kesalahan Proses", ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdKaryawan.Text) || txtIdKaryawan.Enabled)
            {
                MessageBox.Show("Pilih data karyawan yang ingin diperbarui dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Validasi lainnya...
            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE_Karyawan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_karyawan", txtIdKaryawan.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data karyawan berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal memperbarui data karyawan.", "Kesalahan Proses", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdKaryawan.Text) || txtIdKaryawan.Enabled)
            {
                MessageBox.Show("Pilih data karyawan yang ingin dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Apakah Anda yakin ingin menghapus karyawan dengan ID: {txtIdKaryawan.Text}?", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE_Karyawan", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_karyawan", txtIdKaryawan.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Data karyawan berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    ShowError("Gagal menghapus data karyawan.", "Kesalahan Proses", ex);
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
                txtIdKaryawan.Enabled = false;
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
                openFile.Filter = "Excel Workbooks (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    PreviewData(openFile.FileName);
                }
            }
        }

        private void PreviewData(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);
                    DataTable dt = new DataTable();

                    IRow headerRow = sheet.GetRow(0);
                    if (headerRow == null)
                    {
                        MessageBox.Show("File Excel kosong atau tidak memiliki header.", "Data Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (var cell in headerRow.Cells)
                    {
                        dt.Columns.Add(cell.ToString());
                    }

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow dataRow = sheet.GetRow(i);
                        if (dataRow == null) continue;
                        DataRow newRow = dt.NewRow();
                        for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                        {
                            ICell cell = dataRow.GetCell(cellIndex);
                            newRow[cellIndex] = cell?.ToString() ?? string.Empty;
                        }
                        dt.Rows.Add(newRow);
                    }

                    this.Hide();
                    // Asumsi Anda memiliki Form bernama PreviewData
                    PreviewData previewForm = new PreviewData(dt, this);
                    previewForm.ShowDialog();
                    MessageBox.Show("Fungsi preview belum terhubung. Data dari Excel:\n" + dt.Rows.Count + " baris dibaca.", "Info");
                    this.Show(); // Tampilkan kembali form ini setelah preview ditutup
                }
            }
            catch (Exception ex)
            {
                ShowError("Error membaca file Excel. Pastikan file tidak sedang dibuka dan formatnya benar.", "Error Membaca Excel", ex);
            }
        }
    }
}