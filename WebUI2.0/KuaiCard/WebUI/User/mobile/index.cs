namespace KuaiCard.WebUI.User.mobile
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class index : UserPageBase
    {
        protected string gUserTel;

        protected void Page_Load(object sender, EventArgs e)
        {
            gUserTel = Strings.Mark(base.currentUser.Tel);
        }
    }
}

