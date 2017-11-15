namespace KuaiCard.BLL.User
{
    using DBAccess;
    using KuaiCard.Model.User;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class UserLoginLogFactory
    {
        internal const string FIELD_NEWS = "[id]\r\n      ,[type]\r\n      ,[userID]\r\n      ,[lastIP]\r\n      ,[address]\r\n      ,[remark]\r\n      ,[lastTime]\r\n      ,[sessionId]\r\n      ,[userName],[payeeName]";
        internal const string SQL_TABLE = "V_usersLoginLog";

        public static int Add(UserLoginLog logEntity)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into usersLoginLog(");
                builder.Append("type,userID,lastIP,address,remark,lastTime)");
                builder.Append(" values (");
                builder.Append("@type,@userID,@lastIP,@address,@remark,@lastTime)");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@type", logEntity.type), new SqlParameter("@userID", logEntity.userID), new SqlParameter("@lastIP", logEntity.lastIP), new SqlParameter("@address", logEntity.address), new SqlParameter("@remark", logEntity.remark), new SqlParameter("@lastTime", logEntity.lastTime) };
                return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters);
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
                            builder.Append(" AND [userName] like @userName");
                            parameter = new SqlParameter("@userName", SqlDbType.VarChar, 20);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 100) + "%";
                            paramList.Add(parameter);
                            break;

                        case "starttime":
                            builder.Append(" AND [lastTime] > @starttime");
                            parameter = new SqlParameter("@starttime", SqlDbType.DateTime);
                            parameter.Value = Convert.ToDateTime(param2.ParamValue);
                            paramList.Add(parameter);
                            break;

                        case "endtime":
                            builder.Append(" AND [lastTime] < @endtime");
                            parameter = new SqlParameter("@endtime", SqlDbType.DateTime);
                            parameter.Value = Convert.ToDateTime(param2.ParamValue);
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public static bool Del(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersLoginLog_del", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_usersLoginLog";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "lastTime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[type]\r\n      ,[userID]\r\n      ,[lastIP]\r\n      ,[address]\r\n      ,[remark]\r\n      ,[lastTime]\r\n      ,[sessionId]\r\n      ,[userName],[payeeName]", tables, wheres, orderby, key, pageSize, page, false);
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

