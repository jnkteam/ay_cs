namespace OriginalStudio.BLL.Stat
{
    using DBAccess;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

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
    }
}

