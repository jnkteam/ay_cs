namespace KuaiCard.BLL
{
    using KuaiCard.BLL.User;
    using KuaiCard.Cache;
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IDAL;
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using KuaiCard.SysConfig;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Text;
    using System.Transactions;
    using System.Web;
    using KuaiCardLib.Data;
    using KuaiCard.DALFactory;
    using KuaiCard.MessagingFactory;

    public class OrderSms
    {
        private static readonly KuaiCard.IDAL.IOrderSms dal = DataAccess.CreateOrderSms();
        private static readonly IOrderSmsStrategy orderInsertStrategy = LoadInsertStrategy();
        private static readonly KuaiCard.IMessaging.IOrderSms orderQueue = QueueAccess.CreateSmsOrder();

        public string BuilderParms(OrderSmsInfo orderinfo, string backurl)
        {
            StringBuilder builder = new StringBuilder();
            if (!((orderinfo == null) || string.IsNullOrEmpty(backurl)))
            {
                string aPIKey = UserFactory.GetBaseModel(orderinfo.userid).APIKey;
                string str2 = Cryptography.MD5(string.Format("mob={0}&content={1}&opstate={2}&ovalue={3}{4}", new object[] { orderinfo.mobile, orderinfo.userMsgContenct, orderinfo.opstate, Convert.ToInt32(orderinfo.fee.ToString()), aPIKey }));
                builder.AppendFormat("mob={0}", HttpUtility.UrlEncode(orderinfo.mobile));
                builder.AppendFormat("&content={0}", HttpUtility.UrlEncode(orderinfo.userMsgContenct));
                builder.AppendFormat("&opstate={0}", HttpUtility.UrlEncode(orderinfo.opstate));
                builder.AppendFormat("&ovalue={0}", HttpUtility.UrlEncode(orderinfo.fee.ToString("0")));
                builder.AppendFormat("&ekaorderid={0}", HttpUtility.UrlEncode(orderinfo.orderid));
                builder.AppendFormat("&ekatime={0}", HttpUtility.UrlEncode(orderinfo.completetime.ToString("yyyy/MM/dd HH:mm:ss")));
                builder.AppendFormat("&sign={0}", str2);
            }
            return builder.ToString();
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

        public string GenerateUniqueOrderId()
        {
            string str = "99";
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string objId = DateTime.Now.ToString("yyMMddHHmmssff") + str + random.Next(0x3e8).ToString("0000");
            if (WebCache.GetCacheService().RetrieveObject(objId) != null)
            {
                return this.GenerateUniqueOrderId();
            }
            WebCache.GetCacheService().AddObject(objId, objId);
            return objId;
        }

        public string GetCallBackUrl(OrderSmsInfo orderinfo)
        {
            if (string.IsNullOrEmpty(orderinfo.notifyurl))
            {
                return string.Empty;
            }
            return (orderinfo.notifyurl + "?" + this.BuilderParms(orderinfo, orderinfo.notifyurl));
        }

        public OrderSmsInfo GetModel(int id)
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

        public OrderSmsInfo GetModel(string orderId)
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

        public OrderSmsInfo GetModel(int id, int userid)
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

        public void Insert(OrderSmsInfo order)
        {
            orderInsertStrategy.Insert(order);
        }

        private static IOrderSmsStrategy LoadInsertStrategy()
        {
            string orderSmsStrategyAssembly = RuntimeSetting.OrderSmsStrategyAssembly;
            string orderSmsStrategyClass = RuntimeSetting.OrderSmsStrategyClass;
            return (IOrderSmsStrategy) Assembly.Load(orderSmsStrategyAssembly).CreateInstance(orderSmsStrategyClass);
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            return dal.PageSearch(searchParams, pageSize, page, orderby);
        }

        public OrderSmsInfo ReceiveFromQueue(int timeout)
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

        public bool UpdateNotifyInfo(OrderSmsInfo order)
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
    }
}

