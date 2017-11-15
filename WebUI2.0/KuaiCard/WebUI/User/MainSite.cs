namespace KuaiCard.WebUI.User
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MainSite : MasterPage
    {
        private UserInfo _currentUser = null;
        public UsersAmtInfo _currentUserAmt = null;
        private WebInfo _webinfo = null;
        protected ContentPlaceHolder MainContentPlaceHolder;
        protected HtmlForm form1;
        public string getnid = "";
        public string getnm = "";
        protected HtmlAnchor linemail;
        protected HtmlAnchor linshouji;
        public string shouji = "";
        public string youxiang = "";

        /// <summary>
        /// 是否可以前台结算。2017.2.13增加
        /// </summary>
        public bool CanSettlesInFront = false;
        //2017.4.11 修改用变量绑定的方式
        protected string UserLastLoginTime = "";
        protected string UserLastLoginIp = "";
        protected string UserBalance = "0";
        protected string UserName = "";
        protected string UserMsgCount = "0";
        protected string UserDefaultThemes = "";
        protected string UserFullName = "";


        private void InitForm()
        {
            this.getnid = this.currentUser.ID.ToString();
            this.getnm = this.currentUser.UserName;
            try
            {
                UserMsgCount = IMSGFactory.GetUserMsgCount(this.currentUser.ID).ToString();
            }
            catch
            {
                UserMsgCount = "0";
            }
        }

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
            this.UserDefaultThemes = this.currentUser.default_themes;
            this.UserFullName = this.currentUser.full_name;
            this.UserMsgCount = IMSGFactory.GetUserMsgCount(this.currentUser.ID).ToString();
            //if (this.currentUser.IsEmailPass == 1)
            //{
            //    this.linemail.Attributes["href"] = "/user/email/";
            //}
            //else
            //{
            //    this.linemail.Attributes["href"] = "/user/validate/set.aspx";
            //}
            //if (this.currentUser.IsPhonePass == 1)
            //{
            //    this.linshouji.Attributes["href"] = "/user/mobile/";
            //}
            //else
            //{
            //    this.linshouji.Attributes["href"] = "/user/validate/tel.aspx";
            //}
            if (this.currentUser.IsRealNamePass == 1)
            {
                this.UserName = this.currentUser.UserName;
            }
            else
            {
                this.UserName = "平台商户";
            }
            if (!base.IsPostBack)
            {
                this.InitForm();
            }

            this.DataBind();
        }

        public decimal balance
        {
            get
            {
                decimal num = 0M;
                if ((this.currentUserAmt != null) && this.currentUserAmt.balance.HasValue)
                {
                    num = this.currentUserAmt.balance.Value;
                }
                return num;
            }
        }

        public UserInfo currentUser
        {
            get
            {
                if (this._currentUser == null)
                {
                    this._currentUser = UserFactory.CurrentMember;
                    if (this._currentUser == null) return null;

                    this.CanSettlesInFront = (this._currentUser.settles_type == 0) || (this._currentUser.settles_type == 2);
                }
                return this._currentUser;
            }
        }

        public UsersAmtInfo currentUserAmt
        {
            get
            {
                if ((this._currentUserAmt == null) && (this.UserId > 0))
                {
                    this._currentUserAmt = UsersAmt.GetModel(this.UserId);
                }
                return this._currentUserAmt;
            }
        }

        public decimal Freeze
        {
            get
            {
                decimal num = 0M;
                if ((this.currentUserAmt != null) && this.currentUserAmt.Freeze.HasValue)
                {
                    num = this.currentUserAmt.Freeze.Value;
                }
                return num;
            }
        }

        public string SiteName
        {
            get
            {
                if (this.webInfo == null)
                {
                    return string.Empty;
                }
                return this.webInfo.Name;
            }
        }

        public decimal unpayment
        {
            get
            {
                decimal num = 0M;
                if ((this.currentUserAmt != null) && this.currentUserAmt.unpayment.HasValue)
                {
                    num = this.currentUserAmt.unpayment.Value;
                }
                return num;
            }
        }

        public int UserId
        {
            get
            {
                if (this.currentUser == null)
                {
                    return 0;
                }
                return this.currentUser.ID;
            }
        }

        public WebInfo webInfo
        {
            get
            {
                if (this._webinfo == null)
                {
                    this._webinfo = WebInfoFactory.CurrentWebInfo;
                }
                return this._webinfo;
            }
        }
    }
}

