namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model.User;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class PromotionUserFactory
    {
        public static bool Delete(int RegId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@RegId", SqlDbType.Int, 10, RegId) };
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_promotionUser_delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static DataTable Get_Pro_MoneyList(int pid, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (pid > 0)
            {
                list.Add(DataBase.MakeInParam("@pid", SqlDbType.Int, 10, pid));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Get_Pro_MoneyList", list.ToArray());
            DataTable table = null;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
            }
            return table;
        }

        public static DataTable Get_Pro_PayList(int gmid, int pid, int _paytype, int _sid, int pageindex, int pagesize, DateTime stime, DateTime etime, int status, out int total, out double money, out double money2, out double money3)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (gmid > 0)
            {
                list.Add(DataBase.MakeInParam("@gmid", SqlDbType.Int, 10, gmid));
            }
            if (_paytype > 0)
            {
                list.Add(DataBase.MakeInParam("@paytype", SqlDbType.Int, 10, _paytype));
            }
            if (_sid > 0)
            {
                list.Add(DataBase.MakeInParam("@sid", SqlDbType.Int, 10, _sid));
            }
            if (pid > 0)
            {
                list.Add(DataBase.MakeInParam("@pid", SqlDbType.Int, 10, pid));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@Status", SqlDbType.TinyInt, 1, status));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, pagesize));
            SqlParameter item = DataBase.MakeOutParam("@totalmoney", SqlDbType.Decimal, 8);
            list.Add(item);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney2", SqlDbType.Decimal, 8);
            list.Add(parameter2);
            SqlParameter parameter3 = DataBase.MakeOutParam("@totalmoney3", SqlDbType.Decimal, 8);
            list.Add(parameter3);
            SqlParameter parameter4 = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(parameter4);
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Get_Pro_PayList", list.ToArray());
            DataTable table = null;
            money = 0.0;
            total = 0;
            money2 = 0.0;
            money3 = 0.0;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
                money = double.Parse(item.Value.ToString());
                money2 = double.Parse(parameter2.Value.ToString());
                money3 = double.Parse(parameter3.Value.ToString());
                total = int.Parse(parameter4.Value.ToString());
            }
            return table;
        }

        public static DataTable Get_Pro_PayList(int gmid, int pid, int _paytype, int _sid, int pageindex, int pagesize, DateTime stime, DateTime etime, int status, out int total, out double money, out double money2, out double money3, out double money4)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (gmid > 0)
            {
                list.Add(DataBase.MakeInParam("@gmid", SqlDbType.Int, 10, gmid));
            }
            if (_paytype > 0)
            {
                list.Add(DataBase.MakeInParam("@paytype", SqlDbType.Int, 10, _paytype));
            }
            if (_sid > 0)
            {
                list.Add(DataBase.MakeInParam("@sid", SqlDbType.Int, 10, _sid));
            }
            if (pid > 0)
            {
                list.Add(DataBase.MakeInParam("@pid", SqlDbType.Int, 10, pid));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@Status", SqlDbType.TinyInt, 1, status));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, pagesize));
            SqlParameter item = DataBase.MakeOutParam("@totalmoney", SqlDbType.Decimal, 8);
            list.Add(item);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney2", SqlDbType.Decimal, 8);
            list.Add(parameter2);
            SqlParameter parameter3 = DataBase.MakeOutParam("@totalmoney3", SqlDbType.Decimal, 8);
            list.Add(parameter3);
            SqlParameter parameter4 = DataBase.MakeOutParam("@totalmoney4", SqlDbType.Decimal, 8);
            list.Add(parameter4);
            SqlParameter parameter5 = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(parameter5);
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Get_Pro_PayList_pingtai", list.ToArray());
            DataTable table = null;
            money = 0.0;
            total = 0;
            money2 = 0.0;
            money3 = 0.0;
            money4 = 0.0;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
                money = double.Parse(item.Value.ToString());
                money2 = double.Parse(parameter2.Value.ToString());
                money3 = double.Parse(parameter3.Value.ToString());
                money4 = double.Parse(parameter4.Value.ToString());
                total = int.Parse(parameter5.Value.ToString());
            }
            return table;
        }

        public static DataTable Get_Pro_PayList(int gmid, int pid, int _paytype, int _sid, string username, ulong okxrorderid, string outorderid, int pageindex, int pagesize, DateTime stime, DateTime etime, int status, out int total, out double money, out double money2, out double money3, out double money4)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (gmid > 0)
            {
                list.Add(DataBase.MakeInParam("@gmid", SqlDbType.Int, 10, gmid));
            }
            if (_paytype > 0)
            {
                list.Add(DataBase.MakeInParam("@paytype", SqlDbType.Int, 10, _paytype));
            }
            if (_sid > 0)
            {
                list.Add(DataBase.MakeInParam("@sid", SqlDbType.Int, 10, _sid));
            }
            if (pid > 0)
            {
                list.Add(DataBase.MakeInParam("@pid", SqlDbType.Int, 10, pid));
            }
            if (username != "")
            {
                list.Add(DataBase.MakeInParam("@UserName", SqlDbType.VarChar, 50, username));
            }
            if (okxrorderid != 0L)
            {
                list.Add(DataBase.MakeInParam("@OkxrOrderId", SqlDbType.BigInt, 8, okxrorderid));
            }
            if (outorderid != "")
            {
                list.Add(DataBase.MakeInParam("@OutOrderId", SqlDbType.VarChar, 50, outorderid));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@Status", SqlDbType.TinyInt, 1, status));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, pagesize));
            SqlParameter item = DataBase.MakeOutParam("@totalmoney", SqlDbType.Decimal, 8);
            list.Add(item);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney2", SqlDbType.Decimal, 8);
            list.Add(parameter2);
            SqlParameter parameter3 = DataBase.MakeOutParam("@totalmoney3", SqlDbType.Decimal, 8);
            list.Add(parameter3);
            SqlParameter parameter4 = DataBase.MakeOutParam("@totalmoney4", SqlDbType.Decimal, 8);
            list.Add(parameter4);
            SqlParameter parameter5 = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(parameter5);
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Get_Pro_PayList_pingtai", list.ToArray());
            DataTable table = null;
            money = 0.0;
            total = 0;
            money2 = 0.0;
            money3 = 0.0;
            money4 = 0.0;
            if (set.Tables.Count != 0)
            {
                table = set.Tables[0];
                money = double.Parse(item.Value.ToString());
                money2 = double.Parse(parameter2.Value.ToString());
                money3 = double.Parse(parameter3.Value.ToString());
                money4 = double.Parse(parameter4.Value.ToString());
                total = int.Parse(parameter5.Value.ToString());
            }
            return table;
        }

        public static DataTable Get_Pro_UserList(int userId, string username, int pid, int Status, int pageindex, out int total)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter item = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(item);
            if (userId > 0)
            {
                list.Add(DataBase.MakeInParam("@uid", SqlDbType.Int, 10, userId));
            }
            if (pid > 0)
            {
                list.Add(DataBase.MakeInParam("@pid", SqlDbType.Int, 10, pid));
            }
            if (username != "")
            {
                list.Add(DataBase.MakeInParam("@username", SqlDbType.VarChar, 50, username));
            }
            if (Status != 0x3e7)
            {
                list.Add(DataBase.MakeInParam("@status", SqlDbType.Int, 10, Status));
            }
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, 20));
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Get_Pro_UserList", list.ToArray()).Tables[0];
            total = (int) item.Value;
            return table;
        }

        public static PromotionUserInfo GetModel(int regId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  top 1 PromId,RegId,PID,Prices from PromotionUser ");
            builder.Append(" where RegId=@RegId ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@RegId", SqlDbType.Int, 10) };
            commandParameters[0].Value = regId;
            PromotionUserInfo info = new PromotionUserInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                if (set.Tables[0].Rows[0]["PromId"].ToString() != "")
                {
                    info.PromId = int.Parse(set.Tables[0].Rows[0]["PromId"].ToString());
                }
                if (set.Tables[0].Rows[0]["RegId"].ToString() != "")
                {
                    info.RegId = int.Parse(set.Tables[0].Rows[0]["RegId"].ToString());
                }
                if (set.Tables[0].Rows[0]["PID"].ToString() != "")
                {
                    info.PID = int.Parse(set.Tables[0].Rows[0]["PID"].ToString());
                }
                if (set.Tables[0].Rows[0]["Prices"].ToString() != "")
                {
                    info.Prices = decimal.Parse(set.Tables[0].Rows[0]["Prices"].ToString());
                }
                return info;
            }
            return info;
        }

        public static DataTable getparouserlistall(int pid)
        {
            string commandText = "select * from PromotionUser where PID = " + pid;
            return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public static int GetUserNum(int userId)
        {
            try
            {
                string commandText = "select count(0) from PromotionUser(nolock) where PID=@PID";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@PID", SqlDbType.Int, 10) };
                commandParameters[0].Value = userId;
                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.Text, commandText, commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static int Insert(PromotionUserInfo promUser)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@RegId", SqlDbType.Int, 10, promUser.RegId), 
                    DataBase.MakeInParam("@PID", SqlDbType.Int, 10, promUser.PID), 
                    DataBase.MakeInParam("@Prices", SqlDbType.Money, 8, promUser.Prices), 
                    DataBase.MakeInParam("@PTime", SqlDbType.DateTime, 8, promUser.PromTime), 
                    DataBase.MakeInParam("@PStatus", SqlDbType.Int, 10, promUser.PromStatus) 
                };
                int num = Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_promotionUser_insert", commandParameters));
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }
    }
}

