namespace KuaiCard.WebUI.User.IP
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class delete : UserPageBase
    {
        protected string gID;
        protected string gDeleteIP;
        protected string gUserEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string id = KuaiCardLib.Web.WebBase.GetQueryStringString("id", "");
                string ip = KuaiCardLib.Web.WebBase.GetQueryStringString("ip", "");

                if (id == "" || ip == "")
                {
                    this.Response.Redirect("~/user/ip/List.aspx");
                }

                id = KuaiCardLib.Security.Cryptography.DESDecryptString(id, "aywl");
                ip = KuaiCardLib.Security.Cryptography.DESDecryptString(ip, "aywl");

                if (id == "" || ip == "")
                {
                    this.Response.Redirect("~/user/ip/List.aspx");
                }

                this.gID = id;
                this.gDeleteIP = ip;

                this.gUserEmail = base.currentUser.Email;
            }
        }
    }
}

