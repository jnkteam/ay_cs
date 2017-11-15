namespace KuaiCard.WebUI.User.mobile
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class index_old : UserPageBase
    {
        protected Label usertel;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.usertel.Text = Strings.Mark(base.currentUser.Tel);
            if (base.IsPostBack)
            {
                string formString = WebBase.GetFormString("a", "");
                this.Session["codeSession"] = "mobilecode";
                HttpContext.Current.Response.Redirect("/User/validate/");
            }
        }
    }
}

