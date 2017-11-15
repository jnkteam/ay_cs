namespace KuaiCard.SysConfig
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class WebSetting
    {
        public static List<KuaiCardLib.ScheduledTask.ScheduledTask> ScheduledTaskSettings
        {
            get
            {
                if (ConfigurationManager.GetSection("officeKStar/scheduledTaskSettings") != null)
                {
                    return (ConfigurationManager.GetSection("officeKStar/scheduledTaskSettings") as List<KuaiCardLib.ScheduledTask.ScheduledTask>);
                }
                return new List<KuaiCardLib.ScheduledTask.ScheduledTask>();
            }
        }
    }
}

