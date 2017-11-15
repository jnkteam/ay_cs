using System;
using System.Collections.Generic;
using System.Text;
using KuaiCard.WebComponents.Web;
using System.Web.UI.WebControls;

namespace KuaiCard.WebUI.User.Forget
{
    public class SelectMode : PageBase
    {
        protected bool gHasName = false;
        protected string gUserName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string ens = KuaiCardLib.Web.WebBase.GetQueryStringString("us", "");
            if (ens == "")
            {
                this.AlertAndRedirect("用户名为空!");
                return;
            }
            gUserName = ens;

            gHasName = string.IsNullOrEmpty(this.gUserName) == false;
        }
    }
}
