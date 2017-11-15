namespace KuaiCard.WebUI.User.Recharge
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.Security;

    public class post : UserPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Submit();
        }

        private void Submit()
        {
            string orderid = Guid.NewGuid().ToString().Substring(0, 20).Replace("-", "");
            string callBackurl = "http://www.zhifoopay.com/user/recharge/notify.aspx";

            string str;
            string str2 = currentUser.ID.ToString();
            string str3 = currentUser.APIKey;
            //网银支付，支付宝，微信，财付通
            string str4 = Request.Form["bankName"];        //通道代码
            if (string.IsNullOrEmpty(str4))
            {
                this.AlertAndRedirect("请选择银行！");
                return;
            }

            string str5 = Request.Form["money"];
            if (string.IsNullOrEmpty(str5))
            {
                this.AlertAndRedirect("请输入金额！");
                return;
            }
            str = string.Format("parter={0}&type={1}&value={2}&orderid={3}&callbackurl={4}",
                                            new object[] { str2, str4, str5, orderid, callBackurl });
            string url = string.Format("http://116.62.219.213/Bank/?{0}&sign={1}",
                                            str, FormsAuthentication.HashPasswordForStoringInConfigFile(str + str3, "MD5").ToLower());

            Response.Redirect(url);

        }
    }
}

