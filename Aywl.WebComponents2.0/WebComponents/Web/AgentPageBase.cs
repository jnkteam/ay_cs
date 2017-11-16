namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Text;
    using System;
    using System.Web;
    using OriginalStudio.Model.User;
    using OriginalStudio.BLL.User;

    public class AgentPageBase : PageBase
    {
        public UsersAmtInfo _currentUserAmt = null;

        public void checkLogin()
        {
            if (!this.IsLogin)
            {
                HttpContext.Current.Response.Write(string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\ntop.location.href=\"{1}\";\r\n//--></SCRIPT>", AntiXss.JavaScriptEncode("对不起！你的登录信息已失效，请重新登录"), "/Index.aspx"));
                HttpContext.Current.Response.End();
            }
            else
            {
                UserAccessTimeInfo model = new UserAccessTimeInfo();
                model.userid = this.UserId;
                model.lastAccesstime = DateTime.Now;
                UserAccessTime.Add(model);
            }
        }

        public string getUserTitle(string subPageTitle)
        {
            return string.Format("{1}- {0}", base.SiteName, subPageTitle);
        }

        public static string GetViewStatusName(object status)
        {
            if (status == DBNull.Value)
            {
                return string.Empty;
            }
            if (Convert.ToInt32(status) == 8)
            {
                return "失败";
            }
            return Enum.GetName(typeof(OrderStatusEnum), status);
        }

        public static string GetViewSuccessAmt(object status, object amt)
        {
            if (((status == DBNull.Value) || (amt == DBNull.Value)) || (Convert.ToInt32(status) != 2))
            {
                return "0";
            }
            return decimal.Round(Convert.ToDecimal(amt), 2).ToString();
        }

        public UsersUpdateLog newUpdateLog(int userid, string f, string n, string o)
        {
            UsersUpdateLog log = new UsersUpdateLog();
            log.userid = userid;
            log.Addtime = DateTime.Now;
            log.field = f;
            log.newvalue = n;
            log.oldValue = o;
            return log;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.checkLogin();
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
                return UserFactory.CurrentMember;
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

        public decimal enableAmt
        {
            get
            {
                decimal enableAmt = 0M;
                if (this.currentUserAmt != null)
                {
                    enableAmt = this.currentUserAmt.enableAmt;
                }
                return enableAmt;
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

        public int GetUserNum
        {
            get
            {
                return PromotionUserFactory.GetUserNum(this.UserId);
            }
        }

        public bool IsLogin
        {
            get
            {
                return (this.currentUser != null);
            }
        }

        public int SettlesMode
        {
            get
            {
                return this.currentUser.Settles;
            }
        }

        public decimal TodayIncome
        {
            get
            {
                return Trade.GetUserIncome(this.UserId, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), DateTime.Now.AddDays(1.0));
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

        public string UserFullName
        {
            get
            {
                return UserFactory.CurrentMember.full_name;
            }
        }

        public int UserId
        {
            get
            {
                return this.currentUser.ID;
            }
        }

        public string UserViewBankAccout
        {
            get
            {
                return Strings.ReplaceString(this.currentUser.Account, 4, "*");
            }
        }

        public string UserViewEmail
        {
            get
            {
                return Strings.Mark(this.currentUser.Email, '@');
            }
        }

        public string UserViewIdCard
        {
            get
            {
                return Strings.Mark(this.currentUser.IdCard);
            }
        }

        public string UserViewTel
        {
            get
            {
                return Strings.Mark(this.currentUser.Tel);
            }
        }

        public decimal WeekIncome
        {
            get
            {
                DateTime time = DateTime.Now.AddDays(-7.0);
                return Trade.GetUserIncome(this.UserId, Convert.ToDateTime(time.ToString("yyyy-MM-dd 00:00:00")), DateTime.Now.AddDays(1.0));
            }
        }

        public decimal YesterdayIncome
        {
            get
            {
                return Trade.GetUserIncome(this.UserId, DateTime.Now.AddDays(-1.0), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
            }
        }
    }
}

