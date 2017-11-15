using KuaiCardLib.ExceptionHandling;
using System;

namespace KuaiCardLib.TimeControl
{
	public sealed class FormatConvertor
	{
		public static readonly string DATE_FORMAT = "yyyy-MM-dd";

		public static readonly string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

		public static readonly string DATETIME_FORMAT_WITHOUT_SECOND = "yyyy-MM-dd HH:mm";

		public static readonly DateTime SqlDateTimeMinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);

		public static readonly string TIME_HOUR_MINUTE_FORMAT = "hh:mm";

		public static readonly string YEARMONTH_FORMAT = "yyyy-MM";

		private FormatConvertor()
		{
		}

		public static string DateTimeToDateString(DateTime d)
		{
			return FormatConvertor.DateTimeToDateString(d, true);
		}

		public static string DateTimeToDateString(DateTime d, bool viewDay)
		{
			string result;
			if (d == DateTime.MinValue)
			{
				result = string.Empty;
			}
			else if (viewDay)
			{
				result = string.Format("{0:" + FormatConvertor.DATE_FORMAT + "}", d);
			}
			else
			{
				result = string.Format("{0:" + FormatConvertor.YEARMONTH_FORMAT + "}", d);
			}
			return result;
		}

		public static string DateTimeToTimeString(DateTime d)
		{
			return FormatConvertor.DateTimeToTimeString(d, false);
		}

		public static string DateTimeToTimeString(DateTime d, bool viewSecond)
		{
			string result;
			if (d == DateTime.MinValue)
			{
				result = string.Empty;
			}
			else if (!viewSecond)
			{
				result = string.Format("{0:" + FormatConvertor.DATETIME_FORMAT_WITHOUT_SECOND + "}", d);
			}
			else
			{
				result = string.Format("{0:" + FormatConvertor.DATETIME_FORMAT + "}", d);
			}
			return result;
		}

		public static string GetFormatedTime(string s)
		{
			string result;
			if (s == null || s.Length == 0)
			{
				result = "00:00:00";
			}
			else
			{
				string[] strArray = s.Trim().Split(new char[]
				{
					':'
				});
				try
				{
					switch (strArray.Length)
					{
					case 1:
						result = string.Format("{0:00}:00:00", Convert.ToInt32(strArray[0], 10));
						break;
					case 2:
						result = string.Format("{0:00}:{1:00}:00", Convert.ToInt32(strArray[0], 10), Convert.ToInt32(strArray[1], 10));
						break;
					case 3:
						result = string.Format("{0:00}:{1:00}:{2:00}", Convert.ToInt32(strArray[0], 10), Convert.ToInt32(strArray[1], 10), Convert.ToInt32(strArray[2], 10));
						break;
					default:
						result = "00:00:00";
						break;
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
					result = "00:00:00";
				}
			}
			return result;
		}

		public static DateTime StringToDateTime(string s)
		{
			DateTime result;
			if (s == null || s.Length == 0)
			{
				result = DateTime.MinValue;
			}
			else
			{
				string[] strArray = s.Trim().Split(new char[]
				{
					'-',
					' ',
					':'
				});
				try
				{
					switch (strArray.Length)
					{
					case 2:
						result = new DateTime(Convert.ToInt32(strArray[0], 10), Convert.ToInt32(strArray[1], 10), 1);
						break;
					case 3:
						result = new DateTime(Convert.ToInt32(strArray[0], 10), Convert.ToInt32(strArray[1], 10), Convert.ToInt32(strArray[2], 10));
						break;
					case 4:
					case 5:
					case 6:
						result = new DateTime(Convert.ToInt32(strArray[0], 10), Convert.ToInt32(strArray[1], 10), Convert.ToInt32(strArray[2], 10), Convert.ToInt32(strArray[3], 10), (strArray.Length > 4) ? Convert.ToInt32(strArray[4], 10) : 0, (strArray.Length > 5) ? Convert.ToInt32(strArray[5], 10) : 0);
						break;
					default:
						result = DateTime.MinValue;
						break;
					}
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
					result = DateTime.MinValue;
				}
			}
			return result;
		}

		public static TimeSpan StringToTimeSpan(string s)
		{
			TimeSpan result;
			if (s == null || s.Length == 0)
			{
				result = new TimeSpan(0L);
			}
			else
			{
				try
				{
					result = TimeSpan.Parse(s);
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
					result = new TimeSpan(0L);
				}
			}
			return result;
		}

		public static string TimsSpanToString(TimeSpan timeSpan)
		{
			return timeSpan.ToString();
		}
	}
}
