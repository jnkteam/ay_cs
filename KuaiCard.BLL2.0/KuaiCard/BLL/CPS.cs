namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class CPS
    {
        public static int Add(FillMoneyInfo fillmoneyinfo)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@ID", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@UserId", SqlDbType.Int, 10, fillmoneyinfo.UserId), DataBase.MakeInParam("@Money", SqlDbType.Money, 8, fillmoneyinfo.Money), DataBase.MakeInParam("@Status", SqlDbType.TinyInt, 1, (int) fillmoneyinfo.Status), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 4, fillmoneyinfo.AddTime) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "FillMoney_ADD", commandParameters) == 1)
            {
                return (int) parameter.Value;
            }
            return 0;
        }

        public static DataTable GetList(int uid, int cid, int pageindex, out int total, out double countmoney, DateTime stime, DateTime etime)
        {
            SqlParameter parameter = DataBase.MakeOutParam("total", SqlDbType.Int, 10);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney", SqlDbType.Money, 8);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, parameter2, DataBase.MakeInParam("@cid", SqlDbType.Int, 10, cid), DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, 0), DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, uid), DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime), DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime), DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex), DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, 40) };
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "CPS_GetList", commandParameters).Tables[0];
            total = (int) parameter.Value;
            countmoney = double.Parse(parameter2.Value.ToString());
            return table;
        }

        public static DataTable GetList(int uid, int cid, int status, int pageindex, out int total, out double countmoney, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter item = DataBase.MakeOutParam("total", SqlDbType.Int, 10);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney", SqlDbType.Money, 8);
            list.Add(item);
            list.Add(parameter2);
            if (uid != 0)
            {
                list.Add(DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, uid));
            }
            if (cid != 0)
            {
                list.Add(DataBase.MakeInParam("@Cid", SqlDbType.Int, 10, cid));
            }
            if (status != 0x3e7)
            {
                list.Add(DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, status));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, 40));
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "CPS_GetList", list.ToArray()).Tables[0];
            total = (int) item.Value;
            countmoney = double.Parse(parameter2.Value.ToString());
            return table;
        }

        public static DataTable GetPayList(int status, int pageindex, out int total, out double countmoney, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter item = DataBase.MakeOutParam("total", SqlDbType.Int, 10);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney", SqlDbType.Money, 8);
            if (status != 0x3e7)
            {
                list.Add(DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, status));
            }
            list.Add(item);
            list.Add(parameter2);
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, 40));
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Pay_GetList", list.ToArray()).Tables[0];
            total = (int) item.Value;
            countmoney = double.Parse(parameter2.Value.ToString());
            return table;
        }

        public static DataTable GetPayList(int uid, int status, int pageindex, out int total, out double countmoney, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            SqlParameter item = DataBase.MakeOutParam("total", SqlDbType.Int, 10);
            SqlParameter parameter2 = DataBase.MakeOutParam("@totalmoney", SqlDbType.Money, 8);
            if (status != 0x3e7)
            {
                list.Add(DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, status));
            }
            list.Add(item);
            list.Add(parameter2);
            list.Add(DataBase.MakeInParam("@uid", SqlDbType.Int, 10, uid));
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, pageindex));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, 40));
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "Pay_GetList", list.ToArray()).Tables[0];
            total = (int) item.Value;
            countmoney = double.Parse(parameter2.Value.ToString());
            return table;
        }

        public static int GetUserPayTimes(int userid)
        {
            string commandText = "SELECT ISNULL(COUNT(*),0) FROM [RegUser] WHERE UserId=" + userid;
            return int.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, commandText));
        }
    }
}

