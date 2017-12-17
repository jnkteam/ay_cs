namespace OriginalStudio.BLL.User
{
    using DBAccess;
    using OriginalStudio.BLL;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.Lib.Utils;

    public class MchUserFactory
    {
        public static string USER_CACHE_KEY = (Constant.Cache_Mark + "USER_{0}");
        internal const string USER_CONTEXT_KEY = "{FD7BE212-8537-427f-9EF6-1D1AABCA8EA3}";
        internal const string USER_LOGIN_CLIENT_SESSIONID = "{2A1FA22C-201B-471c-B668-2FCC1C4A121A}";
        internal const string USER_LOGIN_SESSIONID = "{10E6C4EE-54C1-4895-8CDE-202A5B3DD9E9}";

        #region 清除缓存信息

        internal static void ClearCache(int userId)
        {
            string objId = string.Format(USER_CACHE_KEY, userId);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        internal static void ClearCache(Int64 userId)
        {
            string objId = string.Format(USER_CACHE_KEY, userId);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        #endregion

        #region 获取用户信息

        /// <summary>
        /// Ds转基本信息对象
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private static MchUserBaseInfo GetUserModelFromDs(DataSet ds)
        {
            MchUserBaseInfo modle = new MchUserBaseInfo();
            if (ds.Tables[0].Rows.Count == 0) return modle;

            DataRow dr = ds.Tables[0].Rows[0];

            modle.UserID = Convert.ToInt32(dr["userid"]);
            modle.ClassID = Utils.StrToInt(dr["classid"].ToString(), 0);
            modle.UserName = Convert.ToString(dr["username"]);
            modle.UserPwd = Convert.ToString(dr["userpwd"]);
            modle.UserPayPwd = Convert.ToString(dr["userpaypwd"]);
            modle.MerchantName = Convert.ToString(dr["merchantname"]);
            modle.ApiKey = Convert.ToString(dr["apikey"]);
            modle.ContactName = Convert.ToString(dr["contactname"]);
            modle.IDCard = Convert.ToString(dr["idcard"]);
            modle.Phone = Convert.ToString(dr["phone"]);
            modle.EMail = Convert.ToString(dr["email"]);
            modle.QQ = Convert.ToString(dr["qq"]);
            modle.IsPhone = Utils.StrToInt(dr["isphone"].ToString(), 0) == 1;
            modle.IsEmail = Utils.StrToInt(dr["isemail"].ToString(), 0) == 1;
            modle.IsRealName = Utils.StrToInt(dr["isrealname"].ToString(), 0) == 1;
            modle.WithdrawSchemeID = Utils.StrToInt(dr["withdrawschemeid"].ToString(), 0);
            modle.PayRateID = Utils.StrToInt(dr["payrateid"].ToString(), 0);
            modle.MaxDayWithdrawTimes = Utils.StrToInt(dr["maxdaywithdrawtimes"].ToString(), 0);
            modle.FirstLoginIP = Convert.ToString(dr["firstloginip"]);
            modle.FirstLoginMac = Convert.ToString(dr["firstloginmac"]);
            modle.FirstLoginTime = Utils.StrToDateTime(dr["firstlogintime"]);
            modle.LastLoginIP = Convert.ToString(dr["lastloginip"]);
            modle.LastLoginMAC = Convert.ToString(dr["lastloginmac"]);
            modle.LastLoginTime = Utils.StrToDateTime(dr["lastlogintime"]);
            modle.SessionID = Convert.ToString(dr["sessionid"]);
            modle.Status = Utils.StrToInt(dr["status"].ToString(), 0);
            modle.AddTime = String.IsNullOrEmpty(dr["AddTime"].ToString()) ? DateTime.Now : Convert.ToDateTime(dr["AddTime"].ToString());
            modle.Company = Convert.ToString(dr["company"]);
            modle.LinkMan = Convert.ToString(dr["linkman"]);
            modle.WithdrawType = Utils.StrToInt(dr["withdrawtype"].ToString(), 0);
            modle.RandomProduct = Utils.StrToInt(dr["randomproduct"].ToString(), 0);
            modle.ManageId = Utils.StrToInt(dr["manageid"].ToString(), 0);
            modle.SiteUrl = Convert.ToString(dr["siteurl"]);
            modle.FrontPic = Convert.ToString(dr["frontpic"]);
            modle.VersoPic = Convert.ToString(dr["versopic"]);
            modle.DefaultThemes = Convert.ToString(dr["defaultthemes"]);
            modle.IsDebug = Utils.StrToInt(dr["isdebug"].ToString(), 0);
            modle.AgentID = Utils.StrToInt(dr["agentid"].ToString(), 0);
            modle.UserType = (UserTypeEnum)(Utils.StrToInt(dr["usertype"].ToString(), 0));
            modle.UserLevel = (UserLevelEnum)(Utils.StrToInt(dr["userlevel"].ToString(), 0));

            modle.MchUsersAmtInfo.Integral = Utils.StrToLong(dr["integral"].ToString(), 0);
            modle.MchUsersAmtInfo.Freeze = Utils.StrToDecimal(dr["freeze"].ToString(), 0);
            modle.MchUsersAmtInfo.Balance = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.MchUsersAmtInfo.Payment = Utils.StrToDecimal(dr["Payment"].ToString(), 0);
            modle.MchUsersAmtInfo.UnPayment = Utils.StrToDecimal(dr["UnPayment"].ToString(), 0);
            modle.MchUsersAmtInfo.UnPayment2 = Utils.StrToDecimal(dr["unpayment2"].ToString(), 0);
            modle.MchUsersAmtInfo.EnableAmt = Utils.StrToDecimal(dr["enableAmt"].ToString(), 0);

            modle.WithdrawScheme.ID = Utils.StrToInt(dr["SchemeID"].ToString(), 0); ;
            modle.WithdrawScheme.Type = Utils.StrToInt(dr["SchemeneType"].ToString(), 0); ;
            modle.WithdrawScheme.SchemeName = Convert.ToString(dr["schemename"]);
            modle.WithdrawScheme.SingleMinAmtLimit = Utils.StrToDecimal(dr["SingleMinAmtLimit"].ToString(), 0);
            modle.WithdrawScheme.SingleMaxAmtLimit = Utils.StrToDecimal(dr["SingleMaxAmtLimit"].ToString(), 0);
            modle.WithdrawScheme.DailyMaxTimes = Utils.StrToInt(dr["dailymaxtimes"],0);
            modle.WithdrawScheme.DailyMaxAmt = Utils.StrToDecimal(dr["DailyMaxAmt"].ToString(), 0);
            modle.WithdrawScheme.ChargeRate = Utils.StrToDecimal(dr["ChargeRate"].ToString(), 0);
            modle.WithdrawScheme.SingleMinCharge = Utils.StrToDecimal(dr["SingleMinCharge"].ToString(), 0);
            modle.WithdrawScheme.SingleMaxCharge = Utils.StrToDecimal(dr["SingleMaxCharge"].ToString(), 0);
            modle.WithdrawScheme.IsTranApi = Utils.StrToInt(dr["istranapi"]);
            modle.WithdrawScheme.IsDefault = Utils.StrToInt(dr["IsDefault"].ToString(), 0);
            modle.WithdrawScheme.IsSys = Utils.StrToInt(dr["IsSys"].ToString(), 0);
            modle.WithdrawScheme.BankDetentionDays = Utils.StrToInt(dr["bankdetentiondays"]);
            modle.WithdrawScheme.QQDetentionDays = Utils.StrToInt(dr["qqdetentiondays"]);
            modle.WithdrawScheme.JDDetentionDays = Utils.StrToInt(dr["jddetentiondays"]);
            modle.WithdrawScheme.IsTranRequiredAudit = Utils.StrToInt(dr["IsTranRequiredAudit"]);
            modle.WithdrawScheme.AlipayDetentionDays = Utils.StrToInt(dr["alipaydetentiondays"]);
            modle.WithdrawScheme.WeiXinDetentionDays = Utils.StrToInt(dr["weixindetentiondays"]);
            modle.WithdrawScheme.OtherDetentionDays = Utils.StrToInt(dr["otherdetentiondays"]);
            modle.WithdrawScheme.TranSupplier = Utils.StrToInt(dr["TranSupplier"]);           

            return modle;
        }

        /// <summary>
        /// 根据userID获取用户信息。
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserBaseByUserID(int userID, Boolean fromCache = false)
        {
            MchUserBaseInfo o = null;
            if (fromCache)
            {
                string objId = string.Format(USER_CACHE_KEY, userID);
                o = (MchUserBaseInfo)WebCache.GetCacheService().RetrieveObject(objId);
            }
            if (o == null)
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@UserID", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = userID;
                return GetUserModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_getByUserID", commandParameters));
            }
            return o;
        }

        /// <summary>
        /// 根据userName获取用户信息。
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserBaseByUserName(string userName, Boolean fromCache = false)
        {
            MchUserBaseInfo o = null;
            if (fromCache)
            {
                string objId = string.Format(USER_CACHE_KEY, userName);
                o = (MchUserBaseInfo)WebCache.GetCacheService().RetrieveObject(objId);
            }
            if (o == null)
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@userName", SqlDbType.VarChar, 20)
                };
                commandParameters[0].Value = userName;
                return GetUserModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_users_getbyname", commandParameters));
            }
            return o;
        }

        /// <summary>
        /// 根据sessionID获取用户信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserBaseBySessionID(string sessionID, Boolean fromCache = false)
        {
            MchUserBaseInfo o = null;
            if (fromCache)
            {
                string objId = string.Format(USER_CACHE_KEY, sessionID);
                o = (MchUserBaseInfo)WebCache.GetCacheService().RetrieveObject(objId);
            }
            if (o == null)
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@sessionID", SqlDbType.VarChar, 100)
                };
                commandParameters[0].Value = sessionID;
                return GetUserModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_getBySessionID", commandParameters));
            }
            return o;
        }

        /// <summary>
        /// 根据merchantName获取用户信息
        /// </summary>
        /// <param name="merchantName"></param>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserBaseByMerchantName(string merchantName, Boolean fromCache = false)
        {
            MchUserBaseInfo o = null;
            if (fromCache)
            {
                string objId = string.Format(USER_CACHE_KEY, merchantName);
                o = (MchUserBaseInfo)WebCache.GetCacheService().RetrieveObject(objId);
            }
            if (o == null)
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@merchantName", SqlDbType.VarChar, 100)
                };
                commandParameters[0].Value = merchantName;
                return GetUserModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_getByMerchantName", commandParameters));
            }
            return o;
        }
        
        public static MchUserBaseInfo GetPromSuperior(int userId)
        {
            string commandText = "SELECT u.* FROM userbase u inner JOIN PromotionUser pu ON u.id = pu.PID\r\nWHERE pu.RegId = @RegId";
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@RegId", SqlDbType.Int, 10) };
            commandParameters[0].Value = userId;
            return GetUserModelFromDs(DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters));
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 增加商户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static Int32 Add(MchUserBaseInfo userInfo)
        {
            try
            {
                SqlParameter[] commandParameters = {
                    new SqlParameter("@userid",SqlDbType.Int),
                    new SqlParameter("@classid",SqlDbType.Int),
                    new SqlParameter("@username",SqlDbType.VarChar,100),
                    new SqlParameter("@userpwd",SqlDbType.VarChar,100),
                    new SqlParameter("@userpaypwd",SqlDbType.VarChar,100),
                    new SqlParameter("@merchantname",SqlDbType.VarChar,100),
                    new SqlParameter("@apikey",SqlDbType.VarChar,100),
                    new SqlParameter("@contactname",SqlDbType.VarChar,100),
                    new SqlParameter("@idcard",SqlDbType.VarChar,100),
                    new SqlParameter("@phone",SqlDbType.VarChar,100),
                    new SqlParameter("@email",SqlDbType.VarChar,100),
                    new SqlParameter("@qq",SqlDbType.VarChar,100),
                    new SqlParameter("@isphone",SqlDbType.Int),
                    new SqlParameter("@isemail",SqlDbType.Int),
                    new SqlParameter("@isrealname",SqlDbType.Int),
                    new SqlParameter("@withdrawschemeid",SqlDbType.Int),
                    new SqlParameter("@payrateid",SqlDbType.Int),
                    new SqlParameter("@maxdaywithdrawtimes",SqlDbType.Int),
                    new SqlParameter("@status",SqlDbType.Int),
                    new SqlParameter("@company",SqlDbType.VarChar,100),
                    new SqlParameter("@linkman",SqlDbType.VarChar,100),
                    new SqlParameter("@withdrawtype",SqlDbType.Int),
                    new SqlParameter("@randomproduct",SqlDbType.Int),
                    new SqlParameter("@manageid",SqlDbType.Int),
                    new SqlParameter("@siteurl",SqlDbType.VarChar,100),
                    new SqlParameter("@frontpic",SqlDbType.VarChar,100),
                    new SqlParameter("@versopic",SqlDbType.VarChar,100),
                    new SqlParameter("@defaultthemes",SqlDbType.VarChar,100),
                    new SqlParameter("@isdebug",SqlDbType.Int),
                    new SqlParameter("@agentid",SqlDbType.Int),
                    new SqlParameter("@cpsdrate",SqlDbType.Int),
                    new SqlParameter("@usertype",SqlDbType.Int),
                    new SqlParameter("@userlevel",SqlDbType.Int)
                };
                commandParameters[0].Direction = ParameterDirection.InputOutput;
                commandParameters[1].Value = userInfo.ClassID;
                commandParameters[2].Value = userInfo.UserName;
                commandParameters[3].Value = userInfo.UserPwd;
                commandParameters[4].Value = userInfo.UserPayPwd;
                commandParameters[5].Value = userInfo.MerchantName;
                commandParameters[6].Value = userInfo.ApiKey;
                commandParameters[7].Value = userInfo.ContactName;
                commandParameters[8].Value = userInfo.IDCard;
                commandParameters[9].Value = userInfo.Phone;
                commandParameters[10].Value = userInfo.EMail;
                commandParameters[11].Value = userInfo.QQ;
                commandParameters[12].Value = userInfo.IsPhone;
                commandParameters[13].Value = userInfo.IsEmail;
                commandParameters[14].Value = userInfo.IsRealName;
                commandParameters[15].Value = userInfo.WithdrawSchemeID;
                commandParameters[16].Value = userInfo.PayRateID;
                commandParameters[17].Value = userInfo.MaxDayWithdrawTimes;
                commandParameters[18].Value = userInfo.Status;
                commandParameters[19].Value = userInfo.Company;
                commandParameters[20].Value = userInfo.LinkMan;
                commandParameters[21].Value = userInfo.WithdrawType;
                commandParameters[22].Value = userInfo.RandomProduct;
                commandParameters[23].Value = userInfo.ManageId;
                commandParameters[24].Value = userInfo.SiteUrl;
                commandParameters[25].Value = userInfo.FrontPic;
                commandParameters[26].Value = userInfo.VersoPic;
                commandParameters[27].Value = userInfo.DefaultThemes;
                commandParameters[28].Value = userInfo.IsDebug;
                commandParameters[29].Value = userInfo.AgentID;
                commandParameters[30].Value = userInfo.CPSDrate;
                commandParameters[31].Value = userInfo.UserType;
                commandParameters[32].Value = userInfo.UserLevel;

                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userbase_add", commandParameters) > 0)
                {
                    return Convert.ToInt32(commandParameters[0].Value);
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 删除商户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool Delete(int userId)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, userId)
                };
                flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userbase_delete", commandParameters) > 0;
                ClearCache(userId);
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public static bool Update(MchUserBaseInfo userInfo)
        {
            try
            {
                SqlParameter[] commandParameters = {
                    new SqlParameter("@userid",SqlDbType.Int),
                    new SqlParameter("@classid",SqlDbType.Int),
                    new SqlParameter("@username",SqlDbType.VarChar,100),
                    new SqlParameter("@userpwd",SqlDbType.VarChar,100),
                    new SqlParameter("@userpaypwd",SqlDbType.VarChar,100),
                    new SqlParameter("@merchantname",SqlDbType.VarChar,100),
                    new SqlParameter("@apikey",SqlDbType.VarChar,100),
                    new SqlParameter("@contactname",SqlDbType.VarChar,100),
                    new SqlParameter("@idcard",SqlDbType.VarChar,100),
                    new SqlParameter("@phone",SqlDbType.VarChar,100),
                    new SqlParameter("@email",SqlDbType.VarChar,100),
                    new SqlParameter("@qq",SqlDbType.VarChar,100),
                    new SqlParameter("@isphone",SqlDbType.Int),
                    new SqlParameter("@isemail",SqlDbType.Int),
                    new SqlParameter("@isrealname",SqlDbType.Int),
                    new SqlParameter("@withdrawschemeid",SqlDbType.Int),
                    new SqlParameter("@payrateid",SqlDbType.Int),
                    new SqlParameter("@maxdaywithdrawtimes",SqlDbType.Int),
                    new SqlParameter("@status",SqlDbType.Int),
                    new SqlParameter("@company",SqlDbType.VarChar,100),
                    new SqlParameter("@linkman",SqlDbType.VarChar,100),
                    new SqlParameter("@withdrawtype",SqlDbType.Int),
                    new SqlParameter("@randomproduct",SqlDbType.Int),
                    new SqlParameter("@manageid",SqlDbType.Int),
                    new SqlParameter("@siteurl",SqlDbType.VarChar,100),
                    new SqlParameter("@frontpic",SqlDbType.VarChar,100),
                    new SqlParameter("@versopic",SqlDbType.VarChar,100),
                    new SqlParameter("@defaultthemes",SqlDbType.VarChar,100),
                    new SqlParameter("@isdebug",SqlDbType.Int),
                    new SqlParameter("@agentid",SqlDbType.Int),
                    new SqlParameter("@cpsdrate",SqlDbType.Int),
                    new SqlParameter("@usertype",SqlDbType.Int),
                    new SqlParameter("@userlevel",SqlDbType.Int)
                };
                commandParameters[0].Value = userInfo.UserID;
                commandParameters[1].Value = userInfo.ClassID;
                commandParameters[2].Value = userInfo.UserName;
                commandParameters[3].Value = userInfo.UserPwd;
                commandParameters[4].Value = userInfo.UserPayPwd;
                commandParameters[5].Value = userInfo.MerchantName;
                commandParameters[6].Value = userInfo.ApiKey;
                commandParameters[7].Value = userInfo.ContactName;
                commandParameters[8].Value = userInfo.IDCard;
                commandParameters[9].Value = userInfo.Phone;
                commandParameters[10].Value = userInfo.EMail;
                commandParameters[11].Value = userInfo.QQ;
                commandParameters[12].Value = userInfo.IsPhone;
                commandParameters[13].Value = userInfo.IsEmail;
                commandParameters[14].Value = userInfo.IsRealName;
                commandParameters[15].Value = userInfo.WithdrawSchemeID;
                commandParameters[16].Value = userInfo.PayRateID;
                commandParameters[17].Value = userInfo.MaxDayWithdrawTimes;
                commandParameters[18].Value = userInfo.Status;
                commandParameters[19].Value = userInfo.Company;
                commandParameters[20].Value = userInfo.LinkMan;
                commandParameters[21].Value = userInfo.WithdrawType;
                commandParameters[22].Value = userInfo.RandomProduct;
                commandParameters[23].Value = userInfo.ManageId;
                commandParameters[24].Value = userInfo.SiteUrl;
                commandParameters[25].Value = userInfo.FrontPic;
                commandParameters[26].Value = userInfo.VersoPic;
                commandParameters[27].Value = userInfo.DefaultThemes;
                commandParameters[28].Value = userInfo.IsDebug;
                commandParameters[29].Value = userInfo.AgentID;
                commandParameters[30].Value = userInfo.CPSDrate;
                commandParameters[31].Value = userInfo.UserType;
                commandParameters[32].Value = userInfo.UserLevel;

                return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userbase_update", commandParameters) > 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        #region 用户登录注销

        public static string SignIn(MchUserBaseInfo userinfo)
        {
            string userloginMsgForUnCheck = string.Empty;
            try
            {
                if (((userinfo == null) || string.IsNullOrEmpty(userinfo.MerchantName)) || string.IsNullOrEmpty(userinfo.UserPwd))
                {
                    return "请输入账号密码";
                }
                userloginMsgForUnCheck = "用户名或者密码错误,请重新输入!";
                string sessionID = Guid.NewGuid().ToString("b");
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@merchantname", SqlDbType.VarChar, 50, userinfo.MerchantName),
                    DataBase.MakeInParam("@password", SqlDbType.VarChar, 100, userinfo.UserPwd),
                    DataBase.MakeInParam("@loginip", SqlDbType.VarChar, 50, userinfo.LastLoginIP),
                    DataBase.MakeInParam("@logintime", SqlDbType.DateTime, 8, DateTime.Now),
                    DataBase.MakeInParam("@sessionId", SqlDbType.VarChar, 100, sessionID),
                    DataBase.MakeInParam("@address", SqlDbType.VarChar, 20, userinfo.LastLoginAddress),
                    DataBase.MakeInParam("@remark", SqlDbType.VarChar, 100, userinfo.LastLoginRemark),
                    DataBase.MakeInParam("@email", SqlDbType.VarChar, 50, userinfo.EMail),
                    DataBase.MakeInParam("@loginType", SqlDbType.TinyInt, 1, userinfo.LoginType),
                    DataBase.MakeInParam("@login_mac", SqlDbType.VarChar, 100, userinfo.LastLoginMAC),
                };
                SqlDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "proc_mch_user_Login", commandParameters);
                if (reader.Read())
                {
                    if (reader["status"] != DBNull.Value)
                    {
                        userinfo.Status = (int)reader["status"];
                        if (userinfo.Status == 1)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginMsgForUnCheck", "商户状态无效！");
                        }
                        else if (userinfo.Status == 2)
                        {
                            userinfo.UserID = (int)reader["userId"];
                            userinfo.UserType = (UserTypeEnum)Convert.ToInt32(reader["userType"]);
                            userinfo.IsEmail = reader["isEmail"].ToString() == "1";
                            userloginMsgForUnCheck = "OK";
                            HttpContext.Current.Session[USER_LOGIN_SESSIONID] = sessionID;
                            HttpContext.Current.Session[USER_LOGIN_CLIENT_SESSIONID] = userinfo.UserID;
                        }
                        else if (userinfo.Status == 4)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginMsgForLock", "商户已冻结！");//.UserloginMsgForlock;
                        }
                        else if (userinfo.Status == 8)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginMsgForCheckFail", "登录失败3！"); //UserloginMsgForCheckfail;
                        }
                        else if (userinfo.Status == 16)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginLimitIPCheckFail", "IP非法！");
                        }
                    }
                    reader.Dispose();
                }
                return userloginMsgForUnCheck;
            }
            catch (Exception exception)
            {
                userloginMsgForUnCheck = exception.Message.ToString();
                ExceptionHandler.HandleException(exception);
                return userloginMsgForUnCheck;
            }
        }

        public static void SignOut()
        {
            HttpContext.Current.Items[USER_CONTEXT_KEY] = null;
            HttpContext.Current.Session[USER_LOGIN_SESSIONID] = null;
            HttpContext.Current.Session[USER_LOGIN_CLIENT_SESSIONID] = null;
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public static MchUserBaseInfo CurrentMember
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items[USER_CONTEXT_KEY] == null)
                    {
                        int current = GetCurrent();
                        if (current <= 0)
                            return null;
                        HttpContext.Current.Items[USER_CONTEXT_KEY] = GetUserBaseByUserID(current);
                    }
                    return (HttpContext.Current.Items[USER_CONTEXT_KEY] as MchUserBaseInfo);
                }
                return null;
            }
        }

        /// <summary>
        /// 获取Session中UserID。
        /// </summary>
        /// <returns></returns>
        public static int GetCurrent()
        {
            try
            {
                object cuSession = HttpContext.Current.Session[USER_LOGIN_CLIENT_SESSIONID];
                if (cuSession != null)
                {
                    return Convert.ToInt32(cuSession);
                }
                object cuSessionID = HttpContext.Current.Session[USER_LOGIN_SESSIONID];
                if (cuSessionID != null)
                {
                    SqlParameter[] commandParameters = new SqlParameter[] {
                         new SqlParameter("@sessionId", SqlDbType.VarChar, 100)
                    };
                    commandParameters[0].Value = cuSessionID;
                    object userID = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_mch_users_getIdBySession", commandParameters);
                    if (userID != DBNull.Value)
                    {
                        return Convert.ToInt32(userID);
                    }
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        #endregion

        #region 查询

        private static string BuilderUpdateLogWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter;
                    SearchParam param2 = param[i];
                    switch (param2.ParamKey.Trim().ToLower())
                    {
                        case "id":
                            builder.Append(" AND [id] = @id");
                            parameter = new SqlParameter("@id", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "userid":
                            builder.Append(" AND [userid] = @userid");
                            parameter = new SqlParameter("@userid", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "usertype":
                            builder.Append(" AND [userType] = @userType");
                            parameter = new SqlParameter("@userType", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter;
                    SearchParam param2 = param[i];
                    switch (param2.ParamKey.Trim().ToLower())
                    {
                        case "id":
                        {
                            builder.Append(" AND [userid] = @id");
                            parameter = new SqlParameter("@id", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "username":
                        {
                            builder.Append(" AND [userName] like @UserName");
                            parameter = new SqlParameter("@UserName", SqlDbType.VarChar, 20);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 20) + "%";
                            paramList.Add(parameter);
                            continue;
                        }
                        case "merchantname":
                            {
                                builder.Append(" AND [merchantname] like @MerchantName");
                                parameter = new SqlParameter("@MerchantName", SqlDbType.VarChar, 20);
                                parameter.Value = "%" + SqlHelper.CleanString((string)param2.ParamValue, 20) + "%";
                                paramList.Add(parameter);
                                continue;
                            }
                        case "qq":
                        {
                            builder.Append(" AND [qq] like @qq");
                            parameter = new SqlParameter("@qq", SqlDbType.VarChar, 50);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                            paramList.Add(parameter);
                            continue;
                        }
                        case "tel":
                        {
                            builder.Append(" AND [Phone] like @tel");
                            parameter = new SqlParameter("@tel", SqlDbType.VarChar, 50);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                            paramList.Add(parameter);
                            continue;
                        }
                        case "email":
                        {
                            builder.Append(" AND [email] like @email");
                            parameter = new SqlParameter("@email", SqlDbType.VarChar, 50);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                            paramList.Add(parameter);
                            continue;
                        }
                        //case "full_name":
                        //{
                        //    builder.Append(" AND [full_name] like @full_name");
                        //    parameter = new SqlParameter("@full_name", SqlDbType.VarChar, 50);
                        //    parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                        //    paramList.Add(parameter);
                        //    continue;
                        //}
                        case "status":
                        {
                            builder.Append(" AND [status] = @status");
                            parameter = new SqlParameter("@status", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "usertype":
                            {
                                builder.Append(" AND [userType] = @userType");
                                parameter = new SqlParameter("@userType", SqlDbType.TinyInt);
                                parameter.Value = (int)param2.ParamValue;
                                paramList.Add(parameter);
                                continue;
                            }
                        case "specialchannel":
                        {
                            string paramValue = (string) param2.ParamValue;
                            if (paramValue == "1")
                            {
                                break;
                            }
                            goto Label_05B7;
                        }
                        case "special":
                        {
                            builder.Append(" AND [PayRateID] = @special");
                            parameter = new SqlParameter("@special", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "isrealnamepass":
                        {
                            builder.Append(" AND [IsRealName] = @isRealNamePass");
                            parameter = new SqlParameter("@isRealNamePass", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "userlevel":
                        {
                            builder.Append(" AND [userlevel] = @userlevel");
                            parameter = new SqlParameter("@userlevel", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "proid":
                            builder.Append(" AND [AgentID] = @proid");
                            parameter = new SqlParameter("@proid", SqlDbType.Int);
                            parameter.Value = (int)param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        case "manageid":
                        {
                            builder.Append(" AND [manageId] = @manageId");
                            parameter = new SqlParameter("@manageId", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "balance":
                        {
                            builder.AppendFormat(" AND [balance] {0} @balance", param2.CmpOperator);
                            parameter = new SqlParameter("@balance", SqlDbType.Decimal);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "enableamt":
                        {
                            builder.AppendFormat(" AND [enableAmt] {0} @enableAmt", param2.CmpOperator);
                            parameter = new SqlParameter("@enableAmt", SqlDbType.Decimal);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        default:
                        {
                            continue;
                        }
                    }
                    builder.Append(" AND exists(select 0 from mch_userChannelType where isnull(SupplierCode,0)>0 and userid=v_mch_user_detail.userid)");
                    continue;
                Label_05B7:
                    builder.Append(" AND not exists(select 0 from mch_userChannelType where isnull(SupplierCode,0)>0 and userid=v_mch_user_detail.userid)");
                }
            }
            return builder.ToString();
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_mch_user_detail";
                string key = "[userid]";
                if (string.IsNullOrEmpty(orderby))
                    orderby = "userid desc";
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" +
                            SqlHelper.GetPageSelectSQL("*", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static DataSet UpdateLogPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "usersupdate";
                string key = "[id]";
                string columns = "id,\r\nuserid,\r\nfield,\r\noldValue,\r\nnewvalue,\r\nAddtime,\r\neditor,\r\noIp";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "Addtime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderUpdateLogWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL(columns, tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        #endregion

        #region 检查账号是否存在

        /// <summary>
        /// 根据UserName检查是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>True存在；False不存在</returns>
        public static bool CheckExistsByUserName(int userName)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@userName", SqlDbType.VarChar, 50, userName)
                };
                flag = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_mch_user_ExistsByUserName", commandParameters).ToString() == "1";
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        /// <summary>
        /// 根据MerchantName检查是否存在
        /// </summary>
        /// <param name="merchantName"></param>
        /// <returns>True存在；False不存在</returns>
        public static bool CheckExistsByMerchantName(string merchantName)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@merchantName", SqlDbType.VarChar, 50, merchantName)
                };
                flag = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_mch_user_ExistsByMerchantName", commandParameters).ToString() == "1";
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        public static bool chkAgent(int agentid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@agentid", SqlDbType.Int, 10) };
                commandParameters[0].Value = agentid;
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_user_chkagent", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        
        public static DataTable getAgentList()
        {
            try
            {
                string commandText = "select userid,userName from mch_userbase with(nolock) where userType = 4";
                return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        public static bool DelUpdateLog(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, id) };
                string commandText = "delete from usersupdate where id=@id";
                return (DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static int EmailExists(string email)
        {
            try
            {
                int num = 0x3e7;
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@email", SqlDbType.NVarChar, 50, email) };
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_EmailExists", commandParameters);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    num = Convert.ToInt32(obj2);
                }
                return num;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x3e7;
            }
        }

        public static List<int> GetUsers(string where)
        {
            List<int> list = new List<int>();
            if (string.IsNullOrEmpty(where))
            {
                where = "1=1";
            }
            string commandText = "select userid from dbo.mch_userbase where " + where;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            while (reader.Read())
            {
                list.Add(reader.GetInt32(0));
            }
            return list;
        }

        public static UserInfo ReaderBind(IDataReader dataReader)
        {
            UserInfo info = new UserInfo();
            object obj2 = dataReader["id"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ID = (int)obj2;
            }
            info.UserName = dataReader["userName"].ToString();
            info.Password = dataReader["password"].ToString();
            obj2 = dataReader["CPSDrate"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.CPSDrate = (int)obj2;
            }
            obj2 = dataReader["CVSNrate"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.CVSNrate = (int)obj2;
            }
            info.Email = dataReader["email"].ToString();
            info.QQ = dataReader["qq"].ToString();
            info.Tel = dataReader["tel"].ToString();
            info.IdCard = dataReader["idCard"].ToString();
            info.Account = dataReader["account"].ToString();
            info.PayeeName = dataReader["payeeName"].ToString();
            info.PayeeBank = dataReader["payeeBank"].ToString();
            info.BankProvince = dataReader["bankProvince"].ToString();
            info.BankCity = dataReader["bankCity"].ToString();
            info.BankAddress = dataReader["bankAddress"].ToString();
            info.versoPic = dataReader["versoPic"].ToString();
            info.frontPic = dataReader["frontPic"].ToString();
            obj2 = dataReader["status"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Status = (int)obj2;
            }
            obj2 = dataReader["regTime"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.RegTime = (DateTime)obj2;
            }
            obj2 = dataReader["balance"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Balance = (decimal)obj2;
            }
            obj2 = dataReader["payment"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Payment = (decimal)obj2;
            }
            obj2 = dataReader["unpayment"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Unpayment = (decimal)obj2;
            }
            obj2 = dataReader["agentId"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.AgentId = (int)obj2;
            }
            info.SiteName = dataReader["siteName"].ToString();
            info.SiteUrl = dataReader["siteUrl"].ToString();
            obj2 = dataReader["userType"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.UserType = (UserTypeEnum)obj2;
            }
            obj2 = dataReader["userLevel"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.UserLevel = (UserLevelEnum)obj2;
            }
            obj2 = dataReader["Integral"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Integral = (Int64)obj2;
            }
            obj2 = dataReader["maxdaytocashTimes"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.MaxDayToCashTimes = (int)obj2;
            }
            obj2 = dataReader["apiaccount"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.APIAccount = (long)obj2;
            }
            info.APIKey = dataReader["apikey"].ToString();
            info.LastLoginIp = dataReader["lastLoginIp"].ToString();
            obj2 = dataReader["lastLoginTime"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.LastLoginTime = (DateTime)obj2;
            }
            info.smsNotifyUrl = dataReader["smsNotifyUrl"].ToString();
            info.question = dataReader["question"].ToString();
            info.answer = dataReader["answer"].ToString();
            obj2 = dataReader["manageId"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.manageId = new int?((int)obj2);
            }
            //==========2017.2.13 add===========
            obj2 = dataReader["settles_type"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.settles_type = (int)obj2;
            }
            //==========2017.2.13 add===========
            //==========2017.2.15add===========
            obj2 = dataReader["bank_limit"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.bank_limit = (decimal)obj2;
            }
            obj2 = dataReader["qq_limit"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.qq_limit = (decimal)obj2;
            }
            obj2 = dataReader["wx_limit"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.wx_limit = (decimal)obj2;
            }
            obj2 = dataReader["ali_limit"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ali_limit = (decimal)obj2;
            }
            //==========2017.2.15add===========
            obj2 = dataReader["random_subject"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.random_subject = (int)obj2;
            }
            obj2 = dataReader["service_channel"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.service_channel = (int)obj2;
            }

            return info;
        }

        public decimal TotalBalance
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_gettotalbalance"));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        public decimal TotalPayment
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_gettotalpayment"));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    return 0M;
                }
            }
        }

        #region 商户图表数据源

        public static DataSet GetUserDayOrderChartSource(int userId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userId", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = userId;
            UserInfo info = new UserInfo();
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_users_getdayorderchart", commandParameters);

        }

        #endregion

        #region 修改商户其余信息

        public static int ChangeUserPassword(string userName, string passWord)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@username", SqlDbType.VarChar, 50),
                new SqlParameter("@password", SqlDbType.VarChar, 255) 
            };
            commandParameters[0].Value = userName;
            commandParameters[1].Value = passWord;
            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_changepwd", commandParameters);

        }

        /// <summary>
        /// 修改用户密钥。2017.8.19 add
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static int ChangeUserApiKey(int userId, string apiKey)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userid", SqlDbType.Int, 10),
                new SqlParameter("@apikey", SqlDbType.VarChar, 50) 
            };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = apiKey;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_updateApiKey", commandParameters);
        }

        public static int ChangeUserDefaultThemes(int userId, string defaultThemes)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@userId", SqlDbType.Int, 10, userId), 
                DataBase.MakeInParam("@defaultthemes", SqlDbType.VarChar, 50, defaultThemes), 
             };

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_changethemes", commandParameters);
        }

        #endregion

        #region 商户IP绑定删除

        /// <summary>
        /// 用户绑定ip.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ipType">1:loginip   2:noticeip</param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static int EditUserBindIp(int userId, int ipType, string ip)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userid", SqlDbType.Int, 10),
                new SqlParameter("@ip_type", SqlDbType.Int, 10),
                new SqlParameter("@ip", SqlDbType.VarChar, 50) 
            };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = ipType;
            commandParameters[2].Value = ip;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_bindip_add", commandParameters);
        }

        /// <summary>
        /// 获取用户绑定IP列表。
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DataSet GetUserBindIpList(int userId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userid", SqlDbType.Int, 10),
               
            };
            commandParameters[0].Value = userId;
           

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_bindip_list", commandParameters);
        }

        /// <summary>
        /// 删除用户绑定IP
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteUserBindIp(int Id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = Id;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_bindip_delete", commandParameters);
        }


        /// <summary>
        /// 获取用户绑定IP。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MchUserBindIP GetUserBindIp(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = id;

            DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_bindip_get", commandParameters);

            MchUserBindIP model = new MchUserBindIP();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return model;

            DataRow dr = ds.Tables[0].Rows[0];
            model.ID = Convert.ToInt32(dr["id"].ToString());
            model.IP = Convert.ToString(dr["ip"].ToString());
            model.IpType = Convert.ToInt32(dr["ip_type"].ToString());
            model.UserID = Convert.ToInt32(dr["userid"].ToString());
            model.BindDate = Convert.ToDateTime(dr["bind_date"].ToString());

            return model;
        }

        #endregion

        #region 代理

        /// <summary>
        /// 代理列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAgentList()
        {
            string commandText = "select userid,userName from mch_userbase with(nolock) where userType = 4";
            return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        #endregion

        #region 通道限额显示及设置

        public static int EditUserChannelTypeLimit(int userID, int typeID, decimal minMoney, decimal maxMoney)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@typeid",SqlDbType.Int),
                new SqlParameter("@userid",SqlDbType.Int),
                new SqlParameter("@minmoney",SqlDbType.Decimal),
                new SqlParameter("@maxmoney",SqlDbType.Decimal)
            };
            parameters[0].Value = typeID;
            parameters[1].Value = userID;
            parameters[2].Value = minMoney;
            parameters[3].Value = maxMoney;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_channeltype_paylimit_edit", parameters);
        }

        /// <summary>
        /// 获取通道限额列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataSet GetUserChannelTypeLimit(int userID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@userid",SqlDbType.Int)
            };
            parameters[0].Value = userID;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_channeltype_paylimit_list", parameters);
        }

        #endregion

        #region 商户银行卡列表

        /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DataSet GetUserPayBankList(int userId)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID",SqlDbType.Int)
                };
            parameters[0].Value = userId;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_paybank_List", parameters);
        }

        /// <summary>
        /// 删除商户银行卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteUserPayBank(int id)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int)
                };
            parameters[0].Value = id;
            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_paybank_Delete", parameters);

        }

        #endregion
    }
}

