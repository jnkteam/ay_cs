namespace OriginalStudio.MSMQMessaging
{
    using OriginalStudio.IMessaging;
    using OriginalStudio.Model.Order;
    using System;
    using System.Messaging;

    /// <summary>
    /// 订单通知操作
    /// </summary>
    public class OrderBankNotify : BaseQueue, IOrderBankNotify
    {
        private static readonly string queuePath = OriginalStudio.Lib.SysConfig.MSMQSetting.BankNotifyPath; //配置  .\private$\banknotify
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
            base.transactionType = MessageQueueTransactionType.Automatic;
            base.Send(orderMessage);
        }
    }
}

