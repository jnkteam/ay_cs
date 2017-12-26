namespace OriginalStudio.BLL.Settled
{
    using DBAccess;
    using OriginalStudio.Model.Settled;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.Lib.Data;

    public class Transfer
    {
        internal string SQL_TABLE = "v_transfer";
        internal string SQL_TABLE_FIELD = "[id]\r\n      ,[year]\r\n      ,[month]\r\n      ,[userid]\r\n      ,[touserid]\r\n      ,[amt]\r\n      ,[charge]\r\n      ,[remark]\r\n      ,[status]\r\n      ,[addtime]\r\n      ,[updatetime]\r\n      ,[username]\r\n      ,[full_name]\r\n      ,[username1]\r\n      ,[full_name1]";

        public int Add(OriginalStudio.Model.Settled.Transfer model)
        {
            try
            {
                int num;
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@id", SqlDbType.Int, 10),
                    new SqlParameter("@year", SqlDbType.Int, 10),
                    new SqlParameter("@month", SqlDbType.Int, 10),
                    new SqlParameter("@userid", SqlDbType.Int, 10),
                    new SqlParameter("@touserid", SqlDbType.Int, 10),
                    new SqlParameter("@amt", SqlDbType.Decimal, 9),
                    new SqlParameter("@charge", SqlDbType.Decimal, 9),
                    new SqlParameter("@remark", SqlDbType.VarChar, 200),
                    new SqlParameter("@status", SqlDbType.Int, 10),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@updatetime", SqlDbType.DateTime)
                };
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = model.year;
                parameters[2].Value = model.month;
                parameters[3].Value = model.userid;
                parameters[4].Value = model.touserid;
                parameters[5].Value = model.amt;
                parameters[6].Value = model.charge;
                parameters[7].Value = model.remark;
                parameters[8].Value = model.status;
                parameters[9].Value = model.addtime;
                parameters[10].Value = model.updatetime;
                DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_transfer_add", parameters);
                return (int) parameters[0].Value;
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

                        case "touserid":
                            builder.Append(" AND [touserid] = @touserid");
                            parameter = new SqlParameter("@touserid", SqlDbType.Int);
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

        public OriginalStudio.Model.Settled.Transfer DataRowToModel(DataRow row)
        {
            OriginalStudio.Model.Settled.Transfer transfer = new OriginalStudio.Model.Settled.Transfer();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    transfer.id = int.Parse(row["id"].ToString());
                }
                if ((row["year"] != null) && (row["year"].ToString() != ""))
                {
                    transfer.year = new int?(int.Parse(row["year"].ToString()));
                }
                if ((row["month"] != null) && (row["month"].ToString() != ""))
                {
                    transfer.month = new int?(int.Parse(row["month"].ToString()));
                }
                if ((row["userid"] != null) && (row["userid"].ToString() != ""))
                {
                    transfer.userid = int.Parse(row["userid"].ToString());
                }
                if ((row["touserid"] != null) && (row["touserid"].ToString() != ""))
                {
                    transfer.touserid = int.Parse(row["touserid"].ToString());
                }
                if ((row["amt"] != null) && (row["amt"].ToString() != ""))
                {
                    transfer.amt = decimal.Parse(row["amt"].ToString());
                }
                if ((row["charge"] != null) && (row["charge"].ToString() != ""))
                {
                    transfer.charge = decimal.Parse(row["charge"].ToString());
                }
                if (row["remark"] != null)
                {
                    transfer.remark = row["remark"].ToString();
                }
                if ((row["status"] != null) && (row["status"].ToString() != ""))
                {
                    transfer.status = int.Parse(row["status"].ToString());
                }
                if ((row["addtime"] != null) && (row["addtime"].ToString() != ""))
                {
                    transfer.addtime = DateTime.Parse(row["addtime"].ToString());
                }
                if ((row["updatetime"] != null) && (row["updatetime"].ToString() != ""))
                {
                    transfer.updatetime = new DateTime?(DateTime.Parse(row["updatetime"].ToString()));
                }
            }
            return transfer;
        }

        public bool Delete(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from transfer ");
            builder.Append(" where id=@id ");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), cmdParms) > 0;
        }

        public bool DeleteList(string idlist)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from transfer ");
            builder.Append(" where id in (" + idlist + ")  ");
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), null) > 0;
        }

        public bool Exists(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) from transfer");
            builder.Append(" where id=@id ");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            return Convert.ToInt32(DataBase.ExecuteScalar(CommandType.Text, builder.ToString(), cmdParms)) > 0;
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,year,month,userid,touserid,amt,charge,remark,status,addtime,updatetime ");
            builder.Append(" FROM transfer ");
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
            builder.Append(" id,year,month,userid,touserid,amt,charge,remark,status,addtime,updatetime ");
            builder.Append(" FROM transfer ");
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
            builder.Append(")AS Row, T.*  from transfer T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
        }

        public int GetMaxId()
        {
            return 0;

            //return DbHelperSQL.GetMaxID("id", "transfer");
        }

        public OriginalStudio.Model.Settled.Transfer GetModel(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  top 1 id,year,month,userid,touserid,amt,charge,remark,status,addtime,updatetime from transfer ");
            builder.Append(" where id=@id ");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            OriginalStudio.Model.Settled.Transfer transfer = new Model.Settled.Transfer();
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), cmdParms);
            if (set.Tables[0].Rows.Count > 0)
            {
                return this.DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public int GetRecordCount(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM transfer ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return Convert.ToInt32(DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null));
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = this.SQL_TABLE;
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = this.BuilderWhere(searchParams, paramList);
                string commandText = SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL(this.SQL_TABLE_FIELD, tables, wheres, orderby, key, pageSize, page, false);
                return DataBase.ExecuteDataset(CommandType.Text, commandText, paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(OriginalStudio.Model.Settled.Transfer model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update transfer set ");
            builder.Append("year=@year,");
            builder.Append("month=@month,");
            builder.Append("userid=@userid,");
            builder.Append("touserid=@touserid,");
            builder.Append("amt=@amt,");
            builder.Append("charge=@charge,");
            builder.Append("remark=@remark,");
            builder.Append("status=@status,");
            builder.Append("addtime=@addtime,");
            builder.Append("updatetime=@updatetime");
            builder.Append(" where id=@id ");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@year", SqlDbType.Int, 10), new SqlParameter("@month", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@touserid", SqlDbType.Int, 10), new SqlParameter("@amt", SqlDbType.Decimal, 9), new SqlParameter("@charge", SqlDbType.Decimal, 9), new SqlParameter("@remark", SqlDbType.VarChar, 200), new SqlParameter("@status", SqlDbType.Int, 10), new SqlParameter("@addtime", SqlDbType.DateTime), new SqlParameter("@updatetime", SqlDbType.DateTime), new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = model.year;
            cmdParms[1].Value = model.month;
            cmdParms[2].Value = model.userid;
            cmdParms[3].Value = model.touserid;
            cmdParms[4].Value = model.amt;
            cmdParms[5].Value = model.charge;
            cmdParms[6].Value = model.remark;
            cmdParms[7].Value = model.status;
            cmdParms[8].Value = model.addtime;
            cmdParms[9].Value = model.updatetime;
            cmdParms[10].Value = model.id;
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), cmdParms) > 0;
        }
    }
}

