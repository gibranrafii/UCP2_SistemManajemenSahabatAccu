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
        Koneksi kn = new Koneksi();

        private Form1 parentForm;

        public Barang(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        // Metode ShowError sudah sangat baik dalam menerjemahkan pesan error SQL.
        // Tidak perlu diubah.
        private void ShowError(string message, string title = "Terjadi Kesalahan", Exception ex = null)
        {
            string fullMessage = message;
            string finalTitle = title;

            if (ex is SqlException sqlEx)
            {
                string sqlErrorMessage = sqlEx.Message.ToLower();

                // Pesan dari RAISERROR di Stored Procedure akan langsung diambil di sini
                if (sqlEx.State >= 16) 
                {
                    fullMessage = sqlEx.Message; 
                    finalTitle = "Kesalahan Logika Bisnis / Server";
                }
                else if (sqlErrorMessage.Contains("duplicate key") && sqlErrorMessage.Contains("primary key"))
                {
                    fullMessage = "ID Barang sudah ada. Masukkan ID lain atau gunakan fitur update.";
                    finalTitle = "Peringatan Input Duplikat";
                }
                else if (sqlErrorMessage.Contains("check constraint"))
                {
                    if (sqlErrorMessage.Contains("ck__barang__id_bara__"))
                    {
                        fullMessage = "Format ID Barang tidak sesuai. Harus 'AK' diikuti 3 angka (contoh: AK001).";
                    }
                    else if (sqlErrorMessage.Contains("ck__barang__jenis__"))
                    {
                        fullMessage = "Jenis barang tidak valid. Pilih 'Aki Basah' atau 'Aki Kering'.";
                    }
                    else if (sqlErrorMessage.Contains("ck__barang__harga__"))
                    {
                        fullMessage = "Harga tidak boleh kurang dari 0.";
                    }
                    else if (sqlErrorMessage.Contains("ck__barang__stok__"))
                    {
                        fullMessage = "Stok tidak boleh kurang dari 0.";
                    }
                    else
                    {
                        fullMessage = $"Terjadi pelanggaran aturan data (constraint). Detail: {sqlEx.Message}";
                    }
                    finalTitle = "Kesalahan Validasi Data Database";
                }
                else if (sqlErrorMessage.Contains("foreign key"))
                {
                    fullMessage = "Barang ini tidak dapat dihapus karena sudah digunakan dalam transaksi lain.";
                    finalTitle = "Kesalahan Referensi Data";
                }
                else if (sqlErrorMessage.Contains("cannot insert the value null into column"))
                {
                    if (sqlErrorMessage.Contains("'nama_barang'")) fullMessage = "Nama barang tidak boleh kosong.";
                    else if (sqlErrorMessage.Contains("'jenis'")) fullMessage = "Jenis barang tidak boleh kosong.";
                    else if (sqlErrorMessage.Contains("'harga'")) fullMessage = "Harga tidak boleh kosong.";
                    else if (sqlErrorMessage.Contains("'stok'")) fullMessage = "Stok tidak boleh kosong.";
                    else fullMessage = "Ada kolom wajib isi yang tidak boleh kosong.";
                    finalTitle = "Kesalahan Input Data Wajib";
                }
                else
                {
                    fullMessage += $"\n\nDetail Error Database (Kode: {sqlEx.Number}):\n{sqlEx.Message}";
                    finalTitle = "Kesalahan Database Umum";
                }
            }
            else if (ex is FormatException)
            {
                fullMessage = "Format input angka tidak valid. Pastikan Harga dan Stok diisi dengan angka yang benar.";
                finalTitle = "Input Data Tidak Valid";
            }
            else if (ex != null)
            {
                fullMessage += $"\n\nDetail Error Umum:\n{ex.Message}";
                finalTitle = "Kesalahan Aplikasi Umum";
            }

            MessageBox.Show(fullMessage, finalTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Barang_Load(object sender, EventArgs e)
        {
            InitComboJenis();
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
                    string query = "SELECT id_barang, nama_barang, jenis, harga, stok FROM Barang ORDER BY id_barang";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvBarang.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data barang.", "Kesalahan Memuat Data", ex);
            }
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtNama.Clear();
            if (comboJenis.Items.Count > 0) comboJenis.SelectedIndex = 0;
            txtHarga.Clear();
            txtStok.Clear();
            txtId.Focus();
        }

        private void InitComboJenis()
        {
            comboJenis.Items.Clear();
            comboJenis.Items.Add("Aki Basah");
            comboJenis.Items.Add("Aki Kering");
            if (comboJenis.Items.Count > 0) comboJenis.SelectedIndex = 0;
        }

        // Validasi input di sisi client tetap penting untuk user experience yang baik
        private bool ValidateInput(out decimal harga, out int stok)
        {
            harga = 0;
            stok = 0;

            if (string.IsNullOrWhiteSpace(txtId.Text) ||
                string.IsNullOrWhiteSpace(txtNama.Text) ||
                string.IsNullOrWhiteSpace(comboJenis.Text) ||
                string.IsNullOrWhiteSpace(txtHarga.Text) ||
                string.IsNullOrWhiteSpace(txtStok.Text))
            {
                MessageBox.Show("Semua kolom wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Regex.IsMatch(txtId.Text.Trim(), "^AK[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Barang tidak sesuai. Harus 'AK' diikuti 3 angka (contoh: AK001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus();
                return false;
            }

            if (!decimal.TryParse(txtHarga.Text, out harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHarga.Focus();
                return false;
            }

            if (!int.TryParse(txtStok.Text, out stok) || stok < 0)
            {
                MessageBox.Show("Stok tidak valid. Masukkan bilangan bulat positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStok.Focus();
                return false;
            }

            return true;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out decimal harga, out int stok)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    // TIDAK PERLU 'SqlTransaction' LAGI
                    SqlCommand cmd = new SqlCommand("INSERT_Barang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_barang", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama_barang", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", comboJenis.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@stok", stok);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Barang berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menambahkan barang.", "Kesalahan Database", ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Pilih data barang yang akan diupdate terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput(out decimal harga, out int stok)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    // TIDAK PERLU 'SqlTransaction' LAGI
                    SqlCommand cmd = new SqlCommand("UPDATE_Barang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_barang", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama_barang", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@jenis", comboJenis.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@stok", stok);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Barang berhasil diupdate.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal mengupdate barang.", "Kesalahan Database", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Pilih data barang yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Yakin ingin menghapus barang dengan ID: {txtId.Text}?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    // TIDAK PERLU 'SqlTransaction' LAGI
                    SqlCommand cmd = new SqlCommand("DELETE_Barang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_barang", txtId.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Barang berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menghapus barang.", "Kesalahan Database", ex);
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