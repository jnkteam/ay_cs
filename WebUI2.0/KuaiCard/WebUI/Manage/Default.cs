namespace KuaiCard.WebUI.Manage
{
    using System;
    using System.Web.UI;
    using KuaiCard.WebComponents.Web;

    public class Default : ManagePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.DefaultThemes = this.currentManage.DefaultThemes;
            this.DataBind();
        }
    }
}

