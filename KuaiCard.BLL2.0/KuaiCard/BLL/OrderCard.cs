namespace KuaiCard.BLL
{
    using KuaiCard.BLL.Channel;
    using KuaiCard.BLL.Payment;
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IDAL;
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using KuaiCard.Model.User;
    using KuaiCard.SysConfig;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Transactions;
    using System.Web;
    using KuaiCardLib.Data;
    using KuaiCard.DALFactory;
    using KuaiCard.MessagingFactory;


    public class OrderCard
    {
        private static readonly KuaiCard.IDAL.IOrderCard dal = DataAccess.CreateOrderCard();
        private static readonly IOrderCardStrategy orderInsertStrategy = LoadInsertStrategy();
        private static readonly KuaiCard.IMessaging.IOrderCard orderQueue = QueueAccess.CreateCardOrder();

        public string BuilderParms(OrderCardInfo orderinfo, string backurl)
        {
            StringBuilder builder = new StringBuilder();
            if ((orderinfo != null) && !string.IsNullOrEmpty(backurl))
            {
                string str;
                string aPIKey = UserFactory.GetCacheUserBaseInfo(orderinfo.userid).APIKey;
                if (orderinfo.ismulticard == 0)
                {
                    str = Cryptography.MD5(string.Format("orderid={0}&opstate={1}&ovalue={2}{3}", new object[] { orderinfo.userorder, orderinfo.opstate, decimal.Round(orderinfo.realvalue.Value, 0), aPIKey }));
                    builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(orderinfo.userorder));
                    builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(orderinfo.opstate));
                    builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(decimal.Round(orderinfo.realvalue.Value, 0).ToString()));
                    builder.AppendFormat("&ekaorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                    builder.AppendFormat("&ekatime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    builder.AppendFormat("&attach={0}", HttpUtility.UrlEncode(HttpUtility.UrlEncode(orderinfo.attach, Encoding.GetEncoding("GB2312"))));
                    builder.AppendFormat("&msg={0}", HttpUtility.UrlEncode(HttpUtility.UrlEncode(orderinfo.msg, Encoding.GetEncoding("GB2312"))));
                    builder.AppendFormat("&sign={0}", str);
                }
                else if (orderinfo.ismulticard == 1)
                {
                    str = Cryptography.MD5(string.Format("orderid={0}&cardno={1}&opstate={2}&ovalue={3}&ototalvalue={4}&attach={5}&msg={6}{7}", new object[] { orderinfo.userorder, orderinfo.cardNo, orderinfo.returnopstate, orderinfo.returnovalue, decimal.Round(orderinfo.realvalue.Value, 0), orderinfo.attach, orderinfo.msg, aPIKey }));
                    builder.AppendFormat("orderid={0}", HttpUtility.UrlEncode(orderinfo.userorder));
                    builder.AppendFormat("&cardno={0}", HttpUtility.UrlEncode(orderinfo.cardNo));
                    builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(orderinfo.returnopstate));
                    builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(orderinfo.returnovalue));
                    decimal d = 0M;
                    if (orderinfo.realvalue.HasValue)
                    {
                        d = orderinfo.realvalue.Value;
                    }
                    builder.AppendFormat("&ototalvalue={0}", HttpUtility.UrlEncode(decimal.Round(d, 0).ToString()));
                    builder.AppendFormat("&attach={0}", HttpUtility.UrlEncode(HttpUtility.UrlEncode(orderinfo.attach, Encoding.GetEncoding("GB2312"))));
                    builder.AppendFormat("&msg={0}", HttpUtility.UrlEncode(HttpUtility.UrlEncode(orderinfo.msg, Encoding.GetEncoding("GB2312"))));
                    builder.AppendFormat("&ekaorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                    builder.AppendFormat("&ekatime={0}", HttpUtility.UrlEncode(orderinfo.completetime.Value.ToString("yyyy/MM/dd HH:mm:ss")));
                    builder.AppendFormat("&sign={0}", str);
                }
            }
            return builder.ToString();
        }

        public void Complete(OrderCardInfo order)
        {
            orderInsertStrategy.Complete(order);
        }

        public DataTable DataItemsByOrderId(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return null;
            }
            return dal.DataItemsByOrderId(orderId);
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

        public bool DoCardComplete(int supplierId, string orderId, string supplierOrderId, int status, string opstate, string msg, string userviewmsg, decimal tranAMT, decimal suppAmt, string errtype, byte returnmethod)
        {
            return this.DoCardComplete(supplierId, orderId, supplierOrderId, status, opstate, msg, userviewmsg, tranAMT, suppAmt, true, errtype, returnmethod);
        }

        public bool DoCardComplete(int supplierId, string orderId, string supplierOrderId, int status, string opstate, string msg, string userviewmsg, decimal tranAMT, decimal suppAmt, bool isDeduct, string errtype, byte returnmethod)
        {
            try
            {
                OrderCardInfo order = WebCache.GetCacheService().RetrieveObject(orderId) as OrderCardInfo;
                if (order == null)
                {
                    order = this.GetModel(orderId);
                }
                if (order != null)
                {
                    order.supplierId = supplierId;
                    UserInfo cacheUserBaseInfo = UserFactory.GetCacheUserBaseInfo(order.userid);
                    order.method = returnmethod;
                    order.cardversion = cacheUserBaseInfo.cardversion;
                    order.orderid = orderId;
                    order.status = status;
                    if (((isDeduct && SysConfig.isOpenDeduct) && ((status == 2) && (cacheUserBaseInfo != null))) && (new Random(Guid.NewGuid().GetHashCode()).Next(1, 0x3e8) <= cacheUserBaseInfo.CPSDrate))
                    {
                        order.status = 8;
                    }
                    order.realvalue = new decimal?(tranAMT);
                    order.supplierId = supplierId;
                    order.completetime = new DateTime?(DateTime.Now);
                    if (order.payRate <= 0M)
                    {
                        if (order.ordertype == 8)
                        {
                            order.payRate = Channelsupplier.GetPayRate(order.typeId, order.supplierId);
                        }
                        if (order.payRate <= 0M)
                        {
                            order.payRate = PayRateFactory.GetUserPayRate(cacheUserBaseInfo, order.userid, order.typeId);
                        }
                    }
                    order.payAmt = order.payRate * tranAMT;
                    if (suppAmt > 0M)
                    {
                        order.supplierRate = suppAmt / order.refervalue;
                        order.supplierAmt = suppAmt;
                    }
                    else
                    {
                        decimal rate = SupplierPayRateFactory.GetRate(supplierId, order.typeId);
                        order.supplierRate = rate;
                        order.supplierAmt = rate * tranAMT;
                    }
                    if (order.agentId > 0)
                    {
                        order.promRate = PayRateFactory.GetUserPayRate(order.agentId, order.typeId);
                        order.promAmt = (order.promRate - order.payRate) * tranAMT;
                        if (order.promAmt < 0M)
                        {
                            order.promAmt = 0M;
                        }
                    }
                    order.profits = (order.supplierAmt - order.payAmt) - order.promAmt;
                    order.opstate = opstate;
                    order.msg = msg;
                    if (!string.IsNullOrEmpty(supplierOrderId))
                    {
                        order.supplierOrder = supplierOrderId;
                    }
                    order.errtype = errtype;
                    this.Complete(order);
                    if (order.ordertype == 1)
                    {
                        new KuaiCard.BLL.OrderCardNotify().DoNotify(order);
                    }
                    WebCache.GetCacheService().RemoveObject(orderId);
                }
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool DoMultiCardComplete(int supplierId, string orderId, int serial, string supplierOrderId, int status, string opstate, string msg, decimal tranAMT, decimal suppAmt, bool isDeduct)
        {
            try
            {
                string objId = orderId + serial.ToString();
                CardItemInfo order = WebCache.GetCacheService().RetrieveObject(objId) as CardItemInfo;
                if (order == null)
                {
                    order = this.GetItemModel(orderId, serial);
                }
                if (order != null)
                {
                    order.status = status;
                    order.realvalue = tranAMT;
                    order.suppid = supplierId;
                    order.opstate = opstate;
                    order.msg = msg;
                    order.completetime = new DateTime?(DateTime.Now);
                    if (status == 2)
                    {
                        order.payrate = new decimal?(PayRateFactory.GetUserPayRate(order.userid, order.cardtype));
                        if (order.agentId > 0)
                        {
                            order.promrate = PayRateFactory.GetUserPayRate(order.agentId, order.cardtype);
                        }
                    }
                    bool allCompleted = false;
                    string str2 = string.Empty;
                    string ovalue = string.Empty;
                    decimal ototalvalue = 0M;
                    this.ItemComplete(order, out allCompleted, out str2, out ovalue, out ototalvalue);
                    if (allCompleted)
                    {
                        OrderCardInfo model = WebCache.GetCacheService().RetrieveObject(orderId) as OrderCardInfo;
                        if (model == null)
                        {
                            model = this.GetModel(orderId);
                        }
                        if (model != null)
                        {
                            model.opstate = str2;
                            model.ovalue = ovalue;
                            model.realvalue = new decimal?(ototalvalue);
                            new KuaiCard.BLL.OrderCardNotify().DoNotify(model);
                            WebCache.GetCacheService().RemoveObject(orderId);
                        }
                    }
                }
                WebCache.GetCacheService().RemoveObject(objId);
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public string GenerateUniqueOrderId(int typeId)
        {
            string str = "00";
            if (typeId.ToString().Length == 3)
            {
                str = typeId.ToString().Substring(1);
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string objId = DateTime.Now.ToString("yyMMddHHmmssff") + str + random.Next(0x3e8).ToString("0000");
            if (WebCache.GetCacheService().RetrieveObject(objId) != null)
            {
                return this.GenerateUniqueOrderId(typeId);
            }
            WebCache.GetCacheService().AddObject(objId, objId);
            return objId;
        }

        public string GetCallBackUrl(OrderCardInfo orderinfo)
        {
            return SystemApiHelper.GetCardBackUrl(orderinfo);
        }

        public CardItemInfo GetItemModel(string orderId, int serial)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return null;
            }
            return dal.GetItemModel(orderId, serial);
        }

        public OrderCardInfo GetModel(long id)
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

        public OrderCardInfo GetModel(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return null;
            }
            return dal.GetModel(orderId);
        }

        public OrderCardInfo GetModel(long id, int userid)
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

        public void Insert(OrderCardInfo order)
        {
            orderInsertStrategy.Insert(order);
        }

        public void InsertItem(CardItemInfo order)
        {
            orderInsertStrategy.InsertItem(order);
        }

        public void ItemComplete(CardItemInfo order, out bool allCompleted, out string opstate, out string ovalue, out decimal ototalvalue)
        {
            orderInsertStrategy.ItemComplete(order, out allCompleted, out opstate, out ovalue, out ototalvalue);
        }

        private static IOrderCardStrategy LoadInsertStrategy()
        {
            string orderCardStrategyAssembly = RuntimeSetting.OrderCardStrategyAssembly;
            string orderCardStrategyClass = RuntimeSetting.OrderCardStrategyClass;
            return (IOrderCardStrategy) Assembly.Load(orderCardStrategyAssembly).CreateInstance(orderCardStrategyClass);
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return dal.PageSearch(searchParams, pageSize, page, orderby);
        }

        public object ReceiveFromQueue(int timeout)
        {
            return orderQueue.Receive(timeout);
        }

        public void ReceiveSuppResult(int suppId, string orderId, string supplierOrderId, int status, string opstate, string msg, decimal tranAMT, decimal suppAmt, string errtype)
        {
            string userviewmsg = msg;
            this.ReceiveSuppResult(true, suppId, orderId, supplierOrderId, status, opstate, msg, userviewmsg, tranAMT, suppAmt, errtype, 1);
        }

        public void ReceiveSuppResult(int suppId, string orderId, string supplierOrderId, int status, string opstate, string msg, string userviewmsg, decimal tranAMT, decimal suppAmt, string errtype, byte returnmethod)
        {
            this.ReceiveSuppResult(true, suppId, orderId, supplierOrderId, status, opstate, msg, userviewmsg, tranAMT, suppAmt, errtype, returnmethod);
        }

        public void ReceiveSuppResult(bool iscache, int suppId, string orderId, string supplierOrderId, int status, string opstate, string msg, string userviewmsg, decimal tranAMT, decimal suppAmt, string errtype, byte returnmethod)
        {
            try
            {
                if (iscache)
                {
                    string key = "ReceiveSuppResult" + orderId;
                    object obj2 = HttpRuntime.Cache[key];
                    if (obj2 != null)
                    {
                        return;
                    }
                    HttpRuntime.Cache.Insert(key, status, null, DateTime.Now.AddSeconds(10.0), TimeSpan.Zero);
                }
                this.DoCardComplete(suppId, orderId, supplierOrderId, status, opstate, msg, userviewmsg, tranAMT, suppAmt, errtype, returnmethod);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
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

        public bool RepairOrder(int suppId, string orderId, string supplierOrderId, int status, string opstate, string msg, string userviewMsg, decimal tranAMT, decimal suppAmt, string errtype, byte method)
        {
            try
            {
                if (orderId.Length > 20)
                {
                    string str = orderId.Substring(0, 20);
                    int serial = Convert.ToInt32(orderId.Substring(20));
                    this.DoMultiCardComplete(suppId, str, serial, supplierOrderId, status, opstate, msg, tranAMT, suppAmt, false);
                    return true;
                }
                return this.DoCardComplete(suppId, orderId, supplierOrderId, status, opstate, msg, userviewMsg, tranAMT, suppAmt, errtype, method);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool UpdateNotifyInfo(OrderCardInfo order)
        {
            if (order == null)
            {
                return false;
            }
            return dal.Notify(order);
        }
    }
}

