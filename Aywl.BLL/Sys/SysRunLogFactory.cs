namespace OriginalStudio.BLL.Sys
{
    using DBAccess;
    using OriginalStudio.Model.Sys;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.Lib.Utils;

    /// <summary>
    /// 系统日志记录
    /// </summary>
    public sealed class SysRunLogFactory
    {
        #region 记录订单Http请求日志

        /// <summary>
        /// 记录Http请求日志
        /// </summary>
        /// <param name="p_merchant_name"></param>
        /// <param name="p_req_url"></param>
        /// <param name="p_req_ip"></param>
        /// <param name="p_req_type"></param>
        /// <returns></returns>
        public static int AddHttpRequestRunLog(string p_merchant_name, string p_req_url, string p_req_ip, int p_req_type)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@MerchantName", SqlDbType.VarChar, 20), 
                    new SqlParameter("@ReqUrl", SqlDbType.VarChar, 1000), 
                    new SqlParameter("@ReqIP", SqlDbType.VarChar, 20), 
                    new SqlParameter("@ReqType", SqlDbType.Int, 10) };
            commandParameters[0].Value = p_merchant_name;
            commandParameters[1].Value = p_req_url;
            commandParameters[2].Value = p_req_ip;
            commandParameters[3].Value = p_req_type;
            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_requst_add_runlog", commandParameters);
        }

        #endregion

        #region 查询订单调试日志

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_sysrunlog";
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
        
        #endregion

        #region 构造参数

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
                        case "merchantname":
                            builder.Append(" AND [MerchantName] = @merchantname");
                            parameter = new SqlParameter("@merchantname", SqlDbType.VarChar,30);
                            parameter.Value = (int)param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "userorder":
                            builder.Append(" AND [userorder] like @userorder");
                            parameter = new SqlParameter("@userorder", SqlDbType.VarChar, 30);
                            parameter.Value = "%" + param2.ParamValue.ToString() + "%";
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

        #endregion

        #region 日志详细
        
        public static SysRunLog GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = id;
            SysRunLog modle = new SysRunLog();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sysrunlog_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                DataRow dr = set.Tables[0].Rows[0];
                modle.Id = Utils.StrToInt(dr["id"].ToString());
                modle.Logtype = Utils.StrToInt(dr["Logtype"].ToString()); ;
                modle.UserID = Utils.StrToInt(dr["userid"]);
                modle.UserOrder = Convert.ToString(dr["userorder"]);
                modle.Url = Convert.ToString(dr["url"]);
                modle.ErrorCode = Convert.ToString(dr["errorcode"]);
                modle.ErrorInfo = Convert.ToString(dr["errorinfo"]);
                modle.Detail = Convert.ToString(dr["detail"]);
                modle.AddTime = Utils.StrToDateTime(dr["addtime"]);

                return modle;
            }
            return null;
        }



        #endregion
    }
}

