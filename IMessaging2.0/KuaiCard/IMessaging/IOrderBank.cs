namespace KuaiCard.IMessaging
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderBank
    {
        void Complete(OrderBankInfo orderMessage);
        OrderBankInfo Receive();
        OrderBankInfo Receive(int timeout);
        void Send(OrderBankInfo orderMessage);
    }
}

