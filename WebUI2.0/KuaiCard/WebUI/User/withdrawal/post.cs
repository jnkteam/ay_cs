namespace KuaiCard.WebUI.User.withdrawal
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using OriginalStudio.Model;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Model.User;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib.TimeControl;
    using System;
    using System.Data;
    using System.Web;
    using OriginalStudio.BLL.Withdraw;
    using OriginalStudio.Lib.Configuration;

    public class post : UserPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string closecashReason = "";
            if (UserFactory.CurrentMember == null)
            {
                closecashReason = "登录信息失效，请重新登录";
            }
            else if (!SysConfig.isopenCash)
            {
                closecashReason = SysConfig.closecashReason;
            }
            else
            {
                string money = HttpContext.Current.Request["money"];
                string verifycode = HttpContext.Current.Request["verifycode"];
                string accountID = HttpContext.Current.Request["accountID"];     //对应 userspaybankapp.id。取商户转账设置表里记录。 
                string Password2 = HttpContext.Current.Request["Password2"];
                decimal result = 0M;
                decimal num2 = 0M;
                TocashSchemeInfo modelByUser = TocashScheme.GetModelByUser(1, UserFactory.CurrentMember.ID);
                if (modelByUser == null)
                {
                    closecashReason = "{result:false,text:'未设置提现方案，请联系客服人员！',ok:true}";
                }
                else
                {
                    decimal num3 = 0M;
                    if (modelByUser.bankdetentiondays > 0)
                    {
                        num3 += base.TodayIncomewangyin;
                    }
                    if (modelByUser.carddetentiondays > 0)
                    {
                        num3 += base.TodayIncomedianka;
                    }
                    if (modelByUser.qqdetentiondays > 0)
                    {
                        num3 += base.TodayIncomeQQ;
                    }
                    if (modelByUser.otherdetentiondays > 0)
                    {
                        num3 += base.TodayIncomeqita;
                    }
                    if (modelByUser.alipaydetentiondays > 0)
                    {
                        num3 += base.TodayIncomealipay;
                    }
                    if (modelByUser.weixindetentiondays > 0)
                    {
                        num3 += base.TodayIncomeweixin;
                    }
                    num2 = ((base.balance - base.unpayment) - base.Freeze) - num3;
                    string objId = "PHONE_VALID_" + UserFactory.CurrentMember.APIKey;
                    string str7 = (string) WebCache.GetCacheService().RetrieveObject(objId);
                    if (verifycode != str7 && SysConfig.TiXianNeedCustValid)
                    {
                        closecashReason = "{result:false,text:'提现手机验证码不正确！',ok:true}";
                    }
                    else if (string.IsNullOrEmpty(accountID))
                    {
                        closecashReason = "{result:false,text:'请选择您要提现的银行卡！',ok:true}";
                    }
                    else if (string.IsNullOrEmpty(money))
                    {
                        closecashReason = "{result:false,text:'请输入您要提现的金额',ok:true}";
                    }
                    else if (!decimal.TryParse(money, out result))
                    {
                        closecashReason = "{result:false,text:'请输入您正确的金额',ok:true}";
                    }
                    else if (string.IsNullOrEmpty(Password2))
                    {
                        closecashReason = "{result:false,text:'请输入提现密码！',ok:true}";
                    }
                    else if (Password2 != UserFactory.CurrentMember.Password2)
                    {
                        closecashReason = "{result:false,text:'提现密码不正确！',ok:true}";
                    }
                    else if (string.IsNullOrEmpty(verifycode) && SysConfig.TiXianNeedCustValid)
                    {
                        closecashReason = "{result:false,text:'请输入手机验证码！',ok:true}";
                    }
                    else if (result > num2)
                    {
                        closecashReason = "{result:false,text:'余额不足,请修改提现金额',ok:true}";
                    }
                    else if (result < modelByUser.minamtlimitofeach)
                    {
                        closecashReason = "{result:false,text:'您的提现金额小于最低提现金额限制',ok:true}";
                    }
                    else if (result > modelByUser.maxamtlimitofeach)
                    {
                        closecashReason = "{result:false,text:'您的提现金额大于最大提现金额限制',ok:true}";
                    }
                    else if (SettledFactory.GetUserDaySettledTimes(UserFactory.CurrentMember.ID, FormatConvertor.DateTimeToDateString(DateTime.Now)) >= modelByUser.dailymaxtimes)
                    {
                        closecashReason = "{result:false,text:'您今天的提现次数已达到最多限制，请明天再试。',ok:true}";
                    }
                    else
                    {
                        decimal userDaySettledAmt = SettledFactory.GetUserDaySettledAmt(UserFactory.CurrentMember.ID, FormatConvertor.DateTimeToDateString(DateTime.Now));
                        if ((userDaySettledAmt + result) >= modelByUser.dailymaxamt)
                        {
                            closecashReason = string.Format("{result:false,text:'您今天的提现将超过最大限额，你最多可以提现{0:f2}',ok:true}", modelByUser.dailymaxamt - userDaySettledAmt);
                        }
                    }
                    string bank_code = "";
                    string PayeeBank = "";
                    string account = "";
                    string payeeName = "";
                    UserPayBankAppInfo info2 = new UserPayBankAppInfo();
                    DataSet list = UserPayBankApp.GetList("id=" + accountID);
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
                    if (string.IsNullOrEmpty(closecashReason))
                    {
                        SettledInfo model = new SettledInfo();
                        model.addtime = DateTime.Now;
                        model.amount = result;
                        model.paytime = DateTime.Now;
                        model.status = SettledStatus.审核中;
                        model.tax = 0;
                        model.userid = UserFactory.CurrentMember.ID;
                        model.BankCode = bank_code;     //银行代码
                        model.PayeeBank = PayeeBank;     //银行名称
                        model.payeeName = payeeName;
                        model.Account = account;
                        model.Paytype = 1;          //手动提现 = 1,（商户自己发起）
                        //======16.11.15增加到帐时间==========
                        if (bank_code == "0002")    //支付宝
                            model.AppType = AppTypeEnum.t1;      //T+1到张
                        else if (bank_code == "0003")   //微信
                            model.AppType = AppTypeEnum.t1;      //T+1到张
                        else
                            model.AppType = AppTypeEnum.t1;      //T+1到张
                        //=============================
                        model.charges = new decimal?(modelByUser.chargerate * result);
                        decimal? charges = model.charges;
                        decimal chargeleastofeach = modelByUser.chargeleastofeach;
                        if ((charges.GetValueOrDefault() < chargeleastofeach) && charges.HasValue)
                        {
                            model.charges = new decimal?(modelByUser.chargeleastofeach);
                        }
                        else
                        {
                            charges = model.charges;
                            chargeleastofeach = modelByUser.chargemostofeach;
                            if ((charges.GetValueOrDefault() > chargeleastofeach) && charges.HasValue)
                            {
                                model.charges = new decimal?(modelByUser.chargemostofeach);
                            }
                        }
                        if (DateTime.Now.Hour > 16)
                        {
                            model.required = DateTime.Now.AddDays(1.0);
                        }
                        else
                        {
                            model.required = DateTime.Now;
                        }
                        model.suppid = 0;   //不用接口
                        //==========
                        model.status = SettledStatus.审核中;   //1
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
                                        .Replace("{@settledmoney}", result.ToString("f2"))
                                        .Replace("{@sitename}", WebInfoFactory.CurrentWebInfo.Name);
                                    SMS.SendSmsWithCheck(SysConfig.textPhone.ToString(), str11, "");
                                }
                            }

                            if (modelByUser.tranRequiredAudit == 0)
                            {
                                //自动审核
                                SettledFactory.Audit(settledResult, 2); //SettledStatus.支付中

                                //@@@@@@@@@ 2017.2.10 处理 @@@@@@@@@@
                                if (modelByUser.vaiInterface == 1)
                                {
                                    //2017.2.10 增加 自动代付功能。参考BankForUser.cs文件。后台代付代码。
                                    model.status = SettledStatus.付款接口支付中;
                                    model.suppid = new channelwithdraw().GetSupplier(model.BankCode);
                                    //自动结算。2017.2.10add
                                    model.id = settledResult;
                                    if (settledResult > 0)
                                    {
                                        //结算不需要审核，在点 结算后直接调用接口进行支付。
                                        KuaiCard.ETAPI.Withdraw.InitDistribution(model);
                                        closecashReason = "{result:true,text:'提现申请已经处理，请及时检查是否入账！',time:2,url:'/user/withdrawal/'}";
                                    }
                                }   //@@@@@@@@@@@@@@@@@@@@@@
                                else
                                {
                                    //原来就下面一句。需要人工处理的。
                                    closecashReason = "{result:true,text:'提现申请已经审核，请等待出款！',time:2,url:'/user/withdrawal/'}";
                                }
                            }                            
                            else
                            {
                                //原来就下面一句。需要人工处理的。
                                closecashReason = "{result:true,text:'提现申请已经受理，请等待审核！',time:2,url:'/user/withdrawal/'}";
                            }
                        }
                        else
                        {
                            closecashReason = "{result:false,text:'提现失败',ok:true}";
                        }
                    }
                }
            }
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Write(closecashReason);
        }

    }
}

