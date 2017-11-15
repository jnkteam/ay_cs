namespace KuaiCard.BLL.order
{
    using DBAccess;
    using KuaiCard.Model.order;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class reconciliation_temp
    {
        public int Add(KuaiCard.Model.order.reconciliation_temp model)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into reconciliation_temp(");
                builder.Append("serverid,orderid,count)");
                builder.Append(" values (");
                builder.Append("@serverid,@orderid,@count)");
                builder.Append(";select @@IDENTITY");
                SqlParameter[] commandParameters = new SqlParameter[] { 
                    new SqlParameter("@serverid", SqlDbType.VarChar, 50), 
                    new SqlParameter("@orderid", SqlDbType.VarChar, 30), 
                    new SqlParameter("@count", SqlDbType.Int, 10) };
                commandParameters[0].Value = model.serverid;
                commandParameters[1].Value = model.orderid;
                commandParameters[2].Value = model.count;
                object obj2 = DataBase.ExecuteScalar(CommandType.Text, builder.ToString(), commandParameters);
                if (obj2 == null)
                {
                    return 0;
                }
                return Convert.ToInt32(obj2);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public KuaiCard.Model.order.reconciliation_temp DataRowToModel(DataRow row)
        {
            KuaiCard.Model.order.reconciliation_temp _temp = new KuaiCard.Model.order.reconciliation_temp();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    _temp.id = int.Parse(row["id"].ToString());
                }
                if (row["serverid"] != null)
                {
                    _temp.serverid = row["serverid"].ToString();
                }
                if (row["orderid"] != null)
                {
                    _temp.orderid = row["orderid"].ToString();
                }
                if ((row["count"] != null) && (row["count"].ToString() != ""))
                {
                    _temp.count = new int?(int.Parse(row["count"].ToString()));
                }
            }
            return _temp;
        }

        public bool Delete(string orderid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from reconciliation_temp ");
            builder.Append(" where orderid=@orderid ");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30) };
            commandParameters[0].Value = orderid;
            return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
        }

        public DataSet GetList(string strWhere)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select id,serverid,orderid,count ");
                builder.Append(" FROM reconciliation_temp ");
                if (strWhere.Trim() != "")
                {
                    builder.Append(" where " + strWhere);
                }
                return DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), null);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public KuaiCard.Model.order.reconciliation_temp GetModel(int id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select  top 1 id,serverid,orderid,count from reconciliation_temp ");
            builder.Append(" where id=@id");
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            commandParameters[0].Value = id;
            KuaiCard.Model.order.reconciliation_temp _temp = new KuaiCard.Model.order.reconciliation_temp();
            DataSet set = DataBase.ExecuteDataset(CommandType.Text, builder.ToString(), commandParameters);
            if (set.Tables[0].Rows.Count > 0)
            {
                return this.DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public bool Update(string orderid, string callback)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("update reconciliation_temp set ");
                builder.Append("count=count+1,callback=@callback");
                builder.Append(" where orderid=@orderid");
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@orderid", SqlDbType.VarChar, 30), new SqlParameter("@callback", SqlDbType.VarChar, 0x7d0) };
                commandParameters[0].Value = orderid;
                commandParameters[1].Value = callback;
                return (DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), commandParameters) > 0);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

