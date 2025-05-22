using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormCN : Form
    {
        string constr = Connecting.GetConnectionString();
        string MaGV;
        public FormCN(String maGV)
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

                        using (SqlCommand command = new SqlCommand("sp_XemLopChuNhiem", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@MaCN", MaGV);

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

      private void loadDiemNamHoc(string maLop, string Hocky, string namHoc)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_XemDiemLopTheoHocKy", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLop", maLop);
                cmd.Parameters.AddWithValue("@NamHoc", namHoc);
                cmd.Parameters.AddWithValue("@HocKy", Hocky);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridGV.DataSource = dt; // Hiển thị vào DataGridView
            }
        }

        private void FormCN_Load(object sender, EventArgs e)
        {
          
        }

        private void dvgCN_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maLop = dataGridGV.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
                loadDiemNamHoc(maLop, comboHK.Text, comboNH.Text);

            }
        }
    }
}
