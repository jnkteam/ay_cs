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

    public class OrderSms : IOrderSms
    {
        internal const string FIELDS = "[id],[status],[orderid],[userorder],[supplierId],[userid],[mobile],[fee],[message],[servicenum],[linkid],[gwid],[payRate],[supplierRate],[promRate],[payAmt],[promAmt],[supplierAmt],[profits],[server],[addtime],[completetime]";
        internal const string SQL_TABLE = "ordersms";

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
                            case "typeid":
                            {
                                builder.Append(" AND [typeId] = @typeId");
                                SqlParameter parameter2 = new SqlParameter("@typeId", SqlDbType.Int);
                                parameter2.Value = (int) param2.ParamValue;
                                paramList.Add(parameter2);
                                break;
                            }
                            case "userorder":
                            {
                                builder.Append(" AND [userorder] like @userorder");
                                SqlParameter parameter3 = new SqlParameter("@userorder", SqlDbType.VarChar, 30);
                                parameter3.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter3);
                                break;
                            }
                            case "supplierorder":
                            {
                                builder.Append(" AND [linkid] like @supplierOrder");
                                SqlParameter parameter4 = new SqlParameter("@supplierOrder", SqlDbType.VarChar, 30);
                                parameter4.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter4);
                                break;
                            }
                            case "orderid_like":
                            {
                                builder.Append(" AND [orderid] like @orderid");
                                SqlParameter parameter5 = new SqlParameter("@orderid", SqlDbType.VarChar, 30);
                                parameter5.Value = SqlHelper.CleanString((string) param2.ParamValue, 30) + "%";
                                paramList.Add(parameter5);
                                break;
                            }
                            case "status":
                            {
                                builder.Append(" AND [status] = @status");
                                SqlParameter parameter6 = new SqlParameter("@status", SqlDbType.TinyInt);
                                parameter6.Value = (int) param2.ParamValue;
                                paramList.Add(parameter6);
                                break;
                            }
                            case "notifystat":
                            {
                                builder.Append(" AND [notifystat] = @notifystat");
                                SqlParameter parameter7 = new SqlParameter("@notifystat", SqlDbType.TinyInt);
                                parameter7.Value = (int) param2.ParamValue;
                                paramList.Add(parameter7);
                                break;
                            }
                            case "promid":
                            {
                                builder.Append(" AND exists(select 0 from PromotionUser where PromotionUser.PID = @promid and PromotionUser.RegId=userid)");
                                SqlParameter parameter8 = new SqlParameter("@promid", SqlDbType.Int);
                                parameter8.Value = (int) param2.ParamValue;
                                paramList.Add(parameter8);
                                break;
                            }
                            case "stime":
                            {
                                builder.Append(" AND [addtime] >= @stime");
                                SqlParameter parameter9 = new SqlParameter("@stime", SqlDbType.DateTime);
                                parameter9.Value = param2.ParamValue;
                                paramList.Add(parameter9);
                                break;
                            }
                            case "etime":
                            {
                                builder.Append(" AND [addtime] <= @etime");
                                SqlParameter parameter10 = new SqlParameter("@etime", SqlDbType.DateTime);
                                parameter10.Value = param2.ParamValue;
                                paramList.Add(parameter10);
                                break;
                            }
                            case "mobile":
                            {
                                builder.Append(" AND [mobile] like @mobile");
                                SqlParameter parameter11 = new SqlParameter("@mobile", SqlDbType.VarChar, 20);
                                parameter11.Value = "%" + ((string) param2.ParamValue) + "%";
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

        public bool Deduct(string orderid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30), new SqlParameter("@result", SqlDbType.Bit) };
            commandParameters[0].Value = orderid;
            commandParameters[1].Direction = ParameterDirection.Output;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordersms_deduct", commandParameters);
            return (bool) commandParameters[1].Value;
        }

        public OrderSmsInfo GetModel(int id)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@userid", SqlDbType.Int) };
            commandParameters[0].Value = id;
            commandParameters[1].Value = DBNull.Value;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordersms_GetById", commandParameters));
        }

        public OrderSmsInfo GetModel(string orderId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderId", SqlDbType.VarChar, 30) };
            commandParameters[0].Value = orderId;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordersms_Get", commandParameters));
        }

        public OrderSmsInfo GetModel(int id, int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@userid", SqlDbType.Int) };
            commandParameters[0].Value = id;
            commandParameters[1].Value = userid;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordersms_GetById", commandParameters));
        }

        public OrderSmsInfo GetModel(int suppId, string linkId)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@supplierId", SqlDbType.Int), new SqlParameter("@linkid", SqlDbType.VarChar, 50) };
            commandParameters[0].Value = suppId;
            commandParameters[1].Value = linkId;
            return this.GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordersms_GetModel", commandParameters));
        }

        internal OrderSmsInfo GetModelFromDs(DataSet ds)
        {
            OrderSmsInfo info = new OrderSmsInfo();
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            if (ds.Tables[0].Rows[0]["id"].ToString() != "")
            {
                info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
            info.orderid = ds.Tables[0].Rows[0]["orderid"].ToString();
            info.userorder = ds.Tables[0].Rows[0]["userorder"].ToString();
            if (ds.Tables[0].Rows[0]["supplierId"].ToString() != "")
            {
                info.supplierId = int.Parse(ds.Tables[0].Rows[0]["supplierId"].ToString());
            }
            if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
            {
                info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
            }
            info.mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
            if (ds.Tables[0].Rows[0]["fee"].ToString() != "")
            {
                info.fee = decimal.Parse(ds.Tables[0].Rows[0]["fee"].ToString());
            }
            info.message = ds.Tables[0].Rows[0]["message"].ToString();
            info.servicenum = ds.Tables[0].Rows[0]["servicenum"].ToString();
            info.linkid = ds.Tables[0].Rows[0]["linkid"].ToString();
            info.gwid = ds.Tables[0].Rows[0]["gwid"].ToString();
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
                info.server = int.Parse(ds.Tables[0].Rows[0]["server"].ToString());
            }
            if (ds.Tables[0].Rows[0]["addtime"].ToString() != "")
            {
                info.addtime = DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString());
            }
            if (ds.Tables[0].Rows[0]["completetime"].ToString() != "")
            {
                info.completetime = DateTime.Parse(ds.Tables[0].Rows[0]["completetime"].ToString());
            }
            if (ds.Tables[0].Rows[0]["status"].ToString() != "")
            {
                info.status = int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
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
            return info;
        }

        public bool Insert(OrderSmsInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@orderid", SqlDbType.NVarChar, 30), new SqlParameter("@userorder", SqlDbType.NVarChar, 30), new SqlParameter("@supplierId", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@mobile", SqlDbType.VarChar, 20), new SqlParameter("@fee", SqlDbType.Decimal, 9), new SqlParameter("@message", SqlDbType.NVarChar, 50), new SqlParameter("@servicenum", SqlDbType.VarChar, 50), new SqlParameter("@linkid", SqlDbType.VarChar, 50), new SqlParameter("@gwid", SqlDbType.VarChar, 2), new SqlParameter("@payRate", SqlDbType.Decimal, 9), new SqlParameter("@supplierRate", SqlDbType.Decimal, 9), new SqlParameter("@promRate", SqlDbType.Decimal, 9), new SqlParameter("@payAmt", SqlDbType.Decimal, 9), new SqlParameter("@promAmt", SqlDbType.Decimal, 9), new SqlParameter("@supplierAmt", SqlDbType.Decimal, 9), 
                new SqlParameter("@profits", SqlDbType.Decimal, 9), new SqlParameter("@server", SqlDbType.Int, 10), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@completetime", SqlDbType.DateTime), new SqlParameter("@status", SqlDbType.TinyInt), new SqlParameter("@manageId", SqlDbType.Int), new SqlParameter("@Cmd", SqlDbType.NVarChar, 10), new SqlParameter("@userMsgContenct", SqlDbType.NVarChar, 50)
             };
            commandParameters[0].Value = model.orderid;
            commandParameters[1].Value = model.userorder;
            commandParameters[2].Value = model.supplierId;
            commandParameters[3].Value = model.userid;
            commandParameters[4].Value = model.mobile;
            commandParameters[5].Value = model.fee;
            commandParameters[6].Value = model.message;
            commandParameters[7].Value = model.servicenum;
            commandParameters[8].Value = model.linkid;
            commandParameters[9].Value = model.gwid;
            commandParameters[10].Value = model.payRate;
            commandParameters[11].Value = model.supplierRate;
            commandParameters[12].Value = model.promRate;
            commandParameters[13].Value = model.payAmt;
            commandParameters[14].Value = model.promAmt;
            commandParameters[15].Value = model.supplierAmt;
            commandParameters[0x10].Value = model.profits;
            commandParameters[0x11].Value = model.server;
            commandParameters[0x12].Value = model.addtime;
            commandParameters[0x13].Value = model.completetime;
            commandParameters[20].Value = model.status;
            commandParameters[0x15].Value = model.manageId;
            commandParameters[0x16].Value = model.Cmd;
            commandParameters[0x17].Value = model.userMsgContenct;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordersms_Insert", commandParameters);
            return true;
        }

        public bool Notify(OrderSmsInfo model)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@linkId", SqlDbType.VarChar, 50), new SqlParameter("@againNotifyUrl", SqlDbType.VarChar, 300), new SqlParameter("@notifycount", SqlDbType.Int, 10), new SqlParameter("@notifystat", SqlDbType.TinyInt, 1), new SqlParameter("@notifycontext", SqlDbType.VarChar, 200), new SqlParameter("@notifytime", SqlDbType.DateTime), new SqlParameter("@userOrder", SqlDbType.VarChar, 30), new SqlParameter("@suppId", SqlDbType.VarChar, 50), new SqlParameter("@issucc", SqlDbType.Bit, 1), new SqlParameter("@errcode", SqlDbType.VarChar, 50) };
            commandParameters[0].Value = model.linkid;
            commandParameters[1].Value = model.againNotifyUrl;
            commandParameters[2].Value = model.notifycount;
            commandParameters[3].Value = model.notifystat;
            commandParameters[4].Value = model.notifycontext;
            commandParameters[5].Value = DateTime.Now;
            if (model.issucc)
            {
                commandParameters[6].Value = model.notifycontext;
            }
            else
            {
                commandParameters[6].Value = string.Empty;
            }
            commandParameters[7].Value = model.supplierId;
            commandParameters[8].Value = model.issucc ? 1 : 0;
            commandParameters[9].Value = model.errcode;
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordersms_notify", commandParameters) > 0);
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_ordersms";
                string columns = "[id],[orderid],[userorder],[supplierId],[userid],[mobile],[fee],[message],[servicenum],[linkid],[gwid],[payRate],[supplierRate],[promRate],[payAmt],[promAmt],[supplierAmt],[profits],[server],[addtime],[completetime],\r\n[againNotifyUrl],[notifycount],[notifystat],[notifycontext],[notifytime],[status],[commission]";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL(columns, tables, wheres, orderby, key, pageSize, page, false) + "\r\nselect sum(fee) realvalue,sum(case when [status]=2 then payAmt else 0 end) payAmt,sum(supplierAmt-(case when [status]=2 then payAmt else 0 end)) profits,sum(promAmt) promAmt,sum(commission) commission from V_ordersms where " + wheres, paramList.ToArray());
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
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_ordersms_rededuct", commandParameters);
            return (bool) commandParameters[1].Value;
        }
    }
}

