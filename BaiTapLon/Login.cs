using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class Login : Form
    {
        String constr = Connecting.GetConnectionString();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tbUserName.Text.Trim();
            string password = tbPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || username == "User name" || password == "Password")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("AuthenticateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // ✅ Băm mật khẩu trước khi gửi đi
                        string hashedPassword = HashPassword(password);

                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string role = reader.GetString(1);
                                string relatedId = reader.IsDBNull(2) ? null : reader.GetString(2);

                                Form nextForm = null;

                                switch (role.ToLower())
                                {
                                    case "admin":
                                        nextForm = new FormMain();
                                        break;
                                    case "sinhvien":
                                        nextForm = new FormDiem1SV(relatedId);
                                        break;
                                    case "giangvien":
                                        nextForm = new Form1GV(relatedId);
                                        break;
                                    case "chunhiem":
                                        nextForm = new FormCN(relatedId);
                                        break;
                                    default:
                                        MessageBox.Show("Vai trò người dùng không hợp lệ.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                }

                                if (nextForm != null)
                                {
                                    this.Hide();
                                    nextForm.ShowDialog();
                                    this.Show();
                                }

                                // Reset
                                tbPassword.Text = "Password";
                                tbPassword.ForeColor = Color.Gray;
                                tbPassword.UseSystemPasswordChar = false;
                                tbUserName.Text = "User name";
                                tbUserName.ForeColor = Color.Gray;
                                checkBox1.Checked = false;
                            }
                            else
                            {
                                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối hoặc truy vấn cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Hàm băm mật khẩu SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // hex string
                }
                return builder.ToString();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            tbPassword.Text = "Password";
            tbPassword.ForeColor = Color.Gray;
            tbPassword.UseSystemPasswordChar = false;

            tbUserName.Text = "User name";
            tbUserName.ForeColor = Color.Gray;

            tbUserName.Focus();
        }

        private void tbUserName_Click(object sender, EventArgs e)
        {
            if (tbUserName.Text == "User name")
            {
                tbUserName.Text = "";
                tbUserName.ForeColor = Color.Black;
            }
        }

        private void tbPassword_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text == "Password")
            {
                tbPassword.Text = "";
                tbPassword.ForeColor = Color.Black;
                tbPassword.UseSystemPasswordChar = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (tbPassword.Text != "Password")
                tbPassword.UseSystemPasswordChar = !checkBox1.Checked;
            else
                tbPassword.UseSystemPasswordChar = false;

            checkBox1.Text = "Hiện mật khẩu";
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void tbUserName_TextChanged(object sender, EventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
    }
}
