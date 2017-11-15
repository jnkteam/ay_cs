namespace KuaiCard.BLL
{
    using KuaiCard.BLL.Order.Notify;
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using KuaiCard.MessagingFactory;

    public class OrderNotifyBase
    {
        private static readonly IOrderBankNotify banknotifyQueue = QueueAccess.OrderBankNotify();
        private const int BUFFER_SIZE = 1024;
        private static readonly IOrderCardNotify cardnotifyQueue = QueueAccess.OrderCardNotify();
        private const int DefaultTimeout = 120000;
        private static readonly IOrderSmsNotify smsnotifyQueue = QueueAccess.OrderSmsNotify();

        public static void AsynchronousNotify(int oclass, object obj)
        {
            string notifyUrl = GetNotifyUrl(oclass, obj);
            if (!string.IsNullOrEmpty(notifyUrl))
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(notifyUrl);
                    RequestState state = new RequestState();
                    state.orderclass = oclass;
                    state.order = obj;
                    state.url = notifyUrl;
                    state.request = request;
                    ThreadPool.RegisterWaitForSingleObject(request.BeginGetResponse(new AsyncCallback(OrderNotifyBase.RespCallback), state).AsyncWaitHandle, 
                                                                                    new WaitOrTimerCallback(OrderNotifyBase.TimeoutCallback), 
                                                                                    state,
                                                                                    120000, 
                                                                                    true);
                    /* 参数
                    waitObject
                    要注册的 WaitHandle。使用 WaitHandle 而非 Mutex。
                    callBack
                    waitObject 参数终止时调用的 WaitOrTimerCallback 委托。
                    state
                    传递给委托的对象。
                    timeout
                    TimeSpan 表示的超时时间。如果 timeout 为零，则函数测试对象的状态并立即返回。如果 timeout 为 -1，则函数的超时间隔永远不过期。
                    executeOnlyOnce
                    如果为 true，表示在调用了委托后，线程将不再在 waitObject 参数上等待；如果为 false，表示每次完成等待操作后都重置计时器，直到注销等待。
                     */
                }
                catch (WebException exception)
                {
                    string callbacktext = exception.Status.ToString() + exception.Message;
                    UpdatetoDB(oclass, obj, notifyUrl, 1, callbacktext, false, callbacktext);
                }
                catch (Exception exception2)
                {
                    KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderNotifyBase.AsynchronousNotify错误：" + exception2.Message.ToString());
                    ExceptionHandler.HandleException(exception2);
                }
            }
        }

        public static string GetNotifyUrl(int oclass, object obj)
        {
            string callBackUrl = string.Empty;
            if (oclass == 1)
            {
                OrderBankInfo orderinfo = obj as OrderBankInfo;
                if (orderinfo == null)
                {
                    return string.Empty;
                }
                OrderBank bank = new OrderBank();
                return bank.GetCallBackUrl(orderinfo);
            }
            if (oclass == 2)
            {
                OrderCardInfo info2 = obj as OrderCardInfo;
                if (info2 == null)
                {
                    return string.Empty;
                }
                OrderCard card = new OrderCard();
                return card.GetCallBackUrl(info2);
            }
            if (oclass == 3)
            {
                OrderSmsInfo info3 = obj as OrderSmsInfo;
                if (info3 == null)
                {
                    return string.Empty;
                }
                callBackUrl = new OrderSms().GetCallBackUrl(info3);
            }
            return callBackUrl;
        }

        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            RequestState asyncState;
            try
            {
                asyncState = (RequestState) asyncResult.AsyncState;
                Stream streamResponse = asyncState.streamResponse;
                int count = streamResponse.EndRead(asyncResult);
                if (count > 0)
                {
                    asyncState.requestData.Append(Encoding.GetEncoding("GB2312").GetString(asyncState.BufferRead, 0, count));
                    IAsyncResult result = streamResponse.BeginRead(asyncState.BufferRead, 0, 1024, 
                                                    new AsyncCallback(OrderNotifyBase.ReadCallBack), asyncState);
                }
                else
                {
                    string callbacktext = string.Empty;
                    if (asyncState.requestData.Length > 1)
                    {
                        callbacktext = asyncState.requestData.ToString();
                    }
                    streamResponse.Close();
                    UpdatetoDB(asyncState.orderclass, asyncState.order, asyncState.url, 1, callbacktext, true, callbacktext);
                }
            }
            catch (WebException exception)
            {
                KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderNotifyBase.ReadCallBack错误：" + exception.Message.ToString());
                    
                asyncState = (RequestState) asyncResult.AsyncState;
                if (asyncState != null)
                {
                    string str2 = exception.Status.ToString() + exception.Message;
                    UpdatetoDB(asyncState.orderclass, asyncState.order, asyncState.url, 1, str2, false, str2);
                }
            }
        }

        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            RequestState asyncState;
            try
            {
                asyncState = (RequestState) asynchronousResult.AsyncState;
                HttpWebRequest request = asyncState.request;
                asyncState.response = (HttpWebResponse) request.EndGetResponse(asynchronousResult);
                Stream responseStream = asyncState.response.GetResponseStream();
                asyncState.streamResponse = responseStream;
                IAsyncResult result = responseStream.BeginRead(asyncState.BufferRead, 
                                                        0, 1024, new AsyncCallback(OrderNotifyBase.ReadCallBack), asyncState);
            }
            catch (WebException exception)
            {
                try
                {
                    HttpWebRequest request2 = ((RequestState)asynchronousResult.AsyncState).request;
                    KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderNotifyBase.RespCallback错误：" + exception.Message.ToString() +
                                                            Environment.NewLine + "。请求地址：" + request2.RequestUri.AbsoluteUri.ToString());
                }
                catch (Exception err)
                {
                    KuaiCardLib.Logging.LogHelper.Write("KuaiCard.BLL.OrderNotifyBase.RespCallback错误2：" + err.Message.ToString());
                }

                asyncState = (RequestState) asynchronousResult.AsyncState;
                if (asyncState != null)
                {
                    string callbacktext = exception.Status.ToString() + exception.Message;
                    UpdatetoDB(asyncState.orderclass, asyncState.order, asyncState.url, 1, callbacktext, false, callbacktext);
                }
            }
        }

        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                RequestState state2 = state as RequestState;
                if (state2 != null)
                {
                    string callbacktext = "访问超时";
                    UpdatetoDB(state2.orderclass, state2.order, state2.url, 1, callbacktext, false, callbacktext);
                }
            }
        }

        private static void UpdatetoDB(int oclass, object obj, string agurl, int times, string callbacktext, bool success, string errcode)
        {
            bool flag;
            if (oclass == 1)
            {
                OrderBankInfo order = obj as OrderBankInfo;
                if (order != null)
                {
                    callbacktext = callbacktext.Trim(); //16.12.21add
                    OrderBank bank = new OrderBank();
                    flag = SystemApiHelper.CheckCallBackIsSuccess(order.version, callbacktext);
                    order.notifystat = flag ? 2 : 4;        //通知状态：2成功；4失败
                    order.againNotifyUrl = agurl;
                    order.notifycontext = callbacktext;
                    order.notifycount = times;
                    order.notifytime = DateTime.Now;
                    bank.UpdateNotifyInfo(order);
                    if (order.notifystat != 2)
                    {
                        //通知不成的订单，放到消息队列里
                        banknotifyQueue.Send(order);
                    }
                }
            }
            else if (oclass == 2)
            {
                OrderCardInfo info2 = obj as OrderCardInfo;
                if (info2 != null)
                {
                    OrderCard card = new OrderCard();
                    flag = SystemApiHelper.CheckCallBackIsSuccess(info2.version, callbacktext);
                    info2.notifystat = flag ? 2 : 4;
                    info2.againNotifyUrl = agurl;
                    info2.notifycontext = callbacktext;
                    info2.notifycount = times;
                    info2.notifytime = DateTime.Now;
                    card.UpdateNotifyInfo(info2);
                    if (info2.notifystat != 2)
                    {
                        cardnotifyQueue.Send(info2);
                    }
                }
            }
            else if (oclass == 3)
            {
                OrderSmsInfo info3 = obj as OrderSmsInfo;
                if (info3 != null)
                {
                    OrderSms sms = new OrderSms();
                    info3.notifystat = success ? 2 : 4;
                    info3.issucc = success;
                    info3.errcode = errcode;
                    info3.againNotifyUrl = agurl;
                    info3.notifycontext = callbacktext;
                    info3.notifycount = times;
                    sms.UpdateNotifyInfo(info3);
                    if (info3.notifystat != 2)
                    {
                        smsnotifyQueue.Send(info3);
                    }
                }
            }
        }
    }
}

