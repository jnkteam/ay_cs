﻿namespace KuaiCard.BLL
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Web;
    using System;
    using System.Net;
    using System.Text;
    using KuaiCard.MessagingFactory;

    public class OrderCardNotify
    {
        private static readonly IOrderCardNotify notifyQueue = QueueAccess.OrderCardNotify();

        public void DoNotify(OrderCardInfo order)
        {
            OrderNotifyBase.AsynchronousNotify(2, order);
        }

        /// <summary>
        /// 检查通知状态。
        /// </summary>
        /// <param name="stateInfo"></param>
        public void NotifyCheckStatus(object stateInfo)
        {
            try
            {
                OrderCard card = new OrderCard();
                KuaiCard.Model.Order.OrderCardNotify notify = (KuaiCard.Model.Order.OrderCardNotify) stateInfo;
                string callBackUrl = card.GetCallBackUrl(notify.orderInfo);
                string str2 = SystemApiHelper.Successflag(notify.orderInfo.version);
                if (string.IsNullOrEmpty(callBackUrl))
                {
                    notify.tmr.Dispose();
                    notify.tmr = null;
                }
                else
                {
                    bool flag = false;
                    string str3 = string.Empty;
                    OrderCardInfo orderInfo = notify.orderInfo;
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
                    try
                    {
                        str3 = WebClientHelper.GetString(callBackUrl, string.Empty, "GET", Encoding.GetEncoding("GB2312"), 0x186a0);
                    }
                    catch
                    {
                    }
                    if (notify.orderInfo.notifycount <= 10)
                    {
                        bool flag2 = str3.StartsWith(str2) || str3.ToLower().StartsWith(str2) || str3.ToLower().EndsWith("pstate=0");
                        notify.orderInfo.notifystat = flag2 ? 2 : 4;
                        notify.orderInfo.againNotifyUrl = callBackUrl;
                        notify.orderInfo.notifycontext = str3;
                        notify.orderInfo.notifytime = DateTime.Now;
                        card.UpdateNotifyInfo(notify.orderInfo);
                    }
                    if (((str3.ToLower() == str2) || (notify.orderInfo.notifycount >= 10)) && (notify != null))
                    {
                        notify.tmr.Dispose();
                        notify.tmr = null;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
        }

        public OrderCardInfo ReceiveFromQueue(int timeout)
        {
            return notifyQueue.Receive(timeout);
        }

        public string SynchronousNotify(string orderId)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(orderId))
            {
                return string.Empty;
            }
            OrderCard card = new OrderCard();
            OrderCardInfo model = card.GetModel(orderId);
            if (model == null)
            {
                return string.Empty;
            }
            string str = SystemApiHelper.Successflag(model.version);
            string callBackUrl = card.GetCallBackUrl(model);
            string str3 = string.Empty;
            try
            {
                flag = true;
                if (!string.IsNullOrEmpty(callBackUrl))
                {
                    str3 = WebClientHelper.GetString(callBackUrl, string.Empty, "GET", Encoding.GetEncoding("GB2312"), 0x186a0);
                }
            }
            catch (WebException exception)
            {
                str3 = exception.Status.ToString() + exception.Message;
                flag = true;
            }
            catch (Exception exception2)
            {
                ExceptionHandler.HandleException(exception2);
            }
            if ((str3.StartsWith(str) || str3.ToLower().StartsWith(str)) && (model.notifystat != 2))
            {
                model.notifystat = 2;
                model.againNotifyUrl = callBackUrl;
                model.notifycontext = str3;
                model.notifycount = 1;
                model.notifytime = DateTime.Now;
                card.UpdateNotifyInfo(model);
            }
            return str3;
        }
    }
}

