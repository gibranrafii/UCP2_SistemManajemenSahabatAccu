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

namespace Sahabat_Accu_1
{
    public partial class Detail : Form
    {
        private string connectionString = "Data Source=LAPTOP-QT79LBKA\\GIBRANRAFI;Initial Catalog=SistemManajemenSahabatAccu;Integrated Security=True;Pooling=False";
        private Form1 parentForm;

        public Detail(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            // Pindahkan LoadData dan LoadComboBoxData ke Detail_Load event
        }

        // Mengubah ShowError agar bisa menerima string title juga, dan detail Exception
        private void ShowError(string message, string title = "Terjadi Kesalahan", Exception ex = null)
        {
            string fullMessage = message;

            // Handle specific SQL errors more gracefully
            if (ex is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627: // Primary Key violation (id_detail must be unique)
                        fullMessage = "ID Detail Layanan sudah ada. Masukkan ID lain atau gunakan fitur update.";
                        title = "Peringatan Input Duplikat";
                        break;
                    case 547: // Constraint violation (Foreign Key atau CHECK constraint)
                        if (sqlEx.Message.Contains("FK__Detail_La__id_pe__"))
                        {
                            fullMessage = "ID Pelayanan tidak ditemukan. Pastikan ID Pelayanan yang Anda pilih valid.";
                        }
                        else if (sqlEx.Message.Contains("FK__Detail_La__id_ba__"))
                        {
                            fullMessage = "ID Barang tidak ditemukan. Pastikan ID Barang yang Anda pilih valid.";
                        }
                        else if (sqlEx.Message.Contains("FK__Detail_La__id_ka__"))
                        {
                            fullMessage = "ID Karyawan tidak ditemukan. Pastikan ID Karyawan yang Anda pilih valid.";
                        }
                        else if (sqlEx.Message.Contains("CHECK constraint 'CK__Detail_La__id_de__"))
                        {
                            fullMessage = "Format ID Detail Layanan tidak sesuai. Harus 'DT' diikuti 3 angka (contoh: DT001).";
                        }
                        else if (sqlEx.Message.Contains("CHECK constraint 'CK__Detail_La__jumlah__"))
                        {
                            fullMessage = "Jumlah tidak boleh kurang dari 0.";
                        }
                        else if (sqlEx.Message.Contains("CHECK constraint 'CK__Detail_La__subto__"))
                        {
                            fullMessage = "Subtotal tidak boleh kurang dari 0.";
                        }
                        else
                        {
                            fullMessage = $"Terjadi pelanggaran constraint: {sqlEx.Message}";
                        }
                        title = "Kesalahan Validasi Data";
                        break;
                    case 50000: // Custom RAISERROR messages from stored procedure (Stok tidak mencukupi, Barang tidak ditemukan)
                        fullMessage = sqlEx.Message; // Ambil langsung pesan dari RAISERROR
                        title = "Peringatan Bisnis Logika";
                        break;
                    case 515: // Cannot insert NULL into NOT NULL column
                        fullMessage = "Ada kolom yang tidak boleh kosong. Mohon periksa kembali input Anda.";
                        title = "Kesalahan Input Data";
                        break;
                    case 8152: // String or binary data would be truncated
                        fullMessage = "Ada input yang terlalu panjang untuk kolom database. Mohon periksa kembali panjang data.";
                        title = "Kesalahan Panjang Data";
                        break;
                    default:
                        // For other SQL errors, append the SQL error message for debugging
                        fullMessage += $"\n\nDetail Error Database:\nSQL Error Code: {sqlEx.Number}";
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            fullMessage += $"\n  Line: {error.LineNumber}, Msg: {error.Message}";
                        }
                        title = "Kesalahan Database Umum";
                        break;
                }
            }
            else if (ex != null)
            {
                // For non-SQL exceptions, append the generic exception message
                fullMessage += $"\n\nDetail Error Umum:\n{ex.Message}";
                title = "Kesalahan Aplikasi Umum";
            }

            MessageBox.Show(fullMessage, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private string GetSqlErrorMessage(SqlException sqlEx)
        {
            // This method is now less critical as ShowError handles most common cases
            // but can still be used if you need raw SQL error details for logging/advanced debugging.
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

        private void Detail_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            LoadData();
            ClearForm();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT id_detail, id_pelayanan, id_barang, id_karyawan, jumlah, subtotal FROM Detail_Layanan", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memuat data detail layanan dari database.", "Kesalahan Memuat Data", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data detail layanan.", "Kesalahan Umum", ex);
            }
        }

        private void ClearForm()
        {
            txtIdDetail.Clear();
            txtJumlah.Clear();
            txtSubtotal.Clear();
            comboBoxIdPelayanan.SelectedIndex = 0;
            comboBoxIdBarang.SelectedIndex = 0;
            comboBoxIdKaryawan.SelectedIndex = 0;
            txtIdDetail.Focus();
        }

        private void LoadComboBoxData()
        {
            try
            {
                LoadComboBox(comboBoxIdPelayanan, "SELECT id_pelayanan FROM Pelayanan", true);
                LoadComboBox(comboBoxIdBarang, "SELECT id_barang FROM Barang", true);
                LoadComboBox(comboBoxIdKaryawan, "SELECT id_karyawan FROM Karyawan", true);
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memuat data untuk ComboBoxes.", "Kesalahan Memuat ComboBox", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data untuk ComboBoxes.", "Kesalahan Umum", ex);
            }
        }

        private void LoadComboBox(ComboBox comboBox, string query, bool allowEmpty = false)
        {
            comboBox.Items.Clear();
            if (allowEmpty)
            {
                comboBox.Items.Add("");
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox.Items.Add(reader[0].ToString());
                }
                reader.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtIdDetail.Text = row.Cells["id_detail"].Value.ToString();
                comboBoxIdPelayanan.Text = row.Cells["id_pelayanan"].Value != DBNull.Value ? row.Cells["id_pelayanan"].Value.ToString() : "";
                comboBoxIdBarang.Text = row.Cells["id_barang"].Value != DBNull.Value ? row.Cells["id_barang"].Value.ToString() : "";
                comboBoxIdKaryawan.Text = row.Cells["id_karyawan"].Value != DBNull.Value ? row.Cells["id_karyawan"].Value.ToString() : "";
                txtJumlah.Text = row.Cells["jumlah"].Value.ToString();
                txtSubtotal.Text = row.Cells["subtotal"].Value.ToString();
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdDetail.Text) ||
                string.IsNullOrWhiteSpace(comboBoxIdPelayanan.Text) ||
                string.IsNullOrWhiteSpace(comboBoxIdKaryawan.Text))
            {
                MessageBox.Show("ID Detail, ID Pelayanan, dan ID Karyawan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtIdDetail.Text.Trim(), "^DT[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Detail Layanan tidak sesuai. Harus 'DT' diikuti 3 angka (contoh: DT001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtJumlah.Text, out int jumlah) || jumlah < 0)
            {
                MessageBox.Show("Jumlah tidak valid. Masukkan bilangan bulat positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtSubtotal.Text, out decimal subtotal) || subtotal < 0)
            {
                MessageBox.Show("Subtotal tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jenisPelayanan = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string kategoriQuery = "SELECT jenis_pelayanan FROM Pelayanan WHERE id_pelayanan = @id_pelayanan";
                    SqlCommand kategoriCmd = new SqlCommand(kategoriQuery, conn);
                    kategoriCmd.Parameters.AddWithValue("@id_pelayanan", comboBoxIdPelayanan.Text.Trim());
                    jenisPelayanan = kategoriCmd.ExecuteScalar()?.ToString();
                }

                if (jenisPelayanan == null)
                {
                    MessageBox.Show("ID Pelayanan tidak ditemukan di database.", "Validasi Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memverifikasi ID Pelayanan.", "Kesalahan Database", sqlEx);
                return;
            }
            catch (Exception ex)
            {
                ShowError("Terjadi kesalahan saat memverifikasi ID Pelayanan.", "Kesalahan Umum", ex);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("INSERT_DetailLayanan", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@id_detail", txtIdDetail.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelayanan", comboBoxIdPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_karyawan", comboBoxIdKaryawan.Text.Trim());
                    cmd.Parameters.AddWithValue("@subtotal", subtotal);

                    if (jenisPelayanan.ToLower() == "charge aki")
                    {
                        cmd.Parameters.AddWithValue("@id_barang", DBNull.Value);
                        cmd.Parameters.AddWithValue("@jumlah", 0);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(comboBoxIdBarang.Text))
                        {
                            MessageBox.Show("Pilih ID Barang untuk jenis layanan ini (Tuker Tambah/Pembelian).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction.Rollback();
                            return;
                        }
                        cmd.Parameters.AddWithValue("@id_barang", comboBoxIdBarang.Text.Trim());
                        cmd.Parameters.AddWithValue("@jumlah", jumlah);
                    }

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("Data detail layanan berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    // Langsung panggil ShowError, biarkan ShowError yang menentukan pesan yang tepat
                    ShowError("Gagal menambahkan data detail layanan ke database.", "Kesalahan Penambahan Data Detail Layanan", sqlEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menambahkan detail layanan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdDetail.Text))
            {
                MessageBox.Show("Pilih data detail layanan yang ingin diupdate!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(comboBoxIdPelayanan.Text) ||
                string.IsNullOrWhiteSpace(comboBoxIdKaryawan.Text))
            {
                MessageBox.Show("ID Pelayanan dan ID Karyawan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtJumlah.Text, out int jumlah) || jumlah < 0)
            {
                MessageBox.Show("Jumlah tidak valid. Masukkan bilangan bulat positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtSubtotal.Text, out decimal subtotal) || subtotal < 0)
            {
                MessageBox.Show("Subtotal tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jenisPelayanan = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string kategoriQuery = "SELECT jenis_pelayanan FROM Pelayanan WHERE id_pelayanan = @id_pelayanan";
                    SqlCommand kategoriCmd = new SqlCommand(kategoriQuery, conn);
                    kategoriCmd.Parameters.AddWithValue("@id_pelayanan", comboBoxIdPelayanan.Text.Trim());
                    jenisPelayanan = kategoriCmd.ExecuteScalar()?.ToString();
                }

                if (jenisPelayanan == null)
                {
                    MessageBox.Show("ID Pelayanan tidak ditemukan di database.", "Validasi Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memverifikasi ID Pelayanan.", "Kesalahan Database", sqlEx);
                return;
            }
            catch (Exception ex)
            {
                ShowError("Terjadi kesalahan saat memverifikasi ID Pelayanan.", "Kesalahan Umum", ex);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("UPDATE_DetailLayanan", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@id_detail", txtIdDetail.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelayanan", comboBoxIdPelayanan.Text.Trim());

                    if (jenisPelayanan.ToLower() == "charge aki")
                    {
                        cmd.Parameters.AddWithValue("@id_barang", DBNull.Value);
                        cmd.Parameters.AddWithValue("@jumlah", 0);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(comboBoxIdBarang.Text))
                        {
                            MessageBox.Show("Pilih ID Barang untuk jenis layanan ini (Tuker Tambah/Pembelian).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction.Rollback();
                            return;
                        }
                        cmd.Parameters.AddWithValue("@id_barang", comboBoxIdBarang.Text.Trim());
                        cmd.Parameters.AddWithValue("@jumlah", jumlah);
                    }
                    cmd.Parameters.AddWithValue("@id_karyawan", comboBoxIdKaryawan.Text.Trim());
                    cmd.Parameters.AddWithValue("@subtotal", subtotal);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("Data detail layanan berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Gagal mengupdate data detail layanan di database.", "Kesalahan Pembaruan Data Detail Layanan", sqlEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat memperbarui detail layanan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdDetail.Text))
            {
                MessageBox.Show("Pilih data detail layanan yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Apakah Anda yakin ingin menghapus data detail layanan dengan ID: {txtIdDetail.Text}?", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("DELETE_DetailLayanan", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@id_detail", txtIdDetail.Text.Trim());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data detail layanan berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Gagal menghapus data detail layanan dari database.", "Kesalahan Penghapusan Data Detail Layanan", sqlEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menghapus detail layanan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadComboBoxData();
            LoadData();
            ClearForm();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.Show();
            this.Close();
        }
    }
}