namespace KuaiCard.SQLServerDAL
{
    using DBAccess;
    using KuaiCard.IDAL;
    using KuaiCard.Model.Order;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.IO;

    public class OrderBank : IOrderBank
    {
        internal const string FIELDS = "[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName],[commission],[notifytime],[version]\r\n      ,cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,agentid";
        internal const string SQL_TABLE = "v_orderbank";

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

        public OrderBankInfo GetModel(long id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.BigInt), 
                new SqlParameter("@userid", SqlDbType.Int) 
            };
            commandParameters[0].Value = id;
            commandParameters[1].Value = DBNull.Value;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_GetModel", commandParameters));
        }

        public OrderBankInfo GetModel(string orderId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30) };
            commandParameters[0].Value = orderId;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_getbankdirectinfo", commandParameters));
        }

        public OrderBankInfo GetModel(long id, int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.BigInt), 
                new SqlParameter("@userid", SqlDbType.Int) 
            };
            commandParameters[0].Value = id;
            commandParameters[1].Value = userid;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_GetModel", commandParameters));
        }

        internal OrderBankInfo GetModelFromDs(DataSet ds)
        {
            OrderBankInfo info = new OrderBankInfo();
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            if (ds.Tables[0].Rows[0]["id"].ToString() != "")
            {
                info.id = long.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            info.orderid = ds.Tables[0].Rows[0]["orderid"].ToString();
            if (ds.Tables[0].Rows[0]["ordertype"].ToString() != "")
            {
                info.ordertype = int.Parse(ds.Tables[0].Rows[0]["ordertype"].ToString());
            }
            if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
            {
                info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
            }
            if (ds.Tables[0].Rows[0]["typeId"].ToString() != "")
            {
                info.typeId = int.Parse(ds.Tables[0].Rows[0]["typeId"].ToString());
            }
            info.paymodeId = ds.Tables[0].Rows[0]["paymodeId"].ToString();
            info.userorder = ds.Tables[0].Rows[0]["userorder"].ToString();
            if (ds.Tables[0].Rows[0]["refervalue"].ToString() != "")
            {
                info.refervalue = decimal.Parse(ds.Tables[0].Rows[0]["refervalue"].ToString());
            }
            if (ds.Tables[0].Rows[0]["realvalue"].ToString() != "")
            {
                info.realvalue = new decimal?(decimal.Parse(ds.Tables[0].Rows[0]["realvalue"].ToString()));
            }
            info.notifyurl = ds.Tables[0].Rows[0]["notifyurl"].ToString();
            info.againNotifyUrl = ds.Tables[0].Rows[0]["againNotifyUrl"].ToString();
            if (ds.Tables[0].Rows[0]["notifycount"].ToString() != "")
            {
                info.notifycount = int.Parse(ds.Tables[0].Rows[0]["notifycount"].ToString());
            }
            if (ds.Tables[0].Rows[0]["notifystat"].ToString() != "")
            {
                info.notifystat = int.Parse(ds.Tables[0].Rows[0]["notifystat"].ToString());
            }
            info.notifycontext = ds.Tables[0].Rows[0]["notifycontext"].ToString();
            info.returnurl = ds.Tables[0].Rows[0]["returnurl"].ToString();
            info.attach = ds.Tables[0].Rows[0]["attach"].ToString();
            info.payerip = ds.Tables[0].Rows[0]["payerip"].ToString();
            info.clientip = ds.Tables[0].Rows[0]["clientip"].ToString();
            info.referUrl = ds.Tables[0].Rows[0]["referUrl"].ToString();
            if (ds.Tables[0].Rows[0]["addtime"].ToString() != "")
            {
                info.addtime = DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString());
            }
            if (ds.Tables[0].Rows[0]["supplierID"].ToString() != "")
            {
                info.supplierId = int.Parse(ds.Tables[0].Rows[0]["supplierID"].ToString());
            }
            info.supplierOrder = ds.Tables[0].Rows[0]["supplierOrder"].ToString();
            if (ds.Tables[0].Rows[0]["status"].ToString() != "")
            {
                info.status = int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
            }
            if (ds.Tables[0].Rows[0]["completetime"].ToString() != "")
            {
                info.completetime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["completetime"].ToString()));
            }
            if (ds.Tables[0].Rows[0]["payRate"].ToString() != "")
            {
                info.payRate = decimal.Parse(ds.Tables[0].Rows[0]["payRate"].ToString());
            }
            if (ds.Tables[0].Rows[0]["supplierRate"].ToString() != "")
            {
                info.supplierRate = decimal.Parse(ds.Tables[0].Rows[0]["supplierRate"].ToString());
            }
            if (ds.Tables[0].Rows[0]["promRate"].ToString() != "")
            {
                info.promRate = decimal.Parse(ds.Tables[0].Rows[0]["promRate"].ToString());
            }
            if (ds.Tables[0].Rows[0]["payAmt"].ToString() != "")
            {
                info.payAmt = decimal.Parse(ds.Tables[0].Rows[0]["payAmt"].ToString());
            }
            if (ds.Tables[0].Rows[0]["promAmt"].ToString() != "")
            {
                info.promAmt = decimal.Parse(ds.Tables[0].Rows[0]["promAmt"].ToString());
            }
            if (ds.Tables[0].Rows[0]["supplierAmt"].ToString() != "")
            {
                info.supplierAmt = decimal.Parse(ds.Tables[0].Rows[0]["supplierAmt"].ToString());
            }
            if (ds.Tables[0].Rows[0]["profits"].ToString() != "")
            {
                info.profits = decimal.Parse(ds.Tables[0].Rows[0]["profits"].ToString());
            }
            if (ds.Tables[0].Rows[0]["server"].ToString() != "")
            {
                info.server = new int?(int.Parse(ds.Tables[0].Rows[0]["server"].ToString()));
            }
            if (ds.Tables[0].Rows[0]["manageId"].ToString() != "")
            {
                info.manageId = new int?(int.Parse(ds.Tables[0].Rows[0]["manageId"].ToString()));
            }
            if (ds.Tables[0].Rows[0]["commission"].ToString() != "")
            {
                info.commission = new decimal?(decimal.Parse(ds.Tables[0].Rows[0]["commission"].ToString()));
            }
            info.version = ds.Tables[0].Rows[0]["version"].ToString();
            if ((ds.Tables[0].Rows[0]["cus_subject"] != null) && (ds.Tables[0].Rows[0]["cus_subject"].ToString() != ""))
            {
                info.cus_subject = ds.Tables[0].Rows[0]["cus_subject"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_price"] != null) && (ds.Tables[0].Rows[0]["cus_price"].ToString() != ""))
            {
                info.cus_price = ds.Tables[0].Rows[0]["cus_price"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_quantity"] != null) && (ds.Tables[0].Rows[0]["cus_quantity"].ToString() != ""))
            {
                info.cus_quantity = ds.Tables[0].Rows[0]["cus_quantity"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_description"] != null) && (ds.Tables[0].Rows[0]["cus_description"].ToString() != ""))
            {
                info.cus_description = ds.Tables[0].Rows[0]["cus_description"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_field1"] != null) && (ds.Tables[0].Rows[0]["cus_field1"].ToString() != ""))
            {
                info.cus_field1 = ds.Tables[0].Rows[0]["cus_field1"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_field2"] != null) && (ds.Tables[0].Rows[0]["cus_field2"].ToString() != ""))
            {
                info.cus_field2 = ds.Tables[0].Rows[0]["cus_field2"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_field3"] != null) && (ds.Tables[0].Rows[0]["cus_field3"].ToString() != ""))
            {
                info.cus_field3 = ds.Tables[0].Rows[0]["cus_field3"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_field4"] != null) && (ds.Tables[0].Rows[0]["cus_field4"].ToString() != ""))
            {
                info.cus_field4 = ds.Tables[0].Rows[0]["cus_field4"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["cus_field5"] != null) && (ds.Tables[0].Rows[0]["cus_field5"].ToString() != ""))
            {
                info.cus_field5 = ds.Tables[0].Rows[0]["cus_field5"].ToString();
            }
            if ((ds.Tables[0].Rows[0]["agentid"] != null) && (ds.Tables[0].Rows[0]["agentid"].ToString() != ""))
            {
                info.agentId = Convert.ToInt32(ds.Tables[0].Rows[0]["agentid"].ToString());
            }
            if (ds.Tables[0].Columns.IndexOf("opstate") > 0)
            {
                info.opstate = ds.Tables[0].Rows[0]["opstate"].ToString();
            }
            return info;
        }

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
                new SqlParameter("@typeId", SqlDbType.Int), 
                new SqlParameter("@paymodeId", SqlDbType.VarChar, 10), 
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
            commandParameters[4].Value = model.typeId;
            commandParameters[5].Value = model.paymodeId;
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
                KuaiCardLib.Logging.LogHelper.Write("OrderBank.PageSearch错误：" + exception.Message.ToString());
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
                KuaiCardLib.Logging.LogHelper.Write("OrderBank.UserPageSearch错误：" + exception.Message.ToString());
                //ExceptionHandler.HandleException(exception);
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
                KuaiCardLib.Logging.LogHelper.Write("OrderBank.AdminPageSearch错误：" + exception.Message.ToString());
                //ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool ReDeduct(string orderid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                new SqlParameter("@result", SqlDbType.Bit) 
            };
            commandParameters[0].Value = orderid;
            commandParameters[1].Direction = ParameterDirection.Output;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_orderbank_rededuct", commandParameters);
            return (bool) commandParameters[1].Value;
        }
    

    }
}

