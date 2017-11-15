namespace KuaiCard.WebUI.User
{
    using KuaiCard.BLL.User;
    using System;
    using System.Web;
    using System.Web.UI;

    public class loginout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserFactory.SignOut();
            string s = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\ntop.location.href=\"{0}\";\r\n//--></SCRIPT>", "/");
            HttpContext.Current.Response.Write(s);
        }
    }
}

