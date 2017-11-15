namespace KuaiCard.BLL.channel
{
    using DBAccess;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class currchannel
    {
        public static int Get(int userid, int typeid, int len, string serverid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@typeid", SqlDbType.Int, 10), new SqlParameter("@len", SqlDbType.Int, 10), new SqlParameter("@serverid", SqlDbType.VarChar, 50), new SqlParameter("@datetime", SqlDbType.DateTime) };
                commandParameters[0].Value = userid;
                commandParameters[1].Value = typeid;
                commandParameters[2].Value = len;
                commandParameters[3].Value = serverid;
                commandParameters[4].Value = DateTime.Now;
                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_currchannel_get", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 1;
            }
        }
    }
}

