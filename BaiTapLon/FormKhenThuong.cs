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

    public partial class FormKhenThuong : Form

    {

        String constr = Connecting.GetConnectionString();

        public FormKhenThuong()

        {

            InitializeComponent();

            LoadKT();

        }

        public void LoadKT()

        {

            try

            {

                using (SqlConnection connection = new SqlConnection(constr))

                {

                    connection.Open();

                    string query = @"SELECT * FROM KhenThuong";



                    using (SqlCommand command = new SqlCommand(query, connection))

                    {

                        // Không cần thêm tham số @MaSV vì không lọc theo sinh viên nữa



                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                        {

                            DataTable dtSV = new DataTable();

                            adapter.Fill(dtSV);

                            dgvKT.DataSource = dtSV;



                            // Đặt lại tên cột hiển thị cho dễ hiểu

                            dgvKT.Columns["MaKT"].HeaderText = "Mã Khen Thưởng";

                            dgvKT.Columns["MaSV"].HeaderText = "Mã SV";

                            dgvKT.Columns["DiemCong"].HeaderText = "Điểm Cộng";

                            dgvKT.Columns["NamHoc"].HeaderText = "Năm Học";

                            dgvKT.Columns["MoTa"].HeaderText = "Mô Tả";



                        }

                    }

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show("Lỗi khi tải dữ liệu khen thưởng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        int rowSelected = -1;

        private int currentYear;



        private void dvgKT_CellClick(object sender, DataGridViewCellEventArgs e)

        {

            if (e.RowIndex >= 0 && dgvKT.CurrentRow != null)

            {

                // Kiểm tra có dữ liệu thực sự hay không

                if (!string.IsNullOrEmpty(dgvKT.CurrentRow.Cells["MaKT"].Value?.ToString()))

                {

                    tbMaKT.Text = dgvKT.CurrentRow.Cells["MaKT"].Value.ToString().Trim();





                    tbMaSV.Text = dgvKT.CurrentRow.Cells["MaSV"].Value.ToString().Trim();







                    tbDiemCong.Text = dgvKT.CurrentRow.Cells["DiemCong"].Value.ToString().Trim();





                    cbbNamHoc.Text = dgvKT.CurrentRow.Cells["NamHoc"].Value.ToString().Trim();

                    rtbMota.Text = dgvKT.CurrentRow.Cells["MoTa"].Value.ToString().Trim();





                    rowSelected = dgvKT.CurrentRow.Index;



                    btnSua.Enabled = true;

                    btnXoa.Enabled = true;

                    btnThem.Enabled = false;

                }

                else

                {

                    RefreshForm(); // Reset các trường nếu là dòng trống

                }

            }
        }



        private void btnLammoi_Click(object sender, EventArgs e)

        {

            RefreshForm();

        }



        private void btnSua_Click(object sender, EventArgs e)

        {

            try

            {

                using (SqlConnection conn = new SqlConnection(constr))

                {

                    conn.Open();

                    string query = @"UPDATE KhenThuong SET MaSV = @MaSV, DiemCong = @DiemCong,

                             NamHoc = @NamHoc, MoTa = @MoTa WHERE MaKT = @MaKT";



                    using (SqlCommand cmd = new SqlCommand(query, conn))

                    {

                        cmd.Parameters.AddWithValue("@MaKT", tbMaKT.Text.Trim());

                        cmd.Parameters.AddWithValue("@MaSV", tbMaSV.Text.Trim());

                        cmd.Parameters.AddWithValue("@DiemCong", tbDiemCong.Text.Trim());

                        cmd.Parameters.AddWithValue("@NamHoc", cbbNamHoc.Text.Trim());

                        cmd.Parameters.AddWithValue("@MoTa", rtbMota.Text.Trim());



                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Sửa thành công!");

                        LoadKT();

                        RefreshForm();

                    }

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show("Lỗi khi sửa dữ liệu: " + ex.Message);

            }

        }



        private void btnThem_Click(object sender, EventArgs e)

        {

            try

            {

                using (SqlConnection conn = new SqlConnection(constr))

                {

                    conn.Open();

                    string query = @"INSERT INTO KhenThuong (MaKT, MaSV, DiemCong, NamHoc, MoTa)

                             VALUES (@MaKT, @MaSV, @DiemCong, @NamHoc, @MoTa)";



                    using (SqlCommand cmd = new SqlCommand(query, conn))

                    {

                        cmd.Parameters.AddWithValue("@MaKT", tbMaKT.Text.Trim());

                        cmd.Parameters.AddWithValue("@MaSV", tbMaSV.Text.Trim());

                        cmd.Parameters.AddWithValue("@DiemCong", tbDiemCong.Text.Trim());

                        cmd.Parameters.AddWithValue("@NamHoc", cbbNamHoc.Text.Trim());

                        cmd.Parameters.AddWithValue("@MoTa", rtbMota.Text.Trim());



                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Thêm thành công!");

                        LoadKT();

                        RefreshForm();

                    }

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);

            }

        }



        private void btnXoa_Click(object sender, EventArgs e)

        {

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

            {

                try

                {

                    using (SqlConnection conn = new SqlConnection(constr))

                    {

                        conn.Open();

                        string query = "DELETE FROM KhenThuong WHERE MaKT = @MaKT";



                        using (SqlCommand cmd = new SqlCommand(query, conn))

                        {

                            cmd.Parameters.AddWithValue("@MaKT", tbMaKT.Text.Trim());

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa thành công!");

                            LoadKT();

                            RefreshForm();

                        }

                    }

                }

                catch (Exception ex)

                {

                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);

                }

            }

        }

        private void RefreshForm()

        {

            tbMaKT.Clear();

            tbMaSV.Clear();

            tbDiemCong.Clear();

            cbbNamHoc.SelectedIndex = -1;

            rtbMota.Clear();



            btnThem.Enabled = true;

            btnSua.Enabled = false;

            btnXoa.Enabled = false;

            rowSelected = -1;

        }



        private void FormKhenThuong_Load(object sender, EventArgs e)

        {



        }

    }

}