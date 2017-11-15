namespace KuaiCard.BLL.Settled
{
    using DBAccess;
    using KuaiCard.Model.Settled;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public sealed class IncreaseAmt
    {
        internal const string SQL_TABLE = "V_increaseAmt";
        internal const string SQL_TABLE_FIELDS = "[id]\r\n      ,[userId]\r\n      ,[increaseAmt]\r\n      ,[addtime]\r\n      ,[mangeId]\r\n      ,[mangeName]\r\n      ,[status]\r\n      ,[desc],[username],[optype]";

        public static int Add(IncreaseAmtInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@increaseAmt", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@mangeId", SqlDbType.Int, 10), new SqlParameter("@mangeName", SqlDbType.NVarChar, 50), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@desc", SqlDbType.NVarChar, 100), new SqlParameter("@optype", SqlDbType.TinyInt) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userId;
                commandParameters[2].Value = model.increaseAmt;
                commandParameters[3].Value = model.addtime;
                commandParameters[4].Value = model.mangeId;
                commandParameters[5].Value = model.mangeName;
                commandParameters[6].Value = model.status;
                commandParameters[7].Value = model.desc;
                commandParameters[8].Value = (int) model.optype;
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_increaseAmt_Insert", commandParameters) > 0)
                {
                    return (int) commandParameters[0].Value;
                }
                return 0;
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

        public static IncreaseAmtInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            IncreaseAmtInfo info = new IncreaseAmtInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Proc_increaseAmt_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                }
                if (set.Tables[0].Rows[0]["userId"].ToString() != "")
                {
                    info.userId = new int?(int.Parse(set.Tables[0].Rows[0]["userId"].ToString()));
                }
                if (set.Tables[0].Rows[0]["increaseAmt"].ToString() != "")
                {
                    info.increaseAmt = new decimal?(decimal.Parse(set.Tables[0].Rows[0]["increaseAmt"].ToString()));
                }
                if (set.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    info.addtime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["addtime"].ToString()));
                }
                if (set.Tables[0].Rows[0]["mangeId"].ToString() != "")
                {
                    info.mangeId = new int?(int.Parse(set.Tables[0].Rows[0]["mangeId"].ToString()));
                }
                info.mangeName = set.Tables[0].Rows[0]["mangeName"].ToString();
                if (set.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.status = new int?(int.Parse(set.Tables[0].Rows[0]["status"].ToString()));
                }
                info.desc = set.Tables[0].Rows[0]["desc"].ToString();
                return info;
            }
            return null;
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_increaseAmt";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "userid asc,id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userId]\r\n      ,[increaseAmt]\r\n      ,[addtime]\r\n      ,[mangeId]\r\n      ,[mangeName]\r\n      ,[status]\r\n      ,[desc],[username],[optype]", tables, wheres, orderby, key, pageSize, page, false);
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

