using KuaiCardLib.Configuration;
using KuaiCardLib.ExceptionHandling;
using KuaiCardLib.Logging;
using KuaiCardLib.TimeControl;
using System;

namespace KuaiCardLib.ScheduledTask
{
	public sealed class ScheduledTaskLog
	{
		private ScheduledTaskLog()
		{
		}

		public static void WriteExecuteLog(Type type, DateTime startTime, DateTime endTime)
		{
			if (LogSetting.ScheduledTaskLogEnabled)
			{
				try
				{
					LogHelper.Write(LogSetting.ScheduleTaskExecuteLogFilePath(DateTime.Today), string.Format("{0},{1:yyyy-MM-dd HH:mm:ss.fff},{2:yyyy-MM-dd HH:mm:ss.fff}", type.FullName, startTime, endTime), false);
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
		}

		public static void WriteLog(ScheduledTaskConfiguration config)
		{
			if (config != null && LogSetting.ScheduledTaskLogEnabled)
			{
				try
				{
					string str = string.Format("Task\t\t\t\t= {0}\r\nTime              = {1}", config.ScheduledTaskType, FormatConvertor.DateTimeToTimeString(DateTime.Now, true));
					LogHelper.Write(LogSetting.ScheduleTaskLogFilePath(DateTime.Today, config), str);
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
		}
	}
}
