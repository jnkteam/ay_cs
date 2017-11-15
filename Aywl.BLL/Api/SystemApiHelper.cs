namespace OriginalStudio.BLL
{
    using OriginalStudio.BLL.Api;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Data;
    using System.Collections.Generic;

    public class SystemApiHelper
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public static string vbapi10 = "vb1.00";
        public static string vbapi10BankNotifySuccessflag = "OK";

        public static string phoneapi10 = "phone1.00";
        public static string v36010 = "v360.10";
        public static string v36010BankNotifySuccessflag = "opstate=0";
        public static string v7010 = "vb70.10";
        public static string v7010ApiName = "70Card平台用户储值接口-版本号1.0";
        public static string v7010BankNotifySuccessflag = "ok";
        public static string v7010BankNotifyVerifyStr = "returncode={0}&userid={1}&orderid={2}&keyvalue={3}";
        public static string v7010BankReceiveVerifyStr = "userid={0}&orderid={1}&bankid={2}";

        public static string vbmyapi20 = "vb2.00";
        public static string vc7010 = "vc70.21";
        public static string vc7010ApiName = "70Card平台接口对接直通车-版本号2.1";
        public static string vc7010CardNotifySuccessflag = "ok";
        public static string vc7010CardNotifyVerifyStr = "returncode={0}&userid={1}&orderid={2}&typeid={3}&productid={4}&cardno={5}&cardpwd={6}&money={7}&realmoney={8}&cardstatus={9}&keyvalue={10}";
        public static string vc7010CardReceiveVerifyStr = "userid={0}&orderid={1}&typeid={2}&productid={3}&cardno={4}&cardpwd={5}&money={6}&url={7}";
        public static string vc7010CardSynchronousNotifyVerifyStr = "returncode={0}&returnorderid={1}&keyvalue={2}";
        public static string vcmyapi10 = "vc1.00";
        public static string vcmyapi20 = "vc2.00";
        public static string vcYee10 = "vcYee.10";
        public static string vcYee10ApiName = "非银行卡专业版（组合支付）";
        public static string vcYee10NotifySuccessflag = "SUCCESS";
        public static string vcYee20 = "vcYee.20";
        public static string vcYee20ApiName = "易宝点卡通用接口";
        public static string vcYee20NotifySuccessflag = "SUCCESS";
        public static string veypa10 = "vyb1.00";
        public static string veypa10BankNotifyVerifyStr = "version={0}&partner={1}&orderid={2}&payamount={3}&opstate={4}&orderno={5}&eypaltime={6}&message={7}&paytype={8}&remark={9}&key={10}";
        public static string vhq10 = "vhq1.00";
        public static string vhq10ApiName = "花旗支付-商户支付功能接口规范版本号1.0";
        public static string vhq10BankReceiveVerifyStr = "{0}|{1}|{2}|{3}|{4}|{5}|{6}";
        public static string vhq10NotifySuccessflag = "errCode=0";
        public static string vYee10 = "vbYee.10";
        public static string vYee10ApiName = "易宝网银";
        public static string vYee10BankNotifySuccessflag = "SUCCESS";

        //16.12.7add
        public static string vChangJie = "vcj1.00";

        #region 正在使用

        /// <summary>
        /// 网银支付 客户通知地址 notify-url
        /// </summary>
        /// <param name="orderinfo"></param>
        /// <param name="isNotify"></param>
        /// <returns></returns>
        public static string GetBankBackUrl(OrderBankInfo orderinfo, bool isNotify)
        {
            string notifyurl = string.Empty;
            if (isNotify)
            {
                notifyurl = orderinfo.notifyurl;
            }
            else
            {
                notifyurl = orderinfo.returnurl;
            }
            //如果是127.0.0.1这种地址，直接不处理
            //if (notifyurl.ToLower().StartsWith("http://127.0.0.1")) return "";
            if (notifyurl.ToLower().StartsWith("http://localhost")) return "";

            if ((orderinfo != null) && !string.IsNullOrEmpty(notifyurl))
            {
                string opstate;
                MchUserBaseInfo baseModel = MchUserFactory.GetUserBaseInfo(orderinfo.userid);
                if (baseModel == null)
                {
                    return notifyurl;
                }

                string aPIKey = baseModel.ApiKey;
                string str = string.Empty;
                string version = orderinfo.version;
                string userorder = orderinfo.userorder;
                StringBuilder builder = new StringBuilder();
                if (string.IsNullOrEmpty(version) || (version == vbapi10))
                {
                    opstate = orderinfo.opstate;
                    string ordermoney = decimal.Round(orderinfo.realvalue.Value, 2).ToString();

                    SortedDictionary<string, string> waitSign = new SortedDictionary<string, string>();
                    waitSign.Add("orderid", userorder);
                    waitSign.Add("opstate", opstate);
                    waitSign.Add("ovalue", ordermoney);
                    waitSign.Add("systime", orderinfo.completetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    waitSign.Add("sysorderid", orderinfo.orderid);
                    waitSign.Add("completiontime", orderinfo.completetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    waitSign.Add("attach", orderinfo.attach);
                    waitSign.Add("msg", orderinfo.msg);
                    str = Cryptography.SignSortedDictionary(waitSign, aPIKey);
                    waitSign.Add("sign", str);
                    foreach (var K in waitSign.Keys)
                    {
                        builder.Append(K + "=" + HttpUtility.UrlEncode(waitSign[K], Encoding.GetEncoding("GB2312")));
                    }
                    return (notifyurl + "?" + builder.ToString());
                }
            }

            return notifyurl;
        }
        
        /// <summary>
        /// 版本号接口返回成功标记
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetSuccessFlag(string version)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(version) || (version == vbapi10))
            {
                return vbapi10BankNotifySuccessflag;
            }
            return str;
        }

        /// <summary>
        /// 检查异步通知返回是否成功
        /// </summary>
        /// <param name="version"></param>
        /// <param name="callbackText"></param>
        /// <returns></returns>
        public static bool CheckCallBackIsSuccess(string version, string callbackText)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(callbackText))
            {
                string str = GetSuccessFlag(version);
                if (callbackText.StartsWith(str) || callbackText.ToLower().StartsWith(str))  //2017.2.20修改
                {
                    flag = true;
                }
            }
            return flag;
        }

        #endregion

        public static bool BankReceiveVerify(string version, string sign, params object[] arg)
        {
            bool flag = false;
            if (version == v7010)
            {
                if (Cryptography.MD5(string.Format(v7010BankReceiveVerifyStr, arg).ToLower() + string.Format("&keyvalue={0}", arg[3])).ToLower() == sign.ToLower())
                {
                    flag = true;
                }
                return flag;
            }
            if ((version == vhq10) && (Cryptography.MD5(string.Format(vhq10BankReceiveVerifyStr, arg)).ToLower() == sign.ToLower()))
            {
                flag = true;
            }
            return flag;
        }

        public static string ConverProductId(string version, string prodcutid)
        {
            string str = string.Empty;
            if (version == vc7010)
            {
                switch (prodcutid)
                {
                    case "cm10":
                        return "001310";

                    case "cm20":
                        return "001320";

                    case "cm30":
                        return "001330";

                    case "cm50":
                        return "001350";

                    case "cm100":
                        return "0013100";

                    case "cm300":
                        return "0013300";

                    case "cm500":
                        return "0013500";

                    case "sd25":
                        return "000225";

                    case "sd30":
                        return "000230";

                    case "sd35":
                        return "000235";

                    case "sd45":
                        return "000245";

                    case "sd50":
                        return "000250";

                    case "sd100":
                        return "0002100";

                    case "zt10":
                        return "000710";

                    case "zt20":
                        return "000720";

                    case "zt30":
                        return "000730";

                    case "zt50":
                        return "000750";

                    case "zt60":
                        return "000760";

                    case "zt100":
                        return "0007100";

                    case "zt300":
                        return "0007300";

                    case "jw4":
                        return "00034";

                    case "jw5":
                        return "00035";

                    case "jw6":
                        return "00036";

                    case "jw10":
                        return "000310";

                    case "jw15":
                        return "000315";

                    case "jw30":
                        return "000330";

                    case "jw50":
                        return "000350";

                    case "jw100":
                        return "0003100";

                    case "qq5":
                        return "00015";

                    case "qq10":
                        return "000110";

                    case "qq15":
                        return "000115";

                    case "qq30":
                        return "000130";

                    case "qq60":
                        return "000160";

                    case "qq100":
                        return "0001100";

                    case "cc20":
                        return "001420";

                    case "cc30":
                        return "001430";

                    case "cc50":
                        return "001450";

                    case "cc100":
                        return "0014100";

                    case "cc300":
                        return "0014300";

                    case "cc500":
                        return "0014500";

                    case "jy5":
                        return "00085";

                    case "jy10":
                        return "000810";

                    case "jy30":
                        return "000830";

                    case "jy50":
                        return "000850";

                    case "wy10":
                        return "000910";

                    case "wy15":
                        return "000915";

                    case "wy30":
                        return "000930";

                    case "wm15":
                        return "000515";

                    case "wm30":
                        return "000530";

                    case "wm50":
                        return "000550";

                    case "wm100":
                        return "0005100";

                    case "sh5":
                        return "00065";

                    case "sh10":
                        return "000610";

                    case "sh15":
                        return "000615";

                    case "sh30":
                        return "000630";

                    case "sh40":
                        return "000640";

                    case "sh100":
                        return "0006100";

                    case "dx50":
                        return "001250";

                    case "dx100":
                        return "0012100";

                    case "gy10":
                        return "001610";

                    case "gy20":
                        return "001620";

                    case "gy30":
                        return "001630";

                    case "gy50":
                        return "001650";

                    case "gy100":
                        return "0016100";

                    case "zy10":
                        return "002110";

                    case "zy15":
                        return "002115";

                    case "zy30":
                        return "002130";

                    case "zy50":
                        return "002150";

                    case "zy100":
                        return "0021100";

                    case "tx10":
                        return "002210";

                    case "tx20":
                        return "002220";

                    case "tx30":
                        return "002230";

                    case "tx40":
                        return "002240";

                    case "tx50":
                        return "002250";

                    case "tx60":
                        return "002260";

                    case "tx70":
                        return "002270";

                    case "tx80":
                        return "002280";

                    case "tx90":
                        return "002290";

                    case "tx100":
                        return "0022100";

                    case "th5":
                        return "00235";

                    case "th10":
                        return "002310";

                    case "th15":
                        return "002315";

                    case "th30":
                        return "002330";

                    case "th50":
                        return "002350";
                }
            }
            return str;
        }

        public static int ConvertChannelCode(string ver, string typeid)
        {
            if (ver == vhq10)
            {
                switch (typeid)
                {
                    case "1":
                        return 0x66;

                    case "2":
                        return 0x65;

                    case "3":
                        return 100;

                    case "4":
                        return 0x6b;

                    case "5":
                        return 0x68;

                    case "6":
                        return 0x6a;

                    case "7":
                        return 0x6f;

                    case "8":
                        return 0x70;

                    case "9":
                        return 0x69;

                    case "10":
                        return 0x6d;

                    case "11":
                        return 110;

                    case "12":
                        return 0x75;

                    case "13":
                        return 0x71;

                    case "14":
                        return 0x67;

                    case "15":
                        return 0x6c;

                    case "16":
                        return 0x74;

                    case "17":
                        return 0x73;

                    case "18":
                        return 0x76;

                    case "19":
                        return 0x77;
                }
            }
            return 0;
        }

        private static string FormatQueryString(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.GetEncoding("GB2312"));
        }

        public static string GetVersionName(string version)
        {
            string versionName = string.Empty;
            if (version == v7010)
            {
                versionName = v7010ApiName;
            }
            if (version == vbmyapi20)
            {
                versionName = PayBank20.VersionName;
            }
            if (version == phoneapi10)
            {
                return PayBank20.VersionName;
            }
            if (version == vc7010)
            {
                return vc7010ApiName;
            }
            if (version == vYee10)
            {
                return vYee10ApiName;
            }
            if (version == vcYee10)
            {
                return vcYee10ApiName;
            }
            if (version == vcYee20)
            {
                versionName = vcYee20ApiName;
            }
            return versionName;
        }

        public static bool NewBankMD5Check(string version, string partner, string orderid, string payamount, string payip, string notifyurl, string returnurl, string paytype, string remark, string key, string sign)
        {
            return (Cryptography.MD5(string.Format("version={0}&partner={1}&orderid={2}&payamount={3}&payip={4}&notifyurl={5}&returnurl={6}&paytype={7}&remark={8}&key={9}", new object[] { version, partner, orderid, payamount, payip, notifyurl, returnurl, paytype, remark, key })).ToLower() == sign);
        }

        public static string NewBankNoticeUrl(OrderBankInfo orderinfo, bool isNotify)
        {
            //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-1");
            string notifyurl = string.Empty;
            if (isNotify)
            {
                notifyurl = orderinfo.notifyurl;
            }
            else
            {
                notifyurl = orderinfo.returnurl;
            }
            //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-2");
            if ((orderinfo != null) && !string.IsNullOrEmpty(notifyurl))
            {
                //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-3");
                string opstate;
                string str3;
                UserInfo baseModel = UserFactory.GetBaseModel(orderinfo.userid);
                if (baseModel == null)
                {
                    //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-4");
                    return notifyurl;
                }
                //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-5");
                string aPIKey = baseModel.APIKey;
                string str = string.Empty;
                string version = orderinfo.version;
                string userorder = orderinfo.userorder;
                //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-6");

                StringBuilder builder = new StringBuilder();
                if ((string.IsNullOrEmpty(version) || (version == v36010)) || (version == vbapi10))
                {
                    //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-7");
                    opstate = orderinfo.opstate;
                    if (!isNotify && String.IsNullOrEmpty(opstate))
                        opstate = "0";
                    //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-8");
                    //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-10 realvalue = " + orderinfo.realvalue.ToString());

                    string str8 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    //OriginalStudio.Lib.Logging.LogHelper.Write("NewBankNoticeUrl-11");

                    str = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { userorder, opstate, str8, aPIKey }));
                    builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(userorder));
                    builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(opstate));
                    builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(str8));
                    builder.AppendFormat("&systime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    if (version == vbapi10)
                    {
                        builder.AppendFormat("&sysorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                        builder.AppendFormat("&completiontime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    }
                    builder.AppendFormat("&attach={0}", HttpUtility.UrlEncode(orderinfo.attach, Encoding.GetEncoding("GB2312")));
                    builder.AppendFormat("&msg={0}", HttpUtility.UrlEncode(orderinfo.msg, Encoding.GetEncoding("GB2312")));
                    builder.AppendFormat("&sign={0}", HttpUtility.UrlEncode(str));

                    if (OriginalStudio.Lib.SysConfig.RuntimeSetting.RecordReturnUrl)
                        OriginalStudio.Lib.Logging.LogHelper.Write("KuaiCard.BLL.SystemApiHelper通知—NewBankNoticeUrl:" + notifyurl + "?" + builder.ToString());

                    return (notifyurl + "?" + builder.ToString());
                }
                if (version == vbmyapi20)
                {
                    return PayBank20.CreateNotifyUrl(orderinfo, isNotify, aPIKey);
                }
                if (version == veypa10)
                {
                    opstate = "0";
                    if ((orderinfo.status == 2) || (orderinfo.status == 8))
                    {
                        opstate = "2";
                    }
                    str3 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    string str9 = orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                    string format = "version={0}&partner={1}&orderid={2}&payamount={3}&opstate={4}&orderno={5}&eypaltime={6}&message={7}&paytype={8}&remark={9}&key={10}";
                    format = string.Format(format, new object[] { "1.0", orderinfo.userid, userorder, str3, opstate, orderinfo.orderid, str9, "success", orderinfo.channelcode, orderinfo.attach, aPIKey });
                    MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                    str = BitConverter.ToString(provider.ComputeHash(Encoding.Default.GetBytes(format))).Replace("-", "").ToLower();
                    return ("<form name=\"frm1\" id=\"frm1\" method=\"post\" action=\"" + notifyurl + "\"><input type=\"hidden\" name=\"version\" value=\"1.0\" /><input type=\"hidden\" name=\"partner\" value=\"" + orderinfo.userid.ToString() + "\" /><input type=\"hidden\" name=\"orderid\" value=\"" + userorder + "\" /><input type=\"hidden\" name=\"payamount\" value=\"" + str3 + "\" /><input type=\"hidden\" name=\"opstate\" value=\"" + opstate + "\" /><input type=\"hidden\" name=\"eypaltime\" value=\"" + str9 + "\" /><input type=\"hidden\" name=\"message\" value=\"success\" /><input type=\"hidden\" name=\"paytype\" value=\"" + orderinfo.channelcode + "\" /><input type=\"hidden\" name=\"remark\" value=\"" + orderinfo.attach + "\" /><input type=\"hidden\" name=\"sign\" value=\"" + str + "\" /></form><script type=\"text/javascript\" language=\"javascript\">setTimeout(\"document.getElementById('frm1').submit();\",100);</script>");
                }
                if (version == v7010)
                {
                    string str11 = "11";
                    if ((orderinfo.status == 2) || (orderinfo.status == 8))
                    {
                        str11 = "1";
                    }
                    str3 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    str = Cryptography.MD5(string.Format(v7010BankNotifyVerifyStr, new object[] { str11, orderinfo.userid, userorder, aPIKey }));
                    builder.AppendFormat("returncode={0}", HttpUtility.UrlEncode(str11));
                    builder.AppendFormat("&userid={0}", HttpUtility.UrlEncode(orderinfo.userid.ToString()));
                    builder.AppendFormat("&orderid={0}", HttpUtility.UrlEncode(userorder));
                    builder.AppendFormat("&money={0}", HttpUtility.UrlEncode(str3));
                    builder.AppendFormat("&sign={0}", HttpUtility.UrlEncode(str));
                    builder.AppendFormat("&ext={0}", HttpUtility.UrlEncode(orderinfo.attach, Encoding.GetEncoding("GB2312")));
                    if (notifyurl.IndexOf("?") > 0)
                    {
                        return (notifyurl + "&" + builder.ToString());
                    }
                    return (notifyurl + "?" + builder.ToString());
                }
                if (version == vhq10)
                {
                    string str24 = decimal.Round(orderinfo.refervalue, 2).ToString();
                    string str25 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    string str26 = "116";
                    if ((orderinfo.status == 2) || (orderinfo.status == 8))
                    {
                        str26 = "0";
                    }
                    str = Cryptography.MD5(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", new object[] { orderinfo.userid, orderinfo.userorder, string.Empty, string.Empty, str24, orderinfo.cus_field2, aPIKey }));
                    builder.AppendFormat("P_UserId={0}", orderinfo.userid);
                    builder.AppendFormat("&P_OrderId={0}", orderinfo.userorder);
                    builder.AppendFormat("&P_CardId={0}", string.Empty);
                    builder.AppendFormat("&P_CardPass={0}", string.Empty);
                    builder.AppendFormat("&P_FaceValue={0}", str24);
                    builder.AppendFormat("&P_ChannelId={0}", orderinfo.cus_field2);
                    builder.AppendFormat("&P_PayMoney={0}", str25);
                    builder.AppendFormat("&P_Subject={0}", orderinfo.cus_subject);
                    builder.AppendFormat("&P_Price={0}", orderinfo.cus_price);
                    builder.AppendFormat("&P_Quantity={0}", orderinfo.cus_quantity);
                    builder.AppendFormat("&P_Description={0}", orderinfo.cus_description);
                    builder.AppendFormat("&P_Notic={0}", orderinfo.attach);
                    builder.AppendFormat("&P_ErrCode={0}", str26);
                    builder.AppendFormat("&P_ErrMsg={0}", orderinfo.msg);
                    builder.AppendFormat("&P_PostKey={0}", str);
                    if (notifyurl.IndexOf("?") > 0)
                    {
                        notifyurl = notifyurl + "&" + builder.ToString();
                    }
                    else
                    {
                        notifyurl = notifyurl + "?" + builder.ToString();
                    }
                }
            }
            return notifyurl;
        }


        public static string UrlEncode(string paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                return string.Empty;
            }
            return HttpUtility.UrlEncode(paramValue, Encoding.GetEncoding("GB2312"));
        }
    }
}

