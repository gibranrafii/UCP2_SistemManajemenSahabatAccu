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
using System.Runtime.Caching;

namespace Sahabat_Accu_1
{
    public partial class Pelanggan : Form
    {

        Koneksi kn = new Koneksi();

        private Form1 parentForm;

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "PelangganData";

        public Pelanggan(Form1 form1)
        {
            InitializeComponent();
            parentForm = form1;
        }

        public void EnsureIndexes()
        {
            try
            {
                using (var conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    var indexScript = @"
                     IF OBJECT_ID('dbo.Pelanggan', 'U') IS NOT NULL
                     BEGIN
                         -- Indeks pada kolom Nama
                         IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_Pelanggan_Nama' AND object_id = OBJECT_ID('dbo.Pelanggan'))
                             CREATE NONCLUSTERED INDEX idx_Pelanggan_Nama ON dbo.Pelanggan(Nama);
                        
                         -- Indeks pada kolom No_hp
                         IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_Pelanggan_NoHp' AND object_id = OBJECT_ID('dbo.Pelanggan'))
                             CREATE NONCLUSTERED INDEX idx_Pelanggan_NoHp ON dbo.Pelanggan(no_hp);
                     END";

                    using (var cmd = new SqlCommand(indexScript, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Terjadi kesalahan saat memastikan indeks.", "Kesalahan Indeks", ex);
            }
        }

        public void AnalyzeQuery(string sqlQuery)
        {
            using (var conn = new SqlConnection(kn.connectionString()))
            {
                conn.InfoMessage += (s, e) =>
                {
                    if (e.Message.Contains("SQL Server parse and compile time") || e.Message.Contains("Table") || e.Message.Contains("Worktable"))
                    {
                        MessageBox.Show(e.Message, "STATISTICS INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };

                try
                {
                    conn.Open();
                    var wrapped = $@"
                     SET STATISTICS IO ON;
                     SET STATISTICS TIME ON;
                     {sqlQuery};
                     SET STATISTICS TIME OFF;
                     SET STATISTICS IO OFF;
                     ";

                    using (var cmd = new SqlCommand(wrapped, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Terjadi kesalahan saat analisis query.", "Kesalahan Analisis Query", ex);
                }
            }
        }

        private void Pelanggan_Load(object sender, EventArgs e)
        {
            EnsureIndexes();
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtNama.Clear();
            txtHp.Clear();
            txtAlamat.Clear();
            txtId.Focus();
            txtCari.Clear();
            txtId.Enabled = true;
        }

        private void ShowError(string message, string title = "Terjadi Kesalahan", Exception ex = null)
        {
            string fullMessage = message;

            if (ex is SqlException sqlEx)
            {
                string sqlErrorMessage = sqlEx.Message.ToLower();
                if (sqlErrorMessage.Contains("duplicate key"))
                {
                    fullMessage = "ID yang Anda masukkan sudah ada.";
                    title = "Peringatan Input Duplikat";
                }
                else if (sqlErrorMessage.Contains("foreign key") || sqlErrorMessage.Contains("check constraint"))
                {
                    fullMessage = "Data ini tidak dapat diubah/dihapus karena terkait dengan data lain (misal: di tabel Pelayanan).";
                    title = "Kesalahan Validasi Data";
                }
                else
                {
                    fullMessage += $"\n\nDetail Error: {sqlEx.Message}";
                }
            }
            else if (ex != null)
            {
                fullMessage += $"\n\nDetail Error Umum:\n{ex.Message}";
            }
            MessageBox.Show(fullMessage, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LoadData()
        {
            DataTable dt;

            if (_cache.Contains(CacheKey))
            {
                dt = _cache.Get(CacheKey) as DataTable;
            }
            else
            {
                dt = new DataTable();
                try
                {
                    using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                    {
                        conn.Open();
                        string query = "SELECT id_pelanggan AS [ID], nama AS [Nama], no_hp AS [No HP], alamat AS [Alamat] FROM Pelanggan";
                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        da.Fill(dt);
                    }
                    _cache.Add(CacheKey, dt, _policy);
                }
                catch (Exception ex)
                {
                    ShowError("Gagal memuat data pelanggan.", "Kesalahan Memuat Data", ex);
                    return;
                }
            }
            dgvPelanggan.DataSource = dt;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtHp.Text))
            {
                MessageBox.Show("ID Pelanggan, Nama, No HP, Alamat wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(txtId.Text.Trim(), "^PG[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Pelanggan tidak sesuai. Harus 'PG' diikuti 3 angka.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai. Harus diawali '08' dan panjang 10-13 digit.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT_Pelanggan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                _cache.Remove(CacheKey);
                MessageBox.Show("Data pelanggan berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menambah data pelanggan.", "Kesalahan Proses", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPelanggan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string id = dgvPelanggan.SelectedRows[0].Cells["ID"].Value.ToString();
            if (MessageBox.Show($"Yakin ingin menghapus pelanggan dengan ID: {id}?", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE_Pelanggan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", id);
                    cmd.ExecuteNonQuery();
                }
                _cache.Remove(CacheKey);
                MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal menghapus data pelanggan.", "Kesalahan Proses", ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || txtId.Enabled)
            {
                MessageBox.Show("Pilih data yang akan diupdate dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    SqlCommand cmd = new SqlCommand("UPDATE_Pelanggan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                _cache.Remove(CacheKey);
                MessageBox.Show("Data berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError("Gagal mengupdate data.", "Kesalahan Proses", ex);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _cache.Remove(CacheKey);
            LoadData();
            ClearForm();
        }

        private void dgvPelanggan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPelanggan.Rows[e.RowIndex];
                txtId.Text = row.Cells["ID"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                txtHp.Text = row.Cells["No HP"].Value.ToString();
                txtAlamat.Text = row.Cells["Alamat"].Value.ToString();
                txtId.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parentForm.Show();
            this.Close();
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            var heavyQuery = "SELECT Id_pelanggan, Nama, No_hp, Alamat FROM dbo.Pelanggan WHERE Nama LIKE 'A%'";
            try
            {
                AnalyzeQuery(heavyQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat analisis query:\n{ex.Message}", "Error Umum Analisis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            string keyword = txtCari.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                dgvPelanggan.DataSource = _cache.Get(CacheKey) as DataTable;
                return;
            }

            DataTable dt = _cache.Get(CacheKey) as DataTable;
            if (dt != null)
            {
                try
                {
                    DataView dv = dt.DefaultView;
                    // Pencarian di beberapa kolom
                    dv.RowFilter = $"Nama LIKE '%{keyword}%' OR [No HP] LIKE '%{keyword}%'";
                    dgvPelanggan.DataSource = dv.ToTable();

                    if (dv.Count == 0)
                    {
                        MessageBox.Show("Data pelanggan tidak ditemukan.", "Informasi Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Gagal melakukan pencarian.", "Kesalahan Pencarian", ex);
                }
            }
        }
    }
}