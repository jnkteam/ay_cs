namespace OriginalStudio.BLL.Channel
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.BLL.Sys;

    /// <summary>
    /// 旧对象，新版作废
    /// </summary>
    public class ChannelTypeUsers
    {
        public static string ChannelTypeUsers_CACHEKEY = (Constant.Cache_Mark + "CHANNEL_TYPE_USER_{0}");
        internal static string SQL_TABLE = "channeltypeusers";
        internal static string SQL_TABLE_FIELD = "[id],[typeId],[userId],[userIsOpen],[suppid],[sysIsOpen],[updateTime]";

        public static int Add(ChannelTypeUserInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@typeId", SqlDbType.Int, 10), new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@userIsOpen", SqlDbType.Bit, 1), new SqlParameter("@sysIsOpen", SqlDbType.Bit, 1), new SqlParameter("@addTime", SqlDbType.DateTime), new SqlParameter("@updateTime", SqlDbType.DateTime) };
                commandParameters[0].Value = model.typeId;
                commandParameters[1].Value = model.userId;
                commandParameters[2].Value = model.userIsOpen;
                commandParameters[3].Value = model.sysIsOpen;
                commandParameters[4].Value = model.addTime;
                commandParameters[5].Value = model.updateTime;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_channeltypeusers_add", commandParameters);
                if (obj2 == null)
                {
                    return 0;
                }
                ClearCache(model.userId);
                return Convert.ToInt32(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static int AddSupp(ChannelTypeUserInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@typeId", SqlDbType.Int, 10), new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@suppid", SqlDbType.Int, 10), new SqlParameter("@addTime", SqlDbType.DateTime), new SqlParameter("@updateTime", SqlDbType.DateTime) };
                commandParameters[0].Value = model.typeId;
                commandParameters[1].Value = model.userId;
                commandParameters[2].Value = model.suppid;
                commandParameters[3].Value = model.addTime;
                commandParameters[4].Value = model.updateTime;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_channeltypeusers_addsuppid", commandParameters);
                if (obj2 == null)
                {
                    return 0;
                }
                ClearCache(model.userId);
                return Convert.ToInt32(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private static void ClearCache(int userId)
        {
            string objId = string.Format(ChannelTypeUsers_CACHEKEY, userId);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        public static int Exists(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@userId", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = userid;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_channeltypeusers_exists", commandParameters);
                if ((obj2 == null) || (obj2 == DBNull.Value))
                {
                    return 0;
                }
                return Convert.ToInt32(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static ChannelTypeUserInfo GetCacheModel(int userId, int typeId)
        {
            DataTable list = GetList(userId, false);    //true);        //2017.8.20修改成这个，不从缓存取数据
            if ((list != null) && (list.Rows.Count > 0))
            {
                DataRow[] rowArray = list.Select("typeId=" + typeId.ToString());
                if ((rowArray != null) && (rowArray.Length > 0))
                {
                    return GetModelFromRow(rowArray[0]);
                }
            }
            return null;
        }

        public static DataTable GetList(int userId, bool iscache)
        {
            try
            {
                string objId = string.Format(ChannelTypeUsers_CACHEKEY, userId);
                DataSet o = new DataSet();
                if (iscache)
                {
                    o = (DataSet) WebCache.GetCacheService().RetrieveObject(objId);
                }
                if ((o == null) || !iscache)
                {
                    IDictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("userId", userId);
                    SqlDependency dependency = DataBase.AddSqlDependency(objId, SQL_TABLE, SQL_TABLE_FIELD, "[userId]=@userId", parameters);
                    StringBuilder builder = new StringBuilder();
                    builder.Append("select [id],[suppid],[typeId],[userId],[userIsOpen],[sysIsOpen],addTime,updateTime ");
                    builder.Append(" FROM [ChannelTypeUsers] ");
                    o = DataBase.ExecuteDataset(CommandType.Text, builder.ToString() + " where userId=" + userId.ToString());
                    if (iscache)
                    {
                        WebCache.GetCacheService().AddObject(objId, o);
                    }
                }
                return o.Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static ChannelTypeUserInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channeltypeusers_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                return GetModelFromRow(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public static ChannelTypeUserInfo GetModel(int userId, int typeId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@typeId", SqlDbType.Int, 10) };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = typeId;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channeltypeusers_GetbyKey", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                return GetModelFromRow(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public static ChannelTypeUserInfo GetModelFromRow(DataRow dr)
        {
            ChannelTypeUserInfo info = new ChannelTypeUserInfo();
            if (dr["id"].ToString() != "")
            {
                info.id = int.Parse(dr["id"].ToString());
            }
            if (dr["typeId"].ToString() != "")
            {
                info.typeId = int.Parse(dr["typeId"].ToString());
            }
            if (dr["userId"].ToString() != "")
            {
                info.userId = int.Parse(dr["userId"].ToString());
            }
            if (dr["userIsOpen"].ToString() != "")
            {
                if ((dr["userIsOpen"].ToString() == "1") || (dr["userIsOpen"].ToString().ToLower() == "true"))
                {
                    info.userIsOpen = true;
                }
                else
                {
                    info.userIsOpen = false;
                }
            }
            else
            {
                info.userIsOpen = null;
            }
            if (dr["sysIsOpen"].ToString() != "")
            {
                if ((dr["sysIsOpen"].ToString() == "1") || (dr["sysIsOpen"].ToString().ToLower() == "true"))
                {
                    info.sysIsOpen = true;
                }
                else
                {
                    info.sysIsOpen = false;
                }
            }
            else
            {
                info.sysIsOpen = null;
            }
            if (dr.Table.Columns.Contains("addTime") && (dr["addTime"].ToString() != ""))
            {
                info.addTime = new DateTime?(DateTime.Parse(dr["addTime"].ToString()));
            }
            if (dr.Table.Columns.Contains("updateTime") && (dr["updateTime"].ToString() != ""))
            {
                info.updateTime = new DateTime?(DateTime.Parse(dr["updateTime"].ToString()));
            }
            info.suppid = null;
            if (dr["suppid"].ToString() != "")
            {
                info.suppid = new int?(int.Parse(dr["suppid"].ToString()));
            }
            return info;
        }

        public static bool Setting(int userId, int isOpen)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@userId", SqlDbType.Int, 10),
                    new SqlParameter("@isOpen", SqlDbType.TinyInt),
                    new SqlParameter("@addtime", SqlDbType.DateTime)
                };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = isOpen;
                commandParameters[2].Value = DateTime.Now;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channeltypeusers_Setting", commandParameters) > 0;
                if (flag)
                {
                    ClearCache(userId);
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

