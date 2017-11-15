namespace KuaiCard.IBLLStrategy
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderSmsStrategy
    {
        void Insert(OrderSmsInfo order);
    }
}

