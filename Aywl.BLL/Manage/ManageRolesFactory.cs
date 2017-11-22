namespace OriginalStudio.BLL
{
    using DBAccess;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    /// <summary>
    /// 角色管理
    /// </summary>
    public class ManageRolesFactory
    {


        internal static string SQL_TABLE = "manageRoles";
        internal static string SQL_TABLE_FIELD = "[id],[module],[type],[title],[description],[status],[rules]";
        public static bool Delete(int id)
        {
            return false;
        }

        public static DataSet GetList(string where)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select "+ SQL_TABLE_FIELD);
            builder.Append(" FROM  "+ SQL_TABLE);
            if (!string.IsNullOrEmpty(where))
            {
                builder.AppendFormat(" where {0}", where);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

       

      
    }
}

