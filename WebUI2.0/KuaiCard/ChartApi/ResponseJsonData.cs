using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using General.Common;

/// <summary>
/// 返回Json格式数据
/// </summary>
public class ResponseJsonData
{
    private string conStr = "";

	public ResponseJsonData()
	{
        conStr = SQLServerHelper.connString;
	}

    /// <summary>
    /// 日汇总数据统计
    /// </summary>
    /// <param name="p_from"></param>
    /// <param name="p_to"></param>
    /// <param name="p_user_id"></param>
    /// <returns></returns>
    public string GetDayOrderBankList(DateTime p_from, DateTime p_to, int p_user_id)
    {
        General.DataProvider.SqlDataProvider sdp = new General.DataProvider.SqlDataProvider(this.conStr);
        sdp.Open();

        sdp.CommandType = System.Data.CommandType.StoredProcedure;
        sdp.CommandText = "proc_st_dayorderlist";
        List<IDbDataParameter> parameters = new List<IDbDataParameter>();
        parameters.Add(new SqlParameter("@p_from", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_to", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_userid", SqlDbType.Int));
        parameters[0].Value = p_from;
        parameters[1].Value = p_to;
        parameters[2].Value = p_user_id;
        sdp.IParameters = parameters;
        DataTable dt = sdp.ExecuteDataTable();
        sdp.Close();

        General.Common.Json json = new General.Common.Json();

        if (dt.Rows.Count == 0)
        {
            json.AddToJson("success", false);
        }
        else
        {
            json.AddToJson("data", dt);
            json.AddToJson("success", true);
        }
        return json.ToString();
    }

    /// <summary>
    /// 用户列表
    /// </summary>
    /// <returns></returns>
    public string GetCustomerList()
    {
        General.DataProvider.SqlDataProvider sdp = new General.DataProvider.SqlDataProvider(this.conStr);
        sdp.Open();

        sdp.CommandType = System.Data.CommandType.Text;
        sdp.CommandText = "select t.id,t.userName from userbase t where t.status = 2 order by t.id asc";
        DataTable dt = sdp.ExecuteDataTable();
        sdp.Close();

        General.Common.Json json = new General.Common.Json();

        if (dt.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            Json jsAll = new Json();
            jsAll.AddToJson("id", 0);
            jsAll.AddToJson("userName", "全部");
            sb.Append(jsAll.ToString()).Append(",");

            foreach (DataRow dr in dt.Rows)
            {
                Json jsSub = new Json();
                jsSub.AddToJson("id", dr["id"].ToString());
                jsSub.AddToJson("userName", dr["userName"].ToString());

                sb.Append(jsSub.ToString()).Append(",");
            };

            sb.Remove(sb.Length - 1, 1);
            return "[" + sb.ToString() + "]";
        }

        return json.ToString();
    }

    /// <summary>
    /// 统计一天之内的分时成交
    /// </summary>
    /// <param name="p_date"></param>
    /// <returns></returns>
    public string GetHourVolume(DateTime p_date, int p_user_id)
    {
        General.DataProvider.SqlDataProvider sdp = new General.DataProvider.SqlDataProvider(this.conStr);
        sdp.Open();

        sdp.CommandType = System.Data.CommandType.StoredProcedure;
        sdp.CommandText = "proc_st_hourorderstat";
        List<IDbDataParameter> parameters = new List<IDbDataParameter>();
        parameters.Add(new SqlParameter("@p_date", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_userid", SqlDbType.Int));
        parameters[0].Value = p_date;
        parameters[1].Value = p_user_id;
        sdp.IParameters = parameters;
        DataTable dt = sdp.ExecuteDataTable();
        sdp.Close();

        General.Common.Json json = new General.Common.Json();

        if (dt.Rows.Count == 0)
        {
            json.AddToJson("success", false);
        }
        else
        {
            json.AddToJson("data", dt);
            json.AddToJson("success", true);
        }
        return json.ToString();
    }

    /// <summary>
    /// 客户利润汇总
    /// </summary>
    /// <param name="p_from"></param>
    /// <param name="p_to"></param>
    /// <returns></returns>
    public string GetCustOrderBandSort(DateTime p_from, DateTime p_to,int p_user_id)
    {
        General.DataProvider.SqlDataProvider sdp = new General.DataProvider.SqlDataProvider(this.conStr);
        sdp.Open();

        sdp.CommandType = System.Data.CommandType.StoredProcedure;
        sdp.CommandText = "proc_st_custorderbanksort";
        List<IDbDataParameter> parameters = new List<IDbDataParameter>();
        parameters.Add(new SqlParameter("@p_from", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_to", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_userid", SqlDbType.Int));
        parameters[0].Value = p_from;
        parameters[1].Value = p_to;
        parameters[2].Value = p_user_id;
        sdp.IParameters = parameters;
        DataTable dt = sdp.ExecuteDataTable();
        sdp.Close();

        General.Common.Json json = new General.Common.Json();

        if (dt.Rows.Count == 0)
        {
            json.AddToJson("success", false);
        }
        else
        {
            json.AddToJson("data", dt);
            json.AddToJson("success", true);
        }
        return json.ToString();
    }

    /// <summary>
    /// 获取通道使用情况统计
    /// </summary>
    /// <param name="p_from"></param>
    /// <param name="p_to"></param>
    /// <param name="p_user_id"></param>
    /// <returns></returns>
    public string GetChannelUseStat(DateTime p_from, DateTime p_to, int p_user_id)
    {
        General.DataProvider.SqlDataProvider sdp = new General.DataProvider.SqlDataProvider(this.conStr);
        sdp.Open();

        sdp.CommandType = System.Data.CommandType.StoredProcedure;
        sdp.CommandText = "proc_st_channelusestat";
        List<IDbDataParameter> parameters = new List<IDbDataParameter>();
        parameters.Add(new SqlParameter("@p_from", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_to", SqlDbType.DateTime));
        parameters.Add(new SqlParameter("@p_userid", SqlDbType.Int));
        parameters[0].Value = p_from;
        parameters[1].Value = p_to;
        parameters[2].Value = p_user_id;
        sdp.IParameters = parameters;
        DataTable dt = sdp.ExecuteDataTable();
        sdp.Close();

        General.Common.Json json = new General.Common.Json();

        if (dt.Rows.Count == 0)
        {
            json.AddToJson("success", false);
        }
        else
        {
            json.AddToJson("data", dt);
            json.AddToJson("success", true);
        }
        return json.ToString();
    }
}