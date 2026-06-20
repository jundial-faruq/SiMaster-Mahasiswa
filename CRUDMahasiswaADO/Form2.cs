using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form2 : Form
    {
        private static string connectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBAkademikADO;Integrated Security=True";

        private SqlConnection conn;
        private SqlDataAdapter da;
        private DataTable dtMahasiswa;
        private DataTable dtProdi;

        public Form2()
        {
            InitializeComponent();

            conn = new SqlConnection(connectionString);

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
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand("SELECT NamaProdi FROM ProgramStudi", conn);

                dtProdi = new DataTable();

                da = new SqlDataAdapter(cmd);
                da.Fill(dtProdi);

                cboProdi.DataSource = dtProdi;
                cboProdi.DisplayMember = "NamaProdi";
                cboProdi.ValueMember = "NamaProdi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand("sp_Report", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@inProdi",
                    cboProdi.SelectedValue.ToString());

                cmd.Parameters.AddWithValue(
                    "@inTglMsuk",
                    dtpTahunMasuk.Value.Year.ToString());

                dtMahasiswa = new DataTable();

                da = new SqlDataAdapter(cmd);
                da.Fill(dtMahasiswa);

                dgvMahasiswa.DataSource = dtMahasiswa;

                btnCetak.Enabled = dtMahasiswa.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
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