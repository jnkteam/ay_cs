namespace OriginalStudio.BLL.Withdraw
{
    using OriginalStudio.DAL.Withdraw;
    using OriginalStudio.Model.Withdraw;
    using OriginalStudio.Lib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using OriginalStudio.DBAccess;

    /// <summary>
    /// 通道结算
    /// </summary>
    public class ChannelWithdraw
    {
        private readonly OriginalStudio.DAL.Withdraw.channelwithdraw dal = new OriginalStudio.DAL.Withdraw.channelwithdraw();

        public int Add(OriginalStudio.Model.Withdraw.ChannelWithdraw model)
        {
            SqlParameter[] parameterArray = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int, 10),
                new SqlParameter("@bankCode", SqlDbType.VarChar, 10),
                new SqlParameter("@bankName", SqlDbType.VarChar, 30),
                new SqlParameter("@supplier", SqlDbType.Int, 10),
                new SqlParameter("@sort", SqlDbType.Int, 10)
            };
            parameterArray[0].Direction = ParameterDirection.Output;
            parameterArray[1].Value = model.bankCode;
            parameterArray[2].Value = model.bankName;
            parameterArray[3].Value = model.supplier;
            parameterArray[4].Value = model.sort;
            DataBase.ExecuteNonQuery(CommandType.StoredProcedure, "proc_channelwithdraw_ADD", parameterArray);
            return (int)parameterArray[0].Value;
        }

        public List<OriginalStudio.Model.Withdraw.ChannelWithdraw> DataTableToList(DataTable dt)
        {
            List<OriginalStudio.Model.Withdraw.ChannelWithdraw> list = new List<OriginalStudio.Model.Withdraw.ChannelWithdraw>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    OriginalStudio.Model.Withdraw.ChannelWithdraw item = this.dal.DataRowToModel(dt.Rows[i]);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public bool Delete(int id)
        {
            return this.dal.Delete(id);
        }

        public bool DeleteList(string idlist)
        {
            return this.dal.DeleteList(idlist);
        }

        public DataSet GetAllList()
        {
            return this.GetList("");
        }

        public DataSet GetList(string strWhere)
        {
            return this.dal.GetList(strWhere);
        }

        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return this.dal.GetList(Top, strWhere, filedOrder);
        }

        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return this.dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        public OriginalStudio.Model.Withdraw.ChannelWithdraw GetModel(int id)
        {
            return this.dal.GetModel(id);
        }

        public OriginalStudio.Model.Withdraw.ChannelWithdraw GetModelByBankName(string bankName)
        {
            try
            {
                return this.dal.GetModelByBankName(bankName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public OriginalStudio.Model.Withdraw.ChannelWithdraw GetModelByBankCode(string bankCode)
        {
            try
            {
                return this.dal.GetModelByBankCode(bankCode);
            }
            catch (Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write(err.Message.ToString());
                return null;
            }
        }

        public List<OriginalStudio.Model.Withdraw.ChannelWithdraw> GetModelList(string strWhere)
        {
            DataSet list = this.dal.GetList(strWhere);
            return this.DataTableToList(list.Tables[0]);
        }

        public int GetRecordCount(string strWhere)
        {
            return this.dal.GetRecordCount(strWhere);
        }

        /// <summary>
        /// 根据银行代码取代付结算供应商。
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public int GetSupplier(string bankCode)
        {
            try
            {
                return this.dal.GetSupplier(bankCode);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public bool Update(OriginalStudio.Model.Withdraw.ChannelWithdraw model)
        {
            return this.dal.Update(model);
        }
    }
}

