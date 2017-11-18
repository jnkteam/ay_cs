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

    public class UserAccessTime
    {
        internal const string SQL_FIELDS = "[id],[userid],[siteip],[sitetype],[hostName],[hostUrl],[status],[desc],[username]";
        internal const string SQL_TABLE = "V_userhost";

        public static bool Add(MchUserAccessTimeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@lastAccesstime", SqlDbType.DateTime) };
                commandParameters[0].Value = model.userid;
                commandParameters[1].Value = model.lastAccesstime;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usertime_add", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
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
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usertime_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public DataTable GetList(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int) };
                commandParameters[0].Value = userId;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_usertime_GetList", commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static MchUserAccessTimeInfo GetModel(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = userId;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_usertime_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return new MchUserAccessTimeInfo();
            }
        }

        public static MchUserAccessTimeInfo GetModelFromDs(DataSet ds)
        {
            MchUserAccessTimeInfo info = new MchUserAccessTimeInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
                {
                    info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["lastAccesstime"].ToString() != "")
                {
                    info.lastAccesstime = DateTime.Parse(ds.Tables[0].Rows[0]["lastAccesstime"].ToString());
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
                string tables = "V_userhost";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "userid desc,id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id],[userid],[siteip],[sitetype],[hostName],[hostUrl],[status],[desc],[username]", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }
    }
}

