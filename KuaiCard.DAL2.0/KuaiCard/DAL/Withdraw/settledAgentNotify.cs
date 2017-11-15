namespace KuaiCard.DAL.Withdraw
{
    using DBAccess;
    using KuaiCard.Model.Withdraw;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class settledAgentNotify
    {
        internal string FIELDS = "[id]\r\n      ,[notify_id]\r\n      ,[userid]\r\n      ,[trade_no]\r\n      ,[out_trade_no]\r\n      ,[notifystatus]\r\n      ,[notifyurl]\r\n      ,[resText]\r\n      ,[addTime]\r\n      ,[resTime]\r\n      ,[ext1]\r\n      ,[ext2]\r\n      ,[ext3]\r\n      ,[remark]";
        internal string SQL_TABLE = "settledAgentNotify";

        public int Add(KuaiCard.Model.Withdraw.settledAgentNotify model)
        {
            int num;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@notify_id", SqlDbType.VarChar, 20), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@trade_no", SqlDbType.VarChar, 40), new SqlParameter("@out_trade_no", SqlDbType.VarChar, 0x40), new SqlParameter("@notifystatus", SqlDbType.TinyInt, 1), new SqlParameter("@notifyurl", SqlDbType.VarChar, 0x3e8), new SqlParameter("@resText", SqlDbType.NVarChar, 200), new SqlParameter("@addTime", SqlDbType.DateTime), new SqlParameter("@resTime", SqlDbType.DateTime), new SqlParameter("@ext1", SqlDbType.VarChar, 50), new SqlParameter("@ext2", SqlDbType.VarChar, 50), new SqlParameter("@ext3", SqlDbType.VarChar, 50), new SqlParameter("@remark", SqlDbType.NVarChar, 500) };
            parameterArray[0].Direction = ParameterDirection.Output;
            parameterArray[1].Value = model.notify_id;
            parameterArray[2].Value = model.userid;
            parameterArray[3].Value = model.trade_no;
            parameterArray[4].Value = model.out_trade_no;
            parameterArray[5].Value = model.notifystatus;
            parameterArray[6].Value = model.notifyurl;
            parameterArray[7].Value = model.resText;
            parameterArray[8].Value = model.addTime;
            parameterArray[9].Value = model.resTime;
            parameterArray[10].Value = model.ext1;
            parameterArray[11].Value = model.ext2;
            parameterArray[12].Value = model.ext3;
            parameterArray[13].Value = model.remark;
            DbHelperSQL.RunProcedure("proc_settledAgentNotify_ADD", (IDataParameter[]) parameterArray, out num);
            return (int) parameterArray[0].Value;
        }

        private static string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
        {
            StringBuilder builder = new StringBuilder(" 1 = 1");
            if ((param != null) && (param.Count > 0))
            {
                for (int i = 0; i < param.Count; i++)
                {
                    SearchParam param2 = param[i];
                    string str2 = param2.ParamKey.Trim().ToLower();
                    if (str2 != null)
                    {
                        if (!(str2 == "userid"))
                        {
                            if (str2 == "notifystatus")
                            {
                                goto Label_00F5;
                            }
                            if (str2 == "trade_no")
                            {
                                goto Label_0135;
                            }
                            if (str2 == "out_trade_no")
                            {
                                goto Label_017A;
                            }
                            if (str2 == "begindate")
                            {
                                goto Label_01BC;
                            }
                            if (str2 == "enddate")
                            {
                                goto Label_01F8;
                            }
                        }
                        else
                        {
                            builder.Append(" AND [userid] = @userid");
                            SqlParameter parameter = new SqlParameter("@userid", SqlDbType.Int);
                            parameter.Value = (int) param2.ParamValue;
                            paramList.Add(parameter);
                        }
                    }
                    continue;
                Label_00F5:
                    builder.Append(" AND [notifystatus] = @notifystatus");
                    SqlParameter item = new SqlParameter("@notifystatus", SqlDbType.TinyInt);
                    item.Value = (int) param2.ParamValue;
                    paramList.Add(item);
                    continue;
                Label_0135:
                    builder.Append(" AND [trade_no] like @trade_no");
                    SqlParameter parameter3 = new SqlParameter("@trade_no", SqlDbType.VarChar);
                    parameter3.Value = ((string) param2.ParamValue) + "%";
                    paramList.Add(parameter3);
                    continue;
                Label_017A:
                    builder.Append(" AND [out_trade_no] like @out_trade_no");
                    SqlParameter parameter4 = new SqlParameter("@out_trade_no", SqlDbType.VarChar);
                    parameter4.Value = ((string) param2.ParamValue) + "%";
                    paramList.Add(parameter4);
                    continue;
                Label_01BC:
                    builder.Append(" AND [addTime] >= @addTime");
                    SqlParameter parameter5 = new SqlParameter("@beginpaytime", SqlDbType.DateTime);
                    parameter5.Value = (DateTime) param2.ParamValue;
                    paramList.Add(parameter5);
                    continue;
                Label_01F8:
                    builder.Append(" AND [addTime] <= @addTime");
                    SqlParameter parameter6 = new SqlParameter("@endpaytime", SqlDbType.DateTime);
                    parameter6.Value = (DateTime) param2.ParamValue;
                    paramList.Add(parameter6);
                }
            }
            return builder.ToString();
        }

        public KuaiCard.Model.Withdraw.settledAgentNotify DataRowToModel(DataRow row)
        {
            KuaiCard.Model.Withdraw.settledAgentNotify notify = new KuaiCard.Model.Withdraw.settledAgentNotify();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    notify.id = int.Parse(row["id"].ToString());
                }
                if (row["notify_id"] != null)
                {
                    notify.notify_id = row["notify_id"].ToString();
                }
                if ((row["userid"] != null) && (row["userid"].ToString() != ""))
                {
                    notify.userid = int.Parse(row["userid"].ToString());
                }
                if (row["trade_no"] != null)
                {
                    notify.trade_no = row["trade_no"].ToString();
                }
                if (row["out_trade_no"] != null)
                {
                    notify.out_trade_no = row["out_trade_no"].ToString();
                }
                if ((row["notifystatus"] != null) && (row["notifystatus"].ToString() != ""))
                {
                    notify.notifystatus = int.Parse(row["notifystatus"].ToString());
                }
                if (row["notifyurl"] != null)
                {
                    notify.notifyurl = row["notifyurl"].ToString();
                }
                if (row["resText"] != null)
                {
                    notify.resText = row["resText"].ToString();
                }
                if ((row["addTime"] != null) && (row["addTime"].ToString() != ""))
                {
                    notify.addTime = DateTime.Parse(row["addTime"].ToString());
                }
                if ((row["resTime"] != null) && (row["resTime"].ToString() != ""))
                {
                    notify.resTime = new DateTime?(DateTime.Parse(row["resTime"].ToString()));
                }
                if (row["ext1"] != null)
                {
                    notify.ext1 = row["ext1"].ToString();
                }
                if (row["ext2"] != null)
                {
                    notify.ext2 = row["ext2"].ToString();
                }
                if (row["ext3"] != null)
                {
                    notify.ext3 = row["ext3"].ToString();
                }
                if (row["remark"] != null)
                {
                    notify.remark = row["remark"].ToString();
                }
            }
            return notify;
        }

        public bool Delete(int id)
        {
            int rowsAffected = 0;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            parameterArray[0].Value = id;
            DbHelperSQL.RunProcedure("proc_settledAgentNotify_Delete", (IDataParameter[]) parameterArray, out rowsAffected);
            return (rowsAffected > 0);
        }

        public bool DeleteList(string idlist)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from settledAgentNotify ");
            builder.Append(" where id in (" + idlist + ")  ");
            return (DbHelperSQL.ExecuteSql(builder.ToString()) > 0);
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,notify_id,userid,trade_no,out_trade_no,notifystatus,notifyurl,resText,addTime,resTime,ext1,ext2,ext3,remark ");
            builder.Append(" FROM settledAgentNotify ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(builder.ToString());
        }

        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ");
            if (Top > 0)
            {
                builder.Append(" top " + Top.ToString());
            }
            builder.Append(" id,notify_id,userid,trade_no,out_trade_no,notifystatus,notifyurl,resText,addTime,resTime,ext1,ext2,ext3,remark ");
            builder.Append(" FROM settledAgentNotify ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            builder.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(builder.ToString());
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
            builder.Append(")AS Row, T.*  from settledAgentNotify T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(builder.ToString());
        }

        public KuaiCard.Model.Withdraw.settledAgentNotify GetModel(int id)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            parameterArray[0].Value = id;
            KuaiCard.Model.Withdraw.settledAgentNotify notify = new KuaiCard.Model.Withdraw.settledAgentNotify();
            DataSet set = DbHelperSQL.RunProcedure("proc_settledAgentNotify_GetModel", (IDataParameter[]) parameterArray, "ds");
            if (set.Tables[0].Rows.Count > 0)
            {
                return this.DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public int GetRecordCount(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM settledAgentNotify ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            object single = DbHelperSQL.GetSingle(builder.ToString());
            if (single == null)
            {
                return 0;
            }
            return Convert.ToInt32(single);
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
                    orderby = "addTime desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = BuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" + SqlHelper.GetPageSelectSQL(this.FIELDS, tables, wheres, orderby, key, pageSize, page, false), paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }

        public bool Update(KuaiCard.Model.Withdraw.settledAgentNotify model)
        {
            int rowsAffected = 0;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@notify_id", SqlDbType.VarChar, 20), new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@trade_no", SqlDbType.VarChar, 40), new SqlParameter("@out_trade_no", SqlDbType.VarChar, 0x40), new SqlParameter("@notifystatus", SqlDbType.TinyInt, 1), new SqlParameter("@notifyurl", SqlDbType.VarChar, 0x3e8), new SqlParameter("@resText", SqlDbType.NVarChar, 200), new SqlParameter("@addTime", SqlDbType.DateTime), new SqlParameter("@resTime", SqlDbType.DateTime), new SqlParameter("@ext1", SqlDbType.VarChar, 50), new SqlParameter("@ext2", SqlDbType.VarChar, 50), new SqlParameter("@ext3", SqlDbType.VarChar, 50), new SqlParameter("@remark", SqlDbType.NVarChar, 500) };
            parameterArray[0].Value = model.id;
            parameterArray[1].Value = model.notify_id;
            parameterArray[2].Value = model.userid;
            parameterArray[3].Value = model.trade_no;
            parameterArray[4].Value = model.out_trade_no;
            parameterArray[5].Value = model.notifystatus;
            parameterArray[6].Value = model.notifyurl;
            parameterArray[7].Value = model.resText;
            parameterArray[8].Value = model.addTime;
            parameterArray[9].Value = model.resTime;
            parameterArray[10].Value = model.ext1;
            parameterArray[11].Value = model.ext2;
            parameterArray[12].Value = model.ext3;
            parameterArray[13].Value = model.remark;
            DbHelperSQL.RunProcedure("proc_settledAgentNotify_Update", (IDataParameter[]) parameterArray, out rowsAffected);
            return (rowsAffected > 0);
        }
    }
}

