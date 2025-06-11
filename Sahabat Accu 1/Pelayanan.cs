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
    public partial class Pelayanan : Form
    {
        private string connectionString = "Data Source=LAPTOP-QT79LBKA\\GIBRANRAFI;Initial Catalog=SistemManajemenSahabatAccu;Integrated Security=True;Pooling=False";
        private Form1 parentForm;

        public Pelayanan(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            // Tambahkan event handler untuk penghitungan harga akhir secara real-time
            txtHarga.TextChanged += CalculateHargaAkhir;
            txtPotongan.TextChanged += CalculateHargaAkhir;
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

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Memuat harga_akhir juga
                    string query = "SELECT id_pelayanan, id_pelanggan, tanggal_pelayanan, jenis_pelayanan, harga, potongan_harga, harga_akhir FROM Pelayanan";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPelayanan.DataSource = dt;
                    ClearForm(); // Clear form after loading data
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memuat data pelayanan dari database: {sqlEx.Message}", "Kesalahan Database", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat data pelayanan:", "Kesalahan Umum", ex);
            }
        }

        private void ClearForm()
        {
            txtIdPelayanan.Text = "";
            comboBoxIdPelanggan.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;
            comboPelayanan.SelectedIndex = 0; // Set ke item pertama
            txtHarga.Text = "";
            txtPotongan.Text = ""; // Clear potongan juga
            // Asumsi ada label lblHargaAkhir untuk menampilkan harga_akhir
            // Jika tidak ada, Anda bisa menghapus baris ini atau menambahkannya di desain form Anda
            // lblHargaAkhir.Text = "0";
            txtIdPelayanan.Focus(); // Fokus ke ID Pelayanan
        }

        private void InitComboBoxJenis()
        {
            comboPelayanan.Items.Clear();
            comboPelayanan.Items.Add("Charge Aki");
            comboPelayanan.Items.Add("Tuker Tambah");
            comboPelayanan.Items.Add("Pembelian");
            comboPelayanan.SelectedIndex = 0; // Set default selection
        }

        private void LoadPelangganToComboBox()
        {
            try
            {
                comboBoxIdPelanggan.Items.Clear(); // Bersihkan item lama
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT id_pelanggan FROM Pelanggan ORDER BY id_pelanggan"; // Order by for better UX
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBoxIdPelanggan.Items.Add(reader["id_pelanggan"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError($"Gagal memuat daftar pelanggan: {sqlEx.Message}", "Kesalahan Database", sqlEx);
            }
            catch (Exception ex)
            {
                ShowError("Gagal memuat daftar pelanggan:", "Kesalahan Umum", ex);
            }
        }

        // Asumsi Anda memiliki Label lblHargaAkhir di form Anda
        private void CalculateHargaAkhir(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtHarga.Text, out decimal harga) && harga >= 0)
            {
                decimal potongan = 0;
                if (decimal.TryParse(txtPotongan.Text, out decimal parsedPotongan) && parsedPotongan >= 0)
                {
                    potongan = parsedPotongan;
                }

                decimal hargaAkhir = harga - potongan;
                if (hargaAkhir < 0) hargaAkhir = 0; // Sesuai dengan logika AS (CASE WHEN potongan_harga > harga THEN 0 ELSE harga - potongan_harga END)
 
            }

        }

        private void Pelayanan_Load(object sender, EventArgs e)
        {
            InitComboBoxJenis();
            LoadPelangganToComboBox();
            LoadData(); // LoadData setelah combo box terisi
            // Panggil CalculateHargaAkhir agar harga awal terhitung jika ada nilai default
            CalculateHargaAkhir(null, EventArgs.Empty);
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtIdPelayanan.Text))
            {
                MessageBox.Show("ID Pelayanan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdPelayanan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(comboBoxIdPelanggan.Text))
            {
                MessageBox.Show("ID Pelanggan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxIdPelanggan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(comboPelayanan.Text))
            {
                MessageBox.Show("Jenis Pelayanan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboPelayanan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                MessageBox.Show("Harga wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHarga.Focus();
                return;
            }

            // Validasi format ID Pelayanan
            if (!Regex.IsMatch(txtIdPelayanan.Text.Trim(), "^PY[0-9]{3}$"))
            {
                MessageBox.Show("Format ID Pelayanan tidak sesuai. Harus 'PY' diikuti 3 angka (contoh: PY001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdPelayanan.Focus();
                return;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHarga.Focus();
                return;
            }

            decimal? potongan = null;
            if (!string.IsNullOrWhiteSpace(txtPotongan.Text))
            {
                if (!decimal.TryParse(txtPotongan.Text, out decimal parsedPotongan) || parsedPotongan < 0)
                {
                    MessageBox.Show("Potongan tidak valid. Masukkan angka positif atau kosongkan.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPotongan.Focus();
                    return;
                }
                potongan = parsedPotongan;
            }

            // Validasi Tanggal Sisi Client (sesuai CHECK constraint SQL)
            DateTime selectedDate = dateTimePicker1.Value.Date;
            DateTime today = DateTime.Today;
            DateTime sevenDaysAgo = today.AddDays(-7);

            if (selectedDate > today)
            {
                MessageBox.Show("Tanggal pelayanan tidak boleh lebih dari hari ini (" + today.ToString("dd/MM/yyyy") + ").", "Peringatan Input Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
                return;
            }
            if (selectedDate < sevenDaysAgo)
            {
                MessageBox.Show("Tanggal pelayanan tidak boleh kurang dari seminggu yang lalu (" + sevenDaysAgo.ToString("dd/MM/yyyy") + ").", "Peringatan Input Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
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

                    SqlCommand cmd = new SqlCommand("INSERT_Pelayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_pelayanan", txtIdPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelanggan", comboBoxIdPelanggan.Text.Trim());
                    cmd.Parameters.AddWithValue("@tanggal_pelayanan", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@jenis_pelayanan", comboPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@potongan_harga", (object)potongan ?? DBNull.Value);

                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data pelayanan berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (SqlException sqlEx)
                {
                    transaction?.Rollback();
                    string errorMessage = $"Gagal menambah data pelayanan ke database: {sqlEx.Message}";
                    Control focusControl = null; // Inisialisasi control yang akan difokuskan

                    switch (sqlEx.Number)
                    {
                        case 2627: // Primary Key violation (id_pelayanan must be unique)
                            errorMessage = "ID Pelayanan sudah ada. Masukkan ID lain atau gunakan fitur update.";
                            focusControl = txtIdPelayanan; // Fokus ke ID Pelayanan
                            break;
                        case 547: // Foreign Key constraint violation or CHECK constraint violation
                            if (sqlEx.Message.Contains("FK__Pelayanan__id_pe__")) // Foreign Key id_pelanggan
                            {
                                errorMessage = "ID Pelanggan tidak ditemukan. Pastikan ID Pelanggan yang Anda pilih valid.";
                                focusControl = comboBoxIdPelanggan; // Fokus ke ID Pelanggan
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__id_pe__")) // id_pelayanan LIKE 'PY[0-9][0-9][0-9]'
                            {
                                errorMessage = "Format ID Pelayanan tidak sesuai. Harus 'PY' diikuti 3 angka (contoh: PY001).";
                                focusControl = txtIdPelayanan; // Fokus ke ID Pelayanan
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__tanggal__")) // tanggal_pelayanan CHECK constraints
                            {
                                // Penanganan lebih spesifik untuk constraint tanggal
                                DateTime selectedDateDb = dateTimePicker1.Value.Date;
                                DateTime todayDb = DateTime.Today;
                                DateTime sevenDaysAgoDb = todayDb.AddDays(-7);

                                if (selectedDateDb > todayDb)
                                {
                                    errorMessage = "Tanggal pelayanan tidak boleh di masa depan (" + todayDb.ToString("dd/MM/yyyy") + ").";
                                }
                                else if (selectedDateDb < sevenDaysAgoDb)
                                {
                                    errorMessage = "Tanggal pelayanan tidak boleh kurang dari seminggu yang lalu (" + sevenDaysAgoDb.ToString("dd/MM/yyyy") + ").";
                                }
                                else
                                {
                                    errorMessage = "Tanggal pelayanan tidak valid. Pastikan tanggal dalam rentang yang diizinkan.";
                                }
                                focusControl = dateTimePicker1; // Fokus ke DatePicker
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__jenis__")) // jenis_pelayanan IN ('Charge Aki', 'Tuker Tambah', 'Pembelian')
                            {
                                errorMessage = "Jenis pelayanan tidak valid. Pilih dari daftar yang tersedia.";
                                focusControl = comboPelayanan; // Fokus ke Jenis Pelayanan
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__harga__")) // harga >= 0
                            {
                                errorMessage = "Harga tidak boleh kurang dari 0.";
                                focusControl = txtHarga; // Fokus ke Harga
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__poton__")) // potongan_harga >= 0
                            {
                                errorMessage = "Potongan harga tidak boleh kurang dari 0.";
                                focusControl = txtPotongan; // Fokus ke Potongan
                            }
                            else
                            {
                                errorMessage = $"Terjadi pelanggaran constraint: {sqlEx.Message}";
                            }
                            break;
                        case 8152: // String or binary data would be truncated (e.g., input too long)
                            errorMessage = "Ada input yang terlalu panjang untuk kolom database. Mohon periksa kembali panjang data.";
                            // Sulit menentukan control spesifik di sini tanpa parsing pesan error yang lebih canggih
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Penambahan Data Pelayanan");
                    focusControl?.Focus(); // Fokuskan control jika sudah ditentukan
                }
                catch (FormatException fEx) // Menangkap kesalahan parsing jika tidak ditangani di TryParse (misal harga/potongan bukan angka)
                {
                    transaction?.Rollback();
                    ShowError("Format input angka tidak valid. Pastikan Harga dan Potongan diisi dengan benar.", "Input Data Tidak Valid", fEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menyimpan data pelayanan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // --- Validasi Input Client-Side ---
            if (string.IsNullOrWhiteSpace(txtIdPelayanan.Text))
            {
                MessageBox.Show("Pilih data pelayanan yang akan diupdate!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(comboBoxIdPelanggan.Text))
            {
                MessageBox.Show("ID Pelanggan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxIdPelanggan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(comboPelayanan.Text))
            {
                MessageBox.Show("Jenis Pelayanan wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboPelayanan.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtHarga.Text))
            {
                MessageBox.Show("Harga wajib diisi.", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHarga.Focus();
                return;
            }

            // ID Pelayanan tidak divalidasi formatnya karena diasumsikan sudah benar saat dipilih dari DGV
            // Validasi format ID Pelayanan (jika masih diperlukan, misalnya jika ID bisa diubah manual)
            // if (!Regex.IsMatch(txtIdPelayanan.Text.Trim(), "^PY[0-9]{3}$"))
            // {
            //     MessageBox.Show("Format ID Pelayanan tidak sesuai. Harus 'PY' diikuti 3 angka (contoh: PY001).", "Peringatan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     txtIdPelayanan.Focus();
            //     return;
            // }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga < 0)
            {
                MessageBox.Show("Harga tidak valid. Masukkan angka positif.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHarga.Focus();
                return;
            }

            decimal? potongan = null;
            if (!string.IsNullOrWhiteSpace(txtPotongan.Text))
            {
                if (!decimal.TryParse(txtPotongan.Text, out decimal parsedPotongan) || parsedPotongan < 0)
                {
                    MessageBox.Show("Potongan tidak valid. Masukkan angka positif atau kosongkan.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPotongan.Focus();
                    return;
                }
                potongan = parsedPotongan;
            }

            // Validasi Tanggal Sisi Client (sesuai CHECK constraint SQL)
            DateTime selectedDate = dateTimePicker1.Value.Date;
            DateTime today = DateTime.Today;
            DateTime sevenDaysAgo = today.AddDays(-7);

            if (selectedDate > today)
            {
                MessageBox.Show("Tanggal pelayanan tidak boleh lebih dari hari ini (" + today.ToString("dd/MM/yyyy") + ").", "Peringatan Input Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
                return;
            }
            if (selectedDate < sevenDaysAgo)
            {
                MessageBox.Show("Tanggal pelayanan tidak boleh kurang dari seminggu yang lalu (" + sevenDaysAgo.ToString("dd/MM/yyyy") + ").", "Peringatan Input Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
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

                    SqlCommand cmd = new SqlCommand("UPDATE_Pelayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_pelayanan", txtIdPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@id_pelanggan", comboBoxIdPelanggan.Text.Trim());
                    cmd.Parameters.AddWithValue("@tanggal_pelayanan", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@jenis_pelayanan", comboPelayanan.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@potongan_harga", (object)potongan ?? DBNull.Value);

                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data pelayanan berhasil diperbarui.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (SqlException sqlEx)
                {
                    transaction?.Rollback();
                    string errorMessage = $"Gagal mengupdate data pelayanan di database: {sqlEx.Message}";
                    Control focusControl = null; // Inisialisasi control yang akan difokuskan

                    switch (sqlEx.Number)
                    {
                        case 547: // Foreign Key constraint violation or CHECK constraint violation
                            if (sqlEx.Message.Contains("FK__Pelayanan__id_pe__")) // Foreign Key id_pelanggan
                            {
                                errorMessage = "ID Pelanggan tidak ditemukan. Pastikan ID Pelanggan yang Anda pilih valid.";
                                focusControl = comboBoxIdPelanggan; // Fokus ke ID Pelanggan
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__id_pe__")) // id_pelayanan LIKE 'PY[0-9][0-9][0-9]'
                            {
                                // Ini jarang terjadi pada update karena ID sudah ada, tapi jika user bisa edit ID, perlu ini
                                errorMessage = "Format ID Pelayanan tidak sesuai. Harus 'PY' diikuti 3 angka (contoh: PY001).";
                                focusControl = txtIdPelayanan; // Fokus ke ID Pelayanan
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__tanggal__")) // tanggal_pelayanan CHECK constraints
                            {
                                // Penanganan lebih spesifik untuk constraint tanggal
                                DateTime selectedDateDb = dateTimePicker1.Value.Date;
                                DateTime todayDb = DateTime.Today;
                                DateTime sevenDaysAgoDb = todayDb.AddDays(-7);

                                if (selectedDateDb > todayDb)
                                {
                                    errorMessage = "Tanggal pelayanan tidak boleh di masa depan (" + todayDb.ToString("dd/MM/yyyy") + ").";
                                }
                                else if (selectedDateDb < sevenDaysAgoDb)
                                {
                                    errorMessage = "Tanggal pelayanan tidak boleh kurang dari seminggu yang lalu (" + sevenDaysAgoDb.ToString("dd/MM/yyyy") + ").";
                                }
                                else
                                {
                                    errorMessage = "Tanggal pelayanan tidak valid. Pastikan tanggal dalam rentang yang diizinkan.";
                                }
                                focusControl = dateTimePicker1; // Fokus ke DatePicker
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__jenis__")) // jenis_pelayanan IN ('Charge Aki', 'Tuker Tambah', 'Pembelian')
                            {
                                errorMessage = "Jenis pelayanan tidak valid. Pilih dari daftar yang tersedia.";
                                focusControl = comboPelayanan; // Fokus ke Jenis Pelayanan
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__harga__")) // harga >= 0
                            {
                                errorMessage = "Harga tidak boleh kurang dari 0.";
                                focusControl = txtHarga; // Fokus ke Harga
                            }
                            else if (sqlEx.Message.Contains("CHECK constraint 'CK__Pelayanan__poton__")) // potongan_harga >= 0
                            {
                                errorMessage = "Potongan harga tidak boleh kurang dari 0.";
                                focusControl = txtPotongan; // Fokus ke Potongan
                            }
                            else
                            {
                                errorMessage = $"Terjadi pelanggaran constraint: {sqlEx.Message}";
                            }
                            break;
                        case 8152: // String or binary data would be truncated
                            errorMessage = "Ada input yang terlalu panjang untuk kolom database. Mohon periksa kembali panjang data.";
                            // Sulit menentukan control spesifik di sini tanpa parsing pesan error yang lebih canggih
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Pembaruan Data Pelayanan");
                    focusControl?.Focus(); // Fokuskan control jika sudah ditentukan
                }
                catch (FormatException fEx) // Menangkap kesalahan parsing jika tidak ditangani di TryParse
                {
                    transaction?.Rollback();
                    ShowError("Format input angka tidak valid. Pastikan Harga dan Potongan diisi dengan benar.", "Input Data Tidak Valid", fEx);
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat memperbarui data pelayanan. Perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdPelayanan.Text))
            {
                MessageBox.Show("Pilih data pelayanan yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Yakin ingin menghapus data pelayanan dengan ID: {txtIdPelayanan.Text}?\n\nSemua detail layanan yang terkait juga akan terhapus secara otomatis (ON DELETE CASCADE).", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("DELETE_Pelayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pelayanan", txtIdPelayanan.Text.Trim());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data pelayanan berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (SqlException sqlEx)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    string errorMessage = $"Gagal menghapus data pelayanan dari database: {sqlEx.Message}";
                    switch (sqlEx.Number)
                    {
                        // Tidak ada FK yang menyebabkan error 547 saat menghapus Pelayanan karena ON DELETE CASCADE
                        // di Detail_Layanan. Error 547 mungkin muncul jika ada FK lain dengan NO ACTION.
                        case 547:
                            errorMessage = $"Terjadi pelanggaran constraint saat menghapus: {sqlEx.Message}\n\nPastikan tidak ada data lain yang menghambat penghapusan.";
                            break;
                        default:
                            errorMessage += "\n\n" + GetSqlErrorMessage(sqlEx);
                            break;
                    }
                    ShowError(errorMessage, "Kesalahan Penghapusan Data Pelayanan");
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    ShowError("Terjadi kesalahan umum saat menghapus data pelayanan. Semua perubahan dibatalkan.", "Kesalahan Umum", ex);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadPelangganToComboBox(); // Refresh pelanggan juga jika ada perubahan
            ClearForm();
        }

        private void dgvPelayanan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPelayanan.Rows[e.RowIndex];
                txtIdPelayanan.Text = row.Cells["id_pelayanan"].Value?.ToString() ?? ""; // Use null-conditional operator
                // Menangani DBNull untuk id_pelanggan jika ON DELETE SET NULL diaktifkan
                comboBoxIdPelanggan.Text = row.Cells["id_pelanggan"].Value != DBNull.Value ? row.Cells["id_pelanggan"].Value.ToString() : "";
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["tanggal_pelayanan"].Value);
                comboPelayanan.Text = row.Cells["jenis_pelayanan"].Value?.ToString() ?? ""; // Use null-conditional operator
                txtHarga.Text = row.Cells["harga"].Value?.ToString() ?? ""; // Mengambil dari kolom 'harga'
                // Menangani DBNull untuk potongan_harga
                txtPotongan.Text = row.Cells["potongan_harga"].Value != DBNull.Value ? row.Cells["potongan_harga"].Value.ToString() : "";
                CalculateHargaAkhir(null, EventArgs.Empty); // Update the calculated final price label
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