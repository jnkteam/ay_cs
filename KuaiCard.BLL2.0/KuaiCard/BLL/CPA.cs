namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.BLL.User;
    using KuaiCard.Model;
    using KuaiCard.Model.User;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class CPA
    {
        public static int Add(RegUserInfo reguserinfo)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@ID", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, reguserinfo.Uid), DataBase.MakeInParam("@Cid", SqlDbType.Int, 10, reguserinfo.Cid), DataBase.MakeInParam("@UserId", SqlDbType.Int, 10, reguserinfo.UserId), DataBase.MakeInParam("@AdsType", SqlDbType.TinyInt, 1, (int) reguserinfo.AdsType), DataBase.MakeInParam("@Prices", SqlDbType.Decimal, 8, reguserinfo.Prices), DataBase.MakeInParam("@Status", SqlDbType.TinyInt, 1, (int) reguserinfo.Status), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, reguserinfo.AddTime) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "RegUser_ADD", commandParameters) == 1)
            {
                return (int) parameter.Value;
            }
            return 0;
        }

        public static DataTable CountByUid(int uid, int cid, int status, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (cid > 0)
            {
                list.Add(DataBase.MakeInParam("@cid", SqlDbType.Int, 10, cid));
            }
            if (status != 0x3e7)
            {
                list.Add(DataBase.MakeInParam("@Status", SqlDbType.Int, 10, status));
            }
            list.Add(DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, uid));
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            SqlParameter[] commandParameters = list.ToArray();
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "CPA_CountByUid", commandParameters).Tables[0];
        }

        public static int GetCountByChannel(int cid)
        {
            string commandText = string.Concat(new object[] { "SELECT ISNULL(COUNT(*),0) FROM RegUser WHERE Status<>", 1, " AND Cid=", cid });
            int num = 0;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                num = (int) reader[0];
            }
            reader.Close();
            return num;
        }

        public static DataTable GetCountByHour(int uid, DateTime date)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, uid), DataBase.MakeInParam("@date", SqlDbType.DateTime, 8, date) };
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "CPA_CountByHour", commandParameters).Tables[0];
        }

        public static DataTable GetDateList(int uid)
        {
            string commandText = string.Concat(new object[] { "SELECT Convert(varchar(10),[AddTime],120) AS AddTime FROM [RegUser] WHERE AdsType=", 0, " AND [Status]=", 0, " AND Uid=", uid.ToString(), " GROUP BY Convert(varchar(10),[AddTime],120) ORDER BY Convert(varchar(10),[AddTime],120) DESC" });
            return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public static DataTable GetList(int uid)
        {
            string commandText = string.Concat(new object[] { "SELECT * FROM [RegUser] WHERE AdsType=", 0, " AND [Status]=", 0, " AND Uid=", uid, " ORDER BY ID DESC" });
            return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public static DataTable GetList(int uid, DateTime date)
        {
            string commandText = string.Concat(new object[] { "SELECT * FROM [RegUser] WHERE AdsType=", 0, " AND [Status]=", 0, " AND Uid=", uid, " AND convert(char(10),[AddTime],120)=convert(char(10),'", date.ToString("yyyy-MM-dd"), "',120) ORDER BY ID DESC" });
            return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public static DataTable GetList(int uid, int status, DateTime stime, DateTime etime)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            if (uid != 0)
            {
                list.Add(DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, uid));
            }
            if (status != 0x3e7)
            {
                list.Add(DataBase.MakeInParam("@status", SqlDbType.TinyInt, 1, status));
            }
            list.Add(DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime));
            list.Add(DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime));
            SqlParameter[] commandParameters = list.ToArray();
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "CPA_GetList", commandParameters).Tables[0];
        }

        public static RegUserInfo GetRegUserInfo(int userid)
        {
            RegUserInfo info = new RegUserInfo();
            string commandText = "SELECT TOP 1 * FROM [RegUser] WHERE [UserId]=" + userid;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                info.AddTime = DateTime.Parse(reader["AddTime"].ToString());
                info.AdsType = (AdsTypeEnum) int.Parse(reader["AdsType"].ToString());
                info.Cid = (int) reader["Cid"];
                info.ID = (int) reader["ID"];
                info.Prices = decimal.Parse(reader["Prices"].ToString());
                info.Status = (CPAInfo.RegUserStatusEnum) int.Parse(reader["Status"].ToString());
                info.Uid = (int) reader["Uid"];
                info.UserId = (int) reader["UserId"];
            }
            reader.Close();
            return info;
        }

        public static UserInfo GetUserInfoByUserId(int userid)
        {
            UserInfo model = new UserInfo();
            string commandText = "SELECT TOP 1 Uid FROM RegUser WHERE UserId=" + userid;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                int uid = (int) reader["Uid"];
                model = UserFactory.GetModel(uid);
            }
            reader.Close();
            return model;
        }

        public static DataTable GetUserList(DateTime stime, DateTime etime)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime), DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime) };
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "GetRegUserList", commandParameters).Tables[0];
        }

        public static DataTable GetUserList(AdsTypeEnum adstype, DateTime stime, DateTime etime)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@adstype", SqlDbType.TinyInt, 1, (int) adstype), DataBase.MakeInParam("@stime", SqlDbType.DateTime, 8, stime), DataBase.MakeInParam("@etime", SqlDbType.DateTime, 8, etime) };
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "GetRegUserList", commandParameters).Tables[0];
        }

        public static WebSiteInfo GetWebSiteInfoByUserId(int userid)
        {
            WebSiteInfo webSiteInfoById = new WebSiteInfo();
            string commandText = "SELECT TOP 1 Cid FROM RegUser WHERE UserId=" + userid;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                int id = (int) reader["Cid"];
                webSiteInfoById = WebSiteFactory.GetWebSiteInfoById(id);
            }
            reader.Close();
            return webSiteInfoById;
        }
    }
}

