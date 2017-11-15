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
        public static MchUserBaseInfo GetBaseModelFromDs(DataSet ds)
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
            modle.IsPhone = Utils.StrToInt(dr["isphone"].ToString(), 0);
            modle.IsEmail = Utils.StrToInt(dr["isemail"].ToString(), 0);
            modle.IsRealName = Utils.StrToInt(dr["isrealname"].ToString(), 0);
            modle.WithdrawSchemeID = Utils.StrToInt(dr["withdrawschemeid"].ToString(), 0);
            modle.PayRateID = Utils.StrToInt(dr["payrateid"].ToString(), 0);
            modle.MaxDayWithdrawTimes = Utils.StrToInt(dr["maxdaywithdrawtimes"].ToString(), 0);
            modle.FirstLoginIP = Convert.ToString(dr["firstloginip"]);
            modle.FirstLoginMac = Convert.ToString(dr["firstloginmac"]);
            modle.LastLoginIP = Convert.ToString(dr["lastloginip"]);
            modle.LastLoginMAC = Convert.ToString(dr["lastloginmac"]);
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

            return modle;
        }

        /// <summary>
        /// Ds转详细信息对象
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetModelFromDs(DataSet ds)
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
            modle.IsPhone = Utils.StrToInt(dr["isphone"].ToString(), 0);
            modle.IsEmail = Utils.StrToInt(dr["isemail"].ToString(), 0);
            modle.IsRealName = Utils.StrToInt(dr["isrealname"].ToString(), 0);
            modle.WithdrawSchemeID = Utils.StrToInt(dr["withdrawschemeid"].ToString(), 0);
            modle.PayRateID = Utils.StrToInt(dr["payrateid"].ToString(), 0);
            modle.MaxDayWithdrawTimes = Utils.StrToInt(dr["maxdaywithdrawtimes"].ToString(), 0);
            modle.FirstLoginIP = Convert.ToString(dr["firstloginip"]);
            modle.FirstLoginMac = Convert.ToString(dr["firstloginmac"]);
            modle.LastLoginIP = Convert.ToString(dr["lastloginip"]);
            modle.LastLoginMAC = Convert.ToString(dr["lastloginmac"]);
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

            return modle;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserBaseInfo(int uid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = uid;
            return GetBaseModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_userbase_get", commandParameters));
        }
        
        /// <summary>
        /// 获取缓存用户基本信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetCacheUserBaseInfo(int uid)
        {
            MchUserBaseInfo o = new MchUserBaseInfo();
            string objId = string.Format(USER_CACHE_KEY, uid);
            o = (MchUserBaseInfo)WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                //IDictionary<string, object> parameters = new Dictionary<string, object>();
                //parameters.Add("id", uid);
                //SqlDependency dependency = DataBase.AddSqlDependency(objId, "userbase", "[id],[pwd2],[full_name],[userName],[password],[CPSDrate],[CVSNrate],[email],[qq],[tel],[idCard],[settles],[status],[regTime],[company],[linkMan],[agentId],[siteName],[siteUrl],[userType],[userLevel],[maxdaytocashTimes],[apiaccount],[apikey],[updatetime],[DESC],isRealNamePass,isEmailPass,isPhonePass,[classid],[isdebug],[frontPic],[versoPic],[settles_type],[bank_limit],[wx_limit],[ali_limit],[qq_limit],[random_subject],[service_channel]", "[id]=@id", parameters);
                o = GetUserBaseInfo(uid);
                WebCache.GetCacheService().AddObject(objId, o);
            }
            return o;
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetUserInfo(int uid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = uid;
            return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_user_get", commandParameters));
        }

        /// <summary>
        /// 获取缓存用户详细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetCacheUserInfo(int uid)
        {
            MchUserBaseInfo o = new MchUserBaseInfo();
            string objId = string.Format(USER_CACHE_KEY, uid);
            o = (MchUserBaseInfo)WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                //IDictionary<string, object> parameters = new Dictionary<string, object>();
                //parameters.Add("id", uid);
                //SqlDependency dependency = DataBase.AddSqlDependency(objId, "userbase", "[id],[pwd2],[full_name],[userName],[password],[CPSDrate],[CVSNrate],[email],[qq],[tel],[idCard],[settles],[status],[regTime],[company],[linkMan],[agentId],[siteName],[siteUrl],[userType],[userLevel],[maxdaytocashTimes],[apiaccount],[apikey],[updatetime],[DESC],isRealNamePass,isEmailPass,isPhonePass,[classid],[isdebug],[frontPic],[versoPic],[settles_type],[bank_limit],[wx_limit],[ali_limit],[qq_limit],[random_subject],[service_channel],[default_themes]", "[id]=@id", parameters);
                //SqlDependency dependency2 = DataBase.AddSqlDependency(objId, "userspaybank", "[userid],[pmode],[account],[payeeName],[payeeBank],[bankProvince],[bankCity],[bankAddress],[status],[accoutType]", "[userid]=@id", parameters);
                o = GetUserInfo(uid);
                WebCache.GetCacheService().AddObject(objId, o);
            }
            return o;
        }

        /// <summary>
        /// 根据用户名获取用户信息。
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static MchUserBaseInfo GetModelByName(string userName)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userName", SqlDbType.VarChar, 20) };
            commandParameters[0].Value = userName;
            return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_users_getbyname", commandParameters));
        }

        public static MchUserBaseInfo GetPromSuperior(int userId)
        {
            string commandText = "SELECT u.* FROM userbase u inner JOIN PromotionUser pu ON u.id = pu.PID\r\nWHERE pu.RegId = @RegId";
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@RegId", SqlDbType.Int, 10) };
            commandParameters[0].Value = userId;
            return GetBaseModelFromDs(DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters));
        }

        #endregion

        public static Int32 Add(UserInfo _userinfo)
        {
            try
            {
                SqlParameter parameter = DataBase.MakeOutParam("@id", SqlDbType.Int, 10);
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    parameter, 
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
                    DataBase.MakeInParam("@lastloginip", SqlDbType.VarChar, 50, _userinfo.LastLoginIp), 
                    DataBase.MakeInParam("@lastlogintime", SqlDbType.DateTime, 8, _userinfo.LastLoginTime), 
                    DataBase.MakeInParam("@regtime", SqlDbType.DateTime, 8, _userinfo.RegTime), 
                    DataBase.MakeInParam("@agentId", SqlDbType.Int, 10, _userinfo.AgentId), 
                    DataBase.MakeInParam("@siteName", SqlDbType.VarChar, 50, _userinfo.SiteName), 
                    DataBase.MakeInParam("@siteUrl", SqlDbType.VarChar, 100, _userinfo.SiteUrl), 
                    DataBase.MakeInParam("@userType", SqlDbType.Int, 10, (int) _userinfo.UserType), 
                    DataBase.MakeInParam("@userLevel", SqlDbType.Int, 10, (int) _userinfo.UserLevel), 
                    DataBase.MakeInParam("@maxdaytocashTimes", SqlDbType.Int, 10, _userinfo.MaxDayToCashTimes), 
                    DataBase.MakeInParam("@apiaccount", SqlDbType.BigInt, 8, _userinfo.APIAccount), 
                    DataBase.MakeInParam("@apikey", SqlDbType.VarChar, 50, _userinfo.APIKey), 
                    DataBase.MakeInParam("@pmode", SqlDbType.TinyInt, 1, _userinfo.PMode), 
                    DataBase.MakeInParam("@settles", SqlDbType.TinyInt, 1, _userinfo.Settles), 
                    DataBase.MakeInParam("@DESC", SqlDbType.VarChar, 0xfa0, _userinfo.Desc), 
                    DataBase.MakeInParam("@manageId", SqlDbType.Int, 10, _userinfo.manageId), 
                    DataBase.MakeInParam("@question", SqlDbType.NVarChar, 150, _userinfo.question), 
                    DataBase.MakeInParam("@answer", SqlDbType.NVarChar, 100, _userinfo.answer), 
                    DataBase.MakeInParam("@full_name", SqlDbType.NVarChar, 100, _userinfo.full_name), 
                    DataBase.MakeInParam("@classid", SqlDbType.TinyInt, 1, _userinfo.classid), 
                    DataBase.MakeInParam("@pwd2", SqlDbType.NVarChar, 50, _userinfo.Password2), 
                    DataBase.MakeInParam("@linkman", SqlDbType.NVarChar, 50, _userinfo.LinkMan), 
                    DataBase.MakeInParam("@isdebug", SqlDbType.TinyInt, 1, _userinfo.isdebug), 
                    DataBase.MakeInParam("@idCardtype", SqlDbType.TinyInt, 1, _userinfo.IdCardType), 
                    DataBase.MakeInParam("@msn", SqlDbType.VarChar, 30, _userinfo.msn), 
                    DataBase.MakeInParam("@fax", SqlDbType.VarChar, 20, _userinfo.fax), 
                    DataBase.MakeInParam("@province", SqlDbType.VarChar, 20, _userinfo.province), 
                    DataBase.MakeInParam("@city", SqlDbType.VarChar, 20, _userinfo.city), 
                    DataBase.MakeInParam("@zip", SqlDbType.VarChar, 8, _userinfo.zip), 
                    DataBase.MakeInParam("@field1", SqlDbType.NVarChar, 50, _userinfo.field1), 
                    DataBase.MakeInParam("@isagentDistribution", SqlDbType.TinyInt, 1, _userinfo.isagentDistribution), 
                    DataBase.MakeInParam("@cardversion", SqlDbType.TinyInt, 1, _userinfo.cardversion),
                    DataBase.MakeInParam("@settles_type", SqlDbType.TinyInt, 1, _userinfo.settles_type),
                    DataBase.MakeInParam("@bank_limit", SqlDbType.Decimal, 18, _userinfo.bank_limit),
                    DataBase.MakeInParam("@wx_limit", SqlDbType.Decimal, 18, _userinfo.wx_limit),
                    DataBase.MakeInParam("@ali_limit", SqlDbType.Decimal, 18, _userinfo.ali_limit),
                    DataBase.MakeInParam("@qq_limit", SqlDbType.Decimal, 18, _userinfo.qq_limit),
                    DataBase.MakeInParam("@random_subject", SqlDbType.TinyInt, 1, _userinfo.random_subject),
                    DataBase.MakeInParam("@service_channel", SqlDbType.TinyInt, 1, _userinfo.service_channel)
                 };
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_add", commandParameters) > 0)
                {
                    _userinfo.ID = Convert.ToInt32(parameter.Value);
                    return _userinfo.ID;
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

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

        public static bool CheckUserOrderId(int userid, string OrderId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@orderNo", SqlDbType.VarChar, 30, OrderId), 
                DataBase.MakeInParam("@userid", SqlDbType.Int, 10, userid) 
            };
            return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_usersorderid_check", commandParameters));
        }

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

        public static bool Del(int userId)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, userId) };
                flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_del", commandParameters) > 0;
                if (flag)
                {
                    ClearCache(userId);
                }
                return flag;
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

        public static bool Exists(int userId)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@userId", SqlDbType.Int, 10, userId) };
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_ExistsId", commandParameters);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    flag = Convert.ToBoolean(obj2);
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Exists(string username)
        {
            try
            {
                bool flag = false;
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@userName", SqlDbType.NVarChar, 50, username) };
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_Exists", commandParameters);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    flag = Convert.ToBoolean(obj2);
                }
                return flag;
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
                string commandText = "select id,userName from userbase with(nolock) where userType = 4";
                return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
            }
            catch
            {
                return null;
            }
        }


        public static string GetClassViewName(int classid)
        {
            string str = string.Empty;
            if (classid == 0)
            {
                return "个人";
            }
            if (classid == 1)
            {
                str = "企业";
            }
            return str;
        }

        public static string GetClassViewName(object obj)
        {
            if ((obj == null) || (obj == DBNull.Value))
            {
                return string.Empty;
            }
            return GetClassViewName(Convert.ToInt32(obj));
        }

        public static int GetCurrent()
        {
            try
            {
                object obj2 = HttpContext.Current.Session["{2A1FA22C-201B-471c-B668-2FCC1C4A121A}"];
                if (obj2 != null)
                {
                    return Convert.ToInt32(obj2);
                }
                object obj3 = HttpContext.Current.Session["{10E6C4EE-54C1-4895-8CDE-202A5B3DD9E9}"];
                if (obj3 != null)
                {
                    SqlParameter[] commandParameters = new SqlParameter[] { 
                        DataBase.MakeInParam("@sessionId", SqlDbType.VarChar, 100, obj3) 
                    };
                    object obj4 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_getIdBySession", commandParameters);
                    if (obj4 != DBNull.Value)
                    {
                        return Convert.ToInt32(obj4);
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

        /// <summary>
        /// 获取商户代理ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int GetAgentID(int userid)
        {
            try
            {
                string commandText = "SELECT agentid FROM mch_userbase with(nolock) WHERE userid=@userid ";
                SqlParameter[] commandParameters = new SqlParameter[] 
                { 
                    new SqlParameter("@userid", SqlDbType.Int, 10) 
                };
                commandParameters[0].Value = userid;
                return Utils.StrToInt(DataBase.ExecuteScalar(CommandType.Text, commandText, commandParameters), 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static string GetUserApiKey(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@user_id", SqlDbType.Int, 10, userId) };
                return DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_getApiKey", commandParameters).ToString();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return string.Empty;
            }
        }

        public static int GetUserIdBySession(string _sessionId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@sessionId", SqlDbType.VarChar, 100, _sessionId) };
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_getIdBySession", commandParameters);
                if (obj2 != DBNull.Value)
                {
                    return Convert.ToInt32(obj2);
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static int GetUserIdByToken(string token)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@token", SqlDbType.VarChar, 100, token) };
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_users_getIdByToken", commandParameters);
                if (obj2 != DBNull.Value)
                {
                    return Convert.ToInt32(obj2);
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
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

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_Users";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + 
                            SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userName]\r\n      ,[password]\r\n      ,[CPSDrate]\r\n      ,[CVSNrate]\r\n      ,[email]\r\n      ,[qq]\r\n      ,[tel]\r\n      ,[idCard]\r\n      ,[settles]\r\n      ,[status]\r\n      ,[regTime]\r\n      ,[company]\r\n      ,[linkMan]\r\n      ,[agentId]\r\n      ,[siteName]\r\n      ,[siteUrl]\r\n      ,[userType]\r\n      ,[userLevel]\r\n      ,[maxdaytocashTimes]\r\n      ,[apiaccount]\r\n      ,[apikey]\r\n      ,[lastLoginIp]\r\n      ,[lastLoginTime]\r\n      ,[sessionId]\r\n      ,[updatetime]\r\n      ,[DESC]\r\n      ,[userid]\r\n      ,[pmode]\r\n      ,[account]\r\n      ,[payeeName]\r\n      ,[payeeBank]\r\n      ,[bankProvince]\r\n      ,[bankCity]\r\n      ,[bankAddress]\r\n      ,[Integral]\r\n      ,[balance]\r\n      ,[payment]\r\n      ,[unpayment]\r\n      ,[enableAmt]\r\n      ,[manageId]\r\n      ,[isRealNamePass]\r\n      ,[isPhonePass]\r\n      ,[isEmailPass]\r\n      ,[question]\r\n      ,[answer]\r\n      ,[smsNotifyUrl]\r\n      ,[full_name]\r\n      ,[classid]\r\n      ,[Freeze]\r\n      ,[schemename]\r\n      ,[idCardtype]\r\n      ,[msn]\r\n      ,[fax]\r\n      ,[province]\r\n      ,[city]\r\n      ,[zip]\r\n      ,[field1],[levName],[frontPic],[versoPic]", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
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

        public static string SignIn(UserInfo userinfo)
        {
            string userloginMsgForUnCheck = string.Empty;
            try
            {
                if (((userinfo == null) || string.IsNullOrEmpty(userinfo.UserName)) || string.IsNullOrEmpty(userinfo.Password))
                {
                    return "请输入账号密码";
                }
                userloginMsgForUnCheck = "用户名或者密码错误,请重新输入!";
                string str2 = Guid.NewGuid().ToString("b");
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@username", SqlDbType.VarChar, 50, userinfo.UserName), 
                    DataBase.MakeInParam("@password", SqlDbType.VarChar, 100, userinfo.Password), 
                    DataBase.MakeInParam("@loginip", SqlDbType.VarChar, 50, userinfo.LastLoginIp), 
                    DataBase.MakeInParam("@logintime", SqlDbType.DateTime, 8, DateTime.Now), 
                    DataBase.MakeInParam("@sessionId", SqlDbType.VarChar, 100, str2), 
                    DataBase.MakeInParam("@address", SqlDbType.VarChar, 20, userinfo.LastLoginAddress), 
                    DataBase.MakeInParam("@remark", SqlDbType.VarChar, 100, userinfo.LastLoginRemark), 
                    DataBase.MakeInParam("@email", SqlDbType.VarChar, 50, userinfo.Email), 
                    DataBase.MakeInParam("@loginType", SqlDbType.TinyInt, 1, userinfo.loginType),
                    DataBase.MakeInParam("@login_mac", SqlDbType.VarChar, 100, userinfo.login_mac), 
                };
                SqlDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "proc_users_Login", commandParameters);
                if (reader.Read())
                {
                    if (reader["status"] != DBNull.Value)
                    {
                        userinfo.Status = (int) reader["status"];
                        if (userinfo.Status == 1)
                        {
                            userloginMsgForUnCheck = SysConfig.UserloginMsgForUnCheck;
                        }
                        else if (userinfo.Status == 2)
                        {
                            userinfo.ID = (int) reader["userId"];
                            userinfo.UserType = (UserTypeEnum) Convert.ToInt32(reader["userType"]);
                            userinfo.IsEmailPass = Convert.ToInt32(reader["isEmailPass"]);
                            userloginMsgForUnCheck = "登录成功";
                            HttpContext.Current.Session["{10E6C4EE-54C1-4895-8CDE-202A5B3DD9E9}"] = str2;
                            if (userinfo.LastLoginRemark == "客户端登录")
                            {
                                HttpContext.Current.Session["{2A1FA22C-201B-471c-B668-2FCC1C4A121A}"] = userinfo.ID;
                            }
                        }
                        else if (userinfo.Status == 4)
                        {
                            userloginMsgForUnCheck = SysConfig.UserloginMsgForlock;
                        }
                        else if (userinfo.Status == 8)
                        {
                            userloginMsgForUnCheck = SysConfig.UserloginMsgForCheckfail;
                        }
                        else if (userinfo.Status == 16)
                        {
                            userloginMsgForUnCheck = SysConfig.UserloginLimitIPCheckfail;
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
            HttpContext.Current.Items["{FD7BE212-8537-427f-9EF6-1D1AABCA8EA3}"] = null;
            HttpContext.Current.Session["{10E6C4EE-54C1-4895-8CDE-202A5B3DD9E9}"] = null;
        }

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
                            if (DataBase.ExecuteNonQuery(transaction, "proc_usersupdate_add", (object[]) parameterArray2) < 0)
                            {
                                transaction.Rollback();
                                connection.Close();
                                return false;
                            }
                        }
                    }
                    if (DataBase.ExecuteNonQuery(transaction, "proc_users_Update", (object[]) parameterArray) > 0)
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

        public static UserInfo CurrentMember
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items["{FD7BE212-8537-427f-9EF6-1D1AABCA8EA3}"] == null)
                    {
                        int current = GetCurrent();
                        if (current <= 0)
                        {
                            return null;
                        }
                        HttpContext.Current.Items["{FD7BE212-8537-427f-9EF6-1D1AABCA8EA3}"] = GetCacheUserInfo(current);
                    }
                    return (HttpContext.Current.Items["{FD7BE212-8537-427f-9EF6-1D1AABCA8EA3}"] as UserInfo);
                }
                return null;
            }
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

        public static DataSet GetUserFirstLogin(int userId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = userId;
            UserInfo info = new UserInfo();
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_users_get_first_login", commandParameters);
        }

        public static int ChangeUserDefaultThemes(int userId, string defaultThemes)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@id", SqlDbType.Int, 10, userId), 
                DataBase.MakeInParam("@default_themes", SqlDbType.VarChar, 50, defaultThemes), 
             };

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_users_changethemes", commandParameters);
        }

        public static DataSet GetUserDayOrderChartSource(int userId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userId", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = userId;
            UserInfo info = new UserInfo();
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_users_getdayorderchart", commandParameters);

        }

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

        #region 支付回调


        #endregion
    }
}

