using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// 数据接口
/// </summary>
public class PayApiHandler : IHttpHandler, IRequiresSessionState
{
    public PayApiHandler()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
    }

    #region IHttpHandler 成员

    public void ProcessRequest(System.Web.HttpContext context) 
    {
        //不缓存
        context.Response.Cache.SetNoStore();

        General.Common.Json json = new General.Common.Json();

        try
        {
            PayApi p_api = (PayApi)General.Web.Request.GetQueryInt("api", 0);

            if (p_api == PayApi.订单日线)
            {
                //
                string p_from = General.Web.Request.GetQueryString("from", "");
                string p_to = General.Web.Request.GetQueryString("to", "");
                int p_userid = General.Web.Request.GetQueryInt("userid", 0);
                DateTime p_from_date = Convert.ToDateTime(p_from);
                DateTime p_to_date = Convert.ToDateTime(p_to);
                context.Response.Write(new ResponseJsonData().GetDayOrderBankList(p_from_date, p_to_date, p_userid));
                return;
            }
            else if (p_api == PayApi.客户列表)
            {
                //客户列表
                context.Response.Write(new ResponseJsonData().GetCustomerList());
                return;
            }
            else if (p_api == PayApi.分时成交)
            {
                //按小时统计
                string p_from = General.Web.Request.GetQueryString("day", "");
                int p_userid = General.Web.Request.GetQueryInt("userid", 0);
                DateTime p_from_date = Convert.ToDateTime(p_from);
                context.Response.Write(new ResponseJsonData().GetHourVolume(p_from_date, p_userid));
                return;
            }
            else if (p_api == PayApi.客户汇总)
            {
                //客户利润汇总
                string p_from = General.Web.Request.GetQueryString("from", "");
                string p_to = General.Web.Request.GetQueryString("to", "");
                int p_userid = General.Web.Request.GetQueryInt("userid", 0);
                DateTime p_from_date = Convert.ToDateTime(p_from);
                DateTime p_to_date = Convert.ToDateTime(p_to);
                context.Response.Write(new ResponseJsonData().GetCustOrderBandSort(p_from_date, p_to_date, p_userid));
                return;
            }
            else if (p_api == PayApi.通道使用量统计)
            {
                //通道使用情况统计
                string p_from = General.Web.Request.GetQueryString("from", "");
                string p_to = General.Web.Request.GetQueryString("to", "");
                int p_userid = General.Web.Request.GetQueryInt("userid", 0);
                DateTime p_from_date = Convert.ToDateTime(p_from);
                DateTime p_to_date = Convert.ToDateTime(p_to);
                context.Response.Write(new ResponseJsonData().GetChannelUseStat(p_from_date, p_to_date, p_userid));
                return;
            }

        }
        catch (Exception err)
        {
            json.AddToJson("success", false);
            json.AddToJson("error", err.Message.ToString());
            context.Response.Write(json.ToString());
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #endregion
}