namespace KuaiCard.BLL.Api
{
    using KuaiCard.BLL;
    using KuaiCard.Model;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Security;
    using System;
    using System.Text;
    using System.Web;

    public class SellCard20
    {
        public static string Successflag = "opstate=0";

        public static string ConvertErrorCode(int suppId, string errcode, decimal refervalue, decimal realvalue)
        {
            SupplierCode code = (SupplierCode) suppId;
            string str = "99";
            switch (code)
            {
                case SupplierCode.OfCard:
                    if (errcode == "2000")
                    {
                        str = "0";
                    }
                    else if (errcode == "2010")
                    {
                        str = "10";
                    }
                    else if (errcode == "2011")
                    {
                        str = "11";
                    }
                    else if (errcode == "2012")
                    {
                        str = "12";
                    }
                    else if (errcode == "2013")
                    {
                        str = "13";
                    }
                    else if (errcode == "2016")
                    {
                        str = "16";
                    }
                    else if (errcode == "2017")
                    {
                        str = "17";
                    }
                    else if (errcode == "2018")
                    {
                        str = "18";
                    }
                    else if (errcode == "2019")
                    {
                        str = "19";
                    }
                    break;

                case SupplierCode.HuiYuan:
                    if (errcode == "0")
                    {
                        str = "0";
                        if (realvalue != refervalue)
                        {
                            str = "11";
                        }
                    }
                    else if (errcode == "9")
                    {
                        str = "16";
                    }
                    else if (errcode == "10")
                    {
                        str = "18";
                    }
                    else if (errcode == "98")
                    {
                        str = "19";
                    }
                    break;

                case SupplierCode.Card60866:
                    if (string.IsNullOrEmpty(errcode) || (errcode == "0"))
                    {
                        str = "0";
                    }
                    else if (errcode == "1001")
                    {
                        str = "16";
                    }
                    else if (errcode == "1003")
                    {
                        str = "13";
                    }
                    else
                    {
                        str = "10";
                    }
                    break;
            }
            if (code == SupplierCode.LongBaoPay)
            {
                str = errcode;
            }
            return str;
        }

        public static string ConvertSynchronousErrorCode(SupplierCode supp, string errcode)
        {
            string str = "99";
            if (supp == SupplierCode.OfCard)
            {
                if (errcode == "2001")
                {
                    return "1";
                }
                if (errcode == "2002")
                {
                    return "2";
                }
                if (errcode == "2005")
                {
                    return "5";
                }
                if (errcode == "2009")
                {
                    return "19";
                }
                if (errcode == "2010")
                {
                    return "10";
                }
                if (errcode == "2012")
                {
                    return "12";
                }
                if (errcode == "2016")
                {
                    str = "16";
                }
                return str;
            }
            if (supp == SupplierCode.HuiYuan)
            {
                str = "99";
                if (errcode == "0")
                {
                    return "1";
                }
                if (errcode == "9")
                {
                    return "16";
                }
                if (errcode == "10")
                {
                    return "18";
                }
                if (errcode == "98")
                {
                    str = "19";
                }
                return str;
            }
            if (supp == SupplierCode.Card60866)
            {
                if (errcode == "1")
                {
                    str = "1";
                }
                return str;
            }
            if (supp == SupplierCode.LongBaoPay)
            {
                str = errcode;
            }
            return str;
        }

        public static string ConvertSynchronousErrorCodeForV1(SupplierCode supp, string errcode)
        {
            string str = "-1";
            if (supp == SupplierCode.OfCard)
            {
                if (errcode == "2001")
                {
                    return "0";
                }
                return "-1";
            }
            if (supp == SupplierCode.HuiYuan)
            {
                str = "-1";
                if (errcode == "0")
                {
                    str = "0";
                }
                return str;
            }
            if (supp == SupplierCode.Card60866)
            {
                if (errcode == "1")
                {
                    str = "0";
                }
                return str;
            }
            if (supp != SupplierCode.LongBaoPay)
            {
                return str;
            }
            if (str == "1")
            {
                return "0";
            }
            return "-1";
        }

        public static string CreateNotifyUrl(OrderCardInfo orderinfo, string apikey)
        {
            string notifyurl = string.Empty;
            if ((orderinfo == null) || string.IsNullOrEmpty(apikey))
            {
                return notifyurl;
            }
            notifyurl = orderinfo.notifyurl;
            decimal realvalue = 0M;
            if (orderinfo.realvalue.HasValue)
            {
                realvalue = decimal.Round(orderinfo.realvalue.Value, 0);
            }
            string paramValue = ConvertErrorCode(orderinfo.supplierId, orderinfo.errtype, orderinfo.refervalue, realvalue);
            string str3 = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { orderinfo.userorder, paramValue, realvalue, apikey }));
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("orderid={0}", UrlEncode(orderinfo.userorder));
            builder.AppendFormat("&opstate={0}", UrlEncode(paramValue));
            builder.AppendFormat("&ovalue={0}", UrlEncode(realvalue.ToString()));
            builder.AppendFormat("&sysorderid={0}", UrlEncode(orderinfo.orderid));
            builder.AppendFormat("&systime={0}", UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
            builder.AppendFormat("&attach={0}", UrlEncode(orderinfo.attach));
            builder.AppendFormat("&msg={0}", UrlEncode(orderinfo.userViewMsg));
            builder.AppendFormat("&sign={0}", UrlEncode(str3));
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
                    return (WebInfoFactory.CurrentWebInfo.apicardname + "[" + WebInfoFactory.CurrentWebInfo.apicardversion + "]");
                }
                return string.Empty;
            }
        }
    }
}

