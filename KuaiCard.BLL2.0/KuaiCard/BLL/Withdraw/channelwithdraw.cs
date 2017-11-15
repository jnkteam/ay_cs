namespace KuaiCard.BLL.Withdraw
{
    using KuaiCard.DAL.Withdraw;
    using KuaiCard.Model.Withdraw;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class channelwithdraw
    {
        private readonly KuaiCard.DAL.Withdraw.channelwithdraw dal = new KuaiCard.DAL.Withdraw.channelwithdraw();

        public int Add(KuaiCard.Model.Withdraw.channelwithdraw model)
        {
            return this.dal.Add(model);
        }

        public List<KuaiCard.Model.Withdraw.channelwithdraw> DataTableToList(DataTable dt)
        {
            List<KuaiCard.Model.Withdraw.channelwithdraw> list = new List<KuaiCard.Model.Withdraw.channelwithdraw>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    KuaiCard.Model.Withdraw.channelwithdraw item = this.dal.DataRowToModel(dt.Rows[i]);
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

        public KuaiCard.Model.Withdraw.channelwithdraw GetModel(int id)
        {
            return this.dal.GetModel(id);
        }

        public KuaiCard.Model.Withdraw.channelwithdraw GetModelByBankName(string bankName)
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

        public KuaiCard.Model.Withdraw.channelwithdraw GetModelByBankCode(string bankCode)
        {
            try
            {
                return this.dal.GetModelByBankCode(bankCode);
            }
            catch (Exception err)
            {
                KuaiCardLib.Logging.LogHelper.Write(err.Message.ToString());
                return null;
            }
        }

        public List<KuaiCard.Model.Withdraw.channelwithdraw> GetModelList(string strWhere)
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

        public bool Update(KuaiCard.Model.Withdraw.channelwithdraw model)
        {
            return this.dal.Update(model);
        }
    }
}

