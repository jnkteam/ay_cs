namespace OriginalStudio.Model.Settled
{
    using System;

    /// <summary>
    /// 账户操作方式。加款或扣款
    /// </summary>
    public enum SettlePayTypeEnum
    {
        空 = 0,
        商户前台 = 1,
        商户API = 2,
        管理后台 = 3
    }
}

