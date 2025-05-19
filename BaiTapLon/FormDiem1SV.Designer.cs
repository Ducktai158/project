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
            this.dataGridSV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSV)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridSV
            // 
            this.dataGridSV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSV.Location = new System.Drawing.Point(0, 0);
            this.dataGridSV.Name = "dataGridSV";
            this.dataGridSV.RowHeadersWidth = 51;
            this.dataGridSV.RowTemplate.Height = 24;
            this.dataGridSV.Size = new System.Drawing.Size(1017, 454);
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

        private System.Windows.Forms.DataGridView dataGridSV;
    }
}