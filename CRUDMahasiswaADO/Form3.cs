using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form3 : Form
    {
        DAL dbLogic = new DAL();
        private string prodi;
        private DateTime tahunMasuk;

        public Form3(string prodi, DateTime tahunMasuk)
        {
            InitializeComponent();
            this.prodi = prodi;
            this.tahunMasuk = tahunMasuk;
            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tahunMasuk);

                List<Data> listMahasiswa = new List<Data>();
                foreach (DataRow row in dtMahasiswa.Rows)
                {
                    Data data = new Data();
                    data.Nama = row["Nama"].ToString();
                    data.JenisKelamin = row["JenisKelamin"].ToString();
                    data.Alamat = row["Alamat"].ToString();
                    data.NamaProdi = row["NamaProdi"].ToString();
                    data.TanggalDaftar = Convert.ToDateTime(row["TanggalDaftar"]);
                    listMahasiswa.Add(data);
                }

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
        }
    }
}