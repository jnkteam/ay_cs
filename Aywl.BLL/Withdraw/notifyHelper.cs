namespace OriginalStudio.BLL.Withdraw
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.Withdraw;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Text;
    using OriginalStudio.Lib;

    public class notifyHelper
    {
        private OriginalStudio.Model.Withdraw.settledAgent _model = null;

        public void DoNotify()
        {
            if (((this.model != null) && PageValidate.IsUrl(this.model.return_url)) && !this.model.is_cancel)
            {
                OriginalStudio.Model.Withdraw.settledAgentNotify model = new OriginalStudio.Model.Withdraw.settledAgentNotify();
                model.userid = this.model.userid;
                model.trade_no = this.model.trade_no;
                model.out_trade_no = this.model.out_trade_no;
                string str = model.notify_id;
                string service = this.model.service;
                string str3 = this.model.input_charset;
                string str4 = this.model.userid.ToString();
                string str5 = this.model.sign_type;
                string str6 = model.addTime.ToString("yyyyMMddHHmmss");
                string str7 = this.model.out_trade_no;
                string str8 = "0.00";
                int num = 1;
                if (this.model.is_cancel)
                {
                    num = 1;
                }
                else
                {
                    if (this.model.audit_status == 1)
                    {
                        num = 1;
                    }
                    else if (this.model.audit_status == 2)
                    {
                        num = 0;
                    }
                    else if (this.model.audit_status == 3)
                    {
                        num = 2;
                    }
                    if (this.model.audit_status == 2)
                    {
                        if (this.model.payment_status == 2)
                        {
                            num = 3;
                            str8 = this.model.amount.ToString("f2");
                        }
                        if (this.model.payment_status == 3)
                        {
                            num = 4;
                        }
                    }
                }
                string str9 = num.ToString();
                string str10 = string.Empty;
                string str11 = CommonHelper.BuildParamString(CommonHelper.BubbleSort(new string[] { "service=" + service, "input_charset=" + str3, "partner=" + str4, "sign_type=" + str5, "notify_id=" + str, "notify_time=" + str6, "out_trade_no=" + str7, "trade_status=" + str9, "error_message=" + str10, "amount_str=" + str8 }));
                string userApiKey = UserFactory.GetUserApiKey(this.model.userid);
                string str13 = CommonHelper.md5(str3, str11 + userApiKey).ToLower();
                string url = this.model.return_url;
                string postData = string.Format("service={0}", service) + string.Format("&input_charset={0}", str3) + string.Format("&partner={0}", str4) + string.Format("&sign_type={0}", str5) + string.Format("&notify_id={0}", str) + string.Format("&notify_time={0}", str6) + string.Format("&out_trade_no={0}", str7) + string.Format("&trade_status={0}", str9) + string.Format("&error_message={0}", str10) + string.Format("&amount_str={0}", str8) + string.Format("&sign={0}", str13);
                try
                {
                    string str16 = WebClientHelper.GetString(url, postData, "get", Encoding.GetEncoding(str3), 0x2710);
                    model.resTime = new DateTime?(DateTime.Now);
                    int num2 = 1;
                    if (str16 == str)
                    {
                        num2 = 2;
                    }
                    else
                    {
                        num2 = 0;
                    }
                    model.notifyurl = url;
                    model.resText = str16;
                    model.notifystatus = num2;
                }
                catch (Exception exception)
                {
                    model.notifyurl = url;
                    model.resText = "";
                    model.notifystatus = 0;
                    model.remark = exception.Message;
                    model.resTime = new DateTime?(DateTime.Now);
                }
                new OriginalStudio.BLL.Withdraw.settledAgentNotify().Add(model);
            }
        }

        public OriginalStudio.Model.Withdraw.settledAgent model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._model = value;
            }
        }
    }
}

