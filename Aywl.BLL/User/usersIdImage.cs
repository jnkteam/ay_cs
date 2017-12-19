namespace OriginalStudio.BLL.User
{
    using DBAccess;
    using OriginalStudio.Model.User;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.Model.Trade;

    public class usersIdImage
    {
        internal const string SQL_FIELDS = "[id]\r\n      ,[userId]\r\n      ,[ptype]\r\n      ,[filesize]\r\n      ,[ptype1]\r\n      ,[filesize1]\r\n      ,[status]\r\n      ,[why]\r\n      ,[admin]\r\n      ,[checktime]\r\n      ,[addtime]\r\n      ,[userName],[payeeName],[account],[IdCard]";
        internal const string SQL_TABLE = "V_usersIdImage";

        public int Add(usersIdImageInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.Int, 10), 
                    new SqlParameter("@userId", SqlDbType.Int, 10), 
                    new SqlParameter("@image_on", SqlDbType.Image), 
                    new SqlParameter("@image_down", SqlDbType.Image), 
                    new SqlParameter("@ptype", SqlDbType.NVarChar, 20), 
                    new SqlParameter("@filesize", SqlDbType.Int, 10), 
                    new SqlParameter("@ptype1", SqlDbType.NVarChar, 20), 
                    new SqlParameter("@filesize1", SqlDbType.Int, 10), 
                    new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                    new SqlParameter("@addtime", SqlDbType.DateTime) 
                };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userId;
                commandParameters[2].Value = model.image_on;
                commandParameters[3].Value = model.image_down;
                commandParameters[4].Value = model.ptype;
                commandParameters[5].Value = model.filesize;
                commandParameters[6].Value = model.ptype1;
                commandParameters[7].Value = model.filesize1;
                commandParameters[8].Value = 0;// model.status;
                commandParameters[9].Value = model.addtime;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersIdImage_add", commandParameters);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter;
                    SearchParam param2 = param[i];
                    switch (param2.ParamKey.Trim().ToLower())
                    {
                        case "userid":
                            builder.Append(" AND [userid] = @userid");
                            parameter = new SqlParameter("@userid", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "username":
                            builder.Append(" AND [userName] like @UserName");
                            parameter = new SqlParameter("@UserName", SqlDbType.VarChar, 20);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 20) + "%";
                            paramList.Add(parameter);
                            break;

                        case "status":
                            builder.Append(" AND [status] = @status");
                            parameter = new SqlParameter("@status", SqlDbType.TinyInt);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "stime":
                            builder.Append(" AND [addtime] >= @stime");
                            parameter = new SqlParameter("@stime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "etime":
                            builder.Append(" AND [addtime] <= @etime");
                            parameter = new SqlParameter("@etime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public bool Check(usersIdImageInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@why", SqlDbType.NVarChar, 150), new SqlParameter("@admin", SqlDbType.Int, 10), new SqlParameter("@checktime", SqlDbType.DateTime) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = 0;// (int) model.status;
                commandParameters[2].Value = model.why;
                commandParameters[3].Value = model.admin;
                commandParameters[4].Value = model.checktime;
                bool flag = DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersIdImage_update", commandParameters) > 0;
                if (flag && (model.status == ImageStatus.空))
                {
                    UserFactory.ClearCache(model.userId.Value);
                }
                return flag;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_usersIdImage_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool Exists(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int, 10) };
                commandParameters[0].Value = userid;
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_usersIdImage_Exists", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public usersIdImageInfo Get(int id)
        {
            usersIdImageInfo info = new usersIdImageInfo();
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            IDataReader reader = DataBase.ExecuteReader(CommandType.StoredProcedure, "proc_usersIdImage_GetModel", commandParameters);
            if (reader.Read())
            {
                object obj2 = reader["image_on"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.image_on = (byte[]) obj2;
                }
                obj2 = reader["image_down"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.image_down = (byte[]) obj2;
                }
                info.ptype = reader["ptype"].ToString();
                obj2 = reader["filesize"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.filesize = new int?((int) obj2);
                }
                info.ptype1 = reader["ptype1"].ToString();
                obj2 = reader["filesize1"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.filesize1 = new int?((int) obj2);
                }
            }
            return info;
        }

        public usersIdImageInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_usersIdImage_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public usersIdImageInfo GetModelByUser(int userid)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10) };
                commandParameters[0].Value = userid;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_usersIdImage_GetByUser", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static usersIdImageInfo GetModelFromDs(DataSet ds)
        {
            usersIdImageInfo info = new usersIdImageInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["userId"].ToString() != "")
                {
                    info.userId = new int?(int.Parse(ds.Tables[0].Rows[0]["userId"].ToString()));
                }
                info.ptype = ds.Tables[0].Rows[0]["ptype"].ToString();
                if (ds.Tables[0].Rows[0]["filesize"].ToString() != "")
                {
                    info.filesize = new int?(int.Parse(ds.Tables[0].Rows[0]["filesize"].ToString()));
                }
                info.ptype1 = ds.Tables[0].Rows[0]["ptype1"].ToString();
                if (ds.Tables[0].Rows[0]["filesize1"].ToString() != "")
                {
                    info.filesize1 = new int?(int.Parse(ds.Tables[0].Rows[0]["filesize1"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    info.status = (ImageStatus) int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                else
                {
                    info.status =ImageStatus.空;
                }
                info.why = ds.Tables[0].Rows[0]["why"].ToString();
                if (ds.Tables[0].Rows[0]["admin"].ToString() != "")
                {
                    info.admin = new int?(int.Parse(ds.Tables[0].Rows[0]["admin"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["checktime"].ToString() != "")
                {
                    info.checktime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["checktime"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["addtime"].ToString() != "")
                {
                    info.addtime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString()));
                }
                return info;
            }
            return null;
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "V_usersIdImage";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);

                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + 
                    SqlHelper.GetPageSelectSQL("[id]\r\n      ,[userId]\r\n      ,[ptype]\r\n      ,[filesize]\r\n      ,[ptype1]\r\n      ,[filesize1]\r\n      ,[status]\r\n      ,[why]\r\n      ,[admin]\r\n      ,[checktime]\r\n      ,[addtime]\r\n      ,[userName],[payeeName],[account],[IdCard]", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }
    }
}

