namespace KuaiCard.BLL.Settled
{
    using DBAccess;
    using KuaiCard.Model.Settled;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class transferscheme
    {
        public int Add(KuaiCard.Model.Settled.transferscheme model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into transferscheme(");
            builder.Append("schemename,minamtlimitofeach,maxamtlimitofeach,dailymaxtimes,dailymaxamt,monthmaxtimes,monthmaxamt,chargerate,chargeleastofeach,chargemostofeach,isdefault)");
            builder.Append(" values (");
            builder.Append("@schemename,@minamtlimitofeach,@maxamtlimitofeach,@dailymaxtimes,@dailymaxamt,@monthmaxtimes,@monthmaxamt,@chargerate,@chargeleastofeach,@chargemostofeach,@isdefault)");
            builder.Append(";select @@IDENTITY");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@schemename", SqlDbType.NVarChar, 50), new SqlParameter("@minamtlimitofeach", SqlDbType.Decimal, 9), new SqlParameter("@maxamtlimitofeach", SqlDbType.Decimal, 9), new SqlParameter("@dailymaxtimes", SqlDbType.Int, 10), new SqlParameter("@dailymaxamt", SqlDbType.Decimal, 9), new SqlParameter("@monthmaxtimes", SqlDbType.Int, 10), new SqlParameter("@monthmaxamt", SqlDbType.Decimal, 9), new SqlParameter("@chargerate", SqlDbType.Decimal, 9), new SqlParameter("@chargeleastofeach", SqlDbType.Decimal, 9), new SqlParameter("@chargemostofeach", SqlDbType.Decimal, 9), new SqlParameter("@isdefault", SqlDbType.TinyInt, 1) };
            cmdParms[0].Value = model.schemename;
            cmdParms[1].Value = model.minamtlimitofeach;
            cmdParms[2].Value = model.maxamtlimitofeach;
            cmdParms[3].Value = model.dailymaxtimes;
            cmdParms[4].Value = model.dailymaxamt;
            cmdParms[5].Value = model.monthmaxtimes;
            cmdParms[6].Value = model.monthmaxamt;
            cmdParms[7].Value = model.chargerate;
            cmdParms[8].Value = model.chargeleastofeach;
            cmdParms[9].Value = model.chargemostofeach;
            cmdParms[10].Value = model.isdefault;
            object single = DbHelperSQL.GetSingle(builder.ToString(), cmdParms);
            if (single == null)
            {
                return 0;
            }
            return Convert.ToInt32(single);
        }

        public KuaiCard.Model.Settled.transferscheme DataRowToModel(DataRow row)
        {
            KuaiCard.Model.Settled.transferscheme transferscheme = new KuaiCard.Model.Settled.transferscheme();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    transferscheme.id = int.Parse(row["id"].ToString());
                }
                if (row["schemename"] != null)
                {
                    transferscheme.schemename = row["schemename"].ToString();
                }
                if ((row["minamtlimitofeach"] != null) && (row["minamtlimitofeach"].ToString() != ""))
                {
                    transferscheme.minamtlimitofeach = decimal.Parse(row["minamtlimitofeach"].ToString());
                }
                if ((row["maxamtlimitofeach"] != null) && (row["maxamtlimitofeach"].ToString() != ""))
                {
                    transferscheme.maxamtlimitofeach = decimal.Parse(row["maxamtlimitofeach"].ToString());
                }
                if ((row["dailymaxtimes"] != null) && (row["dailymaxtimes"].ToString() != ""))
                {
                    transferscheme.dailymaxtimes = int.Parse(row["dailymaxtimes"].ToString());
                }
                if ((row["dailymaxamt"] != null) && (row["dailymaxamt"].ToString() != ""))
                {
                    transferscheme.dailymaxamt = decimal.Parse(row["dailymaxamt"].ToString());
                }
                if ((row["monthmaxtimes"] != null) && (row["monthmaxtimes"].ToString() != ""))
                {
                    transferscheme.monthmaxtimes = int.Parse(row["monthmaxtimes"].ToString());
                }
                if ((row["monthmaxamt"] != null) && (row["monthmaxamt"].ToString() != ""))
                {
                    transferscheme.monthmaxamt = decimal.Parse(row["monthmaxamt"].ToString());
                }
                if ((row["chargerate"] != null) && (row["chargerate"].ToString() != ""))
                {
                    transferscheme.chargerate = decimal.Parse(row["chargerate"].ToString());
                }
                if ((row["chargeleastofeach"] != null) && (row["chargeleastofeach"].ToString() != ""))
                {
                    transferscheme.chargeleastofeach = decimal.Parse(row["chargeleastofeach"].ToString());
                }
                if ((row["chargemostofeach"] != null) && (row["chargemostofeach"].ToString() != ""))
                {
                    transferscheme.chargemostofeach = decimal.Parse(row["chargemostofeach"].ToString());
                }
                if ((row["isdefault"] != null) && (row["isdefault"].ToString() != ""))
                {
                    transferscheme.isdefault = int.Parse(row["isdefault"].ToString());
                }
            }
            return transferscheme;
        }

        public bool Delete(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from transferscheme ");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            return (DbHelperSQL.ExecuteSql(builder.ToString(), cmdParms) > 0);
        }

        public bool DeleteList(string idlist)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from transferscheme ");
            builder.Append(" where id in (" + idlist + ")  ");
            return (DbHelperSQL.ExecuteSql(builder.ToString()) > 0);
        }

        public bool Exists(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) from transferscheme");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            return DbHelperSQL.Exists(builder.ToString(), cmdParms);
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,schemename,minamtlimitofeach,maxamtlimitofeach,dailymaxtimes,dailymaxamt,monthmaxtimes,monthmaxamt,chargerate,chargeleastofeach,chargemostofeach,isdefault ");
            builder.Append(" FROM transferscheme ");
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
            builder.Append(" id,schemename,minamtlimitofeach,maxamtlimitofeach,dailymaxtimes,dailymaxamt,monthmaxtimes,monthmaxamt,chargerate,chargeleastofeach,chargemostofeach,isdefault ");
            builder.Append(" FROM transferscheme ");
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
            builder.Append(")AS Row, T.*  from transferscheme T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(builder.ToString());
        }

        public KuaiCard.Model.Settled.transferscheme GetModel(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  top 1 id,schemename,minamtlimitofeach,maxamtlimitofeach,dailymaxtimes,dailymaxamt,monthmaxtimes,monthmaxamt,chargerate,chargeleastofeach,chargemostofeach,isdefault from transferscheme ");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            KuaiCard.Model.Settled.transferscheme transferscheme = new KuaiCard.Model.Settled.transferscheme();
            DataSet set = DbHelperSQL.Query(builder.ToString(), cmdParms);
            if (set.Tables[0].Rows.Count > 0)
            {
                return this.DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public int GetRecordCount(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM transferscheme ");
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

        public decimal GetUserMonthTotalAmt(int userid)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@year", SqlDbType.Int, 10), new SqlParameter("@month", SqlDbType.Int, 10), new SqlParameter("@userid", SqlDbType.Int, 10) };
            commandParameters[0].Value = DateTime.Now.Month;
            commandParameters[1].Value = DateTime.Now.Year;
            commandParameters[2].Value = userid;
            object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_transfer_getusermonthtotalamt", commandParameters);
            if (obj2 == DBNull.Value)
            {
                return 0M;
            }
            return Convert.ToDecimal(obj2);
        }

        public bool Update(KuaiCard.Model.Settled.transferscheme model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update transferscheme set ");
            builder.Append("schemename=@schemename,");
            builder.Append("minamtlimitofeach=@minamtlimitofeach,");
            builder.Append("maxamtlimitofeach=@maxamtlimitofeach,");
            builder.Append("dailymaxtimes=@dailymaxtimes,");
            builder.Append("dailymaxamt=@dailymaxamt,");
            builder.Append("monthmaxtimes=@monthmaxtimes,");
            builder.Append("monthmaxamt=@monthmaxamt,");
            builder.Append("chargerate=@chargerate,");
            builder.Append("chargeleastofeach=@chargeleastofeach,");
            builder.Append("chargemostofeach=@chargemostofeach,");
            builder.Append("isdefault=@isdefault");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@schemename", SqlDbType.NVarChar, 50), new SqlParameter("@minamtlimitofeach", SqlDbType.Decimal, 9), new SqlParameter("@maxamtlimitofeach", SqlDbType.Decimal, 9), new SqlParameter("@dailymaxtimes", SqlDbType.Int, 10), new SqlParameter("@dailymaxamt", SqlDbType.Decimal, 9), new SqlParameter("@monthmaxtimes", SqlDbType.Int, 10), new SqlParameter("@monthmaxamt", SqlDbType.Decimal, 9), new SqlParameter("@chargerate", SqlDbType.Decimal, 9), new SqlParameter("@chargeleastofeach", SqlDbType.Decimal, 9), new SqlParameter("@chargemostofeach", SqlDbType.Decimal, 9), new SqlParameter("@isdefault", SqlDbType.TinyInt, 1), new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = model.schemename;
            cmdParms[1].Value = model.minamtlimitofeach;
            cmdParms[2].Value = model.maxamtlimitofeach;
            cmdParms[3].Value = model.dailymaxtimes;
            cmdParms[4].Value = model.dailymaxamt;
            cmdParms[5].Value = model.monthmaxtimes;
            cmdParms[6].Value = model.monthmaxamt;
            cmdParms[7].Value = model.chargerate;
            cmdParms[8].Value = model.chargeleastofeach;
            cmdParms[9].Value = model.chargemostofeach;
            cmdParms[10].Value = model.isdefault;
            cmdParms[11].Value = model.id;
            return (DbHelperSQL.ExecuteSql(builder.ToString(), cmdParms) > 0);
        }
    }
}

