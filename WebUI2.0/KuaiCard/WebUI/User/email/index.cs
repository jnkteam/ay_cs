namespace OriginalStudio.WebUI.User.email
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
            gUserEmail = OriginalStudio.Lib.Text.Strings.Mark(base.CurrentUser.Email);
            gEmailPass = base.CurrentUser.IsEmailPass == 1;
        }
    }
}

