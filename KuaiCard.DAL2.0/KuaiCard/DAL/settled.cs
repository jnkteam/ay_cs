namespace KuaiCard.DAL
{
    using DBAccess;
    using KuaiCard.Model;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class settled
    {
        internal const string FIELDS = "[UserName]\r\n      ,[PayeeName]\r\n      ,[Account]\r\n      ,[id]\r\n      ,[userid]\r\n      ,[amount]\r\n      ,[status]\r\n      ,[addTime]\r\n      ,[tax]\r\n      ,ISNULL([charges],0) as charges,[PayTime],[userid],[PayeeBank],[apptype],[required],[settmode],[settles],[tranapi],Payeeaddress\r\n      ,[amount]-isnull([charges],0)-isnull([tax],0) realpay";
        internal const string SQL_TABLE = "V_Settled";

        public bool Allfails()
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

        public bool AllPass(string batchNo)
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

        public bool AllSettle()
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

        public int Add(SettledInfo model)
        {
            try
            {
                SqlParameter parameter = DataBase.MakeOutParam("@id", SqlDbType.Int, 10);
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_add",
                                new SqlParameter[] { parameter, 
                                                        DataBase.MakeInParam("@userid", SqlDbType.Int, 10, model.userid), 
                                                        DataBase.MakeInParam("@amount", SqlDbType.Money, 8, model.amount), 
                                                        DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.status), 
                                                        DataBase.MakeInParam("@addtime", SqlDbType.DateTime, 8, model.addtime), 
                                                        DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, model.paytime), 
                                                        DataBase.MakeInParam("@tax", SqlDbType.Money, 8, model.tax), 
                                                        DataBase.MakeInParam("@charges", SqlDbType.Money, 8, model.charges), 
                                                        DataBase.MakeInParam("@settmode", SqlDbType.TinyInt, 1, model.settmode) 
                                }) == 1)
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

        public int Apply(SettledInfo model)
        {
            try
            {
                //2017.1.15 添加status参数。是否需要审核。
                SqlParameter parameter = DataBase.MakeOutParam("@id", SqlDbType.Int, 10);
                if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_apply", 
                            new SqlParameter[] { parameter, DataBase.MakeInParam("@userid", SqlDbType.Int, 10, model.userid), 
                                                                                DataBase.MakeInParam("@amount", SqlDbType.Decimal, 9, model.amount), 
                                                                                DataBase.MakeInParam("@addtime", SqlDbType.DateTime, 8, model.addtime), 
                                                                                DataBase.MakeInParam("@apptype", SqlDbType.Int, 10, model.AppType), 
                                                                                DataBase.MakeInParam("@required", SqlDbType.DateTime, 8, model.addtime), 
                                                                                DataBase.MakeInParam("@Paytype", SqlDbType.TinyInt, 1, model.Paytype), 
                                                                                DataBase.MakeInParam("@PayeeBank", SqlDbType.VarChar, 50, model.PayeeBank), 
                                                                                DataBase.MakeInParam("@payeeName", SqlDbType.VarChar, 50, model.payeeName), 
                                                                                DataBase.MakeInParam("@Account", SqlDbType.VarChar, 50, model.Account), 
                                                                                DataBase.MakeInParam("@BankAddress", SqlDbType.VarChar, 100, model.Payeeaddress), 
                                                                                DataBase.MakeInParam("@settmode", SqlDbType.TinyInt, 1, model.settmode), 
                                                                                DataBase.MakeInParam("@charges", SqlDbType.Decimal, 9, model.charges), 
                                                                                DataBase.MakeInParam("@tranapi", SqlDbType.Int, 10, model.suppid),
                                                                                DataBase.MakeInParam("@bankcode", SqlDbType.VarChar, 30, model.BankCode),
                                                                                DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.status),}) > 0)
                {
                    return (int) parameter.Value;
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public int Pay(SettledInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    DataBase.MakeInParam("@id", SqlDbType.Int, 10, model.id), 
                    DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.status), 
                    DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, model.paytime), 
                    DataBase.MakeInParam("@tax", SqlDbType.Money, 8, model.tax), 
                    DataBase.MakeInParam("@charges", SqlDbType.Money, 8, model.charges), 
                    DataBase.MakeInParam("@tranapi", SqlDbType.Int, 10, model.suppid), 
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

        public bool Audit(int id, int status)
        {
            try
            {
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_Audit", 
                        new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, id), 
                                                            DataBase.MakeInParam("@status", SqlDbType.Int, 10, status), 
                                                            DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, DateTime.Now) 
                        }));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool AutoSettled(decimal balance)
        {
            try
            {
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "Proc_Sys_AutoSettled", 
                    new SqlParameter[] { DataBase.MakeInParam("@balance", SqlDbType.Decimal, 9, balance) });
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool BatchPass(string ids, string batchNo, out DataTable withdrawListByApi)
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
                return Convert.ToBoolean(commandParameters[2]);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool BatchSettle(string ids)
        {
            try
            {
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_batchsettle",
                    new SqlParameter[] { DataBase.MakeInParam("@ids", SqlDbType.NVarChar, 1000, ids) }));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
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
                            item.Value = (int) param2.ParamValue;
                            paramList.Add(item);
                            break;
                        }
                        case "status":
                        {
                            builder.Append(" AND [status] = @status");
                            SqlParameter parameter2 = new SqlParameter("@status", SqlDbType.Int);
                            parameter2.Value = (int) param2.ParamValue;
                            paramList.Add(parameter2);
                            //KuaiCardLib.Logging.LogHelper.Write("status=" + param2.ParamValue.ToString());
                            break;
                        }
                        case "tranapi":
                        {
                            builder.Append(" AND [tranapi] = @tranapi");
                            SqlParameter parameter3 = new SqlParameter("@tranapi", SqlDbType.Int);
                            parameter3.Value = (int) param2.ParamValue;
                            paramList.Add(parameter3);
                            break;
                        }
                        case "userid":
                        {
                            builder.Append(" AND [userid] = @userid");
                            SqlParameter parameter4 = new SqlParameter("@userid", SqlDbType.Int);
                            parameter4.Value = (int) param2.ParamValue;
                            paramList.Add(parameter4);
                            break;
                        }
                        case "settmode":
                        {
                            builder.Append(" AND [settmode] = @settmode");
                            SqlParameter parameter5 = new SqlParameter("@settmode", SqlDbType.TinyInt);
                            parameter5.Value = (int) param2.ParamValue;
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
                            parameter7.Value = ((string) param2.ParamValue) + "%";
                            paramList.Add(parameter7);
                            break;
                        }
                        case "payeebank":
                        {
                            builder.Append(" AND [PayeeBank] like @PayeeBank");
                            SqlParameter parameter8 = new SqlParameter("@PayeeBank", SqlDbType.VarChar);
                            parameter8.Value = ((string) param2.ParamValue) + "%";
                            paramList.Add(parameter8);
                            break;
                        }
                        case "payeename":
                        {
                            builder.Append(" AND [payeeName] like @payeeName");
                            SqlParameter parameter9 = new SqlParameter("@payeeName", SqlDbType.VarChar);
                            parameter9.Value = ((string) param2.ParamValue) + "%";
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
                            parameter14.Value = (int) param2.ParamValue;
                            paramList.Add(parameter14);
                            break;
                        }
                    }
                }
            }
            //KuaiCardLib.Logging.LogHelper.Write(builder.ToString());
            return builder.ToString();
        }

        /// <summary>
        /// 取消支付接口中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Cancel(int id)
        {
            try
            {
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_cancel", 
                    new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, id) }));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public SettledInfo DataRowToModel(DataRow row)
        {
            SettledInfo info = new SettledInfo();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    info.id = int.Parse(row["id"].ToString());
                }
                if ((row["settmode"] != null) && (row["settmode"].ToString() != ""))
                {
                    info.settmode = (SettledmodeEnum) int.Parse(row["settmode"].ToString());
                }
                if ((row["userid"] != null) && (row["userid"].ToString() != ""))
                {
                    info.userid = int.Parse(row["userid"].ToString());
                }
                if ((row["amount"] != null) && (row["amount"].ToString() != ""))
                {
                    info.amount = decimal.Parse(row["amount"].ToString());
                }
                if ((row["status"] != null) && (row["status"].ToString() != ""))
                {
                    info.status = (SettledStatus) int.Parse(row["status"].ToString());
                }
                if ((row["tranapi"] != null) && (row["tranapi"].ToString() != ""))
                {
                    info.suppid = int.Parse(row["tranapi"].ToString());
                }
                if ((row["addtime"] != null) && (row["addtime"].ToString() != ""))
                {
                    info.addtime = DateTime.Parse(row["addtime"].ToString());
                }
                if ((row["required"] != null) && (row["required"].ToString() != ""))
                {
                    info.required = DateTime.Parse(row["required"].ToString());
                }
                if ((row["paytime"] != null) && (row["paytime"].ToString() != ""))
                {
                    info.paytime = DateTime.Parse(row["paytime"].ToString());
                }
                if ((row["tax"] != null) && (row["tax"].ToString() != ""))
                {
                    info.tax = new decimal?(decimal.Parse(row["tax"].ToString()));
                }
                if ((row["charges"] != null) && (row["charges"].ToString() != ""))
                {
                    info.charges = new decimal?(decimal.Parse(row["charges"].ToString()));
                }
                if ((row["apptype"] != null) && (row["apptype"].ToString() != ""))
                {
                    info.AppType = (AppTypeEnum) int.Parse(row["apptype"].ToString());
                }
                if ((row["Paytype"] != null) && (row["Paytype"].ToString() != ""))
                {
                    info.Paytype = int.Parse(row["Paytype"].ToString());
                }
                if (row["PayeeBank"] != null)
                {
                    info.PayeeBank = row["PayeeBank"].ToString();
                }
                if (row["payeeName"] != null)
                {
                    info.payeeName = row["payeeName"].ToString();
                }
                if (row["Payeeaddress"] != null)
                {
                    info.Payeeaddress = row["Payeeaddress"].ToString();
                }
                if (row["account"] != null)
                {
                    info.Account = row["account"].ToString();
                }
            }
            return info;
        }

        public bool Delete(DateTime etime)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("delete from settled ");
                builder.Append(" where status = 8 and addtime < @etime");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@etime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = etime;
                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public DataTable Export(string ids)
        {
            try
            {
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_settled_Export", new SqlParameter[]
				{
					DataBase.MakeInParam("@ids", SqlDbType.NVarChar, 1000, ids)
				}).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public bool GetListByBatchNo(string batchNo)
        {
            try
            {
                return Convert.ToBoolean(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_allpass", 
                    new SqlParameter[] { DataBase.MakeInParam("@batchNo", SqlDbType.VarChar, 30, batchNo) }));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public DataSet GetListWithdrawByApi(string batchNo)
        {
            try
            {
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_settled_getlist_WithdrawByApi", 
                    new SqlParameter[] { DataBase.MakeInParam("@batchNo", SqlDbType.VarChar, 30, batchNo) });
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public SettledInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@ID", SqlDbType.BigInt, 10, id) };
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

        public decimal GetPayDayMoney(int uid)
        {
            return decimal.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, "SELECT ISNULL(SUM([Amount]*[Pay_Price]),0) FROM [User_Pay_Order] where Status = 2 and datediff(day,CompleteTime,getdate())=0 and UserId=" + uid.ToString()));
        }

        public decimal Getpayingmoney(int uid)
        {
            return decimal.Parse(DataBase.ExecuteScalarToStr(CommandType.Text, "SELECT ISNULL(SUM([Money]),0) FROM [settled] WHERE Status IN(0,1) AND [Uid]=" + uid.ToString()));
        }

        public decimal GetUserDaySettledAmt(int userid, string day)
        {
            try
            {
                return Convert.ToDecimal(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_userdayAmt", 
                    new SqlParameter[] { DataBase.MakeInParam("@userid", SqlDbType.Int, 10, userid), 
                        DataBase.MakeInParam("@day", SqlDbType.VarChar, 20, day) }));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0M;
            }
        }

        public int GetUserDaySettledTimes(int userid, string day)
        {
            try
            {
                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_settled_userdaytimes", 
                    new SqlParameter[] { DataBase.MakeInParam("@userid", SqlDbType.Int, 10, userid), 
                        DataBase.MakeInParam("@day", SqlDbType.VarChar, 20, day) }));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_Settled";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "addTime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();

                string wheres = BuilderWhere(searchParams, paramList);

                return DataBase.ExecuteDataset(CommandType.Text, 
                    SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" +
                    SqlHelper.GetPageSelectSQL("[UserName]\r\n      ,[PayeeName]\r\n      ,[Account]\r\n      ,[id]\r\n      ,[userid]\r\n      ,[amount]\r\n      ,[status]\r\n      ,[addTime]\r\n      ,[tax]\r\n      ,ISNULL([charges],0) as charges,[PayTime],[userid],[PayeeBank],[apptype],[required],[settmode],[settles],[tranapi],Payeeaddress\r\n      ,[amount]-isnull([charges],0)-isnull([tax],0) realpay,trade_no", tables, wheres, orderby, key, pageSize, page, false) 
                    //+ "\r\nselect ISNULL(sum(amount),0) from V_Settled where " + wheres, paramList.ToArray());
                    + "\r\n select sum(1) totalcount,sum(amount) totalamt,sum(case when [status]=8 then 1 else 0 end) successcount,sum(case when [status]=8 then amount else 0 end) successamt,sum(case when [status]=8 then ISNULL([charges],0) else 0 end) successcharges  from V_Settled where " + wheres, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }


        public static SettledInfo ReaderBind(SqlDataReader dataReader)
        {
            SettledInfo info = new SettledInfo();
            object obj2 = dataReader["id"];
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                info.id = (int) obj2;
            }
            object obj3 = dataReader["userid"];
            if ((obj3 != null) && (obj3 != DBNull.Value))
            {
                info.userid = (int) obj3;
            }
            object obj4 = dataReader["amount"];
            if ((obj4 != null) && (obj4 != DBNull.Value))
            {
                info.amount = (decimal) obj4;
            }
            object obj5 = dataReader["status"];
            if ((obj5 != null) && (obj5 != DBNull.Value))
            {
                info.status = (SettledStatus) obj5;
            }
            object obj6 = dataReader["addtime"];
            if ((obj6 != null) && (obj6 != DBNull.Value))
            {
                info.addtime = (DateTime) obj6;
            }
            object obj7 = dataReader["paytime"];
            if ((obj7 != null) && (obj7 != DBNull.Value))
            {
                info.paytime = (DateTime) obj7;
            }
            object obj8 = dataReader["tax"];
            if ((obj8 != null) && (obj8 != DBNull.Value))
            {
                info.tax = new decimal?((decimal) obj8);
            }
            object obj9 = dataReader["charges"];
            if ((obj9 != null) && (obj9 != DBNull.Value))
            {
                info.charges = new decimal?((decimal) obj9);
            }
            object obj10 = dataReader["payeeName"];
            if ((obj10 != null) && (obj10 != DBNull.Value))
            {
                info.payeeName = (string) obj10;
            }
            object obj11 = dataReader["PayeeBank"];
            if ((obj11 != null) && (obj11 != DBNull.Value))
            {
                info.PayeeBank = (string) obj11;
            }
            object obj12 = dataReader["Payeeaddress"];
            if ((obj12 != null) && (obj12 != DBNull.Value))
            {
                info.Payeeaddress = (string) obj12;
            }
            object obj13 = dataReader["Account"];
            if ((obj13 != null) && (obj13 != DBNull.Value))
            {
                info.Account = (string) obj13;
            }
            object obj14 = dataReader["BankCode"];
            if ((obj14 != null) && (obj14 != DBNull.Value))
            {
                info.BankCode = (string) obj14;
            }
            return info;
        }

        public bool Update(SettledInfo model)
        {
            try
            {
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_settled_update", 
                        new SqlParameter[] { 
                            DataBase.MakeInParam("@id", SqlDbType.Int, 10, model.id), 
                            DataBase.MakeInParam("@userid", SqlDbType.Int, 10, model.userid), 
                            DataBase.MakeInParam("@amount", SqlDbType.Money, 8, model.amount), 
                            DataBase.MakeInParam("@status", SqlDbType.Int, 10, model.status), 
                            DataBase.MakeInParam("@addtime", SqlDbType.DateTime, 8, model.addtime), 
                            DataBase.MakeInParam("@paytime", SqlDbType.DateTime, 8, model.paytime), 
                            DataBase.MakeInParam("@tax", SqlDbType.Money, 8, model.tax), 
                            DataBase.MakeInParam("@charges", SqlDbType.Money, 8, model.charges), 
                            DataBase.MakeInParam("@tranapi", SqlDbType.Int, 10, model.suppid) 
                        }) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

