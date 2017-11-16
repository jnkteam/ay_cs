namespace KuaiCard.WebUI.User.email
{
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected string gUserEmail;
        protected bool gEmailPass = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            gUserEmail = KuaiCardLib.Text.Strings.Mark(base.currentUser.Email);
            gEmailPass = base.currentUser.IsEmailPass == 1;
        }
    }
}

