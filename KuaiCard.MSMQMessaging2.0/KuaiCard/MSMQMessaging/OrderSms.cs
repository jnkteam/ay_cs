namespace KuaiCard.MSMQMessaging
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using System.Messaging;

    public class OrderSms : BaseQueue, IOrderSms
    {
        private static readonly string queuePath = KuaiCard.SysConfig.MSMQSetting.SmsOrderPath;
        private static int queueTimeout = 20;

        public OrderSms() : base(queuePath, queueTimeout)
        {
            base.queue.Formatter = new BinaryMessageFormatter();
        }

        public void Complete(OrderSmsInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
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

