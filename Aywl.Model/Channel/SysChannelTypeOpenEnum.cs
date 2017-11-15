namespace OriginalStudio.Model.Channel
{
    using System;

    [Serializable]
    public enum SysChannelTypeOpenEnum
    {
        None = 0,
        /// <summary>
        /// 全部关闭
        /// </summary>
        AllClose = 1,
        /// <summary>
        /// 全部开启
        /// </summary>
        AllOpen = 2,
        /// <summary>
        /// 按配置(默认关闭)
        /// </summary>
        Close = 4,
        /// <summary>
        /// 按配置(默认开启)
        /// </summary>
        Open = 8
    }
}

