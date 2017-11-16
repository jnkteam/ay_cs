namespace KuaiCard.WebUI.User.Api
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected Label userid;
        protected Label userkey;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userid.Text = base.currentUser.ID.ToString();
            this.userkey.Text = base.currentUser.APIKey;
        }
    }
}

