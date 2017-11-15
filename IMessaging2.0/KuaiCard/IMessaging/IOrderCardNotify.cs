namespace KuaiCard.IMessaging
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderCardNotify
    {
        OrderCardInfo Receive();
        OrderCardInfo Receive(int timeout);
        void Send(OrderCardInfo orderMessage);
    }
}

