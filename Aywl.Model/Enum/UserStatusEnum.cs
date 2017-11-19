namespace OriginalStudio.Model.User
{
    using System;

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatusEnum
    {
        待审核 = 1,
        正常 = 2,
        锁定 = 4,
        审核失败 = 8,
        IP非法 = 16        
    }
}

