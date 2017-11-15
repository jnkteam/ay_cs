namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using KuaiCardLib;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Web;

    public class AdsPicFactory
    {
        public static int Add(AdsPicInfo adspicinfo)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@PicId", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@AdsId", SqlDbType.Int, 10, adspicinfo.AdsId), DataBase.MakeInParam("@AdsPicPath", SqlDbType.VarChar, 100, adspicinfo.AdsPicPath), DataBase.MakeInParam("@SizeX", SqlDbType.Int, 10, adspicinfo.SizeX), DataBase.MakeInParam("@SizeY", SqlDbType.Int, 10, adspicinfo.SizeY) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "AdsPic_ADD", commandParameters) == 1)
            {
                adspicinfo.PicId = (int) parameter.Value;
                return adspicinfo.PicId;
            }
            return 0;
        }

        public static void Delete(int PicId)
        {
            string commandText = "DELETE FROM [AdsPic] WHERE PicId=@PicId";
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@PicId", SqlDbType.Int, 10, PicId) };
            DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
        }

        public static bool DeleteByAdsId(int adsid)
        {
            try
            {
                List<AdsPicInfo> adsPicInfosByAdsId = GetAdsPicInfosByAdsId(adsid);
                foreach (AdsPicInfo info in adsPicInfosByAdsId)
                {
                    DeletePic(info);
                }
                string commandText = "DELETE FROM [AdsPic] WHERE AdsId=@AdsId";
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@AdsId", SqlDbType.Int, 10, adsid) };
                DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeletePic(AdsPicInfo adspicinfo)
        {
            string path = HttpContext.Current.Server.MapPath(adspicinfo.AdsPicPath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static List<AdsPicInfo> GetAdsPicInfosByAdsId(int adsid)
        {
            List<AdsPicInfo> list = new List<AdsPicInfo>();
            string commandText = "SELECT [PicId] ,[AdsId] ,[AdsPicPath] ,[SizeX] ,[SizeY] FROM [AdsPic] WHERE [AdsId]=@AdsId";
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@AdsId", SqlDbType.Int, 10, adsid) };
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText, commandParameters);
            while (reader.Read())
            {
                AdsPicInfo item = new AdsPicInfo();
                item.AdsId = (int) reader["AdsId"];
                item.AdsPicPath = reader["AdsPicPath"].ToString();
                item.PicId = (int) reader["PicId"];
                item.SizeX = (int) reader["SizeX"];
                item.SizeY = (int) reader["SizeY"];
                list.Add(item);
            }
            reader.Close();
            return list;
        }

        public static AdsPicInfo GetInfo(int picid)
        {
            AdsPicInfo info = new AdsPicInfo();
            string commandText = "SELECT * FROM [AdsPic] WHERE PicId=@PicId";
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@PicId", SqlDbType.Int, 10, picid) };
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText, commandParameters);
            if (reader.Read())
            {
                info.AdsId = (int) reader["AdsId"];
                info.AdsPicPath = reader["AdsPicPath"].ToString();
                info.PicId = (int) reader["PicId"];
                info.SizeX = (int) reader["SizeX"];
                info.SizeY = (int) reader["SizeY"];
            }
            reader.Close();
            return info;
        }

        public static AdsPicInfo GetInfoADID(int picid)
        {
            AdsPicInfo info = new AdsPicInfo();
            string commandText = "SELECT * FROM [AdsPic] WHERE AdsId=@AdsId";
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@AdsId", SqlDbType.Int, 10, picid) };
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText, commandParameters);
            if (reader.Read())
            {
                info.AdsId = (int) reader["AdsId"];
                info.AdsPicPath = reader["AdsPicPath"].ToString();
                info.PicId = (int) reader["PicId"];
                info.SizeX = (int) reader["SizeX"];
                info.SizeY = (int) reader["SizeY"];
            }
            reader.Close();
            return info;
        }

        public static void SaveAdsPic(int adsid, string filekey)
        {
            List<AdsPicInfo> list = SaveRequestFiles(filekey);
            foreach (AdsPicInfo info in list)
            {
                info.AdsId = adsid;
                Add(info);
            }
        }

        public static List<AdsPicInfo> SaveRequestFiles(string filekey)
        {
            List<AdsPicInfo> list = new List<AdsPicInfo>();
            Random random = new Random((int) DateTime.Now.Ticks);
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (!HttpContext.Current.Request.Files[i].FileName.Equals("") && HttpContext.Current.Request.Files.AllKeys[i].Equals(filekey))
                {
                    string str = Path.GetExtension(Path.GetFileName(HttpContext.Current.Request.Files[i].FileName)).ToLower();
                    string str2 = HttpContext.Current.Request.Files[i].ContentType.ToLower();
                    string str3 = "";
                    string mapPath = Utility.GetMapPath("/");
                    StringBuilder builder = new StringBuilder("");
                    builder.Append("upload");
                    builder.Append(Path.DirectorySeparatorChar);
                    builder.Append(DateTime.Now.ToString("yyyy"));
                    builder.Append("-");
                    builder.Append(DateTime.Now.ToString("MM"));
                    builder.Append("-");
                    builder.Append(DateTime.Now.ToString("dd"));
                    builder.Append(Path.DirectorySeparatorChar);
                    if (!Directory.Exists(mapPath + builder.ToString()))
                    {
                        Utility.CreateDir(mapPath + builder.ToString());
                    }
                    string[] strArray = new string[] { builder.ToString(), (Environment.TickCount & 0x7fffffff).ToString(), i.ToString(), random.Next(0x3e8, 0x270f).ToString(), str };
                    str3 = string.Concat(strArray);
                    try
                    {
                        HttpContext.Current.Request.Files[i].SaveAs(mapPath + str3);
                    }
                    catch
                    {
                        if (!Utility.FileExists(mapPath + str3))
                        {
                            HttpContext.Current.Request.Files[i].SaveAs(mapPath + str3);
                        }
                    }
                    AdsPicInfo item = new AdsPicInfo();
                    item.SizeX = int.Parse(HttpContext.Current.Request.Form["SizeXBox"].Split(new char[] { ',' })[i]);
                    item.SizeY = int.Parse(HttpContext.Current.Request.Form["SizeYBox"].Split(new char[] { ',' })[i]);
                    item.AdsPicPath = @"\" + str3;
                    list.Add(item);
                }
            }
            return list;
        }

        public static bool Update(AdsPicInfo adspicinfo)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@PicId", SqlDbType.Int, 10, adspicinfo.PicId), DataBase.MakeInParam("@AdsId", SqlDbType.Int, 10, adspicinfo.AdsId), DataBase.MakeInParam("@AdsPicPath", SqlDbType.VarChar, 100, adspicinfo.AdsPicPath), DataBase.MakeInParam("@SizeX", SqlDbType.Int, 10, adspicinfo.SizeX), DataBase.MakeInParam("@SizeY", SqlDbType.Int, 10, adspicinfo.SizeY) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "Ads_Update", commandParameters) > 0);
        }
    }
}

