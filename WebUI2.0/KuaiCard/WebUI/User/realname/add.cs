namespace KuaiCard.WebUI.User.realname
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Settled;
    using KuaiCard.Model;
    using KuaiCard.Model.Settled;
    using KuaiCard.WebComponents.Web;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class add : UserPageBase
    {
        public string userid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userid = base.currentUser.ID.ToString();
        }
    }
}

