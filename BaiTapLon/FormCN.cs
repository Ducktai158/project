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
        private int currentYear;

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
        private void LoadDNH(string maLop) // Thêm tham số maLop
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_LayKetQuaNamHocTheoLop", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaLop", maLop); // Sử dụng tham số maLop

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
            // Thêm 5 năm học tính từ năm hiện tại trở về trước
            for (int i = 0; i < 5; i++)
            {
                int startYear = currentYear - i - 1;
                int endYear = currentYear - i;
                comboNH.Items.Add($"{startYear}-{endYear}");
            }

            comboNH.SelectedIndex = 0;
        }

        private void dvgCN_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void lammoiClick(object sender, EventArgs e)
        {
            comboHK.SelectedIndex = -1;
            comboNH.SelectedIndex = -1;
            LoadLopGV();
            btnXemdiemHK.Enabled = true;
            btnXDTK.Enabled = true;
        }

        private void điểmNămHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnXemDiem_Click(object sender, EventArgs e)
        {
            btnXemdiemHK.Enabled = false;
            btnXDTK.Enabled = false;
            if (dataGridGV.CurrentRow != null)
            {
                string maLop = dataGridGV.CurrentRow.Cells["MaLop"].Value.ToString();
                LoadDNH(maLop); // Gọi hàm LoadDNH để hiển thị kết quả năm học tổng quát
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lớp để xem kết quả năm học tổng quát.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void BtnXemDiemHk_Click(object sender, EventArgs e)
        {
           
            if (dataGridGV.CurrentRow != null)
            {
                // Lấy mã lớp từ dòng được chọn
                string maLop = dataGridGV.CurrentRow.Cells["MaLop"].Value.ToString();

                // Kiểm tra học kỳ và năm học có bị để trống không
                if (string.IsNullOrWhiteSpace(comboHK.Text) || string.IsNullOrWhiteSpace(comboNH.Text))
                {
                    MessageBox.Show("Không được để trống Học kỳ và Năm học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Nếu hợp lệ thì gọi hàm load điểm
                loadDiemNamHoc(maLop, comboHK.Text, comboNH.Text);
            }
           
        }
    }

}