namespace KuaiCard.MSMQMessaging
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using System.Messaging;

    public class OrderSmsNotify : BaseQueue, IOrderSmsNotify
    {
        private static readonly string queuePath = KuaiCard.SysConfig.MSMQSetting.SmsNotifyPath;
        private static int queueTimeout = 20;

        public OrderSmsNotify() : base(queuePath, queueTimeout)
        {
            base.queue.Formatter = new BinaryMessageFormatter();
        }

        public OrderSmsInfo Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return (OrderSmsInfo) ((Message) base.Receive()).Body;
        }

        public OrderSmsInfo Receive(int timeout)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return this.Receive();
        }

        public void Send(OrderSmsInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }
    }
}

