namespace KuaiCard.BLL.Financial
{
    using DBAccess;
    using KuaiCardLib.Data;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class tenpay_batch_trans_detail
    {
        internal string SQL_TABLE = "tenpay_batch_trans_detail";
        internal string SQL_TABLE_FIELD = "[id]\r\n      ,[package_id]\r\n      ,[serial]\r\n      ,[settleid]\r\n      ,[hid]\r\n      ,[userid]\r\n      ,[balance]\r\n      ,[status]\r\n      ,[rec_acc]\r\n      ,[rec_name]\r\n      ,[cur_type]\r\n      ,[pay_amt]\r\n      ,[succ_amt]\r\n      ,[remark]\r\n      ,[trans_id]\r\n      ,[message]\r\n      ,[completetime]";

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

                        case "rec_acc":
                            builder.Append(" AND [rec_acc] like @rec_acc");
                            parameter = new SqlParameter("@rec_acc", SqlDbType.VarChar, 20);
                            parameter.Value = "%" + SqlHelper.CleanString((string) param2.ParamValue, 20) + "%";
                            paramList.Add(parameter);
                            break;

                        case "stime":
                            builder.Append(" AND [completetime] >= @stime");
                            parameter = new SqlParameter("@stime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;

                        case "etime":
                            builder.Append(" AND [completetime] <= @etime");
                            parameter = new SqlParameter("@etime", SqlDbType.DateTime);
                            parameter.Value = param2.ParamValue;
                            paramList.Add(parameter);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public int Complete(string package_id, int serial, int status, decimal succ_amt, string message, string trans_id)
        {
            try
            {
                SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@package_id", SqlDbType.VarChar, 20), new SqlParameter("@serial", SqlDbType.Int, 10), new SqlParameter("@status", SqlDbType.TinyInt, 1), new SqlParameter("@succ_amt", SqlDbType.Decimal, 9), new SqlParameter("@trans_id", SqlDbType.VarChar, 50), new SqlParameter("@message", SqlDbType.VarChar, 50), new SqlParameter("@completetime", SqlDbType.DateTime, 8) };
                commandParameters[0].Value = package_id;
                commandParameters[1].Value = serial;
                commandParameters[2].Value = status;
                commandParameters[3].Value = succ_amt;
                commandParameters[4].Value = trans_id;
                commandParameters[5].Value = message;
                commandParameters[6].Value = DateTime.Now;
                object obj2 = DataBase.ExecuteScalar(CommandType.StoredProcedure, "proc_tenpay_batch_trans_detail_complete", commandParameters);
                if (obj2 != DBNull.Value)
                {
                    return Convert.ToInt32(obj2);
                }
                return 0;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public string GetStatusText(object _status)
        {
            if ((_status == null) || (_status == DBNull.Value))
            {
                return string.Empty;
            }
            int num = Convert.ToInt32(_status);
            switch (num)
            {
                case 1:
                    return "处理中";

                case 2:
                    return "已成功";

                case 4:
                    return "失败";

                case 8:
                    return "未确定状态";
            }
            return num.ToString();
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
    }
}

