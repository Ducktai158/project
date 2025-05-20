using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class Form1GV : Form
    {
        string constr = Connecting.GetConnectionString();
        string MaGV;
        public Form1GV(string maGV)
        {
            InitializeComponent();
            this.MaGV = maGV;
            LoadLopGV();
        }
        private void LoadLopGV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetLopGiangVien", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaGV", MaGV);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtDiem = new DataTable();
                            adapter.Fill(dtDiem);
                            dataGridGV.DataSource = dtDiem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1GV_Load(object sender, EventArgs e)
        {

        }

        private void dgvLop_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maLop = dataGridGV.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
                 LoadSinhVienVaDiem(maLop);

            }
        }
        private void LoadSinhVienVaDiem(string maLop)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("LoadSinhVienVaDiemTheoLop", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLop", maLop);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridGV.DataSource = dt; // Hiển thị ra DataGridView khác
            }
        }

       

        private void QuayLai_Click(object sender, EventArgs e)
        {
            LoadLopGV();
        }
    }
}
