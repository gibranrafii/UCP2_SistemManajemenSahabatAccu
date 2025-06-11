namespace Sahabat_Accu_1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnPelanggan = new System.Windows.Forms.Button();
            this.btnBarang = new System.Windows.Forms.Button();
            this.btnPelayanan = new System.Windows.Forms.Button();
            this.btnKelolaKaryawan = new System.Windows.Forms.Button();
            this.btnDetailLayanan = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(183, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Aplikasi Sahabat Accu";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnPelanggan
            // 
            this.btnPelanggan.BackColor = System.Drawing.SystemColors.Info;
            this.btnPelanggan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPelanggan.Location = new System.Drawing.Point(73, 118);
            this.btnPelanggan.Name = "btnPelanggan";
            this.btnPelanggan.Size = new System.Drawing.Size(180, 51);
            this.btnPelanggan.TabIndex = 1;
            this.btnPelanggan.Text = "Pelanggan";
            this.btnPelanggan.UseVisualStyleBackColor = false;
            this.btnPelanggan.Click += new System.EventHandler(this.btnPelanggan_Click);
            // 
            // btnBarang
            // 
            this.btnBarang.BackColor = System.Drawing.SystemColors.Info;
            this.btnBarang.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBarang.Location = new System.Drawing.Point(73, 238);
            this.btnBarang.Name = "btnBarang";
            this.btnBarang.Size = new System.Drawing.Size(180, 51);
            this.btnBarang.TabIndex = 2;
            this.btnBarang.Text = "Barang";
            this.btnBarang.UseVisualStyleBackColor = false;
            this.btnBarang.Click += new System.EventHandler(this.btnBarang_Click);
            // 
            // btnPelayanan
            // 
            this.btnPelayanan.BackColor = System.Drawing.SystemColors.Info;
            this.btnPelayanan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPelayanan.Location = new System.Drawing.Point(424, 118);
            this.btnPelayanan.Name = "btnPelayanan";
            this.btnPelayanan.Size = new System.Drawing.Size(176, 51);
            this.btnPelayanan.TabIndex = 3;
            this.btnPelayanan.Text = "Pelayanan";
            this.btnPelayanan.UseVisualStyleBackColor = false;
            this.btnPelayanan.Click += new System.EventHandler(this.btnPelayanan_Click);
            // 
            // btnKelolaKaryawan
            // 
            this.btnKelolaKaryawan.BackColor = System.Drawing.SystemColors.Info;
            this.btnKelolaKaryawan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKelolaKaryawan.Location = new System.Drawing.Point(424, 238);
            this.btnKelolaKaryawan.Name = "btnKelolaKaryawan";
            this.btnKelolaKaryawan.Size = new System.Drawing.Size(176, 51);
            this.btnKelolaKaryawan.TabIndex = 4;
            this.btnKelolaKaryawan.Text = "Kelola Karyawan";
            this.btnKelolaKaryawan.UseVisualStyleBackColor = false;
            this.btnKelolaKaryawan.Click += new System.EventHandler(this.btnKelolaKaryawan_Click);
            // 
            // btnDetailLayanan
            // 
            this.btnDetailLayanan.BackColor = System.Drawing.SystemColors.Info;
            this.btnDetailLayanan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDetailLayanan.Location = new System.Drawing.Point(249, 319);
            this.btnDetailLayanan.Name = "btnDetailLayanan";
            this.btnDetailLayanan.Size = new System.Drawing.Size(176, 51);
            this.btnDetailLayanan.TabIndex = 5;
            this.btnDetailLayanan.Text = "Detail Layanan";
            this.btnDetailLayanan.UseVisualStyleBackColor = false;
            this.btnDetailLayanan.Click += new System.EventHandler(this.btnDetailLayanan_Click);
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.Location = new System.Drawing.Point(249, 405);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(176, 51);
            this.btnReport.TabIndex = 6;
            this.btnReport.Text = "Report Detail Layanan";
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(662, 519);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnDetailLayanan);
            this.Controls.Add(this.btnKelolaKaryawan);
            this.Controls.Add(this.btnPelayanan);
            this.Controls.Add(this.btnBarang);
            this.Controls.Add(this.btnPelanggan);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPelanggan;
        private System.Windows.Forms.Button btnBarang;
        private System.Windows.Forms.Button btnPelayanan;
        private System.Windows.Forms.Button btnKelolaKaryawan;
        private System.Windows.Forms.Button btnDetailLayanan;
        private System.Windows.Forms.Button btnReport;
    }
}

