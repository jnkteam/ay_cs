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

    public class recharge
    {
        internal const string SQL_FIELDS = "id,userid,orderno,rechargeAmt,balance,addtime,status,paytime,transNo,remark ";
        internal const string SQL_TABLE = "recharge";

        public int Add(RechargeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@orderno", SqlDbType.NVarChar, 30), new SqlParameter("@rechargeAmt", SqlDbType.Decimal, 9), new SqlParameter("@balance", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@paytime", SqlDbType.DateTime), new SqlParameter("@transNo", SqlDbType.NVarChar, 50), new SqlParameter("@remark", SqlDbType.NVarChar, 200) };
                commandParameters[0].Direction = ParameterDirection.Output;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.orderno;
                commandParameters[3].Value = model.rechargeAmt;
                commandParameters[4].Value = model.balance;
                commandParameters[5].Value = model.addtime;
                commandParameters[6].Value = model.status;
                commandParameters[7].Value = model.paytime;
                commandParameters[8].Value = model.transNo;
                commandParameters[9].Value = model.remark;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_recharge_add", commandParameters);
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
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_recharge_Delete", commandParameters) > 0);
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
            builder.Append("select id,userid,orderno,rechargeAmt,balance,addtime,status,paytime,transNo,remark ");
            builder.Append(" FROM recharge ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }

        public RechargeInfo GetModel(int id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
                commandParameters[0].Value = id;
                return GetModelFromDs(DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_recharge_GetModel", commandParameters));
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static RechargeInfo GetModelFromDs(DataSet ds)
        {
            RechargeInfo info = new RechargeInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if ((ds.Tables[0].Rows[0]["id"] != null) && (ds.Tables[0].Rows[0]["id"].ToString() != ""))
                {
                    info.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if ((ds.Tables[0].Rows[0]["userid"] != null) && (ds.Tables[0].Rows[0]["userid"].ToString() != ""))
                {
                    info.userid = new int?(int.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["orderno"] != null) && (ds.Tables[0].Rows[0]["orderno"].ToString() != ""))
                {
                    info.orderno = ds.Tables[0].Rows[0]["orderno"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["rechargeAmt"] != null) && (ds.Tables[0].Rows[0]["rechargeAmt"].ToString() != ""))
                {
                    info.rechargeAmt = new decimal?(decimal.Parse(ds.Tables[0].Rows[0]["rechargeAmt"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["balance"] != null) && (ds.Tables[0].Rows[0]["balance"].ToString() != ""))
                {
                    info.balance = new decimal?(decimal.Parse(ds.Tables[0].Rows[0]["balance"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["addtime"] != null) && (ds.Tables[0].Rows[0]["addtime"].ToString() != ""))
                {
                    info.addtime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["addtime"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["status"] != null) && (ds.Tables[0].Rows[0]["status"].ToString() != ""))
                {
                    info.status = new int?(int.Parse(ds.Tables[0].Rows[0]["status"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["paytime"] != null) && (ds.Tables[0].Rows[0]["paytime"].ToString() != ""))
                {
                    info.paytime = new DateTime?(DateTime.Parse(ds.Tables[0].Rows[0]["paytime"].ToString()));
                }
                if ((ds.Tables[0].Rows[0]["transNo"] != null) && (ds.Tables[0].Rows[0]["transNo"].ToString() != ""))
                {
                    info.transNo = ds.Tables[0].Rows[0]["transNo"].ToString();
                }
                if ((ds.Tables[0].Rows[0]["remark"] != null) && (ds.Tables[0].Rows[0]["remark"].ToString() != ""))
                {
                    info.remark = ds.Tables[0].Rows[0]["remark"].ToString();
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
                string tables = "recharge";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL("id,userid,orderno,rechargeAmt,balance,addtime,status,paytime,transNo,remark ", tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(RechargeInfo model)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@orderno", SqlDbType.NVarChar, 30), new SqlParameter("@rechargeAmt", SqlDbType.Decimal, 9), new SqlParameter("@balance", SqlDbType.Decimal, 9), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@paytime", SqlDbType.DateTime), new SqlParameter("@transNo", SqlDbType.NVarChar, 50), new SqlParameter("@remark", SqlDbType.NVarChar, 200) };
                commandParameters[0].Value = model.id;
                commandParameters[1].Value = model.userid;
                commandParameters[2].Value = model.orderno;
                commandParameters[3].Value = model.rechargeAmt;
                commandParameters[4].Value = model.balance;
                commandParameters[5].Value = model.addtime;
                commandParameters[6].Value = model.status;
                commandParameters[7].Value = model.paytime;
                commandParameters[8].Value = model.transNo;
                commandParameters[9].Value = model.remark;
                return (DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_recharge_Update", commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

