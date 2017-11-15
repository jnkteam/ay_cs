namespace OriginalStudio.BLL
{
    using OriginalStudio.BLL.Channel;
    using OriginalStudio.BLL.User;
    using OriginalStudio.Cache;
    using OriginalStudio.IBLLStrategy;
    using OriginalStudio.IDAL;
    using OriginalStudio.IMessaging;
    using OriginalStudio.Model.Channel;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Web;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.BLL.MessagingFactory;
    using System.IO;
    using System.Transactions;
    using OriginalStudio.BLL.PayRate;
    using OriginalStudio.Model;
    using System.Data.SqlClient;
    using OriginalStudio.DBAccess;
    using OriginalStudio.Lib.Utils;

    public class OrderBank
    {
        private static readonly OriginalStudio.IDAL.IOrderBank orderDal = OriginalStudio.BLL.OrderBankAccess.CreateOrderBank();
        private static readonly IOrderBankStrategy orderInsertStrategy = LoadInsertStrategy();      //KuaiCard.BLL.OrderBankSynchronous
        private static readonly OriginalStudio.IMessaging.IOrderBank orderQueue = QueueAccess.CreateBankOrder();

        #region 加载操作对象
        
        private static IOrderBankStrategy LoadInsertStrategy()
        {
            string orderStrategyAssembly = RuntimeSetting.OrderStrategyAssembly;        //Aywl.BLL
            string orderStrategyClass = RuntimeSetting.OrderStrategyClass;          //OriginalStudio.BLL.OrderBankSynchronous
            return (IOrderBankStrategy)Assembly.Load(orderStrategyAssembly).CreateInstance(orderStrategyClass);
        }

        #endregion

        #region 插入订单

        /// <summary>
        /// 插入新订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public long InsertNew(OrderBankInfo order)
        {
            //return orderDal.Insert(order);
            return orderInsertStrategy.Insert(order);
        }

        /// <summary>
        /// 产生系统订单号。
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GenerateUniqueOrderId(int typeId, string userId)
        {
            //ServerId
            string ServerId = OriginalStudio.Lib.SysConfig.RuntimeSetting.ServerId.ToString();
            long tick = DateTime.Now.Ticks;
            Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            ////random.Next(1000)返回一个小于所指定最大值的非负随机数
            string objId = ServerId + DateTime.Now.ToString("yyMMddHHmmss") + userId + random.Next(100000000).ToString("00000000");
            if (objId.Length > 30)
                objId = objId.Substring(0, 30); //2017.3.29加上取20位
            return objId;
        }

        #endregion

        #region 获取订单信息

        /// <summary>
        /// 根据ID获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderBankInfo GetOrderbankModel(long id)
        {
            if (id <= 0L)
            {
                return null;
            }
            try
            {
                return orderDal.GetModel(id);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 根据OrderID获取订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderBankInfo GetOrderbankModel(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return null;
            }
            try
            {
                return orderDal.GetModel(orderId);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// 根据ID、UserID获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public OrderBankInfo GetOrderbankModel(long id, int userid)
        {
            if (id <= 0L)
            {
                return null;
            }
            try
            {
                return orderDal.GetModel(id, userid);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        #endregion

        #region 处理订单完成操作

        /// <summary>
        /// 获取订单异步通知地址
        /// </summary>
        /// <param name="orderinfo"></param>
        public void BankOrderReturn(OrderBankInfo orderinfo)
        {
            string s = SystemApiHelper.NewBankNoticeUrl(orderinfo, false);
            if (s == "") return;

            try
            {
                if (orderinfo.version == "vyb1.00")
                {
                    HttpContext.Current.Response.Write(s);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(s, false);
                }
            }
            catch
            { 
            }
        }

        /// <summary>
        /// 获取订单同步通知地址
        /// </summary>
        /// <param name="orderinfo"></param>
        /// <returns></returns>
        public string GetCallBackUrl(OrderBankInfo orderinfo)
        {
            return SystemApiHelper.GetBankBackUrl(orderinfo, true);
        }
        
        /// <summary>
        /// 订单完成处理。
        /// </summary>
        /// <param name="order"></param>
        public void Complete(OrderBankInfo order)
        {
            //新版采用这个方法
            orderDal.Complete(order);

            //旧版本，采用了TransactionScope事务操作。现在新版本complete，在数据库中采用了事务操作，所以可以直接使用新版本的。
            //orderInsertStrategy.Complete(order);
        }

        public bool DoBankComplete(int supplierId, string orderId, string supplierOrderId, 
                    int status, string opstate, string msg, decimal tranAMT, out string returnUrl)
        {
            try
            {
                returnUrl = string.Empty;
                bool flag = false;
                OrderBankInfo order = WebCache.GetCacheService().RetrieveObject(orderId) as OrderBankInfo;
                if (order == null)
                {
                    order = this.GetOrderbankModel(orderId);
                    flag = true;
                }
                else
                {
                    flag = order.status == 1;
                }
                if ((order != null) && flag)
                {
                    order.orderid = orderId;
                    order.status = status;
                    if (SysConfig.isOpenDeduct && (status == 2))
                    {
                        UserInfo cacheUserBaseInfo = UserFactory.GetCacheUserBaseInfo(order.userid);
                        if ((cacheUserBaseInfo != null) && (new Random(Guid.NewGuid().GetHashCode()).Next(1, 0x3e8) <= cacheUserBaseInfo.CPSDrate))
                        {
                            order.status = 8;
                        }
                    }
                    order.realvalue = new decimal?(tranAMT);
                    order.supplierId = supplierId;
                    order.completetime = new DateTime?(DateTime.Now);
                    if (order.payRate <= 0M)
                    {
                        order.payRate = PayRateFactory.GetUserPayRate(order.userid, order.channeltypeId);
                    }
                    order.payAmt = order.payRate * tranAMT;
                    if (order.agentId >= 0)
                    {
                        order.promRate = PayRateFactory.GetUserPayRate(order.agentId, order.channeltypeId);
                        order.promAmt = (order.promRate - order.payRate) * tranAMT;
                        if (order.promAmt < 0M)
                        {
                            order.promAmt = 0M;
                        }
                    }
                    if (order.supplierRate <= 0M)
                    {
                        ChannelInfo info3 = OriginalStudio.BLL.Channel.Channel.GetModel(order.channelcode, order.userid, false);
                        if (info3 != null)
                        {
                            order.supplierRate = info3.supprate;
                        }
                    }
                    order.commission = 0;
                    int? manageId = order.manageId;
                    if ((manageId.GetValueOrDefault() > 0) && manageId.HasValue)
                    {
                        Manage model = ManageFactory.GetModel(order.manageId.Value);
                        if ((model != null) && (((manageId = model.commissiontype).GetValueOrDefault() == 2) && manageId.HasValue))
                        {
                            decimal num2 = tranAMT;
                            order.commission = num2 * model.commission;
                        }
                        if (order.commission < 0M)
                        {
                            order.commission = 0;
                        }
                    }
                    order.supplierAmt = order.supplierRate * tranAMT;
                    order.profits = ((order.supplierAmt - order.payAmt) - order.promAmt) - order.commission.Value;
                    order.opstate = opstate;
                    order.msg = msg;
                    if (!string.IsNullOrEmpty(supplierOrderId))
                    {
                        order.supplierOrder = supplierOrderId;
                    }
                    this.Complete(order);
                    if (order.ordertype == 1)
                    {
                        new OrderBankNotify().DoNotify(order);
                    }
                    WebCache.GetCacheService().RemoveObject(orderId);
                }
                if (!string.IsNullOrEmpty(order.returnurl))
                {
                    returnUrl = SystemApiHelper.NewBankNoticeUrl(order, false);
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("o={0}", order.orderid);
                    builder.AppendFormat("&uo={0}", order.userorder);
                    builder.AppendFormat("&t={0}", order.channeltypeId);
                    builder.AppendFormat("&c={0}", order.channelcode);
                    builder.AppendFormat("&s={0}", order.status);
                    builder.AppendFormat("&v={0:f2}", tranAMT);
                    builder.AppendFormat("&e={0}", msg);
                    builder.AppendFormat("&u={0}", order.userid);
                    returnUrl = RuntimeSetting.SiteDomain + "/PayResult.aspx?" + builder.ToString();
                }
                return true;
            }
            catch (ThreadAbortException)
            {
                returnUrl = string.Empty;
                return true;
            }
            catch (Exception exception)
            {
                returnUrl = string.Empty;
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool DoBankComplete(int supplierId, string orderId, string supplierOrderId, 
                    int status, string opstate, string msg, decimal tranAMT, decimal suppAmt, bool removecache, bool isreturn)
        {
            return this.DoBankComplete(supplierId, orderId, supplierOrderId, 
                    status, opstate, msg, tranAMT, suppAmt, removecache, isreturn, true);
        }

        public bool DoBankComplete(int supplierId, string orderId, string supplierOrderId, 
                    int status, string opstate, string msg, decimal tranAMT, decimal suppAmt, 
                    bool removecache, bool isreturn, bool isDeduct)
        {
            try
            {
                bool flag = false;
                //Step1：取网银订单信息
                OrderBankInfo order = WebCache.GetCacheService().RetrieveObject(orderId) as OrderBankInfo;
                if (order == null)
                {
                    order = this.GetOrderbankModel(orderId);
                    flag = true;
                }
                else
                {
                    flag = order.status == 1;   //如果未到帐，status=1
                }
                if (order == null)
                {
                    OriginalStudio.Lib.Logging.LogHelper.Write("通知订单号为空：" + orderId);
                }
                else if ((order != null) && flag && (order.notifystat.ToString() != "2"))        //通知状态=2是成功通知
                {
                    //OriginalStudio.Lib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank DoBankComplete:" + supplierId.ToString() +",订单:" + orderId + ",通知状态:" + order.notifystat.ToString());
                    //OriginalStudio.Lib.Logging.LogHelper.Write("1");
                    order.orderid = orderId;
                    order.status = status;      //传入1：未完成   2：已完成       传入4：失败
                    if ((isDeduct && SysConfig.isOpenDeduct) && (status == 2))
                    {
                        //SysConfig.isOpenDeduct，系统表sysconfig内定义
                        //自动扣量后，状态直接为 已支付
                        MchUserBaseInfo cacheUserBaseInfo = MchUserFactory.GetCacheUserBaseInfo(order.userid);
                        if ((cacheUserBaseInfo != null) && (new Random(Guid.NewGuid().GetHashCode()).Next(1, 1000) <= cacheUserBaseInfo.CPSDrate))
                        {
                            order.status = 8;   //扣量 = 8,
                        }
                    }
                    order.realvalue = new decimal?(tranAMT);        //实际支付金额
                    order.supplierId = supplierId;      //接口商
                    order.completetime = new DateTime?(DateTime.Now);
                    //=======平台给商户=======
                    if (order.payRate <= 0M)
                    {
                        //给用户费率
                        order.payRate = SysPayRateFactory.GetUserChannelTypePayRate(order.userid, order.channeltypeId);
                    }
                    order.payAmt = order.payRate * tranAMT;
                    //OriginalStudio.Lib.Logging.LogHelper.Write(order.orderid + " 商户费率：" + order.payRate.ToString());

                    //=======供应商给平台==========
                    if ((suppAmt > 0M) && (order.refervalue > 0M))
                    {
                        //平台收到的金额。正常这个地方suppAmt= 0
                        order.supplierRate = suppAmt / order.refervalue;
                        order.supplierAmt = suppAmt;
                    }
                    else
                    {
                        //实际执行这个地方
                        if (order.supplierRate <= 0M)
                        {
                            decimal rate = SysSupplierPayRateFactory.GetSupplierChannelTypeRate(supplierId, order.channeltypeId);
                            order.supplierRate = rate;
                            order.supplierAmt = rate * tranAMT;
                        }
                        order.supplierAmt = order.supplierRate * tranAMT;
                    }
                    //=======供应商给平台==========
                    //OriginalStudio.Lib.Logging.LogHelper.Write(order.orderid + " 平台费率：" + order.supplierRate.ToString());

                    //代理
                    if (order.agentId > 0)
                    {
                        order.promRate = SysPayRateFactory.GetUserChannelTypePayRate(order.agentId, order.channeltypeId);
                        order.promAmt = (order.promRate - order.payRate) * tranAMT;
                        if (order.promAmt < 0M)
                        {
                            order.promAmt = 0M;
                        }

                        //OriginalStudio.Lib.Logging.LogHelper.Write(order.orderid + " 代理费率：" + order.promRate.ToString());
                    }

                    //========业务员===========
                    order.commission = 0;       //佣金，手续费
                    int? manageId = order.manageId;     //业务员
                    if ((manageId.Value > 0) && manageId.HasValue)
                    {
                        //取业务员对象
                        Manage model = ManageFactory.GetModel(manageId.Value);
                        if (model != null)
                        {
                            //业务员提成方式
                            int? manage_commissiontype = model.commissiontype;

                            if (manage_commissiontype == 2)
                            {
                                //网银订单提成比例
                                order.commission = tranAMT * model.commission;

                                //OriginalStudio.Lib.Logging.LogHelper.Write(order.orderid + " 业务员费率：" + model.commission.ToString());
                            }
                        }
                        if (order.commission < 0M)
                        {
                            order.commission = 0;
                        }
                    }

                    //利润：供应商给平台的—平台给商家的—代理所得—业务员所得
                    order.profits = ((order.supplierAmt - order.payAmt) - order.promAmt) - order.commission.Value;

                    ////%%%%%订单支付状态//%%%%%%%%%
                    order.opstate = opstate;    //0：支付成功        -1:支付失败
                    //%%%%%%%%%%%%%%%%%%
                    order.msg = msg;
                    if (!string.IsNullOrEmpty(supplierOrderId))
                    {
                        order.supplierOrder = supplierOrderId;
                    }
                    //OriginalStudio.Lib.Logging.LogHelper.Write("走到这里了吗？");
                    //处理订单反馈
                    this.Complete(order);
                    //OriginalStudio.Lib.Logging.LogHelper.Write("4");
                    if (order.ordertype == 1)
                    {
                        if (order.status == 2)
                        {
                            //只有是成功的订单，才通知客户！！！@2017.2.19修改
                            if (false)
                            {
                                //2017.7.3增加 单独程序通知下游
                                //KuaiCard.Model.Order.OrderBankNotifyQueue obj = new Model.Order.OrderBankNotifyQueue();
                                //obj.OrderID = order.orderid;
                                //obj.UserID = order.userid;
                                //obj.NotifyUrl = OrderNotifyBase.GetNotifyUrl(1, order);
                                //new OrderBankNotifyQueue().AddNotifyQueue(obj);
                            }
                            else
                            {
                                //通知下游
                                new OrderBankNotify().DoNotify(order);
                            }
                        }
                        else
                        {
                            OriginalStudio.Lib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank.DoNotify:订单状态支付失败：" + order.orderid + ",返回状态:" + order.status.ToString());
                        }
                    }
                    //OriginalStudio.Lib.Logging.LogHelper.Write("5");
                    if (removecache)
                    {
                        WebCache.GetCacheService().RemoveObject(orderId);
                    }

                    if (isreturn && (RuntimeSetting.Paycompletpage == "0"))
                    {
                        //OriginalStudio.Lib.Logging.LogHelper.Write("6");
                        //OriginalStudio.Lib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank——Order return:" + order.returnurl);
                        if (!string.IsNullOrEmpty(order.returnurl))
                        {
                            this.BankOrderReturn(order);
                        }
                        else
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.AppendFormat("o={0}", order.orderid);
                            builder.AppendFormat("&uo={0}", order.userorder);
                            builder.AppendFormat("&c={0}", order.channelcode);
                            builder.AppendFormat("&t={0}", order.channeltypeId);
                            builder.AppendFormat("&v={0:f2}", tranAMT);
                            builder.AppendFormat("&e={0}", msg);
                            builder.AppendFormat("&u={0}", order.userid);
                            builder.AppendFormat("&s={0}", order.status);

                            try
                            {
                                HttpContext.Current.Response.Redirect(RuntimeSetting.SiteDomain + "/PayResult.aspx?" + builder.ToString(), false);
                            }
                            catch
                            { }
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        /// <summary>
        /// 更新订单通知信息。
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool UpdateNotifyInfo(OrderBankInfo order)
        {
            try
            {
                if (order == null)
                {
                    return false;
                }
                return orderDal.Notify(order);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string getStatusView(int status)
        {
            switch (status)
            {
                case 1:
                    return "处理中";

                case 2:
                    return "已完成";

                case 4:
                    return "失败";
            }
            return "";
        }

        #endregion

        #region 扣单及还单

        /// <summary>
        /// 扣单。
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool Deduct(string orderId)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        flag = orderDal.Deduct(orderId);
                        scope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// 还单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ReDeduct(string orderId)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        flag = orderDal.ReDeduct(orderId);
                        scope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                    flag = false;
                }
            }
            return flag;
        }

        #endregion

        #region 从消息队列取订单信息

        /// <summary>
        /// 从消息队列取消息内容
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public OrderBankInfo ReceiveFromQueue(int timeout)
        {
            return orderQueue.Receive(timeout);
        }

        #endregion

        #region 商户订单检查

        /// <summary>
        /// 订单检查
        /// </summary>
        /// <param name="intput_merchant">商户编号</param>
        /// <param name="ischeckuserorder">是否检查订单</param>
        /// <param name="input_channelcode">通道代码</param>
        /// <param name="intput_userorder">客户订单</param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public int BankOrder_DataCheck(string intput_merchant,
                                                            bool ischeckuserorder,
                                                            string intput_userorder,
                                                            string input_channelcode,
                                                            out MchUserBaseInfo userInfo,
                                                            out DataTable userPayLimit)
        {
            int checkResult = 0;
            userInfo = new MchUserBaseInfo();
            userPayLimit = new DataTable();
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@intput_merchant", SqlDbType.VarChar, 20), 
                new SqlParameter("@ischeckuserorder", SqlDbType.Bit), 
                new SqlParameter("@intput_userorder", SqlDbType.VarChar, 50), 
                new SqlParameter("@input_channelcode", SqlDbType.VarChar, 50), 
                new SqlParameter("@result", SqlDbType.TinyInt)
            };
                commandParameters[0].Value = intput_merchant;
                commandParameters[1].Value = ischeckuserorder;
                commandParameters[2].Value = intput_userorder;
                commandParameters[3].Value = input_channelcode;
                commandParameters[4].Direction = ParameterDirection.Output;
                DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_checkdata", commandParameters);

                checkResult = Convert.ToInt32(commandParameters[4].Value);

                //用户信息
                DataRow row = ds.Tables[0].Rows[0];
                userInfo.UserID = Convert.ToInt32(row["userid"]);
                userInfo.ApiKey = row["apikey"].ToString();
                userInfo.IsDebug = Utils.StrToInt(row["isdebug"].ToString(), 0);
                userInfo.ManageId = Utils.StrToInt(row["manageId"].ToString(), 0);
                //用户限额
                userPayLimit = ds.Tables[1];
            }
            catch (Exception exception)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("BankOrder_DataCheck错误：" + exception.Message.ToString());
                checkResult = 99;
            }
            return checkResult;
        }


        #endregion

        #region 订单验签

        /// <summary>
        /// 订单验签
        /// </summary>
        /// <param name="merchant"></param>
        /// <param name="channeltype"></param>
        /// <param name="money"></param>
        /// <param name="orderid"></param>
        /// <param name="notify_url"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public bool BankMD5Check(string merchant, string channeltype, string money, string orderid, string notify_url, string key, string sign)
        {
            return (OriginalStudio.Lib.Security.Cryptography.MD5(string.Format("merchant={0}&type={1}&value={2}&orderid={3}&callbackurl={4}{5}",
                                                new object[] { merchant, channeltype, money, orderid, notify_url, key })).ToLower() == sign);
        }

        public bool BankMD5Check(string key, string sign)
        {
            SortedDictionary<string, string> waitSign = new SortedDictionary<string, string>();
            //判断是get还是post
            if (OriginalStudio.Lib.XRequest.IsGet())
            {
                foreach (string K in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[K]) && K.ToLower() != "sign")
                    {
                        waitSign.Add(K, HttpContext.Current.Request.QueryString[K].ToString());
                    }
                }
            }
            else if (OriginalStudio.Lib.XRequest.IsPost())
            {
                foreach (string K in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.Form[K]) && K.ToLower() != "sign")
                    {
                        waitSign.Add(K, HttpContext.Current.Request.Form[K].ToString());
                    }
                }
            }
            string tmpStr = "";
            foreach (var K in waitSign.Keys)
            {
                tmpStr += K + "=" + waitSign[K] + "&";
            }
            tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            return OriginalStudio.Lib.Security.Cryptography.MD5(tmpStr + key).ToLower() == sign;

            //return (OriginalStudio.Lib.Security.Cryptography.MD5(string.Format("merchant={0}&type={1}&value={2}&orderid={3}&callbackurl={4}{5}",
            //                                    new object[] { merchant, channeltype, money, orderid, notify_url, key })).ToLower() == sign);
        }

        #endregion

        #region 查询列表

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return orderDal.PageSearch(searchParams, pageSize, page, orderby);
        }

        /// <summary>
        /// 用户专用。
        /// </summary>
        /// <param name="searchParams"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataSet UserPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return orderDal.UserPageSearch(searchParams, pageSize, page, orderby);
        }

        public DataSet AdminPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return orderDal.AdminPageSearch(searchParams, pageSize, page, orderby);
        }

        #endregion

        #region 订单查询API

        /// <summary>
        /// 订单查询API
        /// </summary>
        /// <param name="merchantName"></param>
        /// <param name="userOrderId"></param>
        /// <param name="orderRow"></param>
        /// <returns>订单存在返回0，订单不存在返回99</returns>
        public int SearchOrderAPI(string merchantName, string userOrderId, out DataRow orderRow)
        {
            orderRow = null;
            SqlParameter[] commandParameters = new SqlParameter[] {
                new SqlParameter("@merchantName", SqlDbType.VarChar, 50), 
                new SqlParameter("@userorderid", SqlDbType.VarChar, 50), 
                new SqlParameter("@result", SqlDbType.TinyInt) 
            };
            commandParameters[0].Value = merchantName;
            commandParameters[1].Value = userOrderId;
            commandParameters[2].Direction = ParameterDirection.Output;
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_orderbank_SearchAPI", commandParameters);
            int num = Convert.ToInt32(commandParameters[2].Value);
            if (num == 0)
            {
                orderRow = set.Tables[0].Rows[0];
            }
            return num;
        }

        #endregion
    }
}
