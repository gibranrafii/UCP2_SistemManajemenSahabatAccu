namespace Sahabat_Accu_1
{
    partial class Pelayanan
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtHarga = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.dgvPelayanan = new System.Windows.Forms.DataGridView();
            this.txtIdPelayanan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboPelayanan = new System.Windows.Forms.ComboBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBoxIdPelanggan = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPotongan = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPelayanan)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.Info;
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRefresh.Location = new System.Drawing.Point(677, 413);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 55;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtHarga
            // 
            this.txtHarga.BackColor = System.Drawing.SystemColors.Info;
            this.txtHarga.Location = new System.Drawing.Point(430, 438);
            this.txtHarga.Name = "txtHarga";
            this.txtHarga.Size = new System.Drawing.Size(200, 22);
            this.txtHarga.TabIndex = 54;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 438);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(45, 16);
            this.label5.TabIndex = 53;
            this.label5.Text = "Harga";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.Info;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUpdate.Location = new System.Drawing.Point(677, 384);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 52;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Info;
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(677, 355);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 51;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.BackColor = System.Drawing.SystemColors.Info;
            this.btnTambah.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTambah.Location = new System.Drawing.Point(677, 326);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 50;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dgvPelayanan
            // 
            this.dgvPelayanan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPelayanan.Location = new System.Drawing.Point(12, 12);
            this.dgvPelayanan.Name = "dgvPelayanan";
            this.dgvPelayanan.RowHeadersWidth = 51;
            this.dgvPelayanan.RowTemplate.Height = 24;
            this.dgvPelayanan.Size = new System.Drawing.Size(776, 286);
            this.dgvPelayanan.TabIndex = 49;
            this.dgvPelayanan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPelayanan_CellContentClick);
            // 
            // txtIdPelayanan
            // 
            this.txtIdPelayanan.BackColor = System.Drawing.SystemColors.Info;
            this.txtIdPelayanan.Location = new System.Drawing.Point(430, 316);
            this.txtIdPelayanan.Name = "txtIdPelayanan";
            this.txtIdPelayanan.Size = new System.Drawing.Size(200, 22);
            this.txtIdPelayanan.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 404);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(368, 16);
            this.label4.TabIndex = 44;
            this.label4.Text = "Jenis Pelayanan (\'Charge Aki\', \'Tuker Tambah\', \'Pembelian\')";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 374);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(223, 16);
            this.label3.TabIndex = 43;
            this.label3.Text = "Tanggal Pelayanan (YYYY-MM-DD)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 345);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "ID Pelanggan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 319);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "ID Pelayanan [PY]";
            // 
            // comboPelayanan
            // 
            this.comboPelayanan.BackColor = System.Drawing.SystemColors.Info;
            this.comboPelayanan.FormattingEnabled = true;
            this.comboPelayanan.Location = new System.Drawing.Point(430, 404);
            this.comboPelayanan.Name = "comboPelayanan";
            this.comboPelayanan.Size = new System.Drawing.Size(200, 24);
            this.comboPelayanan.TabIndex = 56;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.IndianRed;
            this.btnBack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBack.Location = new System.Drawing.Point(702, 450);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(74, 23);
            this.btnBack.TabIndex = 57;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(430, 376);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker1.TabIndex = 58;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // comboBoxIdPelanggan
            // 
            this.comboBoxIdPelanggan.FormattingEnabled = true;
            this.comboBoxIdPelanggan.Location = new System.Drawing.Point(430, 346);
            this.comboBoxIdPelanggan.Name = "comboBoxIdPelanggan";
            this.comboBoxIdPelanggan.Size = new System.Drawing.Size(200, 24);
            this.comboBoxIdPelanggan.TabIndex = 59;
            this.comboBoxIdPelanggan.SelectedIndexChanged += new System.EventHandler(this.comboBoxIdPelanggan_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 468);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(106, 16);
            this.label6.TabIndex = 60;
            this.label6.Text = "Potongan Harga";
            // 
            // txtPotongan
            // 
            this.txtPotongan.BackColor = System.Drawing.SystemColors.Info;
            this.txtPotongan.Location = new System.Drawing.Point(430, 468);
            this.txtPotongan.Name = "txtPotongan";
            this.txtPotongan.Size = new System.Drawing.Size(200, 22);
            this.txtPotongan.TabIndex = 61;
            // 
            // Pelayanan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 528);
            this.Controls.Add(this.txtPotongan);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxIdPelanggan);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.comboPelayanan);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtHarga);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.dgvPelayanan);
            this.Controls.Add(this.txtIdPelayanan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Pelayanan";
            this.Text = "Pelayanan";
            this.Load += new System.EventHandler(this.Pelayanan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPelayanan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtHarga;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.DataGridView dgvPelayanan;
        private System.Windows.Forms.TextBox txtIdPelayanan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboPelayanan;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBoxIdPelanggan;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPotongan;
    }
}