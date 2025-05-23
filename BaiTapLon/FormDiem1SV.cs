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
    public partial class FormDiem1SV : Form
    {
        string MaSV;
        string constr = Connecting.GetConnectionString();
        public FormDiem1SV(string MaSV)
        {
            InitializeComponent();
            this.MaSV = MaSV;
            LoadDiemSinhVien();
        }
        private string maSV;

        private void LoadDiemSinhVien()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetDiemSinhVien", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaSV", MaSV);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtDiem = new DataTable();
                            adapter.Fill(dtDiem);
                            dataGridSV.DataSource = dtDiem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormDiem1SV_Load(object sender, EventArgs e)
        {

        }
    }
}