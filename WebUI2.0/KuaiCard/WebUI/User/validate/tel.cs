namespace KuaiCard.WebUI.User.validate
{
    using KuaiCard.WebComponents.Web;
    using System;

    public class tel : UserPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.currentUser.IsPhonePass == 1)
            {
                base.AlertAndRedirect("手机已经认证", "/User/");
            }
        }
    }
}

