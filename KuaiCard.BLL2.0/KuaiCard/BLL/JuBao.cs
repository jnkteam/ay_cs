namespace KuaiCard.BLL
{
    using DBAccess;
    using KuaiCard.Model;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class JuBao
    {
        internal const string SQL_FIELDS = "id,[name],email,tel,url,[type],remark,addtime,status,checktime,[check],checkremark,pwd,field1,field2,field3";
        internal const string SQL_TABLE = "JuBao";

        public int Add(JuBaoInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@name", SqlDbType.NVarChar, 50), new SqlParameter("@email", SqlDbType.NVarChar, 30), new SqlParameter("@tel", SqlDbType.VarChar, 20), new SqlParameter("@url", SqlDbType.NVarChar, 200), new SqlParameter("@type", SqlDbType.TinyInt, 1), new SqlParameter("@remark", SqlDbType.NVarChar, 500), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@checktime", SqlDbType.DateTime), new SqlParameter("@check", SqlDbType.Int, 10), new SqlParameter("@checkremark", SqlDbType.NVarChar, 500), new SqlParameter("@pwd", SqlDbType.NVarChar, 20), new SqlParameter("@field1", SqlDbType.NVarChar, 50), new SqlParameter("@field2", SqlDbType.NVarChar, 50), new SqlParameter("@field3", SqlDbType.NVarChar, 200) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.name;
                commandParameters[2].Value = model.email;
                commandParameters[3].Value = model.tel;
                commandParameters[4].Value = model.url;
                commandParameters[5].Value = (int) model.type;
                commandParameters[6].Value = model.remark;
                commandParameters[7].Value = model.addtime;
                commandParameters[8].Value = (int) model.status;
                commandParameters[9].Value = model.checktime;
                commandParameters[10].Value = model.check;
                commandParameters[11].Value = model.checkremark;
                commandParameters[12].Value = model.pwd;
                commandParameters[13].Value = model.field1;
                commandParameters[14].Value = model.field2;
                commandParameters[15].Value = model.field3;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_JuBao_add", commandParameters);
                return (int) commandParameters[0].Value;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        private string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
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

        public bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_JuBao_Delete", commandParameters) > 0);
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
                return (Convert.ToInt32(DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_JuBao_Exists", commandParameters)) == 1);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public DataTable GetList(int userId)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@userId", SqlDbType.Int) };
                commandParameters[0].Value = userId;
                return DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_JuBao_GetList", commandParameters).Tables[0];
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public JuBaoInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_JuBao_GetModel", commandParameters);
                return this.GetModelFromDs(ds);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public JuBaoInfo GetModelByPwd(string pwd)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@pwd", SqlDbType.NVarChar, 20) };
                commandParameters[0].Value = pwd;
                DataSet ds = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_JuBao_GetModelBypwd", commandParameters);
                return this.GetModelFromDs(ds);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public JuBaoInfo GetModelFromDs(DataSet ds)
        {
            JuBaoInfo info = new JuBaoInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if ((ds.Tables[0].Rows[0]["id"] != null) && (ds.Tables[0].Rows[0]["id"].ToString() != ""))
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["name"] != null) && (ds.Tables[0].Rows[0]["name"].ToString() != ""))
                {
                    info.name = ds.Tables[0].Rows[0]["name"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["email"] != null) && (ds.Tables[0].Rows[0]["email"].ToString() != ""))
                {
                    info.email = ds.Tables[0].Rows[0]["email"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["tel"] != null) && (ds.Tables[0].Rows[0]["tel"].ToString() != ""))
                {
                    info.tel = ds.Tables[0].Rows[0]["tel"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["url"] != null) && (ds.Tables[0].Rows[0]["url"].ToString() != ""))
                {
                    info.url = ds.Tables[0].Rows[0]["url"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["type"] != null) && (ds.Tables[0].Rows[0]["type"].ToString() != ""))
                {
                    info.type = (JuBaoEnum) int.Parse(ds.Tables[0].Rows[0]["type"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["remark"] != null) && (ds.Tables[0].Rows[0]["remark"].ToString() != ""))
                {
                    info.remark = ds.Tables[0].Rows[0]["remark"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["addtime"] != null) && (ds.Tables[0].Rows[0]["addtime"].ToString() != ""))
                {
                    info.addtime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["status"] != null) && (ds.Tables[0].Rows[0]["status"].ToString() != ""))
                {
                    info.status = (JuBaoStatusEnum) int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["checktime"] != null) && (ds.Tables[0].Rows[0]["checktime"].ToString() != ""))
                {
                    info.checktime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["checktime"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["check"] != null) && (ds.Tables[0].Rows[0]["check"].ToString() != ""))
                {
                    info.check = new int?(int.Parse(ds.Tables[0].Rows[0]["check"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["checkremark"] != null) && (ds.Tables[0].Rows[0]["checkremark"].ToString() != ""))
                {
                    info.checkremark = ds.Tables[0].Rows[0]["checkremark"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["pwd"] != null) && (ds.Tables[0].Rows[0]["pwd"].ToString() != ""))
                {
                    info.pwd = ds.Tables[0].Rows[0]["pwd"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["field1"] != null) && (ds.Tables[0].Rows[0]["field1"].ToString() != ""))
                {
                    info.field1 = ds.Tables[0].Rows[0]["field1"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["field2"] != null) && (ds.Tables[0].Rows[0]["field2"].ToString() != ""))
                {
                    info.field2 = ds.Tables[0].Rows[0]["field2"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["field3"] != null) && (ds.Tables[0].Rows[0]["field3"].ToString() != ""))
                {
                    info.field3 = ds.Tables[0].Rows[0]["field3"].ToString();
                }
                return info;
            }
            return null;
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "JuBao";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "addtime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = this.BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("id,[name],email,tel,url,[type],remark,addtime,status,checktime,[check],checkremark,pwd,field1,field2,field3", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(JuBaoInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@name", SqlDbType.NVarChar, 50), new SqlParameter("@email", SqlDbType.NVarChar, 30), new SqlParameter("@tel", SqlDbType.VarChar, 20), new SqlParameter("@url", SqlDbType.NVarChar, 200), new SqlParameter("@type", SqlDbType.TinyInt, 1), new SqlParameter("@remark", SqlDbType.NVarChar, 500), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@checktime", SqlDbType.DateTime), new SqlParameter("@check", SqlDbType.Int, 10), new SqlParameter("@checkremark", SqlDbType.NVarChar, 500), new SqlParameter("@pwd", SqlDbType.NVarChar, 20), new SqlParameter("@field1", SqlDbType.NVarChar, 50), new SqlParameter("@field2", SqlDbType.NVarChar, 50), new SqlParameter("@field3", SqlDbType.NVarChar, 200) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.name;
                commandParameters[2].Value = model.email;
                commandParameters[3].Value = model.tel;
                commandParameters[4].Value = model.url;
                commandParameters[5].Value = (int) model.type;
                commandParameters[6].Value = model.remark;
                commandParameters[7].Value = model.addtime;
                commandParameters[8].Value = (int) model.status;
                commandParameters[9].Value = model.checktime;
                commandParameters[10].Value = model.check;
                commandParameters[11].Value = model.checkremark;
                commandParameters[12].Value = model.pwd;
                commandParameters[13].Value = model.field1;
                commandParameters[14].Value = model.field2;
                commandParameters[15].Value = model.field3;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_JuBao_update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

