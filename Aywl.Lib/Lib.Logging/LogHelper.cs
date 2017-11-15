using System;
using System.IO;
using System.Text;

namespace OriginalStudio.Lib.Logging
{
	public sealed class LogHelper
	{
		public static object obj = new object();

		private LogHelper()
		{
		}

        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="str"></param>
        public static void Write(string str)
		{
            LogHelper.Write(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_运行日志.log"), str);
		}

        /// <summary>
        /// 记录错误日志。
        /// </summary>
        /// <param name="str"></param>
        public static void WriteErrorMsg(string str)
        {
            LogHelper.Write(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_错误日志.log"), str);
        }

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
