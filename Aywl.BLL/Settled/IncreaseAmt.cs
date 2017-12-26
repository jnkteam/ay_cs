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

    public sealed class IncreaseAmt
    {
        internal const string SQL_TABLE = "V_increaseAmt";
        internal const string SQL_TABLE_FIELDS = "*";

        public static int Add(IncreaseAmtInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@userId", SqlDbType.Int, 10),
                    new SqlParameter("@increaseAmt", SqlDbType.Decimal, 9),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@mangeId", SqlDbType.Int, 10),
                    new SqlParameter("@mangeName", SqlDbType.NVarChar, 50),
                    new SqlParameter("@status", SqlDbType.TinyInt, 1),
                    new SqlParameter("@desc", SqlDbType.NVarChar, 100),
                    new SqlParameter("@optype", SqlDbType.TinyInt)
                };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.UserID;
                commandParameters[2].Value = model.IncreaseAmt;
                commandParameters[3].Value = model.AddTime;
                commandParameters[4].Value = model.MangeId;
                commandParameters[5].Value = model.MangeName;
                commandParameters[6].Value = model.Status;
                commandParameters[7].Value = model.Desc;
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

                        case "merchantname":
                            builder.Append(" AND [merchantname] = @merchantname");
                            parameter = new SqlParameter("@merchantname", SqlDbType.VarChar);
                            parameter.Value = (string)param2.ParamValue;
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
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_increaseAmt_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.ID = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                }
                if (set.Tables[0].Rows[0]["userId"].ToString() != "")
                {
                    info.UserID = int.Parse(set.Tables[0].Rows[0]["userId"].ToString());
                }
                if (set.Tables[0].Rows[0]["increaseAmt"].ToString() != "")
                {
                    info.IncreaseAmt = decimal.Parse(set.Tables[0].Rows[0]["increaseAmt"].ToString());
                }
                if (set.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    info.AddTime = DateTime.Parse(set.Tables[0].Rows[0]["addtime"].ToString());
                }
                if (set.Tables[0].Rows[0]["mangeId"].ToString() != "")
                {
                    info.MangeId = int.Parse(set.Tables[0].Rows[0]["mangeId"].ToString());
                }
                info.MangeName = set.Tables[0].Rows[0]["mangeName"].ToString();
                if (set.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.Status = int.Parse(set.Tables[0].Rows[0]["status"].ToString());
                }
                info.Desc = set.Tables[0].Rows[0]["desc"].ToString();
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
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + 
                    SqlHelper.GetPageSelectSQL("*", tables, wheres, orderby, key, pageSize, page, false);
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

