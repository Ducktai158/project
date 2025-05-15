using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbUserName.Text.Trim().ToLower() == "admin" && tbPassword.Text.Trim().ToLower() == "admin") {
                FormMain formMain = new FormMain();
                this.Hide();
                formMain.ShowDialog();
                tbPassword.Text = "";
                tbUserName.Text = "User name"; tbUserName.ForeColor = Color.Gray;
                this.Show();
            }
            else if (tbUserName.Text.Trim().ToLower() == "giaovien" && tbPassword.Text.Trim().ToLower() == "giaovien")
            {
                FormDiemSV formDiemSV = new FormDiemSV();
                this.Hide();
                formDiemSV.ShowDialog();
                tbPassword.Text = "";
                tbUserName.Text = "User name"; tbUserName.ForeColor = Color.Gray;
                this.Show();

            }   
            else if (tbUserName.Text.Trim().ToLower() == "hocvien" && tbPassword.Text.Trim().ToLower() == "hocvien")
            {
                FormDiem1SV formDiem1SV = new FormDiem1SV();
                this.Hide();
                formDiem1SV.ShowDialog();
                tbPassword.Text = "";
                tbUserName.Text = "User name"; tbUserName.ForeColor = Color.Gray;
                this.Show();
            }    
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.");
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
