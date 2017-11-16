using System;
using System.Collections.Generic;
using System.Text;
using OriginalStudio.WebComponents.Web;
using System.Web.UI.WebControls;

namespace OriginalStudio.WebUI.User.Forget
{
    public class Account : PageBase
    {
        protected TextBox username;
        protected TextBox imgcode;
        protected Button btnNext;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string userName = this.username.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                this.AlertAndRedirect("用户名不能为空!");
                return;
            }

            string imgCode = this.imgcode.Text.Trim();
            //支付通 加上 验证码图片模式
            if ((this.Session["CCode"] == null) || (imgCode == null))
            {
                this.AlertAndRedirect("验证码失效!");
                return;
            }
            else if (this.Session["CCode"].ToString().ToUpper() != imgCode.ToUpper())
            {
                this.AlertAndRedirect("验证码不正确!");
                return;
            }

            //记录找回用户名
            string ens = OriginalStudio.Lib.Security.Cryptography.DESEncryptString(userName, "aywl");
            //this.Session["forget_username"] = userName;

            this.Response.Redirect("selectmode.aspx?us=" + ens);
        }
    }
}
