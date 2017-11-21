namespace OriginalStudio.BLL.Channel
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.Lib.Utils;

    /// <summary>
    /// 通道类型操作。
    /// </summary>
    public class SysChannelType
    {
        public static string CHANNELTYPE_CACHEKEY = (Constant.Cache_Mark + "SYS_CHANNEL_TYPE");
        internal static string SQL_TABLE = "sys_channeltype";
        internal static string SQL_TABLE_FIELD = "ID,TypeID,TypeCode,TypeClassID,TypeName,IsOpen,SupplierCode,SupplierRate,AddTime,ListOrder,Release,RunMode,RunModeSet";

        #region 获取缓存对象

        /// <summary>
        /// 清除缓存。
        /// </summary>
        private static void ClearCache()
        {
            string objId = CHANNELTYPE_CACHEKEY;
            WebCache.GetCacheService().RemoveObject(objId);
            objId = SysChannel.CHANEL_CACHEKEY;
            WebCache.GetCacheService().RemoveObject(objId);
        }

        #endregion

        #region 获取对象信息

        /// <summary>
        /// 根据TypeID获取缓存对象。
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static SysChannelTypeInfo GetCacheModel(int typeId)
        {
            try
            {
                DataTable cacheList = GetCacheList();
                if ((cacheList == null) || (cacheList.Rows.Count <= 0))
                    return null;
                DataRow[] rowArray = cacheList.Select("typeId=" + typeId.ToString());
                if ((rowArray == null) || (rowArray.Length <= 0))
                {
                    return null;
                }
                return GetInfoFromRow(rowArray[0]);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取对象。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SysChannelTypeInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltype_GetModel", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                    return GetInfoFromRow(set.Tables[0].Rows[0]);
                return null;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 获取用户通道类型。（返回enable：是否可用）
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="userId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static SysChannelTypeInfo GetModel(int typeId, int userId, out bool enable)
        {
            enable = false;
            int canuser = 0;
            SysChannelTypeInfo cacheModel = GetCacheModel(typeId);
            if (cacheModel == null)
            {
                return null;
            }
            switch (cacheModel.IsOpen)
            {
                case SysChannelTypeOpenEnum.AllClose:
                    canuser = 0;
                    break;

                case SysChannelTypeOpenEnum.AllOpen:
                    canuser = 1;
                    break;

                case SysChannelTypeOpenEnum.Close:
                    canuser = GetSysOpenStatus(userId, typeId, 0);
                    break;

                case SysChannelTypeOpenEnum.Open:
                    canuser = GetSysOpenStatus(userId, typeId, 1);
                    break;
            }
            if (canuser == 1)
            {
                canuser = GetUserOpenStatus(userId, typeId, 1);
            }
            enable = canuser == 1;
            return cacheModel;
        }

        /// <summary>
        /// 根据TypeID获取对象
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static SysChannelTypeInfo GetModelByTypeId(int typeId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@typeId", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = typeId;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltype_GetByTypeId", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                    return GetInfoFromRow(set.Tables[0].Rows[0]);
                return null;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 获取对象。
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        internal static SysChannelTypeInfo GetInfoFromRow(DataRow dr)
        {
            SysChannelTypeInfo modle = new SysChannelTypeInfo();
            modle.ID = Convert.ToInt32(dr["id"]);
            modle.TypeID = Utils.StrToInt(dr["typeid"].ToString(), 0);
            modle.TypeCode = Convert.ToString(dr["typecode"].ToString());
            modle.TypeClassID = Utils.StrToInt(dr["typeclassid"].ToString(), 0);
            modle.TypeName = Convert.ToString(dr["typename"].ToString());
            modle.IsOpen = (SysChannelTypeOpenEnum)Convert.ToInt32(dr["isopen"]);
            modle.SupplierCode = OriginalStudio.Lib.Utils.Utils.StrToInt(dr["SupplierCode"].ToString(), 0);
            modle.SupplierRate = Convert.ToDecimal(OriginalStudio.Lib.Utils.Utils.StrToFloat(dr["SupplierRate"].ToString(), 0));
            modle.AddTime = Convert.ToDateTime(dr["AddTime"].ToString());
            modle.ListOrder = Utils.StrToInt(dr["listorder"].ToString(), 0);
            modle.Release = Convert.ToBoolean(dr["release"]);
            modle.RunMode = Utils.StrToInt(dr["runmode"].ToString(), 0);    //
            modle.RunModeSet = Convert.ToString(dr["runmodeset"]);

            return modle;
        }

        /// <summary>
        /// 用户通道是否可用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static int GetSysOpenStatus(int userId, int typeId, int defaultvalue)
        {
            int num = defaultvalue;
            MchUserChannelType cacheModel = MchUsersChannelType.GetCacheModel(userId, typeId);
            if ((cacheModel != null) && cacheModel.SysIsOpen)
            {
                num = cacheModel.SysIsOpen ? 1 : 0;
            }
            return num;
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 新增通道类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功>0;不成功=0</returns>
        public static int Add(SysChannelTypeInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@typeid",SqlDbType.Int),
                    new SqlParameter("@typecode",SqlDbType.VarChar,50),
                    new SqlParameter("@typeclassid",SqlDbType.Int),
                    new SqlParameter("@typename",SqlDbType.VarChar,50),
                    new SqlParameter("@isopen",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    new SqlParameter("@supplierrate",SqlDbType.Decimal),
                    new SqlParameter("@listorder",SqlDbType.Int),
                    new SqlParameter("@release",SqlDbType.Int),
                    new SqlParameter("@runmode",SqlDbType.Int),
                    new SqlParameter("@runmodeset",SqlDbType.VarChar,1000)
                };
                parameters[0].Value = model.TypeID;
                parameters[1].Value = model.TypeCode;
                parameters[2].Value = model.TypeClassID;
                parameters[3].Value = model.TypeName;
                parameters[4].Value = model.IsOpen;
                parameters[5].Value = model.SupplierCode;
                parameters[6].Value = model.SupplierRate;
                parameters[7].Value = model.ListOrder;
                parameters[8].Value = model.Release;
                parameters[9].Value = model.RunMode;
                parameters[10].Value = model.RunModeSet;
                return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_channeltype_add", parameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 修改通道类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns>成功 True ;不成功 False</returns>
        public static bool Update(SysChannelTypeInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@typeid",SqlDbType.Int),
                    new SqlParameter("@typecode",SqlDbType.VarChar,50),
                    new SqlParameter("@typeclassid",SqlDbType.Int),
                    new SqlParameter("@typename",SqlDbType.VarChar,50),
                    new SqlParameter("@isopen",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    new SqlParameter("@addtime",SqlDbType.DateTime),
                    new SqlParameter("@listorder",SqlDbType.Int),
                    new SqlParameter("@release",SqlDbType.Int),
                    new SqlParameter("@runmode",SqlDbType.Int),
                    new SqlParameter("@runmodeset",SqlDbType.VarChar,1000)
                };
                parameters[0].Value = model.TypeID;
                parameters[1].Value = model.TypeCode;
                parameters[2].Value = model.TypeClassID;
                parameters[3].Value = model.TypeName;
                parameters[4].Value = model.IsOpen;
                parameters[5].Value = model.SupplierCode;
                parameters[6].Value = model.AddTime;     //错误修改
                parameters[7].Value = model.ListOrder;
                parameters[8].Value = model.Release;
                parameters[9].Value = model.RunMode;
                parameters[10].Value = model.RunModeSet;

                return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_channeltype_Update", parameters) > 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        #region 集合

        /// <summary>
        /// SQL缓存对象。
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCacheList()
        {
            try
            {
                string objId = CHANNELTYPE_CACHEKEY;
                DataSet o = new DataSet();

                //o = (DataSet)WebCache.GetCacheService().RetrieveObject(objId);
                //if (o == null)
                //{
                //    SqlDependency dependency = DataBase.AddSqlDependency(objId, SQL_TABLE, SQL_TABLE_FIELD, "", null);
                //    WebCache.GetCacheService().AddObject(objId, o);
                //}
                //2017.10.4先用下面的。
                o = GetList(true);

                return o.Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 获取通道类型列表。
        /// </summary>
        /// <param name="release"></param>
        /// <returns></returns>
        public static DataSet GetList(bool? release)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@release", SqlDbType.Bit, 1)
            };
            if (release.HasValue)
                commandParameters[0].Value = release.Value;
            else
                commandParameters[0].Value = DBNull.Value;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltype_GetList", commandParameters);
        }

        #endregion

        #region 判断是否通道可用
        
        public static bool IsOpen(int typeId, int userId)
        {
            bool flag = false;
            SysChannelTypeInfo cacheModel = GetCacheModel(typeId);
            if (cacheModel != null)
            {
                switch (cacheModel.IsOpen)
                {
                    case SysChannelTypeOpenEnum.AllClose:
                        flag = false;
                        break;

                    case SysChannelTypeOpenEnum.AllOpen:
                        flag = true;
                        break;

                    case SysChannelTypeOpenEnum.Close:
                        flag = GetSysOpenStatus(userId, typeId, false);
                        break;

                    case SysChannelTypeOpenEnum.Open:
                        flag = GetSysOpenStatus(userId, typeId, true);
                        break;
                }
                if (flag)
                {
                    flag = GetUserOpenStatus(userId, typeId, true);
                }
            }
            return flag;
        }
        
        public static bool GetUserOpenStatus(int userId, int typeId, bool defaultvalue)
        {
            bool flag = defaultvalue;
            ChannelTypeUserInfo cacheModel = ChannelTypeUsers.GetCacheModel(userId, typeId);
            if ((cacheModel != null) && cacheModel.userIsOpen.HasValue)
            {
                flag = cacheModel.userIsOpen.Value;
            }
            return flag;
        }

        public static bool GetSysOpenStatus(int userId, int typeId, bool defaultvalue)
        {
            bool flag = defaultvalue;
            ChannelTypeUserInfo cacheModel = ChannelTypeUsers.GetCacheModel(userId, typeId);
            if ((cacheModel != null) && cacheModel.sysIsOpen.HasValue)
            {
                flag = cacheModel.sysIsOpen.Value;
            }
            return flag;
        }

        public static int GetUserOpenStatus(int userId, int typeId, int defaultvalue)
        {
            int num = defaultvalue;
            ChannelTypeUserInfo cacheModel = ChannelTypeUsers.GetCacheModel(userId, typeId);
            if ((cacheModel != null) && cacheModel.userIsOpen.HasValue)
            {
                num = cacheModel.userIsOpen.Value ? 1 : 0;
            }
            return num;
        }

        #endregion
    }
}

