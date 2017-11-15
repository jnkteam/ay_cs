namespace OriginalStudio.DAL.Withdraw
{
    using DBAccess;
    using OriginalStudio.Model.Withdraw;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using OriginalStudio.Lib.Logging;

    public class channelwithdraw
    {
        public int Add(OriginalStudio.Model.Withdraw.channelwithdraw model)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10), new SqlParameter("@bankCode", SqlDbType.VarChar, 10), new SqlParameter("@bankName", SqlDbType.VarChar, 30), new SqlParameter("@supplier", SqlDbType.Int, 10), new SqlParameter("@sort", SqlDbType.Int, 10) };
            parameterArray[0].Direction = ParameterDirection.Output;
            parameterArray[1].Value = model.bankCode;
            parameterArray[2].Value = model.bankName;
            parameterArray[3].Value = model.supplier;
            parameterArray[4].Value = model.sort;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channelwithdraw_ADD", parameterArray);
            return (int) parameterArray[0].Value;
        }

        public OriginalStudio.Model.Withdraw.channelwithdraw DataRowToModel(DataRow row)
        {
            OriginalStudio.Model.Withdraw.channelwithdraw channelwithdraw = new OriginalStudio.Model.Withdraw.channelwithdraw();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    channelwithdraw.id = int.Parse(row["id"].ToString());
                }
                if (row["bankCode"] != null)
                {
                    channelwithdraw.bankCode = row["bankCode"].ToString();
                }
                if (row["bankName"] != null)
                {
                    channelwithdraw.bankName = row["bankName"].ToString();
                }
                if ((row["supplier"] != null) && (row["supplier"].ToString() != ""))
                {
                    channelwithdraw.supplier = int.Parse(row["supplier"].ToString());
                }
                if ((row["sort"] != null) && (row["sort"].ToString() != ""))
                {
                    channelwithdraw.sort = new int?(int.Parse(row["sort"].ToString()));
                }
                if ((row["bankEnName"] != null) && (row["bankEnName"].ToString() != ""))
                {
                    channelwithdraw.bankEnName = row["bankEnName"].ToString();
                }
            }
            
            return channelwithdraw;
        }

        public bool Delete(int id)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            parameterArray[0].Value = id;
            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channelwithdraw_ADD", parameterArray) > 0;
        }

        public bool DeleteList(string idlist)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from channelwithdraw ");
            builder.Append(" where id in (" + idlist + ")  ");
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString()) > 0;
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,bankCode,bankName,supplier,sort,bankEnName");
            builder.Append(" FROM channelwithdraw ");
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
            builder.Append(" id,bankCode,bankName,supplier,sort ");
            builder.Append(" FROM channelwithdraw ");
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
            builder.Append(")AS Row, T.*  from channelwithdraw T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public OriginalStudio.Model.Withdraw.channelwithdraw GetModel(int id)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            parameterArray[0].Value = id;
            OriginalStudio.Model.Withdraw.channelwithdraw channelwithdraw = new OriginalStudio.Model.Withdraw.channelwithdraw();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channelwithdraw_GetModel", parameterArray);
            if (set.Tables[0].Rows.Count > 0)
            {
                return this.DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public OriginalStudio.Model.Withdraw.channelwithdraw GetModelByBankName(string bankName)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@bankName", SqlDbType.VarChar, 30) };
            parameterArray[0].Value = bankName;
            OriginalStudio.Model.Withdraw.channelwithdraw channelwithdraw = new OriginalStudio.Model.Withdraw.channelwithdraw();

            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channelwithdraw_GetModelBybankName", parameterArray);
            if (set.Tables[0].Rows.Count > 0)
            {
                return this.DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }


        public OriginalStudio.Model.Withdraw.channelwithdraw GetModelByBankCode(string bankCode)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { 
                new SqlParameter("@bankCode", SqlDbType.VarChar, 30) 
            };
            parameterArray[0].Value = bankCode;
            OriginalStudio.Model.Withdraw.channelwithdraw channelwithdraw = new OriginalStudio.Model.Withdraw.channelwithdraw();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_channelwithdraw_GetModelByBackCode", parameterArray);
            
            if (set.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public int GetRecordCount(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM channelwithdraw ");
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

        public int GetSupplier(string bankCode)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@bankCode", SqlDbType.VarChar, 10) };
            commandParameters[0].Value = bankCode;
            object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_channelwithdraw_GetSup", commandParameters);
            if (obj2 != DBNull.Value)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public bool Update(OriginalStudio.Model.Withdraw.channelwithdraw model)
        {
            int rowsAffected = 0;
            SqlParameter[] parameterArray = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10), 
                new SqlParameter("@bankCode", SqlDbType.VarChar, 10),
                new SqlParameter("@bankName", SqlDbType.VarChar, 30), 
                new SqlParameter("@supplier", SqlDbType.Int, 10), 
                new SqlParameter("@sort", SqlDbType.Int, 10) };
            parameterArray[0].Value = model.id;
            parameterArray[1].Value = model.bankCode;
            parameterArray[2].Value = model.bankName;
            parameterArray[3].Value = model.supplier;
            parameterArray[4].Value = model.sort;
            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channelwithdraw_Update", parameterArray) > 0;
        }
    }
}

