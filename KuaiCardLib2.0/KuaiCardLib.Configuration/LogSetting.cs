using KuaiCardLib.ScheduledTask;
using KuaiCardLib.TimeControl;
using System;

namespace KuaiCardLib.Configuration
{
	public sealed class LogSetting
	{
		internal static readonly string _group = "logSettings";

		public static bool ExceptionLogEnabled
		{
			get
			{
				string config = ConfigHelper.GetConfig(LogSetting.SettingGroup, "ExceptionLogEnabled");
				return config != null && string.Compare(config, bool.TrueString, true) == 0;
			}
		}

		public static bool ScheduledTaskLogEnabled
		{
			get
			{
				string config = ConfigHelper.GetConfig(LogSetting.SettingGroup, "ScheduledTaskLogEnabled");
				return config != null && string.Compare(config, bool.TrueString, true) == 0;
			}
		}

		public static string SettingGroup
		{
			get
			{
				return LogSetting._group;
			}
		}

		public static bool SMSLogEnabled
		{
			get
			{
				return true;
			}
		}

		private LogSetting()
		{
		}

		public static string ExceptionLogFilePath(DateTime date)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "LogFiles/Exceptions/" + string.Format("{0:yyyy-MM-dd}", date) + ".log";
		}

		public static string ScheduleTaskExecuteLogFilePath(DateTime date)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "LogFiles/ScheduleTask/" + FormatConvertor.DateTimeToDateString(date, true) + "_execute.log";
		}

		public static string ScheduleTaskLogFilePath(DateTime date, ScheduledTaskConfiguration config)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "LogFiles/ScheduleTask/" + FormatConvertor.DateTimeToDateString(date, true) + ".log";
		}
        
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
