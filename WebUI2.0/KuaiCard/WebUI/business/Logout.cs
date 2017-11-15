namespace KuaiCard.WebUI.Business
{
    using KuaiCard.BLL;
    using System;
    using System.Web;
    using System.Web.UI;

    public class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ManageFactory.SignOut();
            string s = string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\ntop.location.href=\"{0}\";\r\n//--></SCRIPT>", "/business/Login.aspx");
            HttpContext.Current.Response.Write(s);
        }
    }
}

