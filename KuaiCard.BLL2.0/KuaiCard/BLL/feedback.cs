namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Cache;
    using KuaiCard.Model;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using KuaiCard.BLL.Sys;

    public class feedback
    {
        public static string CACHE_KEY = (Constant.Cache_Mark + "USERHOST_{0}");
        internal const string SQL_FIELDS = "[id]\r\n      ,[userid]\r\n      ,[typeid]\r\n      ,[title]\r\n      ,[cont]\r\n      ,[status]\r\n      ,[addtime]\r\n      ,[reply]\r\n      ,[replyer]\r\n      ,[replytime]\r\n      ,[userName]\r\n      ,[replyname]\r\n      ,[relname],clientip";
        internal const string SQL_TABLE = "V_feedback";

        public int Add(feedbackInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@typeid", SqlDbType.TinyInt, 1), new SqlParameter("@title", SqlDbType.NVarChar, 50), new SqlParameter("@cont", SqlDbType.NVarChar, 200), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@reply", SqlDbType.NVarChar, 50), new SqlParameter("@replyer", SqlDbType.Int, 10), new SqlParameter("@replytime", SqlDbType.DateTime), new SqlParameter("@clientip", SqlDbType.VarChar, 20) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.typeid;
                commandParameters[3].Value = model.title;
                commandParameters[4].Value = model.cont;
                commandParameters[5].Value = model.status;
                commandParameters[6].Value = model.addtime;
                commandParameters[7].Value = model.reply;
                commandParameters[8].Value = model.replyer;
                commandParameters[9].Value = model.replytime;
                commandParameters[10].Value = model.clientip;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_feedback_add", commandParameters);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
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
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_feedback_ChangeStatus", commandParameters) > 0;
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
            string objId = string.Format(CACHE_KEY, id);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        public bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_feedback_Delete", commandParameters) > 0;
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
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_feedback_Exists", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public feedbackInfo GetCacheModel(int id)
        {
            feedbackInfo o = new feedbackInfo();
            string objId = string.Format(CACHE_KEY, id);
            o = (feedbackInfo) WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("id", id);
                SqlDependency dependency = DataBase.AddSqlDependency(objId, "V_feedback", "[id]\r\n      ,[userid]\r\n      ,[typeid]\r\n      ,[title]\r\n      ,[cont]\r\n      ,[status]\r\n      ,[addtime]\r\n      ,[reply]\r\n      ,[replyer]\r\n      ,[replytime]\r\n      ,[userName]\r\n      ,[replyname]\r\n      ,[relname],clientip", "[id]=@id", parameters);
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
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_feedback_GetList", commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public feedbackInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_feedback_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public feedbackInfo GetModel(int id, int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                commandParameters[1].Value = userid;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_feedback_GetModelByuser", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static feedbackInfo GetModelFromDs(DataSet ds)
        {
            feedbackInfo info = new feedbackInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
                {
                    info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["typeid"].ToString() != "")
                {
                    info.typeid = (feedbacktype) int.Parse(ds.Tables[0].Rows[0]["typeid"].ToString());
                }
                info.title = ds.Tables[0].Rows[0]["title"].ToString();
                info.cont = ds.Tables[0].Rows[0]["cont"].ToString();
                if (ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.status = (feedbackstatus) int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    info.addtime = DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString());
                }
                info.reply = ds.Tables[0].Rows[0]["reply"].ToString();
                if (ds.Tables[0].Rows[0]["replyer"].ToString() != "")
                {
                    info.replyer = new int?(int.Parse(ds.Tables[0].Rows[0]["replyer"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["replytime"].ToString() != "")
                {
                    info.replytime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["replytime"].ToString()));
                }
                return info;
            }
            return null;
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_feedback";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc,userid desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = this.BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userid]\r\n      ,[typeid]\r\n      ,[title]\r\n      ,[cont]\r\n      ,[status]\r\n      ,[addtime]\r\n      ,[reply]\r\n      ,[replyer]\r\n      ,[replytime]\r\n      ,[userName]\r\n      ,[replyname]\r\n      ,[relname],clientip", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(feedbackInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@typeid", SqlDbType.TinyInt, 1), new SqlParameter("@title", SqlDbType.NVarChar, 50), new SqlParameter("@cont", SqlDbType.NVarChar, 200), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@reply", SqlDbType.NVarChar, 50), new SqlParameter("@replyer", SqlDbType.Int, 10), new SqlParameter("@replytime", SqlDbType.DateTime) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = (int) model.typeid;
                commandParameters[3].Value = model.title;
                commandParameters[4].Value = model.cont;
                commandParameters[5].Value = (int) model.status;
                commandParameters[6].Value = model.addtime;
                commandParameters[7].Value = model.reply;
                commandParameters[8].Value = model.replyer;
                commandParameters[9].Value = model.replytime;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_feedback_update", commandParameters) > 0;
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

