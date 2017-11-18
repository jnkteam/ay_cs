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

    public class withdrawal : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
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

            string money = HttpContext.Current.Request["money"];
            string verifycode = HttpContext.Current.Request["verifycode"];
            string accountID = HttpContext.Current.Request["accountid"];     //对应 userspaybankapp.id。取商户转账设置表里记录。 
            string paypwd = HttpContext.Current.Request["paypwd"];
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
                decimal num3 = 0M;
                if (modelByUser.bankdetentiondays > 0)
                {
                    num3 += this.TodayIncomewangyin;
                }
                if (modelByUser.carddetentiondays > 0)
                {
                    num3 += this.TodayIncomedianka;
                }
                if (modelByUser.qqdetentiondays > 0)
                {
                    num3 += this.TodayIncomeQQ;
                }
                if (modelByUser.otherdetentiondays > 0)
                {
                    num3 += this.TodayIncomeqita;
                }
                if (modelByUser.alipaydetentiondays > 0)
                {
                    num3 += this.TodayIncomealipay;
                }
                if (modelByUser.weixindetentiondays > 0)
                {
                    num3 += this.TodayIncomeweixin;
                }
                canGetMoney = ((this.balance - this.Unpayment) - this.Freeze) - num3;
                string objId = "PHONE_VALID_" + UserFactory.CurrentMember.APIKey;
                string str7 = (string)WebCache.GetCacheService().RetrieveObject(objId);
                if (!String.IsNullOrEmpty(verifycode) && verifycode != str7 && SysConfig.TiXianNeedCustValid)
                {
                    msg = "提现手机验证码不正确！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (string.IsNullOrEmpty(accountID))
                {
                    msg = "请选择您要提现的银行卡！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (string.IsNullOrEmpty(money))
                {
                    msg = "请输入您要提现的金额！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (!decimal.TryParse(money, out checkResult))
                {
                    msg = "请输入您正确的金额！";
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
                else if (string.IsNullOrEmpty(verifycode) && SysConfig.TiXianNeedCustValid)
                {
                    msg = "请输入手机验证码！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (checkResult > canGetMoney)
                {
                    msg = "余额不足,请修改提现金额！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (checkResult < modelByUser.minamtlimitofeach)
                {
                    msg = "您的提现金额小于最低提现金额限制！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (checkResult > modelByUser.maxamtlimitofeach)
                {
                    msg = "您的提现金额大于最大提现金额限制！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else if (SettledFactory.GetUserDaySettledTimes(UserFactory.CurrentMember.ID, FormatConvertor.DateTimeToDateString(DateTime.Now)) >= modelByUser.dailymaxtimes)
                {
                    msg = "您今天的提现次数已达到最多限制，请明天再试！";
                    result = "no";
                    ico = "error";
                    goto Label_Exit;
                }
                else
                {
                    decimal userDaySettledAmt = SettledFactory.GetUserDaySettledAmt(UserFactory.CurrentMember.ID, FormatConvertor.DateTimeToDateString(DateTime.Now));
                    if ((userDaySettledAmt + checkResult) >= modelByUser.dailymaxamt)
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
                UserPayBankAppInfo info2 = new UserPayBankAppInfo();
                DataSet list = null;    // UserPayBankApp.GetList("id=" + accountID);
                if (list != null)
                {
                    foreach (DataRow row in list.Tables[0].Rows)
                    {
                        if (row["id"].ToString() == accountID)
                        {
                            bank_code = row["bankcode"].ToString();
                            PayeeBank = row["PayeeBank"].ToString();
                            account = row["account"].ToString();
                            payeeName = row["payeeName"].ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(msg))
                {
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
                        if (SysConfig.radioButtonPhone)
                        {
                            string str11 = SysConfig.sms_caiwu_tocash;
                            if (!(string.IsNullOrEmpty(str11) || string.IsNullOrEmpty(SysConfig.textPhone.ToString())))
                            {
                                str11 = str11.Replace("{@username}", UserFactory.CurrentMember.UserName)
                                    .Replace("{@settledmoney}", checkResult.ToString("f2"))
                                    .Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name);
                                SMS.SendSmsWithCheck(SysConfig.textPhone.ToString(), str11, "");
                            }
                        }
                        //@@@@@@@@@ 2017.2.10 处理 @@@@@@@@@@
                        if (modelByUser.tranRequiredAudit == 0)
                        {
                            //自动审核
                            SettledFactory.Audit(settledResult, 2); //SettledStatus.支付中

                            if (modelByUser.vaiInterface == 1)
                            {
                                model.Status = SettledStatusEnum.付款接口支付中;
                                model.Suppid = new ChannelWithdraw().GetSupplier(model.BankCode);
                                //自动结算。2017.2.10add
                                model.ID = settledResult;
                                if (settledResult > 0)
                                {
                                    //结算不需要审核，在点 结算后直接调用接口进行支付。
                                    OriginalStudio.ETAPI.Withdraw.InitDistribution(model);
                                    msg = "提现申请已经处理，请及时检查是否入账！";
                                    result = "ok";
                                    ico = "error";
                                    goto Label_Exit;
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
                        else
                        {
                            //原来就下面一句。需要人工处理的。
                            msg = "提现申请已经受理，请等待审核！";
                            result = "ok";
                            ico = "error";
                            goto Label_Exit;
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

