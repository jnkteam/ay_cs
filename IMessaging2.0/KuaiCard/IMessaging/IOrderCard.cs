namespace KuaiCard.IMessaging
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderCard
    {
        void Complete(OrderCardInfo orderMessage);
        void ItemComplete(CardItemInfo orderMessage);
        object Receive();
        object Receive(int timeout);
        void Send(OrderCardInfo orderMessage);
        void SendItem(CardItemInfo orderMessage);
    }
}

