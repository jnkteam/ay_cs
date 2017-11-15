namespace KuaiCard.IBLLStrategy
{
    using KuaiCard.Model.Order;
    using System;

    public interface IOrderBankStrategy
    {
        void Complete(OrderBankInfo order);
        long Insert(OrderBankInfo order);
    }
}

