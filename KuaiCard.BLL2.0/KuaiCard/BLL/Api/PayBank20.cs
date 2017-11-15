namespace KuaiCard.BLL.Api
{
    using KuaiCard.BLL;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Security;
    using System;
    using System.Text;
    using System.Web;

    public class PayBank20
    {
        public static string Successflag = "opstate=0";

        public static string CreateNotifyUrl(OrderBankInfo orderinfo, bool isNotify, string apiKey)
        {
            string notifyurl = string.Empty;
            if ((orderinfo == null) || string.IsNullOrEmpty(apiKey))
            {
                return notifyurl;
            }
            if (isNotify)
            {
                notifyurl = orderinfo.notifyurl;
            }
            else
            {
                notifyurl = orderinfo.returnurl;
            }
            string userorder = orderinfo.userorder;
            string opstate = orderinfo.opstate;
            string paramValue = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
            string str5 = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { userorder, opstate, paramValue, apiKey }));
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("orderid={0}", UrlEncode(userorder));
            builder.AppendFormat("&opstate={0}", UrlEncode(opstate));
            builder.AppendFormat("&ovalue={0}", UrlEncode(paramValue));
            builder.AppendFormat("&sysorderid={0}", UrlEncode(orderinfo.orderid));
            builder.AppendFormat("&systime={0}", UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
            builder.AppendFormat("&attach={0}", UrlEncode(orderinfo.attach));
            builder.AppendFormat("&msg={0}", UrlEncode(orderinfo.msg));
            builder.AppendFormat("&sign={0}", UrlEncode(str5));
            return (notifyurl + "?" + builder.ToString());
        }

        public static bool SignVerification(string type, string userid, string cardno, string cardpwd, string value, string orderid, string restrict, string callbackurl, string key, string sign)
        {
            try
            {
                return (Cryptography.MD5(string.Format("type={0}&parter={1}&cardno={2}&cardpwd={3}&value={4}&restrict={5}&orderid={6}&callbackurl={7}{8}", new object[] { type, userid, cardno, cardpwd, value, restrict, orderid, callbackurl, key })).ToLower() == sign);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static string UrlEncode(string paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                return string.Empty;
            }
            return HttpUtility.UrlEncode(paramValue, Encoding.GetEncoding("gb2312"));
        }

        public static string VersionName
        {
            get
            {
                if (WebInfoFactory.CurrentWebInfo != null)
                {
                    return (WebInfoFactory.CurrentWebInfo.apibankname + "[" + WebInfoFactory.CurrentWebInfo.apibankversion + "]");
                }
                return string.Empty;
            }
        }
    }
}

