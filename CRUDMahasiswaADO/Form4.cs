using System;
using System.Data;
using System.Data.SqlClient;

namespace CRUDMahasiswaADO
{
    public class DAL
    {
        public static string GetLoacalIPAddress()
        {
            string localIP = string.Empty;
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error getting local IP address: " + ex.Message);
            }
            return localIP;
        }

        public static string GetConnectionString()
        {
            string connectionString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBAkademikADO;Integrated Security=True";
            return connectionString;
        }

        SqlConnection conn = new SqlConnection(GetConnectionString());
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public int CountMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam = new SqlParameter("@pCount", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }

        public DataTable GetMhs()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public DataTable GetMhsByNim(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_GetMahasiswaByNIM", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@pNIM", nim);
            da = new SqlDataAdapter(cmd);

            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);

            return dtMahasiswa;
        }

        public void InsertMhs(string nim, string nama, string alamat, string jenisKelamin,
                      DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand("sp_InsertMahasiswa", conn, trans);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@pNIM", nim);
                command.Parameters.AddWithValue("@pNama", nama);
                command.Parameters.AddWithValue("@pAlamat", alamat);
                command.Parameters.AddWithValue("@pTanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("@pJenisKelamin", jenisKelamin);
                command.Parameters.AddWithValue("@pKodeProdi", kodeProdi);
                command.Parameters.AddWithValue("@pFoto", foto ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }

        }
        public void UpdateMhs(string nim, string nama, string alamat, string jenisKelamin,
                      DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", conn);

            command.Parameters.AddWithValue("@pNIM", nim);
            command.Parameters.AddWithValue("@pNama", nama);
            command.Parameters.AddWithValue("@pAlamat", alamat);
            command.Parameters.AddWithValue("@pJenisKelamin", jenisKelamin);
            command.Parameters.AddWithValue("@pTanggalLahir", tanggalLahir);
            command.Parameters.AddWithValue("@pKodeProdi", kodeProdi);
            command.Parameters.AddWithValue("@pFoto", foto ?? (object)DBNull.Value);

            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
        }
        public void DeleteMhs(string nim)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
            cmd.Parameters.AddWithValue("@pNIM", nim);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();
        }
    }

}