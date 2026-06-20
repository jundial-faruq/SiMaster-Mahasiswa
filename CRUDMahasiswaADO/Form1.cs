using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ExcelDataReader;

namespace CRUDMahasiswaADO
{
    public partial class Form1 : Form
    {
        DAL dbLogic = new DAL();

        public Form1()
        {
            InitializeComponent();
        }

        // ── CONNECT ──────────────────────────────────────────────────────
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DAL.GetConnectionString()))  // ← ganti dbLogic jadi DAL
                {
                    conn.Open();
                    MessageBox.Show("Koneksi berhasil");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }

        // ── LOAD DATA ────────────────────────────────────────────────────
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                DataTable dtMahasiswa = dbLogic.GetMhs();

                dataGridView1.DataSource = null;
                bindingSource.DataSource = dtMahasiswa;
                dataGridView1.DataSource = bindingSource;

                if (dataGridView1.Columns.Contains("Foto"))
                {
                    DataGridViewImageColumn fotoColumn = (DataGridViewImageColumn)dataGridView1.Columns["Foto"];
                    fotoColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }

                dataGridView1.Refresh();
                HitungTotal();

                dataGridView1.Enabled = true;
                btnImpDb.Enabled = false;
                btnInsert.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnLoad.Enabled = true;
                btnTestInjection.Enabled = true;
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Gagal Load Data: " + ex.Message);
            }
        }

        // ── HITUNG TOTAL ─────────────────────────────────────────────────
        private void HitungTotal()
        {
            try
            {
                int total = (dbLogic.CountMhs().Equals(DBNull.Value)) ? 0 : dbLogic.CountMhs();
                lblTotal.Text = "Total Mahasiswa: " + total;
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Gagal menghitung total: " + ex.Message);
            }
        }

        private void btnHitungTotal_Click(object sender, EventArgs e)
        {
            HitungTotal();
        }

        // ── INSERT ───────────────────────────────────────────────────────
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] ConvertImageToBytes(PictureBox pb)
                {
                    if (pb.Image == null) return null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }

                byte[] imgBytes = ConvertImageToBytes(pictureBox1);

                dbLogic.InsertMhs(txtNIM.Text, txtNama.Text, txtAlamat.Text,
                                  cmbJK.Text, dtpTanggalLahir.Value.Date,
                                  txtKodeProdi.Text, imgBytes);

                MessageBox.Show("Data berhasil ditambahkan");
                ClearForm();
                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog("ROLLBACK INSERT : " + ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog("GENERAL ERROR : " + ex.Message);
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        // ── UPDATE ───────────────────────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNIM.Text == "")
                {
                    MessageBox.Show("Pilih data yang ingin diupdate!");
                    return;
                }

                byte[] ConvertImageToBytes(PictureBox pb)
                {
                    if (pb.Image == null) return null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }

                byte[] imgBytes = ConvertImageToBytes(pictureBox1);

                dbLogic.UpdateMhs(txtNIM.Text.Trim(), txtNama.Text.Trim(), txtAlamat.Text.Trim(),
                                  cmbJK.Text, dtpTanggalLahir.Value.Date,
                                  txtKodeProdi.Text.Trim(), imgBytes);

                MessageBox.Show("Data berhasil diupdate");
                ClearForm();
                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        // ── DELETE ───────────────────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNIM.Text))
                {
                    MessageBox.Show("Pilih data yang ingin dihapus!");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Yakin ingin menghapus data dengan NIM " + txtNIM.Text + "?",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    dbLogic.DeleteMhs(txtNIM.Text.Trim());
                    MessageBox.Show("Data berhasil dihapus");
                    ClearForm();
                    LoadData();
                }
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        // ── TEST INJECTION ───────────────────────────────────────────────
        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.testInject(txtNIM.Text);
                LoadData();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("safe"))
                {
                    SimpanLog(ex.Message);
                    MessageBox.Show("SQL Error : Unsafe UPDATE operation not allowed", "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    SimpanLog(ex.Message);
                    MessageBox.Show("SQL Error : " + ex.Message, "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show(ex.Message, "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // ── UPLOAD GAMBAR (event sudah ter-wire ke button2_Click di Designer) ─
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // ── IMPORT FROM EXCEL ────────────────────────────────────────────
        private void btnImpExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Excel Workbook| *.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            DataTable dt = result.Tables[0];
                            dataGridView1.DataSource = dt;
                            dataGridView1.Enabled = false;

                            btnImpDb.Enabled = true;
                            btnInsert.Enabled = false;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                            btnLoad.Enabled = false;
                            btnTestInjection.Enabled = false;
                        }
                    }
                }
            }
        }

        // ── IMPORT TO DATABASE ───────────────────────────────────────────
        private void btnImpDb_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diimport.");
                    return;
                }

                int sukses = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string nim = row["NIM"].ToString().Trim();
                    string nama = row["Nama"].ToString().Trim();
                    string jk = row["JenisKelamin"].ToString().Trim();
                    string alamat = row["Alamat"].ToString().Trim();
                    string kodeProdi = row.Table.Columns.Contains("NamaProdi")
                                      ? row["NamaProdi"].ToString().Trim()
                                      : row["KodeProdi"].ToString().Trim();
                    string fotoPath = row.Table.Columns.Contains("FotoPath")
                                      ? row["FotoPath"].ToString().Trim()
                                      : string.Empty;

                    if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama))
                        continue;

                    DateTime tglLahir;
                    if (!DateTime.TryParse(row["TanggalLahir"].ToString(), out tglLahir))
                        continue;

                    byte[] ConvertImageFromPath(string path)
                    {
                        if (string.IsNullOrWhiteSpace(path)) return null;
                        if (!File.Exists(path)) return null;
                        return File.ReadAllBytes(path);
                    }

                    byte[] fotoBytes = ConvertImageFromPath(fotoPath);
                    dbLogic.InsertMhs(nim, nama, alamat, jk, tglLahir, kodeProdi, fotoBytes);
                    sukses++;
                }

                MessageBox.Show("Data mahasiswa berhasil ditambahkan");
                ClearForm();
                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog("Rollback Insert : " + ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog("General Error : " + ex.Message);
                MessageBox.Show("General Error : " + ex.Message);
            }
        }

        // ── REKAP DATA → Buka Form2 ───────────────────────────────────────
        private void btnRekapData_Click(object sender, EventArgs e)
        {
            Form2 fm3 = new Form2();
            fm3.Show();
            this.Hide();
        }

        // ── GRID CLICK → ISI FORM ─────────────────────────────────────────
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtNIM.Text = row.Cells["NIM"].Value?.ToString();
                txtNama.Text = row.Cells["Nama"].Value?.ToString();
                cmbJK.Text = row.Cells["JenisKelamin"].Value?.ToString();
                dtpTanggalLahir.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
                txtAlamat.Text = row.Cells["Alamat"].Value?.ToString();
                txtKodeProdi.Text = row.Cells["KodeProdi"].Value?.ToString();

                if (dataGridView1.Columns.Contains("Foto") &&
                    row.Cells["Foto"].Value != null &&
                    row.Cells["Foto"].Value != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row.Cells["Foto"].Value;
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        // ── CLEAR FORM ───────────────────────────────────────────────────
        private void ClearForm()
        {
            txtNIM.Clear();
            txtNama.Clear();
            cmbJK.SelectedIndex = -1;
            txtAlamat.Clear();
            txtKodeProdi.Clear();
            dtpTanggalLahir.Value = DateTime.Now;
            pictureBox1.Image = null;
            txtNIM.Focus();
        }

        // ── SIMPAN LOG ───────────────────────────────────────────────────
        private void SimpanLog(string pesan)
        {
            dbLogic.InsertLog(pesan);
        }

        // ── FORM LOAD ────────────────────────────────────────────────────
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbJK.Items.Clear();
            cmbJK.Items.Add("L");
            cmbJK.Items.Add("P");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.CellClick += dataGridView1_CellClick;

            LoadData();
            HitungTotal();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {
        }

        private void bindingNavigator1_RefreshItems_1(object sender, EventArgs e)
        {
        }

        private void cmbJK_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}