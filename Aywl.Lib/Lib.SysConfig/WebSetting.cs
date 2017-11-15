namespace OriginalStudio.Lib.SysConfig
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class WebSetting
    {
        public static List<ScheduledTask.ScheduledTask> ScheduledTaskSettings
        {
            get
            {
                if (ConfigurationManager.GetSection("officeKStar/scheduledTaskSettings") != null)
                {
                    return (ConfigurationManager.GetSection("officeKStar/scheduledTaskSettings") as List<ScheduledTask.ScheduledTask>);
                }
                return new List<ScheduledTask.ScheduledTask>();
            }
        }
    }
}

