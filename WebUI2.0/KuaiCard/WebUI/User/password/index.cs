namespace KuaiCard.WebUI.User.password
{
    using KuaiCard.WebComponents.Web;
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

