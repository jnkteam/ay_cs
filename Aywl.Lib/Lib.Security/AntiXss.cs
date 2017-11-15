using Microsoft.Security.Application;
using System;

namespace OriginalStudio.Lib.Security
{
    //参考文章：http://www.cnblogs.com/coderzh/archive/2010/06/24/1764725.html


    /// <summary>
    /// Html同时防止XSS攻击操作。
    /// </summary>
	public sealed class AntiXss
	{
		private AntiXss()
		{
		}

		public static string HtmlAttributeEncode(string s)
		{
			return Microsoft.Security.Application.AntiXss.HtmlAttributeEncode(s);
		}

		public static string HtmlEncode(string s)
		{
			return Microsoft.Security.Application.AntiXss.HtmlEncode(s);
		}

		public static string JavaScriptEncode(string s)
		{
			string result;
			if (s != null && s.Length != 0)
			{
				result = Microsoft.Security.Application.AntiXss.JavaScriptEncode(s);
			}
			else
			{
				result = "''";
			}
			return result;
		}

		public static string UrlEncode(string s)
		{
			return Microsoft.Security.Application.AntiXss.UrlEncode(s);
		}

		public static string VisualBasicScriptEncode(string s)
		{
			return Microsoft.Security.Application.AntiXss.VisualBasicScriptEncode(s);
		}

		public static string XmlAttributeEncode(string s)
		{
			return Microsoft.Security.Application.AntiXss.XmlAttributeEncode(s);
		}

		public static string XmlEncode(string s)
		{
			return Microsoft.Security.Application.AntiXss.XmlEncode(s);
		}
	}
}
