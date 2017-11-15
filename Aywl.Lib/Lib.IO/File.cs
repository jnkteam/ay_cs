using OriginalStudio.Lib.ExceptionHandling;
using System;
using System.IO;
using System.Text;

namespace OriginalStudio.Lib.IO
{
	public sealed class File
	{
		public static string ReadContent(Stream stream, Encoding encoding)
		{
			string str = string.Empty;
			if (stream != null)
			{
				using (StreamReader streamReader = new StreamReader(stream, encoding))
				{
					str = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			return str;
		}

		public static bool Exists(string path, bool checkDirectory)
		{
			bool result;
			if (!checkDirectory)
			{
				result = System.IO.File.Exists(path);
			}
			else
			{
				result = (System.IO.File.Exists(path) || Directory.Exists(path));
			}
			return result;
		}

		public static bool Delete(string path)
		{
			bool result;
			try
			{
				if (File.Exists(path, false))
				{
					File.Delete(path);
				}
				result = true;
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex);
				result = false;
			}
			return result;
		}

		public static string ReadFile(string filepath)
		{
			string result;
			if (!File.Exists(filepath, false))
			{
				result = "Error: Not Exists " + filepath;
			}
			else
			{
				using (StreamReader streamReader = new StreamReader(filepath, Encoding.GetEncoding("utf-8")))
				{
					string str = streamReader.ReadToEnd();
					streamReader.Close();
					result = str;
				}
			}
			return result;
		}
	}
}
