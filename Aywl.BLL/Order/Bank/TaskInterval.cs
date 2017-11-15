namespace OriginalStudio.BLL.Order.Bank
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model.Order;
    using OriginalStudio.Lib.ScheduledTask;
    using System;
    using System.Threading;
    using OriginalStudio.Lib.SysConfig;

    public class TaskInterval : IScheduledTaskExecute
    {
        private static int notifybatchSize = MSMQSetting.NotifyBatchSize;
        private static int notifyqueueTimeout = MSMQSetting.NotifyQueueTimeout;
        private static int notifythreadCount = MSMQSetting.NotifyThreadCount;
        private static int notifyTransactionTimeout = MSMQSetting.NotifyTransactionTimeout;

        public void Execute()
        {
            ProcessNotify();
        }

        private static void ProcessNotify()
        {
            OrderBankNotify notify = new OrderBankNotify();
            for (int i = 0; i < notifybatchSize; i++)
            {
                try
                {
                    OrderBankInfo info = notify.ReceiveFromQueue(notifyqueueTimeout);
                    info.notifycount = 0;
                    OrderNotify state = new OrderNotify();
                    state.orderInfo = info;
                    Timer timer = new Timer(new TimerCallback(notify.NotifyCheckStatus), state, 0, 1000);
                    state.tmr = timer;

                    /*System.Threading.Timer:定义该类时，主要有四个参数。
                     * TimerCallBack,一个返回值为void,参数为object的委托,也是计时器执行的方法。
                     * Object state，计时器执行方法的的参数。
                     * int dueTime,调用 callback 之前延迟的时间量（以毫秒为单位）。指定 Timeout.Infinite 以防止计时器开始计时。指定零 (0) 以立即启动计时器。*/
                }
                catch
                {
                }
            }
        }
    }
}

