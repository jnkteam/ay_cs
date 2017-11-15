namespace KuaiCard.WebUI.User.email
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class index_old : UserPageBase
    {
        protected Label useremail;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.useremail.Text = base.currentUser.Email;
            if (base.IsPostBack)
            {
                string formString = WebBase.GetFormString("a", "");
                this.Session["codeSession"] = formString;
                HttpContext.Current.Response.Redirect("/User/validate/");
            }
        }
    }
}

