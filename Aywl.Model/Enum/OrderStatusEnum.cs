namespace OriginalStudio.Model.Order
{
    using System;

    /// <summary>
    /// 订单状态枚举类型。
    /// </summary>
    public enum OrderStatusEnum
    {   
        处理中 = 1,
        已完成 = 2,
        失败 = 4,
        扣量 = 8,
        平台扣量 = 16,
        部份成功 = 32,
        转单 = 64
    }
}

