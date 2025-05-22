using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormMain : Form
    {
        FormDiemSV fDiemSV;
        FormGV fGV;
        f fHP;
        FormSV fSV;
        FormAccout fAc;


        public FormMain()
        {
            InitializeComponent();
            fDiemSV = new FormDiemSV();
            fGV = new FormGV();
            fHP = new f();
            fSV = new FormSV();
            fAc = new FormAccout();

            foreach (var f in new Form[] { fDiemSV, fGV, fHP, fSV, fAc })
            {
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
            }
            setAllItemMenuLeftColorDefault();
            hideAllForm();
        }

        private void hideAllForm()
        {
            fDiemSV.Hide();
            fGV.Hide();
            fSV.Hide();
            fHP.Hide();
            fAc.Hide();
        }

        private void setAllItemMenuLeftColorDefault()
        {
            panelSV.BackColor = Color.FromArgb(255, 128, 0);
            panelMH.BackColor = Color.FromArgb(255, 128, 0);
            panelDiem.BackColor = Color.FromArgb(255, 128, 0);
            panelGV.BackColor = Color.FromArgb(255, 128, 0);
            

            itemMenuDiemClicked = false;
            itemMenuGVClicked = false;
            itemMenuMonHocClicked = false;
            itemMenuSVClicked = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            ((Panel)sender).BackColor = Color.FromArgb(8, 142, 158);
        }

        private void PanelDiem_Leave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
                ((Panel)sender).BackColor = Color.FromArgb(255, 128, 0);
        }
        bool itemMenuDiemClicked, itemMenuGVClicked, itemMenuMonHocClicked, itemMenuSVClicked;

        private void panelGV_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelGV.BackColor = Color.FromArgb(4, 107, 120);
            hideAllForm();
            itemMenuDiemClicked = true;
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = p5.Visible = p6.Visible = p7.Visible = p8.Visible = p9.Visible = p10.Visible = p11.Visible = false;

            fGV.Show();
        }

        private void ItemGV_MouseHover(object sender, EventArgs e)
        {
            panelGV.BackColor = Color.FromArgb(8, 142, 158);
        }

        private void ItemGV_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelGV.BackColor = Color.FromArgb(255, 128, 0);
        }

        private void PanelMH_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelMH.BackColor = Color.FromArgb(4, 107, 120);
            hideAllForm();
            itemMenuDiemClicked = true;
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = p5.Visible = p6.Visible = p7.Visible = p8.Visible = p9.Visible = p10.Visible = p11.Visible = false;


            fHP.Show();
        }

        private void ItemMH_MouseHover(object sender, EventArgs e)
        {
            panelMH.BackColor = Color.FromArgb(8, 142, 158);
        }

        private void ItemMH_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelMH.BackColor = Color.FromArgb(255, 128, 0 );
        }

        private void PanelSV_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelSV.BackColor = Color.FromArgb(4, 107, 120);
            hideAllForm();
            itemMenuDiemClicked = true;
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = p5.Visible = p6.Visible = p7.Visible = p8.Visible = p9.Visible = p10.Visible = p11.Visible = false;

            fSV.Show();
        }

        private void ItemSV_MouseHover(object sender, EventArgs e)
        {
            panelSV.BackColor = Color.FromArgb(8, 142, 158);
        }

        private void PanelDiem_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelDiem.BackColor = Color.FromArgb(4, 107, 120);
            hideAllForm();
            itemMenuDiemClicked = true;
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = p5.Visible = p6.Visible = p7.Visible = p8.Visible = p9.Visible = p10.Visible = p11.Visible = false;
            fDiemSV.Show();

            
        }

        private void tạoTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAllItemMenuLeftColorDefault();
            panelDiem.BackColor = Color.FromArgb(4, 107, 120);
            hideAllForm();
            itemMenuDiemClicked = true;
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = p5.Visible = p6.Visible = p7.Visible = p8.Visible = p9.Visible = p10.Visible = p11.Visible = false;
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

        private void ItemSV_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelSV.BackColor = Color.FromArgb(255, 128, 0);
        }

        private void ItemDiem_MouseLeave(object sender, EventArgs e)
        {
            if (itemMenuDiemClicked == false)
            panelDiem.BackColor = Color.FromArgb(255, 128, 0);
        }

        private void ItemDiem_MouseHover(object sender, EventArgs e)
        {
            panelDiem.BackColor = Color.FromArgb(8, 142, 158);
        }
    }
}
