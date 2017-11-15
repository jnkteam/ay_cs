using System;
using System.Text;

namespace OriginalStudio.Lib.Text
{
	public sealed class Regular
	{
		public static string ImageRegularString
		{
			get
			{
				return Regular.GetFileExtRegularString(new string[]
				{
					"jpg",
					"gif",
					"jpeg",
					"bmp",
					"png"
				});
			}
		}

		private Regular()
		{
		}

		public static string GetFileExtRegularString(string[] exts)
		{
			string result;
			if (exts.Length <= 0)
			{
				result = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(".*(\\.(");
				for (int index = 0; index < exts.Length; index++)
				{
					if (index > 0)
					{
						stringBuilder.Append("|");
					}
					if (exts.Length > 1)
					{
						stringBuilder.Append("(");
					}
					string str = exts[index].Trim().ToLower();
					for (int startIndex = 0; startIndex < str.Length; startIndex++)
					{
						stringBuilder.AppendFormat("({0}|{1})", str.Substring(startIndex, 1).ToLower(), str.Substring(startIndex, 1).ToUpper());
					}
					if (exts.Length > 1)
					{
						stringBuilder.Append(")");
					}
				}
				stringBuilder.Append("))$");
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static string GetRegularString(RegularType type)
		{
			return Regular.GetRegularString(type, 0);
		}

		public static string GetRegularString(RegularType type, int length)
		{
			string result;
			switch (type)
			{
			case RegularType.Word:
				if (length > 0)
				{
					result = "^[\\w]{0," + string.Format("{0:d}", length) + "}$";
				}
				else
				{
					result = "^[\\w]*$";
				}
				break;
			case RegularType.Email:
				result = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
				break;
			case RegularType.Url:
				result = "^((h|H)(t|T)(t|T)(p|P)|(f|F)(t|T)(p|P)|(f|F)(i|I)(l|L)(e|E)|(t|T)(e|E)(l|L)(n|N)(e|E)(t|T)|(g|G)(o|O)(p|P)(h|H)(e|E)(r|R)|(h|H)(t|T)(t|T)(p|P)(s|S)|(m|M)(a|A)(i|I)(l|L)(t|T)(o|O)|(n|N)(e|E)(w|W)(s|S)|(w|W)(a|A)(i|I)(s|S))://([\\w-]+(\\.)?)+[\\w-]+(:\\d+)?(/[\\w- ./?%&=]*)?$";
				break;
			case RegularType.Number:
				result = "^-{0,1}\\d{1,}\\.{0,1}\\d{0,}$";
				break;
			case RegularType.Int:
				result = "^-{0,1}\\d{1,}$";
				break;
			case RegularType.Date:
				result = "^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-8]))|(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
				break;
			case RegularType.DateTime:
				result = "^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-8]))|(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\\d):[0-5]?\\d:[0-5]?\\d$";
				break;
			case RegularType.Time:
				result = "^(20|21|22|23|[0-1]?\\d):[0-5]?\\d:[0-5]?\\d$";
				break;
			case RegularType.ChinesePostalCode:
				result = "\\d{6}";
				break;
			case RegularType.ChineseIDCard:
				result = "(^\\d{17}[xX\\d]{1}$)|(^\\d{15}$)";
				break;
			case RegularType.Domain:
				result = "^\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
				break;
			default:
				result = string.Empty;
				break;
			}
			return result;
		}
	}
}
