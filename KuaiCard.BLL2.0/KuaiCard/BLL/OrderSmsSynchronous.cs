namespace KuaiCard.BLL
{
    using KuaiCard.IBLLStrategy;
    using KuaiCard.IDAL;
    using KuaiCard.Model.Order;
    using System;
    using System.Transactions;
    using KuaiCard.DALFactory;


    public class OrderSmsSynchronous : IOrderSmsStrategy
    {
        private static readonly IOrderSms dal = DataAccess.CreateOrderSms();

        public void Insert(OrderSmsInfo order)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                dal.Insert(order);
                scope.Complete();
            }
        }
    }
}

