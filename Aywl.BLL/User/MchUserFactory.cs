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

        internal const string FIELD_USER = "[id]\r\n      ,[userName]\r\n      ,[password]\r\n      ,[CPSDrate]\r\n      ,[CVSNrate]\r\n      ,[email]\r\n      ,[qq]\r\n      ,[tel]\r\n      ,[idCard]\r\n      ,[settles]\r\n      ,[status]\r\n      ,[regTime]\r\n      ,[company]\r\n      ,[linkMan]\r\n      ,[agentId]\r\n      ,[siteName]\r\n      ,[siteUrl]\r\n      ,[userType]\r\n      ,[userLevel]\r\n      ,[maxdaytocashTimes]\r\n      ,[apiaccount]\r\n      ,[apikey]\r\n      ,[lastLoginIp]\r\n      ,[lastLoginTime]\r\n      ,[sessionId]\r\n      ,[updatetime]\r\n      ,[DESC]\r\n      ,[userid]\r\n      ,[pmode]\r\n      ,[account]\r\n      ,[payeeName]\r\n      ,[payeeBank]\r\n      ,[bankProvince]\r\n      ,[bankCity]\r\n      ,[bankAddress]\r\n      ,[Integral]\r\n      ,[balance]\r\n      ,[payment]\r\n      ,[unpayment]\r\n      ,[enableAmt]\r\n      ,[manageId]\r\n      ,[isRealNamePass]\r\n      ,[isPhonePass]\r\n      ,[isEmailPass]\r\n      ,[question]\r\n      ,[answer]\r\n      ,[smsNotifyUrl]\r\n      ,[full_name]\r\n      ,[classid]\r\n      ,[Freeze]\r\n      ,[schemename]\r\n      ,[idCardtype]\r\n      ,[msn]\r\n      ,[fax]\r\n      ,[province]\r\n      ,[city]\r\n      ,[zip]\r\n      ,[field1],[levName],[frontPic],[versoPic]";
        internal const string SQL_BASE_TABLE = "userbase";
        internal const string SQL_BASE_TABLE_FIELD = "[id],[pwd2],[full_name],[userName],[password],[CPSDrate],[CVSNrate],[email],[qq],[tel],[idCard],[settles],[status],[regTime],[company],[linkMan],[agentId],[siteName],[siteUrl],[userType],[userLevel],[maxdaytocashTimes],[apiaccount],[apikey],[updatetime],[DESC],isRealNamePass,isEmailPass,isPhonePass,[classid],[isdebug],[frontPic],[versoPic],settles_type,bank_limit,wx_limit,ali_limit,qq_limit";
        internal const string SQL_PAYBANK_TABLE = "userspaybank";
        internal const string SQL_PAYBANK_TABLE_FIELD = "[userid],[pmode],[account],[payeeName],[payeeBank],[bankProvince],[bankCity],[bankAddress],[status]";
        internal const string SQL_TABLE = "V_Users";
        internal const string SQL_TABLE_FIELD = "id,userName,password,CPSDrate,CVSNrate,email,qq,tel,idCard,pmode,settles,account,payeeName,payeeBank,bankProvince,bankCity,bankAddress,status,regTime,company,linkMan,agentId,siteName,siteUrl,userType,userLevel,maxdaytocashTimes,apiaccount,apikey,lastLoginIp,lastLoginTime,sessionId,manageId,isRealNamePass,full_name,classid,isdebug,frontPic,versoPic";

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

            modle.ChannelTypeID = Utils.StrToInt(dr["ChannelTypeID"], 0);
            modle.MinMoney = Utils.StrToDecimal(dr["MinMoney"].ToString(), 0);
            modle.MaxMoney = Utils.StrToDecimal(dr["MaxMoney"].ToString(), 0);

            modle.Integral = Utils.StrToLong(dr["integral"].ToString(), 0);
            modle.Freeze = Utils.StrToDecimal(dr["freeze"].ToString(), 0);
            modle.Balance = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.Payment = Utils.StrToDecimal(dr["Payment"].ToString(), 0);
            modle.UnPayment = Utils.StrToDecimal(dr["UnPayment"].ToString(), 0);
            modle.UnPayment2 = Utils.StrToDecimal(dr["unpayment2"].ToString(), 0);
            modle.EnableAmt = Utils.StrToDecimal(dr["enableAmt"].ToString(), 0);

            modle.SchemeneType = Utils.StrToInt(dr["SchemeneType"].ToString(), 0); ;
            modle.SchemeName = Convert.ToString(dr["schemename"]);
            modle.SingleMinAmtLimit = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.SingleMaxAmtLimit = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.DailyMaxTimes = Utils.StrToInt(dr["dailymaxtimes"],0);
            modle.DailyMaxAmt = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.ChargeRate = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.SingleMinCharge = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.SingleMaxCharge = Utils.StrToDecimal(dr["Balance"].ToString(), 0);
            modle.IsTranApi = Utils.StrToInt(dr["istranapi"]);
            modle.IsDefault = Utils.StrToInt(dr["IsDefault"].ToString(), 0);
            modle.IsSys = Utils.StrToInt(dr["IsSys"].ToString(), 0);
            modle.BankDetentionDays = Utils.StrToInt(dr["bankdetentiondays"]);
            modle.QQDetentionDays = Utils.StrToInt(dr["qqdetentiondays"]);
            modle.JDDetentionDays = Utils.StrToInt(dr["jddetentiondays"]);
            modle.IsTranRequiredAudit = Utils.StrToInt(dr["jddetentiondays"]);
            modle.AlipayDetentionDays = Utils.StrToInt(dr["alipaydetentiondays"]);
            modle.WeiXinDetentionDays = Utils.StrToInt(dr["weixindetentiondays"]);
            modle.OtherDetentionDays = Utils.StrToInt(dr["otherdetentiondays"]);

            return modle;
        }

        /// <summary>
        /// 根据userID获取用户信息。
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserBaseByUserID(int userID, Boolean fromCache = false)
        {
            MchUserBaseInfo o = new MchUserBaseInfo();
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
            MchUserBaseInfo o = new MchUserBaseInfo();
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
            MchUserBaseInfo o = new MchUserBaseInfo();
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
            MchUserBaseInfo o = new MchUserBaseInfo();
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

                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_add", commandParameters) > 0)
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

        public static bool Del(int userId)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, userId)
                };
                flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_del", commandParameters) > 0;
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
        /// 暂时未弄好！！！！
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <param name="changeList"></param>
        /// <returns></returns>
        public static bool Update(UserInfo _userinfo, List<UsersUpdateLog> changeList)
        {
            bool flag2;
            SqlParameter[] parameterArray = new SqlParameter[] {
                DataBase.MakeInParam("@id", SqlDbType.Int, 10, _userinfo.ID),
                DataBase.MakeInParam("@userName", SqlDbType.VarChar, 50, _userinfo.UserName),
                DataBase.MakeInParam("@password", SqlDbType.VarChar, 100, _userinfo.Password),
                DataBase.MakeInParam("@cpsdrate", SqlDbType.Int, 10, _userinfo.CPSDrate),
                DataBase.MakeInParam("@cvsnrate", SqlDbType.Int, 10, _userinfo.CVSNrate),
                DataBase.MakeInParam("@email", SqlDbType.VarChar, 50, _userinfo.Email),
                DataBase.MakeInParam("@qq", SqlDbType.VarChar, 50, _userinfo.QQ),
                DataBase.MakeInParam("@tel", SqlDbType.VarChar, 50, _userinfo.Tel),
                DataBase.MakeInParam("@idCard", SqlDbType.VarChar, 50, _userinfo.IdCard),
                DataBase.MakeInParam("@account", SqlDbType.VarChar, 50, _userinfo.Account),
                DataBase.MakeInParam("@payeeName", SqlDbType.VarChar, 50, _userinfo.PayeeName),
                DataBase.MakeInParam("@payeeBank", SqlDbType.VarChar, 50, _userinfo.PayeeBank),
                DataBase.MakeInParam("@bankProvince", SqlDbType.VarChar, 50, _userinfo.BankProvince),
                DataBase.MakeInParam("@bankCity", SqlDbType.VarChar, 50, _userinfo.BankCity),
                DataBase.MakeInParam("@bankAddress", SqlDbType.VarChar, 50, _userinfo.BankAddress),
                DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, _userinfo.Status),
                DataBase.MakeInParam("@agentId", SqlDbType.Int, 10, _userinfo.AgentId),
                DataBase.MakeInParam("@siteName", SqlDbType.VarChar, 50, _userinfo.SiteName),
                DataBase.MakeInParam("@siteUrl", SqlDbType.VarChar, 100, _userinfo.SiteUrl),
                DataBase.MakeInParam("@userType", SqlDbType.Int, 10, (int) _userinfo.UserType),
                DataBase.MakeInParam("@userLevel", SqlDbType.Int, 10, (int) _userinfo.UserLevel),
                DataBase.MakeInParam("@maxdaytocashTimes", SqlDbType.Int, 10, _userinfo.MaxDayToCashTimes),
                DataBase.MakeInParam("@apiaccount", SqlDbType.BigInt, 8, _userinfo.APIAccount),
                DataBase.MakeInParam("@apikey", SqlDbType.VarChar, 50, _userinfo.APIKey),
                DataBase.MakeInParam("@DESC", SqlDbType.VarChar, 0xfa0, _userinfo.Desc),
                DataBase.MakeInParam("@pmode", SqlDbType.Int, 10, _userinfo.PMode),
                DataBase.MakeInParam("@updatetime", SqlDbType.DateTime, 8, DateTime.Now),
                DataBase.MakeInParam("@manageId", SqlDbType.Int, 10, _userinfo.manageId),
                DataBase.MakeInParam("@isRealNamePass", SqlDbType.TinyInt, 1, _userinfo.IsRealNamePass),
                DataBase.MakeInParam("@isEmailPass", SqlDbType.TinyInt, 1, _userinfo.IsEmailPass),
                DataBase.MakeInParam("@isPhonePass", SqlDbType.TinyInt, 1, _userinfo.IsPhonePass),
                DataBase.MakeInParam("@smsNotifyUrl", SqlDbType.NVarChar, 0xff, _userinfo.smsNotifyUrl),
                DataBase.MakeInParam("@full_name", SqlDbType.NVarChar, 50, _userinfo.full_name),
                DataBase.MakeInParam("@male", SqlDbType.NVarChar, 4, _userinfo.male),
                DataBase.MakeInParam("@addtress", SqlDbType.NVarChar, 30, _userinfo.addtress),
                DataBase.MakeInParam("@question", SqlDbType.NVarChar, 150, _userinfo.question),
                DataBase.MakeInParam("@answer", SqlDbType.NVarChar, 100, _userinfo.answer),
                DataBase.MakeInParam("@pwd2", SqlDbType.NVarChar, 50, _userinfo.Password2),
                DataBase.MakeInParam("@linkman", SqlDbType.NVarChar, 50, _userinfo.LinkMan),
                DataBase.MakeInParam("@classid", SqlDbType.TinyInt, 1, _userinfo.classid),
                DataBase.MakeInParam("@settles", SqlDbType.TinyInt, 1, _userinfo.Settles),
                DataBase.MakeInParam("@isdebug", SqlDbType.TinyInt, 1, _userinfo.isdebug),
                DataBase.MakeInParam("@idCardtype", SqlDbType.TinyInt, 1, _userinfo.IdCardType),
                DataBase.MakeInParam("@msn", SqlDbType.VarChar, 30, _userinfo.msn),
                DataBase.MakeInParam("@fax", SqlDbType.VarChar, 20, _userinfo.fax),
                DataBase.MakeInParam("@province", SqlDbType.VarChar, 20, _userinfo.province),
                DataBase.MakeInParam("@city", SqlDbType.VarChar, 20, _userinfo.city),
                DataBase.MakeInParam("@zip", SqlDbType.VarChar, 8, _userinfo.zip),
                DataBase.MakeInParam("@field1", SqlDbType.NVarChar, 50, _userinfo.field1),
                DataBase.MakeInParam("@accoutType", SqlDbType.TinyInt, 1, _userinfo.accoutType),
                DataBase.MakeInParam("@BankCode", SqlDbType.VarChar, 50, _userinfo.BankCode),
                DataBase.MakeInParam("@provinceCode", SqlDbType.VarChar, 50, _userinfo.provinceCode),
                DataBase.MakeInParam("@cityCode", SqlDbType.VarChar, 50, _userinfo.cityCode),
                DataBase.MakeInParam("@isagentDistribution", SqlDbType.TinyInt, 1, _userinfo.isagentDistribution),
                DataBase.MakeInParam("@agentDistscheme", SqlDbType.Int, 10, _userinfo.agentDistscheme),
                DataBase.MakeInParam("@cardversion", SqlDbType.TinyInt, 1, _userinfo.cardversion),
                DataBase.MakeInParam("@versoPic", SqlDbType.VarChar, 500, _userinfo.versoPic),
                DataBase.MakeInParam("@frontPic", SqlDbType.VarChar, 500, _userinfo.frontPic),
                DataBase.MakeInParam("@settles_type", SqlDbType.VarChar, 500, _userinfo.settles_type),
                DataBase.MakeInParam("@bank_limit", SqlDbType.Decimal, 18, _userinfo.bank_limit),
                DataBase.MakeInParam("@wx_limit", SqlDbType.Decimal, 18, _userinfo.wx_limit),
                DataBase.MakeInParam("@ali_limit", SqlDbType.Decimal, 18, _userinfo.ali_limit),
                DataBase.MakeInParam("@qq_limit", SqlDbType.Decimal, 18, _userinfo.qq_limit),
                DataBase.MakeInParam("@random_subject", SqlDbType.TinyInt, 1, _userinfo.random_subject),
                DataBase.MakeInParam("@service_channel", SqlDbType.TinyInt, 1, _userinfo.service_channel)
             };
            using (SqlConnection connection = new SqlConnection(DataBase.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (changeList != null)
                    {
                        foreach (UsersUpdateLog log in changeList)
                        {
                            SqlParameter[] parameterArray2 = new SqlParameter[] {
                                new SqlParameter("@userid", SqlDbType.Int, 10),
                                new SqlParameter("@field", SqlDbType.VarChar, 20),
                                new SqlParameter("@oldValue", SqlDbType.VarChar, 100),
                                new SqlParameter("@newvalue", SqlDbType.VarChar, 100),
                                new SqlParameter("@Addtime", SqlDbType.DateTime),
                                new SqlParameter("@editor", SqlDbType.VarChar, 50),
                                new SqlParameter("@oIp", SqlDbType.VarChar, 50),
                                new SqlParameter("@desc", SqlDbType.VarChar, 0xfa0)
                            };
                            parameterArray2[0].Value = log.userid;
                            parameterArray2[1].Value = log.field;
                            parameterArray2[2].Value = log.oldValue;
                            parameterArray2[3].Value = log.newvalue;
                            parameterArray2[4].Value = log.Addtime;
                            parameterArray2[5].Value = log.Editor;
                            parameterArray2[6].Value = log.OIp;
                            parameterArray2[7].Value = log.Desc;
                            if (DataBase.ExecuteNonQuery(transaction, "proc_usersupdate_add", (object[])parameterArray2) < 0)
                            {
                                transaction.Rollback();
                                connection.Close();
                                return false;
                            }
                        }
                    }
                    if (DataBase.ExecuteNonQuery(transaction, "proc_users_Update", (object[])parameterArray) > 0)
                    {
                        HttpContext.Current.Items["{FD7BE212-8537-427f-9EF6-1D1AABCA8EA3}"] = null;
                        transaction.Commit();
                        connection.Close();
                        ClearCache(_userinfo.ID);
                        return true;
                    }
                    transaction.Rollback();
                    connection.Close();
                    flag2 = false;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    ExceptionHandler.HandleException(exception);
                    flag2 = false;
                }
                finally
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                    }
                }
            }
            return flag2;
        }

        public static bool Update1(UserInfo _userinfo)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                DataBase.MakeInParam("@id", SqlDbType.Int, 10, _userinfo.ID),
                DataBase.MakeInParam("@userName", SqlDbType.VarChar, 50, _userinfo.UserName),
                DataBase.MakeInParam("@password", SqlDbType.VarChar, 100, _userinfo.Password),
                DataBase.MakeInParam("@cpsdrate", SqlDbType.Int, 10, _userinfo.CPSDrate),
                DataBase.MakeInParam("@cvsnrate", SqlDbType.Int, 10, _userinfo.CVSNrate),
                DataBase.MakeInParam("@email", SqlDbType.VarChar, 50, _userinfo.Email),
                DataBase.MakeInParam("@qq", SqlDbType.VarChar, 50, _userinfo.QQ),
                DataBase.MakeInParam("@tel", SqlDbType.VarChar, 50, _userinfo.Tel),
                DataBase.MakeInParam("@idCard", SqlDbType.VarChar, 50, _userinfo.IdCard),
                DataBase.MakeInParam("@account", SqlDbType.VarChar, 50, _userinfo.Account),
                DataBase.MakeInParam("@payeeName", SqlDbType.VarChar, 50, _userinfo.PayeeName),
                DataBase.MakeInParam("@payeeBank", SqlDbType.VarChar, 50, _userinfo.PayeeBank),
                DataBase.MakeInParam("@bankProvince", SqlDbType.VarChar, 50, _userinfo.BankProvince),
                DataBase.MakeInParam("@bankCity", SqlDbType.VarChar, 50, _userinfo.BankCity),
                DataBase.MakeInParam("@bankAddress", SqlDbType.VarChar, 50, _userinfo.BankAddress),
                DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, _userinfo.Status),
                DataBase.MakeInParam("@agentId", SqlDbType.Int, 10, _userinfo.AgentId),
                DataBase.MakeInParam("@siteName", SqlDbType.VarChar, 50, _userinfo.SiteName),
                DataBase.MakeInParam("@siteUrl", SqlDbType.VarChar, 100, _userinfo.SiteUrl),
                DataBase.MakeInParam("@userType", SqlDbType.Int, 10, (int) _userinfo.UserType),
                DataBase.MakeInParam("@userLevel", SqlDbType.Int, 10, (int) _userinfo.UserLevel),
                DataBase.MakeInParam("@maxdaytocashTimes", SqlDbType.Int, 10, _userinfo.MaxDayToCashTimes),
                DataBase.MakeInParam("@apiaccount", SqlDbType.BigInt, 8, _userinfo.APIAccount),
                DataBase.MakeInParam("@apikey", SqlDbType.VarChar, 50, _userinfo.APIKey),
                DataBase.MakeInParam("@DESC", SqlDbType.VarChar, 0xfa0, _userinfo.Desc),
                DataBase.MakeInParam("@pmode", SqlDbType.Int, 10, _userinfo.PMode),
                DataBase.MakeInParam("@updatetime", SqlDbType.DateTime, 8, DateTime.Now),
                DataBase.MakeInParam("@manageId", SqlDbType.Int, 10, _userinfo.manageId),
                DataBase.MakeInParam("@isRealNamePass", SqlDbType.TinyInt, 1, _userinfo.IsRealNamePass),
                DataBase.MakeInParam("@isEmailPass", SqlDbType.TinyInt, 1, _userinfo.IsEmailPass),
                DataBase.MakeInParam("@isPhonePass", SqlDbType.TinyInt, 1, _userinfo.IsPhonePass),
                DataBase.MakeInParam("@smsNotifyUrl", SqlDbType.NVarChar, 0xff, _userinfo.smsNotifyUrl),
                DataBase.MakeInParam("@full_name", SqlDbType.NVarChar, 50, _userinfo.full_name),
                DataBase.MakeInParam("@male", SqlDbType.NVarChar, 4, _userinfo.male),
                DataBase.MakeInParam("@addtress", SqlDbType.NVarChar, 30, _userinfo.addtress),
                DataBase.MakeInParam("@question", SqlDbType.NVarChar, 150, _userinfo.question),
                DataBase.MakeInParam("@answer", SqlDbType.NVarChar, 100, _userinfo.answer),
                DataBase.MakeInParam("@pwd2", SqlDbType.NVarChar, 50, _userinfo.Password2),
                DataBase.MakeInParam("@linkman", SqlDbType.NVarChar, 50, _userinfo.LinkMan),
                DataBase.MakeInParam("@classid", SqlDbType.TinyInt, 1, _userinfo.classid),
                DataBase.MakeInParam("@settles", SqlDbType.TinyInt, 1, _userinfo.Settles),
                DataBase.MakeInParam("@isdebug", SqlDbType.TinyInt, 1, _userinfo.isdebug),
                DataBase.MakeInParam("@idCardtype", SqlDbType.TinyInt, 1, _userinfo.IdCardType),
                DataBase.MakeInParam("@msn", SqlDbType.VarChar, 30, _userinfo.msn),
                DataBase.MakeInParam("@fax", SqlDbType.VarChar, 20, _userinfo.fax),
                DataBase.MakeInParam("@province", SqlDbType.VarChar, 20, _userinfo.province),
                DataBase.MakeInParam("@city", SqlDbType.VarChar, 20, _userinfo.city),
                DataBase.MakeInParam("@zip", SqlDbType.VarChar, 8, _userinfo.zip),
                DataBase.MakeInParam("@field1", SqlDbType.NVarChar, 50, _userinfo.field1),
                DataBase.MakeInParam("@accoutType", SqlDbType.TinyInt, 1, _userinfo.accoutType),
                DataBase.MakeInParam("@BankCode", SqlDbType.VarChar, 50, _userinfo.BankCode),
                DataBase.MakeInParam("@provinceCode", SqlDbType.VarChar, 50, _userinfo.provinceCode),
                DataBase.MakeInParam("@cityCode", SqlDbType.VarChar, 50, _userinfo.cityCode),
                DataBase.MakeInParam("@isagentDistribution", SqlDbType.TinyInt, 1, _userinfo.isagentDistribution),
                DataBase.MakeInParam("@agentDistscheme", SqlDbType.Int, 10, _userinfo.agentDistscheme),
                DataBase.MakeInParam("@cardversion", SqlDbType.TinyInt, 1, _userinfo.cardversion),
                DataBase.MakeInParam("@versoPic", SqlDbType.VarChar, 500, _userinfo.versoPic),
                DataBase.MakeInParam("@frontPic", SqlDbType.VarChar, 500, _userinfo.frontPic),
                DataBase.MakeInParam("@settles_type", SqlDbType.TinyInt, 1, _userinfo.settles_type),
                DataBase.MakeInParam("@bank_limit", SqlDbType.Decimal, 18, _userinfo.bank_limit),
                DataBase.MakeInParam("@wx_limit", SqlDbType.Decimal, 18, _userinfo.wx_limit),
                DataBase.MakeInParam("@ali_limit", SqlDbType.Decimal, 18, _userinfo.ali_limit),
                DataBase.MakeInParam("@qq_limit", SqlDbType.Decimal, 18, _userinfo.qq_limit),
                DataBase.MakeInParam("@random_subject", SqlDbType.TinyInt, 1, _userinfo.random_subject),
                DataBase.MakeInParam("@service_channel", SqlDbType.TinyInt, 1, _userinfo.service_channel)
             };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_Update", commandParameters) > 0);
        }

        #endregion

        #region 用户登录注销

        public static string SignIn(MchUserBaseInfo userinfo)
        {
            string userloginMsgForUnCheck = string.Empty;
            try
            {
                if (((userinfo == null) || string.IsNullOrEmpty(userinfo.UserName)) || string.IsNullOrEmpty(userinfo.UserPwd))
                {
                    return "请输入账号密码";
                }
                userloginMsgForUnCheck = "用户名或者密码错误,请重新输入!";
                string sessionID = Guid.NewGuid().ToString("b");
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@username", SqlDbType.VarChar, 50, userinfo.UserName),
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
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginMsgForUnCheck", "登录失败！");
                        }
                        else if (userinfo.Status == 2)
                        {
                            userinfo.UserID = (int)reader["userId"];
                            userinfo.UserType = (UserTypeEnum)Convert.ToInt32(reader["userType"]);
                            userinfo.IsEmail = reader["isEmailPass"].ToString() == "1";
                            userloginMsgForUnCheck = "登录成功";
                            HttpContext.Current.Session[USER_LOGIN_SESSIONID] = sessionID;
                            HttpContext.Current.Session[USER_LOGIN_CLIENT_SESSIONID] = userinfo.UserID;
                        }
                        else if (userinfo.Status == 4)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginMsgForLock", "登录失败！");//.UserloginMsgForlock;
                        }
                        else if (userinfo.Status == 8)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginMsgForCheckFail", "登录失败！"); //UserloginMsgForCheckfail;
                        }
                        else if (userinfo.Status == 16)
                        {
                            userloginMsgForUnCheck = SysConfig.GetOptionValue("UserloginLimitIPCheckFail", "登录失败！");
                        }
                    }
                    reader.Dispose();
                }
                return userloginMsgForUnCheck;
            }
            catch (Exception exception)
            {
                userloginMsgForUnCheck = "登录失败";
                ExceptionHandler.HandleException(exception);
                return userloginMsgForUnCheck;
            }
        }

        public static void SignOut()
        {
            HttpContext.Current.Items[USER_CONTEXT_KEY] = null;
            HttpContext.Current.Session["{10E6C4EE-54C1-4895-8CDE-202A5B3DD9E9}"] = null;
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
                            builder.Append(" AND [id] = @id");
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
                            builder.Append(" AND [Tel] like @tel");
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
                        case "full_name":
                        {
                            builder.Append(" AND [full_name] like @full_name");
                            parameter = new SqlParameter("@full_name", SqlDbType.VarChar, 50);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                            paramList.Add(parameter);
                            continue;
                        }
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
                            parameter.Value = (int) param2.ParamValue;
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
                            builder.Append(" AND [special] = @special");
                            parameter = new SqlParameter("@special", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
                        case "isrealnamepass":
                        {
                            builder.Append(" AND [isRealNamePass] = @isRealNamePass");
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
                        {
                            builder.Append(" AND Exists(select 0 from PromotionUser where PromotionUser.PID = @proid and PromotionUser.RegId=v_users.id)");
                            parameter = new SqlParameter("@proid", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            continue;
                        }
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
                    builder.Append(" AND exists(select 0 from channeltypeusers where isnull(suppid,0)>0 and userid=v_users.id)");
                    continue;
                Label_05B7:
                    builder.Append(" AND not exists(select 0 from channeltypeusers where isnull(suppid,0)>0 and userid=v_users.id)");
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

        public static DataTable getAgentList()
        {
            try
            {
                string commandText = "select id,userName from userbase with(nolock) where userType = 4";
                return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        public static List<int> GetUsers(string where)
        {
            List<int> list = new List<int>();
            if (string.IsNullOrEmpty(where))
            {
                where = "1=1";
            }
            string commandText = "select id from dbo.userbase where " + where;
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

        #region 修改商户信息

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
                new SqlParameter("@user_id", SqlDbType.Int, 10),
                new SqlParameter("@apikey", SqlDbType.VarChar, 50) 
            };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = apiKey;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_updateApiKey", commandParameters);
        }

        public static int ChangeUserDefaultThemes(int userId, string defaultThemes)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@id", SqlDbType.Int, 10, userId), 
                DataBase.MakeInParam("@default_themes", SqlDbType.VarChar, 50, defaultThemes), 
             };

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_changethemes", commandParameters);
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
        public static int SetUserBindIp(int userId, int ipType, string ip)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userid", SqlDbType.Int, 10),
                new SqlParameter("@ip_type", SqlDbType.Int, 10),
                new SqlParameter("@ip", SqlDbType.VarChar, 50) 
            };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = ipType;
            commandParameters[2].Value = ip;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_bindip_add", commandParameters);
        }

        /// <summary>
        /// 获取用户绑定IP列表。
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DataSet GetUserBindIp(int userId, int ipType = 0)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userid", SqlDbType.Int, 10),
                new SqlParameter("@iptype", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = ipType;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_users_bindip_list", commandParameters);
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

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_bindip_delete", commandParameters);
        }
        #endregion
    }
}

