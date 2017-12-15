namespace OriginalStudio.BLL.Stat
{
    using DBAccess;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class OrderReport
    {
        public static DataTable ReportByUser(DateTime beginTime, DateTime endTime, int uid)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (uid > 0)
            {
                list.Add(DataBase.MakeInParam("@userid", SqlDbType.Int, 10, uid));
            }
            list.Add(DataBase.MakeInParam("@beginTime", SqlDbType.DateTime, 8, beginTime));
            list.Add(DataBase.MakeInParam("@endTime", SqlDbType.DateTime, 8, endTime));
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "Proc_Stat_ReportByUser", list.ToArray()).Tables[0];
        }

        /// <summary>
        /// 统计通道类型总数金额利润
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static DataSet 统计通道类型总数金额利润(DateTime beginTime, DateTime endTime)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@begindate",SqlDbType.DateTime),
                new SqlParameter("@enddate",SqlDbType.DateTime)
            };
            parameters[0].Value = beginTime;
            parameters[1].Value = endTime;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_统计通道类型总数金额利润", parameters);
        }

        /// <summary>
        /// 统计10之内每天利润及订单数
        /// </summary>
        /// <returns></returns>
        public static DataSet 统计每天利润及订单数()
        {
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_统计每天利润及订单数", null);
        }

        /// <summary>
        /// 统计每个小时总数金额利润
        /// </summary>
        /// <returns></returns>
        public static DataSet 统计每个小时总数金额利润()
        {
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_统计每个小时总数金额利润", null);
        }

        /// <summary>
        /// 统计通道商成功率。建议时间差设置为5个小时。
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static DataSet 统计通道商成功率(DateTime beginTime, DateTime endTime)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@begindate",SqlDbType.DateTime),
                new SqlParameter("@enddate",SqlDbType.DateTime)
            };
            parameters[0].Value = beginTime;
            parameters[1].Value = endTime;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_统计通道商成功率", parameters);
        }

        /// <summary>
        /// 统计商户收益
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="typeClassId">通道类型分类代码。0代表所有</param>
        /// <param name="merchantName">可以传递空，表示所有商户</param>
        /// <returns></returns>
        public static DataSet 统计商户收益(DateTime beginTime, DateTime endTime, int typeClassId, string merchantName)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@begindate",SqlDbType.DateTime),
                new SqlParameter("@enddate",SqlDbType.DateTime),
                new SqlParameter("@typeclassid",SqlDbType.Int),
                new SqlParameter("@merchantname",SqlDbType.VarChar,50)
            };
            parameters[0].Value = beginTime;
            parameters[1].Value = endTime;
            parameters[2].Value = typeClassId;
            parameters[3].Value = merchantName;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_商户收益", parameters);
        }

        /// <summary>
        /// 统计代理收益
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="typeClassId">通道类型分类代码。0代表所有</param>
        /// <param name="merchantName">可以传递空，表示所有代理</param>
        /// <returns></returns>
        public static DataSet 统计代理收益(DateTime beginTime, DateTime endTime, int typeClassId, string merchantName)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@begindate",SqlDbType.DateTime),
                new SqlParameter("@enddate",SqlDbType.DateTime),
                new SqlParameter("@typeclassid",SqlDbType.Int),
                new SqlParameter("@merchantname",SqlDbType.VarChar,50)
            };
            parameters[0].Value = beginTime;
            parameters[1].Value = endTime;
            parameters[2].Value = typeClassId;
            parameters[3].Value = merchantName;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_代理收益", parameters);
        }

        /// <summary>
        /// 统计通道类型总数金额利润
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="supplierId"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static DataSet 统计通道商订单金额利润(int supplierId,DateTime beginTime, DateTime endTime)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@supplierid",SqlDbType.Int),
                new SqlParameter("@begindate",SqlDbType.DateTime),
                new SqlParameter("@enddate",SqlDbType.DateTime)
            };
            parameters[0].Value = supplierId;
            parameters[1].Value = beginTime;
            parameters[2].Value = endTime;

            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "订单_统计通道商订单金额利润", parameters);
        }

        /// <summary>
        /// 商户收益统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="merchantName"></param>
        /// <param name="channelTypeId"></param>
        /// <param name="fromMoney"></param>
        /// <param name="toMoney"></param>
        /// <returns></returns>
        public static DataSet 统计商户收益统计(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_usersOrderIncome";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n"
                                    + SqlHelper.GetPageSelectSQL("*", tables, wheres, orderby, key, pageSize, page, false)
                                    + "\r\nselect sum(OrderCount) SumOrderCount,sum(TotalOrderValue) SumTotalOrderValue,sum(SumPay) SumPay,sum(TotalChareValue) SumTotalChareValue from V_usersOrderIncome where " + wheres;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }


        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SearchParam param2 = param[i];
                    if (param2.CmpOperator == "=")
                    {
                        SqlParameter parameter;
                        switch (param2.ParamKey.Trim().ToLower())
                        {
                            case "userid":
                                builder.Append(" AND [userid] = @userid");
                                parameter = new SqlParameter("@userid", SqlDbType.Int);
                                parameter.Value = (int)param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "merchantname":
                                builder.Append(" AND [MerchantName] = @merchantName");
                                parameter = new SqlParameter("@merchantName", SqlDbType.VarChar);
                                parameter.Value = (string)param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "stime":
                                builder.Append(" AND [OrderDate] >= @beginmydate");
                                parameter = new SqlParameter("@beginmydate", SqlDbType.VarChar, 10);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "etime":
                                builder.Append(" AND [OrderDate] <= @endmydate");
                                parameter = new SqlParameter("@endmydate", SqlDbType.VarChar, 10);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "fvaluefrom":
                                builder.Append(" AND [OrderValue] >= @fvaluefrom");
                                parameter = new SqlParameter("@fvaluefrom", SqlDbType.Decimal, 9);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "fvalueto":
                                builder.Append(" AND [OrderValue] <= @fvalueto");
                                parameter = new SqlParameter("@fvalueto", SqlDbType.Decimal, 9);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "typeid":
                                builder.Append(" AND [ChanneltypeId] = @ChanneltypeId");
                                parameter = new SqlParameter("@ChanneltypeId", SqlDbType.Int);
                                parameter.Value = (int)param2.ParamValue;
                                paramList.Add(parameter);
                                break;
                        }
                    }
                }
            }
            return builder.ToString();
        }
    }
}

