﻿namespace OriginalStudio.BLL.SMS
{
    //using com.todaynic.ScpClient;
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Text;
    using OriginalStudio.BLL.SMS;

    public class SMS
    {
        public static bool Send(string mobile, string msg, string type)
        {
            //string sMSUser = RuntimeSetting.SMSUser;
            //string sMSPassword = RuntimeSetting.SMSPassword;
            //string hostPort = RuntimeSetting.HostPort;
            //SMSClient client = new SMSClient(RuntimeSetting.HostName, Convert.ToInt32(hostPort), sMSUser, sMSPassword);
            //return client.sendSMS(mobile, msg, type);
            return false;
        }

        public static string SendJXTWithCheck(string uid, string pwd, string URL, int maxSendTimes, string mobile, string msg, string type)
        {
            return "";
            /*
            string str = string.Empty;
            if (!PageValidate.IsMobile(mobile))
            {
                return "您输入的手机号码不正确！请重新输入。";
            }
            bool flag = false;
            if (!PhoneValidFactory.isLimited(mobile))
            {
                flag = true;
            }
            else if (PhoneValidFactory.SendCount(mobile) < maxSendTimes)
            {
                flag = true;
            }
            if (!flag)
            {
                return "抱歉，你输入的手机发送次数已达到最大允许次数！";
            }
            if (SendtoPost(uid, pwd, mobile, msg, type))
            {
                //PhoneValidLog model = new PhoneValidLog();
                //model.phone = mobile;
                //model.sendTime = DateTime.Now;
                //model.code = msg;
                //model.clientIP = ServerVariables.TrueIP;
                //PhoneValidFactory.Add(model);
                return str;
            }
            return "验证码发送失败，请联系管理员！";*/
        }

        public static string SendSmsWithCheck(string mobile, string msg, string type)
        {
            string sMUID = RuntimeSetting.SMUID;
            string sMPWD = RuntimeSetting.SMPWD;
            string jXTURL = RuntimeSetting.JXTURL;
            int maxInformationNumber = 1000000;  //短信最多发送次数，这个不取配置里的信息 SysConfig.MaxInformationNumber;
            //return SendJXTWithCheck(sMUID, sMPWD, jXTURL, maxInformationNumber, mobile, msg, type);
            return "";    //测试用，成功返回空
        }

        public static string SendSmsWithCheck(string sn, string pwd, int maxSendTimes, string mobile, string msg, string type)
        {
            return "";
            /*
            string str = string.Empty;
            if (!PageValidate.IsMobile(mobile))
            {
                return "您输入的手机号码不正确！请重新输入。";
            }
            bool flag = false;
            if (!PhoneValidFactory.isLimited(mobile))
            {
                flag = true;
            }
            else if (PhoneValidFactory.SendCount(mobile) < maxSendTimes)
            {
                flag = true;
            }
            if (!flag)
            {
                return "抱歉，你输入的手机发送次数已达到最大允许次数！";
            }
            if (SendtoSupp(sn, pwd, mobile, msg, type))
            {
                PhoneValidLog model = new PhoneValidLog();
                model.phone = mobile;
                model.sendTime = DateTime.Now;
                model.code = msg;
                model.clientIP = ServerVariables.TrueIP;
                PhoneValidFactory.Add(model);
                return str;
            }
            return "验证码发送失败，请联系管理员！";
            */
        }

        public static string SendSmsWithCheck2(string mobile, string msg, string type)
        {
            return "";
            /*
            string sMSUser = RuntimeSetting.SMSUser;
            string sMSPassword = RuntimeSetting.SMSPassword;
            string hostPort = RuntimeSetting.HostPort;
            string hostName = RuntimeSetting.HostName;
            int maxInformationNumber = SysConfig.MaxInformationNumber;
            return SendSmsWithCheck(sMSUser, sMSPassword, maxInformationNumber, mobile, msg, type);*/
        }

        public static bool SendtoPost(string uid, string pwd, string mobile, string msg, string ext)
        {
            return false;
            //string jXTURL = RuntimeSetting.JXTURL;
            //StringBuilder builder = new StringBuilder();
            //builder.AppendFormat("id={0}", uid);
            //builder.AppendFormat("&pwd={0}", pwd);
            //builder.AppendFormat("&to={0}", mobile);
            //builder.AppendFormat("&content={0}", msg);
            //builder.AppendFormat("&time={0}", ext);
            //return (WebClientHelper.GetString(jXTURL, builder.ToString(), "POST", Encoding.GetEncoding("gb2312"), 0x2710).Split(new char[] { '/' })[0] == "000");
        }

        public static bool SendtoSupp(string name, string pwd, string mobile, string msg, string ext)
        {
            return false;
            //string hostPort = RuntimeSetting.HostPort;
            //SMSClient client = new SMSClient(RuntimeSetting.HostName, Convert.ToInt32(hostPort), name, pwd);
            //return client.sendSMS(mobile, msg, ext);
        }
    }
}

