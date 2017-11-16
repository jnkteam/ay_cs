using System;
using System.Collections.Generic;
using System.Text;
using OriginalStudio.WebComponents.Web;
using System.Web.UI.WebControls;
using OriginalStudio.Cache;
using OriginalStudio.BLL.User;
using OriginalStudio.WebComponents;

namespace OriginalStudio.WebUI.User.Forget
{
    public class GetByMail : PageBase
    {
        protected string gUserEmail = "";
        protected TextBox txtValidCode;
        protected Button btnNext;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //记录找回用户名
            if (!IsPostBack)
            {
                string ens = OriginalStudio.Lib.Web.WebBase.GetQueryStringString("us", "");
                if (ens == "")
                {
                    this.AlertAndRedirect("用户名为空!");
                    this.Response.Redirect("~/User/forget/account.aspx");
                }
                string UserName = OriginalStudio.Lib.Security.Cryptography.DESDecryptString(ens, "aywl");
                if (UserName == "")
                {
                    this.AlertAndRedirect("用户名为空!");
                    this.Response.Redirect("~/User/forget/account.aspx");
                }

                OriginalStudio.Model.User.UserInfo currentUser = OriginalStudio.BLL.User.UserFactory.GetModelByName(UserName);
                if (currentUser == null || currentUser.UserName == "")
                {
                    this.AlertAndRedirect("用户信息不存在!");
                    this.Response.Redirect("~/User/forget/account.aspx");
                };

                gUserEmail = OriginalStudio.Lib.Text.Strings.Mark(currentUser.Email);

                if (true)       // if (currentUser.IsEmailPass == 1)
                {
                    string to = currentUser.Email;
                    string objId = "EMAIL_VALID_" + to;
                    string o = (string)WebCache.GetCacheService().RetrieveObject(objId);
                    if (o == null)
                    {
                        o = new Random().Next(0x2710, 0x1869f).ToString();
                        WebCache.GetCacheService().AddObject(objId, o);
                    }
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("<p>亲爱的{0}:<p>", currentUser.UserName);
                    builder.AppendFormat("<p style=\"font-size:14px\">您的验证码为：<font style=\"font-size:14px;font-weight:bold;color:blue\">{0}</font>，打死也不能告诉别人！</p>", o);
                    EmailHelper helper = new EmailHelper(string.Empty, to, to + "验证码", builder.ToString(), true, Encoding.GetEncoding("gbk"));
                    helper.Send();
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string input_code = this.txtValidCode.Text.Trim();
            if (string.IsNullOrEmpty(input_code))
            {
                this.AlertAndRedirect("验证码不能为空!");
                return;
            }

            string ens = OriginalStudio.Lib.Web.WebBase.GetQueryStringString("us", "");
            string UserName = OriginalStudio.Lib.Security.Cryptography.DESDecryptString(ens, "aywl");
            OriginalStudio.Model.User.UserInfo currentUser = OriginalStudio.BLL.User.UserFactory.GetModelByName(UserName);

            string objId = "EMAIL_VALID_" + currentUser.Email;
            string valid_code = (string)WebCache.GetCacheService().RetrieveObject(objId);
            //支付通 加上 验证码图片模式
            if (input_code.ToUpper() != valid_code.ToUpper())
            {
                this.AlertAndRedirect("验证码不正确!");
                return;
            }

            this.Response.Redirect("SetPwd.aspx?us=" + ens);

        }
    }
}
