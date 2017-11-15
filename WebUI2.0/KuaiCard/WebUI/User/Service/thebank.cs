namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.BLL.User;
    using KuaiCard.Model.User;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class thebank : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public string bank(string paymodeId)
        {
            switch (paymodeId)
            {
                case "cmb":
                    return "招商银行";

                case "icbc":
                    return "中国工商银行";

                case "abc":
                    return "中国农业银行";

                case "ccb":
                    return "中国建设银行";

                case "boc":
                    return "中国银行";

                case "spdb":
                    return "浦发银行";

                case "comm":
                    return "中国交通银行";

                case "cmbc":
                    return "中国民生银行";

                case "szdb":
                    return "深圳发展银行";

                case "gdb":
                    return "广东发展银行";

                case "citic":
                    return "中信银行";

                case "hxb":
                    return "华夏银行";

                case "cib":
                    return "兴业银行";

                case "sjb":
                    return "盛京银行";

                case "hsbc":
                    return "汇丰银行";

                case "bjbank":
                    return "北京银行";

                case "ceb":
                    return "中国光大银行";

                case "nbbank":
                    return "宁波银行";

                case "hzcb":
                    return "杭州银行";

                case "spabank":
                    return "平安银行";

                case "dlb":
                    return "大连银行";

                case "czsb":
                    return "浙商银行";

                case "shbank":
                    return "上海银行";

                case "postgc":
                    return "中国邮政储蓄银行";
            }
            return "工商银行";
        }

        public void ProcessRequest(HttpContext context)
        {
            int num;
            string str = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;

            if (this.currentUser == null)
            {
                str = "未登录！";
                str3 = "no";
                str2 = "error";
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"result\":\"" + str3 + "\",\"ico\":\"" + str2 + "\",\"msg\":\"" + str + "\",\"url\":\"" + str4 + "\"}");
                return;
            }

            UserPayBankAppInfo info;

            string typeBank = GetFormValue("typeBank");
            string bankCode = GetFormValue("bankCode");
            KuaiCard.Model.Withdraw.channelwithdraw chlObj = new KuaiCard.BLL.Withdraw.channelwithdraw().GetModelByBankCode(bankCode);      
            string accountID = GetFormValue("accountID");
            string str9 = GetFormValue("bankCardNoAgain");
            string payeeName = GetFormValue("accountName");
            string str11 = GetFormValue("alipayID");
            string str12 = GetFormValue("Province");
            string str13 = GetFormValue("City");
            string str14 = GetFormValue("Area");
            string str15 = GetFormValue("bankfullname");
            if (typeBank == "bank")
            {
                if ((typeBank == "") || (typeBank == null))
                {
                    str = "参数错误!";
                    str3 = "no";
                    str2 = "error";
                }
                else
                {
                    switch (bankCode)
                    {
                        case null:
                        case "":
                            str = "请选择提现银行!";
                            str3 = "no";
                            str2 = "error";
                            goto Label_031D;
                    }
                    switch (accountID)
                    {
                        case null:
                        case "":
                            str = "银行卡卡号不能为空!";
                            str3 = "no";
                            str2 = "error";
                            goto Label_031D;
                    }
                    if ((payeeName == "") || (payeeName == null))
                    {
                        str = "收款人姓名不能为空！!";
                        str3 = "no";
                        str2 = "error";
                    }
                    else if (string.IsNullOrEmpty(str))
                    {
                        info = new UserPayBankAppInfo();
                        info.pmode = 1;
                        info.BankCode = bankCode;
                        info.payeeBank = (chlObj == null) ? "空" : chlObj.bankName;
                        info.payeeName = payeeName;
                        info.userid = this.currentUser.ID;
                        info.status = AcctChangeEnum.审核成功;
                        info.bankProvince = string.Empty;
                        info.bankCity = string.Empty;
                        info.bankAddress = string.Empty;
                        info.account = accountID;
                        info.AddTime = new DateTime?(DateTime.Now);
                        num = UserPayBankApp.Add(info);
                        if (num > 0)
                        {
                            info.id = num;
                            info.status = AcctChangeEnum.审核成功;
                            info.SureTime = new DateTime?(DateTime.Now);
                            info.SureUser = 0;
                            if (UserPayBankApp.Check(info))
                            {
                                str = "添加成功!";
                                str3 = "ok";
                                str2 = "success";
                                str4 = "/user/validate/set.aspx";
                            }
                        }
                        else
                        {
                            str3 = "添加失败";
                            str2 = "success";
                            str4 = "/user/validate/set.aspx";
                        }
                    }
                }
            }
        Label_031D:
            if (typeBank == "alipay")
            {
                //支付宝
                if ((typeBank == "") || (typeBank == null))
                {
                    str = "参数错误!";
                    str3 = "no";
                    str2 = "error";
                }
                else if ((str11 == "") || (str11 == null))
                {
                    str = "支付宝账户不能为空!";
                    str3 = "no";
                    str2 = "error";
                }
                else if (string.IsNullOrEmpty(str))
                {
                    info = new UserPayBankAppInfo();
                    info.pmode = 2;
                    info.payeeBank = "支付宝";
                    info.payeeName = str11;
                    info.AddTime = new DateTime?(DateTime.Now);
                    info.userid = this.currentUser.ID;
                    info.status = AcctChangeEnum.审核成功;
                    num = UserPayBankApp.Add(info);
                    if (num > 0)
                    {
                        info.id = num;
                        info.status = AcctChangeEnum.审核成功;
                        info.SureTime = new DateTime?(DateTime.Now);
                        info.SureUser = 0;
                        if (UserPayBankApp.Check(info))
                        {
                            str = "添加成功!";
                            str3 = "ok";
                            str2 = "success";
                            str4 = "/user/verify/";
                        }
                    }
                    else
                    {
                        str = "添加失败!";
                        str3 = "ok";
                        str2 = "success";
                    }
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":\"" + str3 + "\",\"ico\":\"" + str2 + "\",\"msg\":\"" + str + "\",\"url\":\"" + str4 + "\"}");
        }

        public UserInfo currentUser
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

        private string GetFormValue(string key)
        {
            try
            {
                string str = HttpContext.Current.Request.Form[key];
                if (!string.IsNullOrEmpty(str))
                {
                    return str.Trim();
                }
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
    }
}

