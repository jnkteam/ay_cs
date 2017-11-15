namespace KuaiCard.BLL.APP
{
    using DBAccess;
    using KuaiCard.Model.APP;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class entrust
    {
        internal const string SQL_FIELDS = "id,userid,status,bankcardnum,bankname,payee,amount,rate,remittancefee,totalAmt,addtime,cdate,cadmin,remark ";
        internal const string SQL_TABLE = "entrust";

        public int Add(EntrustInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@bankcardnum", SqlDbType.VarChar, 30), new SqlParameter("@bankname", SqlDbType.NVarChar, 150), new SqlParameter("@payee", SqlDbType.NVarChar, 10), new SqlParameter("@amount", SqlDbType.Decimal, 9), new SqlParameter("@rate", SqlDbType.Decimal, 9), new SqlParameter("@remittancefee", SqlDbType.Decimal, 9), new SqlParameter("@totalAmt", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@cdate", SqlDbType.DateTime), new SqlParameter("@cadmin", SqlDbType.Int, 10), new SqlParameter("@remark", SqlDbType.NVarChar, 200) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.status;
                commandParameters[3].Value = model.bankcardnum;
                commandParameters[4].Value = model.bankname;
                commandParameters[5].Value = model.payee;
                commandParameters[6].Value = model.amount;
                commandParameters[7].Value = model.rate;
                commandParameters[8].Value = model.remittancefee;
                commandParameters[9].Value = model.totalAmt;
                commandParameters[10].Value = model.addtime;
                commandParameters[11].Value = model.cdate;
                commandParameters[12].Value = model.cadmin;
                commandParameters[13].Value = model.remark;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_entrust_ADD", commandParameters);
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

        public bool Delete(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_entrust_Delete", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,userid,status,bankcardnum,bankname,payee,amount,rate,remittancefee,totalAmt,addtime,cdate,cadmin,remark ");
            builder.Append(" FROM entrust ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }

        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ");
            if (Top > 0)
            {
                builder.Append(" top " + Top.ToString());
            }
            builder.Append(" id,userid,status,bankcardnum,bankname,payee,amount,rate,remittancefee,totalAmt,addtime,cdate,cadmin,remark ");
            builder.Append(" FROM entrust ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            builder.Append(" order by " + filedOrder);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }

        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM ( ");
            builder.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                builder.Append("order by T." + orderby);
            }
            else
            {
                builder.Append("order by T.id desc");
            }
            builder.Append(")AS Row, T.*  from entrust T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }

        public EntrustInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_entrust_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static EntrustInfo GetModelFromDs(DataSet ds)
        {
            EntrustInfo info = new EntrustInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if ((ds.Tables[0].Rows[0]["id"] != null) && (ds.Tables[0].Rows[0]["id"].ToString() != ""))
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["userid"] != null) && (ds.Tables[0].Rows[0]["userid"].ToString() != ""))
                {
                    info.userid = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["status"] != null) && (ds.Tables[0].Rows[0]["status"].ToString() != ""))
                {
                    info.status = int.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["bankcardnum"] != null) && (ds.Tables[0].Rows[0]["bankcardnum"].ToString() != ""))
                {
                    info.bankcardnum = ds.Tables[0].Rows[0]["bankcardnum"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["bankname"] != null) && (ds.Tables[0].Rows[0]["bankname"].ToString() != ""))
                {
                    info.bankname = ds.Tables[0].Rows[0]["bankname"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["payee"] != null) && (ds.Tables[0].Rows[0]["payee"].ToString() != ""))
                {
                    info.payee = ds.Tables[0].Rows[0]["payee"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["amount"] != null) && (ds.Tables[0].Rows[0]["amount"].ToString() != ""))
                {
                    info.amount = decimal.Parse(ds.Tables[0].Rows[0]["amount"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["rate"] != null) && (ds.Tables[0].Rows[0]["rate"].ToString() != ""))
                {
                    info.rate = decimal.Parse(ds.Tables[0].Rows[0]["rate"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["remittancefee"] != null) && (ds.Tables[0].Rows[0]["remittancefee"].ToString() != ""))
                {
                    info.remittancefee = decimal.Parse(ds.Tables[0].Rows[0]["remittancefee"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["totalAmt"] != null) && (ds.Tables[0].Rows[0]["totalAmt"].ToString() != ""))
                {
                    info.totalAmt = decimal.Parse(ds.Tables[0].Rows[0]["totalAmt"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["addtime"] != null) && (ds.Tables[0].Rows[0]["addtime"].ToString() != ""))
                {
                    info.addtime = DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["cdate"] != null) && (ds.Tables[0].Rows[0]["cdate"].ToString() != ""))
                {
                    info.cdate = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["cdate"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["cadmin"] != null) && (ds.Tables[0].Rows[0]["cadmin"].ToString() != ""))
                {
                    info.cadmin = new int?(int.Parse(ds.Tables[0].Rows[0]["cadmin"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["remark"] != null) && (ds.Tables[0].Rows[0]["remark"].ToString() != ""))
                {
                    info.remark = ds.Tables[0].Rows[0]["remark"].ToString();
                }
                return info;
            }
            return null;
        }

        public int GetRecordCount(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM entrust ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            object obj2 = DataBase.ExecuteScalar(CommandType.Text, builder.ToString(), null);
            if (obj2 == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj2);
        }

        public static DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "entrust";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("id,userid,status,bankcardnum,bankname,payee,amount,rate,remittancefee,totalAmt,addtime,cdate,cadmin,remark ", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(EntrustInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@bankcardnum", SqlDbType.VarChar, 30), new SqlParameter("@bankname", SqlDbType.NVarChar, 150), new SqlParameter("@payee", SqlDbType.NVarChar, 10), new SqlParameter("@amount", SqlDbType.Decimal, 9), new SqlParameter("@rate", SqlDbType.Decimal, 9), new SqlParameter("@remittancefee", SqlDbType.Decimal, 9), new SqlParameter("@totalAmt", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@cdate", SqlDbType.DateTime), new SqlParameter("@cadmin", SqlDbType.Int, 10), new SqlParameter("@remark", SqlDbType.NVarChar, 200) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.status;
                commandParameters[3].Value = model.bankcardnum;
                commandParameters[4].Value = model.bankname;
                commandParameters[5].Value = model.payee;
                commandParameters[6].Value = model.amount;
                commandParameters[7].Value = model.rate;
                commandParameters[8].Value = model.remittancefee;
                commandParameters[9].Value = model.totalAmt;
                commandParameters[10].Value = model.addtime;
                commandParameters[11].Value = model.cdate;
                commandParameters[12].Value = model.cadmin;
                commandParameters[13].Value = model.remark;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_entrust_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

