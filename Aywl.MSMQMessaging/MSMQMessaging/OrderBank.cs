namespace OriginalStudio.MSMQMessaging
{
    using OriginalStudio.IMessaging;
    using OriginalStudio.Model.Order;
    using System;
    using System.Messaging;

    /// <summary>
    /// 订单通知操作。
    /// </summary>
    public class OrderBank : BaseQueue, IOrderBank
    {
        private static readonly string queuePath = OriginalStudio.Lib.SysConfig.MSMQSetting.BankOrderPath; //配置  .\private$\bankorder
        private static int queueTimeout = 20;

        public OrderBank() : base(queuePath, queueTimeout)
        {
            base.queue.Formatter = new BinaryMessageFormatter();
        }

        public void Complete(OrderBankInfo orderMessage)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(orderMessage);
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

