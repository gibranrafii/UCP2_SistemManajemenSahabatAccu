using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sahabat_Accu_1
{
    public partial class FormLaporan : Form
    {
        Koneksi kn = new Koneksi();
        string connect = "";


        private Form1 parentForm;
        public FormLaporan(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void FormLaporan_Load(object sender, EventArgs e)
        {
            SetupReportViewer();
            this.reportViewer1.RefreshReport();
        }

        private void SetupReportViewer()
        {
            // Connection string to your database
            connect = kn.connectionString();

            // SQL query to retrieve the required data from the database
            string query = @"
                SELECT
                    dl.id_detail,
                    dl.id_pelayanan,
                    dl.id_barang,
                    dl.id_karyawan,
                    dl.jumlah,
                    dl.subtotal,
                    p.nama AS nama_pelanggan,         -- Nama pelanggan dari tabel Pelanggan
                    p.no_hp AS no_hp_pelanggan,       -- Nomor HP pelanggan dari tabel Pelanggan
                    k.nama AS nama_karyawan          -- Nama karyawan dari tabel Karyawan
                FROM
                    Detail_Layanan dl
                JOIN
                    Pelayanan py ON dl.id_pelayanan = py.id_pelayanan
                JOIN
                    Pelanggan p ON py.id_pelanggan = p.id_pelanggan
                JOIN
                    Karyawan k ON dl.id_karyawan = k.id_karyawan;";

            // Create a DataTable to store the data
            DataTable dt = new DataTable();

            // Use SqlDataAdapter to fill the DataTable with data from the database
            using (SqlConnection conn = new SqlConnection(kn.connectionString()))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            // Create a ReportDataSource
            ReportDataSource rds = new ReportDataSource("DataSet1", dt); // Make sure "DataSet1" matches your RDLC dataset name

            // Clear any existing data sources and add the new data source
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Set the path to the report (.rdlc file)
            // Change this to the actual path of your RDLC file
            reportViewer1.LocalReport.ReportPath = @"D:\Kuliah\Semester 4\Pengembangan Aplikasi Basis Data\Sahabat Accu 2 (fix bismillah terakhir)\Sahabat Accu 1\Sahabat Accu 1\Report2.rdlc";
            // Refresh the ReportViewer to show the updated report
            reportViewer1.RefreshReport();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.Show();
            this.Close();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
