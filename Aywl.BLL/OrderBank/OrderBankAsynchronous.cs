namespace OriginalStudio.BLL
{
    using OriginalStudio.IBLLStrategy;
    using OriginalStudio.IMessaging;
    using OriginalStudio.Model.Order;
    using System;
    using OriginalStudio.BLL.MessagingFactory;

    /// <summary>
    /// 订单事务异步操作操作（通过消息队列处理）。
    /// </summary>
    public class OrderBankAsynchronous : IOrderBankStrategy
    {
        private static readonly IOrderBank asynchOrder = QueueAccess.CreateBankOrder();

        public void Complete(OrderBankInfo order)
        {
            asynchOrder.Complete(order);
        }

        public long Insert(OrderBankInfo order)
        {
            asynchOrder.Send(order);

            return 0;
        }
    }
}

