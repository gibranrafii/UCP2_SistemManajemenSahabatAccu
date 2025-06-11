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
using System.Text.RegularExpressions; // Tambahkan ini untuk Regex

namespace Sahabat_Accu_1
{
    public partial class Barang : Form
    {
        private string connectionString = "Data Source=LAPTOP-QT79LBKA\\GIBRANRAFI;Initial Catalog=SistemManajemenSahabatAccu;Integrated Security=True;Pooling=False";
        private Form1 parentForm;

        public Barang(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        // Mengubah ShowError agar bisa menerima string title juga, dan detail Exception
        private void ShowError(string message, string title = "Terjadi Kesalahan", Exception ex = null)
        {
            string fullMessage = message;
            if (ex != null)
            {
                fullMessage += $"\n\nDetail Error:\n{ex.Message}";
                if (ex is SqlException sqlEx)
                {
                    fullMessage += $"\nSQL Error Code: {sqlEx.Number}";
                    foreach (SqlError error in sqlEx.Errors)
                    {
                        fullMessage += $"\n  Line: {error.LineNumber}, Msg: {error.Message}";
                    }
                }
            }
            MessageBox.Show(fullMessage, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Helper method to extract more detailed SQL error information.
        /// </summary>
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

        private void Barang_Load(object sender, EventArgs e)
        {
            InitComboJenis(); // Inisialisasi dulu combo box
            LoadData();      // Baru load data
            ClearForm();     // Kosongkan form setelah load data awal
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id_barang, nama_barang, jenis, harga, stok FROM Barang";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvBarang.DataSource = dt;
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memuat data barang dari database: {sqlEx.Message}", "Kesalahan Database", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data barang:", "Kesalahan Umum", ex);
            }
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtNama.Clear();
            if (comboJenis.Items.Count > 0)
            {
                comboJenis.SelectedIndex = 0; // Set ke item pertama jika ada
            }
            txtHarga.Clear();
            txtStok.Clear();
            txtId.Focus(); // Fokus ke ID barang
        }

        private void InitComboJenis()
        {
            comboJenis.Items.Clear();
            comboJenis.Items.Add("Aki Basah");
            comboJenis.Items.Add("Aki Kering");
            if (comboJenis.Items.Count > 0)
            {
                comboJenis.SelectedIndex = 0; // Set default selection
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtNama.Text) ||
                string.IsNullOrWhiteSpace(comboJenis.Text) ||
                string.IsNullOrWhiteSpace(txtHarga.Text) ||
                string.IsNullOrWhiteSpace(txtStok.Text))
            {
                MessageBox.Show("Semua kolom (ID, Nama, Jenis, Harga, Stok) wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi format ID Barang
            if (!Regex.IsMatch(txtId.Text.Trim(), "^AK[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Barang tidak sesuai. Harus 'AK' diikuti 3 angka (contoh: AK001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi Harga
            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi Stok
            if (!int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Stok tidak valid. Masukkan bilangan bulat positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    SqlCommand cmd = new SqlCommand("INSERT_Barang", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_barang", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama_barang", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", comboJenis.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga); // Gunakan harga yang sudah divalidasi
                    cmd.Parameters.AddWithValue("@stok", stok);   // Gunakan stok yang sudah divalidasi
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Barang berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    string errorMessage = $"Gagal menambahkan barang ke database: {sqlEx.Message}";
                    switch (sqlEx.Number)
                    {
                        case 2627: // Primary Key violation (id_barang must be unique)
                            errorMessage = "ID Barang sudah ada. Masukkan ID lain atau gunakan fitur update.";
                            break;
                        case 547: // Constraint violation (CHECK constraint)
                            // Identifikasi CHECK constraint berdasarkan pesan error
                            if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__id_bara__")) // id_barang LIKE 'AK[0-9][0-9][0-9]'
                            {
                                errorMessage = "Format ID Barang tidak sesuai. Harus 'AK' diikuti 3 angka (contoh: AK001).";
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__jenis__")) // jenis IN ('Aki Basah', 'Aki Kering')
                            {
                                errorMessage = "Jenis barang tidak valid. Pilih 'Aki Basah' atau 'Aki Kering'.";
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__harga__")) // harga >= 0
                            {
                                errorMessage = "Harga tidak boleh kurang dari 0.";
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__stok__")) // stok >= 0
                            {
                                errorMessage = "Stok tidak boleh kurang dari 0.";
                            }
                            else
                            {
                                errorMessage = $"Terjadi pelanggaran constraint: {sqlEx.Message}";
                            }
                            break;
                        case 8152: // String or binary data would be truncated (e.g., nama_barang terlalu panjang)
                            errorMessage = "Ada input yang terlalu panjang untuk kolom database (misal: nama barang). Mohon periksa kembali panjang data.";
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Penambahan Data Barang");
                }
                catch (FormatException fEx) // Menangkap kesalahan parsing jika tidak ditangani di TryParse
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Format input angka tidak valid. Pastikan Harga dan Stok diisi dengan benar.", "Input Data Tidak Valid", fEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menambahkan barang. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Pilih data barang yang akan diupdate terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNama.Text) ||
                string.IsNullOrWhiteSpace(comboJenis.Text) ||
                string.IsNullOrWhiteSpace(txtHarga.Text) ||
                string.IsNullOrWhiteSpace(txtStok.Text))
            {
                MessageBox.Show("Nama, Jenis, Harga, dan Stok wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ID Barang tidak divalidasi formatnya karena tidak boleh diubah (PRIMARY KEY)
            // Validasi Harga
            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi Stok
            if (!int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Stok tidak valid. Masukkan bilangan bulat positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    SqlCommand cmd = new SqlCommand("UPDATE_Barang", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_barang", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama_barang", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", comboJenis.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga); // Gunakan harga yang sudah divalidasi
                    cmd.Parameters.AddWithValue("@stok", stok);   // Gunakan stok yang sudah divalidasi
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Barang berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    string errorMessage = $"Gagal mengupdate barang di database: {sqlEx.Message}";
                    switch (sqlEx.Number)
                    {
                        case 547: // Constraint violation (CHECK constraint)
                            if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__jenis__")) // jenis IN ('Aki Basah', 'Aki Kering')
                            {
                                errorMessage = "Jenis barang tidak valid. Pilih 'Aki Basah' atau 'Aki Kering'.";
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__harga__")) // harga >= 0
                            {
                                errorMessage = "Harga tidak boleh kurang dari 0.";
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Barang__stok__")) // stok >= 0
                            {
                                errorMessage = "Stok tidak boleh kurang dari 0.";
                            }
                            else
                            {
                                errorMessage = $"Terjadi pelanggaran constraint: {sqlEx.Message}";
                            }
                            break;
                        case 8152: // String or binary data would be truncated
                            errorMessage = "Ada input yang terlalu panjang untuk kolom database (misal: nama barang). Mohon periksa kembali panjang data.";
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Pembaruan Data Barang");
                }
                catch (FormatException fEx) // Menangkap kesalahan parsing jika tidak ditangani di TryParse
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Format input angka tidak valid. Pastikan Harga dan Stok diisi dengan benar.", "Input Data Tidak Valid", fEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat memperbarui barang. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Pilih data barang yang ingin dihapus terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Yakin ingin menghapus data barang dengan ID: {txtId.Text}?\n\nJika barang ini digunakan dalam Detail Layanan, penghapusan mungkin gagal.", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("DELETE_Barang", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_barang", txtId.Text.Trim());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Barang berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    string errorMessage = $"Gagal menghapus barang dari database: {sqlEx.Message}";
                    switch (sqlEx.Number)
                    {
                        case 547: // Foreign Key Constraint violation
                            errorMessage = "Barang ini tidak dapat dihapus karena masih digunakan dalam Detail Layanan. Hapus semua Detail Layanan yang terkait dengan barang ini terlebih dahulu.";
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Penghapusan Data Barang");
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menghapus barang. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }

        private void dgvBarang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBarang.Rows[e.RowIndex];
                txtId.Text = row.Cells["id_barang"].Value.ToString();
                txtNama.Text = row.Cells["nama_barang"].Value.ToString();
                comboJenis.Text = row.Cells["jenis"].Value.ToString();
                txtHarga.Text = row.Cells["harga"].Value.ToString();
                txtStok.Text = row.Cells["stok"].Value.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.Show();
            this.Close();
        }

        // Event handler yang tidak digunakan, dapat dihapus jika tidak ada logika tambahan
        private void comboJenis_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
    }
}