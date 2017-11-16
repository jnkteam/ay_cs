namespace KuaiCard.WebUI.User.verify
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web;

    public class addbank : UserPageBase
    {
        protected string name = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (!base.IsPostBack)
            {
                if (base.currentUser.IsEmailPass != 1)
                {
                    HttpContext.Current.Response.Redirect("/user/Page.aspx?name=email");
                }
                else if (base.currentUser.IsPhonePass != 1)
                {
                    HttpContext.Current.Response.Redirect("/user/Page.aspx?name=Phone");
                }
                else if (base.currentUser.IsRealNamePass != 1)
                {
                    HttpContext.Current.Response.Redirect("/user/Page.aspx?name=Name");
                }
                else if (base.currentUser.Password2 == "")
                {
                    HttpContext.Current.Response.Redirect("/user/Page.aspx?name=password");
                }
                else
                {
                    this.name = base.currentUser.full_name;
                }
            }*/
        }
    }
}

