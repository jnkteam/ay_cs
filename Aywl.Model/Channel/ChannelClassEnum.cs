namespace OriginalStudio.Model.Channel
{
    using System;

    /// <summary>
    /// 和数据库表sys_channeltype_class相对应
    /// </summary>
    [Serializable]
    public enum ChannelClassEnum
    {
        网银 = 1,
        支付宝 = 2,
        微信 = 3,
        QQ = 4,
        京东 = 5
    }
}

