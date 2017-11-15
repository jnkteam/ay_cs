﻿namespace KuaiCard.IDAL
{
    using KuaiCard.Model.Order;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using KuaiCardLib.Data;

    public interface IOrderBank
    {
        bool Complete(OrderBankInfo order);
        bool Deduct(string orderid);
        OrderBankInfo GetModel(long orderId);
        OrderBankInfo GetModel(string orderId);
        OrderBankInfo GetModel(long orderId, int userid);
        long Insert(OrderBankInfo order);
        bool Notify(OrderBankInfo order);
        DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby);
        DataSet UserPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby);
        DataSet AdminPageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby);
        bool ReDeduct(string orderid);
    }
}

