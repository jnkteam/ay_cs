namespace OriginalStudio.BLL.User
{
    using DBAccess;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class UserPayBankApp
    {
        internal const string SQL_TABLE = "V_userPayAcctChange";
        internal const string SQL_TABLE_FIELDS = "[id]\r\n      ,[userid]\r\n      ,[pmode]\r\n      ,[account],accoutType\r\n      ,[payeeName]\r\n      ,[payeeBank]\r\n      ,[bankProvince]\r\n      ,[bankCity]\r\n      ,[bankAddress]\r\n      ,[status]\r\n      ,[AddTime]\r\n      ,[SureTime]\r\n      ,[SureUser]\r\n      ,[userName]\r\n      ,[relname]";

        public static int Add(UserPayBankAppInfo model)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into userspaybankapp(");
                builder.Append("userid,accoutType,pmode,account,payeeName,payeeBank,bankProvince,bankCity,bankAddress,status,AddTime,SureTime,SureUser,BankCode,provinceCode,cityCode)");
                builder.Append(" values (");
                builder.Append("@userid,@accoutType,@pmode,@account,@payeeName,@payeeBank,@bankProvince,@bankCity,@bankAddress,@status,@AddTime,@SureTime,@SureUser,@BankCode,@provinceCode,@cityCode)");
                builder.Append(";select @@IDENTITY");
                SqlParameter[] cmdParms = new SqlParameter[] { 
                    new SqlParameter("@userid", SqlDbType.Int), 
                    new SqlParameter("@accoutType", SqlDbType.TinyInt, 1),
                    new SqlParameter("@pmode", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@account", SqlDbType.VarChar, 50), 
                    new SqlParameter("@payeeName", SqlDbType.VarChar, 50), 
                    new SqlParameter("@payeeBank", SqlDbType.VarChar, 50), 
                    new SqlParameter("@bankProvince", SqlDbType.VarChar, 50), 
                    new SqlParameter("@bankCity", SqlDbType.VarChar, 50), 
                    new SqlParameter("@bankAddress", SqlDbType.VarChar, 100), 
                    new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@AddTime", SqlDbType.DateTime), 
                    new SqlParameter("@SureTime", SqlDbType.DateTime), 
                    new SqlParameter("@SureUser", SqlDbType.Int, 10), 
                    new SqlParameter("@BankCode", SqlDbType.VarChar, 50), 
                    new SqlParameter("@provinceCode", SqlDbType.VarChar, 50), 
                    new SqlParameter("@cityCode", SqlDbType.VarChar, 50) 
                };
                cmdParms[0].Value = model.userid;
                cmdParms[1].Value = model.accoutType;
                cmdParms[2].Value = model.pmode;
                cmdParms[3].Value = model.account;
                cmdParms[4].Value = model.payeeName;
                cmdParms[5].Value = model.payeeBank;
                cmdParms[6].Value = model.bankProvince;
                cmdParms[7].Value = model.bankCity;
                cmdParms[8].Value = model.bankAddress;
                cmdParms[9].Value = model.status;
                cmdParms[10].Value = model.AddTime;
                cmdParms[11].Value = model.SureTime;
                cmdParms[12].Value = model.SureUser;
                cmdParms[13].Value = model.BankCode;
                cmdParms[14].Value = model.provinceCode;
                cmdParms[15].Value = model.cityCode;
                object single = DbHelperSQL.GetSingle(builder.ToString(), cmdParms);
                if (single == null)
                {
                    return 0;
                }
                return Convert.ToInt32(single);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
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
                        case "userid":
                            builder.Append(" AND [userid] = @userid");
                            parameter = new SqlParameter("@userid", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "status":
                            builder.Append(" AND [status] = @status");
                            parameter = new SqlParameter("@status", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "stime":
                            builder.Append(" AND [AddTime] >= @stime");
                            parameter = new SqlParameter("@stime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "etime":
                            builder.Append(" AND [AddTime] <= @etime");
                            parameter = new SqlParameter("@etime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public static bool Check(UserPayBankAppInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@SureTime", SqlDbType.DateTime), new SqlParameter("@SureUser", SqlDbType.Int, 10) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = (int) model.status;
                commandParameters[2].Value = model.SureTime;
                commandParameters[3].Value = model.SureUser;
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userspaybankapp_Check", commandParameters) > 0)
                {
                    UserFactory.ClearCache(model.userid);
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userspaybankapp_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Exists(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = userId;
                int num = (int) DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_userspaybankapp_Exists", commandParameters);
                return (num == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Exists2(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = userId;
                int num = (int) DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_userspaybank_Exists", commandParameters);
                return (num == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Exists3(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = userId;
                int num = (int) DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_userspaybankapp_Exists", commandParameters);
                return (num == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static int GetIsRead(int userId)
        {
            try
            {
                string commandText = "select count(0) from userspaybankapp(nolock) where userid=@PID";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@PID", SqlDbType.Int, 10) };
                commandParameters[0].Value = userId;
                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.Text, commandText, commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,userid,accoutType,pmode,account,payeeName,payeeBank,bankProvince,bankCity,bankAddress,status,AddTime,SureTime,SureUser,BankCode,provinceCode,cityCode ");
            builder.Append(" FROM userspaybankapp ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(builder.ToString());
        }

        public static UserPayBankAppInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            UserPayBankAppInfo info = new UserPayBankAppInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_userspaybankapp_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                }
                if (set.Tables[0].Rows[0]["userid"].ToString() != "")
                {
                    info.userid = int.Parse(set.Tables[0].Rows[0]["userid"].ToString());
                }
                if (set.Tables[0].Rows[0]["pmode"].ToString() != "")
                {
                    info.pmode = int.Parse(set.Tables[0].Rows[0]["pmode"].ToString());
                }
                info.account = set.Tables[0].Rows[0]["account"].ToString();
                info.payeeName = set.Tables[0].Rows[0]["payeeName"].ToString();
                info.payeeBank = set.Tables[0].Rows[0]["payeeBank"].ToString();
                info.bankProvince = set.Tables[0].Rows[0]["bankProvince"].ToString();
                info.bankCity = set.Tables[0].Rows[0]["bankCity"].ToString();
                info.bankAddress = set.Tables[0].Rows[0]["bankAddress"].ToString();
                if (set.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.status = (AcctChangeEnum) int.Parse(set.Tables[0].Rows[0]["status"].ToString());
                }
                if (set.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    info.AddTime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["AddTime"].ToString()));
                }
                if (set.Tables[0].Rows[0]["SureTime"].ToString() != "")
                {
                    info.SureTime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["SureTime"].ToString()));
                }
                if (set.Tables[0].Rows[0]["SureUser"].ToString() != "")
                {
                    info.SureUser = new int?(int.Parse(set.Tables[0].Rows[0]["SureUser"].ToString()));
                }
                return info;
            }
            return null;
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_userPayAcctChange";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userid]\r\n      ,[pmode]\r\n      ,[account],accoutType\r\n      ,[payeeName]\r\n      ,[payeeBank]\r\n      ,[bankProvince]\r\n      ,[bankCity]\r\n      ,[bankAddress]\r\n      ,[status]\r\n      ,[AddTime]\r\n      ,[SureTime]\r\n      ,[SureUser]\r\n      ,[userName]\r\n      ,[relname]", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static bool Update(UserPayBankAppInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@pmode", SqlDbType.TinyInt, 1), new SqlParameter("@account", SqlDbType.VarChar, 50), new SqlParameter("@payeeName", SqlDbType.VarChar, 50), new SqlParameter("@payeeBank", SqlDbType.VarChar, 50), new SqlParameter("@bankProvince", SqlDbType.VarChar, 50), new SqlParameter("@bankCity", SqlDbType.VarChar, 50), new SqlParameter("@bankAddress", SqlDbType.VarChar, 100), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@AddTime", SqlDbType.DateTime), new SqlParameter("@SureTime", SqlDbType.DateTime), new SqlParameter("@SureUser", SqlDbType.Int, 10) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.pmode;
                commandParameters[3].Value = model.account;
                commandParameters[4].Value = model.payeeName;
                commandParameters[5].Value = model.payeeBank;
                commandParameters[6].Value = model.bankProvince;
                commandParameters[7].Value = model.bankCity;
                commandParameters[8].Value = model.bankAddress;
                commandParameters[9].Value = model.status;
                commandParameters[10].Value = model.AddTime;
                commandParameters[11].Value = model.SureTime;
                commandParameters[12].Value = model.SureUser;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userspaybankapp_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

