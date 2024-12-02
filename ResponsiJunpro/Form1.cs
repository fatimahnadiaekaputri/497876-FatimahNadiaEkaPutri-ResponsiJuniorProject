using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResponsiJunpro
{
    public partial class Form1 : Form
    {
        private Karyawan karyawan;

        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=5432;Username=postgres;Password=admin123;Database=karyawan";
        private string sql = null;
        public static NpgsqlCommand cmd;

        public DataTable dt;
        private DataGridViewRow r;
        public Form1()
        {
            InitializeComponent();

        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            LoadDepartements();
            load();
            
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

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >=0)
            {
                r = dgv.Rows[e.RowIndex];
                tbName.Text = r.Cells["_nama_karyawan"].Value.ToString();
                cbDepartemen.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }

        private void LoadDepartements()
        {
            try
            {
                conn.Open();
                sql = "select * from st_select_dep()";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    using (NpgsqlDataReader rd = cmd.ExecuteReader())
                    {
                        cbDepartemen.Items.Clear();
                        while (rd.Read())
                        {
                            cbDepartemen.Items.Add(rd["_id_dep"].ToString());
                        }
                    }
                }
                conn.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string name = tbName.Text;
            int id_dep = int.Parse(cbDepartemen.Text);
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id_dep = int.Parse(cbDepartemen.Text);
            if (r == null)
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
                cmd.Parameters.AddWithValue("_id_dep", id_dep);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data karyawan berhasil diupdate", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Apakah benar Anda ingin mengahpus data" + r.Cells["_nama_karyawan"].Value.ToString() + " ?", "Hapus data terkonfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
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
