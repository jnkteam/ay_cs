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
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;

    public class OrderCard : IOrderCard
    {
        internal const string FIELDS = "[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[notifytime]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName]\r\n      ,[cardNo]\r\n      ,[cardPwd]\r\n      ,[desc]\r\n      ,[manageId]\r\n      ,[msg]\r\n      ,[commission]\r\n      ,[cardnum]\r\n      ,[resultcode]\r\n      ,[ismulticard]\r\n      ,[version],cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,errtype,agentid,faceValue";
        internal const string SQL_TABLE = "v_ordercard";

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
                            case "userorder":
                            {
                                builder.Append(" AND [userorder] like @userorder");
                                SqlParameter parameter5 = new SqlParameter("@userorder", SqlDbType.VarChar, 30);
                                parameter5.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter5);
                                break;
                            }
                            case "orderid":
                            {
                                builder.Append(" AND [orderid] like @orderid");
                                SqlParameter parameter6 = new SqlParameter("@orderid", SqlDbType.VarChar, 30);
                                parameter6.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter6);
                                break;
                            }
                            case "orderid_like":
                            {
                                builder.Append(" AND [orderid] like @orderid");
                                SqlParameter parameter7 = new SqlParameter("@orderid", SqlDbType.VarChar, 30);
                                parameter7.Value = SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter7);
                                break;
                            }
                            case "cardno":
                            {
                                builder.Append(" AND [cardNo] like @cardno");
                                SqlParameter parameter8 = new SqlParameter("@cardno", SqlDbType.NVarChar, 50);
                                parameter8.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                                paramList.Add(parameter8);
                                break;
                            }
                            case "supplierorder":
                            {
                                builder.Append(" AND [supplierOrder] like @supplierOrder");
                                SqlParameter parameter9 = new SqlParameter("@supplierOrder", SqlDbType.VarChar, 30);
                                parameter9.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
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
                                builder.Append(" AND ([status] = 4 or  [status] = 8)");
                                break;

                            case "notifystat":
                            {
                                builder.Append(" AND ([notifystat] = @notifystat AND ordertype <> 8)");
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
                                SqlParameter parameter13 = new SqlParameter("@stime", SqlDbType.DateTime);
                                parameter13.Value = param2.ParamValue;
                                paramList.Add(parameter13);
                                break;
                            }
                            case "etime":
                            {
                                builder.Append(" AND [processingtime] <= @etime");
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

        public bool Complete(OrderCardInfo model)
        {
            if (HttpRuntime.Cache["Complete" + model.orderid] == null)
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                    new SqlParameter("@method", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@supplierid", SqlDbType.Int, 10), 
                    new SqlParameter("@userId", SqlDbType.Int), 
                    new SqlParameter("@promId", SqlDbType.Int), 
                    new SqlParameter("@manageId", SqlDbType.Int), 
                    new SqlParameter("@status", SqlDbType.TinyInt), 
                    new SqlParameter("@supplierOrder", SqlDbType.VarChar, 50), 
                    new SqlParameter("@refervalue", SqlDbType.Decimal, 9), 
                    new SqlParameter("@faceValue", SqlDbType.Decimal, 9), 
                    new SqlParameter("@realvalue", SqlDbType.Decimal, 9), 
                    new SqlParameter("@withholdAmt", SqlDbType.Decimal, 9), 
                    new SqlParameter("@profits", SqlDbType.Decimal, 9), 
                    new SqlParameter("@payRate", SqlDbType.Decimal, 9), 
                    new SqlParameter("@payAmt", SqlDbType.Decimal, 9), 
                    new SqlParameter("@supplierRate", SqlDbType.Decimal, 9), 
                    new SqlParameter("@supplierAmt", SqlDbType.Decimal, 9), 
                    new SqlParameter("@promRate", SqlDbType.Decimal, 9), 
                    new SqlParameter("@promAmt", SqlDbType.Decimal, 9), 
                    new SqlParameter("@addtime", SqlDbType.DateTime), 
                    new SqlParameter("@completetime", SqlDbType.DateTime),
                    new SqlParameter("@msg", SqlDbType.NVarChar, 200), 
                    new SqlParameter("@userViewMsg", SqlDbType.NVarChar, 200), 
                    new SqlParameter("@opstate", SqlDbType.NVarChar, 200), 
                    new SqlParameter("@errtype", SqlDbType.NVarChar, 50), 
                    new SqlParameter("@typeid", SqlDbType.Int), 
                    new SqlParameter("@cardno", SqlDbType.VarChar, 40), 
                    new SqlParameter("@cardpwd", SqlDbType.VarChar, 40), 
                    new SqlParameter("@cardversion", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@withhold_type", SqlDbType.TinyInt, 1)
                 };
                commandParameters[0].Value = model.orderid;
                commandParameters[1].Value = model.method;
                commandParameters[2].Value = model.supplierId;
                commandParameters[3].Value = model.userid;
                commandParameters[4].Value = model.agentId;
                commandParameters[5].Value = model.manageId;
                commandParameters[6].Value = model.status;
                commandParameters[7].Value = model.supplierOrder;
                commandParameters[8].Value = model.refervalue;
                commandParameters[9].Value = model.faceValue;
                commandParameters[10].Value = model.realvalue;
                commandParameters[11].Value = model.withholdAmt;
                commandParameters[12].Value = model.profits;
                commandParameters[13].Value = model.payRate;
                commandParameters[14].Value = model.payAmt;
                commandParameters[15].Value = model.supplierRate;
                commandParameters[0x10].Value = model.supplierAmt;
                commandParameters[0x11].Value = model.promRate;
                commandParameters[0x12].Value = model.promAmt;
                commandParameters[0x13].Value = DateTime.Now;
                commandParameters[20].Value = model.completetime;
                commandParameters[0x15].Value = model.msg;
                commandParameters[0x16].Value = model.userViewMsg;
                commandParameters[0x17].Value = model.opstate;
                commandParameters[0x18].Value = model.errtype;
                commandParameters[0x19].Value = model.typeId;
                commandParameters[0x1a].Value = model.cardNo;
                commandParameters[0x1b].Value = model.cardPwd;
                commandParameters[0x1c].Value = model.cardversion;
                commandParameters[0x1d].Value = model.withhold_type;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordercard_settled", commandParameters);
                HttpRuntime.Cache.Insert("Complete" + model.orderid, model.status, null, DateTime.Now.AddSeconds(5.0), TimeSpan.Zero);
            }
            return true;
        }

        public DataTable DataItemsByOrderId(string orderId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderId", SqlDbType.NVarChar, 30) };
            commandParameters[0].Value = orderId;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercarditem_Getlistbyorderid", commandParameters).Tables[0];
        }

        public bool Deduct(string orderid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30), new SqlParameter("@result", SqlDbType.Bit) };
            commandParameters[0].Value = orderid;
            commandParameters[1].Direction = ParameterDirection.Output;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordercard_deduct", commandParameters);
            return (bool) commandParameters[1].Value;
        }

        public CardItemInfo GetItemModel(string orderId, int serial)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderId", SqlDbType.NVarChar, 30), new SqlParameter("@serial", SqlDbType.Int, 10) };
            commandParameters[0].Value = orderId;
            commandParameters[1].Value = serial;
            CardItemInfo info = new CardItemInfo();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercarditem_GetModel", commandParameters);
            if (set.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            if (set.Tables[0].Rows[0]["id"].ToString() != "")
            {
                info.id = long.Parse(set.Tables[0].Rows[0]["id"].ToString());
            }
            if (set.Tables[0].Rows[0]["userid"].ToString() != "")
            {
                info.userid = int.Parse(set.Tables[0].Rows[0]["userid"].ToString());
            }
            if (set.Tables[0].Rows[0]["serial"].ToString() != "")
            {
                info.serial = int.Parse(set.Tables[0].Rows[0]["serial"].ToString());
            }
            info.porderid = set.Tables[0].Rows[0]["porderid"].ToString();
            if (set.Tables[0].Rows[0]["suppid"].ToString() != "")
            {
                info.suppid = int.Parse(set.Tables[0].Rows[0]["suppid"].ToString());
            }
            if (set.Tables[0].Rows[0]["cardtype"].ToString() != "")
            {
                info.cardtype = int.Parse(set.Tables[0].Rows[0]["cardtype"].ToString());
            }
            info.cardno = set.Tables[0].Rows[0]["cardno"].ToString();
            info.cardpwd = set.Tables[0].Rows[0]["cardpwd"].ToString();
            if (set.Tables[0].Rows[0]["refervalue"].ToString() != "")
            {
                info.refervalue = new decimal?(decimal.Parse(set.Tables[0].Rows[0]["refervalue"].ToString()));
            }
            if (set.Tables[0].Rows[0]["payrate"].ToString() != "")
            {
                info.payrate = new decimal?(decimal.Parse(set.Tables[0].Rows[0]["payrate"].ToString()));
            }
            if (set.Tables[0].Rows[0]["addtime"].ToString() != "")
            {
                info.addtime = DateTime.Parse(set.Tables[0].Rows[0]["addtime"].ToString());
            }
            info.supplierOrder = set.Tables[0].Rows[0]["supplierOrder"].ToString();
            if (set.Tables[0].Rows[0]["realvalue"].ToString() != "")
            {
                info.realvalue = decimal.Parse(set.Tables[0].Rows[0]["realvalue"].ToString());
            }
            if (set.Tables[0].Rows[0]["status"].ToString() != "")
            {
                info.status = int.Parse(set.Tables[0].Rows[0]["status"].ToString());
            }
            info.opstate = set.Tables[0].Rows[0]["opstate"].ToString();
            info.msg = set.Tables[0].Rows[0]["msg"].ToString();
            if (set.Tables[0].Rows[0]["completetime"].ToString() != "")
            {
                info.completetime = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["completetime"].ToString()));
            }
            if (set.Tables[0].Rows[0]["supplierrate"].ToString() != "")
            {
                info.supplierrate = decimal.Parse(set.Tables[0].Rows[0]["supplierrate"].ToString());
            }
            if (set.Tables[0].Rows[0]["promrate"].ToString() != "")
            {
                info.promrate = decimal.Parse(set.Tables[0].Rows[0]["promrate"].ToString());
            }
            if (set.Tables[0].Rows[0]["commission"].ToString() != "")
            {
                info.commission = decimal.Parse(set.Tables[0].Rows[0]["commission"].ToString());
            }
            if (set.Tables[0].Rows[0]["agent"].ToString() != "")
            {
                info.agentId = int.Parse(set.Tables[0].Rows[0]["agent"].ToString());
            }
            return info;
        }

        public OrderCardInfo GetModel(long id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.BigInt), new SqlParameter("@userid", SqlDbType.Int) };
            commandParameters[0].Value = id;
            commandParameters[1].Value = DBNull.Value;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercard_GetModel", commandParameters));
        }

        public OrderCardInfo GetModel(string orderId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30) };
            commandParameters[0].Value = orderId;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercard_get", commandParameters));
        }

        public OrderCardInfo GetModel(long id, int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.BigInt), new SqlParameter("@userid", SqlDbType.Int) };
            commandParameters[0].Value = id;
            commandParameters[1].Value = userid;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercard_GetModel", commandParameters));
        }

        internal OrderCardInfo GetModelFromDs(DataSet ds)
        {
            OrderCardInfo info = new OrderCardInfo();
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            if (ds.Tables[0].Rows[0]["id"].ToString() != "")
            {
                info.id = long.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            info.orderid = ds.Tables[0].Rows[0]["orderid"].ToString();
            info.cardNo = ds.Tables[0].Rows[0]["cardNo"].ToString();
            info.cardPwd = ds.Tables[0].Rows[0]["cardPwd"].ToString();
            info.Desc = ds.Tables[0].Rows[0]["desc"].ToString();
            if (ds.Tables[0].Rows[0]["ordertype"].ToString() != "")
            {
                info.ordertype = int.Parse(ds.Tables[0].Rows[0]["ordertype"].ToString());
            }
            if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
            {
                info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
            }
            if (ds.Tables[0].Rows[0]["manageId"].ToString() != "")
            {
                info.manageId = new int?(int.Parse(ds.Tables[0].Rows[0]["manageId"].ToString()));
            }
            if (ds.Tables[0].Rows[0]["cardtype"].ToString() != "")
            {
                info.cardType = int.Parse(ds.Tables[0].Rows[0]["cardtype"].ToString());
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
            info.faceValue = 0M;
            if (ds.Tables[0].Rows[0]["faceValue"].ToString() != "")
            {
                info.faceValue = decimal.Parse(ds.Tables[0].Rows[0]["faceValue"].ToString());
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
            info.msg = ds.Tables[0].Rows[0]["msg"].ToString();
            info.userViewMsg = ds.Tables[0].Rows[0]["userViewMsg"].ToString();
            info.opstate = ds.Tables[0].Rows[0]["opstate"].ToString();
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
            if (ds.Tables[0].Rows[0]["cardnum"].ToString() != "")
            {
                info.cardnum = int.Parse(ds.Tables[0].Rows[0]["cardnum"].ToString());
            }
            if (ds.Tables[0].Rows[0]["ismulticard"].ToString() != "")
            {
                info.ismulticard = int.Parse(ds.Tables[0].Rows[0]["ismulticard"].ToString());
            }
            info.resultcode = ds.Tables[0].Rows[0]["resultcode"].ToString();
            info.ovalue = ds.Tables[0].Rows[0]["ovalue"].ToString();
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
            info.errtype = ds.Tables[0].Rows[0]["errtype"].ToString();
            if ((ds.Tables[0].Rows[0]["agentid"] != null) && (ds.Tables[0].Rows[0]["agentid"].ToString() != ""))
            {
                info.agentId = int.Parse(ds.Tables[0].Rows[0]["agentid"].ToString());
            }
            if ((ds.Tables[0].Rows[0]["withhold"] != null) && (ds.Tables[0].Rows[0]["withhold"].ToString() != ""))
            {
                info.withhold_type = byte.Parse(ds.Tables[0].Rows[0]["withhold"].ToString());
            }
            if ((ds.Tables[0].Rows[0]["makeup"] != null) && (ds.Tables[0].Rows[0]["makeup"].ToString() != ""))
            {
                info.makeup = byte.Parse(ds.Tables[0].Rows[0]["makeup"].ToString());
            }
            return info;
        }

        public long Insert(OrderCardInfo model)
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
                new SqlParameter("@notifyurl", SqlDbType.VarChar, 200), 
                new SqlParameter("@returnurl", SqlDbType.VarChar, 200), 
                new SqlParameter("@attach", SqlDbType.VarChar, 0xff), 
                new SqlParameter("@payerip", SqlDbType.VarChar, 20), 
                new SqlParameter("@clientip", SqlDbType.VarChar, 20), 
                new SqlParameter("@referUrl", SqlDbType.NVarChar, 0x7d0), 
                new SqlParameter("@addtime", SqlDbType.DateTime), 
                new SqlParameter("@supplierID", SqlDbType.Int, 10), 
                new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                new SqlParameter("@cardNo", SqlDbType.NVarChar, 0x3e8), 
                new SqlParameter("@cardPwd", SqlDbType.NVarChar, 0x3e8),
                new SqlParameter("@server", SqlDbType.Int), 
                new SqlParameter("@manageId", SqlDbType.Int), 
                new SqlParameter("@cardnum", SqlDbType.Int), 
                new SqlParameter("@resultcode", SqlDbType.NVarChar, 100),
                new SqlParameter("@ismulticard", SqlDbType.TinyInt, 1),
                new SqlParameter("@ovalue", SqlDbType.NVarChar, 200), 
                new SqlParameter("@opstate", SqlDbType.NVarChar, 200), 
                new SqlParameter("@msg", SqlDbType.NVarChar, 200), 
                new SqlParameter("@cardtype", SqlDbType.Int, 10), 
                new SqlParameter("@version", SqlDbType.VarChar, 10), 
                new SqlParameter("@cus_subject", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_price", SqlDbType.NVarChar, 50), 
                new SqlParameter("@cus_quantity", SqlDbType.NVarChar, 50), 
                new SqlParameter("@cus_description", SqlDbType.NVarChar, 0x3e8), 
                new SqlParameter("@cus_field1", SqlDbType.NVarChar, 100),
                new SqlParameter("@cus_field2", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_field3", SqlDbType.NVarChar, 100), 
                new SqlParameter("@cus_field4", SqlDbType.NVarChar, 100),
                new SqlParameter("@cus_field5", SqlDbType.NVarChar, 100),
                new SqlParameter("@agentid", SqlDbType.Int), 
                new SqlParameter("@faceValue", SqlDbType.Decimal, 9), 
                new SqlParameter("@userViewMsg", SqlDbType.NVarChar, 100),
                new SqlParameter("@errtype", SqlDbType.NVarChar, 50), 
                new SqlParameter("@makeup", SqlDbType.TinyInt, 1)
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
            commandParameters[0x10].Value = model.status;
            commandParameters[0x11].Value = model.cardNo;
            commandParameters[0x12].Value = model.cardPwd;
            commandParameters[0x13].Value = model.server;
            commandParameters[20].Value = model.manageId;
            commandParameters[0x15].Value = model.cardnum;
            commandParameters[0x16].Value = model.resultcode;
            commandParameters[0x17].Value = model.ismulticard;
            commandParameters[0x18].Value = model.ovalue;
            commandParameters[0x19].Value = model.opstate;
            commandParameters[0x1a].Value = model.msg;
            commandParameters[0x1b].Value = model.cardType;
            commandParameters[0x1c].Value = model.version;
            commandParameters[0x1d].Value = model.cus_subject;
            commandParameters[30].Value = model.cus_price;
            commandParameters[0x1f].Value = model.cus_quantity;
            commandParameters[0x20].Value = model.cus_description;
            commandParameters[0x21].Value = model.cus_field1;
            commandParameters[0x22].Value = model.cus_field2;
            commandParameters[0x23].Value = model.cus_field3;
            commandParameters[0x24].Value = model.cus_field4;
            commandParameters[0x25].Value = model.cus_field5;
            commandParameters[0x26].Value = model.agentId;
            commandParameters[0x27].Value = model.faceValue;
            commandParameters[40].Value = model.userViewMsg;
            commandParameters[0x29].Value = model.errtype;
            commandParameters[0x2a].Value = model.makeup;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordercard_add", commandParameters);
            return (long) commandParameters[0].Value;
        }

        public long InsertItem(CardItemInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.BigInt, 8), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@serial", SqlDbType.Int, 10), new SqlParameter("@porderid", SqlDbType.NVarChar, 30), new SqlParameter("@suppid", SqlDbType.Int, 10), new SqlParameter("@cardtype", SqlDbType.Int, 10), new SqlParameter("@cardno", SqlDbType.NVarChar, 30), new SqlParameter("@cardpwd", SqlDbType.NVarChar, 50), new SqlParameter("@refervalue", SqlDbType.Decimal, 9), new SqlParameter("@payrate", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@supplierOrder", SqlDbType.VarChar, 50), new SqlParameter("@realvalue", SqlDbType.Decimal, 9), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@opstate", SqlDbType.NVarChar, 20), new SqlParameter("@msg", SqlDbType.NVarChar, 50), 
                new SqlParameter("@completetime", SqlDbType.DateTime), new SqlParameter("@supplierrate", SqlDbType.Decimal, 9), new SqlParameter("@promrate", SqlDbType.Decimal, 9), new SqlParameter("@commission", SqlDbType.Decimal, 9), new SqlParameter("@agentid", SqlDbType.Int, 10)
             };
            commandParameters[0].Direction = ParameterDirection.Output;
            commandParameters[1].Value = model.userid;
            commandParameters[2].Value = model.serial;
            commandParameters[3].Value = model.porderid;
            commandParameters[4].Value = model.suppid;
            commandParameters[5].Value = model.cardtype;
            commandParameters[6].Value = model.cardno;
            commandParameters[7].Value = model.cardpwd;
            commandParameters[8].Value = model.refervalue;
            commandParameters[9].Value = model.payrate;
            commandParameters[10].Value = model.addtime;
            commandParameters[11].Value = model.supplierOrder;
            commandParameters[12].Value = model.realvalue;
            commandParameters[13].Value = model.status;
            commandParameters[14].Value = model.opstate;
            commandParameters[15].Value = model.msg;
            commandParameters[0x10].Value = model.completetime;
            commandParameters[0x11].Value = model.supplierrate;
            commandParameters[0x12].Value = model.promrate;
            commandParameters[0x13].Value = model.commission;
            commandParameters[20].Value = model.agentId;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordercarditem_add", commandParameters);
            return (long) commandParameters[0].Value;
        }

        private static string ItemBuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
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
                            case "manageid":
                            {
                                builder.Append(" AND [manageId] = @manageId");
                                SqlParameter parameter2 = new SqlParameter("@manageId", SqlDbType.Int);
                                parameter2.Value = (int) param2.ParamValue;
                                paramList.Add(parameter2);
                                break;
                            }
                            case "typeId":
                            {
                                builder.Append(" AND [typeId] = @typeId");
                                SqlParameter parameter3 = new SqlParameter("@typeId", SqlDbType.Int);
                                parameter3.Value = (int) param2.ParamValue;
                                paramList.Add(parameter3);
                                break;
                            }
                            case "userorder":
                            {
                                builder.Append(" AND [userorder] like @userorder");
                                SqlParameter parameter4 = new SqlParameter("@userorder", SqlDbType.VarChar, 30);
                                parameter4.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter4);
                                break;
                            }
                            case "cardno":
                            {
                                builder.Append(" AND [cardNo] like @cardno");
                                SqlParameter parameter5 = new SqlParameter("@cardno", SqlDbType.NVarChar, 50);
                                parameter5.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 50) + "%";
                                paramList.Add(parameter5);
                                break;
                            }
                            case "supplierorder":
                            {
                                builder.Append(" AND [supplierOrder] like @supplierOrder");
                                SqlParameter parameter6 = new SqlParameter("@supplierOrder", SqlDbType.VarChar, 30);
                                parameter6.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter6);
                                break;
                            }
                            case "status":
                            {
                                builder.Append(" AND [status] = @status");
                                SqlParameter parameter7 = new SqlParameter("@status", SqlDbType.TinyInt);
                                parameter7.Value = (int) param2.ParamValue;
                                paramList.Add(parameter7);
                                break;
                            }
                            case "statusallfail":
                                builder.Append(" AND ([status] = 4 or  [status] = 8)");
                                break;

                            case "notifystat":
                            {
                                builder.Append(" AND [notifystat] = @notifystat");
                                SqlParameter parameter8 = new SqlParameter("@notifystat", SqlDbType.TinyInt);
                                parameter8.Value = (int) param2.ParamValue;
                                paramList.Add(parameter8);
                                break;
                            }
                            case "promid":
                            {
                                builder.Append(" AND exists(select 0 from PromotionUser where PromotionUser.PID = @promid and PromotionUser.RegId=userid)");
                                SqlParameter parameter9 = new SqlParameter("@promid", SqlDbType.Int);
                                parameter9.Value = (int) param2.ParamValue;
                                paramList.Add(parameter9);
                                break;
                            }
                            case "stime":
                            {
                                builder.Append(" AND [addtime] >= @stime");
                                SqlParameter parameter10 = new SqlParameter("@stime", SqlDbType.DateTime);
                                parameter10.Value = param2.ParamValue;
                                paramList.Add(parameter10);
                                break;
                            }
                            case "etime":
                            {
                                builder.Append(" AND [addtime] <= @etime");
                                SqlParameter parameter11 = new SqlParameter("@etime", SqlDbType.DateTime);
                                parameter11.Value = param2.ParamValue;
                                paramList.Add(parameter11);
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
                            SqlParameter parameter12 = new SqlParameter("@status1", SqlDbType.TinyInt);
                            parameter12.Value = (int) param2.ParamValue;
                            paramList.Add(parameter12);
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public bool ItemComplete(CardItemInfo model, out bool allCompleted, out string opstate, out string ovalue, out decimal ototalvalue)
        {
            allCompleted = false;
            opstate = string.Empty;
            ovalue = string.Empty;
            ototalvalue = 0M;
            if (HttpRuntime.Cache["Item_Complete" + model.porderid + model.serial.ToString()] == null)
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30), new SqlParameter("@serial", SqlDbType.Int), new SqlParameter("@status", SqlDbType.TinyInt), new SqlParameter("@supplierOrder", SqlDbType.VarChar, 50), new SqlParameter("@realvalue", SqlDbType.Decimal, 9), new SqlParameter("@payrate", SqlDbType.Decimal, 9), new SqlParameter("@completetime", SqlDbType.DateTime), new SqlParameter("@opstate", SqlDbType.NVarChar, 10), new SqlParameter("@msg", SqlDbType.NVarChar, 50), new SqlParameter("@completed", SqlDbType.TinyInt), new SqlParameter("@promRate", SqlDbType.Decimal, 9) };
                commandParameters[0].Value = model.porderid;
                commandParameters[1].Value = model.serial;
                commandParameters[2].Value = model.status;
                commandParameters[3].Value = model.supplierOrder;
                commandParameters[4].Value = model.realvalue;
                commandParameters[5].Value = model.payrate;
                commandParameters[6].Value = model.completetime;
                commandParameters[7].Value = model.opstate;
                commandParameters[8].Value = model.msg;
                commandParameters[9].Direction = ParameterDirection.Output;
                commandParameters[10].Value = model.promrate;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercarditem_settled", commandParameters);
                HttpRuntime.Cache.Insert("Item_Complete" + model.porderid + model.serial.ToString(), model.status, null, DateTime.Now.AddSeconds(5.0), TimeSpan.Zero);
                if (commandParameters[9].Value != DBNull.Value)
                {
                    if (Convert.ToInt32(commandParameters[9].Value) == 1)
                    {
                        allCompleted = true;
                    }
                    if (allCompleted)
                    {
                        DataTable table = set.Tables[0];
                        if ((table != null) && (table.Rows.Count > 0))
                        {
                            DataRow row = table.Rows[0];
                            if (row["totalValue"] != DBNull.Value)
                            {
                                ototalvalue = Convert.ToDecimal(row["totalValue"]);
                            }
                            if (row["resultcode"] != DBNull.Value)
                            {
                                opstate = Convert.ToString(row["resultcode"]);
                            }
                            if (row["ovalue"] != DBNull.Value)
                            {
                                ovalue = Convert.ToString(row["ovalue"]);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public DataSet ItemPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_ordercard";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "[id] desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = ItemBuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[notifytime]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName]\r\n      ,[cardNo]\r\n      ,[cardPwd]\r\n      ,[desc]\r\n      ,[manageId]\r\n      ,[msg]\r\n      ,[commission]\r\n      ,[cardnum]\r\n      ,[resultcode]\r\n      ,[ismulticard]\r\n      ,[version],cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,errtype,agentid,faceValue", tables, wheres, orderby, key, pageSize, page, false) + "\r\nselect sum(refervalue) refervalue,sum(case when [status]=2 then realvalue else 0 end) realvalue,sum(case when [status]=2 then payAmt else 0 end) payAmt,sum(supplierAmt-(case when [status]=2 then payAmt else 0 end)) profits,sum(promAmt) promAmt from V_ordercard where " + wheres, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Notify(OrderCardInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 20), new SqlParameter("@againNotifyUrl", SqlDbType.NVarChar, 0x7d0), new SqlParameter("@notifycount", SqlDbType.Int, 10), new SqlParameter("@notifystat", SqlDbType.TinyInt, 1), new SqlParameter("@notifycontext", SqlDbType.VarChar, 200), new SqlParameter("@notifytime", SqlDbType.DateTime) };
            commandParameters[0].Value = model.orderid;
            commandParameters[1].Value = model.againNotifyUrl;
            commandParameters[2].Value = model.notifycount;
            commandParameters[3].Value = model.notifystat;
            commandParameters[4].Value = model.notifycontext;
            commandParameters[5].Value = DateTime.Now;
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordercard_notify", commandParameters) > 0);
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_ordercard";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "[id] desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + 
                    SqlHelper.GetPageSelectSQL("[id]\r\n      ,[orderid]\r\n      ,[ordertype]\r\n      ,[userid]\r\n      ,[typeId]\r\n      ,[paymodeId]\r\n      ,[userorder]\r\n      ,[refervalue]\r\n      ,[realvalue]\r\n      ,[notifyurl]\r\n      ,[againNotifyUrl]\r\n      ,[notifycount]\r\n      ,[notifystat]\r\n      ,[notifycontext]\r\n      ,[notifytime]\r\n      ,[returnurl]\r\n      ,[attach]\r\n      ,[payerip]\r\n      ,[clientip]\r\n      ,[referUrl]\r\n      ,[addtime]\r\n      ,[supplierID]\r\n      ,[supplierOrder]\r\n      ,[status]\r\n      ,[completetime]\r\n      ,[payRate]\r\n      ,[supplierRate]\r\n      ,[promRate]\r\n      ,[payAmt]\r\n      ,[promAmt]\r\n      ,[supplierAmt]\r\n      ,[profits]\r\n      ,[server]\r\n      ,[modetypename]\r\n      ,[modeName]\r\n      ,[cardNo]\r\n      ,[cardPwd]\r\n      ,[desc]\r\n      ,[manageId]\r\n      ,[msg]\r\n      ,[commission]\r\n      ,[cardnum]\r\n      ,[resultcode]\r\n      ,[ismulticard]\r\n      ,[version],cus_subject,cus_price,cus_quantity,cus_description,cus_field1,cus_field2,cus_field3,cus_field4,cus_field5,errtype,agentid,faceValue", tables, wheres, orderby, key, pageSize, page, false) + 
                    "\r\nselect sum(1) ordtotal,sum(case when [status]=2 then 1 else 0 end) succordtotal,sum(refervalue) refervalue,sum(realvalue) realvalue,sum(case when [status]=2 then payAmt else 0 end) payAmt,sum(isnull(promAmt,0)) promAmt,sum(supplierAmt-(case when [status]=2 then payAmt else 0 end)) profits,sum(promAmt) promAmt,sum(commission) commission from V_ordercard where " + wheres, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool ReDeduct(string orderid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30), new SqlParameter("@result", SqlDbType.Bit) };
            commandParameters[0].Value = orderid;
            commandParameters[1].Direction = ParameterDirection.Output;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordercard_rededuct", commandParameters);
            return (bool) commandParameters[1].Value;
        }
    }
}

