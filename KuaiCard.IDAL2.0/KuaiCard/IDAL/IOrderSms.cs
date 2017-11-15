namespace KuaiCard.IDAL
{
    using KuaiCard.Model.Order;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using KuaiCardLib.Data;

    public interface IOrderSms
    {
        bool Deduct(string orderid);
        OrderSmsInfo GetModel(int id);
        OrderSmsInfo GetModel(string orderId);
        OrderSmsInfo GetModel(int id, int userid);
        OrderSmsInfo GetModel(int suppId, string linkId);
        bool Insert(OrderSmsInfo order);
        bool Notify(OrderSmsInfo order);
        DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby);
        bool ReDeduct(string orderid);
    }
}

