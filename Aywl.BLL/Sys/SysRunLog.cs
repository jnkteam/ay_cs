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

    /// <summary>
    /// 系统日志记录
    /// </summary>
    public sealed class SysRunLog
    {
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
    }
}

