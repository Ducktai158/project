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
        string constr = Connecting.GetConnectionString();
        public FormDiemSV()
        {
            InitializeComponent();
            LoadTatCaDiemSinhVien(); // Gọi phương thức để tải dữ liệu điểm
      
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

        }
        int rowSelected = -1;
        private void dgvdiemsinhvien_cellclick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDiemSinhVien.CurrentRow != null)
            {
                // Kiểm tra có dữ liệu thực sự hay không
                if (!string.IsNullOrEmpty(dgvDiemSinhVien.CurrentRow.Cells["MaSV"].Value?.ToString()))
                {
                    tbMaSV.Text = dgvDiemSinhVien.CurrentRow.Cells["MaSV"].Value.ToString().Trim();
                    tbMaSV.Enabled = false;


                    tbHP.Text = dgvDiemSinhVien.CurrentRow.Cells["MaHP"].Value.ToString().Trim();
                    tbHP.Enabled = false;

                   

                    tbSoTC.Text = dgvDiemSinhVien.CurrentRow.Cells["SoTC"].Value.ToString().Trim();

                    tbDiemTX.Text = dgvDiemSinhVien.CurrentRow.Cells["ThuongXuyen"].Value.ToString().Trim();
                    tbDiemCC.Text = dgvDiemSinhVien.CurrentRow.Cells["ChuyenCan"].Value.ToString().Trim();
                    tbDiemGHP.Text = dgvDiemSinhVien.CurrentRow.Cells["GHP"].Value.ToString().Trim();
                    tbDiemThi.Text = dgvDiemSinhVien.CurrentRow.Cells["CHP"].Value.ToString().Trim();
                    tbDiemTK.Text = dgvDiemSinhVien.CurrentRow.Cells["TongKet"].Value.ToString().Trim();

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
            tbHP.ResetText();
            tbMaSV.ResetText();
            tbDiemThi.Text = "0";
            tbDiemTX.Text = "0";
            tbDiemCC.Text = "0";
            tbDiemTK.Text = "0";
            tbDiemGHP.Text = "0";
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = false;
            tbHP.Enabled = true;
            tbMaSV.Enabled = true;
        }
    }
}
