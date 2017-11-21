using OriginalStudio.Lib.ExceptionHandling;
using OriginalStudio.Lib.TimeControl;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace OriginalStudio.Lib.Web
{
	public sealed class WebBase
	{
		private static HttpApplication _httpApplication;

		public static HttpApplicationState Application
		{
			get
			{
				HttpApplicationState result;
				if (WebBase.Context != null)
				{
					result = WebBase.Context.Application;
				}
				else if (WebBase._httpApplication == null)
				{
					result = null;
				}
				else
				{
					result = WebBase._httpApplication.Application;
				}
				return result;
			}
		}

		public static HttpContext Context
		{
			get
			{
				return HttpContext.Current;
			}
		}

		public static HttpApplication HttpApplication
		{
			set
			{
				WebBase._httpApplication = value;
			}
		}

		public static HttpRequest Request
		{
			get
			{
				HttpRequest result;
				if (WebBase.Context == null)
				{
					result = null;
				}
				else
				{
					result = WebBase.Context.Request;
				}
				return result;
			}
		}

		public static HttpResponse Response
		{
			get
			{
				HttpResponse result;
				if (WebBase.Context == null)
				{
					result = null;
				}
				else
				{
					result = WebBase.Context.Response;
				}
				return result;
			}
		}

		public static HttpServerUtility Server
		{
			get
			{
				HttpServerUtility result;
				if (WebBase.Context == null)
				{
					result = null;
				}
				else
				{
					result = WebBase.Context.Server;
				}
				return result;
			}
		}

		public static HttpSessionState Session
		{
			get
			{
				HttpSessionState result;
				if (WebBase.Context == null)
				{
					result = null;
				}
				else
				{
					result = WebBase.Context.Session;
				}
				return result;
			}
		}

		private WebBase()
		{
		}

		public static NameValueCollection BuildQueryString(NameValueCollection querystring, NameValueCollection list)
		{
			NameValueCollection result;
			if ((querystring == null || querystring.Count == 0) && (list == null || list.Count == 0))
			{
				result = new NameValueCollection(0);
			}
			else if (querystring == null || querystring.Count == 0)
			{
				result = new NameValueCollection(list);
			}
			else if (list == null || list.Count == 0)
			{
				result = new NameValueCollection(querystring);
			}
			else
			{
				NameValueCollection nameValueCollection = new NameValueCollection(querystring);
				for (int index = 0; index < list.AllKeys.Length; index++)
				{
					nameValueCollection[list.AllKeys[index]] = list[list.AllKeys[index]];
				}
				result = nameValueCollection;
			}
			return result;
		}

		public static string BuildQueryStringString(NameValueCollection querystring, NameValueCollection list)
		{
			NameValueCollection nameValueCollection = WebBase.BuildQueryString(querystring, list);
			string result;
			if (nameValueCollection.Count == 0)
			{
				result = string.Empty;
			}
			else
			{
				string str = string.Empty;
				string[] allKeys = nameValueCollection.AllKeys;
				if (allKeys != null)
				{
					for (int index = 0; index < allKeys.Length; index++)
					{
						string[] values = nameValueCollection.GetValues(allKeys[index]);
						for (int index2 = 0; index2 < values.Length; index2++)
						{
							str = ((str.Length != 0) ? (str + string.Format("&{0}={1}", allKeys[index], HttpUtility.UrlEncode(values[index2]))) : (str + string.Format("?{0}={1}", allKeys[index], HttpUtility.UrlEncode(values[index2]))));
						}
					}
				}
				result = str;
			}
			return result;
		}

		public static string GetPageUrl(string pagerParam, int page)
		{
			return WebBase.Request.Path + WebBase.BuildQueryStringString(WebBase.Request.QueryString, new NameValueCollection(1)
			{
				{
					pagerParam,
					string.Format("{0:d}", page)
				}
			});
		}

		public static bool GetQueryStringBoolean(string param, bool defaultValue)
		{
			bool result;
			if (WebBase.Request.QueryString[param] != null)
			{
				bool flag = !defaultValue;
				if (string.Compare(WebBase.Request.QueryString[param], flag.ToString(), true) == 0 || string.Compare(WebBase.Request.QueryString[param], defaultValue ? "0" : "1", true) == 0)
				{
					result = !defaultValue;
					return result;
				}
			}
			result = defaultValue;
			return result;
		}

		public static DateTime GetQueryStringDateTime(string param, DateTime defaultValue)
		{
			DateTime result;
			if (WebBase.Request.QueryString[param] != null && WebBase.Request.QueryString[param].Length != 0)
			{
				result = FormatConvertor.StringToDateTime(WebBase.Request.QueryString[param]);
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		public static decimal GetQueryStringDecimal(string param, decimal defaultValue)
		{
			decimal result;
			if (WebBase.Request.QueryString[param] != null && WebBase.Request.QueryString[param].Length != 0)
			{
				try
				{
					result = Convert.ToDecimal(WebBase.Request.QueryString[param]);
					return result;
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
			result = defaultValue;
			return result;
		}

		public static double GetQueryStringDouble(string param, double defaultValue)
		{
			double result;
			if (WebBase.Request.QueryString[param] != null && WebBase.Request.QueryString[param].Length != 0)
			{
				try
				{
					result = Convert.ToDouble(WebBase.Request.QueryString[param]);
					return result;
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
			result = defaultValue;
			return result;
		}

		public static int GetQueryStringInt32(string param, int defaultValue)
		{
			int result;
			if (WebBase.Request.QueryString[param] != null && WebBase.Request.QueryString[param].Length != 0)
			{
				try
				{
					result = Convert.ToInt32(WebBase.Request.QueryString[param], 10);
					return result;
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
			result = defaultValue;
			return result;
		}

		public static long GetQueryStringInt64(string param, long defaultValue)
		{
			long result;
			if (WebBase.Request.QueryString[param] != null && WebBase.Request.QueryString[param].Length != 0)
			{
				try
				{
					result = Convert.ToInt64(WebBase.Request.QueryString[param], 10);
					return result;
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex);
				}
			}
			result = defaultValue;
			return result;
		}

		public static string GetQueryStringString(string param, string defaultValue)
		{
			string result;
			if (WebBase.Request.QueryString[param] == null)
			{
				result = defaultValue;
			}
			else
			{
				result = WebBase.Request.QueryString[param];
			}
			return result;
		}

		public static string GetFormString(string param, string defaultValue)
		{
			string result;
			if (WebBase.Request.Form[param] == null)
			{
				result = defaultValue;
			}
			else
			{
				result = WebBase.Request.Form[param].ToString();
			}
			return result;
		}

        /// <summary>
        /// 获取请求参数。适用于Get或Post都行
        /// </summary>
        /// <param name="param"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetString(string param, string defaultValue)
        {
            string result;
            result = GetQueryStringString(param, defaultValue);
            if (String.IsNullOrEmpty(result))
            {
                return GetFormString(param,defaultValue);
            }            
            return result;
        }
    }
}
