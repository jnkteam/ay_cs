using System;
using System.Collections.Generic;
using System.Text;
using OriginalStudio.WebComponents.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KuaiCard.WebUI.Manage.Channel
{
	public class Tfb :  ManagePageBase
	{
        protected HtmlForm Form1;
        protected Button btnGetBalance;

        protected string 可用余额 = "";
        protected string 垫资限额 = "";

        protected void btnGetBalance_Click(object sender, EventArgs e)
        {
        }

	}
}
