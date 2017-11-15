namespace DBAccess
{
    using System;
    using System.Web;
    using System.Data;
    using System.Text;

    public class SqlHelper
    {
        public static string CleanString(string inputString, int length)
        {
            return CleanString(inputString, length, false);
        }

        public static string CleanString(string inputString, int length, bool cleanSpecial)
        {
            string str = ((length <= 0) || (length >= HttpUtility.HtmlEncode(inputString).Length)) ? HttpUtility.HtmlEncode(inputString) : HttpUtility.HtmlEncode(inputString).Substring(0, length);
            if (cleanSpecial)
            {
                str = str.Replace("'", "''");
            }
            return str;
        }

        public static string GetCountSQL(string tables, string wheres, string distinctField)
        {
            return string.Format("\r\nSELECT \r\n\tCOUNT({0}) \r\nFROM \r\n\t{1} with(nolock)\r\nWHERE\r\n\t{2}", (distinctField.Length > 0) ? ("DISTINCT " + distinctField) : "0", tables, wheres);
        }

        public static string GetPageSelectSQL(string columns, string tables, string wheres, string orders, string key, int pageSize, int pageNum, bool isDistinct)
        {
            int count = pageSize;
            int startIndex = (pageNum - 1) * pageSize;
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            return GetSelectSQL(columns, tables, wheres, orders, key, startIndex, count, isDistinct);
        }

        public static string GetSelectSQL(string columns, string tables, string wheres, string orders, string key, bool isDistinct)
        {
            return GetSelectSQL(columns, tables, wheres, orders, key, 0, 0, isDistinct);
        }

        public static string GetSelectSQL(string columns, string tables, string wheres, string orders, string key, int startIndex, bool isDistinct)
        {
            return GetSelectSQL(columns, tables, wheres, orders, key, startIndex, 0, isDistinct);
        }

        public static string GetSelectSQL(string columns, string tables, string wheres, string orders, string key, int startIndex, int count, bool isDistinct)
        {
            if (startIndex > 1)
            {
                return string.Format("\r\nSELECT {5} {7:d} \r\n\t{0}\r\nFROM \r\n\t{1} with(nolock) \r\nWHERE \r\n\t{2} \r\nAND \r\n\t{4} NOT IN (\r\n\t\tSELECT {5} TOP {6:d}\r\n\t\t\t{4}\r\n\t\tFROM\r\n\t\t\t{1} with(nolock)\r\n\t\tWHERE \r\n\t\t\t{2} \r\n\t\tORDER BY {3}\r\n\t)\r\nORDER BY {3}", new object[] { columns, tables, wheres, orders, key, isDistinct ? "DISTINCT" : string.Empty, startIndex, (count > 0) ? string.Format("TOP {0:d}", count) : string.Empty });
            }
            return string.Format("\r\nSELECT {4} {5}\r\n\t{0} \r\nFROM \r\n\t{1} with(nolock) \r\nWHERE \r\n\t{2} \r\nORDER BY {3}", new object[] { columns, tables, wheres, orders, isDistinct ? "DISTINCT" : "", (count > 0) ? string.Format("TOP {0:d}", count) : string.Empty });
        }

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
        public static string ConstructSplitSQL(string tblName,
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
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " [" + rtnFields + "] FROM [" + tblName + "] ");

                    sbSQL.Append(strWhere + "ORDER BY [" + fldName + "] ASC");
                }
                else
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " [" + rtnFields + "] FROM [" + tblName + "] ");

                    sbSQL.Append("WHERE ([" + fldName + "] > ( SELECT MAX([" + fldName + "]) FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " [" + fldName + "] FROM [" + tblName + "]" + strWhere + " ORDER BY [" + fldName + "] ASC ) AS T )) ");

                    sbSQL.Append(strOldWhere + "ORDER BY [" + fldName + "] ASC");
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

                    sbSQL.Append(strWhere + "ORDER BY " + fldName + " DESC");
                }
                else
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

                    sbSQL.Append("WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ");

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
        public static string ConstructSplitSQL(string tblName,
            string fldName,
            int PageIndex,
            int PageSize,
            string rtnFields,
            int OrderType,
            string strWhere)
        {
            StringBuilder sbSQL = new StringBuilder();
            string strOldWhere = " ";			//Where条件

            //构造检索条件语句的字符串
            if (strWhere != "")
            {
                //去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(":", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            //升序操作	ASC
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

                    sbSQL.Append(strWhere + "ORDER BY " + fldName + " ASC");
                }
                else
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

                    sbSQL.Append("WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ");

                    sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " ASC");
                }
            }
            //降序操作	DESC
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");
                    sbSQL.Append(strWhere + "ORDER BY " + fldName + " DESC");
                }
                else
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ");

                    sbSQL.Append("WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ");

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
        public static string ConstructSplitSQL(string tblName,
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
            if (strWhere != "")
            {
                // 去除不合法的字符，防止SQL注入式攻击
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // 升序操作
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + unionCondition + " ");

                    sbSQL.Append(strWhere + "ORDER BY " + fldName + " ASC");
                }
                else
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + unionCondition + " ");

                    sbSQL.Append("WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ");

                    sbSQL.Append(strOldWhere + "ORDER BY " + fldName + " ASC");
                }
            }
            // 降序操作
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ");

                    sbSQL.Append(strWhere + "ORDER BY " + fldName + " DESC");
                }
                else
                {
                    sbSQL.Append("SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ");

                    sbSQL.Append("WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ");

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
   
    }
}

