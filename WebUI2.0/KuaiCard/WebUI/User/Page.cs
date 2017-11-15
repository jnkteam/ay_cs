namespace KuaiCard.WebUI.User
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Page : System.Web.UI.Page
    {
        protected HtmlForm form1;
        protected Label textprompt;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = HttpContext.Current.Request.QueryString["name"];
            if (str == "email")
            {
                this.textprompt.Text = "您还没有认证邮箱,请完成邮箱认证！";
            }
            else if (str == "Phone")
            {
                this.textprompt.Text = "您还没有认证手机,请完成手机认证！";
            }
            else if (str == "Name")
            {
                this.textprompt.Text = "您还没有实名认证,请完成实名认证！";
            }
            else if (str == "password")
            {
                this.textprompt.Text = "您还没有设置提现密码,请设置提现密码！";
            }
            else
            {
                this.textprompt.Text = "来路异常！";
            }
        }
    }
}

