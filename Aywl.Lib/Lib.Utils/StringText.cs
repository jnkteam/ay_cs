using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OriginalStudio.Lib.Utils
{
	public class StringText
	{
		public static string BuilderCode(int n)
		{
			string[] strArray = new string[]
			{
				"0",
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"a",
				"b",
				"c",
				"d",
				"e",
				"f",
				"g",
				"h",
				"i",
				"j",
				"k",
				"l",
				"m",
				"n",
				"o",
				"p",
				"q",
				"r",
				"s",
				"t",
				"u",
				"v",
				"w",
				"x",
				"y",
				"z",
				"A",
				"B",
				"C",
				"D",
				"E",
				"F",
				"G",
				"H",
				"I",
				"J",
				"K",
				"L",
				"M",
				"N",
				"O",
				"P",
				"Q",
				"R",
				"S",
				"T",
				"U",
				"V",
				"W",
				"X",
				"Y",
				"Z"
			};
			StringBuilder stringBuilder = new StringBuilder();
			int num = -1;
			Random random = new Random();
			string result;
			for (int index = 1; index < n + 1; index++)
			{
				if (num != -1)
				{
					random = new Random(index * num * (int)DateTime.Now.Ticks);
				}
				int index2 = random.Next(57);
				if (num != -1 && num == index2)
				{
					result = StringText.BuilderCode(n);
					return result;
				}
				num = index2;
				stringBuilder.Append(strArray[index2]);
			}
			result = stringBuilder.ToString();
			return result;
		}

		public static string GetPageName()
		{
			string[] strArray = HttpContext.Current.Request.Url.AbsolutePath.Split(new char[]
			{
				'/'
			});
			return strArray[strArray.Length - 1].ToLower();
		}

		public static string GetQueryString(string strName)
		{
			string result;
			if (HttpContext.Current.Request.QueryString[strName] == null)
			{
				result = "";
			}
			else
			{
				result = HttpContext.Current.Request.QueryString[strName];
			}
			return result;
		}

		public static int GetStringLength(string str)
		{
			return Encoding.Default.GetBytes(str).Length;
		}

		public static string getWeekDay(int y, int m, int d)
		{
			DateTime now = DateTime.Now;
			return string.Concat(new string[]
			{
				now.Year.ToString(),
				"年",
				now.Month.ToString(),
				"月",
				now.Day.ToString(),
				"日"
			});
		}

		public static bool IsUnicode(string s)
		{
			string pattern = "^[\\u4E00-\\u9FA5\\uE815-\\uFA29]+$";
			return Regex.IsMatch(s, pattern);
		}

		public static string Left(string str, int need, bool encode)
		{
			string result;
			if (str == null || str == string.Empty)
			{
				result = string.Empty;
			}
			else
			{
				int length = str.Length;
				if (length < need / 2)
				{
					result = (encode ? StringText.TextEncode(str) : str);
				}
				else
				{
					int num = 0;
					int length2;
					for (length2 = 0; length2 < length; length2++)
					{
						num += (StringText.IsUnicode(str[length2].ToString()) ? 2 : 1);
						if (num >= need)
						{
							break;
						}
					}
					string str2 = str.Substring(0, length2);
					if (length > length2)
					{
						int num2;
						for (num2 = 0; num2 < 5; num2++)
						{
							if (length2 - num2 >= str.Length || length2 - num2 < 0)
							{
								num2--;
								break;
							}
							num -= (StringText.IsUnicode(str[length2 - num2].ToString()) ? 2 : 1);
							if (num <= need)
							{
								break;
							}
						}
						str2 = str.Substring(0, length2 - num2) + "...";
					}
					result = (encode ? StringText.TextEncode(str2) : str2);
				}
			}
			return result;
		}

		public static string ShitEncode(string str)
		{
			string input = "";
			string pattern = (input != null && !(input == string.Empty)) ? Regex.Replace(Regex.Replace(input, "\\|{2,}", "|"), "(^\\|)|(\\|$)", "") : "妈的|你妈|他妈|妈b|妈比|我日|我操|法轮|fuck|shit";
			return Regex.Replace(str, pattern, "**", RegexOptions.IgnoreCase);
		}

		public static string TextEncode(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str);
			stringBuilder.Replace("&", "&amp;");
			stringBuilder.Replace("<", "&lt;");
			stringBuilder.Replace(">", "&gt;");
			stringBuilder.Replace("\"", "&quot;");
			stringBuilder.Replace("'", "&#39;");
			return StringText.ShitEncode(stringBuilder.ToString());
		}
	}
}
