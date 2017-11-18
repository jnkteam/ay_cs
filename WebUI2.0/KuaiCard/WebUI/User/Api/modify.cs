namespace OriginalStudio.WebUI.User.Api
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class modify : UserPageBase
    {
        protected string gUserID = "";
        protected string gUserKey = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            gUserID = base.CurrentUser.ID.ToString();
            gUserKey = base.CurrentUser.APIKey;

        }
    }
}

