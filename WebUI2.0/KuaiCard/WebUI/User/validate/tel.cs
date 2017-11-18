namespace OriginalStudio.WebUI.User.validate
{
    using OriginalStudio.WebComponents.Web;
    using System;

    public class tel : UserPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.CurrentUser.IsPhonePass == 1)
            {
                base.AlertAndRedirect("手机已经认证", "/User/");
            }
        }
    }
}

