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
        Koneksi kn = new Koneksi();

        private Form1 parentForm;

        public Detail(Form1 parentForm)
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
                if (sqlEx.State >= 16) // Menangkap pesan dari RAISERROR
                {
                    fullMessage = sqlEx.Message;
                    finalTitle = "Kesalahan Proses";
                }
                else if (sqlEx.Number == 2627)
                {
                    fullMessage = "ID Detail Layanan sudah ada. Coba lagi dengan ID yang berbeda.";
                    finalTitle = "Peringatan Input Duplikat";
                }
                else if (sqlEx.Number == 547)
                {
                    fullMessage = "ID Pelayanan, Barang, atau Karyawan yang dipilih tidak valid.";
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
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT id_detail, id_pelayanan, id_barang, id_karyawan, jumlah, subtotal FROM Detail_Layanan ORDER BY id_detail DESC", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data detail layanan.", "Kesalahan Memuat Data", ex);
            }
        }

        private void ClearForm()
        {
            txtIdDetail.Clear();
            txtJumlah.Clear();
            txtSubtotal.Clear();
            comboBoxIdPelayanan.SelectedIndex = -1;
            comboBoxIdBarang.SelectedIndex = -1;
            comboBoxIdKaryawan.SelectedIndex = -1;
            comboBoxIdPelayanan.Text = " ";
            comboBoxIdBarang.Text = " ";
            comboBoxIdKaryawan.Text = " ";
            txtIdDetail.Focus();
            txtIdDetail.Enabled = true; // Aktifkan kembali untuk entri baru
        }

        private void LoadComboBoxData()
        {
            try
            {
                LoadComboBox(comboBoxIdPelayanan, "SELECT id_pelayanan FROM Pelayanan", "id_pelayanan");
                LoadComboBox(comboBoxIdBarang, "SELECT id_barang FROM Barang", "id_barang");
                LoadComboBox(comboBoxIdKaryawan, "SELECT id_karyawan FROM Karyawan", "id_karyawan");
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data untuk ComboBox.", "Kesalahan Pemuatan", ex);
            }
        }

        private void LoadComboBox(ComboBox comboBox, string query, string displayMember)
        {
            using (SqlConnection conn = new SqlConnection(kn.connectionString()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboBox.DataSource = dt;
                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = displayMember;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtIdDetail.Text = row.Cells["id_detail"].Value.ToString();
                comboBoxIdPelayanan.SelectedValue = row.Cells["id_pelayanan"].Value;
                comboBoxIdBarang.SelectedValue = row.Cells["id_barang"].Value;
                comboBoxIdKaryawan.SelectedValue = row.Cells["id_karyawan"].Value;
                txtJumlah.Text = row.Cells["jumlah"].Value.ToString();
                txtSubtotal.Text = row.Cells["subtotal"].Value.ToString();
                txtIdDetail.Enabled = false; // Nonaktifkan pengeditan ID setelah dipilih
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdDetail.Text) || !txtIdDetail.Enabled)
            {
                MessageBox.Show("Semua kolom wajib diisi. Perhatikan Id wajib diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBoxIdPelayanan.SelectedValue == null || comboBoxIdBarang.SelectedValue == null || comboBoxIdKaryawan.SelectedValue == null)
            {
                MessageBox.Show("ID Pelayanan, ID Barang, dan ID Karyawan wajib dipilih.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(txtIdDetail.Text.Trim(), "^DT[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Detail Layanan tidak sesuai. Harus 'DT' diikuti 3 angka (contoh: DT001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtJumlah.Text, out int jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah harus berupa angka dan lebih dari 0.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtSubtotal.Text, out decimal subtotal) || subtotal < 0)
            {
                MessageBox.Show("Subtotal tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    // Kode menjadi lebih sederhana, tidak perlu mengelola SqlTransaction di C#
                    SqlCommand cmd = new SqlCommand("INSERT_DetailLayanan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_detail", txtIdDetail.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelayanan", comboBoxIdPelayanan.SelectedValue);
                    cmd.Parameters.AddWithValue("@id_barang", comboBoxIdBarang.SelectedValue);
                    cmd.Parameters.AddWithValue("@id_karyawan", comboBoxIdKaryawan.SelectedValue);
                    cmd.Parameters.AddWithValue("@jumlah", jumlah);
                    cmd.Parameters.AddWithValue("@subtotal", subtotal);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Detail layanan berhasil ditambahkan dan stok telah diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                LoadComboBoxData(); // Muat ulang untuk refresh stok
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menambahkan detail layanan.", "Kesalahan Proses", ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdDetail.Text) || txtIdDetail.Enabled)
            {
                MessageBox.Show("Pilih data dari tabel yang ingin diupdate.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtJumlah.Text, out int jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah harus berupa angka dan lebih dari 0.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE_DetailLayanan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_detail", txtIdDetail.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelayanan", comboBoxIdPelayanan.SelectedValue);
                    cmd.Parameters.AddWithValue("@id_barang", comboBoxIdBarang.SelectedValue);
                    cmd.Parameters.AddWithValue("@id_karyawan", comboBoxIdKaryawan.SelectedValue);
                    cmd.Parameters.AddWithValue("@jumlah", jumlah);
                    cmd.Parameters.AddWithValue("@subtotal", decimal.Parse(txtSubtotal.Text));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Detail layanan berhasil diupdate dan stok telah disesuaikan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                LoadComboBoxData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal mengupdate detail layanan.", "Kesalahan Proses", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdDetail.Text) || txtIdDetail.Enabled)
            {
                MessageBox.Show("Pilih data dari tabel yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Apakah Anda yakin ingin menghapus data ini? Stok akan dikembalikan.", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE_DetailLayanan", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_detail", txtIdDetail.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Detail layanan berhasil dihapus dan stok telah dikembalikan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LoadComboBoxData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    ShowError("Gagal menghapus detail layanan.", "Kesalahan Proses", ex);
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