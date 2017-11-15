namespace KuaiCard.BLL.Channel
{
    using DBAccess;
    using KuaiCard.Model.Channel;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class CodeMappingFactory
    {
        public static int Add(CodeMappingInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@pmodeCode", SqlDbType.VarChar, 20), new SqlParameter("@suppId", SqlDbType.Int, 10), new SqlParameter("@suppCode", SqlDbType.VarChar, 20) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.pmodeCode;
                commandParameters[2].Value = model.suppId;
                commandParameters[3].Value = model.suppCode;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_codemapping_ADD", commandParameters);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_codemapping_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select [id]\r\n      ,[pmodeCode]\r\n      ,[suppId]\r\n      ,[suppCode]\r\n      ,[SuppName]\r\n      ,[modeName] ");
            builder.Append(" FROM V_Codemapping ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }

        public static CodeMappingInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                CodeMappingInfo info = new CodeMappingInfo();
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_codemapping_GetModel", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                {
                    if (set.Tables[0].Rows[0]["id"].ToString() != "")
                    {
                        info.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                    }
                    info.pmodeCode = set.Tables[0].Rows[0]["pmodeCode"].ToString();
                    if (set.Tables[0].Rows[0]["suppId"].ToString() != "")
                    {
                        info.suppId = int.Parse(set.Tables[0].Rows[0]["suppId"].ToString());
                    }
                    info.suppCode = set.Tables[0].Rows[0]["suppCode"].ToString();
                    return info;
                }
                return null;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static bool Update(CodeMappingInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@pmodeCode", SqlDbType.VarChar, 20), new SqlParameter("@suppId", SqlDbType.Int, 10), new SqlParameter("@suppCode", SqlDbType.VarChar, 20) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.pmodeCode;
                commandParameters[2].Value = model.suppId;
                commandParameters[3].Value = model.suppCode;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_codemapping_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

