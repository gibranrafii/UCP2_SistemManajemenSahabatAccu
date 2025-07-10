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

namespace Sahabat_Accu_1
{
    public partial class PreviewData : Form
    {
        Koneksi kn = new Koneksi();

        private Form KaryawanFormInstance;

        
        public PreviewData(DataTable data, Form callingForm)
        {
            InitializeComponent();
            dgvPreview.DataSource = data; 
            KaryawanFormInstance = callingForm; 
        }

        private void PreviewForm_Load(object sender, EventArgs e)
        {
            // Auto-resize columns to fit content
            dgvPreview.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (KaryawanFormInstance != null)
            {
                KaryawanFormInstance.Show();
            }
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin mengimpor data ini ke database?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ImportDataToDatabase();
            }
        }

        private bool ValidateRow(DataRow row)
        {

            string idKaryawan = row["id_karyawan"].ToString().Trim();
            string nama = row["nama"].ToString().Trim();
            string noHp = row["no_hp"].ToString().Trim();


            if (string.IsNullOrEmpty(idKaryawan) || !System.Text.RegularExpressions.Regex.IsMatch(idKaryawan, "^KR[0-9]{3}$"))
            {
                MessageBox.Show($"ID Karyawan '{idKaryawan}' tidak valid. Harus 'KR' diikuti 3 digit angka (e.g., KR001).", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(nama))
            {
                MessageBox.Show("Nama Karyawan tidak boleh kosong.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(noHp) || !System.Text.RegularExpressions.Regex.IsMatch(noHp, "^08[0-9]{8,11}$"))
            {
                MessageBox.Show($"Nomor HP '{noHp}' tidak valid. Harus dimulai dengan '08' dan memiliki panjang 10-13 digit angka.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ImportDataToDatabase()
        {
            try
            {
                DataTable dt = (DataTable)dgvPreview.DataSource;
                int importedCount = 0;

                using (SqlConnection conn = new SqlConnection(kn.connectionString()))
                {
                    conn.Open();
                    foreach (DataRow row in dt.Rows)
                    {

                        if (!ValidateRow(row))
                        {

                            continue; // Skip this row if invalid
                        }

                        // Use the stored procedure INSERT_Karyawan
                        using (SqlCommand cmd = new SqlCommand("INSERT_Karyawan", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@id_karyawan", row["id_karyawan"].ToString());
                            cmd.Parameters.AddWithValue("@nama", row["nama"].ToString());
                            cmd.Parameters.AddWithValue("@no_hp", row["no_hp"].ToString());
                            cmd.Parameters.AddWithValue("@alamat", row["alamat"].ToString());

                            cmd.ExecuteNonQuery();
                            importedCount++;
                        }
                    }
                }

                MessageBox.Show($"{importedCount} data berhasil diimpor ke database.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                if (KaryawanFormInstance != null)
                {
                    KaryawanFormInstance.Show();
                }
                this.Close();
            }
            catch (SqlException sqlEx)
            {
               
                string errorMessage = "Terjadi kesalahan database saat mengimpor data: ";
                if (sqlEx.Number == 2627) 
                {
                    errorMessage += "ID Karyawan sudah ada. Mohon gunakan ID yang unik.";
                }
                else if (sqlEx.Number == 547) 
                {
                    errorMessage += "Data tidak memenuhi persyaratan database (misal: format ID/nomor HP salah).";
                }
                else
                {
                    errorMessage += sqlEx.Message;
                }
                MessageBox.Show(errorMessage, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengimpor data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}