namespace KuaiCard.BLL.Withdraw
{
    using KuaiCard.Cache;
    using KuaiCard.DAL.Withdraw;
    using KuaiCard.Model.Withdraw;
    using KuaiCardLib.ExceptionHandling;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Threading;
    using KuaiCardLib.Data;

    public class settledAgent
    {
        private readonly KuaiCard.DAL.Withdraw.settledAgent dal = new KuaiCard.DAL.Withdraw.settledAgent();

        public int Add(KuaiCard.Model.Withdraw.settledAgent model)
        {
            try
            {
                return this.dal.Add(model);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0;
            }
        }

        public int Affirm(string trade_no, byte sure, string clientip)
        {
            try
            {
                return this.dal.Affirm(trade_no, sure, clientip);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public int Audit(string trade_no, int auditstatus, int auditUser, string auditUserName)
        {
            try
            {
                int num = this.dal.Audit(trade_no, auditstatus, auditUser, auditUserName);
                if (num == 0)
                {
                    this.DoNotify(trade_no);
                }
                return num;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public int Cancel(string trade_no)
        {
            try
            {
                int num = this.dal.Cancel(trade_no);
                if (num == 0)
                {
                    this.DoNotify(trade_no);
                }
                return num;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public int ChkParms(int userid, string bankCode, decimal amount, out DataRow row)
        {
            row = null;
            try
            {
                return this.dal.ChkParms(userid, bankCode, amount, out row);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public int Complete(string trade_no, int pstatus)
        {
            try
            {
                int num = this.dal.Complete(trade_no, pstatus);
                if (num == 0)
                {
                    this.DoNotify(trade_no);
                }
                return num;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return 0x63;
            }
        }

        public List<KuaiCard.Model.Withdraw.settledAgent> DataTableToList(DataTable dt)
        {
            List<KuaiCard.Model.Withdraw.settledAgent> list = new List<KuaiCard.Model.Withdraw.settledAgent>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    KuaiCard.Model.Withdraw.settledAgent item = this.dal.DataRowToModel(dt.Rows[i]);
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
            try
            {
                return this.dal.Delete(id);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool Delete(string trade_no)
        {
            try
            {
                return this.dal.Delete(trade_no);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public bool DeleteList(string idlist)
        {
            try
            {
                return this.dal.DeleteList(idlist);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public string doAudit(string trade_no, int auditUser, string auditUserName)
        {
            switch (this.Audit(trade_no, 2, auditUser, auditUserName))
            {
                case 0:
                    return "审核成功";

                case 1:
                    return "不存在此单";

                case 2:
                    return "此单已取消";

                case 3:
                    return "已审核过，不可重复操作";

                case 4:
                    return "输入状态不正确";

                case 5:
                    return "系统故障，请查看日志";

                case 6:
                    return "用户未确认,不可操作";
            }
            return "未知错误";
        }

        public string doCancel(string trade_no)
        {
            switch (this.Cancel(trade_no))
            {
                case 0:
                    return "取消成功";

                case 1:
                    return "不存在此单";

                case 2:
                    return "此单已取消，不可重复操作";

                case 3:
                    return "已审核，不可取消";

                case 4:
                    return "系统故障，请查看日志";

                case 5:
                    return "用户未确认";
            }
            return "未知错误";
        }

        public void DoNotify(string trade_no)
        {
            KuaiCard.Model.Withdraw.settledAgent model = this.GetModel(trade_no);
            if (model != null)
            {
                notifyHelper helper = new notifyHelper();
                helper.model = model;
                new Thread(new ThreadStart(helper.DoNotify)).Start();
            }
        }

        public string doRefuse(string trade_no, int auditUser, string auditUserName)
        {
            switch (this.Audit(trade_no, 3, auditUser, auditUserName))
            {
                case 0:
                    return "操作成功";

                case 1:
                    return "不存在此单";

                case 2:
                    return "此单已取消";

                case 3:
                    return "已审核过，不可重复操作";

                case 4:
                    return "输入状态不正确";

                case 5:
                    return "系统故障，请查看日志";
            }
            return "未知错误";
        }

        public bool Exists(string trade_no)
        {
            try
            {
                return this.dal.Exists(trade_no);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }

        public string GenerateTradeNo(int userid, int serial)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            string objId = string.Concat(new object[] { DateTime.Now.ToString("yyyyMMddHHmmssff"), userid, serial.ToString(), random.Next(0x3e8).ToString("0000") });
            if (WebCache.GetCacheService().RetrieveObject(objId) != null)
            {
                return this.GenerateTradeNo(userid, serial);
            }
            WebCache.GetCacheService().AddObject(objId, objId, 10);
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
                        return "等待审核";

                    case 2:
                        return "审核通过";

                    case 3:
                        return "审核拒绝";
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

        public string GetIsSureText(object issure)
        {
            string str = string.Empty;
            if (issure != DBNull.Value)
            {
                switch (Convert.ToInt32(issure))
                {
                    case 1:
                        return "等待处理";

                    case 2:
                        return "已确认";

                    case 3:
                        return "未确认";
                }
            }
            return str;
        }

        public DataSet GetList(string strWhere)
        {
            try
            {
                return this.dal.GetList(strWhere);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            try
            {
                return this.dal.GetList(Top, strWhere, filedOrder);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return this.dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        public KuaiCard.Model.Withdraw.settledAgent GetModel(int id)
        {
            try
            {
                return this.dal.GetModel(id);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public KuaiCard.Model.Withdraw.settledAgent GetModel(string trade_no)
        {
            try
            {
                return this.dal.GetModel(trade_no);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public List<KuaiCard.Model.Withdraw.settledAgent> GetModelList(string strWhere)
        {
            try
            {
                DataSet list = this.dal.GetList(strWhere);
                return this.DataTableToList(list.Tables[0]);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public string GetModeText(object mode)
        {
            string str = string.Empty;
            if (mode != DBNull.Value)
            {
                switch (Convert.ToInt32(mode))
                {
                    case 1:
                        return "API提交";

                    case 2:
                        return "上传文件";
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

        public string GetPaymentStatusText(object payment_status)
        {
            string str = string.Empty;
            if (payment_status != DBNull.Value)
            {
                switch (Convert.ToInt32(payment_status))
                {
                    case 1:
                        return "未知";

                    case 2:
                        return "成功";

                    case 3:
                        str = "失败";
                        break;

                    case 4:
                        str = "付款中";
                        break;
                }
            }
            return str;
        }

        public int GetRecordCount(string strWhere)
        {
            return this.dal.GetRecordCount(strWhere);
        }

        public string GetStatusText(object is_cancel, object audit_status, object payment_status)
        {
            string str = "未知";
            if (is_cancel != DBNull.Value)
            {
                if (Convert.ToByte(is_cancel) == 1)
                {
                    return "已取消";
                }
                switch (Convert.ToByte(audit_status))
                {
                    case 1:
                        str = "等待审核";
                        break;

                    case 2:
                        str = "已审核(处理中)";
                        break;

                    case 3:
                        str = "审核拒绝";
                        break;
                }
                switch (Convert.ToByte(payment_status))
                {
                    case 2:
                        str = "支付成功";
                        break;

                    case 3:
                        str = "付款失败";
                        break;

                    case 4:
                        str = "已审核(处理中)";
                        break;
                }
            }
            return str;
        }

        public string GetSureText(object is_sure)
        {
            string str = string.Empty;
            if (is_sure != DBNull.Value)
            {
                switch (Convert.ToByte(is_sure))
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

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby, byte stat)
        {
            try
            {
                return this.dal.PageSearch(searchParams, pageSize, page, orderby, stat);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public string PayFail(string trade_no)
        {
            switch (this.Complete(trade_no, 3))
            {
                case 0:
                    return "付款成功";

                case 1:
                    return "不存在此单";

                case 2:
                    return "此单已取消";

                case 3:
                    return "此单未审核，不可完成此操作";

                case 4:
                    return "此单已结案";

                case 5:
                    return "系统故障，请查看日志";
            }
            return "未知错误";
        }

        public string PaySuccess(string trade_no)
        {
            switch (this.Complete(trade_no, 2))
            {
                case 0:
                    return "付款成功";

                case 1:
                    return "不存在此单";

                case 2:
                    return "此单已取消";

                case 3:
                    return "此单未审核，不可完成此操作";

                case 4:
                    return "此单已结案";

                case 5:
                    return "系统故障，请查看日志";
            }
            return "未知错误";
        }

        public bool Update(KuaiCard.Model.Withdraw.settledAgent model)
        {
            try
            {
                return this.dal.Update(model);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return false;
            }
        }
    }
}

