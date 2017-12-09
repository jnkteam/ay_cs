namespace OriginalStudio.BLL.Sys
{
    using DBAccess;
    using OriginalStudio.Model.Sys;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    /// <summary>
    /// 调试信息日志操作
    /// </summary>
    public sealed class DebugLog
    {
        internal const string SQL_TABLE = "v_debuginfo";
        internal const string SQL_TABLE_FIELDS = "id,bugtype,userid,userName,url,errorcode,errorinfo,detail,addtime,userorder";

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

                        case "userorder":
                            builder.Append(" AND [userorder] like @userorder");
                            parameter = new SqlParameter("@userorder", SqlDbType.VarChar, 30);
                            parameter.Value = "%" + param2.ParamValue.ToString() + "%";
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

        public static DebugInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            DebugInfo debuginfo = new DebugInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_debuginfo_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    debuginfo.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                }
                if (set.Tables[0].Rows[0]["bugtype"].ToString() != "")
                {
                    debuginfo.bugtype = (LogTypeEnum) int.Parse(set.Tables[0].Rows[0]["bugtype"].ToString());
                }
                if (set.Tables[0].Rows[0]["userid"].ToString() != "")
                {
                    debuginfo.userid = new int?(int.Parse(set.Tables[0].Rows[0]["userid"].ToString()));
                }
                debuginfo.url = set.Tables[0].Rows[0]["url"].ToString();
                debuginfo.errorcode = set.Tables[0].Rows[0]["errorcode"].ToString();
                debuginfo.errorinfo = set.Tables[0].Rows[0]["errorinfo"].ToString();
                debuginfo.detail = set.Tables[0].Rows[0]["detail"].ToString();
                debuginfo.userorder = set.Tables[0].Rows[0]["userorder"].ToString();
                if (set.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    debuginfo.addtime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["addtime"].ToString()));
                }
                return debuginfo;
            }
            return null;
        }

        public static int Insert(DebugInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@bugtype", SqlDbType.TinyInt, 8), 
                    new SqlParameter("@userid", SqlDbType.Int), 
                    new SqlParameter("@url", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@errorcode", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@errorinfo", SqlDbType.VarChar, 2000), 
                    new SqlParameter("@detail", SqlDbType.VarChar, 8000), 
                    new SqlParameter("@addtime", SqlDbType.DateTime), 
                    new SqlParameter("@userorder", SqlDbType.VarChar, 30) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = (int) model.bugtype;
                commandParameters[2].Value = model.userid;
                commandParameters[3].Value = model.url;
                commandParameters[4].Value = model.errorcode;
                commandParameters[5].Value = model.errorinfo;
                commandParameters[6].Value = model.detail;
                commandParameters[7].Value = model.addtime;
                commandParameters[8].Value = model.userorder;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_debuginfo_Insert", commandParameters);
                if (obj2 != null)
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

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_debuginfo";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("id,bugtype,userid,userName,url,errorcode,errorinfo,detail,addtime,userorder", tables, wheres, orderby, key, pageSize, page, false);
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

