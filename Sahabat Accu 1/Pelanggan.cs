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
        private string connectionString = "Data Source=LAPTOP-QT79LBKA\\GIBRANRAFI;Initial Catalog=SistemManajemenSahabatAccu;Integrated Security=True;Pooling=False";
        private Form1 parentForm;

        // Deklarasi MemoryCache dan CacheItemPolicy
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
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var indexScript = @"
                    IF OBJECT_ID('dbo.Pelanggan', 'U') IS NOT NULL
                    BEGIN
                        -- Indeks pada kolom Nama
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_Pelanggan_Nama')
                            CREATE NONCLUSTERED INDEX idx_Pelanggan_Nama ON dbo.Pelanggan(Nama);
                        
                        -- Indeks pada kolom No_hp
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_Pelanggan_NoHp')
                            CREATE NONCLUSTERED INDEX idx_Pelanggan_NoHp ON dbo.Pelanggan(no_hp);
                    END";

                    using (var cmd = new SqlCommand(indexScript, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // MessageBox.Show("Indeks berhasil dipastikan atau dibuat.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information); //OPSIONAL
                }
            }
            catch (SqlException sqlEx)
            {
                string detailError = GetSqlErrorMessage(sqlEx);
                ShowError($"Terjadi kesalahan database saat memastikan indeks: {sqlEx.Message}\n\n{detailError}", "Kesalahan Database Indeks", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError($"Terjadi kesalahan umum saat memastikan indeks: {ex.Message}", "Kesalahan Indeks", ex);
            }
        }

        public void AnalyzeQuery(string sqlQuery)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.InfoMessage += (s, e) =>
                {
                    // Hanya tampilkan pesan yang relevan (STATISTICS INFO)
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
                catch (SqlException sqlEx)
                {
                    string detailError = GetSqlErrorMessage(sqlEx);
                    ShowError($"Terjadi kesalahan database saat analisis query:\n{sqlEx.Message}\n\n{detailError}", "Kesalahan Analisis Query (SQL)", sqlEx);
                }
                catch (Exception ex)
                {
                    ShowError($"Terjadi kesalahan umum saat analisis query:\n{ex.Message}", "Kesalahan Analisis Query", ex);
                }
            }
        }

        private void Pelanggan_Load(object sender, EventArgs e)
        {
            EnsureIndexes(); // Pastikan indeks dibuat saat form load
            LoadData();
            ClearForm(); // Kosongkan form setelah load data awal
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtNama.Clear();
            txtHp.Clear();
            txtAlamat.Clear();
            txtId.Focus();
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
            DataTable dt;

            // Cek apakah data ada di cache
            if (_cache.Contains(CacheKey))
            {
                dt = _cache.Get(CacheKey) as DataTable;
                
                // MessageBox.Show("Data pelanggan dimuat dari cache.", "Informasi Cache", MessageBoxButtons.OK, MessageBoxIcon.Information); //opsional
            }
            else
            {
                // Jika tidak ada di cache, ambil dari database
                dt = new DataTable();
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT id_pelanggan AS [ID], nama AS [Nama], no_hp AS [No HP], alamat AS [Alamat] FROM Pelanggan";
                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        da.Fill(dt);
                    }
                    // Simpan data ke cache setelah diambil dari database
                    _cache.Add(CacheKey, dt, _policy);

                    // MessageBox.Show("Data pelanggan dimuat dari database dan disimpan ke cache.", "Informasi Cache", MessageBoxButtons.OK, MessageBoxIcon.Information); //OPSIONAL
                }
                catch (SqlException sqlEx)
                {
                    ShowError($"Gagal memuat data pelanggan dari database: {sqlEx.Message}", "Kesalahan Database", sqlEx);
                    return; // Hentikan eksekusi jika gagal memuat data
                }
                catch (Exception ex)
                {
                    ShowError("Gagal memuat data:", "Kesalahan Umum", ex);
                    return; // Hentikan eksekusi jika gagal memuat data
                }
            }

            dgvPelanggan.DataSource = dt;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtHp.Text))
            {
                MessageBox.Show("ID Pelanggan, Nama, dan No HP wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi format ID Pelanggan
            if (!Regex.IsMatch(txtId.Text.Trim(), "^PG[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Pelanggan tidak sesuai. Harus 'PG' diikuti 3 angka (contoh: PG001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi No HP (awalan 08, hanya angka, panjang 10-13)
            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai. Harus diawali '08', hanya terdiri dari angka, dan panjangnya antara 10-13 digit.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    SqlCommand cmd = new SqlCommand("INSERT_Pelanggan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    // Penting: Hapus data dari cache setelah operasi perubahan
                    _cache.Remove(CacheKey);
                    MessageBox.Show("Data pelanggan berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    string errorMessage = $"Gagal menambah data pelanggan ke database."; // Pesan awal yang lebih umum

                    switch (sqlEx.Number)
                    {
                        case 2627: // Primary Key violation (id_pelanggan must be unique)
                            errorMessage = "ID Pelanggan sudah ada. Masukkan ID lain atau gunakan fitur update.";
                            break;
                        case 547: // Foreign Key constraint violation or CHECK constraint violation
                            // Identifikasi CHECK constraint berdasarkan pesan error
                            if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelanggan__id_pe__")) // id_pelanggan LIKE 'PG[0-9][0-9][0-9]'
                            {
                                errorMessage = "Format ID Pelanggan tidak sesuai. Harus 'PG' diikuti 3 angka (contoh: PG001).";
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelanggan__no_hp__")) // LEN(no_hp) >= 10 AND LEN(no_hp) <= 13 AND no_hp LIKE '08%' AND no_hp NOT LIKE '%[^0-9]%'
                            {
                                errorMessage = "Format No HP tidak sesuai. Harus diawali '08', hanya terdiri dari angka, dan panjangnya antara 10-13 digit.";
                            }
                            else if (sqlEx.Message.Contains("FK__Pelayanan__id_pe__")) // Foreign Key (jika Pelanggan direferensikan dan gagal dihapus)
                            {
                                errorMessage = "Terjadi pelanggaran kunci asing. Pastikan data terkait valid.";
                            }
                            else
                            {
                                errorMessage += $"\nTerjadi pelanggaran constraint: {sqlEx.Message}"; // Tambahkan pesan default jika tidak teridentifikasi
                            }
                            break;
                        case 515: // Cannot insert the value NULL into column (e.g. nama NOT NULL tapi kosong)
                            errorMessage = "Ada kolom wajib isi (misal: Nama) yang tidak boleh kosong.";
                            break;
                        case 8152: // String or binary data would be truncated (e.g., input terlalu panjang)
                            errorMessage = "Ada input yang terlalu panjang untuk salah satu kolom. Mohon periksa kembali panjang data.";
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx); // Tambahkan detail error SQL standar
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Penambahan Data Pelanggan", sqlEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menyimpan data pelanggan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
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

            DialogResult dialog = MessageBox.Show("Yakin ingin menghapus data ini? Semua data terkait (misalnya di Pelayanan) akan di-SET NULL.", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction = null;
                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();

                        SqlCommand cmd = new SqlCommand("DELETE_Pelanggan", conn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_pelanggan", id);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        // Penting: Hapus data dari cache setelah operasi perubahan
                        _cache.Remove(CacheKey);
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    catch (SqlException sqlEx)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        string errorMessage = $"Gagal menghapus data pelanggan dari database."; // Pesan awal yang lebih umum
                        switch (sqlEx.Number)
                        {
                            case 547: // Foreign Key Constraint violation (jika ada FK lain dengan ON DELETE NO ACTION/RESTRICT)
                                errorMessage = "Data ini tidak dapat dihapus karena masih terkait dengan data di tabel lain yang tidak memiliki kebijakan 'ON DELETE CASCADE' atau 'SET NULL'. Pastikan semua data terkait sudah tidak ada atau disesuaikan.";
                                break;
                            default:
                                errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                                break;
                        }
                        ShowError(errorMessage, "Kesalahan Penghapusan Data Pelanggan", sqlEx);
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        ShowError("Gagal menghapus data: " + ex.Message, "Kesalahan Umum", ex);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Pilih data yang akan diupdate!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtHp.Text))
            {
                MessageBox.Show("Nama dan No HP wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!Regex.IsMatch(txtHp.Text.Trim(), "^08[0-9]{8,11}$"))
            {
                MessageBox.Show("Format No HP tidak sesuai. Harus diawali '08', hanya terdiri dari angka, dan panjangnya antara 10-13 digit.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("UPDATE_Pelanggan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelanggan", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_hp", txtHp.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    // Penting: Hapus data dari cache setelah operasi perubahan
                    _cache.Remove(CacheKey);
                    MessageBox.Show("Data berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    string errorMessage = $"Gagal mengupdate data pelanggan di database."; // Pesan awal yang lebih umum
                    switch (sqlEx.Number)
                    {
                        case 2627: // Primary Key violation (jika mencoba mengubah ID ke yang sudah ada, meski SP ini tidak mengubah ID)
                            errorMessage = "ID Pelanggan yang diupdate sudah ada untuk data lain.";
                            break;
                        case 547: // Constraint violation (CHECK constraint)
                            if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelanggan__no_hp__")) // LEN(no_hp) >= 10 AND LEN(no_hp) <= 13 AND no_hp LIKE '08%' AND no_hp NOT LIKE '%[^0-9]%'
                            {
                                errorMessage = "Format No HP tidak sesuai. Harus diawali '08', hanya terdiri dari angka, dan panjangnya antara 10-13 digit.";
                            }
                            else if (sqlEx.Message.Contains("FK__Pelayanan__id_pe__")) // Foreign Key (jika Pelanggan direferensikan dan gagal dihapus)
                            {
                                errorMessage = "Terjadi pelanggaran kunci asing. Pastikan data terkait valid.";
                            }
                            else
                            {
                                errorMessage += $"\nTerjadi pelanggaran constraint: {sqlEx.Message}";
                            }
                            break;
                        case 515: // Cannot insert the value NULL into column (e.g. nama NOT NULL tapi kosong)
                            errorMessage = "Ada kolom wajib isi (misal: Nama) yang tidak boleh kosong.";
                            break;
                        case 8152: // String or binary data would be truncated (e.g. nama terlalu panjang)
                            errorMessage = "Input terlalu panjang untuk salah satu kolom. Mohon periksa kembali panjang data.";
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Pembaruan Data Pelanggan", sqlEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Gagal mengupdate data: " + ex.Message, "Kesalahan Umum", ex);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Hapus data dari cache agar data terbaru diambil dari database
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
    }
}
