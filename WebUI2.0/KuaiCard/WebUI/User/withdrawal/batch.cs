namespace OriginalStudio.WebUI.User.withdrawal
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using OriginalStudio.Model.User;

    public class batch : UserPageBase
    {
        protected bool PhoneValid = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PhoneValid = base.currentUser.IsPhonePass == 1;

            this.DataBind();
        }

    }
}

