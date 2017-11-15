namespace KuaiCard.IBLLStrategy
{
    using KuaiCard.Model.Order;
    using System;
    using System.Runtime.InteropServices;

    public interface IOrderCardStrategy
    {
        void Complete(OrderCardInfo order);
        void Insert(OrderCardInfo order);
        void InsertItem(CardItemInfo order);
        bool ItemComplete(CardItemInfo order, out bool allCompleted, out string opstate, out string ovalue, out decimal ototalvalue);
    }
}

