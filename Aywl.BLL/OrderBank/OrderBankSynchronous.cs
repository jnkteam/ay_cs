namespace OriginalStudio.BLL
{
    using OriginalStudio.IBLLStrategy;
    using OriginalStudio.IDAL;
    using OriginalStudio.Model.Order;
    using System;
    using System.Transactions;

    /// <summary>
    /// 订单事务同步操作操作。
    /// </summary>
    public class OrderBankSynchronous : IOrderBankStrategy
    {
        private static readonly IOrderBank dal = OriginalStudio.BLL.OrderBankAccess.CreateOrderBank();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
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

