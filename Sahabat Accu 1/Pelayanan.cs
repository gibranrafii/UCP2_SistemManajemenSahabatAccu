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
using System.Globalization;

namespace Sahabat_Accu_1
{
   public partial class Pelayanan : Form
    {
        Koneksi kn = new Koneksi();

        private Form1 parentForm;

        public Pelayanan(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            // Tambahkan event handler untuk penghitungan harga akhir secara real-time
            txtHarga.TextChanged += CalculateHargaAkhir;
            txtPotongan.TextChanged += CalculateHargaAkhir;
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
                    fullMessage = "ID Pelayanan sudah ada. Masukkan ID lain.";
                    finalTitle = "Peringatan Input Duplikat";
                }
                else if (sqlEx.Number == 547)
                {
                    if (sqlEx.Message.Contains("FK__Pelayanan__id_pe"))
                    {
                        fullMessage = "ID Pelanggan tidak ditemukan. Pastikan ID Pelanggan valid.";
                    }
                    else if (sqlEx.Message.Contains("CK__Pelayanan__tangg"))
                    {
                         fullMessage = "Tanggal pelayanan tidak valid. Pastikan tanggal dalam rentang yang diizinkan.";
                    }
                    else
                    {
                        fullMessage = "Data pelayanan ini tidak dapat dihapus karena masih digunakan di Detail Layanan.";
                    }
                    finalTitle = "Kesalahan Validasi Data";
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

        private void Pelayanan_Load(object sender, EventArgs e)
        {
            InitComboBoxJenis();
            LoadPelangganToComboBox();
            LoadData();
            CalculateHargaAkhir(null, EventArgs.Empty);
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    string query = "SELECT id_pelayanan, id_pelanggan, tanggal_pelayanan, jenis_pelayanan, harga, potongan_harga, harga_akhir FROM Pelayanan ORDER BY tanggal_pelayanan DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPelayanan.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data pelayanan.", "Kesalahan Memuat Data", ex);
            }
        }

        private void ClearForm()
        {
            txtIdPelayanan.Text = "";
            comboBoxIdPelanggan.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;
            if (comboPelayanan.Items.Count > 0) comboPelayanan.SelectedIndex = 0;
            txtHarga.Text = "";
            txtPotongan.Text = "";
            
            // Asumsi ada label lblHargaAkhir untuk menampilkan harga_akhir
            if (Controls.Find("lblHargaAkhir", true).FirstOrDefault() is Label lbl)
            {
                lbl.Text = "0";
            }
            txtIdPelayanan.Focus();
            txtIdPelayanan.Enabled = true;
        }

        private void InitComboBoxJenis()
        {
            comboPelayanan.Items.Clear();
            comboPelayanan.Items.Add("Charge Aki");
            comboPelayanan.Items.Add("Tuker Tambah");
            comboPelayanan.Items.Add("Pembelian");
            if (comboPelayanan.Items.Count > 0) comboPelayanan.SelectedIndex = 0;
        }

        private void LoadPelangganToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    string query = "SELECT id_pelanggan FROM Pelanggan ORDER BY id_pelanggan";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBoxIdPelanggan.DataSource = dt;
                    comboBoxIdPelanggan.DisplayMember = "id_pelanggan";
                    comboBoxIdPelanggan.ValueMember = "id_pelanggan";
                }
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat daftar pelanggan.", "Kesalahan Umum", ex);
            }
        }

        private void CalculateHargaAkhir(object sender, EventArgs e)
        {
            decimal.TryParse(txtHarga.Text, out decimal harga);
            decimal.TryParse(txtPotongan.Text, out decimal potongan);
            decimal hargaAkhir = harga - potongan;
            if (hargaAkhir < 0) hargaAkhir = 0;
            
            if (Controls.Find("lblHargaAkhir", true).FirstOrDefault() is Label lbl)
            {
                lbl.Text = hargaAkhir.ToString("C", new CultureInfo("id-ID")); // Format Rupiah
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtIdPelayanan.Text) || !txtIdPelayanan.Enabled)
            {
                MessageBox.Show("Semua kolom wajib diisi. Perhatikan Id wajib diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBoxIdPelanggan.SelectedValue == null || string.IsNullOrWhiteSpace(comboPelayanan.Text) || string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                MessageBox.Show("ID Pelanggan, Jenis Pelayanan, dan Harga wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(txtIdPelayanan.Text.Trim(), "^PY[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Pelayanan tidak sesuai. Harus 'PY' diikuti 3 angka (contoh: PY001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal? potongan = null;
            if (!string.IsNullOrWhiteSpace(txtPotongan.Text))
            {
                if (decimal.TryParse(txtPotongan.Text, out decimal parsedPotongan) && parsedPotongan >= 0)
                {
                    potongan = parsedPotongan;
                }
                else
                {
                    MessageBox.Show("Potongan harga tidak valid.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    // Kode menjadi lebih sederhana, tidak perlu mengelola SqlTransaction di C#
                    SqlCommand cmd = new SqlCommand("INSERT_Pelayanan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_pelayanan", txtIdPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelanggan", comboBoxIdPelanggan.SelectedValue);
                    cmd.Parameters.AddWithValue("@tanggal_pelayanan", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@jenis_pelayanan", comboPelayanan.Text);
                    cmd.Parameters.AddWithValue("@total_harga", harga); // Menggunakan nama parameter dari SP baru
                    cmd.Parameters.AddWithValue("@potongan_harga", (object)potongan ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data pelayanan berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menambah data pelayanan.", "Kesalahan Proses", ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdPelayanan.Text) || txtIdPelayanan.Enabled)
            {
                MessageBox.Show("Pilih data pelayanan yang akan diupdate dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Validasi lainnya...
            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ...

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE_Pelayanan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_pelayanan", txtIdPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelanggan", comboBoxIdPelanggan.SelectedValue);
                    cmd.Parameters.AddWithValue("@tanggal_pelayanan", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@jenis_pelayanan", comboPelayanan.Text);
                    cmd.Parameters.AddWithValue("@total_harga", harga);
                    cmd.Parameters.AddWithValue("@potongan_harga", string.IsNullOrWhiteSpace(txtPotongan.Text) ? (object)DBNull.Value : decimal.Parse(txtPotongan.Text));
                    
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data pelayanan berhasil diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal mengupdate data pelayanan.", "Kesalahan Proses", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdPelayanan.Text) || txtIdPelayanan.Enabled)
            {
                MessageBox.Show("Pilih data pelayanan yang akan dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Yakin ingin menghapus data pelayanan ID: {txtIdPelayanan.Text}?\nSemua detail layanan terkait juga akan terhapus.", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE_Pelayanan", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_pelayanan", txtIdPelayanan.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Data pelayanan berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    ShowError("Gagal menghapus data pelayanan.", "Kesalahan Proses", ex);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }

        private void dgvPelayanan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPelayanan.Rows[e.RowIndex];
                txtIdPelayanan.Text = row.Cells["id_pelayanan"].Value?.ToString() ?? "";
                comboBoxIdPelanggan.SelectedValue = row.Cells["id_pelanggan"].Value;
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["tanggal_pelayanan"].Value);
                comboPelayanan.Text = row.Cells["jenis_pelayanan"].Value?.ToString() ?? "";
                txtHarga.Text = row.Cells["harga"].Value?.ToString() ?? "";
                txtPotongan.Text = row.Cells["potongan_harga"].Value != DBNull.Value ? row.Cells["potongan_harga"].Value.ToString() : "";
                txtIdPelayanan.Enabled = false; // Nonaktifkan edit ID
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.Show();
            this.Close();
        }


        // Event handler yang tidak digunakan, dapat dihapus jika tidak ada logika tambahan
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void comboBoxIdPelanggan_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { } // Asumsi ini adalah label untuk Harga Akhir

    }
}