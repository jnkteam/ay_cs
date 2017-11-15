using KuaiCardLib.Configuration;
using KuaiCardLib.Logging;
using KuaiCardLib.Web;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace KuaiCardLib.ExceptionHandling
{
	public sealed class ExceptionHandler
	{
		private static readonly string[] IgnoredProperties = new string[]
		{
			"Source",
			"Message",
			"HelpLink",
			"InnerException",
			"StackTrace"
		};

		private ExceptionHandler()
		{
		}

		private static string GetFieldInfo(FieldInfo field, object fieldValue)
		{
			return string.Format("{0} : {1}", field.Name, fieldValue);
		}

		private static string GetPropertyInfo(PropertyInfo propertyInfo, object propertyValue)
		{
			return string.Format("{0} : {1}", propertyInfo.Name, propertyValue);
		}

		private static string GetReflectionInfo(Exception e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Type type = e.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				if (propertyInfo.CanRead && Array.IndexOf<string>(ExceptionHandler.IgnoredProperties, propertyInfo.Name) == -1)
				{
					object propertyValue = propertyInfo.GetValue(e, null);
					stringBuilder.Append(ExceptionHandler.GetPropertyInfo(propertyInfo, propertyValue));
					stringBuilder.Append("\r\n");
				}
			}
			FieldInfo[] array2 = fields;
			for (int i = 0; i < array2.Length; i++)
			{
				FieldInfo field = array2[i];
				object fieldValue = field.GetValue(e);
				stringBuilder.Append(ExceptionHandler.GetFieldInfo(field, fieldValue));
				stringBuilder.Append("\r\n");
			}
			return stringBuilder.ToString();
		}

		public static void HandleException(Exception ex)
		{
			if (ex != null && LogSetting.ExceptionLogEnabled)
			{
				try
				{
					if (WebBase.Context == null || File.Exists(WebBase.Server.MapPath(WebBase.Request.Path)) 
                        || !(ex is HttpException) || ex.InnerException == null || !(ex.InnerException is FileNotFoundException))
					{
						LogHelper.Write(LogSetting.ExceptionLogFilePath(DateTime.Today), 
                                    string.Format("Path              = {0}\r\nTime              = {1}\r\nClientIP          = {2}\r\nType              = {3}\r\nMessage           = {4}\r\nSource            = {5}\r\nHelpLink          = {6}\r\nReflectionInfo    = {7}\r\nStackTrace        = {8}", new object[]
						{
							(WebBase.Context != null) ? WebBase.Request.RawUrl : string.Empty,
							string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now),
							(WebBase.Context != null) ? ServerVariables.TrueIP : string.Empty,
							ex.GetType().AssemblyQualifiedName,
							ex.Message,
							ex.Source,
							ex.HelpLink,
							ExceptionHandler.GetReflectionInfo(ex).Replace("\n", "\n" + new string(' ', 24)),
							ex.StackTrace.Replace("\n", "\n" + new string(' ', 24))
						}));
						if (ex.InnerException != null)
						{
							ExceptionHandler.HandleException(ex.InnerException);
						}
					}
				}
				catch
				{
				}
			}
		}
	}
}
