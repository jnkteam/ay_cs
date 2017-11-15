namespace OriginalStudio.Model.Settled
{
    using System;

    /// <summary>
    /// 结算状态
    /// </summary>
    public enum SettledStatusEnum
    {
        已取消 = 0,
        审核中 = 1,
        支付中 = 2,
        无效 = 4,
        已支付 = 8,
        付款接口支付中 = 16
    }
}

