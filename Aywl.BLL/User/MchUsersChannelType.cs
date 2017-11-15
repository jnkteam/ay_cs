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
    /// 商户自定义通道操作对象。
    /// </summary>
    public class MchUsersChannelType
    {
        public static string ChannelTypeUsers_CACHEKEY = (Constant.Cache_Mark + "SYS_CHANNEL_TYPE_USER_{0}");
        internal static string SQL_TABLE = "mch_userChannelType";
        internal static string SQL_TABLE_FIELD = "id,TypeID,SupplierCode,UserId,UserIsOpen,SysIsOpen,AddTime,UpdateTime";

        #region 缓存对象操作

        /// <summary>
        /// 清除某个商户的缓存设置
        /// </summary>
        /// <param name="userId"></param>
        private static void ClearCache(int userId)
        {
            string objId = string.Format(ChannelTypeUsers_CACHEKEY, userId);
            WebCache.GetCacheService().RemoveObject(objId);
        }

        #endregion

        #region 查询对象
        
        /// <summary>
        /// 取用户通道设置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static MchUserChannelType GetCacheModel(int userId, int typeId)
        {
            DataTable list = GetCatchList(userId, false);    //true);        //2017.8.20修改成这个，不从缓存取数据

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

        public static DataTable GetCatchList(int userId, bool iscache)
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
                    //IDictionary<string, object> parameters = new Dictionary<string, object>();
                    //parameters.Add("userId", userId);
                    //SqlDependency dependency = DataBase.AddSqlDependency(objId, SQL_TABLE, SQL_TABLE_FIELD, "[userId]=@userId", parameters);
                    StringBuilder builder = new StringBuilder();
                    builder.Append("select [id],[TypeID],[SupplierCode],[UserId],[UserIsOpen],[SysIsOpen],[AddTime],[UpdateTime]");
                    builder.Append(" FROM [mch_userChannelType] ");
                    o = DataBase.ExecuteDataset(CommandType.Text, builder.ToString() + " where UserId=" + userId.ToString());
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static MchUserChannelType GetModelFromRow(DataRow dr)
        {
            MchUserChannelType modle = new MchUserChannelType();
            modle.Id = Convert.ToInt32(dr["id"]);
            modle.TypeID = Convert.ToInt32(dr["typeid"]);
            modle.SupplierCode = Convert.ToInt32(dr["suppliercode"]);
            modle.UserId = Convert.ToInt32(dr["userid"]);
            modle.UserIsOpen = dr["userisopen"].ToString() == "1" ? true : false;
            modle.SysIsOpen = dr["sysisopen"].ToString() == "1" ? true : false;
            if (!String.IsNullOrEmpty(dr["AddTime"].ToString()))
                modle.AddTime = Convert.ToDateTime(dr["AddTime"].ToString());
            if (!String.IsNullOrEmpty(dr["UpdateTime"].ToString()))
                modle.UpdateTime = Convert.ToDateTime(dr["UpdateTime"].ToString());

            return modle;
        }

        /// <summary>
        /// 根据ID获取用户通道信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MchUserChannelType GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltypeusers_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                return GetModelFromRow(set.Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 获取用户指定类型通道。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelTypeId"></param>
        /// <returns></returns>
        public static MchUserChannelType GetModel(int userId, int channelTypeId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userId", SqlDbType.Int, 10), 
                new SqlParameter("@typeId", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = channelTypeId;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltypeusers_GetbyKey", commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                return GetModelFromRow(set.Tables[0].Rows[0]);
            }
            return null;
        }

        #endregion

        #region 操作

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
        
        public static int Exists(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
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

        public static bool Setting(int userId, int isOpen)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@isOpen", SqlDbType.TinyInt), new SqlParameter("@addtime", SqlDbType.DateTime) };
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

        #endregion
    }
}

