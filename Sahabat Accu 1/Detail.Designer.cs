namespace Sahabat_Accu_1
{
    partial class Detail
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
            this.txtSubtotal = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.comboBoxIdPelayanan = new System.Windows.Forms.ComboBox();
            this.comboBoxIdBarang = new System.Windows.Forms.ComboBox();
            this.comboBoxIdKaryawan = new System.Windows.Forms.ComboBox();
            this.txtIdDetail = new System.Windows.Forms.TextBox();
            this.txtJumlah = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSubtotal
            // 
            this.txtSubtotal.BackColor = System.Drawing.SystemColors.Info;
            this.txtSubtotal.Location = new System.Drawing.Point(317, 474);
            this.txtSubtotal.Name = "txtSubtotal";
            this.txtSubtotal.Size = new System.Drawing.Size(179, 22);
            this.txtSubtotal.TabIndex = 74;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(95, 477);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 73;
            this.label6.Text = "Subtotal";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.Info;
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRefresh.Location = new System.Drawing.Point(600, 415);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 72;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.Info;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUpdate.Location = new System.Drawing.Point(600, 386);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 69;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Info;
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(600, 357);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 68;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.BackColor = System.Drawing.SystemColors.Info;
            this.btnTambah.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTambah.Location = new System.Drawing.Point(600, 328);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 67;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(759, 264);
            this.dataGridView1.TabIndex = 66;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 394);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(82, 16);
            this.label4.TabIndex = 61;
            this.label4.Text = "ID Karyawan";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 364);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(67, 16);
            this.label3.TabIndex = 60;
            this.label3.Text = "ID Barang";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 335);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 59;
            this.label2.Text = "ID Pelayanan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 309);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 58;
            this.label1.Text = "ID Detail";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.IndianRed;
            this.btnBack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBack.Location = new System.Drawing.Point(697, 451);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(74, 23);
            this.btnBack.TabIndex = 75;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // comboBoxIdPelayanan
            // 
            this.comboBoxIdPelayanan.FormattingEnabled = true;
            this.comboBoxIdPelayanan.Location = new System.Drawing.Point(317, 331);
            this.comboBoxIdPelayanan.Name = "comboBoxIdPelayanan";
            this.comboBoxIdPelayanan.Size = new System.Drawing.Size(179, 24);
            this.comboBoxIdPelayanan.TabIndex = 77;
            // 
            // comboBoxIdBarang
            // 
            this.comboBoxIdBarang.FormattingEnabled = true;
            this.comboBoxIdBarang.Location = new System.Drawing.Point(317, 361);
            this.comboBoxIdBarang.Name = "comboBoxIdBarang";
            this.comboBoxIdBarang.Size = new System.Drawing.Size(179, 24);
            this.comboBoxIdBarang.TabIndex = 78;
            // 
            // comboBoxIdKaryawan
            // 
            this.comboBoxIdKaryawan.FormattingEnabled = true;
            this.comboBoxIdKaryawan.Location = new System.Drawing.Point(317, 391);
            this.comboBoxIdKaryawan.Name = "comboBoxIdKaryawan";
            this.comboBoxIdKaryawan.Size = new System.Drawing.Size(179, 24);
            this.comboBoxIdKaryawan.TabIndex = 79;
            // 
            // txtIdDetail
            // 
            this.txtIdDetail.BackColor = System.Drawing.SystemColors.Info;
            this.txtIdDetail.Location = new System.Drawing.Point(317, 303);
            this.txtIdDetail.Name = "txtIdDetail";
            this.txtIdDetail.Size = new System.Drawing.Size(179, 22);
            this.txtIdDetail.TabIndex = 80;
            // 
            // txtJumlah
            // 
            this.txtJumlah.BackColor = System.Drawing.SystemColors.Info;
            this.txtJumlah.Location = new System.Drawing.Point(317, 445);
            this.txtJumlah.Name = "txtJumlah";
            this.txtJumlah.Size = new System.Drawing.Size(179, 22);
            this.txtJumlah.TabIndex = 81;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(95, 451);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 82;
            this.label5.Text = "Jumlah Aki";
            // 
            // Detail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 543);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtJumlah);
            this.Controls.Add(this.txtIdDetail);
            this.Controls.Add(this.comboBoxIdKaryawan);
            this.Controls.Add(this.comboBoxIdBarang);
            this.Controls.Add(this.comboBoxIdPelayanan);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.txtSubtotal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Detail";
            this.Text = "Detail";
            this.Load += new System.EventHandler(this.Detail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSubtotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ComboBox comboBoxIdPelayanan;
        private System.Windows.Forms.ComboBox comboBoxIdBarang;
        private System.Windows.Forms.ComboBox comboBoxIdKaryawan;
        private System.Windows.Forms.TextBox txtIdDetail;
        private System.Windows.Forms.TextBox txtJumlah;
        private System.Windows.Forms.Label label5;
    }
}