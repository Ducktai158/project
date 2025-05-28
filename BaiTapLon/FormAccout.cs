using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
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
            cmbRole.Items.Add("ChuNhiem");
            cmbRole.SelectedIndex = 0;
        }

        private void FormAccout_Load(object sender, EventArgs e)
        {
            // Nothing here for now
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

        private bool IsRelatedInfoUsed(SqlConnection connection, string role, string relatedID)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Role = @Role AND RelatedID = @RelatedID";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@RelatedID", relatedID);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private void AddUserToDatabase(SqlConnection connection, string username, string hashedPassword, string role, string relatedInfo)
        {
            using (SqlCommand command = new SqlCommand("AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", hashedPassword); // hashed!
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@RelatedID", relatedInfo);
                command.ExecuteNonQuery();
            }
        }

        // ✅ Hàm băm SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();
            string relatedInfo = txtRelatedInfo.Text.Trim();

            // Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(role)
                || string.IsNullOrWhiteSpace(relatedInfo))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    if (IsUsernameExists(connection, username))
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (IsRelatedInfoUsed(connection, role, relatedInfo))
                    {
                        MessageBox.Show("Mã số đã được sử dụng cho vai trò này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ✅ Băm mật khẩu
                    string hashedPassword = HashPassword(password);

                    // Thêm vào CSDL
                    AddUserToDatabase(connection, username, hashedPassword, role, relatedInfo);

                    MessageBox.Show("Người dùng đã được thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nếu bạn muốn thay đổi nhãn hoặc placeholder khi chọn vai trò
        }
    }
}
