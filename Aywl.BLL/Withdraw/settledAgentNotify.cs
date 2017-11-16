namespace OriginalStudio.BLL.Withdraw
{
    using OriginalStudio.BLL.User;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Web;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using OriginalStudio.Lib.Data;
    using OriginalStudio.Lib;

    public class settledAgentNotify
    {
        private readonly OriginalStudio.DAL.Withdraw.settledAgentNotify dal = new OriginalStudio.DAL.Withdraw.settledAgentNotify();

        public int Add(OriginalStudio.Model.Withdraw.settledAgentNotify model)
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

        public List<OriginalStudio.Model.Withdraw.settledAgentNotify> DataTableToList(DataTable dt)
        {
            List<OriginalStudio.Model.Withdraw.settledAgentNotify> list = new List<OriginalStudio.Model.Withdraw.settledAgentNotify>();
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    OriginalStudio.Model.Withdraw.settledAgentNotify item = this.dal.DataRowToModel(dt.Rows[i]);
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

        public void DoNotify(OriginalStudio.Model.Withdraw.settledAgent model)
        {
            if (((model != null) && PageValidate.IsUrl(model.return_url)) && !model.is_cancel)
            {
                OriginalStudio.Model.Withdraw.settledAgentNotify notify = new OriginalStudio.Model.Withdraw.settledAgentNotify();
                notify.userid = model.userid;
                notify.trade_no = model.trade_no;
                string str = notify.notify_id;
                string service = model.service;
                string str3 = model.input_charset;
                string str4 = model.userid.ToString();
                string str5 = model.sign_type;
                string str6 = notify.addTime.ToString("yyyyMMddHHmmss");
                string str7 = model.trade_no;
                string str8 = "0.00";
                int num = 1;
                if (model.is_cancel)
                {
                    num = 1;
                }
                else
                {
                    if (model.audit_status == 1)
                    {
                        num = 1;
                    }
                    else if (model.audit_status == 2)
                    {
                        num = 0;
                    }
                    else if (model.audit_status == 3)
                    {
                        num = 2;
                    }
                    if (model.audit_status == 2)
                    {
                        if (model.payment_status == 2)
                        {
                            num = 3;
                            str8 = model.amount.ToString("f2");
                        }
                        if (model.payment_status == 3)
                        {
                            num = 4;
                        }
                    }
                }
                string str9 = num.ToString();
                string str10 = string.Empty;
                string str11 = CommonHelper.BuildParamString(CommonHelper.BubbleSort(new string[] { "service=" + service, "input_charset=" + str3, "partner=" + str4, "sign_type=" + str5, "notify_id=" + str, "notify_time=" + str6, "out_trade_no=" + str7, "trade_status=" + str9, "error_message=" + str10, "amount_str=" + str8 }));
                string userApiKey = UserFactory.GetUserApiKey(model.userid);
                string str13 = CommonHelper.md5(str3, str11 + userApiKey).ToLower();
                string url = model.return_url + "?" + string.Format("service={0}", service) + string.Format("&input_charset={0}", str3) + string.Format("&partner={0}", str4) + string.Format("&sign_type={0}", str5) + string.Format("&notify_id={0}", str) + string.Format("&notify_time={0}", str6) + string.Format("&out_trade_no={0}", str7) + string.Format("&trade_status={0}", str9) + string.Format("&error_message={0}", str10) + string.Format("&amount_str={0}", str8) + string.Format("&sign={0}", str13);
                try
                {
                    string str15 = WebClientHelper.GetString(url, null, "GET", Encoding.GetEncoding(str3));
                    notify.resTime = new DateTime?(DateTime.Now);
                    int num2 = 1;
                    if (str15 == str)
                    {
                        num2 = 2;
                    }
                    else
                    {
                        num2 = 0;
                    }
                    notify.notifyurl = url;
                    notify.resText = str15;
                    notify.notifystatus = num2;
                }
                catch (Exception exception)
                {
                    notify.notifyurl = url;
                    notify.resText = "";
                    notify.notifystatus = 0;
                    notify.remark = exception.Message;
                }
                new OriginalStudio.BLL.Withdraw.settledAgentNotify().Add(notify);
            }
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

        public OriginalStudio.Model.Withdraw.settledAgentNotify GetModel(int id)
        {
            return this.dal.GetModel(id);
        }

        public List<OriginalStudio.Model.Withdraw.settledAgentNotify> GetModelList(string strWhere)
        {
            DataSet list = this.dal.GetList(strWhere);
            return this.DataTableToList(list.Tables[0]);
        }

        public string GetNotifyStatusText(object notifystatus)
        {
            string str = string.Empty;
            if (notifystatus != DBNull.Value)
            {
                switch (Convert.ToInt32(notifystatus))
                {
                    case 0:
                        return "失败";

                    case 1:
                        return "处理中";

                    case 2:
                        return "成功";
                }
            }
            return str;
        }

        public int GetRecordCount(string strWhere)
        {
            return this.dal.GetRecordCount(strWhere);
        }

        public DataSet PageSearch(List<SearchParam> searchParams, int pageSize, int page, string orderby)
        {
            try
            {
                return this.dal.PageSearch(searchParams, pageSize, page, orderby);
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
                return null;
            }
        }

        public bool Update(OriginalStudio.Model.Withdraw.settledAgentNotify model)
        {
            return this.dal.Update(model);
        }
    }
}

