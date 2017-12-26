namespace OriginalStudio.WebUI.merchant.ajax
{
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class PayAcct1 : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = "";
            if (this.currentUser == null)
            {
                str = "登录信息失效，请重新登录";
            }
            else
            {
                int num = 1;
                string str2 = XRequest.GetString("bankName");
                string str3 = XRequest.GetString("action");
                string str4 = string.Empty;
                string str5 = string.Empty;
                string str6 = XRequest.GetString("cardAddr");
                string str7 = XRequest.GetString("cardId");
                string str8 = XRequest.GetString("recardId");
                string str9 = XRequest.GetString("thiscard");
                if (!string.IsNullOrEmpty(this.currentUser.Account))
                {
                    if (string.IsNullOrEmpty(str9))
                    {
                        str = "当前银行卡号不能为空";
                    }
                    else if (str9 != this.currentUser.Account)
                    {
                        str = "当前银行卡号不正确";
                    }
                }
                if (num == 1)
                {
                    if (string.IsNullOrEmpty(str2))
                    {
                        str = "开户银行不能为空";
                    }
                    if (string.IsNullOrEmpty(str6))
                    {
                        str = "支行名称不能为空";
                    }
                }
                if (string.IsNullOrEmpty(str7))
                {
                    str = "个人银行帐号";
                }
                if (str7 != str8)
                {
                    str = "个人银行帐号与确认个人银行帐号必须一致";
                }
                if (string.IsNullOrEmpty(str))
                {
                    MchUserPayBankInfo model = new MchUserPayBankInfo();
                    /*
                    model.pmode = num;
                    model.payeeBank = str2;
                    model.bankProvince = str4;
                    model.bankCity = str5;
                    model.bankAddress = str6;
                    model.account = str7;
                    model.payeeName = this.currentUser.full_name;
                    model.AddTime = new DateTime?(DateTime.Now);
                    model.userid = this.currentUser.ID;
                    model.status = AcctChangeEnum.审核成功;
                    */
                    int num2 = 0;// UserPayBankApp.Add(model);
                    if (num2 > 0)
                    {   /*
                        model.id = num2;
                        model.status = AcctChangeEnum.审核成功;
                        model.SureTime = new DateTime?(DateTime.Now);
                        model.SureUser = 0;
                        */
                        //UserPayBankApp.Check(model);
                        str = "true";
                    }
                    else
                    {
                        str = "操作失败";
                    }
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(str);
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
    }
}

