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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace BaiTapLon
{
    public partial class FormGV : Form
    {
        String constr = Connecting.GetConnectionString();
        public FormGV()
        {
            InitializeComponent();
            LoadGV();
            LoadComBoBox();
        }
        public void LoadGV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string query = @"SELECT * FROM GiangVien";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Không cần thêm tham số @MaSV vì không lọc theo sinh viên nữa

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtGV = new DataTable();
                            adapter.Fill(dtGV);
                            dgvGV.DataSource = dtGV;

                            // Đặt lại tên cột hiển thị cho dễ hiểu
                            dgvGV.Columns["MaGV"].HeaderText = "Mã GV";
                            dgvGV.Columns["HoTen"].HeaderText = "Họ Tên GV";
                            dgvGV.Columns["NgSinh"].HeaderText = "Ngày Sinh";
                            dgvGV.Columns["GT"].HeaderText = "Giới Tính";
                            dgvGV.Columns["MaKhoa"].HeaderText = "Mã Khoa";
                            dgvGV.Columns["SDT"].HeaderText = "Số điện thoai";
                            dgvGV.Columns["HocVi"].HeaderText = "Học vị";
                            dgvGV.Columns["HocHam"].HeaderText = "Học hàm";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu giảng viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void label17_Click(object sender, EventArgs e)
        {
        }

        private void FormGV_Load(object sender, EventArgs e)
        {

        }
        int rowSelected = -1;
        private void dvgGV_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvGV.CurrentRow != null)
            {
                // Kiểm tra có dữ liệu thực sự hay không
                if (!string.IsNullOrEmpty(dgvGV.CurrentRow.Cells["MaGV"].Value?.ToString()))
                {
                    tbMaGV.Text = dgvGV.CurrentRow.Cells["MaGV"].Value.ToString().Trim();


                    cbbMaKhoa.Text = dgvGV.CurrentRow.Cells["MaKhoa"].Value.ToString().Trim();



                    tbHoTen.Text = dgvGV.CurrentRow.Cells["HoTen"].Value.ToString().Trim();


                    if (DateTime.TryParse(dgvGV.CurrentRow.Cells["NgSinh"].Value?.ToString(), out DateTime ngSinh))
                    {
                        tbNgaysinh.Text = ngSinh.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        tbNgaysinh.Text = ""; // hoặc gán giá trị mặc định nếu parse lỗi
                    }


                    if (dgvGV.CurrentRow.Cells["GT"].Value.ToString().Trim().Equals("nam", StringComparison.InvariantCultureIgnoreCase))
                        rdoNam.Checked = true;
                    else rdoNu.Checked = true;
                    //  tbDiemCC.Text = dgvDiemSinhVien.CurrentRow.Cells["ChuyenCan"].Value.ToString().Trim();
                    cbbHV.Text = dgvGV.CurrentRow.Cells["HocVi"].Value.ToString().Trim();
                    cbbHH.Text = dgvGV.CurrentRow.Cells["HocHam"].Value.ToString().Trim();
                    tbSoDt.Text = dgvGV.CurrentRow.Cells["SDT"].Value.ToString().Trim();



                    rowSelected = dgvGV.CurrentRow.Index;

                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnThem.Enabled = false;
                }
                else
                {
                    refresh(); // Reset các trường nếu là dòng trống
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string query = @"INSERT INTO GiangVien (MaGV, HoTen, NgSinh, GT, MaKhoa, SDT, HocVi, HocHam)
                             VALUES (@MaGV, @HoTen, @NgSinh, @GT, @MaKhoa, @SDT, @HocVi, @HocHam)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaGV", tbMaGV.Text.Trim());
                        command.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                        command.Parameters.AddWithValue("@NgSinh", DateTime.ParseExact(tbNgaysinh.Text.Trim(), "dd/MM/yyyy", null));
                        command.Parameters.AddWithValue("@GT", rdoNam.Checked ? "Nam" : "Nu");
                        command.Parameters.AddWithValue("@MaKhoa", cbbMaKhoa.Text.Trim());
                        command.Parameters.AddWithValue("@SDT", tbSoDt.Text.Trim());
                        command.Parameters.AddWithValue("@HocVi", cbbHV.Text.Trim());
                        command.Parameters.AddWithValue("@HocHam", cbbHH.Text.Trim());

                        command.ExecuteNonQuery();
                        MessageBox.Show("Thêm giảng viên thành công!");
                        LoadGV();
                        refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm giảng viên: " + ex.Message);
            }
        }
        private void refresh()
        {
            tbMaGV.Clear();
            tbHoTen.Clear();
            tbNgaysinh.Clear();
            tbSoDt.Clear();
            cbbHV.SelectedIndex = -1;
            cbbHH.SelectedIndex = -1;
            cbbMaKhoa.SelectedIndex = -1;
            rdoNam.Checked = true;
            rowSelected = -1;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        private void LoadComBoBox()
        {
            cbbHH.Items.Add("");
            cbbHH.Items.Add("GV");
            cbbHH.Items.Add("GVCH");
            cbbHH.Items.Add("PGS");
            cbbHH.Items.Add("GS");

            cbbHV.Items.Add("CN");
            cbbHV.Items.Add("ThS");
            cbbHV.Items.Add("TS");
            cbbHV.Items.Add("TSKH");


            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                string querySV = "SELECT MaGV FROM GiangVien";
                using (SqlCommand cmd = new SqlCommand(querySV, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtSV = new DataTable();
                    dtSV.Load(reader);
                    ccbMGV.DataSource = dtSV;
                    ccbMGV.DisplayMember = ccbMGV.ValueMember = "MaGV";  // Hiển thị tên sinh viên
                }
                string queryML = "SELECT MaKhoa FROM Khoa";
                using (SqlCommand cmd = new SqlCommand(queryML, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                   DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    cbbTimTheoKhoa.DataSource = cbbMaKhoa.DataSource = dtHP;
                    cbbTimTheoKhoa.DisplayMember = cbbTimTheoKhoa.ValueMember = cbbMaKhoa.DisplayMember = cbbMaKhoa.ValueMember = "MaKhoa";     // Giá trị là Mã HP
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)

        {

            if (rowSelected >= 0)

            {

                try

                {

                    using (SqlConnection connection = new SqlConnection(constr))

                    {

                        connection.Open();

                        string query = @"UPDATE GiangVien
                                 SET HoTen = @HoTen, NgSinh = @NgSinh, GT = @GT,
                                    MaKhoa = @MaKhoa, SDT = @SDT, HocVi = @HocVi, HocHam = @HocHam
                                 WHERE MaGV = @MaGV";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaGV", tbMaGV.Text.Trim());
                            command.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                            command.Parameters.AddWithValue("@NgSinh", DateTime.ParseExact(tbNgaysinh.Text.Trim(), "dd/MM/yyyy", null));
                            command.Parameters.AddWithValue("@GT", rdoNam.Checked ? "Nam" : "Nu");
                            command.Parameters.AddWithValue("@MaKhoa", cbbMaKhoa.Text.Trim());
                            command.Parameters.AddWithValue("@SDT", tbSoDt.Text.Trim());
                            command.Parameters.AddWithValue("@HocVi", cbbHV.Text.Trim());
                            command.Parameters.AddWithValue("@HocHam", cbbHH.Text.Trim());

                            command.ExecuteNonQuery();
                            MessageBox.Show("Cập nhật thành công!");
                            LoadGV();
                            refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa giảng viên: " + ex.Message);
                }
            }

        }


        private void btnXoa_Click(object sender, EventArgs e)

        {

            if (rowSelected >= 0)

            {

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa giảng viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)

                {

                    try

                    {

                        using (SqlConnection connection = new SqlConnection(constr))

                        {

                            connection.Open();

                            string query = @"DELETE FROM GiangVien WHERE MaGV = @MaGV";



                            using (SqlCommand command = new SqlCommand(query, connection))

                            {

                                command.Parameters.AddWithValue("@MaGV", tbMaGV.Text.Trim());

                                command.ExecuteNonQuery();

                                MessageBox.Show("Xóa giảng viên thành công!");

                                LoadGV();

                                refresh();

                            }

                        }

                    }

                    catch (Exception ex)

                    {

                        MessageBox.Show("Lỗi khi xóa giảng viên: " + ex.Message);

                    }

                }

            }

        }



        private void BtnLamMoi_Click(object sender, EventArgs e)

        {

            refresh();

        }
        private void LoadGVTheoTimKiem(string maGV = null, string maKhoa = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    // Xây dựng câu truy vấn động dựa trên các tham số tìm kiếm
                    StringBuilder queryBuilder = new StringBuilder("SELECT * FROM GiangVien WHERE 1=1");

                    List<SqlParameter> parameters = new List<SqlParameter>();

                    if (!string.IsNullOrEmpty(maGV))
                    {
                        queryBuilder.Append(" AND MaGV LIKE @MaGV + '%'");
                        parameters.Add(new SqlParameter("@MaGV", maGV));
                    }

                    if (!string.IsNullOrEmpty(maKhoa))
                    {
                        queryBuilder.Append(" AND MaKhoa = @MaKhoa");
                        parameters.Add(new SqlParameter("@MaKhoa", maKhoa));
                    }

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        // Thêm các tham số vào command
                        command.Parameters.AddRange(parameters.ToArray());

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtGV = new DataTable();
                            adapter.Fill(dtGV);
                            dgvGV.DataSource = dtGV;

                            // Đặt lại tên cột hiển thị cho dễ hiểu
                            dgvGV.Columns["MaGV"].HeaderText = "Mã GV";
                            dgvGV.Columns["HoTen"].HeaderText = "Họ Tên GV";
                            dgvGV.Columns["NgSinh"].HeaderText = "Ngày Sinh";
                            dgvGV.Columns["GT"].HeaderText = "Giới Tính";
                            dgvGV.Columns["MaKhoa"].HeaderText = "Mã Khoa";
                            dgvGV.Columns["SDT"].HeaderText = "Số điện thoại";
                            dgvGV.Columns["HocVi"].HeaderText = "Học vị";
                            dgvGV.Columns["HocHam"].HeaderText = "Học hàm";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm giảng viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btntk(object sender, EventArgs e)
        {
            string maGVCanTim = ccbMGV.Text.Trim();
            string maKhoaCanTim = cbbTimTheoKhoa.Text.Trim();

            // Nếu cả hai trường tìm kiếm đều trống, hiển thị tất cả giảng viên
            if (string.IsNullOrEmpty(maGVCanTim) && string.IsNullOrEmpty(maKhoaCanTim))
            {
                LoadGV(); // Tải lại tất cả giảng viên
            }
            else
            {
                // Gọi hàm tìm kiếm với các giá trị đã nhập
                LoadGVTheoTimKiem(maGVCanTim, maKhoaCanTim);
            }
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            LoadGV();
        }
    }

}