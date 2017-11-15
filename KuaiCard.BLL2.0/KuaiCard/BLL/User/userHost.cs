namespace KuaiCard.BLL.User
{
    using DBAccess;
    using KuaiCard.Cache;
    using KuaiCard.Model.User;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class userHost
    {
        public const string CACHE_KEY = "USERHOST_{0}";
        internal const string SQL_FIELDS = "[id],[userid],[siteip],[sitetype],[hostName],[hostUrl],[status],[desc],[username]";
        internal const string SQL_TABLE = "V_userhost";

        public int Add(userHostInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@siteip", SqlDbType.VarChar, 50), new SqlParameter("@sitetype", SqlDbType.TinyInt, 1), new SqlParameter("@hostName", SqlDbType.VarChar, 200), new SqlParameter("@hostUrl", SqlDbType.VarChar, 0xff), new SqlParameter("@desc", SqlDbType.VarChar, 0xff), new SqlParameter("@status", SqlDbType.TinyInt) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.siteip;
                commandParameters[3].Value = model.sitetype;
                commandParameters[4].Value = model.hostName;
                commandParameters[5].Value = model.hostUrl;
                commandParameters[6].Value = model.desc;
                commandParameters[7].Value = 1;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userhost_add", commandParameters);
                return (int) commandParameters[0].Value;
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

        public bool ChangeStatus(int id, int status)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt) };
                commandParameters[0].Value = id;
                commandParameters[1].Value = status;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userhost_ChangeStatus", commandParameters) > 0;
                if (flag)
                {
                    this.ClearCache(id);
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        internal void ClearCache(int id)
        {
            string objId = string.Format("USERHOST_{0}", id);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        public bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userhost_Delete", commandParameters) > 0;
                if (flag)
                {
                    this.ClearCache(id);
                }
                return flag;
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
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_userhost_Exists", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool Exists(int userid, string host)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@hostName", SqlDbType.VarChar, 200) };
                commandParameters[0].Value = userid;
                commandParameters[1].Value = host;
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_userhost_Exists2", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public userHostInfo GetCacheModel(int id)
        {
            userHostInfo o = new userHostInfo();
            string objId = string.Format("USERHOST_{0}", id);
            o = (userHostInfo) WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("id", id);
                SqlDependency dependency = DataBase.AddSqlDependency(objId, "V_userhost", "[id],[userid],[siteip],[sitetype],[hostName],[hostUrl],[status],[desc],[username]", "[id]=@id", parameters);
                o = this.GetModel(id);
                WebCache.GetCacheService().AddObject(objId, o);
            }
            return o;
        }

        public DataTable GetList(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int) };
                commandParameters[0].Value = userId;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_userhost_GetList", commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public userHostInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_userhost_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static userHostInfo GetModelFromDs(DataSet ds)
        {
            userHostInfo info = new userHostInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
                {
                    info.userid = new int?(int.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                }
                info.siteip = ds.Tables[0].Rows[0]["siteip"].ToString();
                if (ds.Tables[0].Rows[0]["sitetype"].ToString() != "")
                {
                    info.sitetype = new int?(int.Parse(ds.Tables[0].Rows[0]["sitetype"].ToString()));
                }
                info.hostName = ds.Tables[0].Rows[0]["hostName"].ToString();
                info.hostUrl = ds.Tables[0].Rows[0]["hostUrl"].ToString();
                info.desc = ds.Tables[0].Rows[0]["desc"].ToString();
                if (ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.status = (userHostStatus) int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                    return info;
                }
                info.status = userHostStatus.未知;
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

        public bool Update(userHostInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@siteip", SqlDbType.VarChar, 50), new SqlParameter("@sitetype", SqlDbType.TinyInt, 1), new SqlParameter("@hostName", SqlDbType.VarChar, 200), new SqlParameter("@hostUrl", SqlDbType.VarChar, 0xff), new SqlParameter("@desc", SqlDbType.VarChar, 0xff), new SqlParameter("@status", SqlDbType.TinyInt) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.siteip;
                commandParameters[3].Value = model.sitetype;
                commandParameters[4].Value = model.hostName;
                commandParameters[5].Value = model.hostUrl;
                commandParameters[6].Value = model.desc;
                commandParameters[7].Value = (int) model.status;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_userhost_update", commandParameters) > 0;
                if (flag)
                {
                    this.ClearCache(model.id);
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

