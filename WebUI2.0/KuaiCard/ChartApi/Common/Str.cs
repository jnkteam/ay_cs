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
	#region "�ַ����Ĵ�������"
	
	
	/// <summary>
	/// �ַ����Ĵ�������
	/// </summary>
	public class Str
	{
        #region ������ʽ��ʹ��

        /// <summary>
        /// �ж�������ַ����Ƿ���ȫƥ������
        /// </summary>
        /// <param name="RegexExpression">������ʽ</param>
        /// <param name="str">���жϵ��ַ���</param>
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
        /// ת�������е�URL·��Ϊ����URL·��
        /// </summary>
        /// <param name="sourceString">Դ����</param>
        /// <param name="replaceURL">�滻Ҫ��ӵ�URL</param>
        /// <returns>string</returns>
        public static string convertURL(string sourceString, string replaceURL)
        {
            Regex rep = new Regex(" (src|href|background|value)=('|\"|)([^('|\"|)http://].*?)('|\"| |>)");
            sourceString = rep.Replace(sourceString, " $1=$2" + replaceURL + "$3$4");

            return sourceString;
        }

        /// <summary>
        /// ��ȡ����������ͼƬ����HTTP��ͷ��URL��ַ
        /// </summary>
        /// <param name="sourceString">��������</param>
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
        /// ��ȡ�����������ļ�����HTTP��ͷ��URL��ַ
        /// </summary>
        /// <param name="sourceString">��������</param>
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
        /// ��ȡһ��SQL����е�������
        /// </summary>
        /// <param name="sql">SQL���</param>
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
        /// ��ȡһ��SQL����е�������
        /// </summary>
        /// <param name="sql">SQL���</param>
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
        /// ��HTML����ת���ɴ��ı�
        /// </summary>
        /// <param name="sourceHTML">HTML����</param>
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
        
		#region	�����ȡ��ҳ����SQL���

		/// <summary>
		/// ��ȡ��ҳ����SQL���(����������ֶα��뽨���������Ż���ҳ��ȡ��ʽ)
		/// </summary>
		/// <param name="tblName">�����������</param>
		/// <param name="fldName">����������ֶ�</param>
		/// <param name="PageIndex">��ǰҳ</param>
		/// <param name="PageSize">ÿҳ��ʾ�ļ�¼��</param>
		/// <param name="OrderType">����ʽ(0Ϊ����1Ϊ����)</param>
		/// <param name="strWhere">������������䣬����Ҫ��WHERE�ؼ���</param>
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

			// ���������������ַ���
			if( strWhere != "" )
			{
				// ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
				strWhere = strWhere.Replace("'", "''");
				strWhere = strWhere.Replace("--", "");
				strWhere = strWhere.Replace(";", "");
				
				strOldWhere = " AND " + strWhere + " ";

				strWhere = " WHERE " + strWhere + " ";
			}

			// �������
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
				// �������
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
			else // �쳣����
			{
				throw new DataException("δָ���κ��������͡�0����1Ϊ����");
			}

			return sbSQL.ToString();	
		}

		
		/// <summary>
		/// ��ȡ��ҳ����SQL���(����������ֶα��뽨������)
		/// </summary>
		/// <param name="tblName">�����������</param>
		/// <param name="fldName">����������ֶ�</param>
		/// <param name="PageIndex">��ǰҳ</param>
		/// <param name="PageSize">ÿҳ��ʾ�ļ�¼��</param>
		/// <param name="rtnFields">�����ֶμ��ϣ��м��ö��Ÿ񿪡�����ȫ���á�*��</param>
		/// <param name="OrderType">����ʽ(0Ϊ����1Ϊ����)</param>
		/// <param name="strWhere">������������䣬����Ҫ��WHERE�ؼ���</param>
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
			string strOldWhere	= " ";			//Where����

			//����������������ַ���
			if ( strWhere != "" )
			{
				//ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
				strWhere = strWhere.Replace("'", "''");				
				strWhere = strWhere.Replace("--","");
				strWhere = strWhere.Replace(":","");

				strOldWhere = " AND " + strWhere + " ";

				strWhere = " WHERE " + strWhere + " ";
			}

			//�������	ASC
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
				//�������	DESC
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
			else	//�쳣����
			{
				throw new DataException("δָ���κ��������͡�0����1Ϊ����");
			}

			return sbSQL.ToString();

		}


	
		/// <summary>
		/// ��ȡ��ҳ����SQL���(����������ֶα��뽨������)
		/// </summary>
		/// <param name="tblName">��������</param>
		/// <param name="fldName">����������ֶ�</param>
		/// <param name="unionCondition">�������ӵ�����������: LEFT JOIN UserInfo u ON (u.UserID = b.UserID)</param>
		/// <param name="PageIndex">��ǰҳ</param>
		/// <param name="PageSize">ÿҳ��ʾ�ļ�¼��</param>
		/// <param name="rtnFields">�����ֶμ��ϣ��м��ö��Ÿ񿪡�����ȫ���á�*��</param>
		/// <param name="OrderType">����ʽ(0Ϊ����1Ϊ����)</param>
		/// <param name="strWhere">������������䣬����Ҫ��WHERE�ؼ���</param>
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

			// ���������������ַ���
			if( strWhere != "" )
			{
				// ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
				strWhere = strWhere.Replace("'", "''");
				strWhere = strWhere.Replace("--", "");
				strWhere = strWhere.Replace(";", "");
				
				strOldWhere = " AND " + strWhere + " ";

				strWhere = " WHERE " + strWhere + " ";
			}

			// �������
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
				// �������
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
			else // �쳣����
			{
				throw new DataException("δָ���κ��������͡�0����1Ϊ����");
			}

			return sbSQL.ToString();
		}

		#endregion
        
		#region �ַ������봦��

		/// <summary>
		/// ���ַ���ת��Ϊ HTML ������ַ������Ա�ʵ�ִ� Web ���������ͻ��˵Ŀɿ��� HTTP ����
		/// </summary>
		/// <param name="str">ת���ַ���</param>
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
		/// ���Ѿ�Ϊ HTTP ������й� HTML ������ַ���ת��Ϊ�ѽ�����ַ�����
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
		/// �� URL �ַ���(��������)���б��룬�Ա�ʵ�ִ� Web ���������ͻ��˵Ŀɿ��� HTTP ���䡣
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
		/// ���Ѿ�Ϊ�� URL �д����������ַ���ת��Ϊ������ַ�����
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
        
		#region ���ַ����ļ���/����

		/// <summary>
		/// ���ַ���������Ӧ ServU �� MD5 ����
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
		/// ���ַ������м��ܣ������棩
		/// </summary>
		/// <param name="Password">Ҫ���ܵ��ַ���</param>
		/// <param name="Format">���ܷ�ʽ,0 is SHA1,1 is MD5</param>
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
		/// ���ַ������м���
		/// </summary>
		/// <param name="Passowrd">�����ܵ��ַ���</param>
		/// <returns>string</returns>
		public static string Encrypt(string Passowrd)
		{
			string strResult = "";

			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(Passowrd, true, 2);
			strResult = FormsAuthentication.Encrypt(ticket).ToString();
			
			return strResult;
		}


		/// <summary>
		/// ���ַ������н���
		/// </summary>
		/// <param name="Passowrd">�ѽ��ܵ��ַ���</param>
		/// <returns></returns>
		public static string Decrypt(string Passowrd)
		{
			string strResult = "";

			strResult = FormsAuthentication.Decrypt(Passowrd).Name.ToString();
			
			return strResult;
		}

		#endregion
        
		#region ��ȡ�����
        
		/// <summary>
		/// ��ȡָ�����ȵĴ�����������ִ�
		/// </summary>
		/// <param name="intLong">���ִ�����</param>
		/// <returns>�ַ���</returns>
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
		/// ��ȡһ����26��Сд��ĸ��ɵ�ָ�����ȵ��漴�ַ���
		/// </summary>
		/// <param name="intLong">ָ������</param>
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
		/// ��ȡһ�������ֺ�26��Сд��ĸ��ɵ�ָ�����ȵ��漴�ַ���
		/// </summary>
		/// <param name="intLong">ָ������</param>
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
        
		#region ��֤����

		/// <summary>
		/// �ж��ַ����Ƿ�Ϊ�ա�
		/// </summary>
		/// <param name="str">������ַ���</param>
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
		/// �ж��ַ����Ƿ�Ϊ��Ч���ʼ���ַ
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email, @"^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$");
		}


		/// <summary>
		/// �ж��ַ����Ƿ�Ϊ��Ч��URL��ַ
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static bool IsValidURL(string url)
		{
			return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
		}


		/// <summary>
		/// �ж��ַ����Ƿ�ΪInt���͵�
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool IsValidInt(string val)
		{
			return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
		}


		/// <summary>
		/// ����ַ����Ƿ�ȫΪ������
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsNum(string str)
		{
			bool blResult = true;//Ĭ��״̬��������

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
		/// ����ַ����Ƿ�ȫΪ������
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsDouble(string str)
		{
			bool blResult = true;//Ĭ��״̬��������

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
        /// ��֤�Ƿ��������ַ�����
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
        
		#region ����ת������

		/// <summary>
		/// ������ObjתΪInt32���͡�
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

        #region ƴ������ĸ

        /// <summary>
        /// ��ȡ�ַ���ƴ������ĸ
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
		/// ��ʽ�������ַ���
		/// </summary>
		/// <param name="str">�����ַ���</param>
		/// <param name="Type">0: ��������   1������ʱ��   2����������+ʱ��</param>
		/// <returns></returns>
		public static string FormatDateTimeString(string str,int Type)
		{
			try
			{
				if (Type == 0)
				{
					return Convert.ToDateTime(str).ToShortDateString();	//�������ַ���,�磺2007-1-1
				}
				else if( Type == 1)
				{
					return Convert.ToDateTime(str).ToShortTimeString();	//�������ַ���,�磺12:00
				}
				else if (Type == 2)
				{
					return Convert.ToDateTime(str).ToString();			//�磺2007-1-1 12��00
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
		/// �õ��ַ�����ǰintCount���ַ����ݡ�
		/// ��ʽ���ַ�������,����������ʾʡ�Ժţ��������ֺ��ָ���ĸ������2���ֽ�,��ĸ����һ���ֽ�
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
			if(System.Text.Encoding.Default.GetByteCount(str)<=n)//������ȱ���Ҫ�ĳ���nС'����ԭ�ַ���
			{
				return str;
			}
			else
			{
				int t=0;
				char[] q=str.ToCharArray();
				for(int i=0;i<q.Length&&t<n;i++)
				{
					if((int)q[i]>=0x4E00 && (int)q[i]<=0x9FA5)//�Ƿ���
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
		/// �������ַ���ת��Ϊ����
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
				//��ֹ����ĸ������
				return 0;
			}
		}
        
		/// <summary>
		/// �� Stream ת���� string
		/// </summary>
		/// <param name="s">Stream��</param>
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


			// �ͷ���Դ
			sr.Close();

			return strResult;
		}

		/// <summary>
		/// �Դ��ݵĲ����ַ������д�����ֹע��ʽ����
		/// </summary>
		/// <param name="str">���ݵĲ����ַ���</param>
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
		/// ��ʽ��ռ�ÿռ��С�����
		/// </summary>
		/// <param name="size">��С����λ�ֽ�</param>
		/// <returns>���� String</returns>
		public static string FormatNUM(long size)
		{
			decimal NUM;
			string strResult;

			if( size > 1073741824 )			//1G = 1024*1024*1024B  ��BתΪG
			{
				NUM = (Convert.ToDecimal(size)/Convert.ToDecimal(1073741824));
				strResult = NUM.ToString("N") + " G";
			}
			else if( size > 1048576 )		//1M = 1024*1024B	��BתΪM
			{
				NUM = (Convert.ToDecimal(size)/Convert.ToDecimal(1048576));
				strResult = NUM.ToString("N") + " M";
			}
			else if( size > 1024 )			//1K = 1024B  ��BתΪK
			{
				NUM = (Convert.ToDecimal(size)/Convert.ToDecimal(1024));
				strResult = NUM.ToString("N") + " KB";
			}
			else
			{
				strResult = size + " �ֽ�";
			}

			return strResult;
		}
        
		/// <summary>
		/// �����ͬһ�ַ���ɵ�ָ�����ȵ��ַ���
		/// </summary>
		/// <param name="Char">����ַ����磺A</param>
		/// <param name="i">ָ������</param>
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
		/// �����ַ�������ʵ���ȣ�һ�������ַ��൱��������λ����
		/// </summary>
		/// <param name="str">ָ���ַ���</param>
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
		/// ������Ϊ��׼���һ�����Ե�����
		/// </summary>
		/// <returns>���� String</returns>
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
		/// �����ַ�������ʵ���ȣ�һ�������ַ��൱��������λ����(ʹ��Encoding��)
		/// </summary>
		/// <param name="str">ָ���ַ���</param>
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
		/// �����ַ�����ʵ�ʳ��Ƚ�ȡָ�����ȵ��ַ���
		/// </summary>
		/// <param name="str">�ַ���</param>
		/// <param name="Length">ָ������</param>
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
		/// ���objֵ�Ƿ���Ч��Ϊ null �� "" ��Ϊ��Ч������false�����򷵻�true
		/// </summary>
		/// <param name="obj">Ҫ����ֵ</param>
		/// <returns></returns>
		public static bool CheckValiable(object obj)
		{
			if( Object.Equals(obj, null) || Object.Equals(obj, string.Empty) )
				return false;
			else
				return true;
		}

        /// <summary>
        /// ���˷Ƿ��ַ���
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
        /// ���˷Ƿ��ַ���
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
        /// ��ʽ��XML�ַ��������ã�
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
        /// ��ʱ���TimeStampת��ΪDateTime   
        /// </summary>   
        /// <param name="timeStamp">ʱ���</param>   
        /// <returns>��ʵʱ��</returns>   
        public static DateTime GetRealTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNowTime = new TimeSpan(lTime);
            return startTime.Add(toNowTime);
        }

        /// <summary>
        /// ��c# DateTimeʱ���ʽת��ΪUnixʱ�����ʽ
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>long</returns>
        public static long ConvertToTimeStamp(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000;            //��10000����Ϊ13λ
            return t;
        }
    }
	#endregion
}
