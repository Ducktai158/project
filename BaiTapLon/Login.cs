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
using static System.Net.Mime.MediaTypeNames;

namespace BaiTapLon
{
    public partial class Login : Form
    {
        String constr = Connecting.GetConnectionString();
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

                    // Sử dụng Stored Procedure AuthenticateUser mà bạn đã tạo trong file SQL
                    using (SqlCommand command = new SqlCommand("AuthenticateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password); // Trong thực tế, bạn nên băm mật khẩu nhập vào để so sánh với mật khẩu đã băm trong CSDL

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string role = reader.GetString(1);
                                string relatedId = reader.IsDBNull(2) ? null : reader.GetString(2);

                                switch (role.ToLower())
                                {
                                    case "admin":
                                        // Mở form dành cho Admin
                                        FormMain formAdmin = new FormMain(); // Giả sử bạn có FormAdmin
                                        this.Hide();
                                        formAdmin.ShowDialog();
                                        this.Show();
                                        break;
                                    case "sinhvien":
                                        // Mở form dành cho Sinh viên, có thể truyền maSV (relatedId)
                                        FormDiem1SV formSinhVien = new FormDiem1SV(relatedId); // Giả sử bạn có FormSinhVien
                                        this.Hide();
                                        formSinhVien.ShowDialog();
                                        this.Show();
                                        break;
                                    case "giangvien":
                                        // Mở form dành cho Giảng viên, có thể truyền maGV (relatedId)
                                        Form1GV formGiangVien = new Form1GV(relatedId); // Giả sử bạn có FormGiangVien
                                        this.Hide();
                                        formGiangVien.ShowDialog();
                                        this.Show();
                                        break;
                                    case "chunhiem":
                                        // Mở form dành cho Giảng viên, có thể truyền maGV (relatedId)
                                        FormCN formGiangVienCN = new FormCN(relatedId); // Giả sử bạn có FormGiangVien
                                        this.Hide();
                                        formGiangVienCN.ShowDialog();
                                        this.Show();
                                        break;
                                    default:
                                        MessageBox.Show("Vai trò người dùng không hợp lệ.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                }


                                // Sau khi đăng nhập thành công, có thể reset các trường nhập liệu

                                tbPassword.Text = "Password";
                                tbPassword.ForeColor = Color.Gray;
                                tbPassword.UseSystemPasswordChar = false;
                                tbUserName.Text = "User name";
                                tbUserName.ForeColor = Color.Gray;
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



        private void Login_Load(object sender, EventArgs e)

        {



        }



        private void lbClose_Click(object sender, EventArgs e)

        {

            this.Close();

        }



        private void label1_Click_1(object sender, EventArgs e)

        {



        }



        private void tbUserName_TextChanged(object sender, EventArgs e)

        {



        }



        private void tbUserName_Click(object sender, EventArgs e)
        {
            this.tbUserName.Text = "";
        }



        private void tbPassword_Click(object sender, EventArgs e)

        {

            this.tbPassword.Text = "";

        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)

        {

            tbPassword.UseSystemPasswordChar = !checkBox1.Checked;

            checkBox1.Text = checkBox1.Checked ? "Hiện mật khẩu" : "Hiện mật khẩu";

        }

    }

}