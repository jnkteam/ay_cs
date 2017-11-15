namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class PayFactory
    {
        public static int Add(PayListInfo model)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@ID", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, model.Uid), DataBase.MakeInParam("@Money", SqlDbType.Money, 8, model.Money), DataBase.MakeInParam("@Status", SqlDbType.Int, 10, model.Status), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, model.AddTime), DataBase.MakeInParam("@PayTime", SqlDbType.DateTime, 8, model.PayTime), DataBase.MakeInParam("@Tax", SqlDbType.Money, 8, model.Tax), DataBase.MakeInParam("@Charges", SqlDbType.Money, 8, model.Charges) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "UP_PayList_ADD", commandParameters) == 1)
            {
                return (int) parameter.Value;
            }
            return 0;
        }

        public static List<PayListInfo> GetListArray(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ID,Uid,Money,Status,AddTime,PayTime,Tax,Charges ");
            builder.Append(" FROM PayList ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            List<PayListInfo> list = new List<PayListInfo>();
            using (SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, builder.ToString()))
            {
                while (reader.Read())
                {
                    list.Add(ReaderBind(reader));
                }
            }
            return list;
        }

        public static PayListInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.Int, 10, id) };
            PayListInfo info = null;
            using (SqlDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "UP_PayList_GetModel", commandParameters))
            {
                if (reader.Read())
                {
                    info = ReaderBind(reader);
                }
            }
            return info;
        }

        public static DataSet getNotSuporder(int notifycount, int day)
        {
            try
            {
                string commandText = "select * from v_orderbank where notifystat=4 and status=2 and notifycount<@notifycount and datediff(day,addtime,getdate())<=@day";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@notifycount", SqlDbType.Int, 10), new SqlParameter("@day", SqlDbType.Int, 10) };
                commandParameters[0].Value = notifycount;
                commandParameters[1].Value = day;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static decimal GetPayDayMoney(int uid)
        {
            string commandText = "SELECT ISNULL(SUM([Amount]*[Pay_Price]),0) FROM [User_Pay_Order] where Status = 2 and datediff(day,CompleteTime,getdate())=0 and UserId=" + uid.ToString();
            return decimal.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, commandText));
        }

        public static decimal Getpayingmoney(int uid)
        {
            string commandText = "SELECT ISNULL(SUM([Money]),0) FROM [PayList] WHERE Status IN(0,1) AND [Uid]=" + uid.ToString();
            return decimal.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, commandText));
        }

        public static PayListInfo ReaderBind(SqlDataReader dataReader)
        {
            PayListInfo info = new PayListInfo();
            object obj2 = dataReader["ID"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ID = (int) obj2;
            }
            obj2 = dataReader["Uid"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Uid = (int) obj2;
            }
            obj2 = dataReader["Money"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Money = (decimal) obj2;
            }
            obj2 = dataReader["Status"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Status = (int) obj2;
            }
            obj2 = dataReader["AddTime"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.AddTime = (DateTime) obj2;
            }
            obj2 = dataReader["PayTime"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.PayTime = (DateTime) obj2;
            }
            obj2 = dataReader["Tax"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Tax = (decimal) obj2;
            }
            obj2 = dataReader["Charges"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Charges = (decimal) obj2;
            }
            return info;
        }

        public static bool Update(PayListInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                DataBase.MakeInParam("@ID", SqlDbType.Int, 10, model.ID), 
                DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, model.Uid), 
                DataBase.MakeInParam("@Money", SqlDbType.Money, 8, model.Money), 
                DataBase.MakeInParam("@Status", SqlDbType.Int, 10, model.Status), 
                DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, model.AddTime), 
                DataBase.MakeInParam("@PayTime", SqlDbType.DateTime, 8, model.PayTime), 
                DataBase.MakeInParam("@Tax", SqlDbType.Money, 8, model.Tax), 
                DataBase.MakeInParam("@Charges", SqlDbType.Money, 8, model.Charges) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "UP_PayList_Update", commandParameters) > 0);
        }
    }
}

