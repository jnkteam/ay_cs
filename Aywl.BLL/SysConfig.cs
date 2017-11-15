namespace OriginalStudio.BLL
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.BLL.Sys;

    /// <summary>
    /// 系统参数对象。
    /// </summary>
    public class SysConfig
    {
        internal static string SQL_TABLE = "sys_options";
        internal static string SQL_TABLE_FIELD = "option_code,option_value";
        public static string SYSCONFIG_CACHEKEY = (Constant.Cache_Mark + "SYSCONFIG");

        /// <summary>
        /// 清除系统参数缓存
        /// </summary>
        internal static void ClearCache()
        {
            string objId = SYSCONFIG_CACHEKEY;
            WebCache.GetCacheService().RemoveObject(objId);
        }

        /// <summary>
        /// 获取系统缓存列表
        /// </summary>
        /// <returns></returns>
        public static DataSet GetCacheList()
        {
            try
            {
                string objId = SYSCONFIG_CACHEKEY;
                DataSet o = new DataSet();
                o = (DataSet) WebCache.GetCacheService().RetrieveObject(objId);
                if (o == null)
                {
                    SqlDependency dependency = DataBase.AddSqlDependency(objId, SQL_TABLE, SQL_TABLE_FIELD, string.Empty, null);
                    StringBuilder builder = new StringBuilder();
                    builder.Append(" select option_code,option_type,option_value,value_type from sys_options");
                    o = DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
                    WebCache.GetCacheService().AddObject(objId, o);
                }
                return o;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static string GetOptionValue(string p_option_code, string p_defalut = "")
        {
            try
            {
                DataSet cacheList = GetCacheList();
                if (cacheList == null)
                {
                    return p_defalut;
                }
                DataRow[] rowArray = cacheList.Tables[0].Select("option_code='" + p_option_code + "'");
                if ((rowArray == null) || (rowArray.Length < 1))
                {
                    return p_defalut;
                }
                return rowArray[0]["option_value"].ToString();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return p_defalut;
            }
        }

        public static string GetValue(int p_option_code)
        {
            try
            {
                DataSet cacheList = GetCacheList();
                if (cacheList == null)
                {
                    return string.Empty;
                }
                DataRow[] rowArray = cacheList.Tables[0].Select("option_code='" + p_option_code.ToString() + "'");
                if ((rowArray == null) || (rowArray.Length < 1))
                {
                    return string.Empty;
                }
                return rowArray[0]["option_value"].ToString();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return string.Empty;
            }
        }

        public static bool Update(int id, string value)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("update SysConfig set ");
                builder.Append("value=@value");
                builder.Append(" where id=@id");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@value", SqlDbType.VarChar, 100), new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = value;
                commandParameters[1].Value = id;
                ClearCache();
                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #region 正在使用
        
        /// <summary>
        /// 启用随机扣量。
        /// </summary>
        public static bool isOpenDeduct
        {
            get
            {
                return GetOptionValue("OpenDeduct", "0") == "1";    // (GetValue(42) == "1");
            }
        }

        #endregion

        #region 取值

        public static decimal alilimit
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x4f)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x4f));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static DateTime ApiDocsUpdateTime
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(GetValue(0x30));
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
        }

        public static decimal banklimit
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x4d)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x4d));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static int BankPaySupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(5)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(5));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int CashTimesEveryDay
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x1b)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(0x1b));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int CashTimesEveryDay1
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x1f)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(0x1f));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int CashTimesEveryDay2
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x23)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(0x23));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static decimal Charges1
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x20)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x20));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal Charges2
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x24)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x24));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal ChargesRate
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x1c)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x1c));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static string closecashReason
        {
            get
            {
                return GetValue(0x2d);
            }
        }

        public static decimal DayMaxGetMoney
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x33)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x33));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static bool debuglog
        {
            get
            {
                try
                {
                    return (Convert.ToInt32(GetValue(0x34)) == 1);
                }
                catch
                {
                    return true;
                }
            }
        }

        public static int DefaultCardPaySupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(6)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(6));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int DefaultCPSDrate
        {
            get
            {
                try
                {
                    return Convert.ToInt32(GetValue(0x2b));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static int DefaultSettledMode
        {
            get
            {
                try
                {
                    return Convert.ToInt32(GetValue(0x2e));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static bool IsAudit
        {
            get
            {
                return (GetValue(1) == "1");
            }
        }

        public static bool isopenCash
        {
            get
            {
                return (GetValue(0x2c) == "1");
            }
        }


        public static bool IsOpenNoLaiLu
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x26)))
                    {
                        return false;
                    }
                    return (Convert.ToInt32(GetValue(0x26)) == 1);
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return false;
                }
            }
        }

        public static bool IsOpenRegistration
        {
            get
            {
                return (GetValue(2) == "1");
            }
        }

        public static bool IsPhoneVerification
        {
            get
            {
                return (GetValue(3) == "1");
            }
        }

        public static string isUserloginByEmail
        {
            get
            {
                return GetValue(0x45);
            }
        }

        public static int LaiLuCount
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x25)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(0x25));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static string MailDisplayName
        {
            get
            {
                return GetValue(0x44);
            }
        }

        public static string MailDomain
        {
            get
            {
                return GetValue(0x3d);
            }
        }

        public static int MailDomainPort
        {
            get
            {
                try
                {
                    return Convert.ToInt32(GetValue(0x3e));
                }
                catch
                {
                    return 0x19;
                }
            }
        }

        public static string MailFrom
        {
            get
            {
                return GetValue(0x42);
            }
        }

        public static string MailIsSsl
        {
            get
            {
                return GetValue(0x43);
            }
        }

        public static string MailServerDisplayName
        {
            get
            {
                return GetValue(0x41);
            }
        }

        public static string MailServerPassWord
        {
            get
            {
                return GetValue(0x40);
            }
        }

        public static string MailServerUserName
        {
            get
            {
                return GetValue(0x3f);
            }
        }

        public static decimal MaxCharges
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(50)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(50));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal MaxGetMoney
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x1a)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x1a));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal MaxGetMoney1
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(30)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(30));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal MaxGetMoney2
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x22)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x22));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static int MaxInformationNumber
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(4)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(4));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static decimal MinCharges
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x31)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x31));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal MinGetMoney
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x19)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x19));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal MinGetMoney1
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x1d)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x1d));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static decimal MinGetMoney2
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x21)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x21));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static int PayDianXinSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x11)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(0x11));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayJiuYouSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(13)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(13));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayJuWangSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(10)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(10));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayLianTongSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(12)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(12));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayQQSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(11)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(11));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayShengDaSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(8)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(8));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayShenZhouXingSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(7)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(7));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayShuHuSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x10)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(0x10));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayWangYiSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(14)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(14));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayWanMeiSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(15)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(15));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static int PayZhengTuSupId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(9)))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(GetValue(9));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0;
                }
            }
        }

        public static bool radioButtonemail
        {
            get
            {
                return (GetValue(0x4c) == "1");
            }
        }

        public static bool radioButtonPhone
        {
            get
            {
                return (GetValue(0x47) == "1");
            }
        }

        public static bool radioButtonshouji
        {
            get
            {
                return (GetValue(0x4b) == "1");
            }
        }

        public static bool RegistrationActivationByEmail
        {
            get
            {
                try
                {
                    return (Convert.ToInt32(GetValue(0x2f)) == 1);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string sms_caiwu_tocash
        {
            get
            {
                return Convert.ToString(GetValue(0x4a));
            }
        }

        public static string sms_caiwu_tocash2
        {
            get
            {
                return Convert.ToString(GetValue(0x49));
            }
        }

        public static string sms_temp_Authenticate
        {
            get
            {
                return Convert.ToString(GetValue(0x36));
            }
        }

        public static string sms_temp_FindPwd
        {
            get
            {
                return Convert.ToString(GetValue(0x38));
            }
        }

        public static string sms_temp_Modify
        {
            get
            {
                return Convert.ToString(GetValue(0x37));
            }
        }

        public static string sms_temp_Register
        {
            get
            {
                return Convert.ToString(GetValue(0x35));
            }
        }

        public static string sms_temp_tocash
        {
            get
            {
                return Convert.ToString(GetValue(60));
            }
        }

        public static string textPhone
        {
            get
            {
                return GetValue(0x48);
            }
        }

        public static string UserloginMsgForCheckfail
        {
            get
            {
                return GetValue(0x29);
            }
        }

        public static string UserloginMsgForlock
        {
            get
            {
                return GetValue(0x27);
            }
        }

        public static string UserloginMsgForUnCheck
        {
            get
            {
                return GetValue(40);
            }
        }

        public static string WebSitedescription
        {
            get
            {
                return GetValue(0x3b);
            }
        }

        public static string WebSiteKey
        {
            get
            {
                return GetValue(0x3a);
            }
        }

        public static string WebSiteTitleSuffix
        {
            get
            {
                return GetValue(0x39);
            }
        }

        public static decimal weixinlimit
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetValue(0x4e)))
                    {
                        return 0M;
                    }
                    return Convert.ToDecimal(GetValue(0x4e));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public static Boolean TiXianNeedCustValid
        {
            get
            {
                try
                {
                    return GetValue(80) == "1";
                }
                catch
                {
                    return true;
                }
            }
        }

        public static string UserloginLimitIPCheckfail
        {
            get
            {
                try
                {
                    return GetValue(81);
                }
                catch
                {
                    return "非法登录";
                }
            }
        }

        #endregion
    }
}

