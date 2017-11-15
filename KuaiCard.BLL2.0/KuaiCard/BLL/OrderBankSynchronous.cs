namespace KuaiCard.BLL
{
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IDAL;
    using KuaiCard.Model.Order;
    using System;
    using System.Transactions;
    using KuaiCard.DALFactory;

    public class OrderBankSynchronous : IOrderBankStrategy
    {
        private static readonly IOrderBank dal = DataAccess.CreateOrderBank();

        public void Complete(OrderBankInfo order)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                dal.Complete(order);
                scope.Complete();
            }
        }

        public long Insert(OrderBankInfo order)
        {
            Int64 id = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                id = dal.Insert(order);
                if (id <= 0L)
                {
                    new ApplicationException("Add orders fails");
                }
                scope.Complete();
            }
            return id;
        }
    }
}

