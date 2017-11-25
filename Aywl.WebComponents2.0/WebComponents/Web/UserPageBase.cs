namespace OriginalStudio.WebComponents.Web
{
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Security;
    using OriginalStudio.Lib.Text;
    using System;
    using System.Web;

    public class UserPageBase : PageBase
    {
        private UserInfo _currentUser = null;
        public MchUsersAmtInfo _currentUserAmt = null;

        /// <summary>
        /// 是否可以前台结算。2017.2.13增加
        /// </summary>
        public bool CanSettlesInFront = false;

        public void CheckLogin()
        {
            if (!this.IsLogin)
            {
                HttpContext.Current.Response.Write(string.Format("\r\n<SCRIPT LANGUAGE='javascript'><!--\r\nalert({0});\r\ntop.location.href=\"{1}\";\r\n//--></SCRIPT>", AntiXss.JavaScriptEncode("对不起！你的登录信息已失效，请重新登录"), "/index.aspx"));
                HttpContext.Current.Response.End();
            }
            else
            {
                MchUserAccessTimeInfo model = new MchUserAccessTimeInfo();
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
            if (Convert.ToInt32(status) == 4)
            {
                return "<font style=\"color: #F40;\">失败</font>";
            }
            if (Convert.ToInt32(status) == 2)
            {
                return "<font style=\"color: #2254F3;\">成功</font>";
            }
            if (Convert.ToInt32(status) == 1)
            {
                return "<font style=\"color: #F39C2D;\">处理中</font>";
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
            this.CheckLogin();
        }

        public decimal Balance
        {
            get
            {
                decimal num = 0M;
                if (this.currentUserAmt != null)
                {
                    num = this.currentUserAmt.Balance;
                }
                return num;
            }
        }

        public UserInfo CurrentUser
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

        /// <summary>
        /// 账户总金额
        /// </summary>
        public MchUsersAmtInfo currentUserAmt
        {
            get
            {
                if ((this._currentUserAmt == null) && (this.UserId > 0))
                {
                    this._currentUserAmt = MchUsersAmtFactory.GetModel(this.UserId);
                }
                return this._currentUserAmt;
            }
        }

        /// <summary>
        /// 账户冻结金额
        /// </summary>
        public decimal Freeze
        {
            get
            {
                decimal num = 0M;
                if (this.currentUserAmt != null)
                {
                    num = this.currentUserAmt.Freeze;
                }
                return num;
            }
        }

        public bool IsLogin
        {
            get
            {
                return (this.CurrentUser != null);
            }
        }

        /// <summary>
        /// 结算方式。0：T+0          1：T+1
        /// </summary>
        public int SettlesMode
        {
            get
            {
                return this.CurrentUser.Settles;
            }
        }

        /// <summary>
        /// 当天总收入
        /// </summary>
        public decimal TodayIncome
        {
            get
            {
                return TradeFactory.GetUserIncome(this.UserId, 
                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天支付宝收入
        /// </summary>
        public decimal TodayIncomealipay
        {
            get
            {
                return TradeFactory.GetUserIncome(5, 
                                                            this.UserId, 
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天点卡收入
        /// </summary>
        public decimal TodayIncomedianka
        {
            get
            {
                return TradeFactory.GetUserIncome(2, this.UserId, 
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天QQ钱包收入
        /// </summary>
        public decimal TodayIncomeQQ
        {
            get
            {
                return TradeFactory.GetUserIncome(7, this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天其他收入
        /// </summary>
        public decimal TodayIncomeqita
        {
            get
            {
                return TradeFactory.GetUserIncome(3, this.UserId, 
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天网银收入
        /// </summary>
        public decimal TodayIncomewangyin
        {
            get
            {
                return TradeFactory.GetUserIncome(1, this.UserId, 
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天微信收入
        /// </summary>
        public decimal TodayIncomeweixin
        {
            get
            {
                return TradeFactory.GetUserIncome(6, this.UserId, 
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")), 
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 未结算金额。
        /// </summary>
        public decimal Unpayment
        {
            get
            {
                decimal num = 0M;
                if (this.currentUserAmt != null)
                {
                    num = this.currentUserAmt.UnPayment;
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
                if (this.CurrentUser == null)
                {
                    return 0;
                }
                return this.CurrentUser.ID;
            }
        }

        public string UserViewBankAccout
        {
            get
            {
                return Strings.ReplaceString(this.CurrentUser.Account, 4, "*");
            }
        }

        public string UserViewEmail
        {
            get
            {
                return Strings.Mark(this.CurrentUser.Email, '@');
            }
        }

        public string UserViewID
        {
            get
            {
                if (string.IsNullOrEmpty(this.CurrentUser.IdCard) || (this.CurrentUser.IdCard.Length < 4))
                {
                    return string.Empty;
                }
                return Strings.ReplaceString(this.CurrentUser.IdCard, 3, (this.CurrentUser.IdCard.Length - 3) - 4, "*");
            }
        }

        public string UserViewIdCard
        {
            get
            {
                return Strings.Mark(this.CurrentUser.IdCard);
            }
        }

        public string UserViewTel
        {
            get
            {
                return Strings.Mark(this.CurrentUser.Tel);
            }
        }

        /// <summary>
        /// 一周总收入
        /// </summary>
        public decimal WeekIncome
        {
            get
            {
                DateTime time = DateTime.Now.AddDays(-7.0);
                return TradeFactory.GetUserIncome(this.UserId, 
                                                            Convert.ToDateTime(time.ToString("yyyy-MM-dd 00:00:00")), 
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        public string UserMsgCount
        {
            get
            {
                return "0";// OriginalStudio.BLL.IMSGFactory.GetUserMsgCount(this.currentUser.ID).ToString();
            }
        }
    }
}

