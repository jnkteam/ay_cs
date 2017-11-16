using System;
using System.Collections.Generic;
using System.Text;
using OriginalStudio.WebComponents.Web;
using System.Web.UI.WebControls;
using OriginalStudio.Cache;
using OriginalStudio.BLL.Tools;
using OriginalStudio.BLL.User;

namespace OriginalStudio.WebUI.User.Forget
{
    public class GetByTel : PageBase
    {
        protected string gUserPhone = "";
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

                gUserPhone = OriginalStudio.Lib.Text.Strings.Mark(currentUser.Tel);

                if (true)       // if (currentUser.IsEmailPass == 1)
                {
                    string objId = "PHONE_VALID_" + currentUser.Tel;
                    string o = (string)WebCache.GetCacheService().RetrieveObject(objId);
                    if (o == null)
                    {
                        o = new Random().Next(0x2710, 0x1869f).ToString();
                        WebCache.GetCacheService().AddObject(objId, o);
                    }
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("亲爱的{0}:", currentUser.UserName + ",");
                    builder.AppendFormat("您的验证码为：{0}，打死也不能告诉别人！", o);

                    SMS.SendSmsWithCheck(currentUser.Tel, builder.ToString(), "");
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

            string objId = "PHONE_VALID_" + currentUser.Tel;
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
