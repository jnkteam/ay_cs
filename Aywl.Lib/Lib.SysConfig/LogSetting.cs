using OriginalStudio.Lib.ScheduledTask;
using OriginalStudio.Lib.TimeControl;
using System;
using OriginalStudio.Lib.Configuration;

namespace OriginalStudio.Lib.SysConfig
{
	public sealed class LogSetting
	{
		private LogSetting()
		{
		}

		internal static readonly string _group = "logSettings";

		public static string SettingGroup
		{
			get
			{
				return LogSetting._group;
			}
		}

        /// <summary>
        /// 全站日志开启标记
        /// </summary>
		public static bool ExceptionLogEnabled
		{
			get
			{
				string config = ConfigHelper.GetConfig(LogSetting.SettingGroup, "ExceptionLogEnabled");
				return config != null && string.Compare(config, bool.TrueString, true) == 0;
			}
		}
        
        /// <summary>
        /// 日志文件命名方式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
		public static string ExceptionLogFilePath(DateTime date)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "LogFiles/Exceptions/" + string.Format("{0:yyyy-MM-dd}", date) + ".log";
		}

        #region 计划任务之类日志

		public static bool ScheduledTaskLogEnabled
		{
			get
			{
				string config = ConfigHelper.GetConfig(LogSetting.SettingGroup, "ScheduledTaskLogEnabled");
				return config != null && string.Compare(config, bool.TrueString, true) == 0;
			}
		}

        public static string ScheduleTaskExecuteLogFilePath(DateTime date)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "LogFiles/ScheduleTask/" + FormatConvertor.DateTimeToDateString(date, true) + "_execute.log";
		}

		public static string ScheduleTaskLogFilePath(DateTime date, ScheduledTaskConfiguration config)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "LogFiles/ScheduleTask/" + FormatConvertor.DateTimeToDateString(date, true) + ".log";
        }

        #endregion

        /// <summary>
        /// 接口调试开关
        /// </summary>
        /// <param name="p_supplier_code">接口代码</param>
        /// <returns></returns>
        public static bool SupplierIsLog(string p_supplier_code)
        {
            return ConfigHelper.GetConfig(SettingGroup, p_supplier_code + "_IsLog") == "1";
        }
    }
}
