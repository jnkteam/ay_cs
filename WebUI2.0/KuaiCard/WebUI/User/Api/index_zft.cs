namespace OriginalStudio.WebUI.User.Api
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
            if (this.CurrentUser == null)
            {
                //session过期，直接跳出
                this.Response.Redirect("~/User/loginout.aspx", true);
            }

            gUserID = base.CurrentUser.ID.ToString();
            gUserKey = base.CurrentUser.APIKey;

            this.UserLastLoginTime = this.CurrentUser.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.UserLastLoginIp = this.CurrentUser.LastLoginIp;
            this.UserBalance = ((this.Balance - this.Unpayment) - this.Freeze).ToString("f2");
            this.mUserFullName = this.CurrentUser.full_name;
            if (this.CurrentUser.IsRealNamePass == 1)
            {
                this.UserName = this.CurrentUser.UserName;
            }
            else
            {
                this.UserName = "平台商户";
            }
        }
    }
}

