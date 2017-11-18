namespace OriginalStudio.WebUI.User.realname
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class add : UserPageBase
    {
        public string userid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userid = base.CurrentUser.ID.ToString();
        }
    }
}

