using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace KuaiCardLib.Utils
{
	public class FileHelp
	{
		public static bool IsExists(string tempDir)
		{
			return File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + tempDir);
		}

		public static string ReadFile(string tempDir)
		{
			string result;
			if (!File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + tempDir))
			{
				result = "未找到模板文件：" + tempDir;
			}
			else
			{
				StreamReader streamReader = new StreamReader(HttpContext.Current.Request.PhysicalApplicationPath + tempDir, Encoding.Default);
				string str = streamReader.ReadToEnd();
				streamReader.Close();
				result = str;
			}
			return result;
		}

		public static string ReadPhoto(string tempDir)
		{
			string result;
			if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + tempDir))
			{
				result = tempDir;
			}
			else
			{
				result = "Images\\onlinenone.jpg";
			}
			return result;
		}

		public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
		{
			bool result;
			try
			{
				FileStream fileStream = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				try
				{
					_Response.AddHeader("Accept-Ranges", "bytes");
					_Response.Buffer = false;
					long length = fileStream.Length;
					long offset = 0L;
					int count = 10240;
					int millisecondsTimeout = (int)Math.Floor((double)((long)(1000 * count) / _speed)) + 1;
					if (_Request.Headers["Range"] != null)
					{
						_Response.StatusCode = 206;
						offset = Convert.ToInt64(_Request.Headers["Range"].Split(new char[]
						{
							'=',
							'-'
						})[1]);
					}
					_Response.AddHeader("Content-Length", (length - offset).ToString());
					if (offset != 0L)
					{
						_Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", offset, length - 1L, length));
					}
					_Response.AddHeader("Connection", "Keep-Alive");
					_Response.ContentType = "application/octet-stream";
					_Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, Encoding.UTF8));
					binaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
					int num = (int)Math.Floor((double)((length - offset) / (long)count)) + 1;
					for (int index = 0; index < num; index++)
					{
						if (_Response.IsClientConnected)
						{
							_Response.BinaryWrite(binaryReader.ReadBytes(count));
							Thread.Sleep(millisecondsTimeout);
						}
						else
						{
							index = num;
						}
					}
				}
				catch
				{
					result = false;
					return result;
				}
				finally
				{
					binaryReader.Close();
					fileStream.Close();
				}
			}
			catch
			{
				result = false;
				return result;
			}
			result = true;
			return result;
		}
	}
}
