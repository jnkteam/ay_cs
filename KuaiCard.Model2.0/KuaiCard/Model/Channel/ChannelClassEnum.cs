﻿namespace KuaiCard.Model.Channel
{
    using System;

    [Serializable]
    public enum ChannelClassEnum
    {
        在线支付 = 1,
        充值卡 = 2,
        声讯 = 4,
        手机网银 = 6,
        短信 = 8,
        支付宝 = 9,
        微信 = 10,
        京东 = 11,
        代付款 = 16
    }
}

