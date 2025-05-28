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

   public partial class f : Form
    {
        String constr = Connecting.GetConnectionString();
        public f()
        {
            InitializeComponent();
            LoadHP();
            LoadComBoBox();
        }
        public void LoadHP()
       {
           try
           {
               using (SqlConnection connection = new SqlConnection(constr))
               {
                   connection.Open();
                   string query = @"
               SELECT DISTINCT 
                   hp.MaHP,
                   hp.TenHP,
                   hp.SoTC,
                   hp.SoTiet,
                   hp.LoaiHP,
                   hp.MaKhoa,
                   gv.HoTen AS TenGiangVien
               FROM 
                   HocPhan hp
               LEFT JOIN GiangDay gd ON hp.MaHP = gd.MaHP
               LEFT JOIN GiangVien gv ON gd.MaGV = gv.MaGV
              ";

                   using (SqlCommand command = new SqlCommand(query, connection))
                   {
                       using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                       {
                           DataTable dtHP = new DataTable();
                           adapter.Fill(dtHP);
                           dgvHP.DataSource = dtHP;
                            // Cập nhật tiêu đề cột cho dễ đọc
                           dgvHP.Columns["MaHP"].HeaderText = "Mã Học Phần";
                           dgvHP.Columns["TenHP"].HeaderText = "Tên Học Phần";
                           dgvHP.Columns["SoTC"].HeaderText = "Số Tín Chỉ";
                           dgvHP.Columns["SoTiet"].HeaderText = "Số Tiết";
                           dgvHP.Columns["LoaiHP"].HeaderText = "Loại Học Phần";
                           dgvHP.Columns["MaKhoa"].HeaderText = "Mã Khoa";
                           dgvHP.Columns["TenGiangVien"].HeaderText = "Giảng Viên Dạy";
                       }
                   }
               }
           }
           catch (Exception ex)
          {
               MessageBox.Show("Lỗi khi tải dữ liệu Học Phần: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FormHP_Load(object sender, EventArgs e)
        {
        }
        private void label11_Click(object sender, EventArgs e)
        {

        }
        int rowSelected = -1;
        private void dgvHP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }



        private void btnSua_Click(object sender, EventArgs e)

        {
                    try

            {

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"UPDATE HocPhan 
                             SET TenHP=@TenHP, SoTC=@SoTC, SoTiet=@SoTiet, LoaiHP=@LoaiHP, MaKhoa=@MaKhoa
                             WHERE MaHP=@MaHP";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHP", tbMaMon.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenHP", tbTenMon.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoTC", int.Parse(tbSoTC.Text.Trim()));
                        cmd.Parameters.AddWithValue("@SoTiet", int.Parse(tbSotiet.Text.Trim()));
                        cmd.Parameters.AddWithValue("@LoaiHP", cbbLoaiHP.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaKhoa", cbbMaKhoa.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Sửa học phần thành công.");
                LoadHP();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa học phần: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"INSERT INTO HocPhan (MaHP, TenHP, SoTC, SoTiet, LoaiHP, MaKhoa)
                             VALUES (@MaHP, @TenHP, @SoTC, @SoTiet, @LoaiHP, @MaKhoa)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHP", tbMaMon.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenHP", tbTenMon.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoTC", int.Parse(tbSoTC.Text.Trim()));
                        cmd.Parameters.AddWithValue("@SoTiet", int.Parse(tbSotiet.Text.Trim()));
                        cmd.Parameters.AddWithValue("@LoaiHP", cbbLoaiHP.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaKhoa", cbbMaKhoa.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm học phần thành công.");
                LoadHP();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm học phần: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
       {
           if (MessageBox.Show("Bạn có chắc chắn muốn xóa học phần này không?", "Xác nhận",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
           {
               try
               {
                   using (SqlConnection conn = new SqlConnection(constr))
                   {
                       conn.Open();
                       string query = "DELETE FROM HocPhan WHERE MaHP=@MaHP";
                       using (SqlCommand cmd = new SqlCommand(query, conn))
                       {
                           cmd.Parameters.AddWithValue("@MaHP", tbMaMon.Text.Trim());
                           cmd.ExecuteNonQuery();
                       }
                   }
                    MessageBox.Show("Xóa học phần thành công.");
                   LoadHP();
                   LamMoi();
               }
               catch (Exception ex)
               {
                    MessageBox.Show("Lỗi khi xóa học phần: " + ex.Message);
                }
            }
        }
        private void LamMoi()

        {
            tbMaMon.Text = "";
            tbTenMon.Text = "";
            tbSoTC.Text = "";
            tbSotiet.Text = "";
            cbbLoaiHP.SelectedIndex = -1;
            cbbMaKhoa.SelectedIndex = -1;
            tbGv.Text = "";

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            rowSelected = -1;
        }

        private void dgvHP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHP.CurrentRow != null)
            {
                // Kiểm tra có dữ liệu thực sự hay không
                if (!string.IsNullOrEmpty(dgvHP.CurrentRow.Cells["MaHP"].Value?.ToString()))
                {
                   tbMaMon.Text = dgvHP.CurrentRow.Cells["MaHP"].Value.ToString().Trim();
                  tbTenMon.Text = dgvHP.CurrentRow.Cells["TenHP"].Value.ToString().Trim();



                    tbSoTC.Text = dgvHP.CurrentRow.Cells["SoTC"].Value.ToString().Trim();
                    tbSotiet.Text = dgvHP.CurrentRow.Cells["SoTiet"].Value.ToString().Trim();

                    //  tbDiemCC.Text = dgvDiemSinhVien.CurrentRow.Cells["ChuyenCan"].Value.ToString().Trim();
                    cbbLoaiHP.Text = dgvHP.CurrentRow.Cells["LoaiHP"].Value.ToString().Trim();
                    cbbMaKhoa.Text = dgvHP.CurrentRow.Cells["MaKhoa"].Value.ToString().Trim();
                    tbGv.Text = dgvHP.CurrentRow.Cells["TenGiangVien"].Value.ToString().Trim();



                    rowSelected = dgvHP.CurrentRow.Index;

                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnThem.Enabled = false;
                }
                else

                {

                    LamMoi(); // Reset các trường nếu là dòng trống

                }

            }

        }

        private void LoadComBoBox()

        {

            cbbLoaiHP.Items.Add("Dai Cuong");
            cbbLoaiHP.Items.Add("Chuyen Nganh");
            cbbLoaiHP.Items.Add("Tu Chon");
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string queryML = "SELECT MaKhoa FROM Khoa";
                using (SqlCommand cmd = new SqlCommand(queryML, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    cbbMaKhoa.DataSource = dtHP;
                    cbbMaKhoa.DisplayMember = cbbMaKhoa.ValueMember = "MaKhoa";
                    cbbTimTheoKhoa.DataSource = dtHP;
                    cbbTimTheoKhoa.DisplayMember = cbbTimTheoKhoa.ValueMember = "MaKhoa";// Giá trị là Mã HP
                }
            }
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string queryML = "SELECT MaHP FROM HocPhan";
                using (SqlCommand cmd = new SqlCommand(queryML, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dtHP = new DataTable();
                    dtHP.Load(reader);
                    ccbTimKiemHP.DataSource = dtHP;
                    ccbTimKiemHP.DisplayMember = ccbTimKiemHP.ValueMember = "MaHP";     // Giá trị là Mã HP
                }

            }
        }
        private void LoadHPTheoTimKiem(string maHP = null, string maKhoa = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    StringBuilder queryBuilder = new StringBuilder(@"
                        SELECT DISTINCT
                            hp.MaHP,
                            hp.TenHP,
                            hp.SoTC,
                            hp.SoTiet,
                            hp.LoaiHP,
                            hp.MaKhoa,
                            gv.HoTen AS TenGiangVien
                        FROM
                            HocPhan hp
                        LEFT JOIN GiangDay gd ON hp.MaHP = gd.MaHP
                        LEFT JOIN GiangVien gv ON gd.MaGV = gv.MaGV
                        WHERE 1=1"); // Sử dụng WHERE 1=1 để dễ dàng thêm các điều kiện AND

                    List<SqlParameter> parameters = new List<SqlParameter>();

                    if (!string.IsNullOrEmpty(maHP))
                    {
                        queryBuilder.Append(" AND hp.MaHP LIKE @MaHP + '%'"); // Tìm kiếm theo tiền tố
                        parameters.Add(new SqlParameter("@MaHP", maHP));
                    }

                    if (!string.IsNullOrEmpty(maKhoa))
                    {
                        queryBuilder.Append(" AND hp.MaKhoa = @MaKhoa");
                        parameters.Add(new SqlParameter("@MaKhoa", maKhoa));
                    }

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtHP = new DataTable();
                            adapter.Fill(dtHP);
                            dgvHP.DataSource = dtHP;

                            // Cập nhật tiêu đề cột cho dễ đọc
                            dgvHP.Columns["MaHP"].HeaderText = "Mã Học Phần";
                            dgvHP.Columns["TenHP"].HeaderText = "Tên Học Phần";
                            dgvHP.Columns["SoTC"].HeaderText = "Số Tín Chỉ";
                            dgvHP.Columns["SoTiet"].HeaderText = "Số Tiết";
                            dgvHP.Columns["LoaiHP"].HeaderText = "Loại Học Phần";
                            dgvHP.Columns["MaKhoa"].HeaderText = "Mã Khoa";
                            dgvHP.Columns["TenGiangVien"].HeaderText = "Giảng Viên Dạy";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm học phần: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string maHPCanTim = ccbTimKiemHP.Text.Trim();
            string maKhoaCanTim = cbbTimTheoKhoa.Text.Trim(); // Hoặc cbbMaKhoa.Text.Trim() nếu dùng chung

            // Gọi hàm tìm kiếm với các giá trị từ các TextBox/ComboBox
            // Nếu cả hai trường đều trống, có thể gọi LoadHP() để hiển thị tất cả
            if (string.IsNullOrEmpty(maHPCanTim) && string.IsNullOrEmpty(maKhoaCanTim))
            {
                LoadHP(); // Tải lại tất cả học phần nếu không có tiêu chí tìm kiếm
            }
            else
            {
                LoadHPTheoTimKiem(maHPCanTim, maKhoaCanTim);
            }
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            LoadHP() ;
        }
    }

}