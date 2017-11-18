namespace OriginalStudio.SQLServerDAL
{
    using DBAccess;
    using OriginalStudio.IDAL;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.IO;
    using OriginalStudio.Lib.Utils;

    public class OrderBank : IOrderBank
    {
        internal const string FIELDS = "";//"[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName],[commission],[notifytime],[version]\r\n      ,cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid";
        internal const string SQL_TABLE = "v_orderbank_list";

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    string str2;
                    SearchParam param2 = param[i];
                    if (param2.CmpOperator == "=")
                    {
                        str2 = param2.ParamKey.Trim().ToLower();
                        switch (str2)
                        {
                            case "userid":
                            {
                                builder.Append(" AND [userid] = @userid");
                                SqlParameter item = new SqlParameter("@userid", SqlDbType.Int);
                                item.Value = (int) param2.ParamValue;
                                paramList.Add(item);
                                break;
                            }
                            case "agentid":
                            {
                                builder.Append(" AND [agentid] = @agentid");
                                SqlParameter parameter2 = new SqlParameter("@agentid", SqlDbType.Int);
                                parameter2.Value = (int) param2.ParamValue;
                                paramList.Add(parameter2);
                                break;
                            }
                            case "manageid":
                            {
                                builder.Append(" AND [manageId] = @manageId");
                                SqlParameter parameter3 = new SqlParameter("@manageId", SqlDbType.Int);
                                parameter3.Value = (int) param2.ParamValue;
                                paramList.Add(parameter3);
                                break;
                            }
                            case "typeid":
                            {
                                builder.Append(" AND [typeId] = @typeId");
                                SqlParameter parameter4 = new SqlParameter("@typeId", SqlDbType.Int);
                                parameter4.Value = (int) param2.ParamValue;
                                paramList.Add(parameter4);
                                break;
                            }
                            case "supplierid":
                            {
                                builder.Append(" AND [supplierId] = @supplierId");
                                SqlParameter parameter5 = new SqlParameter("@supplierId", SqlDbType.Int);
                                parameter5.Value = (int) param2.ParamValue;
                                paramList.Add(parameter5);
                                break;
                            }
                            case "userorder":
                            {
                                builder.Append(" AND [userorder] like @userorder");
                                SqlParameter parameter6 = new SqlParameter("@userorder", SqlDbType.VarChar, 80);
                                parameter6.Value = "%" + SqlHelper.CleanString((string)param2.ParamValue, 80) + "%";
                                paramList.Add(parameter6);
                                break;
                            }
                            case "orderid":
                            {
                                builder.Append(" AND [orderid] like @orderid");
                                SqlParameter parameter7 = new SqlParameter("@orderid", SqlDbType.VarChar, 80);
                                parameter7.Value = "%" + SqlHelper.CleanString((string)param2.ParamValue, 80) + "%";
                                paramList.Add(parameter7);
                                break;
                            }
                            case "supplierorder":
                            {
                                builder.Append(" AND [supplierOrder] like @supplierOrder");
                                SqlParameter parameter8 = new SqlParameter("@supplierOrder", SqlDbType.VarChar, 100);
                                parameter8.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 100) + "%";
                                paramList.Add(parameter8);
                                break;
                            }
                            case "orderid_like":
                            {
                                builder.Append(" AND [orderid] like @orderid");
                                SqlParameter parameter9 = new SqlParameter("@orderid", SqlDbType.VarChar, 100);
                                parameter9.Value = SqlHelper.CleanString((string) param2.ParamValue, 100) + "%";
                                paramList.Add(parameter9);
                                break;
                            }
                            case "status":
                            {
                                builder.Append(" AND [status] = @status");
                                SqlParameter parameter10 = new SqlParameter("@status", SqlDbType.TinyInt);
                                parameter10.Value = (int) param2.ParamValue;
                                paramList.Add(parameter10);
                                break;
                            }
                            case "statusallfail":
                                //builder.Append(" AND ([status] = 4 or  [status] = 8)");
                                builder.Append(" AND ([status] = 4)");  //2017.2.12 =8的情况去掉！！！！！
                                break;

                            case "notifystat":
                            {
                                builder.Append(" AND [notifystat] = @notifystat");
                                SqlParameter parameter11 = new SqlParameter("@notifystat", SqlDbType.TinyInt);
                                parameter11.Value = (int) param2.ParamValue;
                                paramList.Add(parameter11);
                                break;
                            }
                            case "promid":
                            {
                                builder.Append(" AND exists(select 0 from PromotionUser where PromotionUser.PID = @promid and PromotionUser.RegId=userid)");
                                SqlParameter parameter12 = new SqlParameter("@promid", SqlDbType.Int);
                                parameter12.Value = (int) param2.ParamValue;
                                paramList.Add(parameter12);
                                break;
                            }
                            case "stime":
                            {
                                builder.Append(" AND [processingtime] >= @stime");
                                //builder.Append(" AND [addtime] >= @stime");
                                SqlParameter parameter13 = new SqlParameter("@stime", SqlDbType.DateTime);
                                parameter13.Value = param2.ParamValue;
                                paramList.Add(parameter13);
                                break;
                            }
                            case "etime":
                            {
                                builder.Append(" AND [processingtime] <= @etime");
                                //builder.Append(" AND [addtime] <= @etime");
                                SqlParameter parameter14 = new SqlParameter("@etime", SqlDbType.DateTime);
                                parameter14.Value = param2.ParamValue;
                                paramList.Add(parameter14);
                                break;
                            }
                        }
                    }
                    else
                    {
                        str2 = param2.ParamKey.Trim().ToLower();
                        if ((str2 != null) && (str2 == "status"))
                        {
                            builder.AppendFormat(" AND [status] {0} @status1", param2.CmpOperator);
                            SqlParameter parameter15 = new SqlParameter("@status1", SqlDbType.TinyInt);
                            parameter15.Value = (int) param2.ParamValue;
                            paramList.Add(parameter15);
                        }
                    }
                }
            }
            return builder.ToString();
        }

        #region 订单完成操作

        /// <summary>
        /// 处置订单支付状态。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Complete(OrderBankInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                new SqlParameter("@userId", SqlDbType.Int), 
                new SqlParameter("@status", SqlDbType.TinyInt), 
                new SqlParameter("@supplierOrder", SqlDbType.VarChar, 50), 
                new SqlParameter("@realvalue", SqlDbType.Decimal, 9), 
                new SqlParameter("@payRate", SqlDbType.Decimal, 9), 
                new SqlParameter("@supplierRate", SqlDbType.Decimal, 9), 
                new SqlParameter("@payAmt", SqlDbType.Decimal, 9), 
                new SqlParameter("@supplierAmt", SqlDbType.Decimal, 9), 
                new SqlParameter("@profits", SqlDbType.Decimal, 9), 
                new SqlParameter("@addtime", SqlDbType.DateTime), 
                new SqlParameter("@completetime", SqlDbType.DateTime), 
                new SqlParameter("@manageId", SqlDbType.Int), 
                new SqlParameter("@promRate", SqlDbType.Decimal, 9), 
                new SqlParameter("@promAmt", SqlDbType.Decimal, 9), 
                new SqlParameter("@promId", SqlDbType.Int),
                new SqlParameter("@opstate", SqlDbType.VarChar, 30)
            };
            commandParameters[0].Value = model.orderid;
            commandParameters[1].Value = model.userid;
            commandParameters[2].Value = model.status;
            commandParameters[3].Value = model.supplierOrder;
            commandParameters[4].Value = model.realvalue.Value;
            commandParameters[5].Value = model.payRate;
            commandParameters[6].Value = model.supplierRate;
            commandParameters[7].Value = model.payAmt;
            commandParameters[8].Value = model.supplierAmt;
            commandParameters[9].Value = model.profits;
            commandParameters[10].Value = DateTime.Now;
            commandParameters[11].Value = model.completetime;
            commandParameters[12].Value = model.manageId;
            commandParameters[13].Value = model.promRate;
            commandParameters[14].Value = model.promAmt;
            commandParameters[15].Value = model.agentId;
            commandParameters[16].Value = model.opstate;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_settled", commandParameters);
            return true;
        }

        #endregion

        #region 扣单及还单

        /// <summary>
        /// 扣单
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool Deduct(string orderid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                new SqlParameter("@result", SqlDbType.Bit) 
            };
            commandParameters[0].Value = orderid;
            commandParameters[1].Direction = ParameterDirection.Output;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_deduct", commandParameters);
            return (bool) commandParameters[1].Value;
        }
        
        /// <summary>
        /// 还单
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool ReDeduct(string orderid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                new SqlParameter("@result", SqlDbType.Bit) 
            };
            commandParameters[0].Value = orderid;
            commandParameters[1].Direction = ParameterDirection.Output;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_rededuct", commandParameters);
            return (bool)commandParameters[1].Value;
        }

        #endregion

        #region 获取订单详情

        public OrderBankInfo GetModel(long id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.BigInt), 
                new SqlParameter("@userid", SqlDbType.Int) 
            };
            commandParameters[0].Value = id;
            commandParameters[1].Value = DBNull.Value;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_GetModelByID", commandParameters));
        }

        public OrderBankInfo GetModel(string orderId)
        {
            SqlParameter[] commandParameters = new SqlParameter[]
            { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 50)
            };
            commandParameters[0].Value = orderId;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_GetModelByOrderID", commandParameters));
        }

        public OrderBankInfo GetModel(long id, int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.BigInt), 
                new SqlParameter("@userid", SqlDbType.Int) 
            };
            commandParameters[0].Value = id;
            commandParameters[1].Value = userid;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_GetModelByID", commandParameters));
        }
        
        /// <summary>
        /// DS转为订单对象。
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        internal OrderBankInfo GetModelFromDs(DataSet ds)
        {
            OrderBankInfo info = new OrderBankInfo();
            if (ds.Tables[0].Rows.Count == 0) return null;

            DataRow dr = ds.Tables[0].Rows[0];


            info.id = Utils.StrToLong(dr["id"].ToString(), 0);
            info.orderid = dr["orderid"].ToString();
            info.ordertype = Utils.StrToInt(dr["ordertype"].ToString(), 0);
            info.userid = Utils.StrToInt(dr["userid"].ToString(), 0);
            info.channeltypeId = Utils.StrToInt(dr["channeltypeId"].ToString(), 0);
            info.channelcode = dr["channelcode"].ToString();
            info.userorder = dr["userorder"].ToString();
            info.refervalue = Utils.StrToDecimal(dr["refervalue"].ToString(), 0);
            info.realvalue = Utils.StrToDecimal(dr["realvalue"].ToString(), 0);
            info.notifyurl = dr["notifyurl"].ToString();
            info.againNotifyUrl = dr["againNotifyUrl"].ToString();
            info.notifycount = Utils.StrToInt(dr["notifycount"].ToString(), 0);
            info.notifystat = Utils.StrToInt(dr["notifystat"].ToString(), 0);
            info.notifycontext = dr["notifycontext"].ToString();
            info.returnurl = dr["returnurl"].ToString();
            info.attach = dr["attach"].ToString();
            info.payerip = dr["payerip"].ToString();
            info.clientip = dr["clientip"].ToString();
            info.referUrl = dr["referUrl"].ToString();
            info.addtime = Utils.StrToDateTime(dr["addtime"].ToString());
            info.supplierId = Utils.StrToInt(dr["supplierID"].ToString(), 0);
            info.supplierOrder = dr["supplierOrder"].ToString();
            info.status = Utils.StrToInt(dr["status"].ToString(), 0);
            info.completetime = Utils.StrToDateTime(dr["completetime"].ToString());
            info.payRate = Utils.StrToDecimal(dr["payRate"].ToString(), 0);
            info.supplierRate = Utils.StrToDecimal(dr["supplierRate"].ToString(), 0);
            info.promRate = Utils.StrToDecimal(dr["promRate"].ToString(), 0);
            info.payAmt = Utils.StrToDecimal(dr["payAmt"].ToString(), 0);
            info.promAmt = Utils.StrToDecimal(dr["promAmt"].ToString(), 0);
            info.supplierAmt = Utils.StrToDecimal(dr["supplierAmt"].ToString(), 0);
            info.profits = Utils.StrToDecimal(dr["profits"].ToString(), 0);
            info.server = Utils.StrToInt(dr["server"].ToString(), 0);
            info.manageId = Utils.StrToInt(dr["manageId"].ToString(), 0);
            info.agentId = Utils.StrToInt(dr["agentid"].ToString(), 0);
            info.commission = Utils.StrToDecimal(dr["commission"].ToString(), 0);
            info.version = dr["version"].ToString();
            info.cus_subject = dr["cus_subject"].ToString();
            info.cus_price = dr["cus_price"].ToString();
            info.cus_quantity = dr["cus_quantity"].ToString();
            info.cus_description = dr["cus_description"].ToString();
            info.cus_field1 = dr["cus_field1"].ToString();
            info.cus_field2 = dr["cus_field2"].ToString();
            info.cus_field3 = dr["cus_field3"].ToString();
            info.cus_field4 = dr["cus_field4"].ToString();
            info.cus_field5 = dr["cus_field5"].ToString();
            info.opstate = dr["opstate"].ToString();
            return info;
        }

        #endregion

        #region 插入订单

        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Insert(OrderBankInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.BigInt, 8), 
                new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                new SqlParameter("@ordertype", SqlDbType.TinyInt, 1), 
                new SqlParameter("@userid", SqlDbType.Int, 10), 
                new SqlParameter("@channeltypeId", SqlDbType.Int), 
                new SqlParameter("@channelcode", SqlDbType.VarChar, 10), 
                new SqlParameter("@userorder", SqlDbType.VarChar, 30), 
                new SqlParameter("@refervalue", SqlDbType.Decimal, 9), 
                new SqlParameter("@notifyurl", SqlDbType.VarChar, 500), 
                new SqlParameter("@returnurl", SqlDbType.VarChar, 500), 
                new SqlParameter("@attach", SqlDbType.VarChar, 500), 
                new SqlParameter("@payerip", SqlDbType.VarChar, 20), 
                new SqlParameter("@clientip", SqlDbType.VarChar, 20), 
                new SqlParameter("@referUrl", SqlDbType.VarChar, 200), 
                new SqlParameter("@addtime", SqlDbType.DateTime), 
                new SqlParameter("@supplierId", SqlDbType.Int, 10), 
                new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                new SqlParameter("@server", SqlDbType.Int), 
                new SqlParameter("@manageId", SqlDbType.Int), 
                new SqlParameter("@version", SqlDbType.VarChar, 10), 
                new SqlParameter("@cus_subject", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_price", SqlDbType.NVarChar, 50), 
                new SqlParameter("@cus_quantity", SqlDbType.NVarChar, 50), 
                new SqlParameter("@cus_description", SqlDbType.NVarChar, 1000), 
                new SqlParameter("@cus_field1", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_field2", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_field3", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_field4", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_field5", SqlDbType.NVarChar, 100), 
                new SqlParameter("@agentId", SqlDbType.Int, 10)
             };
            commandParameters[0].Direction = ParameterDirection.Output;
            commandParameters[1].Value = model.orderid;
            commandParameters[2].Value = model.ordertype;
            commandParameters[3].Value = model.userid;
            commandParameters[4].Value = model.channeltypeId;
            commandParameters[5].Value = model.channelcode;
            commandParameters[6].Value = model.userorder;
            commandParameters[7].Value = model.refervalue;
            commandParameters[8].Value = model.notifyurl;
            commandParameters[9].Value = model.returnurl;
            commandParameters[10].Value = model.attach;
            commandParameters[11].Value = model.payerip;
            commandParameters[12].Value = model.clientip;
            commandParameters[13].Value = model.referUrl;
            commandParameters[14].Value = model.addtime;
            commandParameters[15].Value = model.supplierId;
            commandParameters[16].Value = model.status;
            commandParameters[17].Value = model.server;
            commandParameters[18].Value = model.manageId;
            commandParameters[19].Value = model.version;
            commandParameters[20].Value = model.cus_subject;
            commandParameters[21].Value = model.cus_price;
            commandParameters[22].Value = model.cus_quantity;
            commandParameters[23].Value = model.cus_description;
            commandParameters[24].Value = model.cus_field1;
            commandParameters[25].Value = model.cus_field2;
            commandParameters[26].Value = model.cus_field3;
            commandParameters[27].Value = model.cus_field4;
            commandParameters[28].Value = model.cus_field5;
            commandParameters[29].Value = model.agentId;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_add", commandParameters);
            return (long) commandParameters[0].Value;
        }

        #endregion

        #region 通知下游

        /// <summary>
        /// 记录异步通知内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Notify(OrderBankInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                new SqlParameter("@againNotifyUrl", SqlDbType.VarChar, 0x7d0), 
                new SqlParameter("@notifycount", SqlDbType.Int, 10), 
                new SqlParameter("@notifystat", SqlDbType.TinyInt, 1), 
                new SqlParameter("@notifycontext", SqlDbType.VarChar, 200), 
                new SqlParameter("@notifytime", SqlDbType.DateTime) 
            };
            commandParameters[0].Value = model.orderid;
            commandParameters[1].Value = model.againNotifyUrl;
            commandParameters[2].Value = model.notifycount;
            commandParameters[3].Value = model.notifystat;
            commandParameters[4].Value = model.notifycontext;
            commandParameters[5].Value = model.notifytime;
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_notify", commandParameters) > 0);
        }

        #endregion

        #region 订单查询统计

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_orderbank";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n"
                    + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName],[commission],[notifytime],[version]\r\n      ,cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid,ipaddress", tables, wheres, orderby, key, pageSize, page, false)
                    + "\r\nselect sum(1) ordtotal,sum(case when [status]=2 then 1 else 0 end) succordtotal,sum(refervalue) refervalue,sum(case when [status]=2 then realvalue else 0 end) realvalue,sum(isnull(promAmt,0)) promAmt,sum(case when [status]=2 then payAmt else 0 end) payAmt,sum(case when [status]=2 then supplierAmt-payAmt-promAmt else 0 end) profits,sum(promAmt) promAmt,sum(commission) commission from " + tables + " where " + wheres, paramList.ToArray());
                    //上面这句话不能随意动，是给客户前台查询用的！！！！！！！2017.2.12备注
            }
            catch (Exception exception)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OrderBank.PageSearch错误：" + exception.Message.ToString());
                //ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        /// <summary>
        /// 用户专用！！！！
        /// </summary>
        /// <param name="searchParams"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataSet UserPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_orderbank_user"; //用户专用查询。2017.3.2增加
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n"
                    + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName],[commission],[notifytime],[version]\r\n      ,cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid", tables, wheres, orderby, key, pageSize, page, false)
                    + "\r\nselect sum(1) ordtotal,sum(case when [status]=2 then 1 else 0 end) succordtotal,sum(refervalue) refervalue,sum(case when [status]=2 then realvalue else 0 end) realvalue,sum(isnull(promAmt,0)) promAmt,sum(case when [status]=2 then payAmt else 0 end) payAmt,sum(case when [status]=2 then supplierAmt-payAmt-promAmt else 0 end) profits,sum(promAmt) promAmt,sum(commission) commission from " + tables + " where " + wheres, paramList.ToArray());
                //上面这句话不能随意动，是给客户前台查询专用的！！
            }
            catch (Exception exception)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OrderBank.UserPageSearch错误：" + exception.Message.ToString());
                return set;
            }
        }

        public DataSet AdminPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_orderbank_admin";        //管理员能查询所有的！！！@2017.3.2修改
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);

 /*
                return DataBase.ExecuteDataset(CommandType.Text,
                            SqlHelper.GetCountSQL(tables, wheres, string.Empty)
                            + "\r\n"+ SqlHelper.GetPageSelectSQL("[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName],[commission],[notifytime],[version]\r\n      ,cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid,ipaddress", tables, wheres, orderby, key, pageSize, page, false)
                            + "\r\nselect sum(1) ordtotal,sum(case when [status] in (1,2,4,8) then 1 else 0 end) succordtotal,sum(refervalue) refervalue,sum(case when [status] in (1,2,4,8) then realvalue else 0 end) realvalue,sum(isnull(promAmt,0)) promAmt,sum(case when [status] in (1,2,4,8) then payAmt else 0 end) payAmt,sum(case when [status] in (1,2,4,8) then supplierAmt-payAmt-promAmt else 0 end) profits,sum(promAmt) promAmt,sum(commission) commission from " + tables + " where " + wheres, paramList.ToArray());

*/

                string rtnField = "[id] ,[orderid] ,[ordertype],[userid],[typeId],[paymodeId],[userorder],[refervalue],[realvalue],[notifyurl],[againNotifyUrl],[notifycount],[notifystat],[notifycontext],[returnurl],[attach],[payerip],[clientip],[referUrl],[addtime],[supplierID],[supplierOrder],[status],[completetime],[payRate],[supplierRate],[promRate],[payAmt],[promAmt],[supplierAmt],[profits],[server],[modetypename],[modeName],[commission],[notifytime],[version],cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid,ipaddress";
                string t = SqlHelper.ConstructSplitSQL("v_orderbank_admin", "id", page, pageSize, rtnField, 1, "id in (select id from @tmpid)");
                //新版本
                string sqlTmp = "declare @tmpid table(id bigint)" + Environment.NewLine
                    + "insert into @tmpid select id from v_orderbank_admin where " + wheres + Environment.NewLine
                    + "select count(*) from @tmpid" + Environment.NewLine
                    //+ SqlHelper.GetPageSelectSQL("[id] ,[orderid] ,[ordertype],[userid],[typeId],[paymodeId],[userorder],[refervalue],[realvalue],[notifyurl],[againNotifyUrl],[notifycount],[notifystat],[notifycontext],[returnurl],[attach],[payerip],[clientip],[referUrl],[addtime],[supplierID],[supplierOrder],[status],[completetime],[payRate],[supplierRate],[promRate],[payAmt],[promAmt],[supplierAmt],[profits],[server],[modetypename],[modeName],[commission],[notifytime],[version],cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid,ipaddress", tables, " id in (select id from @tmpid)", orderby, key, pageSize, page, false).Replace("with(nolock)", "") + Environment.NewLine
                    + t + Environment.NewLine
                    + "select sum(1) ordtotal,sum(case when [status] in (1,2,4,8) then 1 else 0 end) succordtotal,sum(refervalue) refervalue,sum(case when [status] in (1,2,4,8) then realvalue else 0 end) realvalue,sum(isnull(promAmt,0)) promAmt,sum(case when [status] in (1,2,4,8) then payAmt else 0 end) payAmt,sum(case when [status] in (1,2,4,8) then supplierAmt-payAmt-promAmt else 0 end) profits,sum(promAmt) promAmt,sum(commission) commission from  v_orderbank_admin where id in (select id from @tmpid) ";

/*
                //之前版本
                sqlTmp = SqlHelper.GetCountSQL(tables, wheres, string.Empty)
                            + Environment.NewLine
                            + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName],[commission],[notifytime],[version]\r\n      ,cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid,ipaddress", tables, wheres, orderby, key, pageSize, page, false)
                            + Environment.NewLine
                            + "select sum(1) ordtotal,sum(case when [status] in (1,2,4,8) then 1 else 0 end) succordtotal,sum(refervalue) refervalue,sum(case when [status] in (1,2,4,8) then realvalue else 0 end) realvalue,sum(isnull(promAmt,0)) promAmt,sum(case when [status] in (1,2,4,8) then payAmt else 0 end) payAmt,sum(case when [status] in (1,2,4,8) then supplierAmt-payAmt-promAmt else 0 end) profits,sum(promAmt) promAmt,sum(commission) commission from " + tables + " where " + wheres;
*/


                //KuaiCardLib.Logging.LogHelper.Write("OrderBank后台查询语句：" + sqlTmp);

                TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                DataSet ds = DataBase.ExecuteDataset(CommandType.Text, sqlTmp, paramList.ToArray());
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts3 = ts1.Subtract(ts2).Duration();
                //KuaiCardLib.Logging.LogHelper.Write("OrderBank后台查询时间差：" + ts3.TotalMilliseconds.ToString());

                return ds;
                 
            }
            catch (Exception exception)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OrderBank.AdminPageSearch错误：" + exception.Message.ToString());
                //ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public DataSet AdminPageSearch(List<SearchParam> searchParams, int pageSize, int pageIndex)
        {
            DataSet set = new DataSet();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] {
                    new SqlParameter("@serach_Where", SqlDbType.VarChar, 4000),
                    new SqlParameter("@serach_PageIndex", SqlDbType.Int, 10),
                    new SqlParameter("@serach_PageSize", SqlDbType.Int, 10)
                };
                commandParameters[0].Value = searchParams;
                commandParameters[1].Value = pageIndex;
                commandParameters[2].Value = pageSize;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_admin_search", commandParameters);
            }
            catch (Exception exception)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("OrderBank.AdminPageSearch错误：" + exception.Message.ToString());
                return set;
            }
        }

        #endregion

    }
}

