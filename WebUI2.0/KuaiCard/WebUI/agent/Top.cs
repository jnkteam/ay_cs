namespace KuaiCard.WebUI.agent
{
    using OriginalStudio.WebComponents.Web;
    using System;

    public class Top : AgentPageBase
    {
        protected string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.username = base.currentUser.UserName;
            }
        }
    }
}

