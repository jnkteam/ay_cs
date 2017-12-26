namespace OriginalStudio.WebUI.merchant.ajax
{
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class getdaxie : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string s = "";
            if (UserFactory.CurrentMember == null)
            {
                s = "登录信息失效，请重新登录";
            }
            else
            {
                string str2 = this.payMoney.Trim();
                if (!string.IsNullOrEmpty(str2))
                {
                    if (Validate.IsNumeric(str2) || Validate.IsNumber(str2))
                    {
                        s = Strings.MoneyToChinese(str2);
                    }
                }
                else
                {
                    s = "请输入提现金额";
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(s);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string payMoney
        {
            get
            {
                return WebBase.GetQueryStringString("payMoney", "");
            }
        }
    }
}

