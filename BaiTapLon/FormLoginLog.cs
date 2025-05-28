using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient; // Sử dụng thư viện cho SQL Server

namespace BaiTapLon
{
    public partial class FormLoginLog : Form
    {
        // Lấy chuỗi kết nối từ lớp Connecting
        private string constr = Connecting.GetConnectionString();

        public FormLoginLog()
        {
            InitializeComponent();
            LoadUsersData(); // Tải dữ liệu từ bảng Users khi form khởi tạo
        }

        private void LoadUsersData()
        {
            try
            {
                // Sử dụng SqlConnection để kết nối đến cơ sở dữ liệu SQL Server
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open(); // Mở kết nối

                    // Câu truy vấn SQL để lấy dữ liệu từ bảng Users
                    // Chúng ta sẽ hiển thị các thông tin liên quan đến đăng nhập và trạng thái user
                    string query = "SELECT UserID, Username, Role, LastLogin, IsActive FROM Users ORDER BY Username ASC";

                    // Sử dụng SqlCommand để thực thi truy vấn
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Sử dụng SqlDataReader để đọc dữ liệu và điền vào DataTable
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dtUsers = new DataTable();
                            dtUsers.Load(reader); // Điền dữ liệu từ reader vào DataTable
                            dgvLoginLog.DataSource = dtUsers; // Gán DataTable làm nguồn dữ liệu cho DataGridView

                            // Đặt lại tên cột hiển thị cho dễ đọc
                            dgvLoginLog.Columns["UserID"].HeaderText = "ID Người Dùng";
                            dgvLoginLog.Columns["Username"].HeaderText = "Tên Đăng Nhập";
                            dgvLoginLog.Columns["Role"].HeaderText = "Vai Trò";
                            dgvLoginLog.Columns["LastLogin"].HeaderText = "Lần Đăng Nhập Cuối";
                            dgvLoginLog.Columns["IsActive"].HeaderText = "Trạng Thái Hoạt Động";

                            // Tự động điều chỉnh độ rộng cột
                            dgvLoginLog.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có vấn đề khi tải dữ liệu
                MessageBox.Show($"Lỗi khi tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormLoginLog_Load(object sender, EventArgs e)
        {
            // Có thể thêm logic khởi tạo khác khi form được tải
        }

        // Nút Refresh để tải lại dữ liệu từ bảng Users
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsersData(); // Tải lại dữ liệu người dùng
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Đã loại bỏ chức năng btnClearLog_Click vì nó không phù hợp với việc quản lý tài khoản người dùng trực tiếp trên form log.
        // Nếu bạn muốn quản lý tài khoản người dùng (thêm/sửa/xóa user), nên tạo một form riêng biệt cho mục đích đó.
    }
}
