namespace Sahabat_Accu_1
{
    partial class Karyawan
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
            this.dgvKaryawan = new System.Windows.Forms.DataGridView();
            this.txtAlamat = new System.Windows.Forms.TextBox();
            this.txtHp = new System.Windows.Forms.TextBox();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.txtIdKaryawan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnInputData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKaryawan)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.Info;
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRefresh.Location = new System.Drawing.Point(564, 412);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 53;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.Info;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUpdate.Location = new System.Drawing.Point(564, 383);
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
            this.btnDelete.Location = new System.Drawing.Point(564, 354);
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
            this.btnTambah.Location = new System.Drawing.Point(564, 325);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 50;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dgvKaryawan
            // 
            this.dgvKaryawan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKaryawan.Location = new System.Drawing.Point(12, 12);
            this.dgvKaryawan.Name = "dgvKaryawan";
            this.dgvKaryawan.RowHeadersWidth = 51;
            this.dgvKaryawan.RowTemplate.Height = 24;
            this.dgvKaryawan.Size = new System.Drawing.Size(776, 286);
            this.dgvKaryawan.TabIndex = 49;
            this.dgvKaryawan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKaryawan_CellContentClick);
            // 
            // txtAlamat
            // 
            this.txtAlamat.BackColor = System.Drawing.SystemColors.Info;
            this.txtAlamat.Location = new System.Drawing.Point(301, 410);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(179, 22);
            this.txtAlamat.TabIndex = 48;
            // 
            // txtHp
            // 
            this.txtHp.BackColor = System.Drawing.SystemColors.Info;
            this.txtHp.Location = new System.Drawing.Point(301, 380);
            this.txtHp.Name = "txtHp";
            this.txtHp.Size = new System.Drawing.Size(179, 22);
            this.txtHp.TabIndex = 47;
            // 
            // txtNama
            // 
            this.txtNama.BackColor = System.Drawing.SystemColors.Info;
            this.txtNama.Location = new System.Drawing.Point(301, 351);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(179, 22);
            this.txtNama.TabIndex = 46;
            // 
            // txtIdKaryawan
            // 
            this.txtIdKaryawan.BackColor = System.Drawing.SystemColors.Info;
            this.txtIdKaryawan.Location = new System.Drawing.Point(301, 322);
            this.txtIdKaryawan.Name = "txtIdKaryawan";
            this.txtIdKaryawan.Size = new System.Drawing.Size(179, 22);
            this.txtIdKaryawan.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(132, 410);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 44;
            this.label4.Text = "Alamat";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 380);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 43;
            this.label3.Text = "No Telp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 351);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "Nama";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 325);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "ID Karyawan";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.IndianRed;
            this.btnBack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBack.Location = new System.Drawing.Point(705, 427);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(74, 23);
            this.btnBack.TabIndex = 58;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnInputData
            // 
            this.btnInputData.Location = new System.Drawing.Point(704, 325);
            this.btnInputData.Name = "btnInputData";
            this.btnInputData.Size = new System.Drawing.Size(75, 23);
            this.btnInputData.TabIndex = 59;
            this.btnInputData.Text = "Input Data";
            this.btnInputData.UseVisualStyleBackColor = true;
            this.btnInputData.Click += new System.EventHandler(this.btnInputData_Click);
            // 
            // Karyawan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 462);
            this.Controls.Add(this.btnInputData);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.dgvKaryawan);
            this.Controls.Add(this.txtAlamat);
            this.Controls.Add(this.txtHp);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.txtIdKaryawan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Karyawan";
            this.Text = "Karyawan";
            this.Load += new System.EventHandler(this.Karyawan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKaryawan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.DataGridView dgvKaryawan;
        private System.Windows.Forms.TextBox txtAlamat;
        private System.Windows.Forms.TextBox txtHp;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.TextBox txtIdKaryawan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnInputData;
    }
}