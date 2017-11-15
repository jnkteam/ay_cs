namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;

    public class AdsFactory
    {
        public static int Add(AdsInfo _adsinfo)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@ID", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@AdsName", SqlDbType.VarChar, 100, _adsinfo.AdsName), DataBase.MakeInParam("@Description", SqlDbType.VarChar, 200, _adsinfo.Description), DataBase.MakeInParam("@Href", SqlDbType.VarChar, 200, _adsinfo.Href), DataBase.MakeInParam("@AdsType", SqlDbType.TinyInt, 1, _adsinfo.AdsType), DataBase.MakeInParam("@Prices", SqlDbType.Money, 8, _adsinfo.Prices), DataBase.MakeInParam("@ShowStyle", SqlDbType.TinyInt, 1, _adsinfo.ShowStyle), DataBase.MakeInParam("@TargetType", SqlDbType.VarChar, 50, _adsinfo.TargetType), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, _adsinfo.AddTime), DataBase.MakeInParam("@AdvertisersId", SqlDbType.Int, 10, _adsinfo.AdvertisersId), DataBase.MakeInParam("@AdsStatus", SqlDbType.TinyInt, 1, _adsinfo.AdsStatus) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "Ads_ADD", commandParameters) == 1)
            {
                _adsinfo.ID = (int) parameter.Value;
                HttpContext.Current.Cache.Insert("adsinfo" + _adsinfo.ID.ToString(), _adsinfo);
                return _adsinfo.ID;
            }
            return 0;
        }

        public static bool DelAds(int _adsid)
        {
            if (AdsPicFactory.DeleteByAdsId(_adsid))
            {
                string commandText = "DELETE FROM [Ads] WHERE ID=@ID";
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.Int, 10, _adsid) };
                DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
                return true;
            }
            return false;
        }

        public static AdsInfo GetInfo(int id)
        {
            AdsInfo info = new AdsInfo();
            if (HttpContext.Current.Cache["adsinfo" + id.ToString()] != null)
            {
                return (AdsInfo) HttpContext.Current.Cache.Get("adsinfo" + id.ToString());
            }
            string commandText = "SELECT * FROM [Ads] WHERE [ID]=@ID";
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.Int, 10, id) };
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText, commandParameters);
            if (reader.Read())
            {
                info.ID = (int) reader["ID"];
                info.AdsName = reader["AdsName"].ToString();
                info.AdsType = (AdsTypeEnum) int.Parse(reader["AdsType"].ToString());
                info.AdsStatus = (AdsStatusEnum) int.Parse(reader["AdsStatus"].ToString());
                info.AddTime = DateTime.Parse(reader["AddTime"].ToString());
                info.AdvertisersId = (int) reader["AdvertisersId"];
                info.Description = reader["Description"].ToString();
                info.Href = reader["Href"].ToString();
                info.Prices = (decimal) reader["Prices"];
                info.ShowStyle = (ShowStyleEnum) int.Parse(reader["ShowStyle"].ToString());
                info.TargetType = reader["TargetType"].ToString();
            }
            reader.Close();
            HttpContext.Current.Cache.Insert("adsinfo" + id.ToString(), info);
            return info;
        }

        public static bool Update(AdsInfo _adsinfo)
        {
            HttpContext.Current.Cache.Remove("adsinfo" + _adsinfo.ID.ToString());
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.Int, 10, _adsinfo.ID), DataBase.MakeInParam("@AdsName", SqlDbType.VarChar, 100, _adsinfo.AdsName), DataBase.MakeInParam("@Description", SqlDbType.VarChar, 200, _adsinfo.Description), DataBase.MakeInParam("@Href", SqlDbType.VarChar, 200, _adsinfo.Href), DataBase.MakeInParam("@AdsType", SqlDbType.TinyInt, 1, _adsinfo.AdsType), DataBase.MakeInParam("@Prices", SqlDbType.Money, 8, _adsinfo.Prices), DataBase.MakeInParam("@ShowStyle", SqlDbType.TinyInt, 1, _adsinfo.ShowStyle), DataBase.MakeInParam("@TargetType", SqlDbType.VarChar, 50, _adsinfo.TargetType), DataBase.MakeInParam("@AddTime", SqlDbType.DateTime, 8, _adsinfo.AddTime), DataBase.MakeInParam("@AdvertisersId", SqlDbType.Int, 10, _adsinfo.AdvertisersId), DataBase.MakeInParam("@AdsStatus", SqlDbType.TinyInt, 1, _adsinfo.AdsStatus) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "Ads_Update", commandParameters) > 0);
        }
    }
}

