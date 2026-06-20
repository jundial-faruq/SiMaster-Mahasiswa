using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace CRUDMahasiswaADO
{
    public partial class Form1 : Form
    {
        
        private readonly SqlConnection conn;
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBAkademikADO;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }
        private void ConnectDatabase()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open(); 
                }
                MessageBox.Show("Koneksi berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData(); 
        
        }
        private void BindControls()
        {

        }
        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dtMahasiswa = new DataTable();
                            da.Fill(dtMahasiswa);

                            dataGridView1.DataSource = null;

                            bindingSource.DataSource = dtMahasiswa;

                            dataGridView1.DataSource = bindingSource;

                            // TAMBAHKAN DI SINI
                            dataGridView1.Refresh();

                            BindControls();
                        }
                    }
                }

                HitungTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load Data: " + ex.Message);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_InsertMahasiswa",
                    conn,
                    trans);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@JenisKelamin", cmbJK.Text);
                cmd.Parameters.AddWithValue("@TanggalLahir", dtpTanggalLahir.Value.Date);
                cmd.Parameters.AddWithValue("@Alamat", txtAlamat.Text);
                cmd.Parameters.AddWithValue("@KodeProdi", txtKodeProdi.Text);
                cmd.Parameters.AddWithValue("@TanggalDaftar", DateTime.Now);

                cmd.ExecuteNonQuery();

                SqlCommand cmdLog = new SqlCommand(
                    @"INSERT INTO LogAktivitasSalah (aktivitas, waktu) 
            VALUES (@aktivitas, GETDATE())",
                    conn,
                    trans);

                cmdLog.Parameters.AddWithValue("@aktivitas", "INSERT MAHASISWA : " + txtNIM.Text);

                cmdLog.ExecuteNonQuery();

                trans.Commit();

                MessageBox.Show("Data berhasil ditambahkan");

                LoadData();
            }
            catch (SqlException ex)
            {
                trans.Rollback();

                SimpanLog("ROLLBACK INSERT : " + ex.Message);

                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                trans.Rollback();

                SimpanLog("GENERAL ERROR : " + ex.Message);

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void SimpanLog(string pesan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO LogError (waktu, pesan_error) 
                        VALUES (GETDATE(), @pesan)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pesan", pesan);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Perhatikan letak petik tunggal (') sebelum dan sesudah textbox
                    string query = "UPDATE Mahasiswa SET Nama='" + txtNama.Text + "' WHERE NIM='" + txtNIM.Text + "'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Update berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Pesan RAISERROR dari trigger akan ditangkap di sini
                MessageBox.Show(ex.Message, "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnRekapData_Click(object sender, EventArgs e)
        {
            Form2 fm3 = new Form2();
            fm3.Show();
            this.Hide();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // VALIDASI
                if (txtNIM.Text == "")
                {
                    MessageBox.Show("Pilih data yang ingin diupdate!");
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = new SqlCommand("sp_UpdateMahasiswa", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NIM", txtNIM.Text.Trim());
                    cmd.Parameters.AddWithValue("@Nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@JenisKelamin", cmbJK.Text);
                    cmd.Parameters.AddWithValue("@TanggalLahir", dtpTanggalLahir.Value.Date);
                    cmd.Parameters.AddWithValue("@Alamat", txtAlamat.Text.Trim());
                    cmd.Parameters.AddWithValue("@KodeProdi", txtKodeProdi.Text.Trim());

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil diupdate");

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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // VALIDASI
                if (string.IsNullOrEmpty(txtNIM.Text))
                {
                    MessageBox.Show("Pilih data yang ingin dihapus!");
                    return;
                }

                // KONFIRMASI
                DialogResult confirm = MessageBox.Show(
                    "Yakin ingin menghapus data dengan NIM " + txtNIM.Text + "?",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@NIM", SqlDbType.VarChar, 11).Value =
                            txtNIM.Text.Trim();

                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil dihapus");

                        ClearForm();
                        LoadData();
                    }
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtNIM.Text = row.Cells["NIM"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                cmbJK.Text = row.Cells["JenisKelamin"].Value.ToString();
                dtpTanggalLahir.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
                txtAlamat.Text = row.Cells["Alamat"].Value.ToString();
                txtKodeProdi.Text = row.Cells["KodeProdi"].Value.ToString();
            }
        }
        private void ClearForm()
        {
            txtNIM.Clear();
            txtNama.Clear();
            cmbJK.SelectedIndex = -1;
            txtAlamat.Clear();
            txtKodeProdi.Clear();
            dtpTanggalLahir.Value = DateTime.Now;
            txtNIM.Focus();
        }

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

            // TAMBAHKAN INI
            LoadData();
            HitungTotal();
        }

        private void HitungTotal()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Menyiapkan parameter OUTPUT
                        SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Menampilkan hasil ke Label (asumsi nama label adalah lblTotal)
                        lblTotal.Text = "Total Mahasiswa: " + outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghitung total: " + ex.Message);
            }
        }

        private void btnHitungTotal_Click(object sender, EventArgs e)
        {
            // Panggil fungsi HitungTotal yang sudah kita buat sebelumnya
            HitungTotal();
        }


        private void label1_Click(object sender, EventArgs e)
        {
            // Bisa dikosongkan jika tidak digunakan
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