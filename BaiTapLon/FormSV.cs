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
    public partial class FormSV : Form

   {
        string constr = Connecting.GetConnectionString();

        public FormSV()
        {
            InitializeComponent();
            LoadSV();
            LoadComBoBox();
        }
        private void LoadSV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string query = @"SELECT * FROM SinhVien";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Không cần thêm tham số @MaSV vì không lọc theo sinh viên nữa

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtSV = new DataTable();
                            adapter.Fill(dtSV);
                            dgvSV.DataSource = dtSV;

                            // Đặt lại tên cột hiển thị cho dễ hiểu
                            dgvSV.Columns["MaSV"].HeaderText = "Mã SV";
                            dgvSV.Columns["HoTen"].HeaderText = "Họ Tên SV";
                            dgvSV.Columns["NgSinh"].HeaderText = "Ngày Sinh";
                            dgvSV.Columns["GT"].HeaderText = "Giới Tính";
                            dgvSV.Columns["MaLop"].HeaderText = "Mã Lớp";
                            dgvSV.Columns["TrangThai"].HeaderText = "Trạng Thái";
                            dgvSV.Columns["NamNhapHoc"].HeaderText = "Năm Nhập Học";
                            dgvSV.Columns["NamRaTruong"].HeaderText = "Năm Ra Trường";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadComBoBox()

        {

            cbNamNH.Items.Add("2021");
            cbNamNH.Items.Add("2022");
            cbNamNH.Items.Add("2023");
            cbNamNH.Items.Add("2024");
            cbNamNH.Items.Add("2025");

            cbNamRaTruong.Items.Add("2026");
            cbNamRaTruong.Items.Add("2027");
            cbNamRaTruong.Items.Add("2028");
            cbNamRaTruong.Items.Add("2029");
            cbNamRaTruong.Items.Add("2030");

            cbTrangThai.Items.Add("Dang hoc");
            cbTrangThai.Items.Add("Tam Nghi");
            cbTrangThai.Items.Add("Da ra truong");



            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                string queryML = "SELECT MaLop FROM Lop";
                using (SqlCommand cmd = new SqlCommand(queryML, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    cbbMaLopTK.DataSource = cbMaLop.DataSource = dtHP;
                    cbbMaLopTK.DisplayMember = cbbMaLopTK.ValueMember = cbMaLop.DisplayMember = cbMaLop.ValueMember = "MaLop";
                }

                string queryMSV = "SELECT MaSV FROM SinhVien";
                using (SqlCommand cmd = new SqlCommand(queryMSV, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    cbbMaSVTK.DataSource = dtHP;
                    cbbMaSVTK.DisplayMember = cbbMaSVTK.ValueMember = "MaSV";
                }
            }
        }
        private void FormSV_Load(object sender, EventArgs e)
        {

        }
        int rowSelected = -1;
        private void dvgSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && dgvSV.CurrentRow != null)
            {
                // Kiểm tra có dữ liệu thực sự hay không
                if (!string.IsNullOrEmpty(dgvSV.CurrentRow.Cells["MaSV"].Value?.ToString()))
                {
                    tbMaSV.Text = dgvSV.CurrentRow.Cells["MaSV"].Value.ToString().Trim();


                    cbMaLop.Text = dgvSV.CurrentRow.Cells["MaLop"].Value.ToString().Trim();





                    tbHoTen.Text = dgvSV.CurrentRow.Cells["HoTen"].Value.ToString().Trim();



                    if (DateTime.TryParse(dgvSV.CurrentRow.Cells["NgSinh"].Value?.ToString(), out DateTime ngSinh))

                    {

                        tbNgaysinh.Text = ngSinh.ToString("dd/MM/yyyy");

                    }

                    else

                    {

                        tbNgaysinh.Text = ""; // hoặc gán giá trị mặc định nếu parse lỗi

                    }





                    if (dgvSV.CurrentRow.Cells["GT"].Value.ToString().Trim().Equals("Nam", StringComparison.InvariantCultureIgnoreCase))

                        rdoNam.Checked = true;

                    else rdoNu.Checked = true;

                    //  tbDiemCC.Text = dgvDiemSinhVien.CurrentRow.Cells["ChuyenCan"].Value.ToString().Trim();

                    cbTrangThai.Text = dgvSV.CurrentRow.Cells["TrangThai"].Value.ToString().Trim();

                    cbNamNH.Text = dgvSV.CurrentRow.Cells["NamNhapHoc"].Value.ToString().Trim();

                    cbNamRaTruong.Text = dgvSV.CurrentRow.Cells["NamRaTruong"].Value.ToString().Trim();







                    rowSelected = dgvSV.CurrentRow.Index;



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
                if (!ValidateMaSV(tbMaSV.Text.Trim(), cbMaLop.Text.Trim()))
                {
                    return; // Dừng lại nếu dữ liệu không hợp lệ
                }
                // 1. Lấy giá trị Năm Nhập Học và kiểm tra tính hợp lệ (nên có)
                int namNhapHoc;
                if (!int.TryParse(cbNamNH.Text, out namNhapHoc))
                {
                    MessageBox.Show("Vui lòng chọn Năm nhập học hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng lại nếu năm nhập học không hợp lệ
                }

                // 2. Xử lý giá trị Năm Ra Trường: Có thể là số nguyên hoặc NULL
                int? namRaTruong = null; // Khởi tạo là null (nullable int)

                // Kiểm tra xem cbNamRaTruong có giá trị hay không
                if (!string.IsNullOrWhiteSpace(cbNamRaTruong.Text))
                {
                    int tempNamRaTruong;
                    if (int.TryParse(cbNamRaTruong.Text, out tempNamRaTruong))
                    {
                        namRaTruong = tempNamRaTruong; // Gán giá trị nếu là số nguyên hợp lệ
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn Năm ra trường hợp lệ (là số).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng nếu năm ra trường không hợp lệ
                    }
                }
                // Nếu cbNamRaTruong.Text là rỗng hoặc khoảng trắng, namRaTruong vẫn giữ giá trị null

                // 3. Kiểm tra ràng buộc Năm Nhập Học < Năm Ra Trường (chỉ khi NamRaTruong có giá trị)
                if (namRaTruong.HasValue && namNhapHoc >= namRaTruong.Value)
                {
                    MessageBox.Show("Năm nhập học phải nhỏ hơn năm ra trường!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng thao tác nếu vi phạm ràng buộc
                }


                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"INSERT INTO SinhVien (MaSV, HoTen, NgSinh, GT, MaLop, TrangThai, NamNhapHoc, NamRaTruong)
                             VALUES (@MaSV, @HoTen, @NgSinh, @GT, @MaLop, @TrangThai, @NamNhapHoc, @NamRaTruong)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", tbMaSV.Text.Trim());
                        cmd.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgSinh", DateTime.ParseExact(tbNgaysinh.Text.Trim(), "dd/MM/yyyy", null));
                        cmd.Parameters.AddWithValue("@GT", rdoNam.Checked ? "Nam" : "Nữ"); // Đảm bảo đúng 'Nam'/'Nữ' như constraint GT
                        cmd.Parameters.AddWithValue("@MaLop", cbMaLop.Text);
                        cmd.Parameters.AddWithValue("@TrangThai", cbTrangThai.Text);
                        cmd.Parameters.AddWithValue("@NamNhapHoc", namNhapHoc); // Sử dụng biến int đã parse

                        // Đây là phần quan trọng để xử lý NULL cho NamRaTruong
                        if (namRaTruong.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@NamRaTruong", namRaTruong.Value); // Nếu có giá trị, gửi giá trị đó
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@NamRaTruong", DBNull.Value); // Nếu là NULL, gửi DBNull.Value
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm sinh viên thành công!");
                LoadSV();
                refresh(); // Thêm refresh() để làm sạch form sau khi thêm
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng: Vui lòng kiểm tra lại Ngày sinh (dd/MM/yyyy) và Năm nhập học/Năm ra trường phải là số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateMaSV(tbMaSV.Text.Trim(), cbMaLop.Text.Trim()))
                {
                    return; // Dừng lại nếu dữ liệu không hợp lệ
                }
                // 1. Lấy giá trị Năm Nhập Học và kiểm tra tính hợp lệ
                int namNhapHoc;
                if (!int.TryParse(cbNamNH.Text, out namNhapHoc))
                {
                    MessageBox.Show("Vui lòng chọn Năm nhập học hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng lại nếu năm nhập học không hợp lệ
                }

                // 2. Xử lý giá trị Năm Ra Trường: Có thể là số nguyên hoặc NULL
                int? namRaTruong = null; // Khởi tạo là null (nullable int)

                // Kiểm tra xem cbNamRaTruong có giá trị hay không
                if (!string.IsNullOrWhiteSpace(cbNamRaTruong.Text))
                {
                    int tempNamRaTruong;
                    if (int.TryParse(cbNamRaTruong.Text, out tempNamRaTruong))
                    {
                        namRaTruong = tempNamRaTruong; // Gán giá trị nếu là số nguyên hợp lệ
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn Năm ra trường hợp lệ (là số).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Dừng nếu năm ra trường không hợp lệ
                    }
                }
                // Nếu cbNamRaTruong.Text là rỗng hoặc khoảng trắng, namRaTruong vẫn giữ giá trị null

                // 3. Kiểm tra ràng buộc Năm Nhập Học < Năm Ra Trường (chỉ khi NamRaTruong có giá trị)
                if (namRaTruong.HasValue && namNhapHoc >= namRaTruong.Value)
                {
                    MessageBox.Show("Năm nhập học phải nhỏ hơn năm ra trường!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng thao tác nếu vi phạm ràng buộc
                }


                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"UPDATE SinhVien
                             SET HoTen = @HoTen, NgSinh = @NgSinh, GT = @GT, MaLop = @MaLop,
                                 TrangThai = @TrangThai, NamNhapHoc = @NamNhapHoc, NamRaTruong = @NamRaTruong
                             WHERE MaSV = @MaSV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", tbMaSV.Text.Trim());
                        cmd.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                        // Đảm bảo định dạng ngày tháng phải đúng dd/MM/yyyy
                        cmd.Parameters.AddWithValue("@NgSinh", DateTime.ParseExact(tbNgaysinh.Text.Trim(), "dd/MM/yyyy", null));
                        // Kiểm tra lại ràng buộc GT (Nam/Nữ) trong DB để đảm bảo chuỗi gửi đi chính xác
                        cmd.Parameters.AddWithValue("@GT", rdoNam.Checked ? "Nam" : "Nu");
                        cmd.Parameters.AddWithValue("@MaLop", cbMaLop.Text);
                        cmd.Parameters.AddWithValue("@TrangThai", cbTrangThai.Text);
                        cmd.Parameters.AddWithValue("@NamNhapHoc", namNhapHoc); // Sử dụng biến int đã parse

                        // Đây là phần quan trọng để xử lý NULL cho NamRaTruong
                        if (namRaTruong.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@NamRaTruong", namRaTruong.Value); // Nếu có giá trị, gửi giá trị đó
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@NamRaTruong", DBNull.Value); // Nếu là NULL, gửi DBNull.Value
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cập nhật sinh viên thành công!");
                LoadSV();
                refresh(); // Thêm refresh() để làm sạch form sau khi sửa
            }
            catch (FormatException)
            {
                // Bắt lỗi khi parse chuỗi sang số/ngày tháng không thành công
                MessageBox.Show("Lỗi định dạng: Vui lòng kiểm tra lại Ngày sinh (dd/MM/yyyy) và Năm nhập học/Năm ra trường phải là số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi chung khác từ cơ sở dữ liệu
                MessageBox.Show("Lỗi cập nhật sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnXoa_Click(object sender, EventArgs e)

        {

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

            {

                try

                {

                    using (SqlConnection conn = new SqlConnection(constr))

                    {

                        conn.Open();

                        string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                        using (SqlCommand cmd = new SqlCommand(query, conn))

                        {
                            cmd.Parameters.AddWithValue("@MaSV", tbMaSV.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }

                    }



                    MessageBox.Show("Xóa sinh viên thành công!");

                    LoadSV();

                }

                catch (Exception ex)

                {

                    MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message);

                }

            }

        }

        private void refresh()

        {
            tbMaSV.Text = "";
            tbHoTen.Text = "";
            tbNgaysinh.Text = "";
            cbMaLop.SelectedIndex = -1;
            cbTrangThai.SelectedIndex = -1;
            cbNamNH.SelectedIndex = -1;
            cbNamRaTruong.SelectedIndex = -1;

            rdoNam.Checked = false;
            rdoNu.Checked = false;

            tbMaSV.Enabled = true;
            cbMaLop.Enabled = true;

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

           dgvSV.ClearSelection(); // Bỏ chọn dòng trong DataGridView
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            refresh();
        }
        private void dgvSV_CellContentClick(object sender, DataGridViewCellEventArgs e)
       {
        }
       private void SearchStudents(string maSV, string maLop)
       {
           try
           {
               using (SqlConnection connection = new SqlConnection(constr))
               {
                   connection.Open();
                   string query = "SELECT * FROM SinhVien WHERE 1=1"; // Start with a true condition
                    // Add conditions based on provided search criteria
                   if (!string.IsNullOrWhiteSpace(maSV))
                   {
                      query += " AND MaSV LIKE @MaSV";
                    }
                    if (!string.IsNullOrWhiteSpace(maLop))
                    {
                        query += " AND MaLop LIKE @MaLop";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrWhiteSpace(maSV))
                        {
                            command.Parameters.AddWithValue("@MaSV", "%" + maSV.Trim() + "%");
                        }
                        if (!string.IsNullOrWhiteSpace(maLop))
                        {
                            command.Parameters.AddWithValue("@MaLop", "%" + maLop.Trim() + "%");
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtSV = new DataTable();
                            adapter.Fill(dtSV);
                            dgvSV.DataSource = dtSV;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example of how to call the search method (you'd link this to a button click event)
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Assuming you have TextBoxes named 'txtSearchMaSV' and 'txtSearchMaLop' for input
            // For simplicity, let's use the existing tbMaSV and cbMaLop for demonstration,
            // but in a real application, you'd likely have dedicated search input fields.
            string searchMaSV = cbbMaSVTK.Text; // Or a dedicated search textbox for MaSV
            string searchMaLop = cbbMaLopTK.Text; // Or a dedicated search combobox for MaLop

            SearchStudents(searchMaSV, searchMaLop);
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            LoadSV();
        }
        // Thêm hàm này vào class của bạn (ví dụ, FormSV)
        private bool ValidateMaSV(string maSV, string maLop)
        {
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maLop))
            {
                MessageBox.Show("Mã sinh viên và Mã lớp không được để trống.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // --- Kiểm tra phần đầu của MaSV khớp với MaLop ---
            if (maSV.Length < 2)
            {
                MessageBox.Show("Mã sinh viên phải có ít nhất 2 ký tự để kiểm tra định dạng.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string partOfMaSV = maSV.Substring(0, maSV.Length - 2);
            // So sánh không phân biệt hoa thường để mềm dẻo hơn, nếu bạn cần chính xác thì bỏ StringComparison.OrdinalIgnoreCase
            if (!partOfMaSV.Equals(maLop, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Phần mã lớp trong Mã sinh viên không khớp với Mã lớp đã chọn.\n" +
                                "Ví dụ: Nếu Mã lớp là 'CNTT', Mã sinh viên phải bắt đầu bằng 'CNTT' (như 'CNTT01').",
                                "Lỗi định dạng Mã SV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // --- Kiểm tra 2 ký tự cuối của MaSV là số ---
            string lastTwoCharsOfMaSV = maSV.Substring(maSV.Length - 2);

            // Cách 1: Dùng TryParse (đơn giản, hiệu quả)
            if (!int.TryParse(lastTwoCharsOfMaSV, out int num))
            {
                MessageBox.Show("Hai ký tự cuối của Mã sinh viên phải là số (ví dụ: '01', '99').", "Lỗi định dạng Mã SV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Cách 2: Dùng Regex (mạnh mẽ hơn nếu có các quy tắc phức tạp hơn sau này)
            // using System.Text.RegularExpressions; // Cần thêm using này ở đầu file
            // Regex regex = new Regex(@"^\d{2}$"); // ^ bắt đầu chuỗi, \d là số, {2} là 2 lần, $ kết thúc chuỗi
            // if (!regex.IsMatch(lastTwoCharsOfMaSV))
            // {
            //     MessageBox.Show("Hai ký tự cuối của Mã sinh viên phải là số (ví dụ: '01', '99').", "Lỗi định dạng Mã SV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     return false;
            // }

            return true; // Tất cả các kiểm tra đều hợp lệ
        }
    }

}