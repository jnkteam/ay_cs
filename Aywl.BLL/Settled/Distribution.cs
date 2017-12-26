namespace OriginalStudio.BLL.Settled
{
    using OriginalStudio.Cache;
    using OriginalStudio.DAL;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.DBAccess;
    using System.Data.SqlClient;
    using System.Text;

    /// <summary>
    /// 交易记录
    /// </summary>
    public class Distribution
    {
        internal string SQL_TABLE = "distribution";
        internal string SQL_TABLE_FIELD = "[id]\r\n      ,[suppid]\r\n      ,[mode]\r\n      ,[settledId]\r\n      ,[trade_no]\r\n      ,[batchNo]\r\n      ,[supp_trade_no]\r\n      ,[userid]\r\n      ,[balance]\r\n      ,[bankCode]\r\n      ,[bankName]\r\n      ,[bankBranch]\r\n      ,[bankAccountName]\r\n      ,[bankAccount]\r\n      ,[amount]\r\n      ,[charges]\r\n      ,[balance2]\r\n      ,[addTime]\r\n      ,[processingTime]\r\n      ,[supp_message]\r\n      ,[status]\r\n      ,[ext1]\r\n      ,[ext2]\r\n      ,[ext3]\r\n      ,[remark]\r\n      ,isnull(amount,0)+isnull(charges,0) as realpay";

        #region 处理交易记录

        /// <summary>
        /// 处理交易记录
        /// </summary>
        /// <param name="suppId"></param>
        /// <param name="trade_no"></param>
        /// <param name="is_cancel"></param>
        /// <param name="status"></param>
        /// <param name="amount"></param>
        /// <param name="supp_trade_no"></param>
        /// <param name="message"></param>
        /// <param name="bill_trade_no"></param>
        /// <returns>返回0：代表成功</returns>
        public static int Process(int suppId, string trade_no, bool is_cancel, int status, string amount, string supp_trade_no, string message, out string bill_trade_no)
        {
            bill_trade_no = string.Empty;
            SqlParameter[] commandParameters = new SqlParameter[] { 
                new SqlParameter("@suppid", SqlDbType.Int, 10), 
                new SqlParameter("@trade_no", SqlDbType.VarChar, 30), 
                new SqlParameter("@supp_trade_no", SqlDbType.VarChar, 50), 
                new SqlParameter("@is_cancel", SqlDbType.Bit, 1), 
                new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                new SqlParameter("@amount", SqlDbType.Decimal, 9), 
                new SqlParameter("@processingTime", SqlDbType.DateTime, 8), 
                new SqlParameter("@supp_message", SqlDbType.NVarChar, 200)
            };
            commandParameters[0].Value = suppId;
            commandParameters[1].Value = trade_no;
            commandParameters[2].Value = supp_trade_no;
            commandParameters[3].Value = is_cancel ? 1 : 0;
            commandParameters[4].Value = status;
            commandParameters[5].Value = decimal.Parse(amount);
            commandParameters[6].Value = DateTime.Now;
            commandParameters[7].Value = message;
            DataTable table = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_distribution_process", commandParameters).Tables[0];
            if ((table == null) || (table.Rows.Count <= 0))
            {
                return -1;
            }
            DataRow row = table.Rows[0];
            bill_trade_no = Convert.ToString(row["bill_trade_no"]);
            return Convert.ToInt32(row["result"]);  
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 增加交易记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(OriginalStudio.Model.Settled.Distribution model)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10), 
                new SqlParameter("@suppid", SqlDbType.Int, 10), 
                new SqlParameter("@mode", SqlDbType.TinyInt, 1), 
                new SqlParameter("@settledId", SqlDbType.Int, 10), 
                new SqlParameter("@trade_no", SqlDbType.VarChar, 30), 
                new SqlParameter("@batchNo", SqlDbType.Int, 10), 
                new SqlParameter("@supp_trade_no", SqlDbType.VarChar, 50), 
                new SqlParameter("@userid", SqlDbType.Int, 10), 
                new SqlParameter("@balance", SqlDbType.Decimal, 9), 
                new SqlParameter("@bankCode", SqlDbType.VarChar, 50), 
                new SqlParameter("@bankName", SqlDbType.NVarChar, 200), 
                new SqlParameter("@bankBranch", SqlDbType.NVarChar, 0xff), 
                new SqlParameter("@bankAccountName", SqlDbType.NVarChar, 20), 
                new SqlParameter("@bankAccount", SqlDbType.VarChar, 50), 
                new SqlParameter("@amount", SqlDbType.Decimal, 9), 
                new SqlParameter("@charges", SqlDbType.Decimal, 9), 
                new SqlParameter("@balance2", SqlDbType.Decimal, 9), 
                new SqlParameter("@addTime", SqlDbType.DateTime), 
                new SqlParameter("@processingTime", SqlDbType.DateTime), 
                new SqlParameter("@supp_message", SqlDbType.NVarChar, 200), 
                new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                new SqlParameter("@ext1", SqlDbType.VarChar, 50), 
                new SqlParameter("@ext2", SqlDbType.VarChar, 50), 
                new SqlParameter("@ext3", SqlDbType.VarChar, 50), 
                new SqlParameter("@remark", SqlDbType.NVarChar, 500)
             };
            parameterArray[0].Direction = ParameterDirection.Output;
            parameterArray[1].Value = model.suppid;
            parameterArray[2].Value = model.mode;
            parameterArray[3].Value = model.settledId;
            parameterArray[4].Value = model.trade_no;
            parameterArray[5].Value = model.batchNo;
            parameterArray[6].Value = model.supp_trade_no;
            parameterArray[7].Value = model.userid;
            parameterArray[8].Value = model.balance;
            parameterArray[9].Value = model.bankCode;
            parameterArray[10].Value = model.bankName;
            parameterArray[11].Value = model.bankBranch;
            parameterArray[12].Value = model.bankAccountName;
            parameterArray[13].Value = model.bankAccount;
            parameterArray[14].Value = model.amount;
            parameterArray[15].Value = model.charges;
            parameterArray[16].Value = model.balance2;
            parameterArray[17].Value = model.addTime;
            parameterArray[18].Value = model.processingTime;
            parameterArray[19].Value = model.supp_message;
            parameterArray[20].Value = model.status;
            parameterArray[21].Value = model.ext1;
            parameterArray[22].Value = model.ext2;
            parameterArray[23].Value = model.ext3;
            parameterArray[24].Value = model.remark;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_distribution_ADD", parameterArray);
            return (int)parameterArray[0].Value;
        }
        
        public bool Delete(int id)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 10) };
            parameterArray[0].Value = id;

            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_distribution_Delete", parameterArray) > 0;
        }

        public bool Delete(string trade_no)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from distribution ");
            builder.Append(" where trade_no=@trade_no ");
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@trade_no", SqlDbType.VarChar, 30) };
            cmdParms[0].Value = trade_no;
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString(), cmdParms) > 0;
        }

        public bool DeleteList(string idlist)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from distribution ");
            builder.Append(" where id in (" + idlist + ")  ");
            return DataBase.ExecuteNonQuery(CommandType.Text, builder.ToString()) > 0;
        }

        public static bool Exists(string trade_no)
        {
            SqlParameter[] parameterArray = new SqlParameter[] 
            { 
                new SqlParameter("@trade_no", SqlDbType.VarChar, 30) 
            };
            parameterArray[0].Value = trade_no;
            return DataBase.ExecuteScalarToStr(CommandType.StoredProcedure, "proc_distribution_Exists", parameterArray) == "1";
        }

        public static bool Update(OriginalStudio.Model.Settled.Distribution model)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { 
                new SqlParameter("@id", SqlDbType.Int, 10), 
                new SqlParameter("@suppid", SqlDbType.Int, 10), 
                new SqlParameter("@mode", SqlDbType.TinyInt, 1), 
                new SqlParameter("@settledId", SqlDbType.Int, 10), 
                new SqlParameter("@trade_no", SqlDbType.VarChar, 30), 
                new SqlParameter("@batchNo", SqlDbType.Int, 10), 
                new SqlParameter("@supp_trade_no", SqlDbType.VarChar, 50), 
                new SqlParameter("@userid", SqlDbType.Int, 10), 
                new SqlParameter("@balance", SqlDbType.Decimal, 9), 
                new SqlParameter("@bankCode", SqlDbType.VarChar, 50), 
                new SqlParameter("@bankName", SqlDbType.NVarChar, 20), 
                new SqlParameter("@bankBranch", SqlDbType.NVarChar, 0xff), 
                new SqlParameter("@bankAccountName", SqlDbType.NVarChar, 20), 
                new SqlParameter("@bankAccount", SqlDbType.VarChar, 50), 
                new SqlParameter("@amount", SqlDbType.Decimal, 9), 
                new SqlParameter("@charges", SqlDbType.Decimal, 9), 
                new SqlParameter("@balance2", SqlDbType.Decimal, 9), 
                new SqlParameter("@addTime", SqlDbType.DateTime), 
                new SqlParameter("@processingTime", SqlDbType.DateTime), 
                new SqlParameter("@supp_message", SqlDbType.NVarChar, 200), 
                new SqlParameter("@status", SqlDbType.TinyInt, 1), 
                new SqlParameter("@ext1", SqlDbType.VarChar, 50), 
                new SqlParameter("@ext2", SqlDbType.VarChar, 50), 
                new SqlParameter("@ext3", SqlDbType.VarChar, 50), 
                new SqlParameter("@remark", SqlDbType.NVarChar, 500)
             };
            parameterArray[0].Value = model.id;
            parameterArray[1].Value = model.suppid;
            parameterArray[2].Value = model.mode;
            parameterArray[3].Value = model.settledId;
            parameterArray[4].Value = model.trade_no;
            parameterArray[5].Value = model.batchNo;
            parameterArray[6].Value = model.supp_trade_no;
            parameterArray[7].Value = model.userid;
            parameterArray[8].Value = model.balance;
            parameterArray[9].Value = model.bankCode;
            parameterArray[10].Value = model.bankName;
            parameterArray[11].Value = model.bankBranch;
            parameterArray[12].Value = model.bankAccountName;
            parameterArray[13].Value = model.bankAccount;
            parameterArray[14].Value = model.amount;
            parameterArray[15].Value = model.charges;
            parameterArray[0x10].Value = model.balance2;
            parameterArray[0x11].Value = model.addTime;
            parameterArray[0x12].Value = model.processingTime;
            parameterArray[0x13].Value = model.supp_message;
            parameterArray[20].Value = model.status;
            parameterArray[0x15].Value = model.ext1;
            parameterArray[0x16].Value = model.ext2;
            parameterArray[0x17].Value = model.ext3;
            parameterArray[0x18].Value = model.remark;
            return DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_distribution_Update", parameterArray) > 0;
        }

        #endregion

        #region 获取对象

        /// <summary>
        /// DataTable转为对象集合。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<OriginalStudio.Model.Settled.Distribution> DataTableToList(DataTable dt)
        {
            try
            {
                List<OriginalStudio.Model.Settled.Distribution> list = new List<OriginalStudio.Model.Settled.Distribution>();
                int count = dt.Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        OriginalStudio.Model.Settled.Distribution item = this.DataRowToModel(dt.Rows[i]);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        /// <summary>
        /// DataRow转为对象。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public OriginalStudio.Model.Settled.Distribution DataRowToModel(DataRow row)
        {
            OriginalStudio.Model.Settled.Distribution distribution = new OriginalStudio.Model.Settled.Distribution();
            if (row != null)
            {
                if ((row["id"] != null) && (row["id"].ToString() != ""))
                {
                    distribution.id = int.Parse(row["id"].ToString());
                }
                if ((row["suppid"] != null) && (row["suppid"].ToString() != ""))
                {
                    distribution.suppid = int.Parse(row["suppid"].ToString());
                }
                if ((row["mode"] != null) && (row["mode"].ToString() != ""))
                {
                    distribution.mode = new int?(int.Parse(row["mode"].ToString()));
                }
                if ((row["settledId"] != null) && (row["settledId"].ToString() != ""))
                {
                    distribution.settledId = new int?(int.Parse(row["settledId"].ToString()));
                }
                if (row["trade_no"] != null)
                {
                    distribution.trade_no = row["trade_no"].ToString();
                }
                if ((row["batchNo"] != null) && (row["batchNo"].ToString() != ""))
                {
                    distribution.batchNo = int.Parse(row["batchNo"].ToString());
                }
                if (row["supp_trade_no"] != null)
                {
                    distribution.supp_trade_no = row["supp_trade_no"].ToString();
                }
                if ((row["userid"] != null) && (row["userid"].ToString() != ""))
                {
                    distribution.userid = int.Parse(row["userid"].ToString());
                }
                if ((row["balance"] != null) && (row["balance"].ToString() != ""))
                {
                    distribution.balance = decimal.Parse(row["balance"].ToString());
                }
                if (row["bankCode"] != null)
                {
                    distribution.bankCode = row["bankCode"].ToString();
                }
                if (row["bankName"] != null)
                {
                    distribution.bankName = row["bankName"].ToString();
                }
                if (row["bankBranch"] != null)
                {
                    distribution.bankBranch = row["bankBranch"].ToString();
                }
                if (row["bankAccountName"] != null)
                {
                    distribution.bankAccountName = row["bankAccountName"].ToString();
                }
                if (row["bankAccount"] != null)
                {
                    distribution.bankAccount = row["bankAccount"].ToString();
                }
                if ((row["amount"] != null) && (row["amount"].ToString() != ""))
                {
                    distribution.amount = decimal.Parse(row["amount"].ToString());
                }
                if ((row["charges"] != null) && (row["charges"].ToString() != ""))
                {
                    distribution.charges = decimal.Parse(row["charges"].ToString());
                }
                if ((row["balance2"] != null) && (row["balance2"].ToString() != ""))
                {
                    distribution.balance2 = new decimal?(decimal.Parse(row["balance2"].ToString()));
                }
                if ((row["addTime"] != null) && (row["addTime"].ToString() != ""))
                {
                    distribution.addTime = DateTime.Parse(row["addTime"].ToString());
                }
                if ((row["processingTime"] != null) && (row["processingTime"].ToString() != ""))
                {
                    distribution.processingTime = DateTime.Parse(row["processingTime"].ToString());
                }
                if (row["supp_message"] != null)
                {
                    distribution.supp_message = row["supp_message"].ToString();
                }
                if ((row["status"] != null) && (row["status"].ToString() != ""))
                {
                    distribution.status = int.Parse(row["status"].ToString());
                }
                if (row["ext1"] != null)
                {
                    distribution.ext1 = row["ext1"].ToString();
                }
                if (row["ext2"] != null)
                {
                    distribution.ext2 = row["ext2"].ToString();
                }
                if (row["ext3"] != null)
                {
                    distribution.ext3 = row["ext3"].ToString();
                }
                if (row["remark"] != null)
                {
                    distribution.remark = row["remark"].ToString();
                }
            }
            return distribution;
        }
        
        public OriginalStudio.Model.Settled.Distribution GetDistributionModel(int id)
        {
            SqlParameter[] parameterArray = new SqlParameter[] 
            { 
                new SqlParameter("@id", SqlDbType.Int, 10) 
            };
            parameterArray[0].Value = id;
            OriginalStudio.Model.Settled.Distribution distribution = new OriginalStudio.Model.Settled.Distribution();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_distribution_GetModel", parameterArray);
            if (set.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        public OriginalStudio.Model.Settled.Distribution GetModelByNo(string trade_no)
        {
            SqlParameter[] parameterArray = new SqlParameter[] 
            { 
                new SqlParameter("@trade_no", SqlDbType.VarChar, 30) 
            };
            parameterArray[0].Value = trade_no;
            OriginalStudio.Model.Settled.Distribution distribution = new OriginalStudio.Model.Settled.Distribution();
            DataSet set = DataBase.ExecuteDataset(CommandType.StoredProcedure, "proc_distribution_GetModelByNo", parameterArray);
            if (set.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(set.Tables[0].Rows[0]);
            }
            return null;
        }

        #endregion
        
        public int GetRecordCount(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM distribution ");
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

        public static string GenerateTradeNo(int mode)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string objId = mode + new Random().Next(100, 999).ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (WebCache.GetCacheService().RetrieveObject(objId) != null)
            {
                return GenerateTradeNo(mode);
            }
            WebCache.GetCacheService().AddObject(objId, objId, 10);
            return objId;
        }

        public DataSet GetAllList()
        {
            try
            {
                return this.GetList("");
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select id,suppid,mode,settledId,trade_no,batchNo,supp_trade_no,userid,balance,bankCode,bankName,bankBranch,bankAccountName,bankAccount,amount,charges,balance2,addTime,processingTime,supp_message,status,ext1,ext2,ext3,remark ");
            builder.Append(" FROM distribution ");
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
            builder.Append(" id,suppid,mode,settledId,trade_no,batchNo,supp_trade_no,userid,balance,bankCode,bankName,bankBranch,bankAccountName,bankAccount,amount,charges,balance2,addTime,processingTime,supp_message,status,ext1,ext2,ext3,remark ");
            builder.Append(" FROM distribution ");
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
            builder.Append(")AS Row, T.*  from distribution T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                builder.Append(" WHERE " + strWhere);
            }
            builder.Append(" ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DataBase.ExecuteDataset(CommandType.Text, builder.ToString());
        }

        public List<OriginalStudio.Model.Settled.Distribution> GetModelList(string strWhere)
        {
            try
            {
                DataSet list = this.GetList(strWhere);
                return this.DataTableToList(list.Tables[0]);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public static string GetStatusText(object _status)
        {
            if ((_status == null) || (_status == DBNull.Value))
            {
                return string.Empty;
            }
            int num = Convert.ToInt32(_status);
            switch (num)
            {
                case 0:
                    return "已受理";

                case 1:
                    return "未受理";

                case 2:
                    return "审核拒绝";

                case 3:
                    return "代发成功";

                case 4:
                    return "代发失败";

                case 255:
                    return "初始状态";
            }
            return num.ToString();
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            DataSet set = new DataSet();
            try
            {
                string tables = "v_distribution";
                string key = "[id]";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = "id desc";
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                string wheres = this.BuilderWhere(searchParams, paramList);
                return DataBase.ExecuteDataset(CommandType.Text, SqlHelper.GetCountSQL(tables, wheres, string.Empty) + "\r\n" +
                    SqlHelper.GetPageSelectSQL("*", tables, wheres, orderby, key, pageSize, page, false), paramList.ToArray());
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return set;
            }
        }


        private string BuilderWhere(List<SearchParam> param, List<SqlParameter> paramList)
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
                            if (str2 == "trade_no")
                            {
                                goto Label_00F5;
                            }
                            if (str2 == "bankaccount")
                            {
                                goto Label_0148;
                            }
                            if (str2 == "bankcode")
                            {
                                goto Label_019B;
                            }
                            if (str2 == "stime")
                            {
                                goto Label_01EB;
                            }
                            if (str2 == "etime")
                            {
                                goto Label_021D;
                            }
                        }
                        else
                        {
                            builder.Append(" AND [userid] = @userid");
                            SqlParameter parameter = new SqlParameter("@userid", SqlDbType.Int);
                            parameter.Value = (int)param2.ParamValue;
                            paramList.Add(parameter);
                        }
                    }
                    continue;
                Label_00F5:
                    builder.Append(" AND [trade_no] like @trade_no");
                    SqlParameter item = new SqlParameter("@trade_no", SqlDbType.VarChar, 30);
                    item.Value = "%" + SqlHelper.CleanString((string)param2.ParamValue, 30) + "%";
                    paramList.Add(item);
                    continue;
                Label_0148:
                    builder.Append(" AND [bankAccount] like @bankAccount");
                    SqlParameter parameter3 = new SqlParameter("@bankAccount", SqlDbType.VarChar, 30);
                    parameter3.Value = "%" + SqlHelper.CleanString((string)param2.ParamValue, 30) + "%";
                    paramList.Add(parameter3);
                    continue;
                Label_019B:
                    builder.Append(" AND [bankCode] = @bankCode");
                    SqlParameter parameter4 = new SqlParameter("@bankCode", SqlDbType.VarChar, 20);
                    parameter4.Value = "%" + SqlHelper.CleanString((string)param2.ParamValue, 20) + "%";
                    paramList.Add(parameter4);
                    continue;
                Label_01EB:
                    builder.Append(" AND [processingTime] >= @stime");
                    SqlParameter parameter5 = new SqlParameter("@stime", SqlDbType.DateTime);
                    parameter5.Value = param2.ParamValue;
                    paramList.Add(parameter5);
                    continue;
                Label_021D:
                    builder.Append(" AND [processingTime] <= @etime");
                    SqlParameter parameter6 = new SqlParameter("@etime", SqlDbType.DateTime);
                    parameter6.Value = param2.ParamValue;
                    paramList.Add(parameter6);
                }
            }
            return builder.ToString();
        }
    }
}

