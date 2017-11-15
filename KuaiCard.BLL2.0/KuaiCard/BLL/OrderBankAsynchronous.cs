namespace KuaiCard.BLL
{
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using KuaiCard.MessagingFactory;

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

