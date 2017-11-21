﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OriginalStudio.Model.User;
using System.Data.SqlClient;
using System.Data;
using OriginalStudio.DBAccess;
using OriginalStudio.Lib.Utils;
using OriginalStudio.Lib.ExceptionHandling;

namespace OriginalStudio.BLL.BLL.User
{
    /// <summary>
    /// 商户自定义费率操作类。
    /// </summary>
    public class MchUserPayRateFactory
    {
        /// <summary>
        /// 获取商户自定义费率对象
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static MchUserPayRateInfo GetModel(int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@userid", SqlDbType.Int, 10) 
            };
            commandParameters[0].Value = userid;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_mch_userpayrate_GetModel", commandParameters);

            MchUserPayRateInfo modle = new MchUserPayRateInfo();

            if (set.Tables[0].Rows.Count == 0) return modle;
            
            DataRow dr = set.Tables[0].Rows[0];

            modle.UserID = Utils.StrToInt(dr["userid"].ToString(),0);
            modle.DefaultPay = Utils.StrToInt(dr["defaultpay"].ToString(),0);
            modle.Special = Utils.StrToInt(dr["special"].ToString(),0);
            modle.PayrateXML = dr["PayrateXML"].ToString();
            modle.Istransfer = Utils.StrToInt(dr["istransfer"].ToString(), 0) == 1;
            modle.IsRequireAgentDistAudit = Utils.StrToInt(dr["isrequireagentdistaudit"].ToString(), 0) == 1;

            return modle;
        }

        /// <summary>
        /// 获取商户通道费率
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelTypeId"></param>
        /// <returns></returns>
        public static decimal GetUserChannelPayRate(int userId, int channelTypeId)
        {
            string sql = "select PayrateXML.query('(/PayRate/ChannelType[@ID=\"" + channelTypeId.ToString() + "\"])').value('(/ChannelType/@Rate)[1]','nvarchar(max)') " +
                                "from mch_userPayRate where Special = 1 and UserId = " + userId.ToString();
            string tmp = Convert.ToString(DataBase.ExecuteScalar(CommandType.Text, sql));

            return string.IsNullOrEmpty(tmp) ? 0 : Utils.StrToDecimal(tmp);
        }

        /// <summary>
        /// 增加或修改商户通道费率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool EditUserChannelPayRate(MchUserPayRateInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@userid",SqlDbType.Int),
                    new SqlParameter("@defaultpay",SqlDbType.Int),
                    new SqlParameter("@payratexml",SqlDbType.VarChar,4000),
                    new SqlParameter("@special",SqlDbType.Int),
                    new SqlParameter("@istransfer",SqlDbType.Bit),
                    new SqlParameter("@isRequireAgentDistAudit",SqlDbType.Bit)
                };
                parameters[0].Value = model.UserID;
                parameters[1].Value = model.DefaultPay;
                parameters[2].Value = model.PayrateXML;
                parameters[3].Value = model.Special;
                parameters[4].Value = model.Istransfer;
                parameters[5].Value = model.IsRequireAgentDistAudit;

                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_mch_userpayrate_Edit", parameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

    }
}
