namespace OriginalStudio.BLL.Channel
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.Lib.Utils;

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
            objId = OriginalStudio.BLL.Channel.SysChannel.CHANEL_CACHEKEY;
            WebCache.GetCacheService().RemoveObject(objId);
        }

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
                {
                    return null;
                }
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
        /// 获取通道类型列表。
        /// </summary>
        /// <param name="release"></param>
        /// <returns></returns>
        public static DataSet GetList(bool? release)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@release", SqlDbType.Bit, 1) };
            if (release.HasValue)
            {
                commandParameters[0].Value = release.Value;
            }
            else
            {
                commandParameters[0].Value = DBNull.Value;
            }
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltype_GetList", commandParameters);
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
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltype_GetModel", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                {
                    return GetInfoFromRow(set.Tables[0].Rows[0]);
                }
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
            int num = 0;
            SysChannelTypeInfo cacheModel = GetCacheModel(typeId);
            if (cacheModel == null)
            {
                return null;
            }
            switch (cacheModel.IsOpen)
            {
                case SysChannelTypeOpenEnum.AllClose:
                    num = 0;
                    break;

                case SysChannelTypeOpenEnum.AllOpen:
                    num = 1;
                    break;

                case SysChannelTypeOpenEnum.Close:
                    num = GetSysOpenStatus(userId, typeId, 0);
                    break;

                case SysChannelTypeOpenEnum.Open:
                    num = GetSysOpenStatus(userId, typeId, 1);
                    break;
            }
            if (num == 1)
            {
                num = GetUserOpenStatus(userId, typeId, 1);
            }
            enable = num == 1;
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
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@typeId", SqlDbType.Int, 10) };
                commandParameters[0].Value = typeId;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channeltype_GetByTypeId", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                {
                    return GetInfoFromRow(set.Tables[0].Rows[0]);
                }
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

        #endregion

        public static int Add(ChannelTypeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@classid", SqlDbType.TinyInt, 1), new SqlParameter("@typeId", SqlDbType.Int, 10), new SqlParameter("@modetypename", SqlDbType.VarChar, 50), new SqlParameter("@isOpen", SqlDbType.TinyInt, 1), new SqlParameter("@supplier", SqlDbType.Int, 10), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@sort", SqlDbType.Int, 10), new SqlParameter("@release", SqlDbType.Bit, 1), new SqlParameter("@runmode", SqlDbType.TinyInt, 4), new SqlParameter("@runset", SqlDbType.VarChar, 0x3e8) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.Class;
                commandParameters[2].Value = model.typeId;
                commandParameters[3].Value = model.modetypename;
                commandParameters[4].Value = (int) model.isOpen;
                commandParameters[5].Value = model.supplier;
                commandParameters[6].Value = model.addtime;
                commandParameters[7].Value = model.sort;
                commandParameters[8].Value = model.release;
                commandParameters[9].Value = model.runmode;
                commandParameters[10].Value = model.runset;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channeltype_add", commandParameters);
                int num = (int) commandParameters[0].Value;
                if (num > 0)
                {
                    ClearCache();
                }
                return num;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static int GetChannelTypeStatus(int typeId, int userId)
        {
            int num = 0;
            SysChannelTypeInfo cacheModel = GetCacheModel(typeId);
            if (cacheModel != null)
            {
                int suppid = 0;
                switch (cacheModel.IsOpen)
                {
                    case SysChannelTypeOpenEnum.AllClose:
                        num = 0;
                        break;

                    case SysChannelTypeOpenEnum.AllOpen:
                        num = 1;
                        break;

                    case SysChannelTypeOpenEnum.Close:
                        num = OriginalStudio.BLL.Channel.Channel.GetChanelSysStatus(4, userId, string.Empty, typeId, ref suppid);
                        break;

                    case SysChannelTypeOpenEnum.Open:
                        num = OriginalStudio.BLL.Channel.Channel.GetChanelSysStatus(8, userId, string.Empty, typeId, ref suppid);
                        break;
                }
                if (num == 1)
                {
                    num = OriginalStudio.BLL.Channel.Channel.GetUserOpenStatus(userId, string.Empty, typeId, 1);
                }
            }
            return num;
        }

        public static DataTable GetListByUser(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Bit, 1) };
                commandParameters[0].Value = userid;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channeltype_GetListByUser", commandParameters).Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static int GetSysOpenStatus(int userId, int typeId, int defaultvalue)
        {
            int num = defaultvalue;
            ChannelTypeUserInfo cacheModel = ChannelTypeUsers.GetCacheModel(userId, typeId);
            if ((cacheModel != null) && cacheModel.sysIsOpen.HasValue)
            {
                num = cacheModel.sysIsOpen.Value ? 1 : 0;
            }
            return num;
        }

        public static int GetSysTypeId(int interfaceTypeId)
        {
            int num = interfaceTypeId;
            switch (interfaceTypeId)
            {
                case 1:
                    return 0x6b;

                case 2:
                    return 0x68;

                case 3:
                    return 0x6a;

                case 4:
                    return 0x75;

                case 5:
                    return 0x6f;

                case 6:
                    return 0x70;

                case 7:
                    return 0x69;

                case 8:
                    return 0x6d;

                case 9:
                    return 110;

                case 10:
                    return 0x76;

                case 11:
                    return 0x77;

                case 12:
                    return 0x71;

                case 13:
                    return 0x67;

                case 14:
                    return 0x6c;

                case 15:
                    return 0x74;

                case 0x10:
                    return 0x73;

                case 0x11:
                    return 0x67;

                case 0x12:
                    return 0x67;

                case 0x13:
                    return 0x67;

                case 20:
                    return 0x67;

                case 0x15:
                    return 0x76;

                case 0x16:
                    return 0x77;

                case 0x17:
                    return 0x75;

                case 0x18:
                case 0x19:
                    return num;

                case 0x1a:
                    return 0xd0;

                case 0x1b:
                    return 0xd1;

                case 0x1c:
                    return 210;
            }
            return num;
        }

        public static int GetSysTypeId(int interfaceTypeId, string cardno)
        {
            int num = interfaceTypeId;
            switch (interfaceTypeId)
            {
                case 1:
                    return 0x6b;

                case 2:
                    num = 0x68;
                    if (cardno.StartsWith("80"))
                    {
                        num = 210;
                    }
                    return num;

                case 3:
                    return 0x6a;

                case 4:
                    return 0x75;

                case 5:
                    return 0x6f;

                case 6:
                    return 0x70;

                case 7:
                    return 0x69;

                case 8:
                    return 0x6d;

                case 9:
                    return 110;

                case 10:
                    return 0x76;

                case 11:
                    return 0x77;

                case 12:
                    return 0x71;

                case 13:
                    return 0x67;

                case 14:
                    return 0x6c;

                case 15:
                    return 0x74;

                case 0x10:
                    return 0x73;

                case 0x11:
                    return 0x67;

                case 0x12:
                    return 0x67;

                case 0x13:
                    return 0x67;

                case 20:
                    return 0x67;

                case 0x15:
                    return 0x76;

                case 0x16:
                    return 0x77;

                case 0x17:
                    return 0x75;

                case 0x18:
                case 0x19:
                    return num;

                case 0x1a:
                    return 0xd0;

                case 0x1b:
                    return 0xd1;

                case 0x1c:
                    return 210;
            }
            return num;
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

        public static bool IsShengFuTong(string cardno)
        {
            if (string.IsNullOrEmpty(cardno))
            {
                return false;
            }
            string str = "^(8013|YA|YB|YC|YD)";
            return QuickValidate(str, cardno);
        }

        public static bool QuickValidate(string _express, string _value)
        {
            Regex regex = new Regex(_express, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if ((_value == null) || (_value.Length == 0))
            {
                return false;
            }
            return regex.IsMatch(_value);
        }

        public static bool Update(ChannelTypeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@classid", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@typeId", SqlDbType.Int, 10), 
                    new SqlParameter("@modetypename", SqlDbType.VarChar, 50), 
                    new SqlParameter("@isOpen", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@supplier", SqlDbType.Int, 10), 
                    new SqlParameter("@addtime", SqlDbType.DateTime), 
                    new SqlParameter("@sort", SqlDbType.Int, 10), 
                    new SqlParameter("@release", SqlDbType.Bit, 1), 
                    new SqlParameter("@runmode", SqlDbType.TinyInt, 4), 
                    new SqlParameter("@runset", SqlDbType.VarChar, 0x3e8) 
                };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.Class;
                commandParameters[2].Value = model.typeId;
                commandParameters[3].Value = model.modetypename;
                commandParameters[4].Value = (int) model.isOpen;
                commandParameters[5].Value = model.supplier;
                commandParameters[6].Value = DateTime.Now;
                commandParameters[7].Value = model.sort;
                commandParameters[8].Value = model.release;
                commandParameters[9].Value = model.runmode;
                commandParameters[10].Value = model.runset;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channeltype_Update", commandParameters) > 0;
                if (flag)
                {
                    ClearCache();
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #region 判断是否通道可用

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

