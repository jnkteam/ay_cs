namespace KuaiCard.IMessaging
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderSmsNotify
    {
        OrderSmsInfo Receive();
        OrderSmsInfo Receive(int timeout);
        void Send(OrderSmsInfo orderMessage);
    }
}

