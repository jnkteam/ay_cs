namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class TempLabelFactory
    {
        public static int ADD(TempLabel _templabel)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@Id", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@Title", SqlDbType.VarChar, 200, _templabel.Title), DataBase.MakeInParam("@Content", SqlDbType.NText, 500, _templabel.Content), DataBase.MakeInParam("@info", SqlDbType.VarChar, 200, _templabel.Info), DataBase.MakeInParam("@TemplateId", SqlDbType.VarChar, 20, _templabel.TemplateId), DataBase.MakeInParam("@sort", SqlDbType.Int, 10, _templabel.Sort), DataBase.MakeInParam("@source", SqlDbType.VarChar, 200, _templabel.Source) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "UP_TempLabel_ADD", commandParameters) == 1)
            {
                _templabel.ID = (int) parameter.Value;
                return _templabel.ID;
            }
            return 0;
        }

        public static TempLabel Get(int id)
        {
            TempLabel label = new TempLabel();
            string commandText = "SELECT * FROM [TempLabel] WHERE [ID]=" + id;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                label.ID = (int) reader["id"];
                label.Title = reader["Title"].ToString();
                label.Content = reader["Content"].ToString();
                label.Info = reader["Info"].ToString();
                label.TemplateId = reader["TemplateId"].ToString();
                label.Sort = int.Parse(reader["Sort"].ToString());
                label.Source = reader["Source"].ToString();
            }
            reader.Close();
            return label;
        }

        public static bool Update(TempLabel _templabel)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@id", SqlDbType.Int, 10, _templabel.ID), DataBase.MakeInParam("@Title", SqlDbType.VarChar, 200, _templabel.Title), DataBase.MakeInParam("@Content", SqlDbType.NText, 500, _templabel.Content), DataBase.MakeInParam("@info", SqlDbType.VarChar, 8, _templabel.Info), DataBase.MakeInParam("@TemplateId", SqlDbType.VarChar, 200, _templabel.TemplateId), DataBase.MakeInParam("@sort", SqlDbType.Int, 10, _templabel.Sort), DataBase.MakeInParam("@source", SqlDbType.VarChar, 200, _templabel.Source) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "UP_TempLabel_Update", commandParameters) == 1);
        }
    }
}

