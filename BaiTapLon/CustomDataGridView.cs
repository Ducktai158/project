using System.Drawing;
using System.Windows.Forms;

namespace BaiTapLon // Thay bằng namespace thực tế của bạn
{
    public class CustomDataGridView : DataGridView
    {

        public CustomDataGridView()
        {
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            this.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.EnableHeadersVisualStyles = false;

            this.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            this.DefaultCellStyle.ForeColor = Color.Black;
            this.DefaultCellStyle.BackColor = Color.White;
            this.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            this.DefaultCellStyle.SelectionForeColor = Color.Black;

            this.GridColor = Color.LightGray;
            this.BackgroundColor = Color.WhiteSmoke;
            this.BorderStyle = BorderStyle.Fixed3D;

            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.RowTemplate.Height = 30;
            this.AllowUserToAddRows = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.MultiSelect = false;

            this.ReadOnly = true;
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomDataGridView
            // 
            this.RowTemplate.Height = 24;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
