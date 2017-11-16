namespace KuaiCard.WebUI.User.Recharge
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.Security;

    public class notify : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //只返回这个，充值成功
            this.Response.Write("opstate=0");
        }
    }
}

