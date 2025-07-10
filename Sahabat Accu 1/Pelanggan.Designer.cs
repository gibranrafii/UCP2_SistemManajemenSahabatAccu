namespace Sahabat_Accu_1
{
    partial class Pelanggan
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.dgvPelanggan = new System.Windows.Forms.DataGridView();
            this.txtAlamat = new System.Windows.Forms.TextBox();
            this.txtHp = new System.Windows.Forms.TextBox();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.BtnAnalyze = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCari = new System.Windows.Forms.TextBox();
            this.btnCari = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPelanggan)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.Info;
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRefresh.Location = new System.Drawing.Point(572, 443);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 25;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.Info;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUpdate.Location = new System.Drawing.Point(572, 414);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 24;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Info;
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDelete.Location = new System.Drawing.Point(572, 385);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.BackColor = System.Drawing.SystemColors.Info;
            this.btnTambah.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTambah.Location = new System.Drawing.Point(572, 356);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 22;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dgvPelanggan
            // 
            this.dgvPelanggan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPelanggan.Location = new System.Drawing.Point(12, 12);
            this.dgvPelanggan.Name = "dgvPelanggan";
            this.dgvPelanggan.RowHeadersWidth = 51;
            this.dgvPelanggan.RowTemplate.Height = 24;
            this.dgvPelanggan.Size = new System.Drawing.Size(776, 321);
            this.dgvPelanggan.TabIndex = 21;
            this.dgvPelanggan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPelanggan_CellContentClick);
            // 
            // txtAlamat
            // 
            this.txtAlamat.BackColor = System.Drawing.SystemColors.Info;
            this.txtAlamat.Location = new System.Drawing.Point(309, 441);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(179, 22);
            this.txtAlamat.TabIndex = 20;
            // 
            // txtHp
            // 
            this.txtHp.BackColor = System.Drawing.SystemColors.Info;
            this.txtHp.Location = new System.Drawing.Point(309, 411);
            this.txtHp.Name = "txtHp";
            this.txtHp.Size = new System.Drawing.Size(179, 22);
            this.txtHp.TabIndex = 19;
            // 
            // txtNama
            // 
            this.txtNama.BackColor = System.Drawing.SystemColors.Info;
            this.txtNama.Location = new System.Drawing.Point(309, 382);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(179, 22);
            this.txtNama.TabIndex = 18;
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.SystemColors.Info;
            this.txtId.Location = new System.Drawing.Point(309, 353);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(179, 22);
            this.txtId.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 441);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Alamat";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 411);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "No Telp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 382);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Nama";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(140, 356);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "ID pelanggan";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.IndianRed;
            this.btnBack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBack.Location = new System.Drawing.Point(692, 511);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(74, 23);
            this.btnBack.TabIndex = 26;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnAnalyze
            // 
            this.BtnAnalyze.BackColor = System.Drawing.SystemColors.Info;
            this.BtnAnalyze.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnAnalyze.Location = new System.Drawing.Point(691, 356);
            this.BtnAnalyze.Name = "BtnAnalyze";
            this.BtnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.BtnAnalyze.TabIndex = 27;
            this.BtnAnalyze.Text = "Analisis";
            this.BtnAnalyze.UseVisualStyleBackColor = false;
            this.BtnAnalyze.Click += new System.EventHandler(this.BtnAnalyze_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 491);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(63, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "Cari Data";
            // 
            // txtCari
            // 
            this.txtCari.BackColor = System.Drawing.SystemColors.Info;
            this.txtCari.Location = new System.Drawing.Point(309, 488);
            this.txtCari.Name = "txtCari";
            this.txtCari.Size = new System.Drawing.Size(179, 22);
            this.txtCari.TabIndex = 29;
            // 
            // btnCari
            // 
            this.btnCari.BackColor = System.Drawing.SystemColors.Info;
            this.btnCari.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCari.Location = new System.Drawing.Point(572, 491);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(75, 23);
            this.btnCari.TabIndex = 30;
            this.btnCari.Text = "Cari";
            this.btnCari.UseVisualStyleBackColor = false;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // Pelanggan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(800, 546);
            this.Controls.Add(this.btnCari);
            this.Controls.Add(this.txtCari);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnAnalyze);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.dgvPelanggan);
            this.Controls.Add(this.txtAlamat);
            this.Controls.Add(this.txtHp);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Pelanggan";
            this.Text = "Pelanggan";
            this.Load += new System.EventHandler(this.Pelanggan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPelanggan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.DataGridView dgvPelanggan;
        private System.Windows.Forms.TextBox txtAlamat;
        private System.Windows.Forms.TextBox txtHp;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button BtnAnalyze;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCari;
        private System.Windows.Forms.Button btnCari;
    }
}