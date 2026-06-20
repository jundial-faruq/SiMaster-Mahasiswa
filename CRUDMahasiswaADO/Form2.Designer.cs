namespace CRUDMahasiswaADO
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lbljudul = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboProdi = new System.Windows.Forms.ComboBox();
            this.dtpTahunMasuk = new System.Windows.Forms.DateTimePicker();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCetak = new System.Windows.Forms.Button();
            this.dgvMahasiswa = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMahasiswa)).BeginInit();
            this.SuspendLayout();

            // lbljudul
            this.lbljudul.AutoSize = true;
            this.lbljudul.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                7.8F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));

            this.lbljudul.Location = new System.Drawing.Point(295, 9);
            this.lbljudul.Name = "lbljudul";
            this.lbljudul.Size = new System.Drawing.Size(197, 16);
            this.lbljudul.TabIndex = 0;
            this.lbljudul.Text = "REKAP DATA MAHASISWA";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Prodi :";

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tahun Masuk :";

            // cboProdi
            this.cboProdi.FormattingEnabled = true;
            this.cboProdi.Location = new System.Drawing.Point(177, 42);
            this.cboProdi.Name = "cboProdi";
            this.cboProdi.Size = new System.Drawing.Size(106, 24);
            this.cboProdi.TabIndex = 3;

            // dtpTahunMasuk
            this.dtpTahunMasuk.Location = new System.Drawing.Point(395, 42);
            this.dtpTahunMasuk.Name = "dtpTahunMasuk";
            this.dtpTahunMasuk.Size = new System.Drawing.Size(200, 22);
            this.dtpTahunMasuk.TabIndex = 4;

            // btnLoad
            this.btnLoad.Location = new System.Drawing.Point(610, 42);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            // btnCetak
            this.btnCetak.Location = new System.Drawing.Point(621, 376);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 6;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.UseVisualStyleBackColor = true;
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);

            // dgvMahasiswa
            this.dgvMahasiswa.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMahasiswa.Location = new System.Drawing.Point(149, 100);
            this.dgvMahasiswa.Name = "dgvMahasiswa";
            this.dgvMahasiswa.RowHeadersWidth = 51;
            this.dgvMahasiswa.RowTemplate.Height = 24;
            this.dgvMahasiswa.Size = new System.Drawing.Size(547, 250);
            this.dgvMahasiswa.TabIndex = 7;

            // Form2
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);

            this.Controls.Add(this.dgvMahasiswa);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.dtpTahunMasuk);
            this.Controls.Add(this.cboProdi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbljudul);

            this.Name = "Form2";
            this.Text = "Rekap Data Mahasiswa";
            this.Load += new System.EventHandler(this.Form2_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvMahasiswa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbljudul;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboProdi;
        private System.Windows.Forms.DateTimePicker dtpTahunMasuk;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCetak;
        private System.Windows.Forms.DataGridView dgvMahasiswa;
    }
}