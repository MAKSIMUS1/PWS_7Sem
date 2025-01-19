using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplexWinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculateSum_Click(object sender, EventArgs e)
        {
            int x = int.Parse(txtK1.Text);
            int y = int.Parse(txtK2.Text);

            Simplex service = new Simplex();

            int result = service.Add(x, y);
            lblResult.Text = "Результат: " + result;
        }
    }
}
