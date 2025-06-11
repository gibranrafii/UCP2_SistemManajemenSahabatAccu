using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sahabat_Accu_1
{
    public partial class Form1 : Form
    {

        
        public Form1()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {
   
        }

        private void btnPelanggan_Click(object sender, EventArgs e)
        {
            Pelanggan pelangganForm = new Pelanggan(this);
            pelangganForm.Show();
            this.Hide();
        }

        private void btnBarang_Click(object sender, EventArgs e)
        {
            Barang barangForm = new Barang(this);
            barangForm.Show();
            this.Hide();
        }

        private void btnPelayanan_Click(object sender, EventArgs e)
        {
            Pelayanan pelayananForm = new Pelayanan(this);
            pelayananForm.Show();
            this.Hide();
        }

        private void btnKelolaKaryawan_Click(object sender, EventArgs e)
        {
            Karyawan karyawanForm = new Karyawan(this);
            karyawanForm.Show();
            this.Hide();
        }

        private void btnDetailLayanan_Click(object sender, EventArgs e)
        {
            Detail detailForm = new Detail(this);
            detailForm.Show();
            this.Hide();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            FormLaporan laporanForm = new FormLaporan(this);
            laporanForm.Show();
            this.Hide();
        }
    }
}
