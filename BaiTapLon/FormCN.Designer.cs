namespace BaiTapLon
{
    partial class FormCN
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
            this.comboNH = new System.Windows.Forms.ComboBox();
            this.comboHK = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.dataGridGV = new System.Windows.Forms.DataGridView();
            this.btnRefresh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGV)).BeginInit();
            this.SuspendLayout();
            // 
            // comboNH
            // 
            this.comboNH.FormattingEnabled = true;
            this.comboNH.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboNH.Location = new System.Drawing.Point(105, 126);
            this.comboNH.Name = "comboNH";
            this.comboNH.Size = new System.Drawing.Size(102, 24);
            this.comboNH.TabIndex = 46;
            // 
            // comboHK
            // 
            this.comboHK.FormattingEnabled = true;
            this.comboHK.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboHK.Location = new System.Drawing.Point(105, 67);
            this.comboHK.Name = "comboHK";
            this.comboHK.Size = new System.Drawing.Size(102, 24);
            this.comboHK.TabIndex = 47;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 44;
            this.label2.Text = "Năm học";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 45;
            this.label1.Text = "Học kỳ";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(109)))), ((int)(((byte)(226)))));
            this.btnRefresh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnRefresh.Controls.Add(this.label20);
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.ForeColor = System.Drawing.Color.MistyRose;
            this.btnRefresh.Location = new System.Drawing.Point(80, 184);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(127, 45);
            this.btnRefresh.TabIndex = 43;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label20.Font = new System.Drawing.Font("Montserrat Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(11, 7);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(111, 27);
            this.label20.TabIndex = 23;
            this.label20.Text = "Làm mới";
            // 
            // dataGridGV
            // 
            this.dataGridGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGV.Location = new System.Drawing.Point(230, 12);
            this.dataGridGV.Name = "dataGridGV";
            this.dataGridGV.RowHeadersWidth = 51;
            this.dataGridGV.RowTemplate.Height = 24;
            this.dataGridGV.Size = new System.Drawing.Size(1059, 483);
            this.dataGridGV.TabIndex = 39;
            this.dataGridGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvgCN_Cellclick);
            // 
            // FormCN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 507);
            this.Controls.Add(this.comboNH);
            this.Controls.Add(this.comboHK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dataGridGV);
            this.Name = "FormCN";
            this.Text = "FormCN";
            this.Load += new System.EventHandler(this.FormCN_Load);
            this.btnRefresh.ResumeLayout(false);
            this.btnRefresh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboNH;
        private System.Windows.Forms.ComboBox comboHK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel btnRefresh;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DataGridView dataGridGV;
    }
}