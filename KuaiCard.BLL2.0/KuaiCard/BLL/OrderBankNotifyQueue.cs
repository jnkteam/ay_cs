namespace KuaiCard.BLL
{
    using DBAccess;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;
    using KuaiCard.Model.Order;

    public class OrderBankNotifyQueue
    {
        /// <summary>
        /// 添加订单至通知队列
        /// </summary>
        /// <param name="order"></param>
        public void AddNotifyQueue(KuaiCard.Model.Order.OrderBankNotifyQueue order)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 50), 
                new SqlParameter("@userid", SqlDbType.BigInt, 20), 
                new SqlParameter("@notify_url", SqlDbType.VarChar, 2000)
            };
            commandParameters[0].Value = order.OrderID;
            commandParameters[1].Value = order.UserID;
            commandParameters[2].Value = order.NotifyUrl;

            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_queue_notify_add", commandParameters);
        }
    }
}

