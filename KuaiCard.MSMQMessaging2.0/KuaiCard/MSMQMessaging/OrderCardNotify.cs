namespace KuaiCard.MSMQMessaging
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using System.Messaging;

    public class OrderCardNotify : BaseQueue, IOrderCardNotify
    {
        private static readonly string queuePath = KuaiCard.SysConfig.MSMQSetting.CardNotifyPath;
        private static int queueTimeout = 20;

        public OrderCardNotify() : base(queuePath, queueTimeout)
        {
            base.queue.Formatter = new BinaryMessageFormatter();
        }

        public OrderCardInfo Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return (OrderCardInfo) ((Message) base.Receive()).Body;
        }

        public OrderCardInfo Receive(int timeout)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return this.Receive();
        }

        public void Send(OrderCardInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }
    }
}

