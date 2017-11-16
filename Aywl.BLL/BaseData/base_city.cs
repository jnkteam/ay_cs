namespace OriginalStudio.BLL.BaseData
{
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
            return OriginalStudio.DBAccess.DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }
    }
}

