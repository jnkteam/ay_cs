namespace OriginalStudio.IBLLStrategy
{
    using OriginalStudio.Model.Order;
    using System;

    public interface IOrderBankStrategy
    {
        void Complete(OrderBankInfo order);
        long Insert(OrderBankInfo order);
    }
}

