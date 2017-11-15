namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class SinglePageFactory
    {
        public static int ADD(SinglePage _singlepage)
        {
            SqlParameter parameter = DataBase.MakeOutParam("@Sid", SqlDbType.Int, 10);
            SqlParameter[] commandParameters = new SqlParameter[] { parameter, DataBase.MakeInParam("@Title", SqlDbType.VarChar, 200, _singlepage.Title), DataBase.MakeInParam("@Content", SqlDbType.NText, 0x1f40, _singlepage.Content), DataBase.MakeInParam("@Addtime", SqlDbType.DateTime, 8, _singlepage.Addtime), DataBase.MakeInParam("@interface1", SqlDbType.VarChar, 200, _singlepage.Interface1), DataBase.MakeInParam("@Interface2", SqlDbType.VarChar, 200, _singlepage.Interface2) };
            if (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "UP_SinglePage_ADD", commandParameters) == 1)
            {
                _singlepage.Sid = (int) parameter.Value;
                return _singlepage.Sid;
            }
            return 0;
        }

        public static SinglePage Get(int id)
        {
            SinglePage page = new SinglePage();
            string commandText = "SELECT * FROM [SinglePage] WHERE [SID]=" + id;
            SqlDataReader reader = DataBase.ExecuteReader(CommandType.Text, commandText);
            if (reader.Read())
            {
                page.Sid = (int) reader["Sid"];
                page.Title = reader["Title"].ToString();
                page.Content = reader["Content"].ToString();
                page.Addtime = (DateTime) reader["addtime"];
                page.Interface1 = reader["interface1"].ToString();
                page.Interface2 = reader["interface2"].ToString();
            }
            reader.Close();
            return page;
        }

        public static bool Update(SinglePage _singlepage)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@Sid", SqlDbType.Int, 10, _singlepage.Sid), DataBase.MakeInParam("@Title", SqlDbType.VarChar, 200, _singlepage.Title), DataBase.MakeInParam("@Content", SqlDbType.NText, 0x1f40, _singlepage.Content), DataBase.MakeInParam("@Addtime", SqlDbType.DateTime, 8, _singlepage.Addtime), DataBase.MakeInParam("@interface1", SqlDbType.VarChar, 200, _singlepage.Interface1), DataBase.MakeInParam("@Interface2", SqlDbType.VarChar, 200, _singlepage.Interface2) };
            return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "UP_SinglePage_Update", commandParameters) == 1);
        }
    }
}

