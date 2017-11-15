namespace KuaiCard.IMessaging
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderSms
    {
        void Complete(OrderSmsInfo orderMessage);
        OrderSmsInfo Receive();
        OrderSmsInfo Receive(int timeout);
        void Send(OrderSmsInfo orderMessage);
    }
}

