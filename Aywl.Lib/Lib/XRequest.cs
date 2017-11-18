using System.Web;

namespace OriginalStudio.Lib
{
    public class XRequest
	{
		public static string GetCheckListValue(int listlength, string listname)
		{
			string str = "";
			for (int index = 1; index < listlength + 1; index++)
			{
				if (XRequest.GetString(listname + index.ToString()) != "")
				{
					str = str + XRequest.GetString(listname + index.ToString()) + ",";
				}
			}
			if (str.Contains(","))
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}

		public static string GetCurrentFullHost()
		{
			HttpRequest request = HttpContext.Current.Request;
			string result;
			if (!request.Url.IsDefaultPort)
			{
				result = string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
			}
			else
			{
				result = request.Url.Host;
			}
			return result;
		}

		public static float GetFloat(string strName, float defValue)
		{
			float result;
			if ((double)XRequest.GetQueryFloat(strName, defValue) == (double)defValue)
			{
				result = XRequest.GetFormFloat(strName, defValue);
			}
			else
			{
				result = XRequest.GetQueryFloat(strName, defValue);
			}
			return result;
		}

		public static float GetFormFloat(string strName, float defValue)
		{
			return Utils.Utils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
		}

		public static int GetFormInt(string strName, int defValue)
		{
			return Utils.Utils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
		}

		public static string GetFormString(string strName)
		{
			string result;
			if (HttpContext.Current.Request.Form[strName] == null)
			{
				result = "";
			}
			else
			{
				result = HttpContext.Current.Request.Form[strName];
			}
			return result;
		}

		public static string GetHost()
		{
			return HttpContext.Current.Request.Url.Host;
		}

		public static int GetInt(string strName, int defValue)
		{
			int result;
			if (XRequest.GetQueryInt(strName, defValue) == defValue)
			{
				result = XRequest.GetFormInt(strName, defValue);
			}
			else
			{
				result = XRequest.GetQueryInt(strName, defValue);
			}
			return result;
		}

		public static string GetIP()
		{
			string str = string.Empty;
			string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			string text = ip;
			if (text == null || text == "")
			{
				ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			if (ip == null || ip == string.Empty)
			{
				ip = HttpContext.Current.Request.UserHostAddress;
			}
			string result;
			if (ip == null || !(ip != string.Empty) || !Utils.Utils.IsIP(ip))
			{
				result = "0.0.0.0";
			}
			else
			{
				result = ip;
			}
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

		public static int GetParamCount()
		{
			return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
		}

		public static float GetQueryFloat(string strName, float defValue)
		{
			return Utils.Utils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
		}

		public static int GetQueryInt(string strName, int defValue)
		{
			return Utils.Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
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

		public static string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		public static string GetServerString(string strName)
		{
			string result;
			if (HttpContext.Current.Request.ServerVariables[strName] == null)
			{
				result = "";
			}
			else
			{
				result = HttpContext.Current.Request.ServerVariables[strName].ToString();
			}
			return result;
		}

		public static string GetString(string strName)
		{
			string result;
			if ("".Equals(XRequest.GetQueryString(strName)))
			{
				result = XRequest.GetFormString(strName);
			}
			else
			{
				result = XRequest.GetQueryString(strName);
			}
			return result;
		}

		public static string GetUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		public static string GetUrlReferrer()
		{
			string str = null;
			try
			{
				str = HttpContext.Current.Request.UrlReferrer.ToString();
			}
			catch
			{
			}
			return str ?? "";
		}

		public static bool IsBrowserGet()
		{
			string[] strArray = new string[]
			{
				"ie",
				"opera",
				"netscape",
				"mozilla"
			};
			string str = HttpContext.Current.Request.Browser.Type.ToLower();
			bool result;
			for (int index = 0; index < strArray.Length; index++)
			{
				if (str.IndexOf(strArray[index]) >= 0)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static bool IsGet()
		{
			return HttpContext.Current.Request.HttpMethod.Equals("GET");
		}

		public static bool IsPost()
		{
			return HttpContext.Current.Request.HttpMethod.Equals("POST");
		}

		public static bool IsSearchEnginesGet()
		{
			string[] strArray = new string[]
			{
				"google",
				"yahoo",
				"msn",
				"baidu",
				"sogou",
				"sohu",
				"sina",
				"163",
				"lycos",
				"tom"
			};
			string str = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
			bool result;
			for (int index = 0; index < strArray.Length; index++)
			{
				if (str.IndexOf(strArray[index]) >= 0)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static void SaveRequestFile(string path)
		{
			if (HttpContext.Current.Request.Files.Count > 0)
			{
				HttpContext.Current.Request.Files[0].SaveAs(path);
			}
		}
	}
}
