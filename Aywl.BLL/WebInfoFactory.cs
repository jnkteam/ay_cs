namespace OriginalStudio.BLL
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web;
    using OriginalStudio.BLL.Sys;

    public class WebInfoFactory
    {
        internal static string SQL_TABLE = "webinfo";
        internal static string SQL_TABLE_FIELD = "[id],[templateID],[name],[domain],[jsqq],[kfqq],[phone],[footer],[code],[logopath],[payurl]";
        public static string WEBINFO_DOMAIN_CACHEKEY = (Constant.Cache_Mark + "WEBINFOCONFIG_{0}");

        public static void Add(WebInfo model)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into webinfo(");
                builder.Append("[id],[templateID],[name],[domain],[jsqq],[kfqq],[phone],[footer],[code],[logopath],[payurl])");
                builder.Append(" values (");
                builder.Append("@id,@templateID,@name,@domain,@jsqq,@kfqq,@phone,@footer,@code,@logopath,@payurl)");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@templateID", SqlDbType.VarChar, 50), new SqlParameter("@name", SqlDbType.VarChar, 50), new SqlParameter("@domain", SqlDbType.VarChar, 50), new SqlParameter("@jsqq", SqlDbType.VarChar, 50), new SqlParameter("@kfqq", SqlDbType.VarChar, 50), new SqlParameter("@phone", SqlDbType.VarChar, 50), new SqlParameter("@footer", SqlDbType.VarChar, 50), new SqlParameter("@code", SqlDbType.VarChar, 50), new SqlParameter("@logopath", SqlDbType.VarChar, 50), new SqlParameter("@payurl", SqlDbType.VarChar, 50) };
                commandParameters[0].Value = model.ID;
                commandParameters[1].Value = model.TemplateId;
                commandParameters[2].Value = model.Name;
                commandParameters[3].Value = model.Domain;
                commandParameters[4].Value = model.Jsqq;
                commandParameters[5].Value = model.Kfqq;
                commandParameters[6].Value = model.Phone;
                commandParameters[7].Value = model.Footer;
                commandParameters[8].Value = model.Code;
                commandParameters[9].Value = model.LogoPath;
                commandParameters[9].Value = model.PayUrl;
                DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
        }

        private static void ClearCache(string domain)
        {
            string objId = string.Format(WEBINFO_DOMAIN_CACHEKEY, domain);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        public static string GetAgent_Payrate_Setconfig()
        {
            string commandText = "select top 1 isnull(agentpayratesetconfig,'') from webinfo where id = 1";
            return Convert.ToString(DataBase.ExecuteScalar(CommandType.Text, commandText));
        }

        public static WebInfo GetCacheWebInfoByDomain(string domain)
        {
            string objId = string.Format(WEBINFO_DOMAIN_CACHEKEY, domain);
            WebInfo o = new WebInfo();
            o = (WebInfo) WebCache.GetCacheService().RetrieveObject(objId);
            if (o == null)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("domain", domain);
                SqlDependency dependency = DataBase.AddSqlDependency(objId, SQL_TABLE, SQL_TABLE_FIELD, "[domain]=@domain", parameters);
                o = GetWebInfoByDomain(domain);
                if (o == null)
                {
                    return null;
                }
                WebCache.GetCacheService().AddObject(objId, o);
            }
            return o;
        }

        public static DataTable GetList(string where)
        {
            try
            {
                string commandText = "SELECT [id]\r\n      ,[templateID]\r\n      ,[name]\r\n      ,[domain]\r\n      ,[jsqq]\r\n      ,[kfqq]\r\n      ,[phone]\r\n      ,[footer]\r\n      ,[code]\r\n      ,[logopath]\r\n      ,[payurl] ,[apibankname]\r\n      ,[apibankversion]\r\n      ,[apicardname]\r\n      ,[apicardversion]\r\n  FROM [webinfo] ";
                return DataBase.ExecuteDataset(CommandType.Text, commandText).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        private static WebInfo GetObjectFromDR(SqlDataReader dr)
        {
            if (dr == null)
            {
                return null;
            }
            WebInfo info = new WebInfo();
            if (dr.Read())
            {
                info.ID = (int) dr["id"];
                info.TemplateId = dr["templateID"].ToString();
                info.Name = dr["name"].ToString();
                info.Domain = dr["domain"].ToString();
                info.Jsqq = dr["jsqq"].ToString();
                info.Kfqq = dr["kfqq"].ToString();
                info.Phone = dr["phone"].ToString();
                info.Footer = dr["footer"].ToString();
                info.Code = dr["code"].ToString();
                info.LogoPath = dr["logopath"].ToString();
                info.PayUrl = dr["payurl"].ToString();
                info.apibankname = dr["apibankname"].ToString();
                info.apibankversion = dr["apibankversion"].ToString();
                info.apicardname = dr["apicardname"].ToString();
                info.apicardversion = dr["apicardversion"].ToString();
            }
            dr.Close();
            return info;
        }

        public static WebInfo GetWebInfoByDomain(string domain)
        {
            try
            {
                string commandText = "DECLARE @TempID int\r\nSELECT @TempID = [id] FROM [webinfo] WHERE [domain]=@domain\r\nIF(@TempID IS NULL)\r\nSELECT @TempID = 1\r\n\r\nSELECT [id]\r\n      ,[templateID]\r\n      ,[name]\r\n      ,[domain]\r\n      ,[jsqq]\r\n      ,[kfqq]\r\n      ,[phone]\r\n      ,[footer]\r\n      ,[code]\r\n      ,[logopath]\r\n      ,[payurl] ,[apibankname]\r\n      ,[apibankversion]\r\n      ,[apicardname]\r\n      ,[apicardversion]\r\nFROM [dbo].[webinfo] WHERE [id] = @TempID ";
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@domain", SqlDbType.VarChar, 50, domain) };
                return GetObjectFromDR(DataBase.ExecuteReader(CommandType.Text, commandText, commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static WebInfo GetWebInfoById(int id)
        {
            try
            {
                WebInfo info = new WebInfo();
                string commandText = "SELECT [id]\r\n      ,[templateID]\r\n      ,[name]\r\n      ,[domain]\r\n      ,[jsqq]\r\n      ,[kfqq]\r\n      ,[phone]\r\n      ,[footer]\r\n      ,[code]\r\n      ,[logopath]\r\n      ,[payurl]\r\n      ,[apibankname]\r\n      ,[apibankversion]\r\n      ,[apicardname]\r\n      ,[apicardversion]\r\n  FROM [webinfo] WHERE [id]=@id";
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, id) };
                return GetObjectFromDR(DataBase.ExecuteReader(CommandType.Text, commandText, commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static bool SetAgent_Payrate_Setconfig(string config)
        {
            string commandText = "update webinfo set agentpayratesetconfig=@agentpayratesetconfig where id = 1";
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@agentpayratesetconfig", SqlDbType.VarChar, 0xfa0, config) };
            return (DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters) > 0);
        }

        public static bool Update(WebInfo _webinfo)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, _webinfo.ID), DataBase.MakeInParam("@templateID", SqlDbType.VarChar, 20, _webinfo.TemplateId), DataBase.MakeInParam("@name", SqlDbType.VarChar, 100, _webinfo.Name), DataBase.MakeInParam("@domain", SqlDbType.VarChar, 500, _webinfo.Domain), DataBase.MakeInParam("@jsqq", SqlDbType.VarChar, 300, _webinfo.Jsqq), DataBase.MakeInParam("@kfqq", SqlDbType.VarChar, 300, _webinfo.Kfqq), DataBase.MakeInParam("@phone", SqlDbType.VarChar, 50, _webinfo.Phone), DataBase.MakeInParam("@footer", SqlDbType.VarChar, 500, _webinfo.Footer), DataBase.MakeInParam("@code", SqlDbType.VarChar, 500, _webinfo.Code), DataBase.MakeInParam("@logopath", SqlDbType.VarChar, 500, _webinfo.LogoPath), DataBase.MakeInParam("@payurl", SqlDbType.VarChar, 500, _webinfo.PayUrl), DataBase.MakeInParam("@apibankname", SqlDbType.VarChar, 100, _webinfo.apibankname), DataBase.MakeInParam("@apibankversion", SqlDbType.VarChar, 20, _webinfo.apibankversion), DataBase.MakeInParam("@apicardname", SqlDbType.VarChar, 100, _webinfo.apicardname), DataBase.MakeInParam("@apicardversion", SqlDbType.VarChar, 20, _webinfo.apicardversion) };
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_webinfo_Update", commandParameters) > 0;
                if (flag)
                {
                    ClearCache(HttpContext.Current.Request.Url.Host);
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static WebInfo CurrentWebInfo
        {
            get
            {
                return GetCacheWebInfoByDomain(HttpContext.Current.Request.Url.Host);
            }
        }
    }
}

