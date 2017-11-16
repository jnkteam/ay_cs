namespace OriginalStudio.BLL.Order
{
    using OriginalStudio.DBAccess;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class Helper
    {
        public int search_check(int o_userid, string userorderid, out DataRow row)
        {
            row = null;
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@o_userid", SqlDbType.Int, 10),
                new SqlParameter("@userorderid_str", SqlDbType.VarChar, 30),
                new SqlParameter("@result", SqlDbType.TinyInt)
            };
            commandParameters[0].Value = o_userid;
            commandParameters[1].Value = userorderid;
            commandParameters[2].Direction = ParameterDirection.Output;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_order_search_chk", commandParameters);
            int num = Convert.ToInt32(commandParameters[2].Value);
            if (num == 0)
            {
                row = set.Tables[0].Rows[0];
            }
            return num;
        }
    }
}

