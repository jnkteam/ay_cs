using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace OriginalStudio.Lib.Utils
{
    public class Utils
    {
        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        private static Regex RegexBr = new Regex("(\\r\\n)", RegexOptions.IgnoreCase);

        public static Regex RegexFont = new Regex("<font color=\".*?\">([\\s\\S]+?)</font>", Utils.GetRegexCompiledOptions());

        private static string TemplateCookieName = string.Format("dnttemplateid_{0}_{1}_{2}", Utils.AssemblyFileVersion.FileMajorPart, Utils.AssemblyFileVersion.FileMinorPart, Utils.AssemblyFileVersion.FileBuildPart);

        public static string[] Monthes
        {
            get
            {
                return new string[]
				{
					"January",
					"February",
					"March",
					"April",
					"May",
					"June",
					"July",
					"August",
					"September",
					"October",
					"November",
					"December"
				};
            }
        }

        public static string AdDeTime(int times)
        {
            return DateTime.Now.AddMinutes((double)times).ToString();
        }

        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return Utils.BackupFile(sourceFileName, destFileName, true);
        }

        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            bool result;
            if (!overwrite && File.Exists(destFileName))
            {
                result = false;
            }
            else
            {
                bool flag;
                try
                {
                    File.Copy(sourceFileName, destFileName, true);
                    flag = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                result = flag;
            }
            return result;
        }

        public static string ChkSQL(string str)
        {
            string result;
            if (str == null)
            {
                result = "";
            }
            else
            {
                str = str.Replace("'", "''");
                result = str;
            }
            return result;
        }

        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), "[^\\w\\.@-]", "");
        }

        public static string ClearBR(string str)
        {
            Match match = Utils.RegexBr.Match(str);
            while (match.Success)
            {
                str = str.Replace(match.Groups[0].ToString(), "");
                match = match.NextMatch();
            }
            return str;
        }

        public static string ClearLastChar(string str)
        {
            string result;
            if (str == "")
            {
                result = "";
            }
            else
            {
                result = str.Substring(0, str.Length - 1);
            }
            return result;
        }

        public static bool CreateDir(string name)
        {
            return Utils.MakeSureDirectoryPathExists(name);
        }

        public static string CutString(string str, int startIndex)
        {
            return Utils.CutString(str, startIndex, str.Length);
        }

        public static string CutString(string str, int startIndex, int length)
        {
            string result;
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > str.Length)
                {
                    result = "";
                    return result;
                }
            }
            else
            {
                if (length < 0 || length + startIndex <= 0)
                {
                    result = "";
                    return result;
                }
                length += startIndex;
                startIndex = 0;
            }
            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }
            result = str.Substring(startIndex, length);
            return result;
        }

        public static string EncodeHtml(string strHtml)
        {
            string result;
            if (!(strHtml != ""))
            {
                result = "";
            }
            else
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                result = strHtml;
            }
            return result;
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static string[] FindNoUTF8File(string Path)
        {
            StringBuilder stringBuilder = new StringBuilder();
            FileInfo[] files = new DirectoryInfo(Path).GetFiles();
            for (int index = 0; index < files.Length; index++)
            {
                if (files[index].Extension.ToLower().Equals(".htm"))
                {
                    FileStream sbInputStream = new FileStream(files[index].FullName, FileMode.Open, FileAccess.Read);
                    bool flag = Utils.IsUTF8(sbInputStream);
                    sbInputStream.Close();
                    if (!flag)
                    {
                        stringBuilder.Append(files[index].FullName);
                        stringBuilder.Append("\r\n");
                    }
                }
            }
            return Utils.SplitString(stringBuilder.ToString(), "\r\n");
        }

        public static string FormatBytesStr(int bytes)
        {
            string result;
            if (bytes > 1073741824)
            {
                result = ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            else if (bytes > 1048576)
            {
                result = ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            else if (bytes > 1024)
            {
                result = ((double)(bytes / 1024)).ToString("0") + "K";
            }
            else
            {
                result = bytes.ToString() + "Bytes";
            }
            return result;
        }

        public static string FormatDate(DateTime dt)
        {
            return string.Concat(new object[]
			{
				(string)dt.Year.ToString(),
				"-",
				(string)dt.Month.ToString(),
				"-",
				(string)dt.Day.ToString()
			});
        }

        public static string FormatDateYearTwo(DateTime dt)
        {
            return string.Concat(new object[]
			{
				dt.Year.ToString().Substring(2),
				"-",
				(string)dt.Month.ToString(),
				"-",
				(string)dt.Day.ToString()
			});
        }

        public static string FormatYearMonth(DateTime dt)
        {
            return string.Concat(new object[]
			{
				dt.Year,
				"-",
				dt.Month,
				"-"
			});
        }

        public static string Get_Https(string a_strUrl, int timeout)
        {
            string result;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(a_strUrl);
                httpWebRequest.Timeout = timeout;
                StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.Default);
                StringBuilder stringBuilder = new StringBuilder();
                while (-1 != streamReader.Peek())
                {
                    stringBuilder.Append(streamReader.ReadLine());
                }
                result = stringBuilder.ToString();
            }
            catch (Exception ex_5D)
            {
                result = "true";
            }
            return result;
        }

        public static string GetAssemblyCopyright()
        {
            return Utils.AssemblyFileVersion.LegalCopyright;
        }

        public static string GetAssemblyProductName()
        {
            return Utils.AssemblyFileVersion.ProductName;
        }

        public static string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", Utils.AssemblyFileVersion.FileMajorPart, Utils.AssemblyFileVersion.FileMinorPart, Utils.AssemblyFileVersion.FileBuildPart);
        }

        public static string GetCookie(string strName)
        {
            string result;
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                result = HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            else
            {
                result = "";
            }
            return result;
        }

        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string GetDate(string datetimestr, string replacestr)
        {
            string result;
            if (datetimestr == null)
            {
                result = replacestr;
            }
            else if (datetimestr.Equals(""))
            {
                result = replacestr;
            }
            else
            {
                try
                {
                    datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
                }
                catch
                {
                    result = replacestr;
                    return result;
                }
                result = datetimestr;
            }
            return result;
        }

        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays((double)relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        public static string GetEmailHostName(string strEmail)
        {
            string result;
            if (strEmail.IndexOf("@") < 0)
            {
                result = "";
            }
            else
            {
                result = strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
            }
            return result;
        }

        public static string GetFilename(string url)
        {
            string result;
            if (url == null)
            {
                result = "";
            }
            else
            {
                string[] strArray = url.Split(new char[]
				{
					'/'
				});
                result = strArray[strArray.Length - 1].Split(new char[]
				{
					'?'
				})[0];
            }
            return result;
        }

        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return Utils.GetInArrayID(strSearch, stringArray, true);
        }

        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            int result;
            for (int index = 0; index < stringArray.Length; index++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[index].ToLower())
                    {
                        result = index;
                        return result;
                    }
                }
                else if (strSearch == stringArray[index])
                {
                    result = index;
                    return result;
                }
            }
            result = -1;
            return result;
        }

        public static string getIntZero(string j)
        {
            string result;
            if (j.Length < 2)
            {
                result = "0" + j;
            }
            else
            {
                result = j;
            }
            return result;
        }

        public static string GetMapPath(string strPath)
        {
            string result;
            if (HttpContext.Current != null)
            {
                result = HttpContext.Current.Server.MapPath(strPath);
            }
            else
            {
                result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
            return result;
        }

        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
        {
            return Utils.GetPageNumbers(curPage, countPage, url, extendPage, "page");
        }

        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag)
        {
            return Utils.GetPageNumbers(curPage, countPage, url, extendPage, pagetag, null);
        }

        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag, string anchor)
        {
            if (pagetag == "")
            {
                pagetag = "page";
            }
            int num = 1;
            url = ((url.IndexOf("?") <= 0) ? (url + "?") : (url + "&"));
            string str = string.Concat(new string[]
			{
				"<a href=\"",
				url,
				"&",
				pagetag,
				"=1"
			});
            string str2 = string.Concat(new object[]
			{
				"<a href=\"",
				url,
				"&",
				pagetag,
				"=",
				(string)countPage.ToString()
			});
            if (anchor != null)
            {
                str += anchor;
                str2 += anchor;
            }
            string str3 = str + "\">&laquo;</a>";
            string str4 = str2 + "\">&raquo;</a>";
            if (countPage < 1)
            {
                countPage = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            int num2;
            if (countPage > extendPage)
            {
                if (curPage - extendPage / 2 > 0)
                {
                    if (curPage + extendPage / 2 < countPage)
                    {
                        num = curPage - extendPage / 2;
                        num2 = num + extendPage - 1;
                    }
                    else
                    {
                        num2 = countPage;
                        num = num2 - extendPage + 1;
                        str4 = "";
                    }
                }
                else
                {
                    num2 = extendPage;
                    str3 = "";
                }
            }
            else
            {
                num = 1;
                num2 = countPage;
                str3 = "";
                str4 = "";
            }
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append(str3);
            for (int index = num; index <= num2; index++)
            {
                if (index == curPage)
                {
                    stringBuilder.Append("<span>");
                    stringBuilder.Append(index);
                    stringBuilder.Append("</span>");
                }
                else
                {
                    stringBuilder.Append("<a href=\"");
                    stringBuilder.Append(url);
                    stringBuilder.Append(pagetag);
                    stringBuilder.Append("=");
                    stringBuilder.Append(index);
                    if (anchor != null)
                    {
                        stringBuilder.Append(anchor);
                    }
                    stringBuilder.Append("\">");
                    stringBuilder.Append(index);
                    stringBuilder.Append("</a>");
                }
            }
            stringBuilder.Append(str4);
            return stringBuilder.ToString();
        }

        public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
        {
            int num = 1;
            int num2 = 1;
            string str = string.Concat(new string[]
			{
				"<a href=\"",
				url,
				"-1",
				expname,
				"\">&laquo;</a>"
			});
            string str2 = string.Concat(new object[]
			{
				"<a href=\"",
				url,
				"-",
				(string)countPage.ToString(),
				expname,
				"\">&raquo;</a>"
			});
            if (countPage < 1)
            {
                countPage = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            int num3;
            if (countPage > extendPage)
            {
                if (num2 - extendPage / 2 > 0)
                {
                    if (num2 + extendPage / 2 < countPage)
                    {
                        num = num2 - extendPage / 2;
                        num3 = num + extendPage - 1;
                    }
                    else
                    {
                        num3 = countPage;
                        num = num3 - extendPage + 1;
                        str2 = "";
                    }
                }
                else
                {
                    num3 = extendPage;
                    str = "";
                }
            }
            else
            {
                num = 1;
                num3 = countPage;
                str = "";
                str2 = "";
            }
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append(str);
            for (int index = num; index <= num3; index++)
            {
                stringBuilder.Append("<a href=\"");
                stringBuilder.Append(url);
                stringBuilder.Append("-");
                stringBuilder.Append(index);
                stringBuilder.Append(expname);
                stringBuilder.Append("\">");
                stringBuilder.Append(index);
                stringBuilder.Append("</a>");
            }
            stringBuilder.Append(str2);
            return stringBuilder.ToString();
        }

        public static RegexOptions GetRegexCompiledOptions()
        {
            return RegexOptions.None;
        }

        public static string GetStandardDateTime(string fDateTime)
        {
            return Utils.GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            string result;
            if (fDateTime == "0000-0-0 0:00:00")
            {
                result = fDateTime;
            }
            else
            {
                result = Convert.ToDateTime(fDateTime).ToString(formatStr);
            }
            return result;
        }

        public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
        {
            int num = 1;
            string str = string.Concat(new string[]
			{
				"<a href=\"",
				url,
				"-1",
				expname,
				"\">&laquo;</a>"
			});
            string str2 = string.Concat(new object[]
			{
				"<a href=\"",
				url,
				"-",
				(string)countPage.ToString(),
				expname,
				"\">&raquo;</a>"
			});
            if (countPage < 1)
            {
                countPage = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            int num2;
            if (countPage > extendPage)
            {
                if (curPage - extendPage / 2 > 0)
                {
                    if (curPage + extendPage / 2 < countPage)
                    {
                        num = curPage - extendPage / 2;
                        num2 = num + extendPage - 1;
                    }
                    else
                    {
                        num2 = countPage;
                        num = num2 - extendPage + 1;
                        str2 = "";
                    }
                }
                else
                {
                    num2 = extendPage;
                    str = "";
                }
            }
            else
            {
                num = 1;
                num2 = countPage;
                str = "";
                str2 = "";
            }
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append(str);
            for (int index = num; index <= num2; index++)
            {
                if (index == curPage)
                {
                    stringBuilder.Append("<span>");
                    stringBuilder.Append(index);
                    stringBuilder.Append("</span>");
                }
                else
                {
                    stringBuilder.Append("<a href=\"");
                    stringBuilder.Append(url);
                    stringBuilder.Append("-");
                    stringBuilder.Append(index);
                    stringBuilder.Append(expname);
                    stringBuilder.Append("\">");
                    stringBuilder.Append(index);
                    stringBuilder.Append("</a>");
                }
            }
            stringBuilder.Append(str2);
            return stringBuilder.ToString();
        }

        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return Utils.GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string result;
            if (Regex.IsMatch(p_SrcString, "[ࠀ-一]+") || Regex.IsMatch(p_SrcString, "[가-힣]+"))
            {
                if (p_StartIndex >= p_SrcString.Length)
                {
                    result = "";
                }
                else
                {
                    result = p_SrcString.Substring(p_StartIndex, (p_Length + p_StartIndex > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }
            else if (p_Length < 0)
            {
                result = p_SrcString;
            }
            else
            {
                byte[] bytes = Encoding.Default.GetBytes(p_SrcString);
                if (bytes.Length <= p_StartIndex)
                {
                    result = p_SrcString;
                }
                else
                {
                    int num = bytes.Length;
                    if (bytes.Length > p_StartIndex + p_Length)
                    {
                        num = p_Length + p_StartIndex;
                    }
                    else
                    {
                        p_Length = bytes.Length - p_StartIndex;
                        p_TailString = "";
                    }
                    int length = p_Length;
                    int[] numArray = new int[p_Length];
                    int num2 = 0;
                    for (int index = p_StartIndex; index < num; index++)
                    {
                        if (bytes[index] > 127)
                        {
                            num2++;
                            if (num2 == 3)
                            {
                                num2 = 1;
                            }
                        }
                        else
                        {
                            num2 = 0;
                        }
                        numArray[index] = num2;
                    }
                    if (bytes[num - 1] > 127 && numArray[p_Length - 1] == 1)
                    {
                        length = p_Length + 1;
                    }
                    byte[] bytes2 = new byte[length];
                    Array.Copy(bytes, p_StartIndex, bytes2, 0, length);
                    result = Encoding.Default.GetString(bytes2) + p_TailString;
                }
            }
            return result;
        }

        public static string GetTemplateCookieName()
        {
            return Utils.TemplateCookieName;
        }

        public static string GetTextFromHTML(string HTML)
        {
            return new Regex("</?(?!br|/?p|img)[^>]*>", RegexOptions.IgnoreCase).Replace(HTML, "");
        }

        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        public static string GetTrueForumPath()
        {
            string path = HttpContext.Current.Request.Path;
            string result;
            if (path.LastIndexOf("/") != path.IndexOf("/"))
            {
                result = path.Substring(path.IndexOf("/"), path.LastIndexOf("/") + 1);
            }
            else
            {
                result = "/";
            }
            return result;
        }

        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static bool InArray(string str, string stringarray)
        {
            return Utils.InArray(str, Utils.SplitString(stringarray, ","), false);
        }

        public static bool InArray(string str, string[] stringarray)
        {
            return Utils.InArray(str, stringarray, false);
        }

        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return Utils.InArray(str, Utils.SplitString(stringarray, strsplit), false);
        }

        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return Utils.GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return Utils.InArray(str, Utils.SplitString(stringarray, strsplit), caseInsensetive);
        }

        public static bool InIPArray(string ip, string[] iparray)
        {
            string[] strArray = Utils.SplitString(ip, ".");
            int index = 0;
            bool result;
            while (index < iparray.Length)
            {
                string[] strArray2 = Utils.SplitString(iparray[index], ".");
                int num = 0;
                for (int index2 = 0; index2 < strArray2.Length; index2++)
                {
                    if (strArray2[index2] == "*")
                    {
                        result = true;
                        return result;
                    }
                    if (strArray.Length <= index2 || !(strArray2[index2] == strArray[index2]))
                    {
                        break;
                    }
                    num++;
                }
                if (num != 4)
                {
                    index++;
                    continue;
                }
                result = true;
                return result;
            }
            result = false;
            return result;
        }

        public static string IntToStr(int intValue)
        {
            return Convert.ToString(intValue);
        }

        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, "[A-Za-z0-9\\+\\/\\=]");
        }

        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            bool result;
            if (stringarray != "" && stringarray != null)
            {
                str = str.ToLower();
                string[] array = Utils.SplitString(stringarray.ToLower(), strsplit);
                for (int i = 0; i < array.Length; i++)
                {
                    string str2 = array[i];
                    if (str.IndexOf(str2) > -1)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }

        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, "(\\d{4})-(\\d{1,2})-(\\d{1,2})");
        }

        public static bool IsDouble(object Expression)
        {
            return TypeParse.IsDouble(Expression);
        }

        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            bool result;
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                result = false;
            }
            else
            {
                string str = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
                result = (str == "jpg" || str == "jpeg" || str == "png" || str == "bmp" || str == "gif");
            }
            return result;
        }

        public static bool IsInt(string str)
        {
            return Regex.IsMatch(str, "^[0-9]*$");
        }

        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }

        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){2}((2[0-4]\\d|25[0-5]|[01]?\\d\\d?|\\*)\\.)(2[0-4]\\d|25[0-5]|[01]?\\d\\d?|\\*)$");
        }

        public static bool IsNumberId(string _value)
        {
            return Utils.QuickValidate("^[1-9]*[0-9]*$", _value);
        }

        public static bool IsNumeric(object Expression)
        {
            return TypeParse.IsNumeric(Expression);
        }

        public static bool IsNumericArray(string[] strNumber)
        {
            return TypeParse.IsNumericArray(strNumber);
        }

        public static bool IsRuleTip(Hashtable NewHash, string ruletype, out string key)
        {
            key = "";
            bool result;
            foreach (DictionaryEntry entry in NewHash)
            {
                try
                {
                    string[] strArray = Utils.SplitString(entry.Value.ToString(), "\r\n");
                    string[] array = strArray;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string str = array[i];
                        if (str != "")
                        {
                            string text = ruletype.Trim().ToLower();
                            if (text != null)
                            {
                                if (!(text == "email"))
                                {
                                    if (!(text == "ip"))
                                    {
                                        if (text == "timesect")
                                        {
                                            string[] strArray2 = str.Split(new char[]
											{
												'-'
											});
                                            if (!Utils.IsTime(strArray2[1].ToString()) || !Utils.IsTime(strArray2[0].ToString()))
                                            {
                                                throw new Exception();
                                            }
                                        }
                                    }
                                    else if (!Utils.IsIPSect(str.ToString()))
                                    {
                                        throw new Exception();
                                    }
                                }
                                else if (!Utils.IsValidDoEmail(str.ToString()))
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                catch
                {
                    key = entry.Key.ToString();
                    result = false;
                    return result;
                }
            }
            result = true;
            return result;
        }

        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, "[-|;|,|\\/|\\(|\\)|\\[|\\]|\\}|\\{|%|@|\\*|!|\\']");
        }

        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, "^\\s*$|^c:\\\\con\\\\con$|[%,\\*\"\\s\\t\\<\\>\\&]|游客|^Guest");
        }

        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, "^(http|https)\\://([a-zA-Z0-9\\.\\-]+(\\:[a-zA-Z0-9\\.&%\\$\\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\\-]+\\.)*[a-zA-Z0-9\\-]+\\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\\:[0-9]+)*(/($|[a-zA-Z0-9\\.\\,\\?\\'\\\\\\+&%\\$#\\=~_\\-]+))*$");
        }

        private static bool IsUTF8(FileStream sbInputStream)
        {
            bool flag = true;
            long length = sbInputStream.Length;
            byte num = 0;
            int index = 0;
            bool result;
            while ((long)index < length)
            {
                byte num2 = (byte)sbInputStream.ReadByte();
                if ((num2 & 128) != 0)
                {
                    flag = false;
                }
                if (num == 0)
                {
                    if (num2 >= 128)
                    {
                        do
                        {
                            num2 = (byte)(num2 << 1);
                            num += 1;
                        }
                        while ((num2 & 128) != 0);
                        num -= 1;
                        if (num == 0)
                        {
                            result = false;
                            return result;
                        }
                    }
                }
                else
                {
                    if ((num2 & 192) != 128)
                    {
                        result = false;
                        return result;
                    }
                    num -= 1;
                }
                index++;
            }
            result = (num <= 0 && !flag);
            return result;
        }

        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, "^@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }

        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }

        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);

        public static string mashSQL(string str)
        {
            string result;
            if (str == null)
            {
                result = "";
            }
            else
            {
                str = str.Replace("'", "'");
                result = str;
            }
            return result;
        }

        public static string MD5(string str)
        {
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(str));
            string str2 = "";
            for (int index = 0; index < hash.Length; index++)
            {
                str2 += hash[index].ToString("x").PadLeft(2, '0');
            }
            return str2;
        }

        public static bool QuickValidate(string _express, string _value)
        {
            bool result;
            if (_value == null)
            {
                result = false;
            }
            else
            {
                Regex regex = new Regex(_express);
                result = (_value.Length != 0 && regex.IsMatch(_value));
            }
            return result;
        }

        public static string RemoveFontTag(string title)
        {
            Match match = Utils.RegexFont.Match(title);
            string result;
            if (match.Success)
            {
                result = match.Groups[1].Value;
            }
            else
            {
                result = title;
            }
            return result;
        }

        public static string RemoveHtml(string content)
        {
            string pattern = "<[^>]*>";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, "(\\<|\\s+)o([a-z]+\\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "(script|frame|form|meta|behavior|style)([\\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        public static string ReplaceStrToScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            return str;
        }

        public static void ResponseFile(string filepath, string filename, string filetype)
        {
            Stream stream = null;
            byte[] buffer = new byte[10000];
            try
            {
                stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                long num = stream.Length;
                HttpContext.Current.Response.ContentType = filetype;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Utils.UrlEncode(filename.Trim()).Replace("+", " "));
                while (num > 0L)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int count = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, count);
                        HttpContext.Current.Response.Flush();
                        buffer = new byte[10000];
                        num -= (long)count;
                    }
                    else
                    {
                        num = -1L;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return Utils.RestoreFile(backupFileName, targetFileName, null);
        }

        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    File.Copy(targetFileName, backupTargetFileName, true);
                }
                File.Delete(targetFileName);
                File.Copy(backupFileName, targetFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static string RTrim(string str)
        {
            string result;
            try
            {
                for (int length = str.Length; length >= 0; length--)
                {
                    char ch = str[length];
                    if (!ch.Equals(" "))
                    {
                        ch = str[length];
                    }
                    if (ch.Equals("\r") || str[length].Equals("\n"))
                    {
                        str.Remove(length, 1);
                    }
                }
                result = str;
            }
            catch (Exception ex_7E)
            {
                result = str;
            }
            return result;
        }

        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] chars = SBCCase.ToCharArray();
            for (int index = 0; index < chars.Length; index++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(chars, index, 1);
                if (bytes.Length == 2 && bytes[1] == 255)
                {
                    bytes[0] = (byte)(bytes[0] + 32);
                    bytes[1] = 0;
                    chars[index] = Encoding.Unicode.GetChars(bytes)[0];
                }
            }
            return new string(chars);
        }

        public static string SHA256(string str)
        {
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(str)));
        }

        public static string Spaces(int nSpaces)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < nSpaces; index++)
            {
                stringBuilder.Append(" &nbsp;&nbsp;");
            }
            return stringBuilder.ToString();
        }

        public static string[] SplitString(string strContent, string strSplit)
        {
            string[] result;
            if (string.IsNullOrEmpty(strContent))
            {
                result = new string[0];
            }
            else if (strContent.IndexOf(strSplit) >= 0)
            {
                result = Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                result = new string[]
				{
					strContent
				};
            }
            return result;
        }

        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] strArray = new string[p_3];
            string[] strArray2 = Utils.SplitString(strContent, strSplit);
            for (int index = 0; index < p_3; index++)
            {
                strArray[index] = ((index >= strArray2.Length) ? string.Empty : strArray2[index]);
            }
            return strArray;
        }

        public static int StrDateDiffHours(string time, int hours)
        {
            int result;
            if (time == "" || time == null)
            {
                result = 1;
            }
            else
            {
                TimeSpan timeSpan = DateTime.Now - DateTime.Parse(time).AddHours((double)hours);
                if (timeSpan.TotalHours > 2147483647.0)
                {
                    result = 2147483647;
                }
                else if (timeSpan.TotalHours < -2147483648.0)
                {
                    result = -2147483648;
                }
                else
                {
                    result = (int)timeSpan.TotalHours;
                }
            }
            return result;
        }

        public static int StrDateDiffMinutes(string time, int minutes)
        {
            int result;
            if (time == "" || time == null)
            {
                result = 1;
            }
            else
            {
                TimeSpan timeSpan = DateTime.Now - DateTime.Parse(time).AddMinutes((double)minutes);
                if (timeSpan.TotalMinutes > 2147483647.0)
                {
                    result = 2147483647;
                }
                else if (timeSpan.TotalMinutes < -2147483648.0)
                {
                    result = -2147483648;
                }
                else
                {
                    result = (int)timeSpan.TotalMinutes;
                }
            }
            return result;
        }

        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan timeSpan = DateTime.Now - DateTime.Parse(Time).AddSeconds((double)Sec);
            int result;
            if (timeSpan.TotalSeconds > 2147483647.0)
            {
                result = 2147483647;
            }
            else if (timeSpan.TotalSeconds < -2147483648.0)
            {
                result = -2147483648;
            }
            else
            {
                result = (int)timeSpan.TotalSeconds;
            }
            return result;
        }

        public static string StrFilter(string str, string bantext)
        {
            string[] strArray = Utils.SplitString(bantext, "\r\n");
            for (int index = 0; index < strArray.Length; index++)
            {
                string oldValue = strArray[index].Substring(0, strArray[index].IndexOf("="));
                string newValue = strArray[index].Substring(strArray[index].IndexOf("=") + 1);
                str = str.Replace(oldValue, newValue);
            }
            return str;
        }

        public static string StrFormat(string str)
        {
            string result;
            if (str == null)
            {
                result = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                result = str;
            }
            return result;
        }

        public static bool StrToBool(object expression, bool defValue = false)
        {
            return TypeParse.StrToBool(expression, defValue);
        }

        public static float StrToFloat(object strValue, float defValue = 0)
        {
            return TypeParse.StrToFloat(strValue, defValue);
        }

        public static decimal StrToDecimal(object strValue, float defValue = 0)
        {
            return Convert.ToDecimal(TypeParse.StrToFloat(strValue, defValue));
        }

        public static int StrToInt(object expression, int defValue = 0)
        {
            return TypeParse.StrToInt(expression, defValue);
        }

        public static long StrToLong(object expression, int defValue = 0)
        {
            return TypeParse.StrToInt(expression, defValue);
        }

        public static DateTime StrToDateTime(object expression)
        {
            return TypeParse.StrToDateTime(expression, DateTime.Now);
        }

        public static Color ToColor(string color)
        {
            color = color.TrimStart(new char[]
			{
				'#'
			});
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            int length = color.Length;
            Color result;
            if (length != 3)
            {
                if (length != 6)
                {
                    result = Color.FromName(color);
                }
                else
                {
                    char[] chArray2 = color.ToCharArray();
                    result = Color.FromArgb(Convert.ToInt32(chArray2[0].ToString() + chArray2[1].ToString(), 16), Convert.ToInt32(chArray2[2].ToString() + chArray2[3].ToString(), 16), Convert.ToInt32(chArray2[4].ToString() + chArray2[5].ToString(), 16));
                }
            }
            else
            {
                char[] chArray3 = color.ToCharArray();
                result = Color.FromArgb(Convert.ToInt32(chArray3[0].ToString() + chArray3[0].ToString(), 16), Convert.ToInt32(chArray3[1].ToString() + chArray3[1].ToString(), 16), Convert.ToInt32(chArray3[2].ToString() + chArray3[2].ToString(), 16));
            }
            return result;
        }

        public void transHtml(string path, string outpath)
        {
            Page page = new Page();
            StringWriter stringWriter = new StringWriter();
            page.Server.Execute(path, stringWriter);
            FileStream fileStream;
            if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outpath);
                fileStream = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            else
            {
                fileStream = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            byte[] bytes = Encoding.Default.GetBytes(stringWriter.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        private static string Unicode2UnitCharacter(string str)
        {
            string result;
            if (str.Length != 4)
            {
                result = str;
            }
            else
            {
                try
                {
                    byte num = Convert.ToByte(str.Substring(0, 2), 16);
                    result = Encoding.Unicode.GetString(new byte[]
					{
						Convert.ToByte(str.Substring(2), 16),
						num
					});
                }
                catch (Exception ex_51)
                {
                    result = str;
                }
            }
            return result;
        }

        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes((double)expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
    }
}
