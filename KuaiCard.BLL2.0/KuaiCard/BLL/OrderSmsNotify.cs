namespace KuaiCard.BLL
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Web;
    using System;
    using System.Net;
    using System.Text;
    using KuaiCard.MessagingFactory;

    public class OrderSmsNotify
    {
        private static readonly IOrderSmsNotify notifyQueue = QueueAccess.OrderSmsNotify();

        public void DoNotify(OrderSmsInfo order)
        {
            OrderNotifyBase.AsynchronousNotify(3, order);
        }

        public void NotifyCheckStatus(object stateInfo)
        {
            try
            {
                OrderSms sms = new OrderSms();
                KuaiCard.Model.Order.OrderSmsNotify notify = (KuaiCard.Model.Order.OrderSmsNotify) stateInfo;
                string callBackUrl = sms.GetCallBackUrl(notify.orderInfo);
                if (string.IsNullOrEmpty(callBackUrl))
                {
                    notify.tmr.Dispose();
                    notify.tmr = null;
                }
                else
                {
                    bool flag = true;
                    string str2 = string.Empty;
                    try
                    {
                        flag = true;
                        str2 = WebClientHelper.GetString(callBackUrl, string.Empty, "GET", Encoding.GetEncoding("GB2312"), 0x186a0);
                    }
                    catch (WebException exception)
                    {
                        flag = false;
                        str2 = exception.Status.ToString() + exception.Message;
                    }
                    OrderSmsInfo orderInfo = notify.orderInfo;
                    orderInfo.notifycount++;
                    if (notify.tmr != null)
                    {
                        if (notify.orderInfo.notifycount == 1)
                        {
                            notify.tmr.Change(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(200.0));
                        }
                        if (notify.orderInfo.notifycount == 2)
                        {
                            notify.tmr.Change(TimeSpan.FromSeconds(60.0), TimeSpan.FromSeconds(200.0));
                        }
                        if (notify.orderInfo.notifycount == 3)
                        {
                            notify.tmr.Change(TimeSpan.FromSeconds(1200.0), TimeSpan.FromSeconds(200.0));
                        }
                        if (notify.orderInfo.notifycount == 4)
                        {
                            notify.tmr.Change(TimeSpan.FromSeconds(3600.0), TimeSpan.FromSeconds(200.0));
                        }
                        if (notify.orderInfo.notifycount == 5)
                        {
                            notify.tmr.Change(TimeSpan.FromSeconds(7200.0), TimeSpan.FromSeconds(200.0));
                        }
                    }
                    if (!((notify.orderInfo.notifycount != 5) && string.IsNullOrEmpty(str2.ToLower())))
                    {
                        notify.orderInfo.notifystat = (!string.IsNullOrEmpty(str2.ToLower()) && flag) ? 2 : 4;
                        notify.orderInfo.userorder = str2;
                        notify.orderInfo.againNotifyUrl = callBackUrl;
                        notify.orderInfo.notifycontext = str2;
                        notify.orderInfo.issucc = flag;
                        notify.orderInfo.errcode = str2;
                        sms.UpdateNotifyInfo(notify.orderInfo);
                    }
                    if ((!string.IsNullOrEmpty(str2.ToLower()) || (notify.orderInfo.notifycount >= 5)) && (notify.tmr != null))
                    {
                        notify.tmr.Dispose();
                        notify.tmr = null;
                    }
                }
            }
            catch (Exception exception2)
            {
                ExceptionHandler.HandleException(exception2);
            }
        }

        public OrderSmsInfo ReceiveFromQueue(int timeout)
        {
            return notifyQueue.Receive(timeout);
        }

        public string SynchronousNotify(OrderSmsInfo orderInfo)
        {
            if (orderInfo == null)
            {
                return string.Empty;
            }
            bool flag = false;
            OrderSms sms = new OrderSms();
            string callBackUrl = sms.GetCallBackUrl(orderInfo);
            string str2 = string.Empty;
            try
            {
                flag = true;
                str2 = WebClientHelper.GetString(callBackUrl, string.Empty, "GET", Encoding.GetEncoding("GB2312"), 0x186a0);
            }
            catch (WebException exception)
            {
                str2 = exception.Status.ToString() + exception.Message;
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionHandler.HandleException(exception2);
                flag = false;
            }
            if (!(string.IsNullOrEmpty(str2) || (orderInfo.notifystat == 2)))
            {
                orderInfo.notifystat = flag ? 2 : 4;
                orderInfo.againNotifyUrl = callBackUrl;
                orderInfo.notifycontext = str2;
                orderInfo.issucc = false;
                orderInfo.errcode = str2;
                sms.UpdateNotifyInfo(orderInfo);
            }
            return str2;
        }

        public string SynchronousNotify(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return string.Empty;
            }
            OrderSmsInfo model = new OrderSms().GetModel(orderId);
            if (model == null)
            {
                return string.Empty;
            }
            return this.SynchronousNotify(model);
        }
    }
}

