namespace KuaiCard.BLL
{
    using DBAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class PayLogFactory
    {
        public static int Add(int uid, decimal prices, decimal money)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, uid), 
                DataBase.MakeInParam("@AdsType", SqlDbType.TinyInt, 1, 2), 
                DataBase.MakeInParam("@AdsPrices", SqlDbType.Decimal, 8, prices), 
                DataBase.MakeInParam("@PayMoney", SqlDbType.Decimal, 8, money), 
                DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, DateTime.Now) 
            };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "PayLog_ADD", commandParameters) != 1)
            {
                return 0;
            }
            return 1;
        }
    }
}

