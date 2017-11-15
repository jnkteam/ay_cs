namespace OriginalStudio.BLL.Settled
{
    using DBAccess;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class UsersAmtFreeze
    {
        internal const string SQL_TABLE = "v_usersAmtFreeze";
        internal const string SQL_TABLE_FIELDS = "[id]\r\n      ,[userid]\r\n      ,[freezeAmt]\r\n      ,[addtime]\r\n      ,[manageId]\r\n      ,[status]\r\n      ,[checktime]\r\n      ,[why]\r\n      ,[unfreezemode],username,full_name";

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

        public static bool Freeze(UsersAmtFreezeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@result", SqlDbType.Bit), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@Freeze", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@manageId", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@why", SqlDbType.VarChar, 50), new SqlParameter("@unfreezemode", SqlDbType.TinyInt, 1) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.freezeAmt;
                commandParameters[3].Value = model.addtime;
                commandParameters[4].Value = model.manageId;
                commandParameters[5].Value = (int) model.status;
                commandParameters[6].Value = model.why;
                commandParameters[7].Value = (int) model.unfreezemode;
                return ((DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersAmt_Freeze", commandParameters) > 0) && ((bool) commandParameters[0].Value));
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
                string tables = "v_usersAmtFreeze";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userid]\r\n      ,[freezeAmt]\r\n      ,[addtime]\r\n      ,[manageId]\r\n      ,[status]\r\n      ,[checktime]\r\n      ,[why]\r\n      ,[unfreezemode],username,full_name", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static bool unFreeze(int id, AmtunFreezeMode mode)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@result", SqlDbType.Bit), new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@checktime", SqlDbType.DateTime), new SqlParameter("@unfreezemode", SqlDbType.TinyInt, 1) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = id;
                commandParameters[2].Value = DateTime.Now;
                commandParameters[3].Value = (int) mode;
                return ((DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersAmt_unFreeze", commandParameters) > 0) && ((bool) commandParameters[0].Value));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

