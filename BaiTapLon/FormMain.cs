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
    public partial class FormMain : Form
    {
        FormDiemSV fDiemSV;
        FormGV fGV;
        f fHP;
        FormSV fSV;
        FormAccout fAc;
        FormKhenThuong fKT;
        FormLoginLog fLoginLog;
        private Dictionary<Panel, Form> menuForms;
        private Panel currentSelectedPanel;
        

        public FormMain()
        {
            InitializeComponent();

            fDiemSV = new FormDiemSV();
            fGV = new FormGV();
            fHP = new f(); // Bạn nên đặt lại tên class từ 'f' thành 'FormHP' cho rõ
            fSV = new FormSV();
            fAc = new FormAccout();
            fKT = new FormKhenThuong();
            fLoginLog = new FormLoginLog();

            menuForms = new Dictionary<Panel, Form>()
                {
                    { panelDiem, fDiemSV },
                    { panelGV, fGV },
                    { panelMH, fHP },
                    { panelSV, fSV },
                   // { panelSV, fLoginLog },
                };

            foreach (var f in new Form[] { fDiemSV, fGV, fHP, fSV, fAc, fKT, fLoginLog })
            {
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
            }
            setAllItemMenuLeftColorDefault();
            hideAllForm();


            ResetMenuColor();
            HideAllForms();
            panelDiem.BackColor = Color.FromArgb(240, 240, 240);
            panelGV.BackColor = Color.FromArgb(240, 240, 240);
            panelMH.BackColor = Color.FromArgb(240, 240, 240);
            panelSV.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void MenuPanel_Click(object sender, EventArgs e)
        {
            var clickedPanel = sender as Panel;
            if (clickedPanel == null || !menuForms.ContainsKey(clickedPanel)) return;

            ResetMenuColor();
            clickedPanel.BackColor = Color.FromArgb(4, 107, 120);
            currentSelectedPanel = clickedPanel;

            HideAllForms();
            menuForms[clickedPanel].Show();
            panelDiem.Click += MenuPanel_Click;
            panelGV.Click += MenuPanel_Click;
            panelMH.Click += MenuPanel_Click;
            panelSV.Click += MenuPanel_Click;

        }
        private void MenuPanel_Hover(object sender, EventArgs e)
        {
            var p = sender as Panel;
            if (p != null && p != currentSelectedPanel)
                p.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void MenuPanel_Leave(object sender, EventArgs e)
        {
            var p = sender as Panel;
            if (p != null && p != currentSelectedPanel)
                p.BackColor = Color.FromArgb(255, 128, 0);
        }
        private void HideAllForms()
        {
            foreach (var f in menuForms.Values.Append(fAc).Append(fKT))
                f.Hide();
        }

        private void ResetMenuColor()
        {
            foreach (var p in menuForms.Keys)
                p.BackColor = Color.FromArgb(255, 128, 0);
            currentSelectedPanel = null;
        }

        private void hideAllForm()
        {
            fDiemSV.Hide();
            fGV.Hide();
            fSV.Hide();
            fHP.Hide();
            fAc.Hide();
            fKT.Hide();
            fLoginLog.Hide();

        }

        private void setAllItemMenuLeftColorDefault()
        {
            panelDiem.BackColor = Color.FromArgb(240, 240, 240);
            panelGV.BackColor = Color.FromArgb(240, 240, 240);
            panelMH.BackColor = Color.FromArgb(240, 240, 240);
            panelSV.BackColor = Color.FromArgb(240, 240, 240);



            itemMenuDiemClicked = false;
            itemMenuGVClicked = false;
            itemMenuMonHocClicked = false;
            itemMenuSVClicked = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            this.FormBorderStyle = FormBorderStyle.Sizable; // Ẩn viền & thanh tiêu đề
            this.WindowState = FormWindowState.Maximized; // Full màn hình

            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PanelDiem_Hover(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.FromArgb(224, 238, 224);
        }

        private void PanelDiem_Leave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
                ((Panel)sender).BackColor = Color.FromArgb(240, 240, 240);
        }
        bool itemMenuDiemClicked, itemMenuGVClicked, itemMenuMonHocClicked, itemMenuSVClicked;

        private void panelGV_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelGV.BackColor = Color.FromArgb(224, 238, 224);
            hideAllForm();
            itemMenuDiemClicked = true;
            this.panelngu.Visible = false;



            fGV.Show();
        }

        private void ItemGV_MouseHover(object sender, EventArgs e)
        {
            panelGV.BackColor = Color.FromArgb(224, 238, 224);
        }

        private void ItemGV_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelGV.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void PanelMH_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelMH.BackColor = Color.FromArgb(224, 238, 224);
            hideAllForm();
            itemMenuDiemClicked = true;

            this.panelngu.Visible = false;
            

            fHP.Show();
        }

        private void ItemMH_MouseHover(object sender, EventArgs e)
        {
            panelMH.BackColor = Color.FromArgb(224, 238, 224);
        }

        private void ItemMH_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelMH.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void PanelSV_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelSV.BackColor = Color.FromArgb(224, 238, 224);
            hideAllForm();
            itemMenuDiemClicked = true;
            this.panelngu.Visible = false;


            fSV.Show();
        }

        private void ItemSV_MouseHover(object sender, EventArgs e)
        {
            panelSV.BackColor = Color.FromArgb(224, 238, 224);
        }

        private void PanelDiem_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelDiem.BackColor = Color.FromArgb(224, 238, 224);
            hideAllForm();
            itemMenuDiemClicked = true;
            this.panelngu.Visible = false;

            fDiemSV.Show();

            
        }

        private void tạoTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelDiem.BackColor = Color.FromArgb(4, 107, 120);
            hideAllForm();
            itemMenuDiemClicked = true;
            this.panelngu.Visible = false;

            fAc.Show();

        }

        private void panelDiem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel12_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panelSV_Paint(object sender, PaintEventArgs e)
        {

        }

        private void KT_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            
            hideAllForm();
            itemMenuDiemClicked = true;
            this.panelngu.Visible = false;

            fKT.Show();
        }

        private void ccToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();

            hideAllForm();
            itemMenuDiemClicked = true;
            this.panelngu.Visible = false;
            fLoginLog.Show();
        }

        

        private void ItemSV_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelSV.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void ItemDiem_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false);
            
        }

        private void ItemDiem_MouseHover(object sender, EventArgs e)
        {
            panelDiem.BackColor = Color.FromArgb(224, 238, 224);
        }
        
    }
}
