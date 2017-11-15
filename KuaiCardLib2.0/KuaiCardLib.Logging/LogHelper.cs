using System;
using System.IO;
using System.Text;

namespace KuaiCardLib.Logging
{
	public sealed class LogHelper
	{
		public static object obj = new object();

		private LogHelper()
		{
		}

		public static string GetTenPayLogPath()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\payLog\\tenpay.log");
		}

        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="str"></param>
		public static void Write(string str)
		{
            LogHelper.Write(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\日志_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"), str);
		}

        /// <summary>
        /// 记录错误日志。
        /// </summary>
        /// <param name="str"></param>
        public static void WriteErrMsg(string str)
        {
            LogHelper.Write(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\错误日志_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"), str);
        }

        /// <summary>
        /// 记录调试日志。
        /// </summary>
        /// <param name="str"></param>
        public static void WriteWxDebug(string str)
        {
            LogHelper.Write(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\微信调试日志_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"), str);
        }

        //public static void WriteS(string str)
        //{
        //    LogHelper.Write(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\yikaduixianlog.log"), str);
        //}

		public static void Write(string path, string str)
		{
			LogHelper.Write(path, str, true);
		}

        public static void Write(string path, string str, bool withSeparator)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try
            {
                lock (LogHelper.obj)
                {
                    using (StreamWriter resource_0 = new StreamWriter(path, true, Encoding.UTF8))
                    {
                        if (withSeparator)
                        {
                            resource_0.WriteLine(new string('-', 50) + Environment.NewLine);
                        }
                        resource_0.WriteLine(DateTime.Now.ToString() + "：" + str);
                        resource_0.Close();
                    }
                }
            }
            catch
            {
                ;
            }
        }
	}
}
