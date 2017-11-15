namespace KuaiCard.BLL.Order.Card
{
    using KuaiCard.BLL;
    using KuaiCard.Model.Order;
    using KuaiCardLib.ScheduledTask;
    using System;
    using System.Collections;
    using System.Threading;
    using KuaiCard.SysConfig;

    public class TaskInterval : IScheduledTaskExecute
    {
        private static int notifybatchSize = MSMQSetting.NotifyBatchSize_Card;
        private static int notifyqueueTimeout = MSMQSetting.NotifyQueueTimeout_Card;
        private static int notifythreadCount = MSMQSetting.NotifyThreadCount_Card;
        private static int notifyTransactionTimeout = MSMQSetting.NotifyTransactionTimeout_Card;

        public void Execute()
        {
            ProcessNotify();
        }

        private static void ProcessNotify()
        {
            TimeSpan span = TimeSpan.FromSeconds(Convert.ToDouble((int) (notifyTransactionTimeout * notifybatchSize)));
            KuaiCard.BLL.OrderCardNotify notify = new KuaiCard.BLL.OrderCardNotify();
            ArrayList list = new ArrayList();
            for (int i = 0; i < notifybatchSize; i++)
            {
                try
                {
                    OrderCardInfo info = notify.ReceiveFromQueue(notifyqueueTimeout);
                    info.notifycount = 0;
                    KuaiCard.Model.Order.OrderCardNotify state = new KuaiCard.Model.Order.OrderCardNotify();
                    state.orderInfo = info;
                    Timer timer = new Timer(new TimerCallback(notify.NotifyCheckStatus), state, 0, 0x3e8);
                    state.tmr = timer;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

