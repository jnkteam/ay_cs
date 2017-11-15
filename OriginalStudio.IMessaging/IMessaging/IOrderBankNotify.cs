namespace OriginalStudio.IMessaging
{
    using OriginalStudio.Model.Order;
    using System;

    public interface IOrderBankNotify
    {
        OrderBankInfo Receive();
        OrderBankInfo Receive(int timeout);
        void Send(OrderBankInfo orderMessage);
    }
}

