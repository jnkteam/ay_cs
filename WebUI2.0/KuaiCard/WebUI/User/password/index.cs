namespace OriginalStudio.WebUI.User.password
{
    using OriginalStudio.WebComponents.Web;
    using System;

    public class index : UserPageBase
    {
        protected string id = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.id = base.currentUser.ID.ToString();
        }
    }
}

