namespace OriginalStudio.WebUI.User.IP
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
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
                string id = OriginalStudio.Lib.Web.WebBase.GetQueryStringString("id", "");
                string ip = OriginalStudio.Lib.Web.WebBase.GetQueryStringString("ip", "");

                if (id == "" || ip == "")
                {
                    this.Response.Redirect("~/user/ip/List.aspx");
                }

                id = OriginalStudio.Lib.Security.Cryptography.DESDecryptString(id, "aywl");
                ip = OriginalStudio.Lib.Security.Cryptography.DESDecryptString(ip, "aywl");

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

