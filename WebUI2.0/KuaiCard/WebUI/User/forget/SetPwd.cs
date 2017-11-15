using System;
using System.Collections.Generic;
using System.Text;
using KuaiCard.WebComponents.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace KuaiCard.WebUI.User.Forget
{
    public class SetPwd : PageBase
    {
        protected HtmlInputText txtPwd1;
        protected HtmlInputText txtPwd2;
        protected HtmlInputText txtImgCode;
        protected Button btnNext;
        string gUserName = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string ens = KuaiCardLib.Web.WebBase.GetQueryStringString("us", "");
                if (ens == "")
                {
                    this.AlertAndRedirect("用户名为空!");
                    return;
                }
                gUserName = KuaiCardLib.Security.Cryptography.DESDecryptString(ens, "aywl");
                if (gUserName == "")
                {
                    this.AlertAndRedirect("用户名为空!");
                    return;
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string pwd1 = this.txtPwd1.Value.Trim();
            string pwd2 = this.txtPwd2.Value.Trim();
            if (string.IsNullOrEmpty(pwd1) || string.IsNullOrEmpty(pwd2))
            {
                this.AlertAndRedirect("密码不能为空!");
                return;
            }
            if (pwd1 != pwd2)
            {
                this.AlertAndRedirect("两遍密码不相同!");
                return;
            }

            string imgCode = this.txtImgCode.Value.Trim();
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

            //更新密码
            KuaiCard.BLL.User.UserFactory.ChangeUserPassword(this.gUserName, this.txtPwd1.Value.Trim());
            this.AlertAndRedirect("密码修改成功!");
            //跳转
            this.Response.Redirect("success.html");
        }
    }
}
