﻿namespace OriginalStudio.BLL.Settled
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

    public class MchUsersAmtFactory
    {
        internal const string SQL_TABLE = "mch_userAmt";
        internal const string SQL_TABLE_FIELD = "[ID],[UserID],[Integral],[Freeze],[Balance],[Payment],[UnPayment],[unpayment2],(ISNULL([balance],0)-ISNULL([unpayment],0)-ISNULL([Freeze],0)) as enableAmt";

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter;
                    string str;
                    SearchParam param2 = param[i];
                    if (param2.CmpOperator == "=")
                    {
                        str = param2.ParamKey.Trim().ToLower();
                        if ((str != null) && (str == "userid"))
                        {
                            builder.Append(" AND [userid] = @userid");
                            parameter = new SqlParameter("@userid", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                        }
                    }
                    else
                    {
                        str = param2.ParamKey.Trim().ToLower();
                        if ((str != null) && (str == "balance"))
                        {
                            builder.AppendFormat(" AND [balance] {0} @balance", param2.CmpOperator);
                            parameter = new SqlParameter("@balance", SqlDbType.Decimal);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static MchUsersAmtInfo GetModel(int userId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@userId", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = userId;
            MchUsersAmtInfo info = new MchUsersAmtInfo();
            return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_userAmt_GetModel", commandParameters));
        }

        public static MchUsersAmtInfo GetModelFromDs(DataSet ds)
        {
            MchUsersAmtInfo info = new MchUsersAmtInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.ID = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["userId"].ToString() != "")
                {
                    info.UserID = int.Parse(ds.Tables[0].Rows[0]["userId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Integral"].ToString() != "")
                {
                    info.Integral = Int64.Parse(ds.Tables[0].Rows[0]["Integral"].ToString());
                }
                if (ds.Tables[0].Rows[0]["balance"].ToString() != "")
                {
                    info.Balance = decimal.Parse(ds.Tables[0].Rows[0]["balance"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payment"].ToString() != "")
                {
                    info.Payment = decimal.Parse(ds.Tables[0].Rows[0]["payment"].ToString());
                }
                if (ds.Tables[0].Rows[0]["unpayment"].ToString() != "")
                {
                    info.UnPayment = decimal.Parse(ds.Tables[0].Rows[0]["unpayment"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Freeze"].ToString() != "")
                {
                    info.Freeze = decimal.Parse(ds.Tables[0].Rows[0]["Freeze"].ToString());
                }
                if (ds.Tables[0].Rows[0]["enableAmt"].ToString() != "")
                {
                    info.EnableAmt = decimal.Parse(ds.Tables[0].Rows[0]["enableAmt"].ToString());
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
                string tables = "mch_userAmt";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userId]\r\n      ,[Integral]\r\n      ,[Freeze]\r\n      ,[balance]\r\n      ,[payment]\r\n      ,[unpayment],[Freeze],(ISNULL([balance],0)-ISNULL([unpayment],0)-ISNULL([Freeze],0)) as enableAmt", tables, wheres, orderby, key, pageSize, page, false);
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

