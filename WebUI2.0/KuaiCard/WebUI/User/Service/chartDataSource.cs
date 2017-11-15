namespace KuaiCard.WebUI.User.Service
{
    using KuaiCard.Model.Json;
    using KuaiCard.WebComponents;
    using KuaiCard.WebComponents.Web;
    using KuaiCardLib.Web;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Data;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class chartDataSource : UserHandlerBase
    {
        public override void OnLoad(HttpContext context)
        {  
            string s = "";

            AjaxResult model = new AjaxResult();
            if (this.CurrentUser == null)
            {
                s = "{\"result\":\"no\",\"desc\":\"未登录\"}";
            }
            else
            {
                try
                {
                    int userid = this.CurrentUser.ID;
                    DataSet ds = KuaiCard.BLL.User.UserFactory.GetUserDayOrderChartSource(userid);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        List<string> dateList = new List<string>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string tmp = dr["order_date"].ToString();
                            if (dateList.IndexOf(tmp) < 0)
                            {
                                dateList.Add(tmp);
                            }
                        }

                        SortedDictionary<string, string> wy = new SortedDictionary<string, string>();   //typeid = 102
                        SortedDictionary<string, string> zfb = new SortedDictionary<string, string>();  //typeid = 101,980
                        SortedDictionary<string, string> wx = new SortedDictionary<string, string>();   //typeid = 99,990,116
                        SortedDictionary<string, string> qq = new SortedDictionary<string, string>();   //typeid = 209

                        foreach (DataRow dr in dt.Rows)
                        {
                            string date = dr["order_date"].ToString();
                            string idx = (dateList.IndexOf(date) + 1).ToString();

                            if (dateList.IndexOf(date) >= 0)
                            {
                                string typeid = dr["typeId"].ToString();
                                string value = dr["total_value"].ToString();
                                if (typeid == "wy")
                                {
                                    wy.Add(idx, value);
                                }
                                else if (typeid == "zfb")
                                {
                                    zfb.Add(idx, value);
                                }
                                else if (typeid == "wx")
                                {
                                    wx.Add(idx, value);
                                }
                                else if (typeid == "qq")
                                {
                                    qq.Add(idx, value);
                                }
                            }
                        }

                        string tmpWy = "";
                        string tmpWx = "";
                        string tmpQQ = "";
                        string tmpZfb = "";
                        //返回格式：[[1,160], [2,30], [3,50], [4,100], [5,10], [6,90], [7,85], [8,40], [9,5]]
                        if (wy.Count > 0)
                        {
                            StringBuilder sbwy = new StringBuilder();
                            foreach (string key in wy.Keys)
                            {
                                sbwy.Append("[" + key + "," + wy[key] + "],");
                            }
                            tmpWy = sbwy.ToString();
                            if (tmpWy.Length > 0)
                            {
                                tmpWy = tmpWy.Substring(0, tmpWy.Length - 1);
                                tmpWy = "[" + tmpWy + "]";
                            }
                        }

                        if (wx.Count > 0)
                        {
                            StringBuilder sbwx = new StringBuilder();
                            foreach (string key in wx.Keys)
                            {
                                sbwx.Append("[" + key + "," + wx[key] + "],");
                            }
                            tmpWx = sbwx.ToString();
                            if (tmpWx.Length > 0)
                            {
                                tmpWx = tmpWx.Substring(0, tmpWx.Length - 1);
                                tmpWx = "[" + tmpWx + "]";
                            }
                        }

                        if (zfb.Count > 0)
                        {
                            StringBuilder sbzfb = new StringBuilder();
                            foreach (string key in zfb.Keys)
                            {
                                sbzfb.Append("[" + key + "," + zfb[key] + "],");
                            }
                            tmpZfb = sbzfb.ToString();
                            if (tmpZfb.Length > 0)
                            {
                                tmpZfb = tmpZfb.Substring(0, tmpZfb.Length - 1);
                                tmpZfb = "[" + tmpZfb + "]";
                            }
                        }

                        if (qq.Count > 0)
                        {
                            StringBuilder sbqq = new StringBuilder();
                            foreach (string key in qq.Keys)
                            {
                                sbqq.Append("[" + key + "," + qq[key] + "],");
                            }
                            tmpQQ = sbqq.ToString();
                            if (tmpQQ.Length > 0)
                            {
                                tmpQQ = tmpQQ.Substring(0, tmpQQ.Length - 1);
                                tmpQQ = "[" + tmpQQ + "]";
                            }
                        } 

                        s = "{\"result\":\"ok\",\"wy\":\"" + tmpWy + "\",\"wx\":\"" + tmpWx + "\",\"zfb\":\"" + tmpZfb + "\",\"qq\":\"" + tmpQQ + "\"}";

                    }
                }
                catch (Exception exception)
                {
                    s = exception.Message;
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
        }
    }
}

