namespace KuaiCard.BLL.basedata
{
    using DBAccess;
    using System;
    using System.Data;
    using System.Text;

    public class base_city
    {
        public static DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select CityID,CityName,ZipCode,ProvinceID,DateCreated,DateUpdated ");
            builder.Append(" FROM base_city ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(builder.ToString());
        }
    }
}

