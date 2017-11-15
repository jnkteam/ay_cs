namespace KuaiCard.BLL.User
{
    using DBAccess;
    using KuaiCard.Model.User;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class FindPwdFactory
    {
        public static int Add(FindPwd model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@uid", SqlDbType.Int, 10), new SqlParameter("@username", SqlDbType.VarChar, 50), new SqlParameter("@oldpwd", SqlDbType.VarChar, 100), new SqlParameter("@newpwd", SqlDbType.VarChar, 100), new SqlParameter("@status", SqlDbType.Int, 10), new SqlParameter("@addtimer", SqlDbType.DateTime) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.uid;
                commandParameters[2].Value = model.username;
                commandParameters[3].Value = model.oldpwd;
                commandParameters[4].Value = model.newpwd;
                commandParameters[5].Value = model.status;
                commandParameters[6].Value = model.addtimer;
                int num = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_findpwd_add", commandParameters);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public static bool Exists(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_findpwd_Exists", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static bool FindSucess(FindPwd model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@uid", SqlDbType.Int, 10), new SqlParameter("@newpwd", SqlDbType.VarChar, 100), new SqlParameter("@status", SqlDbType.Int, 10) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.uid;
                commandParameters[2].Value = model.newpwd;
                commandParameters[3].Value = model.status;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_findpwd_success", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public static FindPwd GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                FindPwd pwd = new FindPwd();
                DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_findpwd_GetModel", commandParameters);
                if (set.Tables[0].Rows.Count > 0)
                {
                    if (set.Tables[0].Rows[0]["id"].ToString() != "")
                    {
                        pwd.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
                    }
                    if (set.Tables[0].Rows[0]["uid"].ToString() != "")
                    {
                        pwd.uid = new int?(int.Parse(set.Tables[0].Rows[0]["uid"].ToString()));
                    }
                    pwd.username = set.Tables[0].Rows[0]["username"].ToString();
                    pwd.oldpwd = set.Tables[0].Rows[0]["oldpwd"].ToString();
                    pwd.newpwd = set.Tables[0].Rows[0]["newpwd"].ToString();
                    if (set.Tables[0].Rows[0]["status"].ToString() != "")
                    {
                        pwd.status = new int?(int.Parse(set.Tables[0].Rows[0]["status"].ToString()));
                    }
                    if (set.Tables[0].Rows[0]["addtimer"].ToString() != "")
                    {
                        pwd.addtimer = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["addtimer"].ToString()));
                    }
                    return pwd;
                }
                return null;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public bool Update(FindPwd model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@uid", SqlDbType.Int, 10), new SqlParameter("@username", SqlDbType.VarChar, 50), new SqlParameter("@oldpwd", SqlDbType.VarChar, 100), new SqlParameter("@newpwd", SqlDbType.VarChar, 100), new SqlParameter("@status", SqlDbType.Int, 10), new SqlParameter("@addtimer", SqlDbType.DateTime) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.uid;
                commandParameters[2].Value = model.username;
                commandParameters[3].Value = model.oldpwd;
                commandParameters[4].Value = model.newpwd;
                commandParameters[5].Value = model.status;
                commandParameters[6].Value = model.addtimer;
                return (DataBase.ExecuteNonQuery("proc_findpwd_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

