namespace KuaiCard.BLL
{
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IMessaging;
    using KuaiCard.Model.Order;
    using System;
    using System.Runtime.InteropServices;
    using KuaiCard.MessagingFactory;

    public class OrderCardAsynchronous : IOrderCardStrategy
    {
        private static readonly IOrderCard asynchOrder = QueueAccess.CreateCardOrder();

        public void Complete(OrderCardInfo order)
        {
            asynchOrder.Complete(order);
        }

        public void Insert(OrderCardInfo order)
        {
            asynchOrder.Send(order);
        }

        public void InsertItem(CardItemInfo order)
        {
            asynchOrder.SendItem(order);
        }

        public bool ItemComplete(CardItemInfo order, out bool allCompleted, out string opstate, out string ovalue, out decimal ototalvalue)
        {
            allCompleted = false;
            opstate = string.Empty;
            ovalue = string.Empty;
            ototalvalue = 0M;
            asynchOrder.ItemComplete(order);
            return true;
        }
    }
}

