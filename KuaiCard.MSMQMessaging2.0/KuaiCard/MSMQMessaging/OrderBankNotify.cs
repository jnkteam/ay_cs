namespace KuaiCard.MSMQMessaging
{
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using System.Messaging;

    public class OrderBankNotify : BaseQueue, IOrderBankNotify
    {
        private static readonly string queuePath = KuaiCard.SysConfig.MSMQSetting.BankNotifyPath;
        private static int queueTimeout = 20;

        public OrderBankNotify() : base(queuePath, queueTimeout)
        {
            base.queue.Formatter = new BinaryMessageFormatter();
        }

        public OrderBankInfo Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return (OrderBankInfo) ((Message) base.Receive()).Body;
        }

        public OrderBankInfo Receive(int timeout)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return this.Receive();
        }

        public void Send(OrderBankInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
        }
    }
}

