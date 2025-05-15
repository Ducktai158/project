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
    public partial class FormHP : Form
    {
        public FormHP()
        {
            InitializeComponent();
        }

        private void FormHP_Load(object sender, EventArgs e)
        {

        }

        private void tbTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            this.tbTimKiemTheoTen.Text = "";
        }
    }
}
