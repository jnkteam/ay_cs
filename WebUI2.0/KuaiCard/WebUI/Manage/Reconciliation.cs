namespace KuaiCard.WebUI.Manage
{
    using KuaiCard.BLL;
    using KuaiCard.ETAPI;
    using KuaiCard.Model;
    using KuaiCard.WebComponents.Web;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Reconciliation : ManagePageBase
    {
        protected Button btn_search;
        protected DropDownList ddlsupp;
        protected HtmlForm form1;
        protected HtmlHead Head1;
        protected Repeater rptOrders;
        protected TextBox txtorders;

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtorders.Text.Trim()))
            {
                base.AlertAndRedirect("请输入订单号");
            }
            else
            {
                DataRow row;
                DataTable table = new DataTable();
                table.Columns.Add("orderid", typeof(string));
                table.Columns.Add("supporder", typeof(string));
                table.Columns.Add("realamt", typeof(string));
                table.Columns.Add("result", typeof(string));
                table.Columns.Add("status", typeof(string));
                table.Columns.Add("coin", typeof(string));
                table.Columns.Add("cardtype", typeof(string));
                string str = string.Empty;
                string selectedValue = this.ddlsupp.SelectedValue;
                string[] strArray = this.txtorders.Text.Split(new char[] { '\n' });
                string orderids = string.Empty;
                foreach (string str4 in strArray)
                {
                    string[] strArray2;
                    string str21 = selectedValue;
                    if (str21 != null)
                    {
                        if (!(str21 == "70"))
                        {
                            if (str21 == "60866")
                            {
                                goto Label_0291;
                            }
                            if (str21 == "700")
                            {
                                goto Label_036A;
                            }
                            if (str21 == "80")
                            {
                                goto Label_0421;
                            }
                            if (str21 == "81")
                            {
                                goto Label_054A;
                            }
                            if (str21 == "86")
                            {
                                goto Label_0652;
                            }
                        }
                        else
                        {
                            row = table.NewRow();
                            row["orderid"] = str4;
                            str = "";
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    strArray2 = str.Split(new char[] { '&' });
                                    row["status"] = strArray2[0].Replace("returncode=", "");
                                    row["realamt"] = strArray2[1].Replace("realmoney=", "");
                                    row["result"] = strArray2[2].Replace("message=", "");
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                row["result"] = "查询失败";
                            }
                            table.Rows.Add(row);
                        }
                    }
                    continue;
                Label_0291:
                    row = table.NewRow();
                    row["orderid"] = str4;
                    str = "";
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            strArray2 = str.Split(new char[] { '&' });
                            row["status"] = strArray2[0].Replace("returncode=", "");
                            row["realamt"] = strArray2[1].Replace("realmoney=", "");
                            row["result"] = strArray2[2].Replace("message=", "");
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        row["result"] = "查询失败";
                    }
                    table.Rows.Add(row);
                    continue;
                Label_036A:
                    row = table.NewRow();
                    row["orderid"] = str4;
                    str = "";
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            strArray2 = str.Split(new char[] { '&' });
                            row["status"] = strArray2[0].Replace("opstate=", "");
                            row["realamt"] = strArray2[1].Replace("ovalue=", "");
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        row["result"] = "查询失败";
                    }
                    table.Rows.Add(row);
                    continue;
                Label_0421:
                    row = table.NewRow();
                    row["orderid"] = str4;
                    str = "";
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(str);
                            string innerText = document.GetElementsByTagName("billid")[0].InnerText;
                            string str6 = document.GetElementsByTagName("result")[0].InnerText;
                            string str7 = document.GetElementsByTagName("info")[0].InnerText;
                            string str8 = document.GetElementsByTagName("value")[0].InnerText;
                            string str9 = document.GetElementsByTagName("accountvalue")[0].InnerText;
                            row["supporder"] = innerText;
                            row["realamt"] = str8;
                            row["result"] = str7;
                            row["status"] = str6;
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        row["result"] = "查询失败";
                    }
                    table.Rows.Add(row);
                    continue;
                Label_054A:
                    row = table.NewRow();
                    row["orderid"] = str4;
                    str = "";
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            strArray2 = str.Split(new char[] { '&' });
                            if (strArray2.Length == 11)
                            {
                                string str10 = strArray2[0];
                                string str11 = strArray2[1];
                                string str12 = strArray2[2];
                                string str13 = strArray2[3];
                                string str14 = strArray2[4];
                                string str15 = strArray2[5];
                                string str16 = strArray2[6];
                                string str17 = strArray2[7];
                                string str18 = strArray2[8];
                                string str19 = strArray2[9];
                                string str20 = strArray2[10];
                                row["supporder"] = str14;
                                row["realamt"] = str16;
                                row["result"] = str11;
                                row["status"] = str11;
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        row["result"] = "查询失败";
                    }
                    table.Rows.Add(row);
                    continue;
                Label_0652:
                    orderids = orderids + str4 + "|";
                }
                if (selectedValue == "86")
                {
                    orderids = orderids.Substring(0, orderids.Length - 1);
                    str = "";
                    if (!string.IsNullOrEmpty(str))
                    {
                        ;
                    }
                    else
                    {
                        row = table.NewRow();
                        row["orderid"] = orderids;
                        row["result"] = "查询失败";
                        table.Rows.Add(row);
                    }
                }
                this.rptOrders.DataSource = table;
                this.rptOrders.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
            }
        }

        private void setPower()
        {
            if (!ManageFactory.CheckCurrentPermission(false, ManageRole.System))
            {
                base.Response.Write("Sorry,No authority!");
                base.Response.End();
            }
        }
    }
}

