namespace KuaiCard.BLL.Settled
{
    using DBAccess;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class Trade
    {
        internal const string SQL_TABLE = "V_Trade";
        internal const string SQL_TABLE_FIELDS = "[id]\r\n      ,[userid]\r\n      ,[type]\r\n      ,[billType]\r\n      ,[billNo]\r\n      ,[tradeTime]\r\n      ,[Amt]\r\n      ,[Balance]\r\n      ,[Remark]\r\n      ,[username]";

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

                        case "billtype":
                            builder.Append(" AND [billtype] = @billtype");
                            parameter = new SqlParameter("@billtype", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "supplier":
                            builder.Append(" AND exists(select 0 from ordercard with(nolock) where v_trade.billNo = ordercard.orderid and ordercard.supplierID = @supplier)");
                            parameter = new SqlParameter("@supplier", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public static decimal GetNdaysIncome(int classid, int userid, int days)
        {
            try
            {
                DateTime sdate = DateTime.Today.AddDays((double) (-days + 1));
                DateTime edate = DateTime.Today.AddDays(1.0);
                return GetUserIncome(classid, userid, sdate, edate);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static decimal GetUserIncome(int userId, DateTime sdate, DateTime edate)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@userid", SqlDbType.Int), 
                    new SqlParameter("@btime", SqlDbType.VarChar, 10), 
                    new SqlParameter("@etime", SqlDbType.VarChar, 10) 
                };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = sdate.ToString("yyyy-MM-dd");
                commandParameters[2].Value = edate.ToString("yyyy-MM-dd");
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_trade_getuserIncome", commandParameters);
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

        public static decimal GetUserIncome(int classid, int userId, DateTime sdate, DateTime edate)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@userid", SqlDbType.Int), 
                    new SqlParameter("@classid", SqlDbType.TinyInt), 
                    new SqlParameter("@btime", SqlDbType.DateTime, 8), 
                    new SqlParameter("@etime", SqlDbType.DateTime, 8) 
                };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = classid;
                commandParameters[2].Value = sdate;
                commandParameters[3].Value = edate;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_trade_getuserIncomex", commandParameters);
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

        public static decimal GetUserIncome2(int userId, DateTime sdate, DateTime edate)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int), new SqlParameter("@btime", SqlDbType.DateTime, 8), new SqlParameter("@etime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = sdate;
                commandParameters[2].Value = edate;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_trade_getuserIncome2", commandParameters);
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

        public static decimal GetUserOrderAmt(int userId, DateTime sdate, DateTime edate)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int), new SqlParameter("@btime", SqlDbType.VarChar, 10), new SqlParameter("@etime", SqlDbType.VarChar, 10) };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = sdate.ToString("yyyy-MM-dd");
                commandParameters[2].Value = edate.ToString("yyyy-MM-dd");
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_order_getuserOrdAmt", commandParameters);
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
                string tables = "V_Trade";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "userid asc,id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userid]\r\n      ,[type]\r\n      ,[billType]\r\n      ,[billNo]\r\n      ,[tradeTime]\r\n      ,[Amt]\r\n      ,[Balance]\r\n      ,[Remark]\r\n      ,[username]", tables, wheres, orderby, key, pageSize, page, false) + "\r\nselect \r\n sum(case when [type] = 1 then Amt else 0 end) income\r\n,sum(case when [billType] = 2 then Amt else 0 end) agentincome\r\n,sum(case when [type] = 0 then 0-Amt else 0 end) expenditure from v_trade where " + wheres;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static DataSet GetUserLeftBalance(int p_schemeid, int p_userid, DateTime sdate, DateTime edate)
        {
            DataSet set = new DataSet();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@schemeid", SqlDbType.Int), 
                    new SqlParameter("@userid", SqlDbType.Int), 
                    new SqlParameter("@btime", SqlDbType.VarChar, 10), 
                    new SqlParameter("@etime", SqlDbType.VarChar, 10) 
                };
                commandParameters[0].Value = p_schemeid;
                commandParameters[1].Value = p_userid;
                commandParameters[2].Value = sdate.ToString("yyyy-MM-dd");
                commandParameters[3].Value = edate.ToString("yyyy-MM-dd");
                set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_user_getleftbalance", commandParameters);
                return set;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

    }
}

