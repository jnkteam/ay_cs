namespace OriginalStudio.WebUI.User.Service
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using OriginalStudio.Model;
    using OriginalStudio.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.BLL.Settled;
    using System.Data;
    using OriginalStudio.Lib.TimeControl;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.BLL.Tools;

    public class withdrawalbatch : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        private WebInfo _webinfo = null;

        public void ProcessRequest(HttpContext context)
        {
            string msg = "";            //返回消息
            string ico = "";            //显示图标success，error
            string result = "";         //结果ok，no
            string url = "";            //跳转url

            if (this.currentUser == null)
            {
                //未登录
                msg = "未登录!";
                result = "no";
                ico = "error";
                goto Label_Exit;
            }

            string lstmoney = HttpContext.Current.Request["moneys"];        //金额
            string lstacccard = HttpContext.Current.Request["acccards"];    //银行帐号
            string lstaccname = HttpContext.Current.Request["accnames"];  //银行帐号姓名
            string lstbankcode = HttpContext.Current.Request["bankcodes"];              //银行代码
            string lstbankname = HttpContext.Current.Request["banknames"]; //银行名称

            string msgcode = HttpContext.Current.Request["msgcode"];      //手机验证码
            string paypwd = HttpContext.Current.Request["paypwd"];              //支付密码
            string imgcode = HttpContext.Current.Request["imgcode"];         //图片验证码

            decimal checkResult = 0M;
            decimal canGetMoney = 0M;
            TocashSchemeInfo modelByUser = TocashScheme.GetModelByUser(1, UserFactory.CurrentMember.ID);
            if (modelByUser == null)
            {
                msg = "未设置提现方案，请联系客服人员！";
                result = "no";
                ico = "error";
                goto Label_Exit;
            }
            else
            {
                if ((context.Session["CCode"] == null) || (imgcode == null))
                {
                    msg = "验证码失效!";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (context.Session["CCode"].ToString().ToUpper() != imgcode.ToUpper())
                {
                    msg = "验证码不正确!";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }

                //先计算一次提现的总金额
                string[] moneys = lstmoney.Split(',');
                string[] acccards = lstacccard.Split(',');
                string[] accnames = lstaccname.Split(',');
                string[] bankcodes = lstbankcode.Split(',');
                string[] banknames = lstbankname.Split(',');

                decimal totalMs = 0.00M;
                bool flag = true;
                foreach (string m in moneys)
                {
                    if (!decimal.TryParse(m, out checkResult))
                    {
                        flag = false;       //金额格式不准确，跳出
                        break;
                    }
                    totalMs = totalMs + checkResult;
                }

                if (!flag)
                {
                    msg = "请输入您正确的金额！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }

                decimal todayIncome = 0M;       //今日收益
                if (modelByUser.bankdetentiondays > 0)
                {
                    todayIncome += this.TodayIncomewangyin;
                }
                if (modelByUser.carddetentiondays > 0)
                {
                    todayIncome += this.TodayIncomedianka;
                }
                if (modelByUser.qqdetentiondays > 0)
                {
                    todayIncome += this.TodayIncomeQQ;
                }
                if (modelByUser.otherdetentiondays > 0)
                {
                    todayIncome += this.TodayIncomeqita;
                }
                if (modelByUser.alipaydetentiondays > 0)
                {
                    todayIncome += this.TodayIncomealipay;
                }
                if (modelByUser.weixindetentiondays > 0)
                {
                    todayIncome += this.TodayIncomeweixin;
                }
                canGetMoney = ((this.balance - this.Unpayment) - this.Freeze) - todayIncome;

                bool phoneValid = this.currentUser.IsPhonePass == 1;
                if (phoneValid)
                {
                    //手机验证通过的，必须通过手机验证码
                    string mobile = this.currentUser.Tel;
                    string objId = "PHONE_VALID_" + mobile;
                    string str7 = (string)WebCache.GetCacheService().RetrieveObject(objId);
                    if (!String.IsNullOrEmpty(msgcode) && msgcode != str7)
                    {
                        msg = "提现手机验证码不正确！";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                }

                if (string.IsNullOrEmpty(lstacccard))
                {
                    msg = "提现的银行卡号为空！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (string.IsNullOrEmpty(lstmoney))
                {
                    msg = "提现的金额为空！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (string.IsNullOrEmpty(paypwd))
                {
                    msg = "请输入提现密码！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (paypwd != this.currentUser.Password2)
                {
                    msg = "提现密码不正确！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (totalMs > canGetMoney)
                {
                    msg = "余额不足,请修改提现金额！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (totalMs < modelByUser.minamtlimitofeach)
                {
                    msg = "您的提现金额小于最低提现金额限制！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (totalMs > modelByUser.maxamtlimitofeach)
                {
                    msg = "您的提现金额大于最大提现金额限制！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else
                {
                    decimal userDaySettledAmt = SettledFactory.GetUserDaySettledAmt(UserFactory.CurrentMember.ID, FormatConvertor.DateTimeToDateString(DateTime.Now));
                    if ((userDaySettledAmt + totalMs) >= modelByUser.dailymaxamt)
                    {
                        msg = string.Format("您今天的提现将超过最大限额，你最多可以提现{0:f2}", modelByUser.dailymaxamt - userDaySettledAmt);
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                }
                string bank_code = "";
                string PayeeBank = "";
                string account = "";
                string payeeName = "";

                //循环产生数据库记录
                for (int K = 0; K < bankcodes.Length; K++)
                {
                    bank_code = bankcodes[K];
                    PayeeBank = banknames[K];
                    payeeName = accnames[K];
                    account = acccards[K];
                    checkResult = Convert.ToDecimal(moneys[K]); //提现金额

                    SettledInfo model = new SettledInfo();
                    model.AddTime = DateTime.Now;
                    model.Amount = checkResult;
                    model.PayTime = DateTime.Now;
                    model.Status = SettledStatusEnum.审核中;
                    model.Tax = 0;
                    model.UserID = UserFactory.CurrentMember.ID;
                    model.BankCode = bank_code;     //银行代码
                    model.PayeeBank = PayeeBank;     //银行名称
                    model.PayeeName = payeeName;
                    model.Account = account;
                    model.PayType = 1;          //手动提现 = 1,（商户自己发起）
                    //======16.11.15增加到帐时间==========
                    if (bank_code == "0002")    //支付宝
                        model.AppType = AppTypeEnum.t1;      //T+1到张
                    else if (bank_code == "0003")   //微信
                        model.AppType = AppTypeEnum.t1;      //T+1到张
                    else
                        model.AppType = AppTypeEnum.t1;      //T+1到张
                    //=============================
                    model.Charges = new decimal?(modelByUser.chargerate * checkResult);
                    decimal? charges = model.Charges;
                    decimal chargeleastofeach = modelByUser.chargeleastofeach;
                    if ((charges.GetValueOrDefault() < chargeleastofeach) && charges.HasValue)
                    {
                        model.Charges = new decimal?(modelByUser.chargeleastofeach);
                    }
                    else
                    {
                        charges = model.Charges;
                        chargeleastofeach = modelByUser.chargemostofeach;
                        if ((charges.GetValueOrDefault() > chargeleastofeach) && charges.HasValue)
                        {
                            model.Charges = new decimal?(modelByUser.chargemostofeach);
                        }
                    }
                    if (DateTime.Now.Hour > 16)
                    {
                        model.Required = DateTime.Now.AddDays(1.0);
                    }
                    else
                    {
                        model.Required = DateTime.Now;
                    }
                    //==========
                    model.Suppid = 0;
                    model.Status = SettledStatusEnum.审核中;   //1
                    //==========

                    int settledResult = SettledFactory.Apply(model);
                    if (settledResult > 0)
                    {
                        if (modelByUser.tranRequiredAudit == 0)
                        {
                            //自动审核
                            SettledFactory.Audit(settledResult, 2); //SettledStatus.支付中

                            if (modelByUser.vaiInterface == 1)
                            {
                                model.Suppid = new ChannelWithdraw().GetSupplier(model.BankCode);
                                if (model.Suppid > 0)
                                {
                                    //重新取一遍
                                    model.PayTime = DateTime.Now;
                                    model.Status = SettledStatusEnum.已支付;
                                    model.ID = settledResult;
                                    if (SettledFactory.Pay(model) == 0)
                                    {
                                        //结算不需要审核，在点 结算后直接调用接口进行支付。
                                        OriginalStudio.ETAPI.Withdraw.InitDistribution(model);
                                        System.Threading.Thread.Sleep(500);
                                    }
                                }
                            }
                            else
                            {
                                msg = "提现申请已经审核，请等待出款！";
                                result = "ok";
                                ico = "error";
                                goto Label_Exit;
                            }
                        }
                    }
                    else
                    {
                        msg = "提现失败！";
                        result = "no";
                        ico = "error";
                        goto Label_Exit;
                    }
                }

                //所有都处理完，产生最终提示
                msg = "提现申请已经受理，请等待审核！";
                result = "ok";
                ico = "error";
                goto Label_Exit;
            }
            
        Label_Exit:
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + result + "\",\"ico\":\"" + ico + "\",\"msg\":\"" + msg + "\",\"url\":\"" + url + "\"}");
        }

        private UserInfo currentUser
        {
            get
            {
                return UserFactory.CurrentMember;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string SiteName
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

        private WebInfo webInfo
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

        #region 取金额

        private int UserId
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

        private MchUsersAmtInfo _currentUserAmt = null;

        /// <summary>
        /// 账户总金额
        /// </summary>
        private MchUsersAmtInfo currentUserAmt
        {
            get
            {
                if ((this._currentUserAmt == null) && (this.UserId > 0))
                {
                    this._currentUserAmt = MchUsersAmt.GetModel(this.UserId);
                }
                return this._currentUserAmt;
            }
        }

        private decimal balance
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

        /// <summary>
        /// 当天总收入
        /// </summary>
        private decimal TodayIncome
        {
            get
            {
                return Trade.GetUserIncome(this.UserId,
                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天支付宝收入
        /// </summary>
        private decimal TodayIncomealipay
        {
            get
            {
                return Trade.GetUserIncome(5,
                                                            this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天点卡收入
        /// </summary>
        private decimal TodayIncomedianka
        {
            get
            {
                return Trade.GetUserIncome(2, this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天QQ钱包收入
        /// </summary>
        private decimal TodayIncomeQQ
        {
            get
            {
                return Trade.GetUserIncome(7, this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天其他收入
        /// </summary>
        private decimal TodayIncomeqita
        {
            get
            {
                return Trade.GetUserIncome(3, this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天网银收入
        /// </summary>
        private decimal TodayIncomewangyin
        {
            get
            {
                return Trade.GetUserIncome(1, this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 当天微信收入
        /// </summary>
        private decimal TodayIncomeweixin
        {
            get
            {
                return Trade.GetUserIncome(6, this.UserId,
                                                            Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                                                            DateTime.Now.AddDays(1.0));
            }
        }

        /// <summary>
        /// 未结算金额。
        /// </summary>
        private decimal Unpayment
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

        /// <summary>
        /// 账户冻结金额
        /// </summary>
        private decimal Freeze
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

        #endregion
    }
}

