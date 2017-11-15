namespace KuaiCard.BLL
{
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using KuaiCard.MessagingFactory;

    public class OrderSmsAsynchronous : IOrderSmsStrategy
    {
        private static readonly IOrderSms asynchOrder = QueueAccess.CreateSmsOrder();

        public void Complete(OrderSmsInfo order)
        {
            asynchOrder.Complete(order);
        }

        public void Insert(OrderSmsInfo order)
        {
            asynchOrder.Send(order);
        }
    }
}

