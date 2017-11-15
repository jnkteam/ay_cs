namespace KuaiCard.WebUI.User.Recharge
{
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.Security;

    public class index : UserPageBase
    {
        //2017.4.11 修改用变量绑定的方式
        protected string UserLastLoginTime = "";
        protected string UserLastLoginIp = "";
        protected string UserBalance = "0";
        protected string UserName = "";
        protected string UserMsgCount = "0";
        protected string mUserFullName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.currentUser == null)
            {
                //session过期，直接跳出
                this.Response.Redirect("~/User/loginout.aspx", true);
            }

            this.UserLastLoginTime = this.currentUser.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.UserLastLoginIp = this.currentUser.LastLoginIp;
            this.UserBalance = ((this.balance - this.unpayment) - this.Freeze).ToString("f2");
            this.mUserFullName = this.currentUser.full_name;
            if (this.currentUser.IsRealNamePass == 1)
            {
                this.UserName = this.currentUser.UserName;
            }
            else
            {
                this.UserName = "平台商户";
            }
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

