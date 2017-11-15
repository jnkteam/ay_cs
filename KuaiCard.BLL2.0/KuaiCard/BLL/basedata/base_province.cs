namespace KuaiCard.BLL.basedata
{
    using DBAccess;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Text;

    public class base_province
    {
        public static DataSet GetList(string strWhere)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select ProvinceID,ProvinceName,DateCreated,DateUpdated ");
                builder.Append(" FROM base_province ");
                if (strWhere.Trim() != "")
                {
                    builder.Append(" where " + strWhere);
                }
                return DbHelperSQL.Query(builder.ToString());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }
    }
}

