namespace OriginalStudio.BLL
{
    using OriginalStudio.BLL.Order.Notify;
    using OriginalStudio.IMessaging;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using OriginalStudio.BLL.MessagingFactory;
    using OriginalStudio.Lib.Web;

    public class OrderNotifyBase
    {
        private static readonly IOrderBankNotify banknotifyQueue = QueueAccess.OrderBankNotify();
        private const int BUFFER_SIZE = 1024;
        private const int DefaultTimeout = 120000;

        /// <summary>
        /// 下发异步通知
        /// </summary>
        /// <param name="oclass"></param>
        /// <param name="obj"></param>
        public static void AsynchronousNotify(int oclass, object obj)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(doOrderNotifyThread));
            thread.Start(obj);
        }

        /// <summary>
        /// 下发访问线程调用。
        /// </summary>
        /// <param name="obj">订单信息</param>
        static void doOrderNotifyThread(object obj)
        {
            callOrderNotify(obj);
        }

        /// <summary>
        /// 调用通知下发订单。这个方法公开使用。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>正常：下游返回内容OK</returns>
        public static string callOrderNotify(object obj)
        {
            //1、获取订单下发通知地址
            string notifyUrl = "";
            OrderBankInfo orderinfo = obj as OrderBankInfo;
            if (orderinfo == null)
                notifyUrl = "";
            else
                notifyUrl = SystemApiHelper.GetBankBackUrl(orderinfo, true);
            if (string.IsNullOrEmpty(notifyUrl)) return "";

            //5s内取回数据
            string callbacktext = "";
            try
            {
                callbacktext = WebClientHelper.GetString(notifyUrl, string.Empty, "GET", Encoding.UTF8, 10 * 1000);

                UpdatetoDB(1, orderinfo, notifyUrl, 1, callbacktext, true, callbacktext);
            }
            catch (WebException webexp)
            {
                callbacktext = "";
                //网络访问错误
                UpdatetoDB(1, orderinfo, notifyUrl, 1, webexp.Message, false, webexp.Message);
            }
            catch (Exception exp)
            {
                callbacktext = "";
                //其余错误
                UpdatetoDB(1, orderinfo, notifyUrl, 1, exp.Message, false, exp.Message);
            }

            return callbacktext;
        }

        /// <summary>
        /// 更新订单通知状态。
        /// </summary>
        /// <param name="oclass"></param>
        /// <param name="obj"></param>
        /// <param name="agurl"></param>
        /// <param name="times"></param>
        /// <param name="callbacktext"></param>
        /// <param name="success"></param>
        /// <param name="errcode"></param>
        private static Boolean UpdatetoDB(int oclass, object obj, string agurl, int times, string callbacktext, bool success, string errcode)
        {
            bool flag = false;
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
                        //通知不成的订单，放到消息队列里。消息队列不能用，用控制台处理
                        //banknotifyQueue.Send(order);
                    }
                }
            }

            return flag;
        }
    }
}
