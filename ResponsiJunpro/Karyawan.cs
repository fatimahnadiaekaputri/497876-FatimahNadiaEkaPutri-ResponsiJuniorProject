using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResponsiJunpro
{
    public class Karyawan : Form1
    {
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=5432;Username=postgres;Password=admin123;Database=karyawan";
        private string sql = null;
        public static NpgsqlCommand cmd;

        public DataTable dt;
        private DataGridViewRow r;

        public Karyawan()
        {
            conn = new NpgsqlConnection(connstring);
            load();
            Form1 form = new Form1();
            
        }

        public void load()
        {
            try
            {
                conn.Open();
                dgv.DataSource = null;
                sql = "select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgv.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public KaryawanModel insert(string name, int id_dep)
        {
            KaryawanModel karyawanModel = new KaryawanModel();
            try
            {
                conn.Open();
                sql = @"Select * from st_insert(:_nama_karyawan, :_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama_karyawan", name);
                cmd.Parameters.AddWithValue("_id_dep", id_dep);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data karyawan berhasil diinputkan", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    tbName.Text = cbDepartemen.Text = null;
                    load();
                    karyawanModel.id_dep = id_dep;
                    karyawanModel.name = name;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return karyawanModel;
        }

        public void update()
        {
            if(r==null)
            {
                MessageBox.Show("Mohon pilih baris data", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_update(:_id_karyawan, :_nama_karyawan, :_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama_karyawan", tbName.Text);
                cmd.Parameters.AddWithValue("_id_dep", cbDepartemen.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data karyawan berhasil diinputkan", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    tbName.Text = cbDepartemen.Text = null;
                    r = null;
                    load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void delete()
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(MessageBox.Show("Apakah benar Anda ingin mengahpus data"+ r.Cells["_nama_karyawan"].Value.ToString()+" ?", "Hapus data terkonfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                try
                {
                    conn.Open();
                    sql = @"select * from st_delete(:_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                    if ((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Data karyawan berhasil dihapus", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                        tbName.Text = cbDepartemen.Text = null;
                        r = null;
                        load();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        
    }
    

}
