namespace OriginalStudio.BLL.Email
{
    using DBAccess;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Model.Email;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class EmailCheck
    {
        internal const string SQL_FIELDS = "[id]\r\n      ,[userId]\r\n      ,[ptype]\r\n      ,[filesize]\r\n      ,[ptype1]\r\n      ,[filesize1]\r\n      ,[status]\r\n      ,[why]\r\n      ,[admin]\r\n      ,[checktime]\r\n      ,[addtime]\r\n      ,[userName],[payeeName],[account],[IdCard]";
        internal const string SQL_TABLE = "V_usersIdImage";

        public int Add(EmailCheckInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@typeid", SqlDbType.TinyInt, 1), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@email", SqlDbType.NVarChar, 50), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@checktime", SqlDbType.DateTime), new SqlParameter("@Expired", SqlDbType.DateTime), new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = (int) model.typeid;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.email;
                commandParameters[3].Value = model.addtime;
                commandParameters[4].Value = (int) model.status;
                commandParameters[5].Value = model.checktime;
                commandParameters[6].Value = model.Expired;
                commandParameters[7].Direction = ParameterDirection.Output;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_useremailcheck_add", commandParameters);
                return (int) commandParameters[7].Value;
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

                        case "username":
                            builder.Append(" AND [userName] like @UserName");
                            parameter = new SqlParameter("@UserName", SqlDbType.VarChar, 20);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 20) + "%";
                            paramList.Add(parameter);
                            break;

                        case "status":
                            builder.Append(" AND [status] = @status");
                            parameter = new SqlParameter("@status", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "stime":
                            builder.Append(" AND [addtime] >= @stime");
                            parameter = new SqlParameter("@stime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "etime":
                            builder.Append(" AND [addtime] <= @etime");
                            parameter = new SqlParameter("@etime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_useremailcheck_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool Exists(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = userid;
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_useremailcheck_Exists", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public EmailCheckInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_useremailcheck_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public EmailCheckInfo GetModelByUser(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10) };
                commandParameters[0].Value = userid;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_useremailcheck_GetByUser", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static EmailCheckInfo GetModelFromDs(DataSet ds)
        {
            EmailCheckInfo info = new EmailCheckInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["typeid"].ToString() != "")
                {
                    info.typeid = (EmailCheckType) int.Parse(ds.Tables[0].Rows[0]["typeid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
                {
                    info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                }
                info.email = ds.Tables[0].Rows[0]["email"].ToString();
                if (ds.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    info.addtime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.status = (EmailCheckStatus) int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["checktime"].ToString() != "")
                {
                    info.checktime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["checktime"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["Expired"].ToString() != "")
                {
                    info.Expired = DateTime.Parse(ds.Tables[0].Rows[0]["Expired"].ToString());
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
                string tables = "V_usersIdImage";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userId]\r\n      ,[ptype]\r\n      ,[filesize]\r\n      ,[ptype1]\r\n      ,[filesize1]\r\n      ,[status]\r\n      ,[why]\r\n      ,[admin]\r\n      ,[checktime]\r\n      ,[addtime]\r\n      ,[userName],[payeeName],[account],[IdCard]", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(EmailCheckInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@typeid", SqlDbType.TinyInt, 1), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@email", SqlDbType.NVarChar, 50), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@checktime", SqlDbType.DateTime) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = (int) model.typeid;
                commandParameters[2].Value = model.userid;
                commandParameters[3].Value = model.email;
                commandParameters[4].Value = (int) model.status;
                commandParameters[5].Value = model.checktime;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_useremailcheck_update", commandParameters) > 0;
                if (flag && (model.status == EmailCheckStatus.已审核))
                {
                    UserFactory.ClearCache(model.userid);
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

