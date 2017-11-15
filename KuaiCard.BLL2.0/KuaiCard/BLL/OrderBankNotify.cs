namespace KuaiCard.BLL
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Text;
    using KuaiCardLib.Web;
    using System;
    using System.Net;
    using System.Text;
    using KuaiCard.MessagingFactory;

    public class OrderBankNotify
    {
        private static readonly IOrderBankNotify notifyQueue = QueueAccess.OrderBankNotify();

        /// <summary>
        /// 执行异步通知
        /// </summary>
        /// <param name="order"></param>
        public void DoNotify(OrderBankInfo order)
        {
            if (order != null)
            {
                OrderNotifyBase.AsynchronousNotify(1, order);
            }
        }

        public void NotifyCheckStatus(object stateInfo)
        {
            try
            {
                OrderBank bank = new OrderBank();
                OrderNotify notify = (OrderNotify) stateInfo;
                string callBackUrl = bank.GetCallBackUrl(notify.orderInfo);
                string str2 = SystemApiHelper.Successflag(notify.orderInfo.version);
                if (string.IsNullOrEmpty(callBackUrl))
                {
                    notify.tmr.Dispose();
                    notify.tmr = null;
                }
                else
                {
                    bool flag = false;
                    OrderBankInfo orderInfo = notify.orderInfo;
                    orderInfo.notifycount++;
                    if (notify.tmr != null)
                    {
                        switch (notify.orderInfo.notifycount)
                        {
                            case 1:
                                flag = notify.tmr.Change(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 2:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(1.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 3:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(2.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 4:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(5.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 5:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(10.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 6:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(20.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 7:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(30.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 8:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(60.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 9:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(120.0), TimeSpan.FromSeconds(200.0));
                                break;

                            case 10:
                                flag = notify.tmr.Change(TimeSpan.FromMinutes(240.0), TimeSpan.FromSeconds(200.0));
                                break;
                        }
                    }
                    string str3 = string.Empty;
                    try
                    {
                        str3 = WebClientHelper.GetString(callBackUrl, string.Empty, "GET", Encoding.GetEncoding("GB2312"), 0x186a0);
                    }
                    catch (WebException exception)
                    {
                        str3 = exception.Status.ToString() + exception.Message;
                    }
                    if (notify.orderInfo.notifycount <= 10)
                    {
                        bool flag2 = str3.StartsWith(str2) || str3.ToLower().StartsWith(str2);
                        notify.orderInfo.notifystat = flag2 ? 2 : 4;
                        notify.orderInfo.againNotifyUrl = callBackUrl;
                        notify.orderInfo.notifycontext = str3;
                        notify.orderInfo.notifytime = DateTime.Now;
                        bank.UpdateNotifyInfo(notify.orderInfo);
                    }
                    if (((str3.ToLower() == str2) || (notify.orderInfo.notifycount >= 10)) && (notify.tmr != null))
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

        public OrderBankInfo ReceiveFromQueue(int timeout)
        {
            return notifyQueue.Receive(timeout);
        }

        /// <summary>
        /// 异步通知
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string SynchronousNotify(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return string.Empty;
            }
            OrderBank bank = new OrderBank();
            OrderBankInfo model = bank.GetModel(orderId);
            if (model == null)
            {
                return string.Empty;
            }
            string str = SystemApiHelper.Successflag(model.version);
            string callBackUrl = bank.GetCallBackUrl(model);

            string str3 = string.Empty;
            try
            {
                if (PageValidate.IsUrl(callBackUrl))
                {
                    str3 = WebClientHelper.GetString(callBackUrl, string.Empty, "GET", Encoding.GetEncoding("GB2312"), 10000);

                    //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.Order.SynchronousNotify callBackUrl地址:" + callBackUrl);
                    //KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.Order.SynchronousNotify返回:" + str3 + Environment.NewLine + str + Environment.NewLine + model.notifystat.ToString());

                    if ((str3.StartsWith(str) || str3.ToLower().StartsWith(str) || str3.ToLower().EndsWith("pstate=0")) && (model.notifystat != 2))
                    {
                        //KuaiCardLib.Logging.LogHelper.Write("通知:" + model.orderid);

                        model.notifystat = 2;
                        model.againNotifyUrl = callBackUrl;
                        model.notifycontext = str3;
                        model.notifytime = DateTime.Now;
                        bank.UpdateNotifyInfo(model);
                    }
                }
                else
                {
                    str3 = "返回地址不正确！";
                }
            }
            catch
            {
            }
            return str3;
        }
    }
}

