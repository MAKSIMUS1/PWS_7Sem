using SimplexClient.ServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SimplexClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new SimplexSoapClient();

                var a1 = new A
                {
                    S = textBoxS1.Text,
                    K = int.Parse(textBoxK1.Text),
                    F = double.Parse(textBoxF1.Text)
                };
                var a2 = new A
                {
                    S = textBoxS2.Text,
                    K = int.Parse(textBoxK2.Text),
                    F = double.Parse(textBoxF2.Text)
                };

                var result = client.Sum(a1, a2);

                resultLabel.Text = $"s: {result.S}, k: {result.K}, f: {result.F}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
