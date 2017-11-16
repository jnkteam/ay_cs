namespace KuaiCard.WebUI.User.Api
{
    using OriginalStudio.WebComponents.Web;
    using System;
    using System.Web.UI.WebControls;

    public class index_zft : UserPageBase
    {
        protected string gUserID = "";
        protected string gUserKey = "";

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

            gUserID = base.currentUser.ID.ToString();
            gUserKey = base.currentUser.APIKey;

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
    }
}

