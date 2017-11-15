namespace KuaiCard.BLL.Withdraw
{
    using KuaiCard.Cache;
    using KuaiCard.DAL.Withdraw;
    using KuaiCard.Model.Withdraw;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using KuaiCardLib.Data;

    public class settledAgentSummary
    {
        private readonly KuaiCard.DAL.Withdraw.settledAgentSummary dal = new KuaiCard.DAL.Withdraw.settledAgentSummary();

        public int Add(KuaiCard.Model.Withdraw.settledAgentSummary model)
        {
            return this.dal.Add(model);
        }

        public int Affirm(string lot_no, int auditstatus, int auditUser, string auditUserName, string clientip)
        {
            try
            {
                return this.dal.Affirm(lot_no, auditstatus, auditUser, auditUserName, clientip);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public int ChkParms(int userid, decimal tamount)
        {
            try
            {
                return this.dal.ChkParms(userid, tamount);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public List<KuaiCard.Model.Withdraw.settledAgentSummary> DataTableToList(DataTable dt)
        {
            List<KuaiCard.Model.Withdraw.settledAgentSummary> list = new List<KuaiCard.Model.Withdraw.settledAgentSummary>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    KuaiCard.Model.Withdraw.settledAgentSummary item = this.dal.DataRowToModel(dt.Rows[i]);
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

        public bool Delete(string lotno)
        {
            return this.dal.Delete(lotno);
        }

        public bool DeleteList(string idlist)
        {
            return this.dal.DeleteList(idlist);
        }

        public bool Exists(string lotno)
        {
            return this.dal.Exists(lotno);
        }

        public string Generatelotno()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string objId = DateTime.Now.ToString("yyyyMMddHHmmssfff") + random.Next(0x2710).ToString("0000");
            //if (WebCache.GetCacheService().RetrieveObject(objId) != null)
            //{
            //    return this.Generatelotno();
            //}
            //WebCache.GetCacheService().AddObject(objId, objId, 10);
            return objId;
        }

        public DataSet GetAllList()
        {
            return this.GetList("");
        }

        public string GetAuditStatusText(object audit_status)
        {
            string str = string.Empty;
            if (audit_status != DBNull.Value)
            {
                switch (Convert.ToInt32(audit_status))
                {
                    case 1:
                        return "等待处理";

                    case 2:
                        return "已确认";

                    case 3:
                        return "已取消";
                }
            }
            return str;
        }

        public string GetCancelText(object is_cancel)
        {
            string str = string.Empty;
            if (is_cancel != DBNull.Value)
            {
                switch (Convert.ToInt32(is_cancel))
                {
                    case 0:
                        return "未取消";

                    case 1:
                        return "已取消";
                }
            }
            return str;
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

        public KuaiCard.Model.Withdraw.settledAgentSummary GetModel(int id)
        {
            return this.dal.GetModel(id);
        }

        public List<KuaiCard.Model.Withdraw.settledAgentSummary> GetModelList(string strWhere)
        {
            DataSet list = this.dal.GetList(strWhere);
            return this.DataTableToList(list.Tables[0]);
        }

        public string GetModeText(object mode)
        {
            string str = string.Empty;
            if (mode != DBNull.Value)
            {
                switch (Convert.ToInt32(mode))
                {
                    case 1:
                        return "API接口提交";

                    case 2:
                        return "手动增加";
                }
            }
            return str;
        }

        public string GetNotifyStatusText(object notifystatus)
        {
            string str = string.Empty;
            if (notifystatus != DBNull.Value)
            {
                switch (Convert.ToInt32(notifystatus))
                {
                    case 0:
                        return "发送失败";

                    case 1:
                        return "处理中";

                    case 2:
                        return "已成功";
                }
            }
            return str;
        }

        public int GetRecordCount(string strWhere)
        {
            return this.dal.GetRecordCount(strWhere);
        }

        public string GetStatus(object status)
        {
            string str = string.Empty;
            if (status != DBNull.Value)
            {
                switch (Convert.ToInt32(status))
                {
                    case 1:
                        return "等待处理";

                    case 2:
                        return "部分完成";

                    case 3:
                        return "全部完成";
                }
            }
            return str;
        }

        public int Insert(KuaiCard.Model.Withdraw.settledAgentSummary summarymodel, List<KuaiCard.Model.Withdraw.settledAgent> itemlist)
        {
            try
            {
                return this.dal.Insert(summarymodel, itemlist);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby, bool isstat)
        {
            try
            {
                return this.dal.PageSearch(searchParams, pageSize, page, orderby, isstat);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public bool Update(KuaiCard.Model.Withdraw.settledAgentSummary model)
        {
            return this.dal.Update(model);
        }
    }
}

