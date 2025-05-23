namespace BaiTapLon
{
    partial class FormDiem1SV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridSV = new BaiTapLon.CustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSV)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridSV
            // 
            this.dataGridSV.AllowUserToAddRows = false;
            this.dataGridSV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridSV.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridSV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridSV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridSV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridSV.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridSV.EnableHeadersVisualStyles = false;
            this.dataGridSV.GridColor = System.Drawing.Color.LightGray;
            this.dataGridSV.Location = new System.Drawing.Point(2, 1);
            this.dataGridSV.MultiSelect = false;
            this.dataGridSV.Name = "dataGridSV";
            this.dataGridSV.ReadOnly = true;
            this.dataGridSV.RowHeadersWidth = 51;
            this.dataGridSV.RowTemplate.Height = 30;
            this.dataGridSV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridSV.Size = new System.Drawing.Size(1013, 449);
            this.dataGridSV.TabIndex = 0;
            // 
            // FormDiem1SV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 450);
            this.Controls.Add(this.dataGridSV);
            this.Name = "FormDiem1SV";
            this.Text = "Bảng điểm sinh viên";
            this.Load += new System.EventHandler(this.FormDiem1SV_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomDataGridView dataGridSV;
    }
}