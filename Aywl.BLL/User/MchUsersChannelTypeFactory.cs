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
    using OriginalStudio.Lib.Utils;

    /// <summary>
    /// 商户自定义通道操作对象。
    /// </summary>
    public class MchUsersChannelTypeFactory
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

        /// <summary>
        /// 修改商户通道设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns>增加成功>0；失败=0</returns>
        public static int Add(MchUserChannelType model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@typeId", SqlDbType.Int, 10),
                    new SqlParameter("@userId", SqlDbType.Int, 10),
                    new SqlParameter("@userIsOpen", SqlDbType.Bit, 1),
                    new SqlParameter("@sysIsOpen", SqlDbType.Bit, 1)
                };
                commandParameters[0].Value = model.TypeID;
                commandParameters[1].Value = model.UserId;
                commandParameters[2].Value = model.UserIsOpen;
                commandParameters[3].Value = model.SysIsOpen;
                String R = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_mch_user_channeltype_add", commandParameters).ToString();

                return Utils.StrToInt(R, 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 设置商户通道类型供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddSupp(MchUserChannelType model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@typeid", SqlDbType.Int, 10),
                    new SqlParameter("@suppliercode", SqlDbType.Int, 10),
                    new SqlParameter("@userid", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = model.TypeID;
                commandParameters[1].Value = model.SupplierCode;
                commandParameters[2].Value = model.UserId;
                String R = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_mch_user_channeltype_editsuppid", commandParameters).ToString();
                return Utils.StrToInt(R, 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }
        
        /// <summary>
        /// 商户是否独立的供应商通道。
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>存在=1；不存在=0</returns>
        public static int Exists(int userID)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@userId", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = userID;
                string R = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_mch_user_channeltype_exists", commandParameters).ToString();

                return Utils.StrToInt(R, 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 批量设置商户通道
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isOpen"></param>
        /// <returns>成功True；不成功False</returns>
        public static bool BatchSettingSupp(int userId, int sysIsOpen)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@userId", SqlDbType.Int, 10),
                    new SqlParameter("@sysIsOpen", SqlDbType.TinyInt)
                };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = sysIsOpen;
                return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_user_channeltype_setting", commandParameters) > 0;
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

