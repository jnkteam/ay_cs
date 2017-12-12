using OriginalStudio.BLL.User;
using OriginalStudio.DBAccess;
using OriginalStudio.Lib.ExceptionHandling;
using OriginalStudio.Lib.Utils;
using OriginalStudio.Model.Withdraw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OriginalStudio.BLL.Withdraw
{
    public class WithdrawSchemeFactory
    {

        #region 增删改

        public static int Add(WithdrawSchemeInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@type",SqlDbType.Int),
                    new SqlParameter("@schemename",SqlDbType.NVarChar,100),
                    new SqlParameter("@singleminamtlimit",SqlDbType.Decimal),
                    new SqlParameter("@singlemaxamtlimit",SqlDbType.Decimal),
                    new SqlParameter("@dailymaxtimes",SqlDbType.Int),
                    new SqlParameter("@dailymaxamt",SqlDbType.Decimal),
                    new SqlParameter("@chargerate",SqlDbType.Decimal),
                    new SqlParameter("@singlemincharge",SqlDbType.Decimal),
                    new SqlParameter("@singlemaxcharge",SqlDbType.Decimal),
                    new SqlParameter("@istranapi",SqlDbType.Int),
                    new SqlParameter("@isdefault",SqlDbType.Int),
                    new SqlParameter("@issys",SqlDbType.Int),
                    new SqlParameter("@bankdetentiondays",SqlDbType.Int),
                    new SqlParameter("@qqdetentiondays",SqlDbType.Int),
                    new SqlParameter("@jddetentiondays",SqlDbType.Int),
                    new SqlParameter("@istranrequiredaudit",SqlDbType.Int),
                    new SqlParameter("@alipaydetentiondays",SqlDbType.Int),
                    new SqlParameter("@weixindetentiondays",SqlDbType.Int),
                    new SqlParameter("@otherdetentiondays",SqlDbType.Int),
                    new SqlParameter("@transupplier",SqlDbType.Int)
                };
                parameters[0].Value = model.Type;
                parameters[1].Value = model.SchemeName;
                parameters[2].Value = model.SingleMinAmtLimit;
                parameters[3].Value = model.SingleMaxAmtLimit;
                parameters[4].Value = model.DailyMaxTimes;
                parameters[5].Value = model.DailyMaxAmt;
                parameters[6].Value = model.ChargeRate;
                parameters[7].Value = model.SingleMinCharge;
                parameters[8].Value = model.SingleMaxCharge;
                parameters[9].Value = model.IsTranApi;
                parameters[10].Value = model.IsDefault;
                parameters[11].Value = model.IsSys;
                parameters[12].Value = model.BankDetentionDays;
                parameters[13].Value = model.QQDetentionDays;
                parameters[14].Value = model.JDDetentionDays;
                parameters[15].Value = model.IsTranRequiredAudit;
                parameters[16].Value = model.AlipayDetentionDays;
                parameters[17].Value = model.WeiXinDetentionDays;
                parameters[18].Value = model.OtherDetentionDays;
                parameters[19].Value = model.TranSupplier;
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_whd_withdraw_scheme_ADD", parameters) > 0)
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

        public static bool Update(WithdrawSchemeInfo model)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@type",SqlDbType.Int),
                    new SqlParameter("@schemename",SqlDbType.NVarChar,100),
                    new SqlParameter("@singleminamtlimit",SqlDbType.Decimal),
                    new SqlParameter("@singlemaxamtlimit",SqlDbType.Decimal),
                    new SqlParameter("@dailymaxtimes",SqlDbType.Int),
                    new SqlParameter("@dailymaxamt",SqlDbType.Decimal),
                    new SqlParameter("@chargerate",SqlDbType.Decimal),
                    new SqlParameter("@singlemincharge",SqlDbType.Decimal),
                    new SqlParameter("@singlemaxcharge",SqlDbType.Decimal),
                    new SqlParameter("@istranapi",SqlDbType.Int),
                    new SqlParameter("@isdefault",SqlDbType.Int),
                    new SqlParameter("@issys",SqlDbType.Int),
                    new SqlParameter("@bankdetentiondays",SqlDbType.Int),
                    new SqlParameter("@qqdetentiondays",SqlDbType.Int),
                    new SqlParameter("@jddetentiondays",SqlDbType.Int),
                    new SqlParameter("@istranrequiredaudit",SqlDbType.Int),
                    new SqlParameter("@alipaydetentiondays",SqlDbType.Int),
                    new SqlParameter("@weixindetentiondays",SqlDbType.Int),
                    new SqlParameter("@otherdetentiondays",SqlDbType.Int),
                    new SqlParameter("@transupplier",SqlDbType.Int)
                };
                parameters[0].Value = model.ID;
                parameters[1].Value = model.Type;
                parameters[2].Value = model.SchemeName;
                parameters[3].Value = model.SingleMinAmtLimit;
                parameters[4].Value = model.SingleMaxAmtLimit;
                parameters[5].Value = model.DailyMaxTimes;
                parameters[6].Value = model.DailyMaxAmt;
                parameters[7].Value = model.ChargeRate;
                parameters[8].Value = model.SingleMinCharge;
                parameters[9].Value = model.SingleMaxCharge;
                parameters[10].Value = model.IsTranApi;
                parameters[11].Value = model.IsDefault;
                parameters[12].Value = model.IsSys;
                parameters[13].Value = model.BankDetentionDays;
                parameters[14].Value = model.QQDetentionDays;
                parameters[15].Value = model.JDDetentionDays;
                parameters[16].Value = model.IsTranRequiredAudit;
                parameters[17].Value = model.AlipayDetentionDays;
                parameters[18].Value = model.WeiXinDetentionDays;
                parameters[19].Value = model.OtherDetentionDays;
                parameters[20].Value = model.TranSupplier;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_whd_withdraw_scheme_update", parameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_whd_withdraw_scheme_delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        #endregion

        #region 获取对象集合
        public static DataSet GetList(string strWhere)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select  * FROM whd_withdraw_scheme ");
                if (strWhere.Trim() != "")
                    builder.Append(" where " + strWhere);
                return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static WithdrawSchemeInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_whd_withdraw_scheme_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static WithdrawSchemeInfo GetModelFromDs(DataSet ds)
        {
            WithdrawSchemeInfo model = new WithdrawSchemeInfo();
            if (ds == null || ds.Tables[0].Rows.Count == 0) return model;

            DataRow dr = ds.Tables[0].Rows[0];
            model.ID = Convert.ToInt32(dr["id"]);
            model.Type = Convert.ToInt32(dr["Type"]); ;
            model.SchemeName = Convert.ToString(dr["schemename"]);
            model.SingleMinAmtLimit = Utils.StrToDecimal(dr["SingleMinAmtLimit"], 0);
            model.SingleMaxAmtLimit = Utils.StrToDecimal(dr["SingleMaxAmtLimit"], 0);
            model.DailyMaxTimes = Convert.ToInt32(dr["dailymaxtimes"]);
            model.DailyMaxAmt = Utils.StrToDecimal(dr["DailyMaxAmt"], 0);
            model.ChargeRate = Utils.StrToDecimal(dr["ChargeRate"], 0);
            model.SingleMinCharge = Utils.StrToDecimal(dr["SingleMinCharge"], 0);
            model.SingleMaxCharge = Utils.StrToDecimal(dr["SingleMaxCharge"], 0);
            model.IsTranApi = Convert.ToInt32(dr["IsTranApi"]);
            model.IsDefault = Convert.ToInt32(dr["IsDefault"]); ;
            model.IsSys = Convert.ToInt32(dr["IsSys"]); ;
            model.BankDetentionDays = Convert.ToInt32(dr["bankdetentiondays"]);
            model.QQDetentionDays = Convert.ToInt32(dr["qqdetentiondays"]);
            model.JDDetentionDays = Convert.ToInt32(dr["jddetentiondays"]);
            model.IsTranRequiredAudit = Convert.ToInt32(dr["IsTranRequiredAudit"]); ;
            model.AlipayDetentionDays = Convert.ToInt32(dr["alipaydetentiondays"]);
            model.WeiXinDetentionDays = Convert.ToInt32(dr["weixindetentiondays"]);
            model.OtherDetentionDays = Convert.ToInt32(dr["otherdetentiondays"]);
            model.TranSupplier = Convert.ToInt32(dr["transupplier"]);


            return model;
        }


        /// <summary>
        /// 获取商户的体现方案。这个应该不用了
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static WithdrawSchemeInfo GetModelByUser(int type, int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@type", SqlDbType.TinyInt, 1),
                    new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = type;
                commandParameters[1].Value = userId;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_tocashscheme_GetModelByUser", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        #endregion
    }
}
