namespace KuaiCard.BLL.Order
{
    using DBAccess;
    using KuaiCard.Model.Order;
    using KuaiCard.Model.User;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Dal
    {
        internal const string FIELDS = "[id]\r\n      ,[mydate]\r\n      ,[typeId]\r\n      ,[modetypename]\r\n      ,[faceValue]\r\n      ,[payrate]\r\n      ,[s_num]\r\n      ,[userId]\r\n      ,[Username]\r\n      ,[full_name]\r\n      ,[sumpay]";
        internal const string SQL_TABLE = "V_usersOrderIncome";

        public static DataSet AgentStat2(DateTime sdt, DateTime edt, int page, int pagesize, string orderby)
        {
            try
            {
                string commandText = "\r\nselect count(0) as C\r\n\tfrom(\r\n\tselect agentid\r\n\tfrom v_order with(nolock)\r\n\twhere agentid > 0 and promAmt > 0 and processingtime >= @sdt and processingtime < @edt \r\n\tgroup by agentid) A\r\n\r\n\r\nselect D1.agentid,payAmt,promAmt,supplierAmt,realvalue,B.username,B.full_name\r\nfrom(\r\n\tselect agentid,payAmt,promAmt,supplierAmt,realvalue,ROW_NUMBER() OVER(ORDER BY D.agentid) AS P_ROW \r\n\tfrom(\r\n\tselect agentid,sum(payAmt) payAmt,sum(promAmt) as promAmt,sum(supplierAmt) as supplierAmt,sum(realvalue) as realvalue\r\n\tfrom v_order with(nolock)\r\n\twhere agentid > 0 and promAmt > 0 and processingtime >= @sdt and processingtime < @edt  \r\n\tgroup by agentid) D \r\n)D1  left join userbase B with(nolock)  on D1.agentid = B.id\r\nWHERE D1.P_ROW BETWEEN @page*@pagesize+1 AND @page*@pagesize+@pagesize\r\norder by " + orderby;
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@sdt", SqlDbType.DateTime, 8), new SqlParameter("@edt", SqlDbType.DateTime, 8), new SqlParameter("@page", SqlDbType.Int, 10), new SqlParameter("@pagesize", SqlDbType.Int, 10) };
                commandParameters[0].Value = sdt;
                commandParameters[1].Value = edt;
                commandParameters[2].Value = page;
                commandParameters[3].Value = pagesize;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataSet AgentStat3(int agentid, DateTime sdt, DateTime edt, int page, int pagesize, string orderby)
        {
            try
            {
                string commandText = "\r\nselect count(0) as C\r\n\t\tfrom orderbankamt a with(nolock)\r\n\t\t\t\tleft join orderbank b with(nolock) \r\n\t\t\t\t\t\t\ton a.orderid = b.orderid\t\t\r\n\t\twhere a.[status] = 2 and b.agentid > 0 and processingtime >= @begintime and processingtime <= @endtime and (b.agentid \r\n\t\tgroup by b.agentid\r\n\r\n\r\nselect \r\n\ta.agentid\r\n\t,userName\r\n\t,tradeAmt\r\n\t,promAmt\r\n\t,profits\r\n\t,lowercount = (select count(0) from PromotionUser where PID = a.agentid)\r\n\t,ROW_NUMBER() OVER(ORDER BY a.agentid) AS P_ROW\r\nfrom \r\n\t(\r\n\t\tselect b.agentid,sum(realvalue) as tradeAmt,sum(promAmt) as promAmt,sum(profits) as profits\r\n\t\tfrom orderbankamt a with(nolock)\r\n\t\t\t\tleft join orderbank b with(nolock) \r\n\t\t\t\t\t\t\ton a.orderid = b.orderid\t\t\r\n\t\twhere a.[status] = 2 and b.agentid > 0 and processingtime >= @begintime and processingtime <= @endtime and (b.agentid = @userid or @userid = 0)\r\n\t\tgroup by b.agentid\r\n\t) a \r\n\tleft join userbase b with(nolock) \r\n\t\t\t\t\ton a.agentid = b.id\r\nwhere a.P_ROW BETWEEN @page*@pagesize+1 AND @page*@pagesize+@pagesize";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@begintime", SqlDbType.DateTime, 8), new SqlParameter("@endtime", SqlDbType.DateTime, 8), new SqlParameter("@page", SqlDbType.Int, 10), new SqlParameter("@pagesize", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10) };
                commandParameters[0].Value = sdt;
                commandParameters[1].Value = edt;
                commandParameters[2].Value = page;
                commandParameters[3].Value = pagesize;
                commandParameters[4].Value = agentid;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataSet AgentStat4(int userid, int typeid, string sdt, string edt, int pagesize, int page, string orderby)
        {
            try
            {
                string commandText = "\r\nselect count(*) C from\r\n(select mydate,typeId,userId\r\nfrom usersOrderIncome with(nolock)\r\nwhere 1=1\r\nand (mydate >= @sdate or @sdate = '')\r\nand (mydate >= @edate or @edate = '')\r\nand (userId = @userId or @userId = 0)\r\nand (typeId = @typeId or @typeId = 0)\r\ngroup by mydate,typeId,userId) a\r\n\r\nselect mydate,userId,username,full_name,d.typeId,sumpay,pecent,P_ROW,f.modetypename\r\nfrom(\r\nselect a.mydate,a.userId,c.username,c.full_name,a.typeId,a.sumpay,a.sumpay/b.total pecent,ROW_NUMBER() OVER(ORDER BY a.mydate) AS P_ROW\r\nfrom \r\n(select mydate, userId, typeId,sum(sumpay) as sumpay\r\nfrom usersOrderIncome with(nolock)\r\nwhere 1=1\r\nand (mydate >= @sdate or @sdate = '')\r\nand (mydate >= @edate or @edate = '')\r\nand (userId = @userId or @userId = 0)\r\nand (typeId = @typeId or @typeId = 0)\r\ngroup by mydate,typeId,userId ) a\r\nleft join (select mydate, userId,sum(sumpay) as total\r\nfrom usersOrderIncome with(nolock)\r\nwhere 1=1\r\nand (mydate >= @sdate or @sdate = '')\r\nand (mydate >= @edate or @edate = '')\r\nand (userId = @userId or @userId = 0)\r\nand (typeId = @typeId or @typeId = 0)\r\ngroup by mydate,userId ) b on a.mydate=b.mydate\r\nand a.userId = b.userId\r\nleft join userbase c on a.userId = c.id) d\r\nleft join channeltype f ON d.typeId = f.typeId\r\nwhere P_ROW BETWEEN @page*@pagesize+1 AND @page*@pagesize+@pagesize";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@sdate", SqlDbType.VarChar, 10), new SqlParameter("@edate", SqlDbType.VarChar, 10), new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@typeId", SqlDbType.Int, 10), new SqlParameter("@page", SqlDbType.Int, 10), new SqlParameter("@pagesize", SqlDbType.Int, 10) };
                commandParameters[0].Value = sdt;
                commandParameters[1].Value = edt;
                commandParameters[2].Value = userid;
                commandParameters[3].Value = typeid;
                commandParameters[4].Value = page;
                commandParameters[5].Value = pagesize;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static int BankOrder_DataCheck(int intput_userid, bool ischeckuserorder, string intput_userorder, out UserInfo userInfo)
        {
            int num = 0;
            userInfo = null;
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@intput_userid", SqlDbType.Int, 10), 
                    new SqlParameter("@ischeckuserorder", SqlDbType.Bit), 
                    new SqlParameter("@intput_userorder", SqlDbType.VarChar, 30), 
                    new SqlParameter("@result", SqlDbType.TinyInt)
                };
                commandParameters[0].Value = intput_userid;
                commandParameters[1].Value = ischeckuserorder;
                commandParameters[2].Value = intput_userorder;
                commandParameters[3].Direction = ParameterDirection.Output;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_checkdata", commandParameters);
                num = Convert.ToInt32(commandParameters[3].Value);
                if (((num != 0) || (set == null)) || (set.Tables.Count <= 0))
                {
                    return num;
                }
                userInfo = new UserInfo();
                DataRow row = set.Tables[0].Rows[0];
                userInfo.ID = intput_userid;
                userInfo.APIKey = row["apikey"].ToString();
                if (row["isdebug"] != DBNull.Value)
                {
                    userInfo.isdebug = Convert.ToInt32(row["isdebug"]);
                }
                if (row["manageId"] != DBNull.Value)
                {
                    userInfo.manageId = new int?(Convert.ToInt32(row["manageId"]));
                }
                //2017.2.15add
                userInfo.bank_limit = Convert.ToDecimal(row["bank_limit"].ToString());
                userInfo.wx_limit = Convert.ToDecimal(row["wx_limit"].ToString());
                userInfo.ali_limit = Convert.ToDecimal(row["ali_limit"].ToString());
                userInfo.qq_limit = Convert.ToDecimal(row["qq_limit"].ToString());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                num = 99;
            }
            return num;
        }

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SearchParam param2 = param[i];
                    if (param2.CmpOperator == "=")
                    {
                        SqlParameter parameter;
                        switch (param2.ParamKey.Trim().ToLower())
                        {
                            case "userid":
                                builder.Append(" AND [userid] = @userid");
                                parameter = new SqlParameter("@userid", SqlDbType.Int);
                                parameter.Value = (int) param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "stime":
                                builder.Append(" AND [mydate] >= @beginmydate");
                                parameter = new SqlParameter("@beginmydate", SqlDbType.VarChar, 10);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "etime":
                                builder.Append(" AND [mydate] <= @endmydate");
                                parameter = new SqlParameter("@endmydate", SqlDbType.VarChar, 10);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "fvaluefrom":
                                builder.Append(" AND [faceValue] >= @fvaluefrom");
                                parameter = new SqlParameter("@fvaluefrom", SqlDbType.Decimal, 9);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "fvalueto":
                                builder.Append(" AND [faceValue] <= @fvalueto");
                                parameter = new SqlParameter("@fvalueto", SqlDbType.Decimal, 9);
                                parameter.Value = param2.ParamValue;
                                paramList.Add(parameter);
                                break;

                            case "typeid":
                                builder.Append(" AND [typeId] = @typeId");
                                parameter = new SqlParameter("@typeId", SqlDbType.Int);
                                parameter.Value = (int) param2.ParamValue;
                                paramList.Add(parameter);
                                break;
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static DataSet BusinessStat4(DateTime sdt, DateTime edt, int page, int pagesize, string orderby)
        {
            try
            {
                string commandText = "\r\nselect count(0) as C\r\n\tfrom(\r\n\tselect manageid\r\n\tfrom v_order with(nolock)\r\n\twhere manageid > 0  and processingtime >= @sdt and processingtime <= @edt \r\n\tgroup by manageid) A\r\n\r\n\r\nselect D1.manageid,payAmt,promAmt,supplierAmt,realvalue,B.username,B.relname\r\nfrom(\r\n\tselect manageid,payAmt,promAmt,supplierAmt,realvalue,ROW_NUMBER() OVER(ORDER BY D.manageid) AS P_ROW \r\n\tfrom(\r\n\tselect manageid,sum(payAmt) payAmt,sum(commission) as promAmt,sum(supplierAmt) as supplierAmt,sum(realvalue) as realvalue\r\n\tfrom v_order with(nolock)\r\n\twhere manageid > 0 and processingtime >= @sdt and processingtime <= @edt  \r\n\tgroup by manageid) D \r\n)D1  left join manage B with(nolock)  on D1.manageid = B.id\r\nWHERE D1.P_ROW BETWEEN @page*@pagesize+1 AND @page*@pagesize+@pagesize\r\norder by " + orderby;
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@sdt", SqlDbType.DateTime, 8), new SqlParameter("@edt", SqlDbType.DateTime, 8), new SqlParameter("@page", SqlDbType.Int, 10), new SqlParameter("@pagesize", SqlDbType.Int, 10) };
                commandParameters[0].Value = sdt;
                commandParameters[1].Value = edt;
                commandParameters[2].Value = page;
                commandParameters[3].Value = pagesize;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataSet BusinessStat7(DateTime sdt, DateTime edt)
        {
            /*
            try
            {
                //string commandText = "\r\nselect '网银利润' as class,sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=102 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '支付宝利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=101 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '财付通利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=100 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '点卡利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_ordercard with(nolock) \r\nwhere status=2 and  processingtime>=@sdt and processingtime <= @edt";
                //string commandText = "\r\nselect '网银利润' as class,sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=102 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '微信利润' as class,sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid in (99,990,116) and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '支付宝利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid in (101,980) and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '财付通利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=100 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '点卡利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_ordercard with(nolock) \r\nwhere status=2 and  processingtime>=@sdt and processingtime <= @edt";
                string commandText = "\r\nselect '网银利润' as class,sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=102 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '微信利润' as class,sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid in (99,990,116) and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect 'QQ钱包利润' as class,sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid in (209) and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '支付宝利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid in (101,980) and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '财付通利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_orderbank with(nolock) \r\nwhere status=2 and typeid=100 and  processingtime>=@sdt and processingtime <= @edt\r\n\r\nunion all\r\n\r\nselect '点卡利润',sum(supplierAmt-isnull(payAmt,0)-isnull(promAmt,0)) amt\r\nfrom v_ordercard with(nolock) \r\nwhere status=2 and  processingtime>=@sdt and processingtime <= @edt";
                //commandText += "\r\n union all \r\n select '扣单' as class,sum(t.realvalue) amt from v_orderbank t where t.status=8 and  t.processingtime>=@sdt and t.processingtime <= @edt";
                commandText += "\r\n union all \r\n select '提现' as class,sum(t.charges + t.tax) amt from settled t where t.status = 8 and t.paytime>=@sdt and t.paytime <= @edt";
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@sdt", SqlDbType.DateTime, 8), 
                    new SqlParameter("@edt", SqlDbType.DateTime, 8)
                };
                commandParameters[0].Value = sdt;
                commandParameters[1].Value = edt;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }*/
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@sdt", SqlDbType.DateTime, 8), 
                    new SqlParameter("@edt", SqlDbType.DateTime, 8)
                };
                commandParameters[0].Value = sdt;
                commandParameters[1].Value = edt;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_profit_stat", commandParameters);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static CheckCardResult CheckCardRepeat(int userid, string userorderid, int cardtype, string cardno, string cardpwd)
        {
            CheckCardResult result = new CheckCardResult();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@cardtype", SqlDbType.Int, 10), new SqlParameter("@cardNo", SqlDbType.NVarChar, 100), new SqlParameter("@cardPwd", SqlDbType.NVarChar, 100), new SqlParameter("@userorder", SqlDbType.NVarChar, 30) };
                commandParameters[0].Value = userid;
                commandParameters[1].Value = cardtype;
                commandParameters[2].Value = cardno;
                commandParameters[3].Value = cardpwd;
                commandParameters[4].Value = userorderid;
                DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_checkcard_repeat", commandParameters).Tables[0];
                if ((table != null) && (table.Rows.Count > 0))
                {
                    DataRow row = table.Rows[0];
                    result.isRepeat = Convert.ToByte(row["repeat"]);
                    result.makeup = Convert.ToByte(row["makeup"]);
                    result.withhold = Convert.ToDecimal(row["withhold"]);
                    result.cardpwd = Convert.ToString(row["cardpwd"]);
                    result.supprate = Convert.ToDecimal(row["supprate"]);
                    result.supplierid = Convert.ToInt32(row["supplierid"]);
                }
                else
                {
                    result = null;
                }
                return result;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static int CheckCardRepeat2(int userid, string userorderid, int cardtype, string cardno, string cardpwd)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@cardtype", SqlDbType.Int, 10), new SqlParameter("@cardNo", SqlDbType.NVarChar, 100), new SqlParameter("@userorder", SqlDbType.NVarChar, 30) };
                commandParameters[0].Value = userid;
                commandParameters[1].Value = cardtype;
                commandParameters[2].Value = cardno;
                commandParameters[3].Value = userorderid;
                return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_checkcard_repeat2", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static CardReSendInfo CheckIsReSend(string orderid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 50) };
                commandParameters[0].Value = orderid;
                DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercard_Resend", commandParameters).Tables[0];
                if (table == null)
                {
                    return null;
                }
                CardReSendInfo info = null;
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    info.sysorderid = orderid;
                    if ((row["result"] != null) && (row["result"].ToString() != ""))
                    {
                        info.result = int.Parse(row["result"].ToString());
                    }
                    if ((row["seq"] != null) && (row["seq"].ToString() != ""))
                    {
                        info.seq = int.Parse(row["seq"].ToString());
                    }
                    if ((row["suppid"] != null) && (row["suppid"].ToString() != ""))
                    {
                        info.suppid = int.Parse(row["suppid"].ToString());
                    }
                    if ((row["typeid"] != null) && (row["typeid"].ToString() != ""))
                    {
                        info.typeid = int.Parse(row["typeid"].ToString());
                    }
                    if ((row["cardno"] != null) && (row["cardno"].ToString() != ""))
                    {
                        info.cardno = row["cardno"].ToString();
                    }
                    if ((row["cardpwd"] != null) && (row["cardpwd"].ToString() != ""))
                    {
                        info.cardpwd = row["cardpwd"].ToString();
                    }
                    if ((row["cardvalue"] != null) && (row["cardvalue"].ToString() != ""))
                    {
                        info.cardvalue = decimal.Parse(row["cardvalue"].ToString());
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

        public static decimal GetAgentIncome(int agentid, DateTime sdt, DateTime edt)
        {
            decimal num = 0M;
            try
            {
                string commandText = "select sum(amt) from trade where billType = 2 and userid = @userid and tradeTime >= @begintime and tradeTime < @endtime";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@begintime", SqlDbType.DateTime, 8), new SqlParameter("@endtime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = agentid;
                commandParameters[1].Value = sdt;
                commandParameters[2].Value = edt;
                object obj2 = DataBase.ExecuteScalar(CommandType.Text, commandText, commandParameters);
                if (obj2 == DBNull.Value)
                {
                    return 0M;
                }
                num = Convert.ToDecimal(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
            return num;
        }

        public static decimal GetAgentproAmt(int agentid, DateTime sdt, DateTime edt)
        {
            decimal num = 0M;
            try
            {
                string commandText = "declare @amt decimal(18,4)\r\nselect @amt = isnull(@amt,0)+ isnull(sum(promAmt),0) from v_order where agentid=@agentid and status = 2\r\n and processingtime > @begintime and processingtime < @endtime\r\n\r\nselect isnull(@amt,0)";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@agentid", SqlDbType.Int, 10), new SqlParameter("@begintime", SqlDbType.DateTime, 8), new SqlParameter("@endtime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = agentid;
                commandParameters[1].Value = sdt;
                commandParameters[2].Value = edt;
                object obj2 = DataBase.ExecuteScalar(CommandType.Text, commandText, commandParameters);
                if (obj2 == DBNull.Value)
                {
                    return 0M;
                }
                num = Convert.ToDecimal(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
            return num;
        }

        public static decimal GetAgentTotalAmt(int agentid, DateTime sdt, DateTime edt)
        {
            decimal num = 0M;
            try
            {
                string commandText = "declare @amt decimal(18,4)\r\nselect @amt = sum(realvalue) from v_orderbank \r\nwhere agentid=@agentid and status = 2 and processingtime > @begintime and processingtime < @endtime\r\n\r\nselect @amt = isnull(@amt,0)+ isnull(sum(realvalue),0) from v_order where agentid=@agentid and status = 2\r\n and processingtime > @begintime and processingtime < @endtime\r\n\r\nselect isnull(@amt,0)";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@agentid", SqlDbType.Int, 10), new SqlParameter("@begintime", SqlDbType.DateTime, 8), new SqlParameter("@endtime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = agentid;
                commandParameters[1].Value = sdt;
                commandParameters[2].Value = edt;
                object obj2 = DataBase.ExecuteScalar(CommandType.Text, commandText, commandParameters);
                if (obj2 == DBNull.Value)
                {
                    return 0M;
                }
                num = Convert.ToDecimal(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
            return num;
        }

        public static DataTable GetFailOrders()
        {
            string commandText = "select orderid from ordercard with(nolock) where typeid=106 and supplierID = 85\r\nand not exists(select 0 from ordercardamt with(nolock) where ordercard.orderid = ordercardamt.orderid)";
            return DataBase.ExecuteDataset(CommandType.Text, commandText, null).Tables[0];
        }

        public static DataTable GetFailOrders2(DateTime sdt, DateTime edt)
        {
            string commandText = "select userid,ordercard.typeId,channeltype.modetypename,supplierID,orderid,cardNo,refervalue,processingtime from ordercard with(nolock), channeltype \r\nwhere \r\n\tordercard.typeId=channeltype.typeId\r\nand\r\n\tprocessingtime >= @sdt and processingtime <= @edt\r\nand\r\n    makeup=0\r\nand not exists(select 0 from ordercardamt with(nolock) where ordercardamt.orderid = ordercard.orderid)";
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@sdt", SqlDbType.DateTime, 30), 
                new SqlParameter("@edt", SqlDbType.DateTime, 30) };
            commandParameters[0].Value = sdt;
            commandParameters[1].Value = edt;
            return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters).Tables[0];
        }

        public static CardReSendInfo GetResendInfo2(string orderid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 50) };
                commandParameters[0].Value = orderid;
                DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_ordercard_Resend2", commandParameters).Tables[0];
                if (table == null)
                {
                    return null;
                }
                CardReSendInfo info = null;
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    info.sysorderid = orderid;
                    if ((row["result"] != null) && (row["result"].ToString() != ""))
                    {
                        info.result = int.Parse(row["result"].ToString());
                    }
                    if ((row["seq"] != null) && (row["seq"].ToString() != ""))
                    {
                        info.seq = int.Parse(row["seq"].ToString());
                    }
                    if ((row["suppid"] != null) && (row["suppid"].ToString() != ""))
                    {
                        info.suppid = int.Parse(row["suppid"].ToString());
                    }
                    if ((row["typeid"] != null) && (row["typeid"].ToString() != ""))
                    {
                        info.typeid = int.Parse(row["typeid"].ToString());
                    }
                    if ((row["cardno"] != null) && (row["cardno"].ToString() != ""))
                    {
                        info.cardno = row["cardno"].ToString();
                    }
                    if ((row["cardpwd"] != null) && (row["cardpwd"].ToString() != ""))
                    {
                        info.cardpwd = row["cardpwd"].ToString();
                    }
                    if ((row["cardvalue"] != null) && (row["cardvalue"].ToString() != ""))
                    {
                        info.cardvalue = decimal.Parse(row["cardvalue"].ToString());
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

        public static DataTable GetUserDayAmt(int userId, string begin, string edate)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@begin", SqlDbType.VarChar, 10), new SqlParameter("@end", SqlDbType.VarChar, 10) };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = begin;
            commandParameters[2].Value = edate;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_stat_user_order_getdaymoney", commandParameters).Tables[0];
        }

        public static DataTable GetUserOrderCount(int userId, string begin, string edate)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@begin", SqlDbType.VarChar, 10), new SqlParameter("@end", SqlDbType.VarChar, 10) };
            commandParameters[0].Value = userId;
            commandParameters[1].Value = begin;
            commandParameters[2].Value = edate;
            return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_stat_user_ordercount_bychannel", commandParameters).Tables[0];
        }

        public static DataTable OrderSearch(int userId, string userorder)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10), new SqlParameter("@userorder", SqlDbType.VarChar, 30) };
                commandParameters[0].Value = userId;
                commandParameters[1].Value = userorder;
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_order_search", commandParameters);
                if ((set != null) && (set.Tables.Count > 0))
                {
                    return set.Tables[0];
                }
                return null;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_usersOrderIncome";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n"
                                    + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[mydate]\r\n      ,[typeId]\r\n      ,[modetypename]\r\n      ,[faceValue]\r\n      ,[payrate]\r\n      ,[s_num]\r\n      ,[userId]\r\n      ,[Username]\r\n      ,[full_name]\r\n      ,[sumpay],[chargeValue]", tables, wheres, orderby, key, pageSize, page, false)
                                    + "\r\nselect sum(sumpay) sumpay,sum(s_num) s_num,sum(chargeValue) chargeValue,sum(faceValue) faceValue,sum(totalFaceValue) totalFaceValue from V_usersOrderIncome where " + wheres;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        /// <summary>
        /// 客户专用！！
        /// </summary>
        /// <param name="searchParams"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static DataSet UserPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_usersOrderIncome_user";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("[id]\r\n      ,[mydate]\r\n      ,[typeId]\r\n      ,[modetypename]\r\n      ,[faceValue]\r\n      ,[payrate]\r\n      ,[s_num]\r\n      ,[userId]\r\n      ,[Username]\r\n      ,[full_name]\r\n      ,[sumpay]", tables, wheres, orderby, key, pageSize, page, false) + "\r\nselect sum(sumpay) sumpay,sum(s_num) s_num from V_usersOrderIncome where " + wheres;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public static DataTable Stat(int suppid, DateTime sdt, DateTime edt)
        {
            try
            {
                string commandText = "select b.typeId,c.modetypename,sum(realvalue) realvalue,sum(supplierAmt) supplierAmt,sum(payAmt) payAmt,sum(profits) profits \r\nfrom ordercardamt a with(nolock) \r\n\t\tleft join ordercard b with(nolock) on a.orderid = b.orderid\r\n\t\tleft join channeltype c with(nolock) on b.typeId = c.typeId\r\n\t\tleft join supplier d with(nolock) on b.supplierID = d.id\r\nwhere (a.[status] = 2 or a.[status] = 8)\r\nand (b.supplierID = @suppid or @suppid is null)\r\nand a.completetime >= @begindt\r\nand a.completetime < @enddt\r\ngroup by b.typeId,c.modetypename";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@suppid", SqlDbType.Int, 10), new SqlParameter("@begindt", SqlDbType.DateTime, 8), new SqlParameter("@enddt", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = suppid;
                if (suppid == 0)
                {
                    commandParameters[0].Value = DBNull.Value;
                }
                commandParameters[1].Value = sdt;
                commandParameters[2].Value = edt;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataTable StatForAgent(int agentid, DateTime sdt, DateTime edt)
        {
            try
            {
                string commandText = "select typeId,modetypename,sum(realvalue) as realvalue,sum(supplierAmt) supplierAmt,sum(promAmt) promAmt,sum(payAmt) payAmt\r\nfrom v_order\r\nwhere ([status] = 2 or [status] = 8)\r\nand (agentid = @agentid)\r\nand processingtime >= @begindt\r\nand processingtime < @enddt\r\ngroup by typeId,modetypename";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@agentid", SqlDbType.Int, 10), new SqlParameter("@begindt", SqlDbType.DateTime, 8), new SqlParameter("@enddt", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = agentid;
                commandParameters[1].Value = sdt;
                commandParameters[2].Value = edt;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static DataTable StatForBusiness(int manageId, DateTime sdt, DateTime edt)
        {
            try
            {
                string commandText = "select typeId,modetypename,sum(realvalue) as realvalue,sum(supplierAmt) supplierAmt,sum(commission) commission,sum(payAmt) payAmt\r\nfrom v_order\r\nwhere ([status] = 2 or [status] = 8)\r\nand (manageId = @manageId)\r\nand processingtime >= @begindt\r\nand processingtime < @enddt\r\ngroup by typeId,modetypename";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@manageId", SqlDbType.Int, 10), new SqlParameter("@begindt", SqlDbType.DateTime, 8), new SqlParameter("@enddt", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = manageId;
                commandParameters[1].Value = sdt;
                commandParameters[2].Value = edt;
                return DataBase.ExecuteDataset(CommandType.Text, commandText, commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static bool Update(string orderid, string bankcode, int suppid)
        {
            try
            {
                string commandText = "update orderbank set supplierId = @supplierId,paymodeId=@paymodeId where orderid = @orderid";
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                    new SqlParameter("@paymodeId", SqlDbType.VarChar, 10), 
                    new SqlParameter("@supplierId", SqlDbType.Int, 10) 
                };
                commandParameters[0].Value = orderid;
                commandParameters[1].Value = bankcode;
                commandParameters[2].Value = suppid;
                return (DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool UpdateCardOrderStatus(string orderid)
        {
            try
            {
                string commandText = "update ordercardamt set [status] = 1 where orderid=@orderid and [status] = 4";
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30) };
                commandParameters[0].Value = orderid;
                return (DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool UpdateCardOrderStatus(string orderid, int suppid, decimal refervalue)
        {
            try
            {
                string commandText = "declare @status tinyint\r\nset @status = 1\r\nselect @status = [status] from ordercardamt with(nolock) where orderid=@orderid\r\nset @status = isnull(@status,1)\r\n\r\nif (@status = 1 or @status = 4)\r\nbegin\r\n\tupdate ordercard set supplierID = @supplierID,refervalue=@refervalue where orderid=@orderid\r\n\tupdate ordercardamt set [status] = 1 where orderid=@orderid\r\nend";
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                    new SqlParameter("@supplierID", SqlDbType.Int, 10), 
                    new SqlParameter("@refervalue", SqlDbType.Decimal, 9) 
                };
                commandParameters[0].Value = orderid;
                commandParameters[1].Value = suppid;
                commandParameters[2].Value = refervalue;
                return (DataBase.ExecuteNonQuery(CommandType.Text, commandText, commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

