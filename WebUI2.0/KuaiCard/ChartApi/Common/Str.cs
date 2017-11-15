using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.IO;
using System.Web;
using System.Xml;


namespace General.Common
{
	#region "字符串的处理函数集"
	
	
	/// <summary>
	/// 字符串的处理函数集
	/// </summary>
	public class Str
	{
        #region 正则表达式的使用

        /// <summary>
        /// 判断输入的字符串是否完全匹配正则
        /// </summary>
        /// <param name="RegexExpression">正则表达式</param>
        /// <param name="str">待判断的字符串</param>
        /// <returns></returns>
        public static bool IsValiable(string RegexExpression, string str)
        {
            bool blResult = false;

            Regex rep = new Regex(RegexExpression, RegexOptions.IgnoreCase);

            //blResult = rep.IsMatch(str);
            Match mc = rep.Match(str);

            if (mc.Success)
            {
                if (mc.Value == str) blResult = true;
            }


            return blResult;
        }

        /// <summary>
        /// 转换代码中的URL路径为绝对URL路径
        /// </summary>
        /// <param name="sourceString">源代码</param>
        /// <param name="replaceURL">替换要添加的URL</param>
        /// <returns>string</returns>
        public static string convertURL(string sourceString, string replaceURL)
        {
            Regex rep = new Regex(" (src|href|background|value)=('|\"|)([^('|\"|)http://].*?)('|\"| |>)");
            sourceString = rep.Replace(sourceString, " $1=$2" + replaceURL + "$3$4");

            return sourceString;
        }

        /// <summary>
        /// 获取代码中所有图片的以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetImgFileUrl(string sourceString)
        {
            ArrayList imgArray = new ArrayList();

            Regex r = new Regex("<IMG(.*?)src=('|\"|)(http://.*?)('|\"| |>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!imgArray.Contains(mc[i].Result("$3")))
                {
                    imgArray.Add(mc[i].Result("$3"));
                }
            }

            return imgArray;
        }

        /// <summary>
        /// 获取代码中所有文件的以HTTP开头的URL地址
        /// </summary>
        /// <param name="sourceString">代码内容</param>
        /// <returns>ArrayList</returns>
        public static Hashtable getFileUrlPath(string sourceString)
        {
            Hashtable url = new Hashtable();

            Regex r = new Regex(" (src|href|background|value)=('|\"|)(http://.*?)('|\"| |>)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!url.ContainsValue(mc[i].Result("$3")))
                {
                    url.Add(i, mc[i].Result("$3"));
                }
            }

            return url;
        }

        /// <summary>
        /// 获取一条SQL语句中的所参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static ArrayList SqlParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@"@(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// 获取一条SQL语句中的所参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static ArrayList OracleParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@":(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// 将HTML代码转化成纯文本
        /// </summary>
        /// <param name="sourceHTML">HTML代码</param>
        /// <returns></returns>
        public static string convertText(string sourceHTML)
        {
            string strResult = "";
            Regex r = new Regex("<(.*?)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sourceHTML);

            if (mc.Count == 0)
            {
                strResult = sourceHTML;
            }
            else
            {
                strResult = sourceHTML;

                for (int i = 0; i < mc.Count; i++)
                {
                    strResult = strResult.Replace(mc[i].ToString(), "");
                }
            }

            return strResult;
        }
        #endregion
        
		#region	构造获取分页操作SQL语句

		/// <summary>
		/// 获取分页操作SQL语句(对于排序的字段必须建立索引，优化分页提取方式)
		/// </summary>
		/// <param name="tblName">操作表的名称</param>
		/// <param name="fldName">排序的索引字段</param>
		/// <param name="PageIndex">当前页</param>
		/// <param name="PageSize">每页显示的记录数</param>
		/// <param name="OrderType">排序方式(0为升序、1为降序)</param>
		/// <param name="strWhere">检索的条件语句，不需要加WHERE关键字</param>
		/// <returns></returns>
		public static string ConstructSplitSQL( string tblName, 
			string fldName,
			int PageIndex, 
			int PageSize, 
			int OrderType, 
			string strWhere)
		{
			StringBuilder sbSQL = new StringBuilder();
			string strOldWhere = "";
			string rtnFields = "*";

			// 构造检索条件语句字符串
			if( strWhere != "" )
			{
				// 去除不合法的字符，防止SQL注入式攻击
				strWhere = strWhere.Replace("'", "''");
				strWhere = strWhere.Replace("--", "");
				strWhere = strWhere.Replace(";", "");
				
				strOldWhere = " AND " + strWhere + " ";

				strWhere = " WHERE " + strWhere + " ";
			}

			// 升序操作
			if( OrderType == 0 )
			{
				if( PageIndex == 1 )
				{
					sbSQL.Append("SELECT TOP " + PageSize + " [" + rtnFields + "] FROM [" + tblName + "] ");

					sbSQL.Append(strWhere + "ORDER BY [" + fldName + "] ASC");
				}
				else
				{
					sbSQL.Append("SELECT TOP " + PageSize + " [" + rtnFields + "] FROM [" + tblName + "] ");

					sbSQL.Append("WHERE ([" + fldName + "] > ( SELECT MAX([" + fldName + "]) FROM (SELECT TOP " + ((PageIndex - 1)*PageSize) + " [" + fldName + "] FROM [" + tblName + "]" + strWhere + " ORDER BY [" + fldName + "] ASC ) AS T )) ");

					sbSQL.Append(strOldWhere + "ORDER BY [" + fldName + "] ASC");
				}
			}
				// 降序操作
			else if( OrderType == 1 )
			{
				if( PageIndex == 1 )
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

					sbSQL.Append(strWhere + "ORDER BY " + fldName + " DESC");
				}
				else
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

					sbSQL.Append("WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1)*PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ");

					sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " DESC");
				}
			}
			else // 异常处理
			{
				throw new DataException("未指定任何排序类型。0升序，1为降序");
			}

			return sbSQL.ToString();	
		}

		
		/// <summary>
		/// 获取分页操作SQL语句(对于排序的字段必须建立索引)
		/// </summary>
		/// <param name="tblName">操作表的名称</param>
		/// <param name="fldName">排序的索引字段</param>
		/// <param name="PageIndex">当前页</param>
		/// <param name="PageSize">每页显示的记录数</param>
		/// <param name="rtnFields">返回字段集合，中间用逗号格开。返回全部用“*”</param>
		/// <param name="OrderType">排序方式(0为升序、1为降序)</param>
		/// <param name="strWhere">检索的条件语句，不需要加WHERE关键字</param>
		/// <returns></returns>
		public static string ConstructSplitSQL( string tblName,
			string fldName,
			int PageIndex,
			int PageSize,
			string rtnFields,
			int OrderType,
			string strWhere)
		{
			StringBuilder sbSQL = new StringBuilder();
			string strOldWhere	= " ";			//Where条件

			//构造检索条件语句的字符串
			if ( strWhere != "" )
			{
				//去除不合法的字符，防止SQL注入式攻击
				strWhere = strWhere.Replace("'", "''");				
				strWhere = strWhere.Replace("--","");
				strWhere = strWhere.Replace(":","");

				strOldWhere = " AND " + strWhere + " ";

				strWhere = " WHERE " + strWhere + " ";
			}

			//升序操作	ASC
			if ( OrderType == 0 )
			{
				if ( PageIndex == 1 )
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");
					
					sbSQL.Append(strWhere + "ORDER BY " + fldName + " ASC");
				}
				else
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

					sbSQL.Append("WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1)*PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ");

					sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " ASC");
				}
			}
				//降序操作	DESC
			else if ( OrderType == 1 )
			{
				if ( PageIndex == 1 )
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");
					sbSQL.Append(strWhere + "ORDER BY " + fldName + " DESC");
				}
				else
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

					sbSQL.Append("WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1)*PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ");

					sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " DESC");
				}
			}
			else	//异常处理
			{
				throw new DataException("未指定任何排序类型。0升序，1为降序");
			}

			return sbSQL.ToString();

		}


	
		/// <summary>
		/// 获取分页操作SQL语句(对于排序的字段必须建立索引)
		/// </summary>
		/// <param name="tblName">操作表名</param>
		/// <param name="fldName">排序的索引字段</param>
		/// <param name="unionCondition">用于连接的条件，例如: LEFT JOIN UserInfo u ON (u.UserID = b.UserID)</param>
		/// <param name="PageIndex">当前页</param>
		/// <param name="PageSize">每页显示的记录数</param>
		/// <param name="rtnFields">返回字段集合，中间用逗号格开。返回全部用“*”</param>
		/// <param name="OrderType">排序方式(0为升序、1为降序)</param>
		/// <param name="strWhere">检索的条件语句，不需要加WHERE关键字</param>
		/// <returns></returns>
		public static string ConstructSplitSQL(	string tblName,
			string fldName,
			string unionCondition, 
			int PageIndex,
			int PageSize,
			string rtnFields,
			int OrderType,
			string strWhere)
		{
			StringBuilder sbSQL = new StringBuilder();
			string strOldWhere = "";

			// 构造检索条件语句字符串
			if( strWhere != "" )
			{
				// 去除不合法的字符，防止SQL注入式攻击
				strWhere = strWhere.Replace("'", "''");
				strWhere = strWhere.Replace("--", "");
				strWhere = strWhere.Replace(";", "");
				
				strOldWhere = " AND " + strWhere + " ";

				strWhere = " WHERE " + strWhere + " ";
			}

			// 升序操作
			if( OrderType == 0 )
			{
				if( PageIndex == 1 )
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + unionCondition + " ");

					sbSQL.Append(strWhere + "ORDER BY " + fldName + " ASC");
				}
				else
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + unionCondition + " ");

					sbSQL.Append("WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1)*PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ");

					sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " ASC");
				}
			}
				// 降序操作
			else if( OrderType == 1 )
			{
				if( PageIndex == 1 )
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ");

					sbSQL.Append(strWhere + "ORDER BY " + fldName + " DESC");
				}
				else
				{
					sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ");

					sbSQL.Append("WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1)*PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ");

					sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " DESC");
				}
			}
			else // 异常处理
			{
				throw new DataException("未指定任何排序类型。0升序，1为降序");
			}

			return sbSQL.ToString();
		}

		#endregion
        
		#region 字符串编码处理

		/// <summary>
		/// 将字符串转换为 HTML 编码的字符串，以便实现从 Web 服务器到客户端的可靠的 HTTP 传输
		/// </summary>
		/// <param name="str">转换字符串</param>
		/// <returns></returns>
		public static string HtmlEncode(string str)
		{
			try
			{
				return System.Web.HttpUtility.HtmlEncode(str);
			}
			catch
			{
				return string.Empty;
			}
		}


		/// <summary>
		/// 将已经为 HTTP 传输进行过 HTML 编码的字符串转换为已解码的字符串。
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string HtmlDecode(string str)
		{
			try
			{
				return System.Web.HttpUtility.HtmlDecode(str);
				//string S = System.Web.HttpUtility.HtmlDecode(str);
				//S = S.Replace("&quot;", "\"").Replace("&nbsp;"," "); 
				//return S;
			}
			catch
			{
				return string.Empty;
			}

		}


		/// <summary>
		/// 对 URL 字符串(包括中文)进行编码，以便实现从 Web 服务器到客户端的可靠的 HTTP 传输。
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlEncode(string str)
		{
			try
			{
				return System.Web.HttpUtility.UrlEncodeUnicode(str);
			}
			catch
			{
				return string.Empty;
			}
		}


		/// <summary>
		/// 将已经为在 URL 中传输而编码的字符串转换为解码的字符串。
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlDecode(string str)
		{
			try
			{
				return System.Web.HttpUtility.UrlDecode(str);
			}
			catch
			{
				return string.Empty;
			}
		}



		#endregion
        
		#region 对字符串的加密/解密

		/// <summary>
		/// 对字符串进行适应 ServU 的 MD5 加密
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string strServUPWD(string str)
		{
			string strResult = "";
			strResult = Str.RandomSTR(2);
			str = strResult + str;
			str = NoneEncrypt(str,1);
			str = strResult + str;

			return str;
		}


		/// <summary>
		/// 对字符串进行加密（不可逆）
		/// </summary>
		/// <param name="Password">要加密的字符串</param>
		/// <param name="Format">加密方式,0 is SHA1,1 is MD5</param>
		/// <returns></returns>
		public static string NoneEncrypt(string Password,int Format)
		{
			string strResult = "";
			switch( Format )
			{
				case 0	:
					strResult = FormsAuthentication.HashPasswordForStoringInConfigFile(Password,"SHA1");
					break;
				case 1	:
					strResult = FormsAuthentication.HashPasswordForStoringInConfigFile(Password,"MD5");
					break;
				default	:
					strResult = Password;
					break;
			}

			return strResult;
		}


		/// <summary>
		/// 对字符串进行加密
		/// </summary>
		/// <param name="Passowrd">待加密的字符串</param>
		/// <returns>string</returns>
		public static string Encrypt(string Passowrd)
		{
			string strResult = "";

			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(Passowrd, true, 2);
			strResult = FormsAuthentication.Encrypt(ticket).ToString();
			
			return strResult;
		}


		/// <summary>
		/// 对字符串进行解密
		/// </summary>
		/// <param name="Passowrd">已解密的字符串</param>
		/// <returns></returns>
		public static string Decrypt(string Passowrd)
		{
			string strResult = "";

			strResult = FormsAuthentication.Decrypt(Passowrd).Name.ToString();
			
			return strResult;
		}

		#endregion
        
		#region 获取随机数
        
		/// <summary>
		/// 获取指定长度的纯数字随机数字串
		/// </summary>
		/// <param name="intLong">数字串长度</param>
		/// <returns>字符串</returns>
		public static string RandomNUM(int intLong)
		{
			string strResult = "";

			Random r = new Random();
			for (int i = 0; i < intLong; i++)
			{
				strResult = strResult + r.Next(10);
			}

			return strResult;
		}
        
		/// <summary>
		/// 获取一个由26个小写字母组成的指定长度的随即字符串
		/// </summary>
		/// <param name="intLong">指定长度</param>
		/// <returns></returns>
		public static string RandomSTR(int intLong)
		{
			string strResult = "";
			string[] array = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

			Random r = new Random();

			for (int i = 0; i < intLong; i++)
			{
				strResult += array[r.Next(26)];
			}

			return strResult;
		}
        
		/// <summary>
		/// 获取一个由数字和26个小写字母组成的指定长度的随即字符串
		/// </summary>
		/// <param name="intLong">指定长度</param>
		/// <returns></returns>
		public static string RandomNUMSTR(int intLong)
		{
			string strResult = "";
			string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

			Random r = new Random();

			for (int i = 0; i < intLong; i++)
			{
				strResult += array[r.Next(36)];
			}

			return strResult;
		}
		#endregion
        
		#region 验证函数

		/// <summary>
		/// 判断字符串是否为空。
		/// </summary>
		/// <param name="str">待检测字符串</param>
		/// <returns></returns>
		public static bool IsEmpty(string str)
		{
			try
			{
				if (str.Trim().ToString() == string.Empty)
					return true;
				else
					return false;
			}
			catch
			{
				return false;
			}

		}

		/// <summary>
		/// 判断字符串是否为有效的邮件地址
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email, @"^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$");
		}


		/// <summary>
		/// 判断字符串是否为有效的URL地址
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static bool IsValidURL(string url)
		{
			return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
		}


		/// <summary>
		/// 判断字符串是否为Int类型的
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool IsValidInt(string val)
		{
			return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
		}


		/// <summary>
		/// 检测字符串是否全为正整数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsNum(string str)
		{
			bool blResult = true;//默认状态下是数字

			if (str == "")
				blResult = false;
			else
			{
				foreach (char Char in str)
				{
					if (!char.IsNumber(Char))
					{
						blResult = false;
						break;
					}
				}
				if (blResult)
				{
					if (int.Parse(str) == 0)
						blResult = false;
				}
			}
			return blResult;
		}


		/// <summary>
		/// 检测字符串是否全为数字型
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsDouble(string str)
		{
			bool blResult = true;//默认状态下是数字

			if (str == "")
				blResult = false;
			else
			{
				foreach (char Char in str)
				{
					if (!char.IsNumber(Char) && Char.ToString() != "-")
					{
						blResult = false;
						break;
					}
				}
			}
			return blResult;
		}

        /// <summary>
        /// 验证是否是日期字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                DateTime t = Convert.ToDateTime(str);
                return true;
            }
            catch
            {
                return false; 
            }
        }

		#endregion
        
		#region 类型转化函数

		/// <summary>
		/// 将变量Obj转为Int32类型。
		/// </summary>
		/// <param name="Obj"></param>
		/// <returns></returns>
		public static Int32 ConvertToInt32(Object Obj)
		{
			try
			{
				return Convert.ToInt32(Obj);
			}
			catch (Exception E)
			{
				return -1;
			}
		}

        public static double ConvertToDouble(Object Obj)
        {
            try
            {
                return Convert.ToDouble(Obj);
            }
            catch (Exception E)
            {
                return 0;
            }
        }
		#endregion

        #region 拼音首字母

        /// <summary>
        /// 获取字符串拼音首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseFirstSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        private static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = {45217,45253,45761,46318,46826,47010,47297,47614,48119,48119,49062,
49324,49896,50371,50614,50622,50906,51387,51446,52218,52698,52698,52698,52980,53689,
54481};
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }


        #endregion

        /// <summary>
		/// 格式化日期字符串
		/// </summary>
		/// <param name="str">日期字符串</param>
		/// <param name="Type">0: 返回日期   1：返回时间   2：返回日期+时间</param>
		/// <returns></returns>
		public static string FormatDateTimeString(string str,int Type)
		{
			try
			{
				if (Type == 0)
				{
					return Convert.ToDateTime(str).ToShortDateString();	//短日期字符串,如：2007-1-1
				}
				else if( Type == 1)
				{
					return Convert.ToDateTime(str).ToShortTimeString();	//短日期字符串,如：12:00
				}
				else if (Type == 2)
				{
					return Convert.ToDateTime(str).ToString();			//如：2007-1-1 12：00
				}
				else
					return string.Empty;
			}
			catch
			{
				return string.Empty;
			}
		}
        		
		/// <summary>
		/// 得到字符串的前intCount个字符内容。
		/// 格式化字符串长度,超出部分显示省略号，可以区分汉字跟字母。汉字2个字节,字母数字一个字节
		/// </summary>
		/// <param name="str"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static string GetNumString(string str,int n)
		{
			///
			///
			///
			string temp=string.Empty;
			if(System.Text.Encoding.Default.GetByteCount(str)<=n)//如果长度比需要的长度n小'返回原字符串
			{
				return str;
			}
			else
			{
				int t=0;
				char[] q=str.ToCharArray();
				for(int i=0;i<q.Length&&t<n;i++)
				{
					if((int)q[i]>=0x4E00 && (int)q[i]<=0x9FA5)//是否汉字
					{
						temp+=q[i];
						t+=2;
					}
					else
					{
						temp+=q[i];
						t++;
					}
				}
				return (temp+"...");
			}
		}
        
		/// <summary>
		/// 将数字字符串转换为整数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int StringToInt(string str)
		{
			if (str == null) return 0;

			if (str.Trim() == string.Empty)
				return 0;
			try
			{
				return Convert.ToInt32(str);
			}
			catch
			{
				//防止有字母或文字
				return 0;
			}
		}
        
		/// <summary>
		/// 将 Stream 转化成 string
		/// </summary>
		/// <param name="s">Stream流</param>
		/// <returns>string</returns>
		public static string ConvertStreamToString(Stream s)
		{
			string strResult	= "";
			StreamReader sr		= new StreamReader( s, Encoding.UTF8 );

			Char[] read = new Char[256];

			// Read 256 charcters at a time.    
			int count = sr.Read( read, 0, 256 );

			while (count > 0) 
			{
				// Dump the 256 characters on a string and display the string onto the console.
				string str = new String(read, 0, count);
				strResult += str;
				count = sr.Read(read, 0, 256);
			}


			// 释放资源
			sr.Close();

			return strResult;
		}

		/// <summary>
		/// 对传递的参数字符串进行处理，防止注入式攻击
		/// </summary>
		/// <param name="str">传递的参数字符串</param>
		/// <returns>String</returns>
		public static string ConvertSql(string str)
		{
			str = str.Trim();
			str = str.Replace("'", "''");
			str = str.Replace(";--", "");
			str = str.Replace("=", "");
			str = str.Replace(" or ", "");
			str = str.Replace(" and ", "");

			return str;
		}

		/// <summary>
		/// 格式化占用空间大小的输出
		/// </summary>
		/// <param name="size">大小，单位字节</param>
		/// <returns>返回 String</returns>
		public static string FormatNUM(long size)
		{
			decimal NUM;
			string strResult;

			if( size > 1073741824 )			//1G = 1024*1024*1024B  将B转为G
			{
				NUM = (Convert.ToDecimal(size)/Convert.ToDecimal(1073741824));
				strResult = NUM.ToString("N") + " G";
			}
			else if( size > 1048576 )		//1M = 1024*1024B	将B转为M
			{
				NUM = (Convert.ToDecimal(size)/Convert.ToDecimal(1048576));
				strResult = NUM.ToString("N") + " M";
			}
			else if( size > 1024 )			//1K = 1024B  将B转为K
			{
				NUM = (Convert.ToDecimal(size)/Convert.ToDecimal(1024));
				strResult = NUM.ToString("N") + " KB";
			}
			else
			{
				strResult = size + " 字节";
			}

			return strResult;
		}
        
		/// <summary>
		/// 输出由同一字符组成的指定长度的字符串
		/// </summary>
		/// <param name="Char">输出字符，如：A</param>
		/// <param name="i">指定长度</param>
		/// <returns></returns>
		public static string Strings(char Char, int i)
		{
			string strResult = null;

			for(int j = 0; j < i; j++)
			{
				strResult += Char;
			}
			return strResult;
		}
        
		/// <summary>
		/// 返回字符串的真实长度，一个汉字字符相当于两个单位长度
		/// </summary>
		/// <param name="str">指定字符串</param>
		/// <returns></returns>
		public static int Len(string str)
		{
			int intResult = 0;

			foreach(char Char in str)
			{
				if((int)Char > 127)
					intResult += 2;
				else
					intResult ++;
			}
			return intResult;
		}
        
		/// <summary>
		/// 以日期为标准获得一个绝对的名称
		/// </summary>
		/// <returns>返回 String</returns>
		public static string MakeName()
		{
			/*
				string y = DateTime.Now.Year.ToString();
				string m = DateTime.Now.Month.ToString();
				string d = DateTime.Now.Day.ToString();
				string h = DateTime.Now.Hour.ToString();
				string n = DateTime.Now.Minute.ToString();
				string s = DateTime.Now.Second.ToString();
				return y + m + d + h + n + s;
				*/
            string newFileName;
            int K = Guid.NewGuid().GetHashCode();
            if (K < 0) K = 0 - K;
            string dateName = System.DateTime.Now.ToString("yyyyMMddhhmmssfff");
            newFileName = dateName + K.ToString().Substring(0, 5);
            return newFileName; 
		}
        
		/// <summary>
		/// 返回字符串的真实长度，一个汉字字符相当于两个单位长度(使用Encoding类)
		/// </summary>
		/// <param name="str">指定字符串</param>
		/// <returns></returns>
		public static int getLen(string str)
		{
			int intResult = 0;
			Encoding gb2312 = Encoding.GetEncoding("gb2312"); 
			byte[] bytes = gb2312.GetBytes(str);
			intResult = bytes.Length;
			return intResult;
		}
        
		/// <summary>
		/// 按照字符串的实际长度截取指定长度的字符串
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="Length">指定长度</param>
		/// <returns></returns>
		public static string CutLen(string str, int Length)
		{
			int i = 0, j = 0;

			foreach(char Char in str)
			{
				if((int)Char > 127)
					i += 2;
				else
					i ++;

				if(i > Length)
				{
					str = str.Substring(0, j - 2) + "...";
					break;
				}
				j ++;
			}
			return str;
		}
        
		/// <summary>
		/// 检测obj值是否有效，为 null 或 "" 均为无效，返回false。否则返回true
		/// </summary>
		/// <param name="obj">要检测的值</param>
		/// <returns></returns>
		public static bool CheckValiable(object obj)
		{
			if( Object.Equals(obj, null) || Object.Equals(obj, string.Empty) )
				return false;
			else
				return true;
		}

        /// <summary>
        /// 过滤非法字符。
        /// </summary>
        /// <param name="strTmp"></param>
        /// <returns></returns>
        public static string CheckFileName(string strTmp)
        {
            return strTmp.Replace(@"/", "").
                //Replace(@"\", "").
                //Replace(":", "").
              Replace("*", "").
              Replace("?", "").
              Replace("/", "").
              Replace(@"<", "").
              Replace(@">", "").
              Replace(" ", "").
              Replace(@"|", "").
              Replace(":", "").
              Replace("\"", "").
              Replace("\t", "").
              Replace("\r\n", "").
              Replace("&quot;", "").
              Replace("&nbsp;", "");
        }

        /// <summary>
        /// 过滤非法字符。
        /// </summary>
        /// <param name="strTmp"></param>
        /// <returns></returns>
        public static string CheckString(string strTmp)
        {
            return strTmp.Replace(@"/", "").
                //Replace(@"\", "").
                //Replace(":", "").
              Replace("*", "").
              Replace("?", "").
              Replace("/", "").
              Replace(@"<", "").
              Replace(@">", "").
              Replace(" ", "").
              Replace(@"|", "").
              Replace(":", "").
              Replace("\"", "").
              Replace("\t", "").
              Replace("&quot;", "").
              Replace("&nbsp;", "");
        }

        /// <summary>
        /// 格式化XML字符串（少用）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatToXml(string str)
        {
            MemoryStream mstream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mstream, null);
            XmlDocument xmldoc = new XmlDocument();
            writer.Formatting = Formatting.Indented;

            xmldoc.LoadXml(str);
            xmldoc.WriteTo(writer);
            writer.Flush();
            writer.Close();

            Encoding encoding = Encoding.GetEncoding("utf-8");
            string strReturn = encoding.GetString(mstream.ToArray());
            mstream.Close();
            return strReturn;
        }

        /// <summary>   
        /// 将时间戳TimeStamp转换为DateTime   
        /// </summary>   
        /// <param name="timeStamp">时间戳</param>   
        /// <returns>真实时间</returns>   
        public static DateTime GetRealTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNowTime = new TimeSpan(lTime);
            return startTime.Add(toNowTime);
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertToTimeStamp(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t;
        }
    }
	#endregion
}
