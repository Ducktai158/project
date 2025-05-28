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
    public partial class FormDiemSV : Form
    {
        string MaGV;
        String MaLop;
        string constr = Connecting.GetConnectionString();
        public FormDiemSV()
        {
            InitializeComponent();
            LoadComboBoxData();
            LoadTatCaDiemSinhVien(); // Gọi phương thức để tải dữ liệu điểm

        }
        public FormDiemSV(string MaGV, string MaLop)
        {

            InitializeComponent();

            this.MaGV = MaGV;

            this.MaLop = MaLop;

            LoadComboBoxDataGV();

            LoadDiemTheoGiangVien();

        }

        private void LoadTatCaDiemSinhVien()

        {

            try

            {

                using (SqlConnection connection = new SqlConnection(constr))

                {

                    connection.Open();

                    string query = @"SELECT KQHP.MaSV, SV.HoTen, HP.MaHP, HP.TenHP, HP.SoTC, 

                                    KQHP.ThuongXuyen, KQHP.ChuyenCan, KQHP.GHP, KQHP.CHP, KQHP.TongKet, 

                                    KQHP.HocKy, KQHP.NamHoc

                            FROM KetQuaHocPhan KQHP

                            INNER JOIN HocPhan HP ON KQHP.MaHP = HP.MaHP

                            INNER JOIN SinhVien SV ON KQHP.MaSV = SV.MaSV

                            ORDER BY KQHP.MaSV, KQHP.NamHoc, KQHP.HocKy, HP.TenHP";



                    using (SqlCommand command = new SqlCommand(query, connection))

                    {

                        // Không cần thêm tham số @MaSV vì không lọc theo sinh viên nữa



                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                        {

                            DataTable dtDiem = new DataTable();

                            adapter.Fill(dtDiem);

                            dgvDiemSinhVien.DataSource = dtDiem;



                            // Đặt lại tên cột hiển thị cho dễ hiểu

                            dgvDiemSinhVien.Columns["MaSV"].HeaderText = "Mã SV";

                            dgvDiemSinhVien.Columns["HoTen"].HeaderText = "Họ Tên SV";

                            dgvDiemSinhVien.Columns["MaHP"].HeaderText = "Mã HP";

                            dgvDiemSinhVien.Columns["TenHP"].HeaderText = "Tên HP";

                            dgvDiemSinhVien.Columns["SoTC"].HeaderText = "Số TC";

                            dgvDiemSinhVien.Columns["ThuongXuyen"].HeaderText = "Thường Xuyên";

                            dgvDiemSinhVien.Columns["ChuyenCan"].HeaderText = "Chuyên Cần";

                            dgvDiemSinhVien.Columns["GHP"].HeaderText = "Giữa HK";

                            dgvDiemSinhVien.Columns["CHP"].HeaderText = "Cuối HK";

                            dgvDiemSinhVien.Columns["TongKet"].HeaderText = "Tổng Kết";

                            dgvDiemSinhVien.Columns["HocKy"].HeaderText = "Học Kỳ";

                            dgvDiemSinhVien.Columns["NamHoc"].HeaderText = "Năm Học";

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show("Lỗi khi tải điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void label8_Click(object sender, EventArgs e)

        {



        }



        private void label10_Click(object sender, EventArgs e)

        {

            this.Close();

        }



        private void FormDiemSV_Load(object sender, EventArgs e)

        {

            cbNH.Items.Clear();

            tbDiemTK.Enabled = false;

            tbSoTC.Enabled = false;

            int currentYear = DateTime.Now.Year;



            // Thêm 5 năm học tính từ năm hiện tại trở về trước

            for (int i = 0; i < 5; i++)

            {

                int startYear = currentYear - i - 1;

                int endYear = currentYear - i;

                cbNH.Items.Add($"{startYear}-{endYear}");

            }



            cbNH.SelectedIndex = 0; // Chọn năm học đầu tiên

        }

        int rowSelected = -1;

        private void dgvdiemsinhvien_cellclick(object sender, DataGridViewCellEventArgs e)

        {

            if (e.RowIndex >= 0 && dgvDiemSinhVien.CurrentRow != null)

            {

                // Kiểm tra có dữ liệu thực sự hay không

                if (!string.IsNullOrEmpty(dgvDiemSinhVien.CurrentRow.Cells["MaSV"].Value?.ToString()))

                {
                    cbMaSV.Text = dgvDiemSinhVien.CurrentRow.Cells["MaSV"].Value.ToString().Trim();
                    cbMaSV.Enabled = false;


                    cbMaHP.Text = dgvDiemSinhVien.CurrentRow.Cells["MaHP"].Value.ToString().Trim();
                    cbMaHP.Enabled = false;



                    tbSoTC.Text = dgvDiemSinhVien.CurrentRow.Cells["SoTC"].Value.ToString().Trim();

                    tbDiemTX.Text = dgvDiemSinhVien.CurrentRow.Cells["ThuongXuyen"].Value.ToString().Trim();
                    tbDiemCC.Text = dgvDiemSinhVien.CurrentRow.Cells["ChuyenCan"].Value.ToString().Trim();
                    tbDiemGHP.Text = dgvDiemSinhVien.CurrentRow.Cells["GHP"].Value.ToString().Trim();
                    tbDiemThi.Text = dgvDiemSinhVien.CurrentRow.Cells["CHP"].Value.ToString().Trim();
                    tbDiemTK.Text = dgvDiemSinhVien.CurrentRow.Cells["TongKet"].Value.ToString().Trim();
                    cbHK.Text = dgvDiemSinhVien.CurrentRow.Cells["HocKy"].Value.ToString().Trim();
                    cbNH.Text = dgvDiemSinhVien.CurrentRow.Cells["NamHoc"].Value.ToString().Trim();


                    rowSelected = dgvDiemSinhVien.CurrentRow.Index;

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



        private void refresh()

        {

            cbMaHP.ResetText();

            cbMaSV.ResetText();

            tbDiemThi.Text = "0";

            tbDiemTX.Text = "0";

            tbDiemCC.Text = "0";

            tbDiemTK.Text = "0";

            tbDiemGHP.Text = "0";

            btnSua.Enabled = false;

            btnThem.Enabled = true;

            btnXoa.Enabled = false;

            cbMaSV.Enabled = true;

            cbMaHP.Enabled = true;

        }



        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSV = cbMaSV.Text.Trim();
            string maHP = cbMaHP.Text.Trim();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SV và Mã HP.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                double diemCC, diemTX, diemGHP, diemCHP;

                // --- Bắt đầu kiểm tra điểm từ 0 đến 10 ---

                // Kiểm tra Điểm Chuyên Cần
                if (!double.TryParse(tbDiemCC.Text, out diemCC) || diemCC < 0 || diemCC > 10)
                {
                    MessageBox.Show("Điểm Chuyên Cần phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemCC = Math.Round(diemCC, 1);

                // Kiểm tra Điểm Thường Xuyên
                if (!double.TryParse(tbDiemTX.Text, out diemTX) || diemTX < 0 || diemTX > 10)
                {
                    MessageBox.Show("Điểm Thường Xuyên phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemTX = Math.Round(diemTX, 1);

                // Kiểm tra Điểm Giữa Học Phần
                if (!double.TryParse(tbDiemGHP.Text, out diemGHP) || diemGHP < 0 || diemGHP > 10)
                {
                    MessageBox.Show("Điểm Giữa Học Phần phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemGHP = Math.Round(diemGHP, 1);

                // Kiểm tra Điểm Cuối Học Phần
                if (!double.TryParse(tbDiemThi.Text, out diemCHP) || diemCHP < 0 || diemCHP > 10)
                {
                    MessageBox.Show("Điểm Cuối Học Phần phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemCHP = Math.Round(diemCHP, 1);

                // --- Kết thúc kiểm tra điểm ---


                double diemTK = (diemCC * 0.1) + (diemTX * 0.1) + (diemGHP * 0.2) + (diemCHP * 0.6);
                diemTK = Math.Round(diemTK, 1); // Làm tròn đến 1 chữ số thập phân

                tbDiemTK.Text = diemTK.ToString("0.0"); // Hiển thị 1 số sau dấu chấm

                // Thêm dữ liệu vào CSDL
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // Kiểm tra xem điểm này đã tồn tại chưa (tránh trùng)
                    string checkQuery = "SELECT COUNT(*) FROM KetQuaHocPhan WHERE MaSV = @MaSV AND MaHP = @MaHP";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        checkCmd.Parameters.AddWithValue("@MaHP", maHP);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Sinh viên đã có điểm cho học phần này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    // Thêm mới điểm
                    string insertQuery = @"INSERT INTO KetQuaHocPhan
                                 (MaSV, MaHP, ChuyenCan, ThuongXuyen, GHP, CHP, TongKet, HocKy, NamHoc)
                                 VALUES
                                 (@MaSV, @MaHP, @CC, @TX, @GHP, @CHP, @TongKet, @HocKy, @NamHoc)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaHP", maHP);
                        cmd.Parameters.AddWithValue("@CC", diemCC);
                        cmd.Parameters.AddWithValue("@TX", diemTX);
                        cmd.Parameters.AddWithValue("@GHP", diemGHP);
                        cmd.Parameters.AddWithValue("@CHP", diemCHP);
                        cmd.Parameters.AddWithValue("@TongKet", diemTK);

                        // Đảm bảo cbHK.SelectedItem và cbNH.SelectedItem không null trước khi gọi ToString()
                        if (cbHK.SelectedItem == null)
                        {
                            MessageBox.Show("Vui lòng chọn Học kỳ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (cbNH.SelectedItem == null)
                        {
                            MessageBox.Show("Vui lòng chọn Năm học.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        cmd.Parameters.AddWithValue("@HocKy", cbHK.SelectedItem?.ToString());
                        cmd.Parameters.AddWithValue("@NamHoc", cbNH.SelectedItem?.ToString());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Thêm điểm thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTatCaDiemSinhVien(); // Đảm bảo phương thức này tồn tại để tải lại DGV
                            refresh();
                        }
                        else
                        {
                            MessageBox.Show("Thêm điểm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (FormatException)
            {
                // Lỗi này giờ ít xảy ra hơn do đã kiểm tra TryParse trước đó
                MessageBox.Show("Lỗi định dạng số. Vui lòng kiểm tra lại các trường điểm.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadComboBoxData()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // Load danh sách sinh viên
                string querySV = "SELECT MaSV FROM SinhVien";
                using (SqlCommand cmd = new SqlCommand(querySV, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtSV = new DataTable();
                    dtSV.Load(reader);
                    cbMaSV.DataSource = dtSV;
                    cbMaSV.DisplayMember = cbMaSV.ValueMember = "MaSV";  // Hiển thị tên sinh viên
                }

                // Load danh sách học phần
                string queryHP = "SELECT MaHP FROM HocPhan";
                using (SqlCommand cmd = new SqlCommand(queryHP, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    cbMaHP.DataSource = dtHP;
                    cbMaHP.DisplayMember = cbMaHP.ValueMember = "MaHP";     // Giá trị là Mã HP
                }


                string queryML = "SELECT MaLop FROM Lop";
                using (SqlCommand cmd = new SqlCommand(queryML, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    cbTimKiem.DataSource = dtHP;
                    cbTimKiem.DisplayMember = cbTimKiem.ValueMember = "MaLop";     // Giá trị là Mã HP
                }
            }
        }


        private void tbDiemTK_TextChanged(object sender, EventArgs e)

        {



        }



        private void btnRefesh_Click(object sender, EventArgs e)

        {

            refresh();

            LoadComboBoxData();

        }



        private void btnSua_Click(object sender, EventArgs e)
        {
            string maSV = cbMaSV.Text.Trim();
            string maHP = cbMaHP.Text.Trim();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP))
            {
                MessageBox.Show("Vui lòng chọn sinh viên và học phần để sửa.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                double diemCC, diemTX, diemGHP, diemCHP;

                // --- Bắt đầu kiểm tra điểm từ 0 đến 10 ---

                // Kiểm tra Điểm Chuyên Cần
                if (!double.TryParse(tbDiemCC.Text, out diemCC) || diemCC < 0 || diemCC > 10)
                {
                    MessageBox.Show("Điểm Chuyên Cần phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemCC = Math.Round(diemCC, 1);

                // Kiểm tra Điểm Thường Xuyên
                if (!double.TryParse(tbDiemTX.Text, out diemTX) || diemTX < 0 || diemTX > 10)
                {
                    MessageBox.Show("Điểm Thường Xuyên phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemTX = Math.Round(diemTX, 1);

                // Kiểm tra Điểm Giữa Học Phần
                if (!double.TryParse(tbDiemGHP.Text, out diemGHP) || diemGHP < 0 || diemGHP > 10)
                {
                    MessageBox.Show("Điểm Giữa Học Phần phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemGHP = Math.Round(diemGHP, 1);

                // Kiểm tra Điểm Cuối Học Phần
                if (!double.TryParse(tbDiemThi.Text, out diemCHP) || diemCHP < 0 || diemCHP > 10)
                {
                    MessageBox.Show("Điểm Cuối Học Phần phải là số từ 0 đến 10.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diemCHP = Math.Round(diemCHP, 1);

                // --- Kết thúc kiểm tra điểm ---

                double diemTK = (diemCC * 0.1) + (diemTX * 0.1) + (diemGHP * 0.2) + (diemCHP * 0.6);
                diemTK = Math.Round(diemTK, 1); // Làm tròn đến 1 chữ số thập phân
                tbDiemTK.Text = diemTK.ToString("0.0"); // Hiển thị 1 số sau dấu chấm

                // Kiểm tra Học Kỳ và Năm Học (đây là phần bạn nên thêm kiểm tra nếu chúng NOT NULL trong DB)
                if (cbHK.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn Học kỳ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cbNH.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn Năm học.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    // Trong lệnh UPDATE, bạn không cần update MaSV và MaHP nếu chúng là khóa chính
                    // và bạn đang dùng chúng trong mệnh đề WHERE.
                    // Đồng thời, nếu Học Kỳ và Năm Học không thay đổi hoặc bạn muốn cập nhật chúng,
                    // thì thêm vào SET. Nếu không, hãy bỏ chúng khỏi SET.
                    // Hiện tại, tôi sẽ giữ chúng trong SET để đảm bảo tính đầy đủ nếu bạn có nhu cầu.
                    string updateQuery = @"UPDATE KetQuaHocPhan
                                   SET ChuyenCan = @CC, ThuongXuyen = @TX, GHP = @GHP, CHP = @CHP,
                                       TongKet = @TongKet, HocKy = @HocKy, NamHoc = @NamHoc
                                   WHERE MaSV = @MaSV AND MaHP = @MaHP";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CC", diemCC);
                        cmd.Parameters.AddWithValue("@TX", diemTX);
                        cmd.Parameters.AddWithValue("@GHP", diemGHP);
                        cmd.Parameters.AddWithValue("@CHP", diemCHP);
                        cmd.Parameters.AddWithValue("@TongKet", diemTK);
                        if (cbHK.SelectedItem == null)
                        {
                            MessageBox.Show("Vui lòng chọn Học kỳ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (cbNH.SelectedItem == null)
                        {
                            MessageBox.Show("Vui lòng chọn Năm học.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        cmd.Parameters.AddWithValue("@HocKy", cbHK.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@NamHoc", cbNH.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@MaSV", maSV); // Dùng trong WHERE
                        cmd.Parameters.AddWithValue("@MaHP", maHP); // Dùng trong WHERE

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật điểm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTatCaDiemSinhVien(); // Đảm bảo phương thức này tồn tại để tải lại DGV
                            refresh();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy điểm để cập nhật. Vui lòng kiểm tra lại Mã SV và Mã HP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (FormatException)
            {
                // Lỗi này giờ ít xảy ra hơn do đã kiểm tra TryParse trước đó
                MessageBox.Show("Lỗi định dạng số. Vui lòng kiểm tra lại các trường điểm (phải là số) và Học kỳ/Năm học.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnXoa_Click(object sender, EventArgs e)

        {
            string maSV = cbMaSV.Text.Trim();
            string maHP = cbMaHP.Text.Trim();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP))
            {
                MessageBox.Show("Vui lòng chọn sinh viên và học phần để xóa.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa điểm của sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();

                       string deleteQuery = "DELETE FROM KetQuaHocPhan WHERE MaSV = @MaSV AND MaHP = @MaHP";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaSV", maSV);
                            cmd.Parameters.AddWithValue("@MaHP", maHP);

                            int rows = cmd.ExecuteNonQuery();
                            if (rows > 0)
                            {
                                MessageBox.Show("Xóa điểm thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadTatCaDiemSinhVien();
                                refresh();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy điểm để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }


        private void BtnSearch_Click(object sender, EventArgs e)

        {

            string maLop = cbTimKiem.Text.Trim();

            TimKiem(maLop);



        }



        private void cbTimKiem_SelectedIndexChanged(object sender, EventArgs e)

        {



        }

        public void TimKiem(String ml)

        {

            if (ml == "") return;



            string query = @"

        SELECT KQHP.MaSV, SV.HoTen, HP.MaHP, HP.TenHP, HP.SoTC,

               KQHP.ThuongXuyen, KQHP.ChuyenCan, KQHP.GHP, KQHP.CHP, KQHP.TongKet,

               KQHP.HocKy, KQHP.NamHoc

        FROM KetQuaHocPhan KQHP

        JOIN HocPhan HP ON KQHP.MaHP = HP.MaHP

        JOIN SinhVien SV ON KQHP.MaSV = SV.MaSV

        WHERE SV.MaLop LIKE @MaLop

        ORDER BY KQHP.MaSV, KQHP.NamHoc, KQHP.HocKy, HP.TenHP";



            using (SqlConnection conn = new SqlConnection(constr))

            using (SqlCommand cmd = new SqlCommand(query, conn))

            {

                cmd.Parameters.AddWithValue("@MaLop", "%" + ml + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvDiemSinhVien.DataSource = dt;

            }

        }

        private void TatCa_Click(object sender, EventArgs e)

        {

            LoadTatCaDiemSinhVien();

        }

        private void LoadComboBoxDataGV()

        {

            using (SqlConnection conn = new SqlConnection(constr))

            {

                conn.Open();



                // Chỉ load MaHP mà giáo viên đang dạy

                string queryHP = @"

            SELECT DISTINCT HP.MaHP

            FROM GiangDay GD

            JOIN HocPhan HP ON GD.MaHP = HP.MaHP

            WHERE GD.MaGV = @MaGV";



                using (SqlCommand cmd = new SqlCommand(queryHP, conn))

                {



                    cmd.Parameters.AddWithValue("@MaGV", MaGV);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    cbMaHP.DataSource = dt;

                    cbMaHP.DisplayMember = "MaHP";

                    cbMaHP.ValueMember = "MaHP";

                }



                // Chỉ load MaSV trong các lớp mà giáo viên đang giảng dạy

                string querySV = @"

            SELECT DISTINCT SV.MaSV

            FROM SinhVien SV

            JOIN GiangDay GD ON SV.MaLop = GD.MaLop

            WHERE GD.MaGV = @MaGV";



                using (SqlCommand cmd = new SqlCommand(querySV, conn))

                {

                    cmd.Parameters.AddWithValue("@MaGV", MaGV);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    cbMaSV.DataSource = dt;

                    cbMaSV.DisplayMember = "MaSV";

                    cbMaSV.ValueMember = "MaSV";

                }

            }

        }

        private void LoadDiemTheoGiangVien()

        {

            using (SqlConnection conn = new SqlConnection(constr))

            {

                conn.Open();



                string query = @"

                 SELECT

        sv.MaSV,
        sv.HoTen AS HoTenSV,
        sv.MaLop,
        gv.HoTen AS HoTenGV, -- Tên giảng viên
        hp.MaHP,
        hp.TenHP,
        hp.SoTC,
        kqhp.ChuyenCan,
        kqhp.ThuongXuyen,
        kqhp.GHP,
        kqhp.CHP,
        kqhp.TongKet,
        kqhp.HocKy,
        kqhp.NamHoc
    FROM SinhVien sv
    JOIN KetQuaHocPhan kqhp ON sv.MaSV = kqhp.MaSV
    JOIN HocPhan hp ON kqhp.MaHP = hp.MaHP
    JOIN GiangDay gd ON kqhp.MaHP = gd.MaHP
                     AND kqhp.HocKy = gd.HocKy
                     AND kqhp.NamHoc = gd.NamHoc
                     AND sv.MaLop = gd.MaLop -- Đảm bảo lớp học khớp với bản ghi giảng dạy
    JOIN GiangVien gv ON gd.MaGV = gv.MaGV -- Tham gia bảng GiangVien để lấy tên giảng viên
    WHERE sv.MaLop = @MaLop AND gd.MaGV = @MaGV -- Lọc theo mã lớp và mã giảng viên
    ORDER BY sv.MaSV, hp.MaHP;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
               {
                   cmd.Parameters.AddWithValue("@MaGV", MaGV);
                   cmd.Parameters.AddWithValue("@MaLop", MaLop);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                   {
                       DataTable dt = new DataTable();
                       adapter.Fill(dt);
                       dgvDiemSinhVien.DataSource = dt;
                    }
                }
            }
        }

        private void cbNH_SelectedIndexChanged(object sender, EventArgs e)

        {



        }

        private void btnSua_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}