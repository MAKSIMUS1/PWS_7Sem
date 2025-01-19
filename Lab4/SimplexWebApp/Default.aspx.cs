using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimplexWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int x = int.Parse(txtNumber1.Text);
            int y = int.Parse(txtNumber2.Text);

            // Создаем экземпляр proxy-класса
            Simplex service = new Simplex();

            // Вызываем метод Add и отображаем результат
            int result = service.Add(x, y);
            lblResult.Text = "Результат: " + result;
        }
    }
}