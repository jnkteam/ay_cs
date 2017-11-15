using System;
using System.Web;

namespace OriginalStudio.Lib
{
	public class SqlKey
	{
		public bool ProcessSqlStr(string Str)
		{
			bool flag = true;
			try
			{
				if (Str.Trim() != "")
				{
					string str = "and |exec |insert |select |delete |update |count | * |chr |mid |master |truncate |char |declare ";
					char[] chArray = new char[]
					{
						'|'
					};
					string[] array = str.Split(chArray);
					for (int i = 0; i < array.Length; i++)
					{
						string str2 = array[i];
						if (Str.ToLower().IndexOf(str2) >= 0)
						{
							flag = false;
							break;
						}
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public void StartProcessRequest()
		{
			try
			{
				string url = "default.aspx";
				if (HttpContext.Current.Request.QueryString != null)
				{
					for (int index = 0; index < HttpContext.Current.Request.QueryString.Count; index++)
					{
						if (!this.ProcessSqlStr(HttpContext.Current.Request.QueryString[HttpContext.Current.Request.QueryString.Keys[index]]))
						{
							HttpContext.Current.Response.Redirect(url);
							HttpContext.Current.Response.End();
						}
					}
				}
				if (HttpContext.Current.Request.Form != null)
				{
					for (int index2 = 0; index2 < HttpContext.Current.Request.Form.Count; index2++)
					{
						string index3 = HttpContext.Current.Request.Form.Keys[index2];
						if (!(index3 == "__VIEWSTATE") && !this.ProcessSqlStr(HttpContext.Current.Request.Form[index3]))
						{
							HttpContext.Current.Response.Redirect(url);
							HttpContext.Current.Response.End();
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
