namespace OriginalStudio.BLL.Settled
{
    using DBAccess;
    using OriginalStudio.Model.User;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class userspaybank
    {
        public int Add(OriginalStudio.Model.User.userspaybank model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into userspaybank(");
            builder.Append("userid,accoutType,pmode,account,payeeName,BankCode,payeeBank,provinceCode,bankProvince,cityCode,bankCity,bankAddress,status,AddTime,updateTime)");
            builder.Append(" values (");
            builder.Append("@userid,@accoutType,@pmode,@account,@payeeName,@BankCode,@payeeBank,@provinceCode,@bankProvince,@cityCode,@bankCity,@bankAddress,@status,@AddTime,@updateTime)");
            builder.Append(";select @@IDENTITY");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@accoutType", SqlDbType.TinyInt, 1), new SqlParameter("@pmode", SqlDbType.TinyInt, 1), new SqlParameter("@account", SqlDbType.VarChar, 50), new SqlParameter("@payeeName", SqlDbType.VarChar, 50), new SqlParameter("@BankCode", SqlDbType.VarChar, 50), new SqlParameter("@payeeBank", SqlDbType.VarChar, 50), new SqlParameter("@provinceCode", SqlDbType.VarChar, 50), new SqlParameter("@bankProvince", SqlDbType.VarChar, 50), new SqlParameter("@cityCode", SqlDbType.VarChar, 50), new SqlParameter("@bankCity", SqlDbType.VarChar, 50), new SqlParameter("@bankAddress", SqlDbType.VarChar, 100), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@AddTime", SqlDbType.DateTime), new SqlParameter("@updateTime", SqlDbType.DateTime) };
            cmdParms[0].Value = model.userid;
            cmdParms[1].Value = model.accoutType;
            cmdParms[2].Value = model.pmode;
            cmdParms[3].Value = model.account;
            cmdParms[4].Value = model.payeeName;
            cmdParms[5].Value = model.BankCode;
            cmdParms[6].Value = model.payeeBank;
            cmdParms[7].Value = model.provinceCode;
            cmdParms[8].Value = model.bankProvince;
            cmdParms[9].Value = model.cityCode;
            cmdParms[10].Value = model.bankCity;
            cmdParms[11].Value = model.bankAddress;
            cmdParms[12].Value = model.status;
            cmdParms[13].Value = model.AddTime;
            cmdParms[14].Value = model.updateTime;
            object single = DataBase.ExecuteScalar(CommandType.Text, builder.ToString(), cmdParms);
            if (single == null)
            {
                return 0;
            }
            return Convert.ToInt32(single);
        }

        public OriginalStudio.Model.User.userspaybank DataRowToModel(DataRow row)
        {
            OriginalStudio.Model.User.userspaybank userspaybank = new OriginalStudio.Model.User.userspaybank();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    userspaybank.id = int.Parse(row["id"].ToString());
                }
                if ((row["userid"] != null) && (row["userid"].ToString() != ""))
                {
                    userspaybank.userid = int.Parse(row["userid"].ToString());
                }
                if ((row["accoutType"] != null) && (row["accoutType"].ToString() != ""))
                {
                    userspaybank.accoutType = int.Parse(row["accoutType"].ToString());
                }
                if ((row["pmode"] != null) && (row["pmode"].ToString() != ""))
                {
                    userspaybank.pmode = int.Parse(row["pmode"].ToString());
                }
                if (row["account"] != null)
                {
                    userspaybank.account = row["account"].ToString();
                }
                if (row["payeeName"] != null)
                {
                    userspaybank.payeeName = row["payeeName"].ToString();
                }
                if (row["BankCode"] != null)
                {
                    userspaybank.BankCode = row["BankCode"].ToString();
                }
                if (row["payeeBank"] != null)
                {
                    userspaybank.payeeBank = row["payeeBank"].ToString();
                }
                if (row["provinceCode"] != null)
                {
                    userspaybank.provinceCode = row["provinceCode"].ToString();
                }
                if (row["bankProvince"] != null)
                {
                    userspaybank.bankProvince = row["bankProvince"].ToString();
                }
                if (row["cityCode"] != null)
                {
                    userspaybank.cityCode = row["cityCode"].ToString();
                }
                if (row["bankCity"] != null)
                {
                    userspaybank.bankCity = row["bankCity"].ToString();
                }
                if (row["bankAddress"] != null)
                {
                    userspaybank.bankAddress = row["bankAddress"].ToString();
                }
                if ((row["status"] != null) && (row["status"].ToString() != ""))
                {
                    userspaybank.status = new int?(int.Parse(row["status"].ToString()));
                }
                if ((row["AddTime"] != null) && (row["AddTime"].ToString() != ""))
                {
                    userspaybank.AddTime = DateTime.Parse(row["AddTime"].ToString());
                }
                if ((row["updateTime"] != null) && (row["updateTime"].ToString() != ""))
                {
                    userspaybank.updateTime = new DateTime?(DateTime.Parse(row["updateTime"].ToString()));
                }
            }
            return userspaybank;
        }

        public bool Delete(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from userspaybank ");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            return DataBase.ExecuteNonQuery(builder.ToString(), cmdParms) > 0;
        }

        public bool DeleteList(string idlist)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from userspaybank ");
            builder.Append(" where id in (" + idlist + ")  ");
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString()) > 0;
        }

        public bool Exists(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) from userspaybank");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = id;
            return DataBase.ExecuteScalar(builder.ToString(), cmdParms).ToString() != "0";
        }

        public static string GetAccoutTypeName(object obj)
        {
            if (obj == DBNull.Value)
            {
                return string.Empty;
            }
            string str = string.Empty;
            switch (Convert.ToInt32(obj))
            {
                case 0:
                    return "私";

                case 1:
                    return "公";
            }
            return str;
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,userid,accoutType,pmode,account,payeeName,BankCode,payeeBank,provinceCode,bankProvince,cityCode,bankCity,bankAddress,status,AddTime,updateTime ");
            builder.Append(" FROM userspaybank ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ");
            if (Top > 0)
            {
                builder.Append(" top " + Top.ToString());
            }
            builder.Append(" id,userid,accoutType,pmode,account,payeeName,BankCode,payeeBank,provinceCode,bankProvince,cityCode,bankCity,bankAddress,status,AddTime,updateTime ");
            builder.Append(" FROM userspaybank ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            builder.Append(" order by " + filedOrder);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
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
            builder.Append(")AS Row, T.*  from userspaybank T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public OriginalStudio.Model.User.userspaybank GetModel(int userid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  top 1 id,userid,accoutType,pmode,account,payeeName,BankCode,payeeBank,provinceCode,bankProvince,cityCode,bankCity,bankAddress,status,AddTime,updateTime from userspaybank ");
            builder.Append(" where userid=@userid");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10) };
            cmdParms[0].Value = userid;
            OriginalStudio.Model.User.userspaybank userspaybank = new OriginalStudio.Model.User.userspaybank();
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
            builder.Append("select count(1) FROM userspaybank ");
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            object single = DataBase.ExecuteScalar(CommandType.Text, builder.ToString());
            if (single == null)
            {
                return 0;
            }
            return Convert.ToInt32(single);
        }

        public static string GetSettleModeName(object obj)
        {
            if (obj == DBNull.Value)
            {
                return string.Empty;
            }
            string str = string.Empty;
            switch (Convert.ToInt32(obj))
            {
                case 1:
                    return "银行帐户";

                case 2:
                    return "支付宝";

                case 3:
                    return "财付通";
            }
            return str;
        }

        public bool Update(OriginalStudio.Model.User.userspaybank model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update userspaybank set ");
            builder.Append("userid=@userid,");
            builder.Append("accoutType=@accoutType,");
            builder.Append("pmode=@pmode,");
            builder.Append("account=@account,");
            builder.Append("payeeName=@payeeName,");
            builder.Append("BankCode=@BankCode,");
            builder.Append("payeeBank=@payeeBank,");
            builder.Append("provinceCode=@provinceCode,");
            builder.Append("bankProvince=@bankProvince,");
            builder.Append("cityCode=@cityCode,");
            builder.Append("bankCity=@bankCity,");
            builder.Append("bankAddress=@bankAddress,");
            builder.Append("status=@status,");
            builder.Append("AddTime=@AddTime,");
            builder.Append("updateTime=@updateTime");
            builder.Append(" where id=@id");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.Int, 10), new SqlParameter("@accoutType", SqlDbType.TinyInt, 1), new SqlParameter("@pmode", SqlDbType.TinyInt, 1), new SqlParameter("@account", SqlDbType.VarChar, 50), new SqlParameter("@payeeName", SqlDbType.VarChar, 50), new SqlParameter("@BankCode", SqlDbType.VarChar, 50), new SqlParameter("@payeeBank", SqlDbType.VarChar, 50), new SqlParameter("@provinceCode", SqlDbType.VarChar, 50), new SqlParameter("@bankProvince", SqlDbType.VarChar, 50), new SqlParameter("@cityCode", SqlDbType.VarChar, 50), new SqlParameter("@bankCity", SqlDbType.VarChar, 50), new SqlParameter("@bankAddress", SqlDbType.VarChar, 100), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@AddTime", SqlDbType.DateTime), new SqlParameter("@updateTime", SqlDbType.DateTime), new SqlParameter("@id", SqlDbType.Int, 10) };
            cmdParms[0].Value = model.userid;
            cmdParms[1].Value = model.accoutType;
            cmdParms[2].Value = model.pmode;
            cmdParms[3].Value = model.account;
            cmdParms[4].Value = model.payeeName;
            cmdParms[5].Value = model.BankCode;
            cmdParms[6].Value = model.payeeBank;
            cmdParms[7].Value = model.provinceCode;
            cmdParms[8].Value = model.bankProvince;
            cmdParms[9].Value = model.cityCode;
            cmdParms[10].Value = model.bankCity;
            cmdParms[11].Value = model.bankAddress;
            cmdParms[12].Value = model.status;
            cmdParms[13].Value = model.AddTime;
            cmdParms[14].Value = model.updateTime;
            cmdParms[15].Value = model.id;
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), cmdParms) > 0;
        }
    }
}

