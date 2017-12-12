namespace OriginalStudio.BLL.User
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
    using OriginalStudio.Model.User;

    public class UsersAmtFreeze
    {
        internal const string SQL_TABLE = "v_usersAmtFreeze";
        internal const string SQL_TABLE_FIELDS = "*";

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

        /// <summary>
        /// 冻结款项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Freeze(UsersAmtFreezeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@result", SqlDbType.Bit),
                    new SqlParameter("@userid", SqlDbType.Int, 10),
                    new SqlParameter("@Freeze", SqlDbType.Decimal, 9),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@manageId", SqlDbType.Int, 10),
                    new SqlParameter("@status", SqlDbType.TinyInt, 1),
                    new SqlParameter("@why", SqlDbType.VarChar, 50),
                    new SqlParameter("@unfreezemode", SqlDbType.TinyInt, 1)
                };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.UserID;
                commandParameters[2].Value = model.FreezeAmt;
                commandParameters[3].Value = model.Addtime;
                commandParameters[4].Value = model.ManageId;
                commandParameters[5].Value = (int)model.Status;
                commandParameters[6].Value = model.Why;
                commandParameters[7].Value = (int)model.UnFreezeMode;
                return ((DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_usersAmt_Freeze", commandParameters) > 0) && ((bool)commandParameters[0].Value));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        /// <summary>
        /// 解冻款项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool UnFreeze(int id, AmtunFreezeMode mode)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@result", SqlDbType.Bit),
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@checktime", SqlDbType.DateTime),
                    new SqlParameter("@unfreezemode", SqlDbType.TinyInt, 1)
                };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = id;
                commandParameters[2].Value = DateTime.Now;
                commandParameters[3].Value = (int) mode;
                return ((DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_usersAmt_unFreeze", commandParameters) > 0) && ((bool) commandParameters[0].Value));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

