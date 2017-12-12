namespace OriginalStudio.BLL.User
{
    using DBAccess;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class ManageTrade
    {
        internal const string SQL_TABLE = "V_ManageTrade";
        internal const string SQL_TABLE_FIELDS = "[id]\r\n      ,[userid]\r\n      ,[type]\r\n      ,[billType]\r\n      ,[billNo]\r\n      ,[tradeTime]\r\n      ,[Amt]\r\n      ,[Balance]\r\n      ,[Remark]\r\n      ,[relname]";

        public static int Add(int manageId, int _type, int _billType, string billNo, DateTime tradeTime, decimal Amt, string Remark)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@manageid", SqlDbType.Int, 10), new SqlParameter("@type", SqlDbType.TinyInt, 1), new SqlParameter("@billType", SqlDbType.TinyInt, 1), new SqlParameter("@billNo", SqlDbType.NVarChar, 50), new SqlParameter("@tradeTime", SqlDbType.DateTime), new SqlParameter("@Amt", SqlDbType.Decimal, 9), new SqlParameter("@Remark", SqlDbType.VarChar, 100) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = manageId;
                commandParameters[2].Value = _type;
                commandParameters[3].Value = _billType;
                commandParameters[4].Value = billNo;
                commandParameters[5].Value = tradeTime;
                commandParameters[6].Value = Amt;
                commandParameters[7].Value = Remark;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_managetrade_add", commandParameters);
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

                        case "stime":
                            builder.Append(" AND [tradeTime] >= @stime");
                            parameter = new SqlParameter("@stime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "etime":
                            builder.Append(" AND [tradeTime] <= @etime");
                            parameter = new SqlParameter("@etime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public static decimal GetManageIncome(int ManageId, DateTime sdate, DateTime edate)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@ManageId", SqlDbType.Int), new SqlParameter("@btime", SqlDbType.DateTime, 8), new SqlParameter("@etime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = ManageId;
                commandParameters[1].Value = sdate;
                commandParameters[2].Value = edate;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_trade_getManageIncome", commandParameters);
                if (obj2 != DBNull.Value)
                {
                    return Convert.ToDecimal(obj2);
                }
                return 0M;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static decimal GetSettledAmt(int ManageId, DateTime sdate, DateTime edate)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@ManageId", SqlDbType.Int), new SqlParameter("@btime", SqlDbType.DateTime, 8), new SqlParameter("@etime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = ManageId;
                commandParameters[1].Value = sdate;
                commandParameters[2].Value = edate;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_Managetrade_get", commandParameters);
                if (obj2 != DBNull.Value)
                {
                    return Convert.ToDecimal(obj2);
                }
                return 0M;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_ManageTrade";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "userid asc,id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + 
                    SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userid]\r\n      ,[type]\r\n      ,[billType]\r\n      ,[billNo]\r\n      ,[tradeTime]\r\n      ,[Amt]\r\n      ,[Balance]\r\n      ,[Remark]\r\n      ,[relname]", tables, wheres, orderby, key, pageSize, page, false);
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

