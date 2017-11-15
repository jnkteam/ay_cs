namespace KuaiCard.MSMQMessaging
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using System.Messaging;

    public class OrderCard : BaseQueue, IOrderCard
    {
        private static readonly string queuePath = KuaiCard.SysConfig.MSMQSetting.CardOrderPath;
        private static int queueTimeout = 20;

        public OrderCard() : base(queuePath, queueTimeout)
        {
            base.queue.Formatter = new BinaryMessageFormatter();
        }

        public void Complete(OrderCardInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }

        public void ItemComplete(CardItemInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }

        public object Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return ((Message) base.Receive()).Body;
        }

        public object Receive(int timeout)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return this.Receive();
        }

        public void Send(OrderCardInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }

        public void SendItem(CardItemInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }
    }
}

