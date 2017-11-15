namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web;

    public class WebSiteFactory
    {
        public static bool AddSite(WebSiteInfo _websiteinfo)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@SiteName", SqlDbType.VarChar, 100, _websiteinfo.SiteName), DataBase.MakeInParam("@Domain", SqlDbType.VarChar, 100, _websiteinfo.Domain), DataBase.MakeInParam("@Description", SqlDbType.VarChar, 500, _websiteinfo.Description), DataBase.MakeInParam("@SiteType", SqlDbType.Int, 10, _websiteinfo.SiteType), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, _websiteinfo.AddTime), DataBase.MakeInParam("@Status", SqlDbType.Int, 10, _websiteinfo.Status), DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, _websiteinfo.Uid) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "WebSite_Add", commandParameters) > 0);
        }

        public static List<WebSiteInfo> GetListArray(int uid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ID,SiteName,Domain,Description,SiteType,AddTime,Status,Uid ");
            builder.Append(" FROM WebSite WHERE Uid=" + uid);
            List<WebSiteInfo> list = new List<WebSiteInfo>();
            using (SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, builder.ToString()))
            {
                while (reader.Read())
                {
                    list.Add(ReaderBind(reader));
                }
            }
            return list;
        }

        public static WebSiteInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.Int, 10, id) };
            WebSiteInfo info = null;
            using (SqlDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "UP_WebSite_GetModel", commandParameters))
            {
                if (reader.Read())
                {
                    info = ReaderBind(reader);
                }
            }
            return info;
        }

        public static DataTable GetMySiteList(int uid)
        {
            string commandText = "SELECT * FROM [WebSite] WHERE [Uid]=" + uid;
            return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        public static WebSiteInfo GetWebSiteInfoByDomain(string domain)
        {
            WebSiteInfo info = new WebSiteInfo();
            if (HttpContext.Current.Cache[domain] != null)
            {
                return (WebSiteInfo) HttpContext.Current.Cache.Get(domain);
            }
            string commandText = string.Concat(new object[] { "SELECT * FROM [WebSite] WHERE Status=", 1, " AND [Domain]='", domain, "'" });
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                info.ID = (int) reader["ID"];
                info.SiteName = reader["SiteName"].ToString();
                info.Domain = reader["Domain"].ToString();
                info.Description = reader["Description"].ToString();
                info.SiteType = (int) reader["SiteType"];
                info.AddTime = DateTime.Parse(reader["AddTime"].ToString());
                info.Status = (int) reader["Status"];
                info.Uid = (int) reader["Uid"];
            }
            reader.Close();
            reader.Dispose();
            HttpContext.Current.Cache.Insert(domain, info);
            return info;
        }

        public static WebSiteInfo GetWebSiteInfoById(int id)
        {
            WebSiteInfo info = new WebSiteInfo();
            if (HttpContext.Current.Cache["websiteinfo" + id.ToString()] != null)
            {
                return (WebSiteInfo) HttpContext.Current.Cache.Get("websiteinfo" + id.ToString());
            }
            string commandText = "SELECT * FROM [WebSite] WHERE [ID]=" + id;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                info.ID = (int) reader["ID"];
                info.SiteName = reader["SiteName"].ToString();
                info.Domain = reader["Domain"].ToString();
                info.Description = reader["Description"].ToString();
                info.SiteType = (int) reader["SiteType"];
                info.AddTime = DateTime.Parse(reader["AddTime"].ToString());
                info.Status = (int) reader["Status"];
                info.Uid = (int) reader["Uid"];
            }
            reader.Close();
            HttpContext.Current.Cache.Insert("websiteinfo" + id.ToString(), info);
            return info;
        }

        public static WebSiteInfo ReaderBind(SqlDataReader dataReader)
        {
            WebSiteInfo info = new WebSiteInfo();
            object obj2 = dataReader["ID"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ID = (int) obj2;
            }
            info.SiteName = dataReader["SiteName"].ToString();
            info.Domain = dataReader["Domain"].ToString();
            info.Description = dataReader["Description"].ToString();
            obj2 = dataReader["SiteType"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.SiteType = (int) obj2;
            }
            obj2 = dataReader["AddTime"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.AddTime = (DateTime) obj2;
            }
            obj2 = dataReader["Status"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Status = (int) obj2;
            }
            obj2 = dataReader["Uid"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.Uid = (int) obj2;
            }
            return info;
        }

        public static bool UpdateWebSite(WebSiteInfo _websiteinfo)
        {
            HttpContext.Current.Cache.Remove("websiteinfo" + _websiteinfo.ID.ToString());
            HttpContext.Current.Cache.Remove(_websiteinfo.Domain);
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.Int, 10, _websiteinfo.ID), DataBase.MakeInParam("@SiteName", SqlDbType.VarChar, 100, _websiteinfo.SiteName), DataBase.MakeInParam("@Domain", SqlDbType.VarChar, 100, _websiteinfo.Domain), DataBase.MakeInParam("@Description", SqlDbType.VarChar, 500, _websiteinfo.Description), DataBase.MakeInParam("@SiteType", SqlDbType.Int, 10, _websiteinfo.SiteType), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, _websiteinfo.AddTime), DataBase.MakeInParam("@Status", SqlDbType.Int, 10, _websiteinfo.Status), DataBase.MakeInParam("@Uid", SqlDbType.Int, 10, _websiteinfo.Uid) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "WebSite_Update", commandParameters) > 0);
        }
    }
}

