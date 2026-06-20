using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form3 : Form
    {
        private static string connectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBAkademikADO;Integrated Security=True";

        private SqlConnection conn;

        private string prodi;
        private DateTime tahunMasuk;

        public Form3(string prodi, DateTime tahunMasuk)
        {
            InitializeComponent();

            this.prodi = prodi;
            this.tahunMasuk = tahunMasuk;

            conn = new SqlConnection(connectionString);

            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tahunMasuk.Year.ToString());

                SqlDataReader dr = cmd.ExecuteReader();

                List<Data> listMahasiswa = new List<Data>();

                while (dr.Read())
                {
                    Data data = new Data();

                    data.Nama = dr["Nama"].ToString();
                    data.JenisKelamin = dr["JenisKelamin"].ToString();
                    data.Alamat = dr["Alamat"].ToString();
                    data.NamaProdi = dr["NamaProdi"].ToString();
                    data.TanggalDaftar = Convert.ToDateTime(dr["TanggalDaftar"]);

                    listMahasiswa.Add(data);
                }

                dr.Close();

                // CEK JUMLAH DATA
                MessageBox.Show(
                    "Jumlah Data = " + listMahasiswa.Count,
                    "Debug Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ListMahasiswa rpt = new ListMahasiswa();

                rpt.SetDataSource(listMahasiswa);

                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}