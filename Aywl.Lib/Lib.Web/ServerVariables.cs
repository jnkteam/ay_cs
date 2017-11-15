using System;

namespace OriginalStudio.Lib.Web
{
	public sealed class ServerVariables
	{
		public static string TrueIP
		{
			get
			{
				string result;
				if (WebBase.Request != null)
				{
					if (WebBase.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
					{
						if (WebBase.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
						{
							result = WebBase.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
							return result;
						}
						result = string.Empty;
						return result;
					}
					else if (WebBase.Request.ServerVariables["REMOTE_ADDR"] != null)
					{
						result = WebBase.Request.ServerVariables["REMOTE_ADDR"];
						return result;
					}
				}
				result = string.Empty;
				return result;
			}
		}

		private ServerVariables()
		{
		}
	}
}
