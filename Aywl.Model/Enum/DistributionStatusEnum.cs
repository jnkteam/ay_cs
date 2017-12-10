namespace OriginalStudio.Model.Distribution
{
    using System;

    /// <summary>
    /// 代付状态。
    /// </summary>
    public enum DistributionStatusEnum
    {
        已受理 = 0,
        未受理 = 1,
        审核拒绝 = 2,
        代发成功 = 3,
        代发失败 = 4,
        初始状态 = 255
    }
}

