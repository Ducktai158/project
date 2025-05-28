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
    public partial class Form1GV : Form
    {
        string constr = Connecting.GetConnectionString();
        string MaGV;

        private string _selectedMaLop;
        private string _selectedMaGV, _selectedMaHP, _selectedMaHK, _selectedMaNH;

        public Form1GV(string maGV)
        {
            InitializeComponent();
            this.MaGV = maGV;
            dataGridGV1.Visible = false;
            LoadLopGV();
           ; // Call this to populate your ComboBoxes when the form loads
            refresh(); // Initialize button states and clear fields
        }

        private void LoadLopGV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetLopGiangVien", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaGV", MaGV);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dtDiem = new DataTable();
                            adapter.Fill(dtDiem);
                            dataGridGV.DataSource = dtDiem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1GV_Load(object sender, EventArgs e)
        {
            dataGridGV1.AllowUserToAddRows = false;
            TTpanel2.Enabled = false;
            // Any specific actions on form load can go here
            cbMaSV.Enabled = false;
            cbMaHP.Enabled = false;
            cbHK.Enabled = false;
            cbNH.Enabled = false;
            tbDiemTK.Enabled = false;
        }

        private void dgvLop_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedMaLop = dataGridGV.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
                _selectedMaGV = dataGridGV.Rows[e.RowIndex].Cells["MaGV"].Value.ToString();
                _selectedMaHP = dataGridGV.Rows[e.RowIndex].Cells["MaHP"].Value.ToString();
                _selectedMaHK = dataGridGV.Rows[e.RowIndex].Cells["HocKy"].Value.ToString();
                _selectedMaNH = dataGridGV.Rows[e.RowIndex].Cells["NamHoc"].Value.ToString();



                LoadSinhVienVaDiem(_selectedMaLop, _selectedMaGV, _selectedMaHP);
                LoadComboBoxData();
                TTpanel2.Enabled = true;
                dataGridGV.Visible = false;
                dataGridGV1.Visible = true;
                refresh(); // Clear input fields and enable/disable buttons appropriately
            }
        }

        private void LoadSinhVienVaDiem(string maLop, string maGV, String maHP)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("LoadSinhVienVaDiemTheoLop", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaLop", maLop);
                cmd.Parameters.AddWithValue("@MaGV", maGV);
                cmd.Parameters.AddWithValue("@MaHP", maHP);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridGV1.DataSource = dt; // Display in the second DataGridView
            }
        }

        private void QuayLai_Click(object sender, EventArgs e)
        {
            dataGridGV1.Visible = false;
            dataGridGV.Visible = true;
            TTpanel2.Enabled=false;
            LoadLopGV();
            refresh(); // Ensure consistent state when returning to the main grid
        }

        // --- Removed btnCapNhat_Click method ---

        int rowSelected = -1; // This variable will store the index of the currently selected row in dataGridGV1
        private void dvgirdGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridGV1.CurrentRow != null)
            {
                // Ensure we don't try to read from a new, empty row if allow user to add new rows
                if (dataGridGV1.Rows[e.RowIndex].Cells["MaSV"].Value != null && !string.IsNullOrEmpty(dataGridGV1.Rows[e.RowIndex].Cells["MaSV"].Value.ToString()))
                {
                    cbMaSV.Text = dataGridGV1.Rows[e.RowIndex].Cells["MaSV"].Value.ToString().Trim();
                    cbMaSV.Enabled = false; // Disable to prevent changing existing record's key

                    cbMaHP.Text = dataGridGV1.Rows[e.RowIndex].Cells["MaHP"].Value.ToString().Trim();
                    cbMaHP.Enabled = false; // Disable to prevent changing existing record's key

                    // Populate other fields
                    tbSoTC.Text = dataGridGV1.Rows[e.RowIndex].Cells["SoTC"].Value?.ToString().Trim() ?? "0"; // Use null-conditional and null-coalescing
                    tbDiemTX.Text = dataGridGV1.Rows[e.RowIndex].Cells["ThuongXuyen"].Value?.ToString().Trim() ?? "0";
                    tbDiemCC.Text = dataGridGV1.Rows[e.RowIndex].Cells["ChuyenCan"].Value?.ToString().Trim() ?? "0";
                    tbDiemGHP.Text = dataGridGV1.Rows[e.RowIndex].Cells["GHP"].Value?.ToString().Trim() ?? "0";
                    tbDiemThi.Text = dataGridGV1.Rows[e.RowIndex].Cells["CHP"].Value?.ToString().Trim() ?? "0";
                    tbDiemTK.Text = dataGridGV1.Rows[e.RowIndex].Cells["TongKet"].Value?.ToString().Trim() ?? "0";
                    cbHK.Text = dataGridGV1.Rows[e.RowIndex].Cells["HocKy"].Value?.ToString().Trim();
                    cbNH.Text = dataGridGV1.Rows[e.RowIndex].Cells["NamHoc"].Value?.ToString().Trim();

                    rowSelected = e.RowIndex; // Update the selected row index

                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    btnThem.Enabled = false; // Disable Add when editing
                    cbNH.Enabled = false;
                    cbHK.Enabled = false;
                }
                else
                {
                    refresh(); // Reset fields if an empty row (for new entry) is clicked
                }
            }
        }

        private void refresh()
        {
            // Clear all textboxes
            cbMaHP.ResetText();
            cbMaSV.ResetText();
            tbSoTC.Text = "";
            tbDiemThi.Text = "0";
            tbDiemTX.Text = "0";
            tbDiemCC.Text = "0";
            tbDiemTK.Text = "0";
            tbDiemGHP.Text = "0";
            cbHK.SelectedIndex = -1; // Clear selected item
            cbNH.SelectedIndex = -1; // Clear selected item

            tbDiemTK.Enabled = false;
            // Reset button states
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = false;

            // Re-enable ComboBoxes for new entry
            cbMaSV.Enabled = true;
            cbMaHP.Enabled = true;
            cbNH.Enabled   = true;
            cbHK.Enabled = true;

            rowSelected = -1; // No row selected
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSV = cbMaSV.Text.Trim();
            string maHP = cbMaHP.Text.Trim();
            string hocKy = cbHK.SelectedItem?.ToString();
            string namHoc = cbNH.SelectedItem?.ToString();

            // 1. Basic input validation
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maHP) || string.IsNullOrEmpty(hocKy) || string.IsNullOrEmpty(namHoc))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SV, Mã HP, Học Kỳ và Năm Học.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double diemCC, diemTX, diemGHP, diemCHP;

            // 2. Validate numeric inputs
            if (!double.TryParse(tbDiemCC.Text, out diemCC) ||
                !double.TryParse(tbDiemTX.Text, out diemTX) ||
                !double.TryParse(tbDiemGHP.Text, out diemGHP) ||
                !double.TryParse(tbDiemThi.Text, out diemCHP))
            {
                MessageBox.Show("Điểm Chuyên Cần, Thường Xuyên, Giữa Học Phần và Cuối Học Phần phải là số.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validate score ranges (assuming 0-10)
            if (diemCC < 0 || diemCC > 10 ||
                diemTX < 0 || diemTX > 10 ||
                diemGHP < 0 || diemGHP > 10 ||
                diemCHP < 0 || diemCHP > 10)
            {
                MessageBox.Show("Điểm phải nằm trong khoảng từ 0 đến 10.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                double diemTK = (diemCC * 0.1) + (diemTX * 0.1) + (diemGHP * 0.2) + (diemCHP * 0.6);
                diemTK = Math.Round(diemTK, 1); // Round to 1 decimal place

                tbDiemTK.Text = diemTK.ToString("0.0"); // Display with one decimal place

                // Add data to the database
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // Check if the record already exists (MaSV, MaHP, HocKy, NamHoc should be unique together)
                    string checkQuery = "SELECT COUNT(*) FROM KetQuaHocPhan WHERE MaSV = @MaSV AND MaHP = @MaHP AND HocKy = @HocKy AND NamHoc = @NamHoc";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                        checkCmd.Parameters.AddWithValue("@MaHP", maHP);
                        checkCmd.Parameters.AddWithValue("@HocKy", hocKy);
                        checkCmd.Parameters.AddWithValue("@NamHoc", namHoc);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Sinh viên đã có điểm cho học phần này trong học kỳ và năm học đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    // Insert new score
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
                        cmd.Parameters.AddWithValue("@HocKy", hocKy);
                        cmd.Parameters.AddWithValue("@NamHoc", namHoc);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Thêm điểm thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Only reload if _selectedMaLop and _selectedMaGV are available
                            LoadSinhVienVaDiem(_selectedMaLop, _selectedMaGV, _selectedMaHP);
                            refresh(); // Clear fields after successful addition
                        }
                        else
                        {
                            MessageBox.Show("Thêm điểm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxData()
        {
            if (string.IsNullOrEmpty(_selectedMaLop) || string.IsNullOrEmpty(_selectedMaHP))
            {
                // Không có lớp hoặc học phần được chọn, không cần load
                return;
            }

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // 1. Load sinh viên thuộc lớp được chọn
                string querySV = "SELECT MaSV FROM SinhVien WHERE MaLop = @MaLop";
                using (SqlCommand cmd = new SqlCommand(querySV, conn))
                {
                    cmd.Parameters.AddWithValue("@MaLop", _selectedMaLop);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dtSV = new DataTable();
                        dtSV.Load(reader);
                        cbMaSV.DataSource = dtSV;
                        cbMaSV.DisplayMember = "MaSV";
                        cbMaSV.ValueMember = "MaSV";
                    }
                }

                // 2. Gán trực tiếp mã học phần (đã chọn từ danh sách lớp)
                cbMaHP.DataSource = null;
                cbMaHP.Items.Clear();
                cbMaHP.Items.Add(_selectedMaHP);
                cbMaHP.SelectedIndex = 0;

                // Populate NamHoc (e.g., current year +/- a few, or from DB)
                cbHK.DataSource = null;
                cbHK.Items.Clear();
                cbHK.Items.Add(_selectedMaHK);
                cbHK.SelectedIndex = 0;

                cbNH.DataSource = null;
                cbNH.Items.Clear();
                cbNH.Items.Add(_selectedMaNH);
                cbNH.SelectedIndex = 0;
            }
        }

        //--- Sửa (Edit) Button Implementation ---
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
                double diemCC = Math.Round(double.Parse(tbDiemCC.Text), 1);
                double diemTX = Math.Round(double.Parse(tbDiemTX.Text), 1);
                double diemGHP = Math.Round(double.Parse(tbDiemGHP.Text), 1);
                double diemCHP = Math.Round(double.Parse(tbDiemThi.Text), 1);
                double diemTK = (diemCC * 0.1) + (diemTX * 0.1) + (diemGHP * 0.2) + (diemCHP * 0.6);
                diemTK = Math.Round(diemTK, 1); // Làm tròn đến 1 chữ số thập phân
                tbDiemTK.Text = diemTK.ToString("0.0"); // Hiển thị 1 số sau dấu chấm
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string updateQuery = @"UPDATE KetQuaHocPhan
                                   SET ChuyenCan = @CC, ThuongXuyen = @TX, GHP = @GHP, CHP = @CHP, TongKet = @TongKet
                                   WHERE MaSV = @MaSV AND MaHP = @MaHP";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CC", diemCC);
                        cmd.Parameters.AddWithValue("@TX", diemTX);
                        cmd.Parameters.AddWithValue("@GHP", diemGHP);
                        cmd.Parameters.AddWithValue("@CHP", diemCHP);
                        cmd.Parameters.AddWithValue("@TongKet", diemTK);
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@MaHP", maHP);
                        cmd.Parameters.AddWithValue("@HocKy", cbHK.SelectedItem.ToString()); // Sửa lại nếu có chọn học kỳ
                        cmd.Parameters.AddWithValue("@NamHoc", cbNH.SelectedItem.ToString()); // Sửa lại nếu có chọn năm học

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật điểm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSinhVienVaDiem(_selectedMaLop,_selectedMaGV,_selectedMaHP);
                            refresh();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy điểm để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }

            }

            catch (FormatException)

            {

                MessageBox.Show("Vui lòng nhập đúng định dạng điểm (số).", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                            LoadSinhVienVaDiem(_selectedMaLop,_selectedMaGV,_selectedMaHP);
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
    }
}