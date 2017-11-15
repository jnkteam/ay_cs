namespace KuaiCard.BLL
{
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IDAL;
    using KuaiCard.Model.Order;
    using System;
    using System.Runtime.InteropServices;
    using System.Transactions;
    using KuaiCard.DALFactory;

    public class OrderCardSynchronous : IOrderCardStrategy
    {
        private static readonly IOrderCard dal = DataAccess.CreateOrderCard();

        public void Complete(OrderCardInfo order)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                dal.Complete(order);
                scope.Complete();
            }
        }

        public void Insert(OrderCardInfo order)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                dal.Insert(order);
                scope.Complete();
            }
        }

        public void InsertItem(CardItemInfo order)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                dal.InsertItem(order);
                scope.Complete();
            }
        }

        public bool ItemComplete(CardItemInfo order, out bool allCompleted, out string opstate, out string ovalue, out decimal ototalvalue)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                allCompleted = false;
                opstate = string.Empty;
                ovalue = string.Empty;
                ototalvalue = 0M;
                dal.ItemComplete(order, out allCompleted, out opstate, out ovalue, out ototalvalue);
                scope.Complete();
                return true;
            }
        }
    }
}

