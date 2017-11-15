namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class RegUserFactory : BaseFactory
    {
        public DataTable GetUserList(RegUserInfo reguserinfo, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (reguserinfo.Cid > 0)
            {
                list.Add(DataBase.MakeInParam("@Cid", SqlDbType.Int, 10, reguserinfo.Cid));
            }
            if (reguserinfo.Uid > 0)
            {
                list.Add(DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, reguserinfo.Uid));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, base.Page));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, base.PageSize));
            SqlParameter item = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(item);
            SqlParameter[] commandParameters = list.ToArray();
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure,  "GetRegUserList", commandParameters).Tables[0];
            base.Total = (int) item.Value;
            return table;
        }

        public DataTable GetUserList(RegUserInfo reguserinfo, AdsTypeEnum adstype, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (reguserinfo.Cid > 0)
            {
                list.Add(DataBase.MakeInParam("@Cid", SqlDbType.Int, 10, reguserinfo.Cid));
            }
            if (reguserinfo.Uid > 0)
            {
                list.Add(DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, reguserinfo.Uid));
            }
            list.Add(DataBase.MakeInParam("@adstype", SqlDbType.TinyInt, 1, (int) adstype));
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            list.Add(DataBase.MakeInParam("@page", SqlDbType.Int, 10, base.Page));
            list.Add(DataBase.MakeInParam("@pagesize", SqlDbType.Int, 10, base.PageSize));
            SqlParameter item = DataBase.MakeOutParam("@total", SqlDbType.Int, 10);
            list.Add(item);
            SqlParameter[] commandParameters = list.ToArray();
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "GetRegUserList", commandParameters).Tables[0];
            base.Total = (int) item.Value;
            return table;
        }
    }
}

