namespace OriginalStudio.BLL.Settled
{
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.DBAccess;
    using System.Data.SqlClient;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// 结算操作类。
    /// </summary>
    public class SettledFactory
    {
        //private static OriginalStudio.DAL.Settled.Settled dal = new DAL.Settled.Settled();

        #region 增删改

        /// <summary>
        /// 提交代付申请。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Apply(SettledInfo model)
        {
            try
            {
                //2017.1.15 添加status参数。是否需要审核。
                SqlParameter parameter = DataBase.MakeOutParam("@id", SqlDbType.Int, 10);
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_apply",
                            new SqlParameter[] { parameter,
                                DataBase.MakeInParam("@userid", SqlDbType.Int, 10, model.UserID),
                                DataBase.MakeInParam("@settleamount", SqlDbType.Money,9, model.SettleAmount),
                                DataBase.MakeInParam("@addtime", SqlDbType.DateTime, 8, model.AddTime),
                                DataBase.MakeInParam("@apptype", SqlDbType.Int, 10, model.AppType),
                                DataBase.MakeInParam("@required", SqlDbType.DateTime, 8, model.AddTime),
                                DataBase.MakeInParam("@Paytype", SqlDbType.TinyInt, 1, model.PayType),
                                DataBase.MakeInParam("@PayeeBank", SqlDbType.VarChar, 50, model.PayeeBank),
                                DataBase.MakeInParam("@payeeName", SqlDbType.VarChar, 50, model.PayeeName),
                                DataBase.MakeInParam("@Account", SqlDbType.VarChar, 50, model.Account),
                                DataBase.MakeInParam("@BankAddress", SqlDbType.VarChar, 100, model.PayeeAddress),
                                DataBase.MakeInParam("@settmode", SqlDbType.TinyInt, 1, model.SettledMode),
                                DataBase.MakeInParam("@charges", SqlDbType.Decimal, 9, model.Charges),
                                DataBase.MakeInParam("@tranapi", SqlDbType.Int, 10, model.Suppid),
                                DataBase.MakeInParam("@bankcode", SqlDbType.VarChar, 30, model.BankCode),
                                DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.Status),}) > 0)
                {
                    return (int)parameter.Value;
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static bool Update(SettledInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, model.ID), 
                    DataBase.MakeInParam("@userid", SqlDbType.Int, 10, model.UserID), 
                    DataBase.MakeInParam("@settleamount", SqlDbType.Money, 8, model.SettleAmount), 
                    DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.Status), 
                    DataBase.MakeInParam("@addtime", SqlDbType.DateTime, 8, model.AddTime), 
                    DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, model.PayTime), 
                    DataBase.MakeInParam("@tax", SqlDbType.Money, 8, model.Tax), 
                    DataBase.MakeInParam("@charges", SqlDbType.Money, 8, model.Charges), 
                    DataBase.MakeInParam("@tranapi", SqlDbType.Int, 10, model.Suppid) 
                };

                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        
        public static bool Cancel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, id)
                };
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_cancel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
        
        public static bool Delete(DateTime etime)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("delete from settled where status = 8 and addtime < @etime");
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@etime", SqlDbType.DateTime, 8) 
                };
                commandParameters[0].Value = etime;
                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        #region 审核之类操作

        public static bool Allfails()
        {
            try
            {
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_fails", null));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool AllPass(string batchNo)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@batchNo", SqlDbType.VarChar, 30, batchNo), 
                    DataBase.MakeOutParam("@result", SqlDbType.Bit, 1) 
                };
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_allpass", commandParameters);
                return Convert.ToBoolean(commandParameters[1].Value);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool AllSettle()
        {
            try
            {
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_allsettle", null);
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool Audit(int id, int status)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, id), 
                    DataBase.MakeInParam("@status", SqlDbType.Int, 10, status), 
                    DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, DateTime.Now) 
                };
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_Audit", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool AutoSettled(decimal balance)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[]
                {
                    DataBase.MakeInParam("@balance", SqlDbType.Decimal, 9, balance)
                };
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "Proc_Sys_AutoSettled", commandParameters);
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool BatchPass(string ids, string batchNo, out DataTable withdrawListByApi)
        {
            withdrawListByApi = null;
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@ids", SqlDbType.NVarChar, 1000, ids), 
                    DataBase.MakeInParam("@batchNo", SqlDbType.VarChar, 30, batchNo), 
                    DataBase.MakeOutParam("@result", SqlDbType.Bit, 1) 
                };
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_settled_batchpass", commandParameters);
                if ((set != null) && (set.Tables.Count > 0))
                {
                    withdrawListByApi = set.Tables[0];
                }
                return Convert.ToBoolean(commandParameters[2].Value);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool BatchSettle(string ids)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@ids", SqlDbType.NVarChar, 1000, ids)
                };
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_batchsettle",commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        #endregion

        #region 执行付款

        /// <summary>
        /// 执行代付付款。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Pay(SettledInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, model.ID), 
                    DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.Status), 
                    DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, model.PayTime), 
                    DataBase.MakeInParam("@tax", SqlDbType.Money, 8, model.Tax), 
                    DataBase.MakeInParam("@charges", SqlDbType.Money, 8, model.Charges), 
                    DataBase.MakeInParam("@tranapi", SqlDbType.Int, 10, model.Suppid), 
                    DataBase.MakeOutParam("@result", SqlDbType.TinyInt, 1) 
                };
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_pay", commandParameters);
                return Convert.ToInt32(commandParameters[6].Value);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 99;
            }
        }

        #endregion

        #region 获取对象
        
        public static SettledInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@ID", SqlDbType.BigInt, 10, id)
                };
                SettledInfo info = null;
                using (SqlDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "proc_settled_GetModel", commandParameters))
                {
                    if (reader.Read())
                    {
                        info = ReaderBind(reader);
                    }
                }
                return info;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        #endregion

        public static DataTable Export(string ids)
        {
            try
            {
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_settled_Export", 
                    new SqlParameter[]{
                        DataBase.MakeInParam("@ids", SqlDbType.NVarChar, 1000, ids)	
                    }).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataTable GetListWithdrawByApi(string batchNo)
        {
            try
            {
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_settled_getlist_WithdrawByApi",
                    new SqlParameter[] { 
                        DataBase.MakeInParam("@batchNo", SqlDbType.VarChar, 30, batchNo) 
                    }).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static decimal GetPayDayMoney(int uid)
        {
            try
            {
                return decimal.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, 
                    "SELECT ISNULL(SUM([Amount]*[Pay_Price]),0) FROM [User_Pay_Order] where Status = 2 and datediff(day,CompleteTime,getdate())=0 and UserId=" + uid.ToString()));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static decimal Getpayingmoney(int uid)
        {
            try
            {
                return decimal.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, 
                    "SELECT ISNULL(SUM([Money]),0) FROM [settled] WHERE Status IN(0,1) AND [Uid]=" + uid.ToString()));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public static string GetSettleBankName(string code)
        {
            string str = code;
            return str;
        }


        #region 提现操作

        /// <summary>
        /// 获取商户体现总额
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static decimal GetUserDaySettledAmt(int userid, string day)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    DataBase.MakeInParam("@userid", SqlDbType.Int, 10, userid),
                    DataBase.MakeInParam("@day", SqlDbType.VarChar, 20, day)
                };

                return Convert.ToDecimal(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_userdayAmt", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        /// <summary>
        /// 商户指定某天提现次数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetUserDaySettledTimes(int userid, string day)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@userid", SqlDbType.Int, 10, userid), 
                    DataBase.MakeInParam("@day", SqlDbType.VarChar, 20, day)
                };

                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_userdaytimes", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        #endregion

        #region 查询操作

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_settled";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "addTime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();

                string wheres = BuilderWhere(searchParams, paramList);

                return DataBase.ExecuteDataset(CommandType.Text,
                        SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" +
                        SqlHelper.GetPageSelectSQL("*", tables, wheres, orderby, key, pageSize, page, false) + "\r\n" +
                        "select sum(1) totalcount," +
                        "sum(amount) totalamt," +
                        "sum(case when [status]=8 then 1 else 0 end) successcount," +
                        "sum(case when [status]=8 then amount else 0 end) successamt," +
                        "sum(case when [status]=8 then settleamount else 0 end) success_settleamt," +
                        "sum(case when [status]=8 then ISNULL([charges],0) else 0 end) successcharges  " +
                        " from V_Settled where " + wheres, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static List<SettledInfo> DataTableToList(DataTable dt)
        {
            List<SettledInfo> list = new List<SettledInfo>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    SettledInfo item = DataRowToModel(dt.Rows[i]);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        
        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SearchParam param2 = param[i];
                    switch (param2.ParamKey.Trim().ToLower())
                    {
                        case "id":
                            {
                                builder.Append(" AND [id] = @id");
                                SqlParameter item = new SqlParameter("@id", SqlDbType.Int);
                                item.Value = (int)param2.ParamValue;
                                paramList.Add(item);
                                break;
                            }
                        case "status":
                            {
                                builder.Append(" AND [status] = @status");
                                SqlParameter parameter2 = new SqlParameter("@status", SqlDbType.Int);
                                parameter2.Value = (int)param2.ParamValue;
                                paramList.Add(parameter2);
                                //OriginalStudio.Lib.Logging.LogHelper.Write("status=" + param2.ParamValue.ToString());
                                break;
                            }
                        case "tranapi":
                            {
                                builder.Append(" AND [tranapi] = @tranapi");
                                SqlParameter parameter3 = new SqlParameter("@tranapi", SqlDbType.Int);
                                parameter3.Value = (int)param2.ParamValue;
                                paramList.Add(parameter3);
                                break;
                            }
                        case "userid":
                            {
                                builder.Append(" AND [userid] = @userid");
                                SqlParameter parameter4 = new SqlParameter("@userid", SqlDbType.Int);
                                parameter4.Value = (int)param2.ParamValue;
                                paramList.Add(parameter4);
                                break;
                            }
                        case "settmode":
                            {
                                //OriginalStudio.Lib.Logging.LogHelper.Write("param2.ParamValue:" + param2.ParamValue.ToString());
                                builder.Append(" AND [settmode] = @settmode");
                                SqlParameter parameter5 = new SqlParameter("@settmode", SqlDbType.TinyInt);
                                parameter5.Value = (int)param2.ParamValue;
                                paramList.Add(parameter5);
                                break;
                            }
                        case "username":
                            {
                                builder.Append(" AND [UserName] like @UserName");
                                SqlParameter parameter6 = new SqlParameter("@UserName", SqlDbType.VarChar);
                                parameter6.Value = "%" + param2.ParamValue + "%";
                                paramList.Add(parameter6);
                                break;
                            }
                        case "account":
                            {
                                builder.Append(" AND [account] like @account");
                                SqlParameter parameter7 = new SqlParameter("@account", SqlDbType.VarChar);
                                parameter7.Value = ((string)param2.ParamValue) + "%";
                                paramList.Add(parameter7);
                                break;
                            }
                        case "payeebank":
                            {
                                builder.Append(" AND [PayeeBank] like @PayeeBank");
                                SqlParameter parameter8 = new SqlParameter("@PayeeBank", SqlDbType.VarChar);
                                parameter8.Value = ((string)param2.ParamValue) + "%";
                                paramList.Add(parameter8);
                                break;
                            }
                        case "payeename":
                            {
                                builder.Append(" AND [payeeName] like @payeeName");
                                SqlParameter parameter9 = new SqlParameter("@payeeName", SqlDbType.VarChar);
                                parameter9.Value = ((string)param2.ParamValue) + "%";
                                paramList.Add(parameter9);
                                break;
                            }
                        case "begindate":
                            {
                                builder.Append(" AND [paytime] >= @beginpaytime");
                                SqlParameter parameter10 = new SqlParameter("@beginpaytime", SqlDbType.DateTime);
                                parameter10.Value = param2.ParamValue;
                                paramList.Add(parameter10);
                                break;
                            }
                        case "enddate":
                            {
                                builder.Append(" AND [paytime] <= @endpaytime");
                                SqlParameter parameter11 = new SqlParameter("@endpaytime", SqlDbType.DateTime);
                                parameter11.Value = param2.ParamValue;
                                paramList.Add(parameter11);
                                break;
                            }
                        case "saddtime":
                            {
                                builder.Append(" AND [addTime] >= @saddtime");
                                SqlParameter parameter12 = new SqlParameter("@saddtime", SqlDbType.DateTime);
                                parameter12.Value = param2.ParamValue;
                                paramList.Add(parameter12);
                                break;
                            }
                        case "eaddtime":
                            {
                                builder.Append(" AND [addTime] <= @eaddtime");
                                SqlParameter parameter13 = new SqlParameter("@eaddtime", SqlDbType.DateTime);
                                parameter13.Value = param2.ParamValue;
                                paramList.Add(parameter13);
                                break;
                            }
                        case "trade_no":
                            {
                                builder.Append(" AND [trade_no] = @trade_no");
                                SqlParameter parameter9 = new SqlParameter("@trade_no", SqlDbType.VarChar);
                                parameter9.Value = ((string)param2.ParamValue);
                                paramList.Add(parameter9);
                                break;
                            }
                        case "all":
                            {
                                builder.Append(" AND ([userid] = @id or [id] = @id)");
                                SqlParameter parameter14 = new SqlParameter("@id", SqlDbType.Int);
                                parameter14.Value = (int)param2.ParamValue;
                                paramList.Add(parameter14);
                                break;
                            }
                        case "merchantname":
                            {
                                builder.Append(" AND [MerchantName] = @MerchantName");
                                SqlParameter parameter15 = new SqlParameter("@MerchantName", SqlDbType.VarChar);
                                parameter15.Value = ((string)param2.ParamValue);
                                paramList.Add(parameter15);
                                break;
                            }
                    }
                }
            }
            //OriginalStudio.Lib.Logging.LogHelper.Write(builder.ToString());
            return builder.ToString();
        }
        
        #endregion

        #region DataRow转为对象

        public static SettledInfo DataRowToModel(DataRow row)
        {
            SettledInfo info = new SettledInfo();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    info.ID = int.Parse(row["id"].ToString());
                }
                if ((row["settmode"] != null) && (row["settmode"].ToString() != ""))
                {
                    info.SettledMode = (SettledModeEnum)int.Parse(row["settmode"].ToString());
                }
                if ((row["userid"] != null) && (row["userid"].ToString() != ""))
                {
                    info.UserID = int.Parse(row["userid"].ToString());
                }
                if ((row["amount"] != null) && (row["amount"].ToString() != ""))
                {
                    info.Amount = decimal.Parse(row["amount"].ToString());
                }
                if ((row["settleamount"] != null) && (row["settleamount"].ToString() != ""))
                {
                    info.SettleAmount = decimal.Parse(row["settleamount"].ToString());
                }
                if ((row["status"] != null) && (row["status"].ToString() != ""))
                {
                    info.Status = (SettledStatusEnum)int.Parse(row["status"].ToString());
                }
                if ((row["tranapi"] != null) && (row["tranapi"].ToString() != ""))
                {
                    info.Suppid = int.Parse(row["tranapi"].ToString());
                }
                if ((row["addtime"] != null) && (row["addtime"].ToString() != ""))
                {
                    info.AddTime = DateTime.Parse(row["addtime"].ToString());
                }
                if ((row["required"] != null) && (row["required"].ToString() != ""))
                {
                    info.Required = DateTime.Parse(row["required"].ToString());
                }
                if ((row["paytime"] != null) && (row["paytime"].ToString() != ""))
                {
                    info.PayTime = DateTime.Parse(row["paytime"].ToString());
                }
                if ((row["tax"] != null) && (row["tax"].ToString() != ""))
                {
                    info.Tax = decimal.Parse(row["tax"].ToString());
                }
                if ((row["charges"] != null) && (row["charges"].ToString() != ""))
                {
                    info.Charges = decimal.Parse(row["charges"].ToString());
                }
                if ((row["apptype"] != null) && (row["apptype"].ToString() != ""))
                {
                    info.AppType = (AppTypeEnum)int.Parse(row["apptype"].ToString());
                }
                if ((row["Paytype"] != null) && (row["Paytype"].ToString() != ""))
                {
                    info.PayType = (SettlePayTypeEnum)int.Parse(row["Paytype"].ToString());
                }
                if (row["PayeeBank"] != null)
                {
                    info.PayeeBank = row["PayeeBank"].ToString();
                }
                if (row["payeeName"] != null)
                {
                    info.PayeeName = row["payeeName"].ToString();
                }
                if (row["Payeeaddress"] != null)
                {
                    info.PayeeAddress = row["Payeeaddress"].ToString();
                }
                if (row["account"] != null)
                {
                    info.Account = row["account"].ToString();
                }
            }
            return info;
        }

        public static SettledInfo ReaderBind(SqlDataReader dataReader)
        {
            SettledInfo info = new SettledInfo();
            object obj2 = dataReader["id"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.ID = (int)obj2;
            }
            object obj3 = dataReader["userid"];
            if ((obj3 != null) && (obj3 != DBNull.Value))
            {
                info.UserID = (int)obj3;
            }
            object obj4 = dataReader["amount"];
            if ((obj4 != null) && (obj4 != DBNull.Value))
            {
                info.Amount = (decimal)obj4;
            }
            object obj44 = dataReader["settleamount"];
            if ((obj44 != null) && (obj44 != DBNull.Value))
            {
                info.SettleAmount = (decimal)obj44;
            }
            object obj5 = dataReader["status"];
            if ((obj5 != null) && (obj5 != DBNull.Value))
            {
                info.Status = (SettledStatusEnum)obj5;
            }
            object obj6 = dataReader["addtime"];
            if ((obj6 != null) && (obj6 != DBNull.Value))
            {
                info.AddTime = (DateTime)obj6;
            }
            object obj7 = dataReader["paytime"];
            if ((obj7 != null) && (obj7 != DBNull.Value))
            {
                info.PayTime = (DateTime)obj7;
            }
            object obj8 = dataReader["tax"];
            if ((obj8 != null) && (obj8 != DBNull.Value))
            {
                info.Tax = (decimal)obj8;
            }
            object obj9 = dataReader["charges"];
            if ((obj9 != null) && (obj9 != DBNull.Value))
            {
                info.Charges = (decimal)obj9;
            }
            object obj10 = dataReader["payeeName"];
            if ((obj10 != null) && (obj10 != DBNull.Value))
            {
                info.PayeeName = (string)obj10;
            }
            object obj11 = dataReader["PayeeBank"];
            if ((obj11 != null) && (obj11 != DBNull.Value))
            {
                info.PayeeBank = (string)obj11;
            }
            object obj12 = dataReader["Payeeaddress"];
            if ((obj12 != null) && (obj12 != DBNull.Value))
            {
                info.PayeeAddress = (string)obj12;
            }
            object obj13 = dataReader["Account"];
            if ((obj13 != null) && (obj13 != DBNull.Value))
            {
                info.Account = (string)obj13;
            }
            object obj14 = dataReader["BankCode"];
            if ((obj14 != null) && (obj14 != DBNull.Value))
            {
                info.BankCode = (string)obj14;
            }
            object obj15 = dataReader["tranapi"];
            if ((obj15 != null) && (obj15 != DBNull.Value))
            {
                info.Suppid = (int)obj15;
            }
            return info;
        }

        #endregion

        #region 调用结算接口

        public static string InvokeSettleInterface(string p_merchantName, string p_bankcode, 
                                                                        string p_account, string p_bankname ,
                                                                        string p_payeename, SettlePayTypeEnum p_paytype,
                                                                        SettledModeEnum p_mode,
                                                                        decimal p_money,
                                                                        decimal p_charge,
                                                                        decimal p_tax,
                                                                        string p_notifyurl)
        {
            //组织参数即可
            SortedDictionary<string, string> waitSign = new SortedDictionary<string, string>();
            waitSign.Add("money", p_money.ToString());
            waitSign.Add("charge", p_charge.ToString());
            waitSign.Add("tax", p_tax.ToString());
            waitSign.Add("merchant", p_merchantName.ToString());
            waitSign.Add("bankcode", p_bankcode.ToString());
            waitSign.Add("bankname", p_bankname.ToString());
            waitSign.Add("payeename", p_payeename.ToString());
            waitSign.Add("account", p_account.ToString());
            waitSign.Add("notifyurl", p_notifyurl.ToString());
            waitSign.Add("paytype", ((int)p_paytype).ToString());
            waitSign.Add("mode", ((int)p_mode).ToString());

            Model.User.MchUserBaseInfo mch = OriginalStudio.BLL.Settled.MchUserFactory.GetUserBaseByMerchantName(p_merchantName);
            string sign = Lib.Security.Cryptography.SignSortedDictionary(waitSign, mch.ApiKey).ToLower();
            waitSign.Add("sign", sign.ToString());

            string tmpPostParm = "";
            foreach (var kk in waitSign.Keys)
            {
                tmpPostParm += kk + "=" + waitSign[kk] + "&";
            }
            tmpPostParm = tmpPostParm.Substring(0, tmpPostParm.Length - 1);

            string url = Lib.SysConfig.RuntimeSetting.GateWayServer + "PostDistributionHandler.ashx?" + tmpPostParm;

            //Lib.Logging.LogHelper.Write("代付地址：" + url);

            string msg = OriginalStudio.Lib.Web.WebClientHelper.GetString(url, string.Empty, "GET", Encoding.UTF8, 5 * 1000);

            return msg;
        }

        public static void DoNotify(string trade_no)
        {
            OriginalStudio.Model.Settled.SettledInfo model = OriginalStudio.BLL.Settled.SettledFactory.GetModel(0);
            if (model != null)
            {
                SettledNotifyHelper helper = new SettledNotifyHelper();
                helper.SettleModel = model;
                new Thread(new ThreadStart(helper.DoNotify)).Start();
            }
        }

        #endregion
    }
}

