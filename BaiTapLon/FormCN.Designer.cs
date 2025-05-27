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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboNH = new System.Windows.Forms.ComboBox();
            this.comboHK = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridGV = new BaiTapLon.CustomDataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnXDTK = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.btnXemdiemHK = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGV)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.btnXDTK.SuspendLayout();
            this.btnRefresh.SuspendLayout();
            this.btnXemdiemHK.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboNH
            // 
            this.comboNH.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboNH.FormattingEnabled = true;
            this.comboNH.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboNH.Location = new System.Drawing.Point(127, 131);
            this.comboNH.Name = "comboNH";
            this.comboNH.Size = new System.Drawing.Size(139, 31);
            this.comboNH.TabIndex = 46;
            // 
            // comboHK
            // 
            this.comboHK.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboHK.FormattingEnabled = true;
            this.comboHK.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboHK.Location = new System.Drawing.Point(127, 72);
            this.comboHK.Name = "comboHK";
            this.comboHK.Size = new System.Drawing.Size(139, 31);
            this.comboHK.TabIndex = 47;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Montserrat SemiBold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 24);
            this.label2.TabIndex = 44;
            this.label2.Text = "Năm học:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat SemiBold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 24);
            this.label1.TabIndex = 45;
            this.label1.Text = "Học kỳ:";
            // 
            // dataGridGV
            // 
            this.dataGridGV.AllowUserToAddRows = false;
            this.dataGridGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridGV.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridGV.EnableHeadersVisualStyles = false;
            this.dataGridGV.GridColor = System.Drawing.Color.LightGray;
            this.dataGridGV.Location = new System.Drawing.Point(295, 23);
            this.dataGridGV.MultiSelect = false;
            this.dataGridGV.Name = "dataGridGV";
            this.dataGridGV.ReadOnly = true;
            this.dataGridGV.RowHeadersWidth = 51;
            this.dataGridGV.RowTemplate.Height = 30;
            this.dataGridGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridGV.Size = new System.Drawing.Size(992, 472);
            this.dataGridGV.TabIndex = 48;
            this.dataGridGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvgCN_Cellclick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Controls.Add(this.btnXDTK);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.btnXemdiemHK);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboHK);
            this.groupBox1.Controls.Add(this.comboNH);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Green;
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 472);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "XEM ĐIỂM";
            // 
            // btnXDTK
            // 
            this.btnXDTK.BackColor = System.Drawing.Color.OliveDrab;
            this.btnXDTK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnXDTK.Controls.Add(this.label3);
            this.btnXDTK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXDTK.ForeColor = System.Drawing.Color.MistyRose;
            this.btnXDTK.Location = new System.Drawing.Point(38, 291);
            this.btnXDTK.Margin = new System.Windows.Forms.Padding(4);
            this.btnXDTK.Name = "btnXDTK";
            this.btnXDTK.Size = new System.Drawing.Size(199, 45);
            this.btnXDTK.TabIndex = 50;
            this.btnXDTK.Click += new System.EventHandler(this.btnXemDiem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Montserrat SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(22, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 27);
            this.label3.TabIndex = 23;
            this.label3.Text = "Xem Điểm TK";
            this.label3.Click += new System.EventHandler(this.btnXemDiem_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(109)))), ((int)(((byte)(226)))));
            this.btnRefresh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnRefresh.Controls.Add(this.label20);
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.ForeColor = System.Drawing.Color.MistyRose;
            this.btnRefresh.Location = new System.Drawing.Point(38, 378);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(199, 45);
            this.btnRefresh.TabIndex = 43;
            this.btnRefresh.Click += new System.EventHandler(this.lammoiClick);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label20.Font = new System.Drawing.Font("Montserrat SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(42, 8);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(103, 27);
            this.label20.TabIndex = 23;
            this.label20.Text = "Làm mới";
            this.label20.Click += new System.EventHandler(this.lammoiClick);
            // 
            // btnXemdiemHK
            // 
            this.btnXemdiemHK.BackColor = System.Drawing.Color.OliveDrab;
            this.btnXemdiemHK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnXemdiemHK.Controls.Add(this.label4);
            this.btnXemdiemHK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXemdiemHK.ForeColor = System.Drawing.Color.MistyRose;
            this.btnXemdiemHK.Location = new System.Drawing.Point(38, 206);
            this.btnXemdiemHK.Margin = new System.Windows.Forms.Padding(4);
            this.btnXemdiemHK.Name = "btnXemdiemHK";
            this.btnXemdiemHK.Size = new System.Drawing.Size(199, 45);
            this.btnXemdiemHK.TabIndex = 51;
            this.btnXemdiemHK.Click += new System.EventHandler(this.BtnXemDiemHk_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Montserrat SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(22, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 27);
            this.label4.TabIndex = 23;
            this.label4.Text = "Xem Điểm HK";
            this.label4.Click += new System.EventHandler(this.BtnXemDiemHk_Click);
            // 
            // FormCN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 507);
            this.Controls.Add(this.dataGridGV);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormCN";
            this.Text = "FormCN";
            this.Load += new System.EventHandler(this.FormCN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGV)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.btnXDTK.ResumeLayout(false);
            this.btnXDTK.PerformLayout();
            this.btnRefresh.ResumeLayout(false);
            this.btnRefresh.PerformLayout();
            this.btnXemdiemHK.ResumeLayout(false);
            this.btnXemdiemHK.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboNH;
        private System.Windows.Forms.ComboBox comboHK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private CustomDataGridView dataGridGV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel btnXDTK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel btnRefresh;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Panel btnXemdiemHK;
        private System.Windows.Forms.Label label4;
    }
}