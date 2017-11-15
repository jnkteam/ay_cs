namespace KuaiCard.BLL.Tools
{
    using DBAccess;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class db
    {
        public static bool Backup(string path)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { DataBase.MakeInParam("@datapath", SqlDbType.VarChar, 200, path) };
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_database_backup", commandParameters);
                return true;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

