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
    public partial class FormAccout : Form
    {
        string constr = Connecting.GetConnectionString();
        public FormAccout()
        {
            InitializeComponent();
            LoadRoles();
        }
        
   
        private void LoadRoles()
        {
            cmbRole.Items.Add("SinhVien");
            cmbRole.Items.Add("GiangVien");
            cmbRole.SelectedIndex = 0; // Chọn vai trò đầu tiên mặc định
        }

        private void Accout_Load(object sender, EventArgs e)
        {

        }

     
        private bool IsUsernameExists(SqlConnection connection, string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        private void AddUserToDatabase(SqlConnection connection, string username, string hashedPassword, string role, string relatedInfo)
        {
            using (SqlCommand command = new SqlCommand("AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@RelatedID", relatedInfo);
                command.ExecuteNonQuery();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string role = cmbRole.SelectedItem.ToString();
            string relatedInfo = txtRelatedInfo.Text;

            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6) // Ví dụ: Yêu cầu mật khẩu tối thiểu 6 ký tự
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    // 2. Kiểm tra tên đăng nhập có bị trùng không
                    if (IsUsernameExists(connection, username))
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 3. Băm mật khẩu

                    // 4. Thêm người dùng vào cơ sở dữ liệu (sử dụng Stored Procedure AddNewUser)
                    AddUserToDatabase(connection, username, password, role, relatedInfo);

                    MessageBox.Show("Người dùng đã được thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // Đóng form và báo hiệu thành công
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
