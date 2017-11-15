namespace KuaiCard.WebUI.User.withdrawal
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Settled;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.Settled;
    using KuaiCard.WebComponents.Web;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using KuaiCard.Model.User;

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

