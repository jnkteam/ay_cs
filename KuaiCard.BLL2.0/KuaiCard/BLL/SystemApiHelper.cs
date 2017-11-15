namespace KuaiCard.BLL
{
    using KuaiCard.BLL.Api;
    using KuaiCard.BLL.Sys.Transaction.YeePay;
    using KuaiCard.BLL.User;
    using KuaiCard.Model.Order;
    using KuaiCard.Model.User;
    using KuaiCardLib.Security;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Data;

    public class SystemApiHelper
    {
        public static string phoneapi10 = "phone1.00";
        public static string v36010 = "v360.10";
        public static string v36010BankNotifySuccessflag = "opstate=0";
        public static string v7010 = "vb70.10";
        public static string v7010ApiName = "70Card平台用户储值接口-版本号1.0";
        public static string v7010BankNotifySuccessflag = "ok";
        public static string v7010BankNotifyVerifyStr = "returncode={0}&userid={1}&orderid={2}&keyvalue={3}";
        public static string v7010BankReceiveVerifyStr = "userid={0}&orderid={1}&bankid={2}";
        public static string vbmyapi10 = "vb1.00";
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

        public static bool CardReceiveVerify(string version, string sign, params object[] arg)
        {
            bool flag = false;
            if (version == vc7010)
            {
                if (Cryptography.MD5(string.Format(vc7010CardReceiveVerifyStr, arg).ToLower() + string.Format("&keyvalue={0}", arg[8]), "UTF-8") == sign)
                {
                    return true;
                }
                if (Cryptography.MD5(string.Format(vc7010CardReceiveVerifyStr, arg) + string.Format("&keyvalue={0}", arg[8])).ToLower() == sign)
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static bool CheckCallBackIsSuccess(string version, string callbackText)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(callbackText))
            {
                string str = Successflag(version);
                if (callbackText.StartsWith(str) || callbackText.ToLower().StartsWith(str) || callbackText.ToLower().EndsWith("pstate=0"))  //2017.2.20修改
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static int CodeMapping(int typeid)
        {
            int num = 0;
            switch (typeid)
            {
                case 0x67:
                    return 13;

                case 0x68:
                    return 2;

                case 0x69:
                    return 7;

                case 0x6a:
                    return 3;

                case 0x6b:
                    return 1;

                case 0x6c:
                    return 14;

                case 0x6d:
                    return 8;

                case 110:
                    return 9;

                case 0x6f:
                    return 5;

                case 0x70:
                    return 6;

                case 0x71:
                    return 12;

                case 0x72:
                    return num;

                case 0x73:
                    return 0x10;

                case 0x74:
                    return 15;

                case 0x75:
                    return 0x15;

                case 0x76:
                    return 0x16;

                case 0x77:
                    return 0x17;

                case 200:
                    return 0x11;

                case 0xc9:
                    return 0x12;

                case 0xca:
                    return 0x13;

                case 0xcb:
                    return 20;

                case 0xcc:
                    return 10;

                case 0xcd:
                    return 11;

                case 0xce:
                case 0xcf:
                case 0xd0:
                case 0xd1:
                    return num;

                case 210:
                    return 0x1c;
            }
            return num;
        }

        public static string ConverBankCode(string version, string bankcode)
        {
            string str = string.Empty;
            if (!(version == v7010))
            {
                return str;
            }
            switch (bankcode)
            {
                case "1001":
                    return "970";

                case "1002":
                    return "967";

                case "1005":
                    return "964";

                case "1003":
                    return "965";

                case "1052":
                    return "963";

                case "1004":
                    return "977";

                case "1020":
                    return "981";

                case "1006":
                    return "980";

                case "1008":
                    return "974";

                case "1027":
                    return "985";

                case "1021":
                    return "962";

                case "1025":
                    return "982";

                case "1009":
                    return "972";

                case "1032":
                    return "989";

                case "1022":
                    return "986";

                case "1010":
                    return "978";

                case "1024":
                    return "975";

                case "1028":
                    return "971";

                case "1101":
                    return "992";

                case "1102":
                    return "993";
            }
            return "967";
        }

        public static string ConverCardCode(string version, string cardtype, string cardno)
        {
            string str = string.Empty;
            if (version == vc7010)
            {
                switch (cardtype)
                {
                    case "cm":
                        return "103";

                    case "sd":
                        return "104";

                    case "zt":
                        return "105";

                    case "jw":
                        return "106";

                    case "qq":
                        return "107";

                    case "cc":
                        return "108";

                    case "jy":
                        return "109";

                    case "wy":
                        return "110";

                    case "wm":
                        return "111";

                    case "sh":
                        return "112";

                    case "dx":
                        return "113";

                    case "gy":
                        return "115";

                    case "zy":
                        return "117";

                    case "tx":
                        return "118";

                    case "th":
                        return "119";
                }
            }
            return str;
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

        public static string CreateSynchronousNotifySign(string version, params object[] arg)
        {
            string str = string.Empty;
            if (version == vc7010)
            {
                str = Cryptography.MD5(string.Format(vc7010CardSynchronousNotifyVerifyStr, arg)).ToLower();
            }
            return str;
        }

        private static string FormatQueryString(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.GetEncoding("GB2312"));
        }

        public static string Get70errtype(int suppid, string errtype)
        {
            string str = string.Empty;
            if ((suppid == 0xedc2) || (suppid == 70))
            {
                return errtype;
            }
            if (suppid == 80)
            {
                switch (errtype)
                {
                    case "7":
                        return "1001";

                    case "1008":
                        return "1002";

                    case "2018":
                        return "1003";

                    case "2005":
                        return "1005";

                    case "2009":
                        return "1007";

                    case "2013":
                        return "1006";

                    case "2019":
                        return "1008";

                    case "10000":
                        return "1009";
                }
                return str;
            }
            if (suppid == 0x66)
            {
                switch (errtype)
                {
                    case "7":
                        return "1001";

                    case "1008":
                        return "1002";

                    case "1007":
                        return "1003";

                    case "1002":
                    case "1010":
                    case "2005":
                    case "2006":
                        return "1005";

                    case "2007":
                        return "1007";

                    case "2013":
                        return "1006";

                    case "1006":
                    case "1003":
                    case "2008":
                    case "2009":
                    case "2010":
                    case "2011":
                    case "2012":
                    case "2014":
                        return "1008";

                    case "10000":
                        return "1009";
                }
            }
            return str;
        }

        public static string Get70Paycardno(int _type)
        {
            string str = string.Empty;
            switch (_type)
            {
                case 0x67:
                    return "cm";

                case 0x68:
                    return "sd";

                case 0x69:
                    return "zt";

                case 0x6a:
                    return "jw";

                case 0x6b:
                    return "qq";

                case 0x6c:
                    return "cc";

                case 0x6d:
                    return "jy";

                case 110:
                    return "wy";

                case 0x6f:
                    return "wm";

                case 0x70:
                    return "sh";

                case 0x71:
                    return "dx";

                case 0x72:
                case 0x74:
                    return str;

                case 0x73:
                    return "gy";

                case 0x75:
                    return "zy";

                case 0x76:
                    return "tx";

                case 0x77:
                    return "th";
            }
            return str;
        }

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
            if (notifyurl.ToLower().StartsWith("http://127.0.0.1")) return "";
            if (notifyurl.ToLower().StartsWith("http://localhost")) return "";


            //检查地址是否在下发名单里
            if (KuaiCard.SysConfig.RuntimeSetting.SiteUser == "zft")
            {
                DataSet ds = UserFactory.GetUserBindIp(orderinfo.userid, 2);
                if (ds == null)
                { }
                else if (ds.Tables.Count == 0)
                { }
                else
                {
                    if (notifyurl.Length >7)
                        notifyurl = notifyurl.Substring(7);
                    DataTable dt = ds.Tables[0];
                }
            }

            if ((orderinfo != null) && !string.IsNullOrEmpty(notifyurl))
            {
                string opstate;
                string str3;
                UserInfo baseModel = UserFactory.GetBaseModel(orderinfo.userid);
                if (baseModel == null)
                {
                    return notifyurl;
                }
                string aPIKey = baseModel.APIKey;
                string str = string.Empty;
                string version = orderinfo.version;
                string userorder = orderinfo.userorder;
                StringBuilder builder = new StringBuilder();
                if ((string.IsNullOrEmpty(version) || (version == v36010)) || (version == vbmyapi10))
                {
                    opstate = orderinfo.opstate;
                    string str8 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    str = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { userorder, opstate, str8, aPIKey }));
                    builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(userorder));
                    builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(opstate));
                    builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(str8));
                    builder.AppendFormat("&systime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    if (version == vbmyapi10)
                    {
                        builder.AppendFormat("&sysorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                        builder.AppendFormat("&completiontime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    }
                    builder.AppendFormat("&attach={0}", HttpUtility.UrlEncode(orderinfo.attach, Encoding.GetEncoding("GB2312")));
                    builder.AppendFormat("&msg={0}", HttpUtility.UrlEncode(orderinfo.msg, Encoding.GetEncoding("GB2312")));
                    builder.AppendFormat("&sign={0}", HttpUtility.UrlEncode(str));
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
                    format = string.Format(format, new object[] { "1.0", orderinfo.userid, userorder, str3, opstate, orderinfo.orderid, str9, "success", orderinfo.paymodeId, orderinfo.attach, aPIKey });
                    MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                    str = BitConverter.ToString(provider.ComputeHash(Encoding.Default.GetBytes(format))).Replace("-", "").ToLower();
                    builder.AppendFormat("version={0}", "1.0");
                    builder.AppendFormat("&partner={0}", orderinfo.userid.ToString());
                    builder.AppendFormat("&orderid={0}", userorder);
                    builder.AppendFormat("&payamount={0}", str3);
                    builder.AppendFormat("&opstate={0}", opstate);
                    builder.AppendFormat("&orderno={0}", orderinfo.orderid);
                    builder.AppendFormat("&eypaltime={0}", str9);
                    builder.AppendFormat("&message={0}", "success");
                    builder.AppendFormat("&paytype={0}", orderinfo.paymodeId);
                    builder.AppendFormat("&remark={0}", orderinfo.attach);
                    builder.AppendFormat("&sign={0}", str);
                    if (notifyurl.IndexOf("?") > 0)
                    {
                        return (notifyurl + "&" + builder.ToString());
                    }
                    return (notifyurl + "?" + builder.ToString());
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
                if (version == vYee10)
                {
                    string str12 = "0";
                    string str13 = orderinfo.userid.ToString();
                    string str14 = "Buy";
                    if ((orderinfo.status == 2) || (orderinfo.status == 8))
                    {
                        str12 = "1";
                    }
                    string orderid = orderinfo.orderid;
                    string str16 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    string str17 = "RMB";
                    string str18 = orderinfo.cus_subject;
                    string str19 = orderinfo.userorder;
                    string str20 = "";
                    string attach = orderinfo.attach;
                    string str22 = "1";
                    if (isNotify)
                    {
                        str22 = "2";
                    }
                    string str23 = Digest.HmacSign(str13 + str14 + str12 + orderid + str16 + str17 + str18 + str19 + str20 + attach + str22, aPIKey);
                    builder.AppendFormat("p1_MerId={0}", FormatQueryString(str13));
                    builder.AppendFormat("&r0_Cmd={0}", FormatQueryString(str14));
                    builder.AppendFormat("&r1_Code={0}", FormatQueryString(str12));
                    builder.AppendFormat("&r2_TrxId={0}", FormatQueryString(orderid));
                    builder.AppendFormat("&r3_Amt={0}", FormatQueryString(str16));
                    builder.AppendFormat("&r4_Cur={0}", FormatQueryString("RMB"));
                    builder.AppendFormat("&r5_Pid={0}", FormatQueryString(str18));
                    builder.AppendFormat("&r6_Order={0}", FormatQueryString(str19));
                    builder.AppendFormat("&r7_Uid={0}", FormatQueryString(str20));
                    builder.AppendFormat("&r8_MP={0}", FormatQueryString(attach));
                    builder.AppendFormat("&r9_BType={0}", FormatQueryString(str22));
                    builder.AppendFormat("&rb_BankId={0}", FormatQueryString(orderinfo.paymodeId));
                    builder.AppendFormat("&ro_BankOrderId={0}", FormatQueryString(orderinfo.supplierOrder));
                    builder.AppendFormat("&rp_PayDate={0}", FormatQueryString(orderinfo.completetime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                    builder.AppendFormat("&ro_BankOrderId={0}", FormatQueryString(orderinfo.supplierOrder));
                    builder.AppendFormat("&rq_CardNo={0}", FormatQueryString(string.Empty));
                    builder.AppendFormat("&ru_Trxtime={0}", FormatQueryString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    builder.AppendFormat("&hmac={0}", FormatQueryString(str23));
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

            //2017.9.3 增加下游通知 IP验证
            bool ipValid = true;
            string siteuser = KuaiCard.SysConfig.RuntimeSetting.SiteUser;
            if (siteuser == "zft")
            {
                ipValid = false;
                if (!string.IsNullOrEmpty(notifyurl))
                {
                    DataSet ds = UserFactory.GetUserBindIp(orderinfo.userid, 2);
                    if ((ds != null) && (ds.Tables.Count > 0))
                    {
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            string tmpip = dr["ip"].ToString();
                            if (notifyurl.StartsWith("http://" + tmpip))
                            {
                                ipValid = true;
                                break;
                            }
                        }
                    }
                    if (!ipValid)
                        notifyurl = "";
                }
            }

            return notifyurl;
        }

        public static string GetCardBackUrl(OrderCardInfo orderinfo)
        {
            string notifyurl = string.Empty;
            notifyurl = orderinfo.notifyurl;
            if ((orderinfo != null) && !string.IsNullOrEmpty(notifyurl))
            {
                string str2;
                string str3;
                string str4;
                string str5;
                UserInfo baseModel = UserFactory.GetBaseModel(orderinfo.userid);
                if (baseModel == null)
                {
                    return notifyurl;
                }
                string aPIKey = baseModel.APIKey;
                string paramValue = string.Empty;
                string version = orderinfo.version;
                string userorder = orderinfo.userorder;
                decimal num = 0M;
                if (orderinfo.realvalue.HasValue)
                {
                    num = decimal.Round(orderinfo.realvalue.Value, 0);
                }
                string str10 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                if (orderinfo.completetime.HasValue)
                {
                    str10 = orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                }
                StringBuilder builder = new StringBuilder();
                if ((string.IsNullOrEmpty(version) || (version == v36010)) || (version == vcmyapi10))
                {
                    if (orderinfo.ismulticard == 0)
                    {
                        paramValue = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { orderinfo.userorder, orderinfo.opstate, num, aPIKey }));
                        builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(orderinfo.userorder));
                        if (orderinfo.opstate == "10")
                        {
                            orderinfo.opstate = "-1";
                        }
                        builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(orderinfo.opstate));
                        builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(num.ToString()));
                        builder.AppendFormat("&systime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                        if (version == vcmyapi10)
                        {
                            builder.AppendFormat("&sysorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                            builder.AppendFormat("&completiontime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                        }
                        builder.AppendFormat("&attach={0}", UrlEncode(orderinfo.attach));
                        builder.AppendFormat("&msg={0}", UrlEncode(orderinfo.userViewMsg));
                        builder.AppendFormat("&sign={0}", UrlEncode(paramValue));
                        return (notifyurl + "?" + builder.ToString());
                    }
                    if (orderinfo.ismulticard != 1)
                    {
                        return notifyurl;
                    }
                    paramValue = Cryptography.MD5(string.Format("orderid={0}&cardno={1}&opstate={2}&ovalue={3}&ototalvalue={4}&attach={5}&msg={6}{7}", new object[] { orderinfo.userorder, orderinfo.cardNo, orderinfo.returnopstate, orderinfo.returnovalue, num, orderinfo.attach, orderinfo.msg, aPIKey }));
                    builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(orderinfo.userorder));
                    builder.AppendFormat("&cardno={0}", HttpUtility.UrlEncode(orderinfo.cardNo));
                    builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(orderinfo.returnopstate));
                    builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(orderinfo.returnovalue));
                    builder.AppendFormat("&ototalvalue={0}", HttpUtility.UrlEncode(num.ToString()));
                    builder.AppendFormat("&attach={0}", UrlEncode(orderinfo.attach));
                    builder.AppendFormat("&msg={0}", UrlEncode(orderinfo.msg));
                    builder.AppendFormat("&ekaorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                    builder.AppendFormat("&ekatime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    if (version == vbmyapi10)
                    {
                        builder.AppendFormat("&sysorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                        builder.AppendFormat("&completiontime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    }
                    builder.AppendFormat("&sign={0}", UrlEncode(paramValue));
                    return (notifyurl + "?" + builder.ToString());
                }
                if (version == vcmyapi20)
                {
                    return SellCard20.CreateNotifyUrl(orderinfo, aPIKey);
                }
                if (version == vc7010)
                {
                    string opstate = orderinfo.opstate;
                    string str12 = "0";
                    if (opstate == "0")
                    {
                        str12 = "1";
                        opstate = "1";
                    }
                    else
                    {
                        opstate = "11";
                    }
                    string str13 = decimal.Round(orderinfo.refervalue, 0).ToString();
                    string str14 = Get70Paycardno(orderinfo.typeId);
                    string str15 = Get70Paycardno(orderinfo.typeId) + str13;
                    paramValue = Cryptography.MD5(string.Format(vc7010CardNotifyVerifyStr, new object[] { opstate, orderinfo.userid, userorder, str14, str15, orderinfo.cardNo, orderinfo.cardPwd, str13, num, str12, aPIKey })).ToLower();
                    builder.AppendFormat("returncode={0}", UrlEncode(opstate));
                    builder.AppendFormat("&userid={0}", UrlEncode(orderinfo.userid.ToString()));
                    builder.AppendFormat("&orderid={0}", UrlEncode(userorder));
                    builder.AppendFormat("&typeid={0}", UrlEncode(str14));
                    builder.AppendFormat("&productid={0}", UrlEncode(str15));
                    builder.AppendFormat("&cardno={0}", UrlEncode(orderinfo.cardNo));
                    builder.AppendFormat("&cardpwd={0}", UrlEncode(orderinfo.cardPwd));
                    builder.AppendFormat("&money={0}", UrlEncode(str13));
                    builder.AppendFormat("&realmoney={0}", UrlEncode(num.ToString()));
                    builder.AppendFormat("&cardstatus={0}", UrlEncode(str12.ToString()));
                    builder.AppendFormat("&sign={0}", UrlEncode(paramValue));
                    builder.AppendFormat("&ext={0}", UrlEncode(orderinfo.attach));
                    if (opstate == "0")
                    {
                        builder.AppendFormat("&errtype={0}", string.Empty);
                    }
                    else
                    {
                        builder.AppendFormat("&errtype={0}", Get70errtype(orderinfo.supplierId, orderinfo.errtype));
                    }
                    if (notifyurl.IndexOf("?") > 0)
                    {
                        return (notifyurl + "&" + builder.ToString());
                    }
                    return (notifyurl + "?" + builder.ToString());
                }
                if (version == vcYee10)
                {
                    bool flag = orderinfo.cus_field4 == "true";
                    str2 = "ChargeCardDirect";
                    str3 = "0";
                    if (orderinfo.status == 2)
                    {
                        str3 = "1";
                    }
                    str4 = orderinfo.userid.ToString();
                    string str16 = orderinfo.userorder;
                    string str17 = num.ToString();
                    string yeeCardcardno = GetYeeCardcardno(orderinfo.typeId);
                    string cardNo = orderinfo.cardNo;
                    string returnovalue = num.ToString();
                    string str21 = num.ToString();
                    string returnopstate = orderinfo.opstate;
                    if (returnopstate == "-1")
                    {
                        returnopstate = "1006";
                    }
                    if ((flag && (returnopstate == "0")) && (orderinfo.refervalue > num))
                    {
                        returnopstate = "1";
                        str3 = "2";
                    }
                    if (orderinfo.ismulticard == 1)
                    {
                        returnovalue = orderinfo.returnovalue;
                        str21 = orderinfo.returnovalue;
                        returnopstate = orderinfo.returnopstate;
                        bool flag2 = flag && (orderinfo.refervalue > num);
                        if (flag2)
                        {
                            str3 = "2";
                        }
                        string[] strArray = returnopstate.Split(new char[] { ',' });
                        returnopstate = string.Empty;
                        foreach (string str23 in strArray)
                        {
                            if (str23 == "0")
                            {
                                if (flag2)
                                {
                                    returnopstate = returnopstate + "1,";
                                }
                                else
                                {
                                    returnopstate = returnopstate + str23 + ",";
                                }
                            }
                            else if (str23 == "-1")
                            {
                                returnopstate = returnopstate + "1006,";
                            }
                            else
                            {
                                returnopstate = returnopstate + str23 + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(returnopstate))
                        {
                            returnopstate = returnopstate.Substring(0, returnopstate.Length - 1);
                        }
                    }
                    string attach = orderinfo.attach;
                    string str25 = "0M";
                    string str26 = "";
                    str5 = Digest.HmacSign(str2 + str3 + str4 + str16 + str17 + yeeCardcardno + cardNo + returnovalue + str21 + returnopstate + attach + str25 + str26, aPIKey);
                    builder.AppendFormat("r0_Cmd={0}", UrlEncode(str2));
                    builder.AppendFormat("&r1_Code={0}", UrlEncode(str3));
                    builder.AppendFormat("&p1_MerId={0}", UrlEncode(str4));
                    builder.AppendFormat("&p2_Order={0}", UrlEncode(str16));
                    builder.AppendFormat("&p3_Amt={0}", UrlEncode(str17));
                    builder.AppendFormat("&p4_FrpId={0}", UrlEncode(yeeCardcardno));
                    builder.AppendFormat("&p5_CardNo={0}", UrlEncode(cardNo));
                    builder.AppendFormat("&p6_confirmAmount={0}", UrlEncode(returnovalue));
                    builder.AppendFormat("&p7_realAmount={0}", UrlEncode(str21));
                    builder.AppendFormat("&p8_cardStatus={0}", UrlEncode(returnopstate));
                    builder.AppendFormat("&p9_MP={0}", UrlEncode(attach));
                    builder.AppendFormat("&pb_BalanceAmt={0}", UrlEncode(str25));
                    builder.AppendFormat("&pc_BalanceAct={0}", UrlEncode(str26));
                    builder.AppendFormat("&hmac={0}", UrlEncode(str5));
                    if (notifyurl.IndexOf("?") > 0)
                    {
                        return (notifyurl + "&" + builder.ToString());
                    }
                    return (notifyurl + "?" + builder.ToString());
                }
                if (version == vcYee20)
                {
                    str2 = "AnnulCard";
                    str3 = "0";
                    if (orderinfo.status == 2)
                    {
                        str3 = "1";
                    }
                    string str27 = orderinfo.userorder;
                    string orderid = orderinfo.orderid;
                    string str29 = orderinfo.attach;
                    string str30 = num.ToString();
                    string str31 = orderinfo.cardNo;
                    str4 = orderinfo.userid.ToString();
                    str5 = Digest.HmacSign(str2 + str3 + str4 + str27 + orderid + str29 + str30, aPIKey);
                    builder.AppendFormat("r0_Cmd={0}", UrlEncode(str2));
                    builder.AppendFormat("&r1_Code={0}", UrlEncode(str3));
                    builder.AppendFormat("&rb_Order={0}", UrlEncode(str27));
                    builder.AppendFormat("&r2_TrxId={0}", UrlEncode(orderid));
                    builder.AppendFormat("&pa_MP={0}", UrlEncode(str29));
                    builder.AppendFormat("&rc_Amt={0}", UrlEncode(str30));
                    builder.AppendFormat("&rq_CardNo={0}", UrlEncode(str31));
                    builder.AppendFormat("&hmac={0}", UrlEncode(str5));
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

        public static string GetVersionName(string version)
        {
            string versionName = string.Empty;
            if (version == v7010)
            {
                versionName = v7010ApiName;
            }
            if (version == vcmyapi20)
            {
                versionName = SellCard20.VersionName;
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

        public static string GetYeeCardcardno(int _type)
        {
            string str = string.Empty;
            switch (_type)
            {
                case 0x67:
                    return "SZX";

                case 0x68:
                    return "SNDACARD";

                case 0x69:
                    return "ZHENGTU";

                case 0x6a:
                    return "JUNNET";

                case 0x6b:
                    return "QQCARD";

                case 0x6c:
                    return "UNICOM";

                case 0x6d:
                    return "JIUYOU";

                case 110:
                    return "NETEASE";

                case 0x6f:
                    return "WANMEI";

                case 0x70:
                    return "SOHU";

                case 0x71:
                    return "TELECOM";

                case 0x72:
                case 0x73:
                case 0x74:
                    return str;

                case 0x75:
                    return "ZONGYOU";

                case 0x76:
                    return "TIANXIA";

                case 0x77:
                    return "TIANHONG";
            }
            return str;
        }

        public static bool NewBankMD5Check(string version, string partner, string orderid, string payamount, string payip, string notifyurl, string returnurl, string paytype, string remark, string key, string sign)
        {
            return (Cryptography.MD5(string.Format("version={0}&partner={1}&orderid={2}&payamount={3}&payip={4}&notifyurl={5}&returnurl={6}&paytype={7}&remark={8}&key={9}", new object[] { version, partner, orderid, payamount, payip, notifyurl, returnurl, paytype, remark, key })).ToLower() == sign);
        }

        public static string NewBankNoticeUrl(OrderBankInfo orderinfo, bool isNotify)
        {
            //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-1");
            string notifyurl = string.Empty;
            if (isNotify)
            {
                notifyurl = orderinfo.notifyurl;
            }
            else
            {
                notifyurl = orderinfo.returnurl;
            }
            //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-2");
            if ((orderinfo != null) && !string.IsNullOrEmpty(notifyurl))
            {
                //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-3");
                string opstate;
                string str3;
                UserInfo baseModel = UserFactory.GetBaseModel(orderinfo.userid);
                if (baseModel == null)
                {
                    //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-4");
                    return notifyurl;
                }
                //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-5");
                string aPIKey = baseModel.APIKey;
                string str = string.Empty;
                string version = orderinfo.version;
                string userorder = orderinfo.userorder;
                //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-6");

                StringBuilder builder = new StringBuilder();
                if ((string.IsNullOrEmpty(version) || (version == v36010)) || (version == vbmyapi10))
                {
                    //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-7");
                    opstate = orderinfo.opstate;
                    if (!isNotify && String.IsNullOrEmpty(opstate))
                        opstate = "0";
                    //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-8");
                    //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-10 realvalue = " + orderinfo.realvalue.ToString());

                    string str8 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    //KuaiCardLib.Logging.LogHelper.Write("NewBankNoticeUrl-11");

                    str = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { userorder, opstate, str8, aPIKey }));
                    builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(userorder));
                    builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(opstate));
                    builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(str8));
                    builder.AppendFormat("&systime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    if (version == vbmyapi10)
                    {
                        builder.AppendFormat("&sysorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                        builder.AppendFormat("&completiontime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    }
                    builder.AppendFormat("&attach={0}", HttpUtility.UrlEncode(orderinfo.attach, Encoding.GetEncoding("GB2312")));
                    builder.AppendFormat("&msg={0}", HttpUtility.UrlEncode(orderinfo.msg, Encoding.GetEncoding("GB2312")));
                    builder.AppendFormat("&sign={0}", HttpUtility.UrlEncode(str));

                    if (KuaiCard.SysConfig.RuntimeSetting.RecordReturnUrl)
                        KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.SystemApiHelper通知—NewBankNoticeUrl:" + notifyurl + "?" + builder.ToString());

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
                    format = string.Format(format, new object[] { "1.0", orderinfo.userid, userorder, str3, opstate, orderinfo.orderid, str9, "success", orderinfo.paymodeId, orderinfo.attach, aPIKey });
                    MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                    str = BitConverter.ToString(provider.ComputeHash(Encoding.Default.GetBytes(format))).Replace("-", "").ToLower();
                    return ("<form name=\"frm1\" id=\"frm1\" method=\"post\" action=\"" + notifyurl + "\"><input type=\"hidden\" name=\"version\" value=\"1.0\" /><input type=\"hidden\" name=\"partner\" value=\"" + orderinfo.userid.ToString() + "\" /><input type=\"hidden\" name=\"orderid\" value=\"" + userorder + "\" /><input type=\"hidden\" name=\"payamount\" value=\"" + str3 + "\" /><input type=\"hidden\" name=\"opstate\" value=\"" + opstate + "\" /><input type=\"hidden\" name=\"eypaltime\" value=\"" + str9 + "\" /><input type=\"hidden\" name=\"message\" value=\"success\" /><input type=\"hidden\" name=\"paytype\" value=\"" + orderinfo.paymodeId + "\" /><input type=\"hidden\" name=\"remark\" value=\"" + orderinfo.attach + "\" /><input type=\"hidden\" name=\"sign\" value=\"" + str + "\" /></form><script type=\"text/javascript\" language=\"javascript\">setTimeout(\"document.getElementById('frm1').submit();\",100);</script>");
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
                if (version == vYee10)
                {
                    string str12 = "0";
                    string str13 = orderinfo.userid.ToString();
                    string str14 = "Buy";
                    if ((orderinfo.status == 2) || (orderinfo.status == 8))
                    {
                        str12 = "1";
                    }
                    string orderid = orderinfo.orderid;
                    string str16 = decimal.Round(orderinfo.realvalue.Value, 2).ToString();
                    string str17 = "RMB";
                    string str18 = orderinfo.cus_subject;
                    string str19 = orderinfo.userorder;
                    string str20 = "";
                    string attach = orderinfo.attach;
                    string str22 = "1";
                    if (isNotify)
                    {
                        str22 = "2";
                    }
                    string str23 = Digest.HmacSign(str13 + str14 + str12 + orderid + str16 + str17 + str18 + str19 + str20 + attach + str22, aPIKey);
                    builder.AppendFormat("p1_MerId={0}", FormatQueryString(str13));
                    builder.AppendFormat("&r0_Cmd={0}", FormatQueryString(str14));
                    builder.AppendFormat("&r1_Code={0}", FormatQueryString(str12));
                    builder.AppendFormat("&r2_TrxId={0}", FormatQueryString(orderid));
                    builder.AppendFormat("&r3_Amt={0}", FormatQueryString(str16));
                    builder.AppendFormat("&r4_Cur={0}", FormatQueryString("RMB"));
                    builder.AppendFormat("&r5_Pid={0}", FormatQueryString(str18));
                    builder.AppendFormat("&r6_Order={0}", FormatQueryString(str19));
                    builder.AppendFormat("&r7_Uid={0}", FormatQueryString(str20));
                    builder.AppendFormat("&r8_MP={0}", FormatQueryString(attach));
                    builder.AppendFormat("&r9_BType={0}", FormatQueryString(str22));
                    builder.AppendFormat("&rb_BankId={0}", FormatQueryString(orderinfo.paymodeId));
                    builder.AppendFormat("&ro_BankOrderId={0}", FormatQueryString(orderinfo.supplierOrder));
                    builder.AppendFormat("&rp_PayDate={0}", FormatQueryString(orderinfo.completetime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                    builder.AppendFormat("&ro_BankOrderId={0}", FormatQueryString(orderinfo.supplierOrder));
                    builder.AppendFormat("&rq_CardNo={0}", FormatQueryString(string.Empty));
                    builder.AppendFormat("&ru_Trxtime={0}", FormatQueryString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    builder.AppendFormat("&hmac={0}", FormatQueryString(str23));
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

        public static string NewConverBankCode(string bankcode)
        {
            switch (bankcode)
            {
                case "CMB":
                    return "970";

                case "ICBC":
                    return "967";

                case "ABC":
                    return "964";

                case "CCB":
                    return "965";

                case "BOC":
                    return "963";

                case "SPDB":
                    return "977";

                case "BOCM":
                    return "981";

                case "CMBC":
                    return "980";

                case "SDD":
                    return "974";

                case "CGB":
                    return "985";

                case "CTITC":
                    return "962";

                case "HXB":
                    return "982";

                case "CIB":
                    return "972";

                case "BCCB":
                    return "989";

                case "CEB":
                    return "986";

                case "SDB":
                    return "978";

                case "SHBANK":
                    return "975";

                case "PSBC":
                    return "971";

                case "1101":
                    return "992";

                case "1102":
                    return "993";

                case "UNION":
                    return "996";

                case "SHNS":
                    return "976";

                case "BOHAI":
                    return "998";

                case "ALIPAY":
                    return "992";

                case "TENPAY":
                    return "993";

                case "WECHAT":
                    return "1004";
            }
            return "UNION";
        }

        public static string Successflag(string version)
        {
            string str = string.Empty;
            if (((string.IsNullOrEmpty(version) || (version == vbmyapi10)) || ((version == v36010) || (version == vcmyapi10))) || (version == phoneapi10))
            {
                return v36010BankNotifySuccessflag;
            }
            if (version == vcmyapi20)
            {
                return SellCard20.Successflag;
            }
            if (version == phoneapi10)
            {
                return SellCard20.Successflag;
            }
            if (version == v7010)
            {
                return v7010BankNotifySuccessflag;
            }
            if (version == vc7010)
            {
                return vc7010CardNotifySuccessflag;
            }
            if (version == vYee10)
            {
                return vYee10BankNotifySuccessflag;
            }
            if (version == vcYee10)
            {
                return vcYee10NotifySuccessflag;
            }
            if (version == vcYee20)
            {
                return vcYee20NotifySuccessflag;
            }
            if (version == vhq10)
            {
                str = vhq10NotifySuccessflag;
            }
            return str;
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

