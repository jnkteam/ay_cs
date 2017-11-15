using System;
using System.Text;

namespace OriginalStudio.Lib.Text
{
	public class Strings
	{
		public static string MoneyToChinese(string LowerMoney)
		{
			bool flag = false;
			if (LowerMoney.Trim().Substring(0, 1) == "-")
			{
				LowerMoney = LowerMoney.Trim().Remove(0, 1);
				flag = true;
			}
			string str = null;
			LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
			if (LowerMoney.IndexOf(".") > 0)
			{
				if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
				{
					LowerMoney += "0";
				}
			}
			else
			{
				LowerMoney += ".00";
			}
			string str2 = LowerMoney;
			int num = 1;
			string str3 = "";
			while (num <= str2.Length)
			{
				string text = str2.Substring(str2.Length - num, 1);
				switch (text)
				{
				case ".":
					str = "圆";
					break;
				case "0":
					str = "零";
					break;
				case "1":
					str = "壹";
					break;
				case "2":
					str = "贰";
					break;
				case "3":
					str = "叁";
					break;
				case "4":
					str = "肆";
					break;
				case "5":
					str = "伍";
					break;
				case "6":
					str = "陆";
					break;
				case "7":
					str = "柒";
					break;
				case "8":
					str = "捌";
					break;
				case "9":
					str = "玖";
					break;
				}
				switch (num)
				{
				case 1:
					str += "分";
					break;
				case 2:
					str += "角";
					break;
				case 3:
					str = (str ?? "");
					break;
				case 4:
					str = (str ?? "");
					break;
				case 5:
					str += "拾";
					break;
				case 6:
					str += "佰";
					break;
				case 7:
					str += "仟";
					break;
				case 8:
					str += "万";
					break;
				case 9:
					str += "拾";
					break;
				case 10:
					str += "佰";
					break;
				case 11:
					str += "仟";
					break;
				case 12:
					str += "亿";
					break;
				case 13:
					str += "拾";
					break;
				case 14:
					str += "佰";
					break;
				case 15:
					str += "仟";
					break;
				case 16:
					str += "万";
					break;
				default:
					str = (str ?? "");
					break;
				}
				str3 = str + str3;
				num++;
			}
			string str4 = str3.Replace("零拾", "零").Replace("零佰", "零").Replace("零仟", "零").Replace("零零零", "零").Replace("零零", "零").Replace("零角零分", "整").Replace("零分", "整").Replace("零角", "零").Replace("零亿零万零圆", "亿圆").Replace("亿零万零圆", "亿圆").Replace("零亿零万", "亿").Replace("零万零圆", "万圆").Replace("零亿", "亿").Replace("零万", "万").Replace("零圆", "圆").Replace("零零", "零");
			if (str4.Substring(0, 1) == "圆")
			{
				str4 = str4.Substring(1, str4.Length - 1);
			}
			if (str4.Substring(0, 1) == "零")
			{
				str4 = str4.Substring(1, str4.Length - 1);
			}
			if (str4.Substring(0, 1) == "角")
			{
				str4 = str4.Substring(1, str4.Length - 1);
			}
			if (str4.Substring(0, 1) == "分")
			{
				str4 = str4.Substring(1, str4.Length - 1);
			}
			if (str4.Substring(0, 1) == "整")
			{
				str4 = "零圆整";
			}
			string str5 = str4;
			string result;
			if (flag)
			{
				result = "负" + str5;
			}
			else
			{
				result = str5;
			}
			return result;
		}

		public static string ReplaceStringSeparator(string s)
		{
			return s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
		}

		public static string MergeString(string source, string target)
		{
			return Strings.MergeString(source, target, ",");
		}

		public static string MergeString(string source, string target, string mergechar)
		{
			target = ((!string.IsNullOrEmpty(target)) ? (target + mergechar + source) : source);
			return target;
		}

		public static string ReplaceString(string source, int start, int len, string repchar)
		{
			string result;
			try
			{
				string str = string.Empty;
				if (string.IsNullOrEmpty(source) || source.Length < start + len)
				{
					result = str;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int index = 0; index < len; index++)
					{
						stringBuilder.Append(repchar);
					}
					result = source.Replace(source.Substring(start, len), stringBuilder.ToString());
				}
			}
			catch
			{
				result = source;
			}
			return result;
		}

		public static string ReplaceString(string source, int lev, string repchar)
		{
			string result;
			try
			{
				string str = string.Empty;
				if (string.IsNullOrEmpty(source) || source.Length < lev)
				{
					result = str;
				}
				else
				{
					int length = source.Length - lev;
					StringBuilder stringBuilder = new StringBuilder();
					for (int index = 0; index < length; index++)
					{
						stringBuilder.Append(repchar);
					}
					result = source.Replace(source.Substring(0, length), stringBuilder.ToString());
				}
			}
			catch
			{
				result = source;
			}
			return result;
		}

		public static string Mark(string num)
		{
			string str = "";
			string result;
			if (num.Length > 4)
			{
				int length = num.Length / 3;
				for (int index = 0; index < num.Length - length - length; index++)
				{
					str += "*";
				}
				result = num.Substring(0, length) + str + num.Substring(num.Length - length, length);
			}
			else if (num.Length <= 1)
			{
				result = num;
			}
			else
			{
				for (int index = 0; index < num.Length - 1; index++)
				{
					str += "*";
				}
				result = num.Substring(0, 1) + str;
			}
			return result;
		}

		public static string Mark(string num, char split)
		{
			string[] strArray = num.Split(new char[]
			{
				split
			});
			string result;
			if (strArray.Length >= 2)
			{
				result = Strings.Mark(strArray[0]) + split.ToString() + strArray[1];
			}
			else
			{
				result = num;
			}
			return result;
		}

		public static string ReplaceString(string source, char split, int lev, string repchar)
		{
			string result;
			try
			{
				string str = string.Empty;
				if (string.IsNullOrEmpty(source))
				{
					result = str;
				}
				else
				{
					string[] strArray = source.Split(new char[]
					{
						split
					});
					if (strArray.Length == 1)
					{
						result = str;
					}
					else
					{
						string str2 = strArray[0];
						if (str2.Length < lev)
						{
							result = str;
						}
						else
						{
							int num = str2.Length - lev;
							StringBuilder stringBuilder = new StringBuilder();
							for (int index = 0; index < num; index++)
							{
								stringBuilder.Append(repchar);
							}
							result = source.Replace(source.Substring(lev - 1, num - 1), stringBuilder.ToString());
						}
					}
				}
			}
			catch
			{
				result = source;
			}
			return result;
		}
	}
}
