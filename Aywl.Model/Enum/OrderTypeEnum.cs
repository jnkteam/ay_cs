namespace OriginalStudio.Model.Order
{
    using System;

    /// <summary>
    /// 订单类型枚举类型。
    /// </summary>
    public enum OrderTypeEnum
    {
        API = 1,
        无来路 = 2,
        有来路 = 4,
        批量销卡 = 8
    }
}

