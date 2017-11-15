namespace KuaiCard.BLL
{
    using KuaiCard.BLL.Channel;
    using KuaiCard.BLL.Payment;
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IDAL;
    using KuaiCard.IMessaging;
    using KuaiCard.Model;
    using KuaiCard.Model.Channel;
    using KuaiCard.Model.Order;
    using KuaiCard.Model.User;
    using KuaiCard.SysConfig;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Transactions;
    using System.Web;
    using KuaiCardLib.Data;
    using KuaiCard.DALFactory;
    using KuaiCard.MessagingFactory;
    using System.IO;

    public class OrderBank
    {
        private static readonly KuaiCard.IDAL.IOrderBank dal = DataAccess.CreateOrderBank();
        private static readonly IOrderBankStrategy orderInsertStrategy = LoadInsertStrategy();      //KuaiCard.BLL.OrderBankSynchronous
        private static readonly KuaiCard.IMessaging.IOrderBank orderQueue = QueueAccess.CreateBankOrder();

        public void BankOrderReturn(OrderBankInfo orderinfo)
        {
            string s = SystemApiHelper.NewBankNoticeUrl(orderinfo, false);
            //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank——orderinfo.version:" + orderinfo.version);
            //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank——s:" + s);
            if (s == "") return;

            try
            {
                if (orderinfo.version == "vyb1.00")
                {
                    HttpContext.Current.Response.Write(s);
                }
                else
                {
                    //KuaiCardLib.Logging.LogHelper.Write("BankOrderReturn地址:" + s);
                    HttpContext.Current.Response.Redirect(s, false);
                }
            }
            catch
            { 
            }
        }

        public void Complete(OrderBankInfo order)
        {
            orderInsertStrategy.Complete(order);
        }

        public bool Deduct(string orderId)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        flag = dal.Deduct(orderId);
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
                    order = this.GetModel(orderId);
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
                        order.payRate = PayRateFactory.GetUserPayRate(order.userid, order.typeId);
                    }
                    order.payAmt = order.payRate * tranAMT;
                    if (order.agentId >= 0)
                    {
                        order.promRate = PayRateFactory.GetUserPayRate(order.agentId, order.typeId);
                        order.promAmt = (order.promRate - order.payRate) * tranAMT;
                        if (order.promAmt < 0M)
                        {
                            order.promAmt = 0M;
                        }
                    }
                    if (order.supplierRate <= 0M)
                    {
                        ChannelInfo info3 = KuaiCard.BLL.Channel.Channel.GetModel(order.paymodeId, order.userid, false);
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
                    builder.AppendFormat("&t={0}", order.typeId);
                    builder.AppendFormat("&c={0}", order.paymodeId);
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
                    order = this.GetModel(orderId);
                    flag = true;
                }
                else
                {
                    flag = order.status == 1;   //如果未到帐，status=1
                }
                if ((order != null) && flag && (order.notifystat.ToString() != "2"))        //通知状态=2是成功通知
                {
                    //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank DoBankComplete:" + supplierId.ToString() +",订单:" + orderId + ",通知状态:" + order.notifystat.ToString());
                    //KuaiCardLib.Logging.LogHelper.Write("1");
                    order.orderid = orderId;
                    order.status = status;      //传入2：已完成       传入4：失败
                    if ((isDeduct && SysConfig.isOpenDeduct) && (status == 2))
                    {
                        //SysConfig.isOpenDeduct，系统表sysconfig内定义
                        //扣量后，状态直接为 已支付
                        UserInfo cacheUserBaseInfo = UserFactory.GetCacheUserBaseInfo(order.userid);
                        if ((cacheUserBaseInfo != null) && (new Random(Guid.NewGuid().GetHashCode()).Next(1, 1000) <= cacheUserBaseInfo.CPSDrate))
                        {
                            order.status = 8;   //扣量 = 8,
                        }
                    }
                    order.realvalue = new decimal?(tranAMT);        //实际支付金额
                    order.supplierId = supplierId;      //接口商
                    order.completetime = new DateTime?(DateTime.Now);
                    //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank——order.payRate:" + order.payRate.ToString());
                    //=======平台给商户=======
                    if (order.payRate <= 0M)
                    {
                        //给用户费率
                        order.payRate = PayRateFactory.GetUserPayRate(order.userid, order.typeId);
                    }
                    order.payAmt = order.payRate * tranAMT;
                    //=======供应商给平台==========
                    if ((suppAmt > 0M) && (order.refervalue > 0M))
                    {
                        //平台收到的金额
                        order.supplierRate = suppAmt / order.refervalue;
                        order.supplierAmt = suppAmt;
                    }
                    else
                    {
                        if (order.supplierRate <= 0M)
                        {
                            decimal rate = SupplierPayRateFactory.GetRate(supplierId, order.typeId);
                            order.supplierRate = rate;
                            order.supplierAmt = rate * tranAMT;
                        }
                        order.supplierAmt = order.supplierRate * tranAMT;
                    }
                    //=======供应商给平台==========

                    if (order.agentId > 0)
                    {
                        order.promRate = PayRateFactory.GetUserPayRate(order.agentId, order.typeId);
                        order.promAmt = (order.promRate - order.payRate) * tranAMT;
                        if (order.promAmt < 0M)
                        {
                            order.promAmt = 0M;
                        }
                    }
                    //KuaiCardLib.Logging.LogHelper.Write("2");
                    //========业务员===========
                    order.commission = 0;       //佣金，手续费
                    int? manageId = order.manageId;     //业务员
                    if ((manageId.GetValueOrDefault() > 0) && manageId.HasValue)
                    {
                        Manage model = ManageFactory.GetModel(order.manageId.Value);
                        if ((model != null) && (((manageId = model.commissiontype).GetValueOrDefault() == 2) && manageId.HasValue))
                        {
                            decimal num3 = tranAMT;
                            order.commission = num3 * model.commission;
                        }
                        if (order.commission < 0M)
                        {
                            order.commission = 0;
                        }
                    }
                    //平台
                    order.supplierAmt = order.supplierRate * tranAMT;
                    //利润：供应商给平台的—平台给商家的—代理所得—业务员所得
                    order.profits = ((order.supplierAmt - order.payAmt) - order.promAmt) - order.commission.Value;
                    ////%%%%%订单支付状态//%%%%%%%%%
                    order.opstate = opstate;    //0：支付失败        -1:支付失败
                    //%%%%%%%%%%%%%%%%%%
                    order.msg = msg;
                    if (!string.IsNullOrEmpty(supplierOrderId))
                    {
                        order.supplierOrder = supplierOrderId;
                    }
                    //KuaiCardLib.Logging.LogHelper.Write("3");
                    //处理订单反馈
                    this.Complete(order);
                    //KuaiCardLib.Logging.LogHelper.Write("4");
                    if (order.ordertype == 1)
                    {
                        //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank——DoNotify:开始");
                        if (order.status == 2)
                        {
                            //只有是成功的订单，才通知客户！！！@2017.2.19修改
                            if (false)       //KuaiCard.SysConfig.RuntimeSetting.SingleNoticeUser
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
                                new OrderBankNotify().DoNotify(order);
                            }
                        }
                        else
                        {
                            KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank.DoNotify:订单状态支付失败：" + order.orderid + ",返回状态:" + order.status.ToString());
                        }
                    }
                    //KuaiCardLib.Logging.LogHelper.Write("5");
                    if (removecache)
                    {
                        WebCache.GetCacheService().RemoveObject(orderId);
                    }

                    if (isreturn && (RuntimeSetting.Paycompletpage == "0"))
                    {
                        //KuaiCardLib.Logging.LogHelper.Write("6");
                        //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderBank——Order return:" + order.returnurl);
                        if (!string.IsNullOrEmpty(order.returnurl))
                        {
                            this.BankOrderReturn(order);
                        }
                        else
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.AppendFormat("o={0}", order.orderid);
                            builder.AppendFormat("&uo={0}", order.userorder);
                            builder.AppendFormat("&c={0}", order.paymodeId);
                            builder.AppendFormat("&t={0}", order.typeId);
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
            catch (ThreadAbortException)
            {
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public string GenerateUserUniqueOrderId(string userid)
        {
            //ServerId
            string ServerId = KuaiCard.SysConfig.RuntimeSetting.ServerId.ToString();
            long tick = DateTime.Now.Ticks;
            Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            ////random.Next(1000)返回一个小于所指定最大值的非负随机数
            string objId = ServerId + DateTime.Now.ToString("yyMMddHHmmss") + userid + random.Next(100000000).ToString("00000000");
            if (objId.Length > 30)
                objId = objId.Substring(0, 30); //2017.3.29加上取20位
            return objId;
        }

        public string GenerateTypeIdUniqueOrderId(string typeId)
        {
            //ServerId
            string ServerId = KuaiCard.SysConfig.RuntimeSetting.ServerId.ToString();
            long tick = DateTime.Now.Ticks;
            Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            ////random.Next(1000)返回一个小于所指定最大值的非负随机数
            string objId = ServerId + DateTime.Now.ToString("yyMMddHHmmss") + typeId + random.Next(100000000).ToString("00000000");
            if (objId.Length > 30)
                objId = objId.Substring(0, 30); //2017.3.29加上取20位
            return objId;
        }

        public string Generate16UniqueOrderId()
        {
            //ServerId
            string ServerId = KuaiCard.SysConfig.RuntimeSetting.ServerId.ToString();
            long tick = DateTime.Now.Ticks;
            Random random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            ////random.Next(1000)返回一个小于所指定最大值的非负随机数
            string objId = DateTime.Now.ToString("MMddHHmmss") + random.Next(1000000).ToString("000000");
            if (objId.Length > 30)
                objId = objId.Substring(0, 30); //2017.3.29加上取20位
            return objId;
        }

        public string GetCallBackUrl(OrderBankInfo orderinfo)
        {
            return SystemApiHelper.GetBankBackUrl(orderinfo, true);
        }

        public OrderBankInfo GetModel(long id)
        {
            if (id <= 0L)
            {
                return null;
            }
            try
            {
                return dal.GetModel(id);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public OrderBankInfo GetModel(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return null;
            }
            try
            {
                return dal.GetModel(orderId);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public OrderBankInfo GetModel(long id, int userid)
        {
            if (id <= 0L)
            {
                return null;
            }
            try
            {
                return dal.GetModel(id, userid);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

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

        public long Insert(OrderBankInfo order)
        {
            return orderInsertStrategy.Insert(order);
        }

        public long InsertNew(OrderBankInfo order)
        {
            return dal.Insert(order);
        }

        private static IOrderBankStrategy LoadInsertStrategy()
        {
            string orderStrategyAssembly = RuntimeSetting.OrderStrategyAssembly;
            string orderStrategyClass = RuntimeSetting.OrderStrategyClass;
            return (IOrderBankStrategy) Assembly.Load(orderStrategyAssembly).CreateInstance(orderStrategyClass);
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return dal.PageSearch(searchParams, pageSize, page, orderby);
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
            return dal.UserPageSearch(searchParams, pageSize, page, orderby);
        }

        public DataSet AdminPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return dal.AdminPageSearch(searchParams, pageSize, page, orderby);
        }

        /// <summary>
        /// 从消息队列取消息内容
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public OrderBankInfo ReceiveFromQueue(int timeout)
        {
            return orderQueue.Receive(timeout);
        }

        public bool ReDeduct(string orderId)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        flag = dal.ReDeduct(orderId);
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

        public bool UpdateNotifyInfo(OrderBankInfo order)
        {
            try
            {
                if (order == null)
                {
                    return false;
                }
                return dal.Notify(order);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        private bool WriteFile(string strContent, bool Append)
        {
            try
            {
                //建立文件
                string file = @"C:\网站调试\日志.txt";
                FileInfo fi = new FileInfo(file);
                //判断文件所在的文件夹是否存在，不存在创建新的文件夹
                if (!Directory.Exists(fi.DirectoryName))
                    Directory.CreateDirectory(fi.DirectoryName.ToString());

                using (StreamWriter sWriter = new StreamWriter(file, Append, Encoding.GetEncoding("UTF-8")))	//GB2312
                {
                    sWriter.AutoFlush = true;		//设置True,原因是设置写入器在每次执行写操作后就立即将缓存中的数据写入到流中

                    sWriter.WriteLine(strContent);

                    sWriter.Close();
                    return true;
                }

            }
            catch (System.Exception e)
            {
                throw new Exception("错误描述：" + e.Message);
            }
        }
    }
}

