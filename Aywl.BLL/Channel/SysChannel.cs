namespace OriginalStudio.BLL.Channel
{
    using DBAccess;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Security.Cryptography;
    using System.Text;
    using OriginalStudio.BLL.Sys;

    /// <summary>
    /// 通道操作类。
    /// </summary>
    public class SysChannel
    {
        public static string CHANEL_CACHEKEY = (Constant.Cache_Mark + "SYSCHANNELS");
        internal static string SQL_TABLE = "sys_channel";
        internal static string SQL_TABLE_FIELD = "ID,ChannelCode,ChannelTypeId,SupplierCode,SupplierRate,ChannelName,ChannelEnName,FaceValue,IsOpen,AddTime,ListSort,CreateUserID,CreateTime";

        #region 查询缓存

        /// <summary>
        /// 获取通道缓存列表。
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCacheList()
        {
            try
            {
                string objId = CHANEL_CACHEKEY;
                DataSet o = new DataSet();
                o = (DataSet)WebCache.GetCacheService().RetrieveObject(objId);
                if (o == null)
                {
                    //SqlDependency dependency = DataBase.AddSqlDependency(objId, SQL_TABLE, SQL_TABLE_FIELD, "", null);
                    o = GetList(0);
                    WebCache.GetCacheService().AddObject(objId, o);
                }
                return o.Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataSet GetList(int typeId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@typeId", SqlDbType.Int)
                };
                commandParameters[0].Value = typeId;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channel_GetList", commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        private static void ClearCache()
        {
            string objId = CHANEL_CACHEKEY;
            WebCache.GetCacheService().RemoveObject(objId);
        }

        #endregion

        #region 获取实例

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="channelcode"></param>
        /// <returns></returns>
        public static SysChannelInfo GetChannelModelByCode(string channelCode)
        {
            try
            {
                DataTable cacheList = GetCacheList();
                if (cacheList == null)
                {
                    return null;
                }
                DataRow[] rowArray = cacheList.Select("ChannelCode='" + channelCode + "'");
                if ((rowArray == null) || (rowArray.Length <= 0))
                {
                    return null;
                }
                return GetModelFromRow(rowArray[0]);
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
        public static SysChannelInfo GetChannelModelByID(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_channel_GetModel", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                {
                    return GetModelFromRow(set.Tables[0].Rows[0]);
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
        /// 获取用户通道信息
        /// </summary>
        /// <param name="channelCode"></param>
        /// <param name="merchantName"></param>
        /// <param name="isupdatecurr_channel"></param>
        /// <returns></returns>
        public static SysChannelInfo GetChannelModel(string channelCode, int userId, bool isupdatecurr_channel)
        {
            //通道
            SysChannelInfo cacheModel = GetChannelModelByCode(channelCode);
            if (cacheModel != null)
            {
                //通道类型
                SysChannelTypeInfo typeInfo = SysChannelType.GetCacheModel(cacheModel.ChannelTypeId);
                if (typeInfo == null)
                {
                    return null;
                }

                if (cacheModel.SupplierCode == 0)
                {
                    //通道没有指定 供应商，按照 通道类型里的接口商。
                    cacheModel.SupplierCode = typeInfo.SupplierCode;
                    cacheModel.SupplierRate = typeInfo.SupplierRate;
                }

                if (isupdatecurr_channel && (typeInfo.RunMode == 1))
                {
                    //轮训模式
                    string runset = typeInfo.RunModeSet;
                    List<int> list = new List<int>();
                    List<ushort> list2 = new List<ushort>();
                    foreach (string str2 in runset.Split(new char[] { '|' }))
                    {
                        string[] strArray = str2.Split(new char[] { ':' });
                        list.Add(Convert.ToInt32(strArray[0]));
                        list2.Add(Convert.ToUInt16(strArray[1]));
                    }
                    RandomController controller = new RandomController(1);
                    controller.datas = list;
                    controller.weights = list2;
                    Random rand = new Random(GetRandomSeed());
                    int[] numArray = controller.ControllerRandomExtract(rand);
                    cacheModel.SupplierCode = Utils.StrToInt(numArray[0], 0);
                }
                int suppcode = -1;
                int isopen = 0;

                //OriginalStudio.Lib.Logging.LogHelper.Write("测试:" + userId.ToString() + "," + typeInfo.IsOpen.ToString());

                switch (typeInfo.IsOpen)
                {
                    case SysChannelTypeOpenEnum.AllClose:
                        //全部关闭  AllClose = 1,
                        suppcode = -1;  //GetUserSupplierCode(userId, cacheModel.ChannelTypeId);
                        isopen = 0;
                        break;

                    case SysChannelTypeOpenEnum.AllOpen:
                        //全部开启  AllOpen = 2,
                        suppcode = GetUserSupplierCode(userId, cacheModel.ChannelTypeId);
                        isopen = 1;
                        break;

                    case SysChannelTypeOpenEnum.Close:
                        //按配置(默认关闭) Close = 4,
                        isopen = GetChanelSysStatus(4, userId, channelCode, cacheModel.ChannelTypeId, ref suppcode);
                        break;

                    case SysChannelTypeOpenEnum.Open:
                        //按配置(默认开启) Open = 8
                        isopen = GetChanelSysStatus(8, userId, channelCode, cacheModel.ChannelTypeId, ref suppcode);
                        break;
                }
                //通道
                if (suppcode > -1)
                    cacheModel.SupplierCode = suppcode;

                if (isopen == 1)
                    isopen = GetUserSupplierOpenStatus(userId, channelCode, cacheModel.ChannelTypeId, 1);

                cacheModel.IsOpen = isopen == 1;
            }
            return cacheModel;
        }
        
        /// <summary>
        /// DataRow转为对象。
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static SysChannelInfo GetModelFromRow(DataRow dr)
        {
            SysChannelInfo modle = new SysChannelInfo();
            modle.ID = Convert.ToInt32(dr["id"]);
            modle.ChannelCode = Convert.ToString(dr["channelcode"].ToString());
            modle.ChannelTypeId = Utils.StrToInt(dr["channeltypeid"].ToString(), 0);
            modle.SupplierCode = Utils.StrToInt(dr["suppliercode"].ToString(), 0);
            modle.SupplierRate = Convert.ToDecimal(Utils.StrToInt(dr["SupplierRate"].ToString(), 0)); ;
            modle.ChannelName = Convert.ToString(dr["channelname"]);
            modle.ChannelEnName = Convert.ToString(dr["channelenname"]);
            modle.FaceValue = Utils.StrToInt(dr["facevalue"].ToString(), 0);
            modle.IsOpen = dr["IsOpen"].ToString() == "1" ? true : false;
            modle.AddTime = Convert.ToDateTime(dr["AddTime"]);
            modle.ListSort = Utils.StrToInt(dr["listsort"].ToString(), 0);
            modle.CreateUserID = Utils.StrToInt(dr["createuserid"].ToString(), 0);
            if (!String.IsNullOrEmpty(dr["CreateTime"].ToString()))
                modle.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

            return modle;
        }

        //public static SysChannelInfo GetModel(int typeId, int value, int userId, bool isupdatecurr_channel)
        //{
        //    try
        //    {
        //        DataTable cacheList = GetCacheList();
        //        if (cacheList == null)
        //        {
        //            return null;
        //        }
        //        DataRow[] rowArray = cacheList.Select("typeId=" + typeId.ToString() + " and faceValue=" + value.ToString());
        //        if ((rowArray == null) || (rowArray.Length <= 0))
        //        {
        //            return null;
        //        }
        //        return GetModel(rowArray[0]["code"].ToString(), userId, isupdatecurr_channel);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public static SysChannelInfo GetModelByCode(string code)
        //{
        //    try
        //    {
        //        SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@code", SqlDbType.VarChar, 10) };
        //        commandParameters[0].Value = code;
        //        DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channel_GetBycode", commandParameters);
        //        if (set.Tables[0].Rows.Count > 0)
        //        {
        //            return GetModelFromRow(set.Tables[0].Rows[0]);
        //        }
        //        return null;
        //    }
        //    catch (Exception exception)
        //    {
        //        ExceptionHandler.HandleException(exception);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] data = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(data);
            return BitConverter.ToInt32(data, 0);
        }

        #endregion

        #region 用户通道操作

        /// <summary>
        /// 获取系统通道类型。
        /// </summary>
        /// <param name="typeStatus"></param>
        /// <param name="userId"></param>
        /// <param name="channelCode"></param>
        /// <param name="typeId"></param>
        /// <param name="suppid"></param>
        /// <returns></returns>
        public static int GetChanelSysStatus(int typeStatus, int userId, string channelCode, int typeId, ref int suppid)
        {
            suppid = -1;
            int status = 0;
            int issysopen = -1;
            int isuseropen = -1;
            SysChannelInfo channelInfo = null;
            if (!string.IsNullOrEmpty(channelCode))
            {
                channelInfo = GetChannelModelByCode(channelCode);
            }

            if (channelInfo != null)     // && channelInfo.IsOpen
            {
                issysopen = channelInfo.IsOpen ? 1 : 0;
            }

            MchUserChannelType userchannelInfo = MchUsersChannelTypeFactory.GetCacheModel(userId, typeId);
            if (userchannelInfo != null)       //) && userchannelInfo.SysIsOpen
            {                
                isuseropen = userchannelInfo.SysIsOpen ? 1 : 0;
                if (userchannelInfo.SupplierCode > 0)
                {
                    suppid = userchannelInfo.SupplierCode;
                }
            }

            if (typeStatus == 4)
            {
                //按配置(默认关闭)
                if (issysopen == -1)
                {
                    issysopen = 0;
                }
                if (isuseropen == -1)
                {
                    isuseropen = 0;
                }
            }
            else if (typeStatus == 8)
            {
                //按配置(默认开启)
                if (issysopen == -1)
                {
                    issysopen = 1;
                }
                if (isuseropen == -1)
                {
                    isuseropen = 1;
                }
            }
            if ((issysopen == 1) && (isuseropen == 1))
            {
                status = 1;
            }
            return status;
        }

        /// <summary>
        /// 获取用户供应商通道状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelCode"></param>
        /// <param name="typeId"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static int GetUserSupplierOpenStatus(int userId, string channelCode, int typeId, int defaultvalue)
        {
            int num = defaultvalue;
            MchUserChannelType cacheModel = MchUsersChannelTypeFactory.GetCacheModel(userId, typeId);
            if ((cacheModel != null) && cacheModel.UserIsOpen) //cacheModel.userIsOpen
            {
                num = cacheModel.UserIsOpen ? 1 : 0;
            }
            return num;
        }

        /// <summary>
        /// 获取用户自定义供应商通道
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private static int GetUserSupplierCode(int userId, int typeId)
        {
            int suppcode = -1;
            MchUserChannelType cacheModel = MchUsersChannelTypeFactory.GetCacheModel(userId, typeId);
            if (cacheModel != null && cacheModel.SupplierCode > 0)
            {
                suppcode = cacheModel.SupplierCode;
            }
            return suppcode;
        }

        #endregion

        #region 增删改

        public static int Add(SysChannelInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@channelcode",SqlDbType.VarChar,10),
                    new SqlParameter("@channeltypeid",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    //new SqlParameter("@supplierrate",SqlDbType.Decimal),
                    new SqlParameter("@channelname",SqlDbType.VarChar,50),
                    new SqlParameter("@channelenname",SqlDbType.VarChar,50),
                    new SqlParameter("@facevalue",SqlDbType.Int),
                    new SqlParameter("@isopen",SqlDbType.Int),
                    new SqlParameter("@listsort",SqlDbType.Int),
                    new SqlParameter("@createuserid",SqlDbType.Int)
                };
                parameters[0].Direction = ParameterDirection.InputOutput;
                parameters[1].Value = model.ChannelCode;
                parameters[2].Value = model.ChannelTypeId;
                parameters[3].Value = model.SupplierCode;
                //parameters[4].Value = model.SupplierRate;
                parameters[4].Value = model.ChannelName;
                parameters[5].Value = model.ChannelEnName;
                parameters[6].Value = model.FaceValue;
                parameters[7].Value = model.IsOpen;
                parameters[8].Value = model.ListSort;
                parameters[9].Value = model.CreateUserID;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_channel_add", parameters);
                int num = (int)parameters[0].Value;
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

        public static bool Update(SysChannelInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@channeltypeid",SqlDbType.Int),
                    new SqlParameter("@suppliercode",SqlDbType.Int),
                    new SqlParameter("@channelname",SqlDbType.VarChar,50),
                    new SqlParameter("@channelenname",SqlDbType.VarChar,50),
                    new SqlParameter("@facevalue",SqlDbType.Int),
                    new SqlParameter("@isopen",SqlDbType.Int),
                    new SqlParameter("@listsort",SqlDbType.Int)
                };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.ChannelTypeId;
                parameters[2].Value = model.SupplierCode;
                parameters[3].Value = model.ChannelName;
                parameters[4].Value = model.ChannelEnName;
                parameters[5].Value = model.FaceValue;
                parameters[6].Value = model.IsOpen;
                parameters[7].Value = model.ListSort; 
                return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_channel_Update", parameters) > 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        #region 查询

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter;
                    SearchParam param2 = param[i];
                    switch (param2.ParamKey.Trim().ToLower())
                    {
                        case "id":
                            builder.Append(" AND [id] = @id");
                            parameter = new SqlParameter("@id", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "typeid":
                            builder.Append(" AND [ChannelTypeId] = @typeId");
                            parameter = new SqlParameter("@typeId", SqlDbType.Int, 10);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public static bool Delete(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = id;
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_channel_Delete", commandParameters) > 0);
        }

        public static DataSet GetBankChanels(int pageindex, int pagesize, int userid, int typeid, int facevalue, int chanelstatus)
        {
            string commandText = "select count(*) as total\r\nfrom (\r\nselect\r\n\tdbo.f_getuserChanelStatus(a.isOpen,b.isOpen,c.sysIsOpen,c.userIsOpen) as chanelstatus\r\nfrom\r\n\tchanneltype a left join channel b on a.typeId = b.typeId\r\n\t\t\t\t  left join channeltypeusers c on a.typeId = c.typeId and c.userId = @userId\r\nwhere\r\n\ta.release = 1 and a.typeid <= 102\r\n\tand (a.typeid = @typeid or @typeid is null)\r\n\tand (b.faceValue = @faceValue or @faceValue is null)\r\n\t) dd\r\nwhere (dd.chanelstatus = @chanelstatus or @chanelstatus is null)\r\n\r\nselect typeid,code,faceValue,modetypename,chanelstatus,modeName\r\nfrom (\r\nselect\r\n\ta.typeid,\r\n\tb.code,\r\n\tb.faceValue,\r\n\ta.modetypename,b.modeName,\r\n\tdbo.f_getuserChanelStatus(a.isOpen,b.isOpen,c.sysIsOpen,c.userIsOpen) as chanelstatus\r\n\t,ROW_NUMBER() OVER(ORDER BY a.typeid,b.facevalue) AS P_ROW \r\nfrom\r\n\tchanneltype a left join channel b on a.typeId = b.typeId\r\n\t\t\t\t  left join channeltypeusers c on a.typeId = c.typeId and c.userId = @userId\r\nwhere\r\n\ta.release = 1 and a.typeid <= 102\r\n\tand (a.typeid = @typeid or @typeid is null)\r\n\tand (b.faceValue = @faceValue or @faceValue is null)\r\n\t) dd\r\nwhere (dd.chanelstatus = @chanelstatus or @chanelstatus is null) \r\nand dd.P_ROW BETWEEN @page*@pagesize+1 AND @page*@pagesize+@pagesize ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@page", pageindex - 1));
            list.Add(new SqlParameter("@pagesize", pagesize));
            list.Add(new SqlParameter("@userId", userid));
            if (typeid > 0)
            {
                list.Add(new SqlParameter("@typeid", typeid));
            }
            else
            {
                list.Add(new SqlParameter("@typeid", DBNull.Value));
            }
            if (facevalue > 0)
            {
                list.Add(new SqlParameter("@facevalue", facevalue));
            }
            else
            {
                list.Add(new SqlParameter("@facevalue", DBNull.Value));
            }
            if (chanelstatus > -1)
            {
                list.Add(new SqlParameter("@chanelstatus", chanelstatus));
            }
            else
            {
                list.Add(new SqlParameter("@chanelstatus", DBNull.Value));
            }
            return DataBase.ExecuteDataset(CommandType.Text, commandText, list.ToArray());
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = SQL_TABLE;
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + 
                                                    SqlHelper.GetPageSelectSQL(SQL_TABLE_FIELD, tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        #endregion

    }

}

