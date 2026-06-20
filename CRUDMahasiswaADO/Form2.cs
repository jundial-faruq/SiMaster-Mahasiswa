using System;
using System.Data;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form2 : Form
    {
        DAL dbLogic = new DAL();
        private DataTable dtMahasiswa;
        private DataTable dtProdi;

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
            btnLoad.Click += btnLoad_Click;
            btnCetak.Click += btnCetak_Click;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dtpTahunMasuk.Format = DateTimePickerFormat.Custom;
            dtpTahunMasuk.CustomFormat = "yyyy";
            dtpTahunMasuk.ShowUpDown = true;
            btnCetak.Enabled = false;
            LoadProdi();
        }

        private void LoadProdi()
        {
            try
            {
                dtProdi = dbLogic.getProdi();
                cboProdi.DataSource = dtProdi;
                cboProdi.DisplayMember = "NamaProdi";
                cboProdi.ValueMember = "NamaProdi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                dtMahasiswa = dbLogic.getDataRekap(
                    cboProdi.SelectedValue.ToString(),
                    dtpTahunMasuk.Value);

                dgvMahasiswa.DataSource = dtMahasiswa;
                btnCetak.Enabled = dtMahasiswa.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            if (dtMahasiswa == null || dtMahasiswa.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data yang akan dicetak.");
                return;
            }

            Form3 frm = new Form3(
                cboProdi.SelectedValue.ToString(),
                dtpTahunMasuk.Value);
            frm.Show();
            this.Hide();
        }
    }
}