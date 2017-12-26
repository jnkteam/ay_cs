namespace OriginalStudio.BLL.PayRate
{
    using DBAccess;
    using OriginalStudio.BLL.Settled;
    using OriginalStudio.Cache;
    using OriginalStudio.Model.PayRate;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.BLL.BLL.User;
    using OriginalStudio.Lib.Utils;

    /// <summary>
    /// 费率操作类（平台、商户、代理）。
    /// </summary>
    public sealed class SysPayRateFactory
    {
        internal static string PAYRATEFACTORY_CACHEKEY = (Constant.Cache_Mark + "{{D2F59122-0613-4968-ABD1-9A8F9FFD7B13}}_{0}_{1}");
        internal static string SQL_TABLE = "sys_payrate";
        internal static string SQL_TABLE_FIELD = "[ID],[RateName],[RateType],[Active],[CreateUserID],[CreateTime],[PayrateXML],[UserLevel]";

        #region 获取商户费率

        //XML格式：<PayRate><ChannelType ID="21" Rate="0.982"/><ChannelType ID="22" Rate="0.982"/><ChannelType ID="31" Rate="0.982"/></PayRate>

        /// <summary>
        /// 获取用户通道费率
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelTypeId"></param>
        /// <returns></returns>
        public static decimal GetUserChannelTypePayRate(int userId, int channelTypeId)
        {
            //select PayrateXML.query('(/PayRate/ChannelType[@ID="22"])').value('(/ChannelType/@Rate)[1]','nvarchar(max)') from mch_userPayRate;
            MchUserBaseInfo cacheUserBaseInfo = MchUserFactory.GetUserBaseByUserID(userId);
            if (cacheUserBaseInfo == null)
            {
                return 0M;
            }
            if (cacheUserBaseInfo.Status != 2)
            {
                //非审核通过账户,费率一律为0
                return 0M;
            }

            decimal tmp= 0;

            //自定义费率为0，再根据商户的费率类型PayRateID取值
            if (cacheUserBaseInfo.UserType == UserTypeEnum.代理)
            {
                //OriginalStudio.Lib.Logging.LogHelper.Write("order.agentId ID:" + userId.ToString());

                tmp = GetSysChannelTypePayRate(RateTypeEnum.代理, cacheUserBaseInfo.UserLevel, channelTypeId);
            }
            else if (cacheUserBaseInfo.UserType == UserTypeEnum.商户)
            {
                //首先取商户自定义费率
                tmp = MchUserPayRateFactory.GetUserChannelPayRate(userId, channelTypeId);

                if (tmp > 0)
                    return tmp;

                //再取 系统费率 设置
                tmp = GetSysChannelTypePayRate(RateTypeEnum.会员, cacheUserBaseInfo.UserLevel, channelTypeId);
            }
            return tmp;
        }

        /// <summary>
        /// 获取系统定义费率类型
        /// </summary>
        /// <param name="payRateId">费率序号</param>
        /// <param name="channelTypeId">通道类型</param>
        /// <returns></returns>
        public static decimal GetSysChannelTypePayRate(int payRateId, int channelTypeId)
        {
            string sql = "select PayrateXML.query('(/PayRate/ChannelType[@ID=\"" + channelTypeId.ToString() + "\"])').value('(/ChannelType/@Rate)[1]','nvarchar(max)') " +
                                "from sys_payrate where ID = " + payRateId.ToString();
            string tmp = Convert.ToString(DataBase.ExecuteScalar(CommandType.Text, sql));

            return string.IsNullOrEmpty(tmp) ? 0 : Utils.StrToDecimal(tmp);
        }

        /// <summary>
        /// 获取会员或代理等级指定通道费率
        /// </summary>
        /// <param name="rateType"></param>
        /// <param name="userLevel"></param>
        /// <param name="channelTypeId"></param>
        /// <returns></returns>
        public static decimal GetSysChannelTypePayRate(RateTypeEnum rateType, UserLevelEnum userLevel, int channelTypeId)
        {
            string sql = "select PayrateXML.query('(/PayRate/ChannelType[@ID=\"" + channelTypeId.ToString() + "\"])').value('(/ChannelType/@Rate)[1]','nvarchar(max)') "+
                                "from sys_payrate where Active = 1 and RateType = " + ((int)rateType).ToString() + " and UserLevel = " + ((int)userLevel).ToString();
            string tmp = Convert.ToString(DataBase.ExecuteScalar(CommandType.Text, sql));

            return string.IsNullOrEmpty(tmp) ? 0 : Utils.StrToDecimal(tmp);
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 增加费率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(SysPayRateInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@ratename",SqlDbType.VarChar,100),
                    new SqlParameter("@ratetype",SqlDbType.Int),
                    new SqlParameter("@active",SqlDbType.Int),
                    new SqlParameter("@createuserid",SqlDbType.Int),
                    new SqlParameter("@payratexml",SqlDbType.VarChar,4000),
                    new SqlParameter("@userlevel",SqlDbType.Int)
                };
                parameters[0].Direction = ParameterDirection.InputOutput;
                parameters[1].Value = model.RateName;
                parameters[2].Value = model.RateType;
                parameters[3].Value = model.Active;
                parameters[4].Value = model.CreateUserID;
                parameters[5].Value = model.PayrateXML;
                parameters[6].Value = model.UserLevel;

                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_payrate_add", parameters) > 0)
                {
                    return (int)parameters[0].Value;
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        /// <summary>
        /// 修改费率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update(SysPayRateInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@ratename",SqlDbType.VarChar,100),
                    new SqlParameter("@ratetype",SqlDbType.Int),
                    new SqlParameter("@active",SqlDbType.Int),
                    new SqlParameter("@createuserid",SqlDbType.Int),
                    new SqlParameter("@payratexml",SqlDbType.VarChar,4000),
                    new SqlParameter("@userlevel",SqlDbType.Int)
                };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.RateName;
                parameters[2].Value = model.RateType;
                parameters[3].Value = model.Active;
                parameters[4].Value = model.CreateUserID;
                parameters[5].Value = model.PayrateXML;
                parameters[6].Value = model.UserLevel;

                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_sys_payrate_Update", parameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        #region 获取对象或列表

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SysPayRateInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = id;
            SysPayRateInfo modle = new SysPayRateInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_payrate_GetModel", commandParameters);
            return GetModelFromDs(set);
        }

        private static SysPayRateInfo GetModelFromDs(DataSet ds)
        {
            SysPayRateInfo modle = new SysPayRateInfo();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                modle.ID = Utils.StrToInt(dr["id"], 0);
                modle.RateName = Convert.ToString(dr["ratename"]);
                modle.RateType = (RateTypeEnum)Utils.StrToInt(dr["ratetype"], 1);
                modle.Active = Utils.StrToInt(dr["active"], 0);
                modle.CreateUserID = Utils.StrToInt(dr["createuserid"], 0);
                modle.CreateTime = Utils.StrToDateTime(dr["createuserid"]);
                modle.PayrateXML = dr["PayrateXML"].ToString();
                modle.UserLevel = Utils.StrToInt(dr["userlevel"], 0);
                return modle;
            }
            else
                return null;
        }

        public static DataTable GetList(RateTypeEnum rateType, int userLevel)
        {
            try
            {
                string commandText = "select [ID],[RateName],[RateType],[Active],[CreateUserID],[CreateTime],[PayrateXML],[UserLevel] from sys_payrate " +
                                                    " where RateType = " + ((Int32)rateType).ToString();
                DataSet o = DataBase.ExecuteDataset(CommandType.Text, commandText);
                return o.Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }
        public static DataTable GetAllList()
        {
            try
            {
                string commandText = "select [ID],[RateName],[RateType],[Active],[CreateUserID],[CreateTime],[PayrateXML],[UserLevel] from sys_payrate ";
                DataSet o = DataBase.ExecuteDataset(CommandType.Text, commandText);
                return o.Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }
        public static SysPayRateInfo GetModelByUser(int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@userid", SqlDbType.Int, 10)
            };
            commandParameters[0].Value = userid;
            PayRate rate = new PayRate();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_sys_payrate_GetModelByUser", commandParameters);
            return GetModelFromDs(set);
        }

        public static DataTable GetLevName(RateTypeEnum rateType)
        {
            string commandText = "SELECT [ID],[RateName] FROM sys_payrate WHERE rateType = @rateType";
            SqlParameter[] commandParameters = new SqlParameter[] 
            {
                new SqlParameter("@rateType", SqlDbType.TinyInt, 1)
            };
            commandParameters[0].Value = (int)rateType;
            return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters).Tables[0];
        }

        #endregion
    }
}

