using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace OriginalStudio.Lib
{
	public class Utility
	{
		public static string[] Monthes
		{
			get
			{
				return new string[]
				{
					"January",
					"February",
					"March",
					"April",
					"May",
					"June",
					"July",
					"August",
					"September",
					"October",
					"November",
					"December"
				};
			}
		}

		public static string ChkSQL(string str)
		{
			string result;
			if (str == null)
			{
				result = "";
			}
			else
			{
				str = str.Replace("'", "''");
				result = str;
			}
			return result;
		}

		public static string CleanInput(string strIn)
		{
			return Regex.Replace(strIn.Trim(), "[^\\w\\.@-]", "");
		}

		public static string ClearBR(string str)
		{
			Match match = new Regex("(\\r\\n)", RegexOptions.IgnoreCase).Match(str);
			while (match.Success)
			{
				str = str.Replace(match.Groups[0].ToString(), "");
				match = match.NextMatch();
			}
			return str;
		}

		public static string ClearHtml(string strHtml)
		{
			if (strHtml != "")
			{
				Match match = new Regex("<\\/?[^>]*>", RegexOptions.IgnoreCase).Match(strHtml);
				while (match.Success)
				{
					strHtml = strHtml.Replace(match.Groups[0].ToString(), "");
					match = match.NextMatch();
				}
			}
			return strHtml;
		}

		public static bool CreateDir(string name)
		{
			return Utility.MakeSureDirectoryPathExists(name);
		}

		public static string CutString(string str, int startIndex)
		{
			return Utility.CutString(str, startIndex, str.Length);
		}

		public static string CutString(string str, int startIndex, int length)
		{
			string result;
			if (startIndex >= 0)
			{
				if (length < 0)
				{
					length *= -1;
					if (startIndex - length < 0)
					{
						length = startIndex;
						startIndex = 0;
					}
					else
					{
						startIndex -= length;
					}
				}
				if (startIndex > str.Length)
				{
					result = "";
					return result;
				}
			}
			else
			{
				if (length < 0 || length + startIndex <= 0)
				{
					result = "";
					return result;
				}
				length += startIndex;
				startIndex = 0;
			}
			if (str.Length - startIndex < length)
			{
				length = str.Length - startIndex;
			}
			try
			{
				result = str.Substring(startIndex, length);
			}
			catch
			{
				result = str;
			}
			return result;
		}

		public static string EncodeHtml(string strHtml)
		{
			string result;
			if (!(strHtml != ""))
			{
				result = "";
			}
			else
			{
				strHtml = strHtml.Replace(",", "&def");
				strHtml = strHtml.Replace("'", "&dot");
				strHtml = strHtml.Replace(";", "&dec");
				result = strHtml;
			}
			return result;
		}

		public static bool FileExists(string filename)
		{
			return File.Exists(filename);
		}

		public static string[] FindNoUTF8File(string Path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			FileInfo[] files = new DirectoryInfo(Path).GetFiles();
			for (int index = 0; index < files.Length; index++)
			{
				if (files[index].Extension.ToLower().Equals(".htm"))
				{
					FileStream sbInputStream = new FileStream(files[index].FullName, FileMode.Open, FileAccess.Read);
					bool flag = Utility.IsUTF8(sbInputStream);
					sbInputStream.Close();
					if (!flag)
					{
						stringBuilder.Append(files[index].FullName);
						stringBuilder.Append("\r\n");
					}
				}
			}
			return Utility.SplitString(stringBuilder.ToString(), "\r\n");
		}

		public static string FormatBytesStr(int bytes)
		{
			string result;
			if (bytes > 1073741824)
			{
				result = ((double)(bytes / 1073741824)).ToString("0") + "G";
			}
			else if (bytes > 1048576)
			{
				result = ((double)(bytes / 1048576)).ToString("0") + "M";
			}
			else if (bytes > 1024)
			{
				result = ((double)(bytes / 1024)).ToString("0") + "K";
			}
			else
			{
				result = bytes.ToString() + "Bytes";
			}
			return result;
		}

		public static string GetAssemblyCopyright()
		{
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright;
		}

		public static string GetAssemblyProductName()
		{
			return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;
		}

		public static string GetAssemblyVersion()
		{
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			return string.Format("{0}.{1}.{2}", versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart);
		}

		public static string GetCookie(string strName)
		{
			string result;
			if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
			{
				result = HttpContext.Current.Request.Cookies[strName].Value.ToString();
			}
			else
			{
				result = "";
			}
			return result;
		}

		public static string GetDate()
		{
			return DateTime.Now.ToString("yyyy-MM-dd");
		}

		public static string GetDate(string datetimestr, string replacestr)
		{
			string result;
			if (datetimestr == null)
			{
				result = replacestr;
			}
			else if (datetimestr.Equals(""))
			{
				result = replacestr;
			}
			else
			{
				try
				{
					datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
				}
				catch
				{
					result = replacestr;
					return result;
				}
				result = datetimestr;
			}
			return result;
		}

		public static string GetDateTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public static string GetDateTime(int relativeday)
		{
			return DateTime.Now.AddDays((double)relativeday).ToString("yyyy-MM-dd HH:mm:ss");
		}

		public static string GetDateTimeF()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
		}

		public static string GetEmailHostName(string strEmail)
		{
			string result;
			if (strEmail.IndexOf("@") < 0)
			{
				result = "";
			}
			else
			{
				result = strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
			}
			return result;
		}

		public static string GetFilename(string url)
		{
			string result;
			if (url == null)
			{
				result = "";
			}
			else
			{
				string[] strArray = url.Split(new char[]
				{
					'/'
				});
				result = strArray[strArray.Length - 1].Split(new char[]
				{
					'?'
				})[0];
			}
			return result;
		}

		public static int GetInArrayID(string strSearch, string[] stringArray)
		{
			return Utility.GetInArrayID(strSearch, stringArray, true);
		}

		public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
		{
			int result;
			for (int index = 0; index < stringArray.Length; index++)
			{
				if (caseInsensetive)
				{
					if (strSearch.ToLower() == stringArray[index].ToLower())
					{
						result = index;
						return result;
					}
				}
				else if (strSearch == stringArray[index])
				{
					result = index;
					return result;
				}
			}
			result = -1;
			return result;
		}

		public static string GetMapPath(string strPath)
		{
			string result;
			if (HttpContext.Current != null)
			{
				result = HttpContext.Current.Server.MapPath(strPath);
			}
			else
			{
				result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
			}
			return result;
		}

		public static string GetPageNumbers(int page, int pageSize, int Count, string Url)
		{
			string str = "";
			int num = page - 1;
			int num2 = page + 1;
			int num3 = (int)Math.Ceiling((double)Count / (double)pageSize);
			string str2 = string.Concat(new object[]
			{
				str,
				"<span>页码：",
				(string)page.ToString(),
				"/",
				(string)num3.ToString(),
				"</span>"
			});
			string str3;
			if (num < 1)
			{
				str3 = str2 + "<span title='首页'>首页</span><span title='上一页'>上一页</span>";
			}
			else
			{
				str3 = string.Concat(new object[]
				{
					str2,
					"<span title='首页'><a href='",
					Url,
					"=1'>首页</a></span>",
					"<span title='上一页'><a href='",
					Url,
					"=",
					(string)num.ToString(),
					"'>上一页</a></span>"
				});
			}
			int num4 = (page % pageSize != 0) ? (page - page % pageSize + 1) : (page - pageSize + 1);
			if (num4 > pageSize)
			{
				str3 = string.Concat(new object[]
				{
					str3,
					"<span title='前",
					(string)pageSize.ToString(),
					"页'><a href='",
					Url,
					"=",
					(string)(num4 - 1).ToString(),
					"'>...</a></span>"
				});
			}
			int index = num4;
			while (index < num4 + pageSize && index <= num3)
			{
				if (index == page)
				{
					str3 = string.Concat(new object[]
					{
						str3,
						"<span title='页 ",
						(string)index.ToString(),
						"'> <font color='#ff0000'>[",
						(string)index.ToString(),
						"]</font> </span>"
					});
				}
				else
				{
					str3 = string.Concat(new object[]
					{
						str3,
						"<span title='页 ",
						(string)index.ToString(),
						"'> <a href='",
						Url,
						"=",
						(string)index.ToString(),
						"'>[",
						(string)index.ToString(),
						"]</a> </span>"
					});
				}
				index++;
			}
			if (num3 >= num4 + pageSize)
			{
				str3 = string.Concat(new object[]
				{
					str3,
					"<span title='后",
					(string)pageSize.ToString(),
					"页'><a href='",
					Url,
					"=",
					(string)(num4 + pageSize).ToString(),
					"'>...</a></span>"
				});
			}
			string result;
			if (num2 > num3)
			{
				result = str3 + "<span title='下一页'>下一页</span><span title='末页'>末页</span>";
			}
			else
			{
				result = string.Concat(new object[]
				{
					str3,
					"<span title='下一页'><a href='",
					Url,
					"=",
					(string)num2.ToString(),
					"'>下一页</a></span>",
					"<span title='末页'><a href='",
					Url,
					"=",
					(string)num3.ToString(),
					"'>末页</a></span>"
				});
			}
			return result;
		}

		public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag, string anchor)
		{
			if (pagetag == "")
			{
				pagetag = "page";
			}
			int num = 1;
			url = ((url.IndexOf("?") <= 0) ? (url + "?") : (url + "&"));
			string str = string.Concat(new string[]
			{
				"<a href=\"",
				url,
				"&",
				pagetag,
				"=1"
			});
			string str2 = string.Concat(new object[]
			{
				"<a href=\"",
				url,
				"&",
				pagetag,
				"=",
				(string)countPage.ToString()
			});
			if (anchor != null)
			{
				str += anchor;
				str2 += anchor;
			}
			string str3 = str + "\">第一页</a>";
			string str4 = str2 + "\">最后一页</a>";
			if (countPage < 1)
			{
				countPage = 1;
			}
			if (extendPage < 3)
			{
				extendPage = 2;
			}
			int num2;
			if (countPage > extendPage)
			{
				if (curPage - extendPage / 2 > 0)
				{
					if (curPage + extendPage / 2 < countPage)
					{
						num = curPage - extendPage / 2;
						num2 = num + extendPage - 1;
					}
					else
					{
						num2 = countPage;
						num = num2 - extendPage + 1;
						str4 = "";
					}
				}
				else
				{
					num2 = extendPage;
					str3 = "";
				}
			}
			else
			{
				num = 1;
				num2 = countPage;
				str3 = "";
				str4 = "";
			}
			StringBuilder stringBuilder = new StringBuilder("");
			stringBuilder.Append(str3);
			for (int index = num; index <= num2; index++)
			{
				if (index == curPage)
				{
					stringBuilder.Append("<span>");
					stringBuilder.Append(index);
					stringBuilder.Append("</span>");
				}
				else
				{
					stringBuilder.Append("<a href=\"");
					stringBuilder.Append(url);
					stringBuilder.Append(pagetag);
					stringBuilder.Append("=");
					stringBuilder.Append(index);
					if (anchor != null)
					{
						stringBuilder.Append(anchor);
					}
					stringBuilder.Append("\">");
					if (num > 1 && index == num)
					{
						stringBuilder.Append("...");
					}
					stringBuilder.Append(index);
					if (num2 < countPage && index == num2)
					{
						stringBuilder.Append("...");
					}
					stringBuilder.Append("</a>");
				}
			}
			stringBuilder.Append(str4);
			return stringBuilder.ToString();
		}

		public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
		{
			int num = 1;
			int num2 = 1;
			string str = string.Concat(new string[]
			{
				"<a href=\"",
				url,
				"-1",
				expname,
				"\">&laquo;</a>&nbsp;"
			});
			string str2 = string.Concat(new object[]
			{
				"<a href=\"",
				url,
				"-",
				(string)countPage.ToString(),
				expname,
				"\">&raquo;</a>&nbsp;"
			});
			if (countPage < 1)
			{
				countPage = 1;
			}
			if (extendPage < 3)
			{
				extendPage = 2;
			}
			int num3;
			if (countPage > extendPage)
			{
				if (num2 - extendPage / 2 > 0)
				{
					if (num2 + extendPage / 2 < countPage)
					{
						num = num2 - extendPage / 2;
						num3 = num + extendPage - 1;
					}
					else
					{
						num3 = countPage;
						num = num3 - extendPage + 1;
						str2 = "";
					}
				}
				else
				{
					num3 = extendPage;
					str = "";
				}
			}
			else
			{
				num = 1;
				num3 = countPage;
				str = "";
				str2 = "";
			}
			StringBuilder stringBuilder = new StringBuilder("");
			stringBuilder.Append(str);
			for (int index = num; index <= num3; index++)
			{
				stringBuilder.Append("&nbsp;<a href=\"");
				stringBuilder.Append(url);
				stringBuilder.Append("-");
				stringBuilder.Append(index);
				stringBuilder.Append(expname);
				stringBuilder.Append("\">");
				stringBuilder.Append(index);
				stringBuilder.Append("</a>&nbsp;");
			}
			stringBuilder.Append(str2);
			return stringBuilder.ToString();
		}

		public static string GetStandardDateTime(string fDateTime)
		{
			return Utility.GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
		}

		public static string GetStandardDateTime(string fDateTime, string formatStr)
		{
			return Convert.ToDateTime(fDateTime).ToString(formatStr);
		}

		public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
		{
			int num = 1;
			string str = string.Concat(new string[]
			{
				"<a href=\"",
				url,
				"-1",
				expname,
				"\">&laquo;</a>&nbsp;"
			});
			string str2 = string.Concat(new object[]
			{
				"<a href=\"",
				url,
				"-",
				(string)countPage.ToString(),
				expname,
				"\">&raquo;</a>&nbsp;"
			});
			if (countPage < 1)
			{
				countPage = 1;
			}
			if (extendPage < 3)
			{
				extendPage = 2;
			}
			int num2;
			if (countPage > extendPage)
			{
				if (curPage - extendPage / 2 > 0)
				{
					if (curPage + extendPage / 2 < countPage)
					{
						num = curPage - extendPage / 2;
						num2 = num + extendPage - 1;
					}
					else
					{
						num2 = countPage;
						num = num2 - extendPage + 1;
						str2 = "";
					}
				}
				else
				{
					num2 = extendPage;
					str = "";
				}
			}
			else
			{
				num = 1;
				num2 = countPage;
				str = "";
				str2 = "";
			}
			StringBuilder stringBuilder = new StringBuilder("");
			stringBuilder.Append(str);
			for (int index = num; index <= num2; index++)
			{
				if (index == curPage)
				{
					stringBuilder.Append("&nbsp;");
					stringBuilder.Append(index);
					stringBuilder.Append("&nbsp;");
				}
				else
				{
					stringBuilder.Append("&nbsp;<a href=\"");
					stringBuilder.Append(url);
					stringBuilder.Append("-");
					stringBuilder.Append(index);
					stringBuilder.Append(expname);
					stringBuilder.Append("\">");
					stringBuilder.Append(index);
					stringBuilder.Append("</a>&nbsp;");
				}
			}
			stringBuilder.Append(str2);
			return stringBuilder.ToString();
		}

		public static int GetStringLength(string str)
		{
			return Encoding.Default.GetBytes(str).Length;
		}

		public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
		{
			string result;
			if (p_Length < 0)
			{
				result = p_SrcString;
			}
			else
			{
				byte[] bytes = Encoding.Default.GetBytes(p_SrcString);
				if (bytes.Length <= p_Length)
				{
					result = p_SrcString;
				}
				else
				{
					int length = p_Length;
					int[] numArray = new int[p_Length];
					int num = 0;
					for (int index = 0; index < p_Length; index++)
					{
						if (bytes[index] > 127)
						{
							num++;
							if (num == 3)
							{
								num = 1;
							}
						}
						else
						{
							num = 0;
						}
						numArray[index] = num;
					}
					if (bytes[p_Length - 1] > 127 && numArray[p_Length - 1] == 1)
					{
						length = p_Length + 1;
					}
					byte[] bytes2 = new byte[length];
					Array.Copy(bytes, bytes2, length);
					result = Encoding.Default.GetString(bytes2) + p_TailString;
				}
			}
			return result;
		}

		public static string GetTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		public static string GetTrueForumPath()
		{
			string path = HttpContext.Current.Request.Path;
			string result;
			if (path.LastIndexOf("/") != path.IndexOf("/"))
			{
				result = path.Substring(path.IndexOf("/"), path.LastIndexOf("/") + 1);
			}
			else
			{
				result = "/";
			}
			return result;
		}

		public static string HtmlDecode(string str)
		{
			return HttpUtility.HtmlDecode(str);
		}

		public static string HtmlEncode(string str)
		{
			return HttpUtility.HtmlEncode(str);
		}

		public static bool InArray(string str, string stringarray)
		{
			return Utility.InArray(str, Utility.SplitString(stringarray, ","), false);
		}

		public static bool InArray(string str, string[] stringarray)
		{
			return Utility.InArray(str, stringarray, false);
		}

		public static bool InArray(string str, string stringarray, string strsplit)
		{
			return Utility.InArray(str, Utility.SplitString(stringarray, strsplit), false);
		}

		public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
		{
			return Utility.GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
		}

		public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
		{
			return Utility.InArray(str, Utility.SplitString(stringarray, strsplit), caseInsensetive);
		}

		public static bool InIPArray(string ip, string[] iparray)
		{
			string[] strArray = Utility.SplitString(ip, ".");
			int index = 0;
			bool result;
			while (index < iparray.Length)
			{
				string[] strArray2 = Utility.SplitString(iparray[index], ".");
				int num = 0;
				for (int index2 = 0; index2 < strArray2.Length; index2++)
				{
					if (strArray2[index2] == "*")
					{
						result = true;
						return result;
					}
					if (strArray.Length <= index2 || !(strArray2[index2] == strArray[index2]))
					{
						break;
					}
					num++;
				}
				if (num != 4)
				{
					index++;
					continue;
				}
				result = true;
				return result;
			}
			result = false;
			return result;
		}

		public static string IntToStr(int intValue)
		{
			return Convert.ToString(intValue);
		}

		public static bool IsBase64String(string str)
		{
			return Regex.IsMatch(str, "[A-Za-z0-9\\+\\/\\=]");
		}

		public static bool IsCompriseStr(string str, string stringarray, string strsplit)
		{
			bool result;
			if (stringarray != "" && stringarray != null)
			{
				str = str.ToLower();
				string[] array = Utility.SplitString(stringarray.ToLower(), strsplit);
				for (int i = 0; i < array.Length; i++)
				{
					string str2 = array[i];
					if (str.IndexOf(str2) > -1)
					{
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}

		public static bool IsDateString(string str)
		{
			return Regex.IsMatch(str, "(\\d{4})-(\\d{1,2})-(\\d{1,2})");
		}

		public static bool IsImgFilename(string filename)
		{
			filename = filename.Trim();
			bool result;
			if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
			{
				result = false;
			}
			else
			{
				string str = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
				result = (str == "jpg" || str == "jpeg" || str == "png" || str == "bmp" || str == "gif");
			}
			return result;
		}

		public static bool IsIP(string ip)
		{
			return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
		}

		public static bool IsNumber(string strNumber)
		{
			return new Regex("^([0-9])[0-9]*(\\.\\w*)?$").IsMatch(strNumber);
		}

		public static bool IsNumberArray(string[] strNumber)
		{
			bool result;
			if (strNumber == null || strNumber.Length < 1)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < strNumber.Length; i++)
				{
					string strNumber2 = strNumber[i];
					if (!Utility.IsNumber(strNumber2))
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
			return result;
		}

		public static bool IsSafeSqlString(string str)
		{
			return !Regex.IsMatch(str, "[-|;|,|\\/|\\(|\\)|\\[|\\]|\\}|\\{|%|@|\\*|!|\\']");
		}

		public static bool IsSafeUserInfoString(string str)
		{
			return !Regex.IsMatch(str, "/^\\s*$|^c:\\\\con\\\\con$|[%,\\*\"\\s\\t\\<\\>\\&]|$guestexp/is");
		}

		public static bool IsTime(string timeval)
		{
			return Regex.IsMatch(timeval, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
		}

		private static bool IsUTF8(FileStream sbInputStream)
		{
			bool flag = true;
			long length = sbInputStream.Length;
			byte num = 0;
			int index = 0;
			bool result;
			while ((long)index < length)
			{
				byte num2 = (byte)sbInputStream.ReadByte();
				if ((num2 & 128) != 0)
				{
					flag = false;
				}
				if (num == 0)
				{
					if (num2 >= 128)
					{
						do
						{
							num2 = (byte)(num2 << 1);
							num += 1;
						}
						while ((num2 & 128) != 0);
						num -= 1;
						if (num == 0)
						{
							result = false;
							return result;
						}
					}
				}
				else
				{
					if ((num2 & 192) != 128)
					{
						result = false;
						return result;
					}
					num -= 1;
				}
				index++;
			}
			result = (num <= 0 && !flag);
			return result;
		}

		public static bool IsValidEmail(string strEmail)
		{
			return Regex.IsMatch(strEmail, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
		}

		[DllImport("dbgHelp", SetLastError = true)]
		private static extern bool MakeSureDirectoryPathExists(string name);

		public static string mashSQL(string str)
		{
			string result;
			if (str == null)
			{
				result = "";
			}
			else
			{
				str = str.Replace("'", "'");
				result = str;
			}
			return result;
		}

		public static string MD5(string str)
		{
			byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(str));
			string str2 = "";
			for (int index = 0; index < hash.Length; index++)
			{
				str2 += hash[index].ToString("x").PadLeft(2, '0');
			}
			return str2;
		}

		public static int RandomInt(int _up, int _down)
		{
			return new Random().Next(_up, _down);
		}

		public static string RemoveHtml(string content)
		{
			string pattern = "<[^>]*>";
			return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
		}

		public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
		{
			return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
		}

		public static string ReplaceStrToScript(string str)
		{
			str = str.Replace("\\", "\\\\");
			str = str.Replace("'", "\\'");
			str = str.Replace("\"", "\\\"");
			return str;
		}

		public static void ResponseFile(string filepath, string filename, string filetype)
		{
			Stream stream = null;
			byte[] buffer = new byte[10000];
			try
			{
				stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
				long num = stream.Length;
				HttpContext.Current.Response.ContentType = filetype;
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Utility.UrlEncode(filename.Trim()).Replace("+", " "));
				while (num > 0L)
				{
					if (HttpContext.Current.Response.IsClientConnected)
					{
						int count = stream.Read(buffer, 0, 10000);
						HttpContext.Current.Response.OutputStream.Write(buffer, 0, count);
						HttpContext.Current.Response.Flush();
						buffer = new byte[10000];
						num -= (long)count;
					}
					else
					{
						num = -1L;
					}
				}
			}
			catch (Exception ex)
			{
				HttpContext.Current.Response.Write("Error : " + ex.Message);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			HttpContext.Current.Response.End();
		}

		public static string RTrim(string str)
		{
			for (int length = str.Length; length >= 0; length--)
			{
				char ch = str[length];
				if (!ch.Equals(" "))
				{
					ch = str[length];
				}
				if (ch.Equals("\r") || str[length].Equals("\n"))
				{
					str.Remove(length, 1);
				}
			}
			return str;
		}

		public static int SafeInt32(object objNum)
		{
			int result;
			if (objNum != null)
			{
				string str = objNum.ToString();
				if (Utility.IsNumber(str))
				{
					if (str.ToString().Length > 9)
					{
						result = 2147483647;
						return result;
					}
					result = int.Parse(str);
					return result;
				}
			}
			result = 0;
			return result;
		}

		public static string SHA256(string str)
		{
			return Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(str)));
		}

		public static string Spaces(int nSpaces)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int index = 0; index < nSpaces; index++)
			{
				stringBuilder.Append(" &nbsp;&nbsp;");
			}
			return stringBuilder.ToString();
		}

		public static string[] SplitString(string strContent, string strSplit)
		{
			string[] result;
			if (strContent.IndexOf(strSplit) >= 0)
			{
				result = Regex.Split(strContent, strSplit.Replace(".", "\\."), RegexOptions.IgnoreCase);
			}
			else
			{
				result = new string[]
				{
					strContent
				};
			}
			return result;
		}

		public static int StrDateDiffHours(string time, int hours)
		{
			int result;
			if (time == "" || time == null)
			{
				result = 1;
			}
			else
			{
				TimeSpan timeSpan = DateTime.Now - DateTime.Parse(time).AddHours((double)hours);
				if (timeSpan.TotalHours > 2147483647.0)
				{
					result = 2147483647;
				}
				else if (timeSpan.TotalHours < -2147483648.0)
				{
					result = -2147483648;
				}
				else
				{
					result = (int)timeSpan.TotalHours;
				}
			}
			return result;
		}

		public static int StrDateDiffMinutes(string time, int minutes)
		{
			int result;
			if (time == "" || time == null)
			{
				result = 1;
			}
			else
			{
				TimeSpan timeSpan = DateTime.Now - DateTime.Parse(time).AddMinutes((double)minutes);
				if (timeSpan.TotalMinutes > 2147483647.0)
				{
					result = 2147483647;
				}
				else if (timeSpan.TotalMinutes < -2147483648.0)
				{
					result = -2147483648;
				}
				else
				{
					result = (int)timeSpan.TotalMinutes;
				}
			}
			return result;
		}

		public static int StrDateDiffSeconds(string Time, int Sec)
		{
			TimeSpan timeSpan = DateTime.Now - DateTime.Parse(Time).AddSeconds((double)Sec);
			int result;
			if (timeSpan.TotalSeconds > 2147483647.0)
			{
				result = 2147483647;
			}
			else if (timeSpan.TotalSeconds < -2147483648.0)
			{
				result = -2147483648;
			}
			else
			{
				result = (int)timeSpan.TotalSeconds;
			}
			return result;
		}

		public static string StrFilter(string str, string bantext)
		{
			string[] strArray = Utility.SplitString(bantext, "\r\n");
			for (int index = 0; index < strArray.Length; index++)
			{
				string oldValue = strArray[index].Substring(0, strArray[index].IndexOf("="));
				string newValue = strArray[index].Substring(strArray[index].IndexOf("=") + 1);
				str = str.Replace(oldValue, newValue);
			}
			return str;
		}

		public static string StrFormat(string str)
		{
			string result;
			if (str == null)
			{
				result = "";
			}
			else
			{
				str = str.Replace("\r\n", "<br />");
				str = str.Replace("\n", "<br />");
				result = str;
			}
			return result;
		}

		public static float StrToFloat(object strValue, float defValue)
		{
			float result;
			if (strValue == null || strValue.ToString().Length > 10)
			{
				result = defValue;
			}
			else
			{
				float num = defValue;
				if (strValue != null && new Regex("^([-]|[0-9])[0-9]*(\\.\\w*)?$").IsMatch(strValue.ToString()))
				{
					num = Convert.ToSingle(strValue);
				}
				result = num;
			}
			return result;
		}

		public static int StrToInt(object strValue, int defValue)
		{
			int result;
			if (strValue == null || strValue.ToString() == string.Empty || strValue.ToString().Length > 10)
			{
				result = defValue;
			}
			else
			{
				string str = strValue.ToString();
				string str2 = str[0].ToString();
				if ((str.Length == 10 && Utility.IsNumber(str2) && int.Parse(str2) > 1) || (str.Length == 10 && !Utility.IsNumber(str2)))
				{
					result = defValue;
				}
				else
				{
					int num = defValue;
					if (strValue != null && new Regex("^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString()))
					{
						num = Convert.ToInt32(strValue);
					}
					result = num;
				}
			}
			return result;
		}

		public static string ToSChinese(string str)
		{
			return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
		}

		public static string ToTChinese(string str)
		{
			return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
		}

		public void transHtml(string path, string outpath)
		{
			Page page = new Page();
			StringWriter stringWriter = new StringWriter();
			page.Server.Execute(path, stringWriter);
			FileStream fileStream;
			if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
			{
				File.Delete(page.Server.MapPath("") + "\\" + outpath);
				fileStream = File.Create(page.Server.MapPath("") + "\\" + outpath);
			}
			else
			{
				fileStream = File.Create(page.Server.MapPath("") + "\\" + outpath);
			}
			byte[] bytes = Encoding.Default.GetBytes(stringWriter.ToString());
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Close();
		}

		public static string UrlDecode(string str)
		{
			return HttpUtility.UrlDecode(str);
		}

		public static string UrlEncode(string str)
		{
			return HttpUtility.UrlEncode(str);
		}

		public static void WriteCookie(string strName, string strValue)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
			cookie.Value = strValue;
			HttpContext.Current.Response.AppendCookie(cookie);
		}

		public static void WriteCookie(string strName, string strValue, int expires)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
			cookie.Value = strValue;
			cookie.Expires = DateTime.Now.AddMinutes((double)expires);
			HttpContext.Current.Response.AppendCookie(cookie);
		}
	}
}
