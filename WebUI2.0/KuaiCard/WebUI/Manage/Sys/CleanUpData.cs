namespace OriginalStudio.WebUI.Manage.Sys
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Tools;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib;
    using System;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Text;
    using System.Data;
    using System.Data.SqlClient;
    using DBAccess;

    public class CleanUpData : ManagePageBase
    {
        protected Button btnCleanUp;
        protected CheckBoxList cb_stat;
        protected CheckBoxList cb_where;
        protected CheckBoxList cbl_clearType;
        protected TextBox EtimeBox;
        protected HtmlForm form1;
        protected Label lbmsg;
        protected TextBox txtcaozuo;


        protected void btndel_Click(object sender, EventArgs e)
        {
            if (!ManageFactory.SecPwdVaild(this.txtcaozuo.Text.Trim()))
            {
                this.lbmsg.Text = "二级密码不正确，请重新输入!";
            }
            else
            {
                DateTime _endTime = DateTime.MinValue;
                DateTime.TryParse(this.EtimeBox.Text, out _endTime);
                //TimeSpan _diff = (TimeSpan)(DateTime.Now - _endTime);
                //if (_diff.TotalDays < 7.0)
                //{
                //    _endTime = DateTime.Now.AddDays(-7.0);
                //}
                bool order = false;
                bool settled = false;
                foreach (ListItem li in this.cbl_clearType.Items)
                {
                    if (li.Selected && (li.Value == "order"))
                    {
                        order = true;
                    }
                    else if (li.Selected && (li.Value == "settled"))
                    {
                        settled = true;
                    }
                }
                StringBuilder sql = new StringBuilder();
                StringBuilder where = new StringBuilder();
                if (order)
                {
                    bool _bank = false;
                    bool _card = false;
                    bool _sms = false;
                    foreach (ListItem li in this.cb_where.Items)
                    {
                        if (li.Selected)
                        {
                            if (li.Value == "bank")
                            {
                                //17.2.4修改
                                //sql.Append("\r\ndeclare @t table(orderid varchar(30))\r\ninsert into @t select orderid from v_orderbank where addtime<@addtime {0}\r\ndelete from orderbankamt where orderid in (select orderid from @t)\r\ndelete from orderbanknotify where orderid in (select orderid from @t)\r\ndelete from orderbank where orderid in (select orderid from @t)");
                                sql.Append("\r\ndeclare @t table(orderid varchar(30))\r\ninsert into @t select orderid from orderbank where addtime<@addtime {0}\r\ndelete from orderbankamt where orderid in (select orderid from @t)\r\ndelete from orderbanknotify where orderid in (select orderid from @t)\r\ndelete from orderbank where orderid in (select orderid from @t)");
                                _bank = true;
                            }
                            else if (li.Value == "card")
                            {
                                sql.Append("\r\ndeclare @t1 table(orderid varchar(30))\r\ninsert into @t1 select orderid from v_ordercard where addtime<@addtime {0}\r\ndelete from ordercardamt where orderid in (select orderid from @t1)\r\ndelete from ordercardnotify where orderid in (select orderid from @t1)\r\ndelete from  ordercard where orderid in (select orderid from @t1)");
                                _card = true;
                            }
                            else if (li.Value == "sms")
                            {
                                _sms = true;
                            }
                        }
                    }
                    if (sql.Length > 0)
                    {
                        bool isselected = false;
                        where.Append(" and status in (");
                        foreach (ListItem li in this.cb_stat.Items)
                        {
                            if (li.Selected)
                            {
                                if (!isselected)
                                {
                                    where.Append(li.Value);
                                }
                                else
                                {
                                    where.Append("," + li.Value);
                                }
                                isselected = true;
                            }
                        }
                        where.Append(")");
                        if (!isselected)
                        {
                            sql = new StringBuilder();
                        }
                        else
                        {
                            sql.Replace("{0}", where.ToString());
                        }
                    }
                }
                if (settled)
                {
                    sql.Append("delete from settled where addtime < @addtime \r\n");
                    sql.Append("delete from distribution where addtime < @addtime\r\n");
                    sql.Append("delete from trade where tradetime < @addtime");          
                }

                if (sql.Length > 0)
                {
                    try
                    {
                        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@addtime", SqlDbType.DateTime) };
                        parameters[0].Value = _endTime;
                        if (DataBase.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters) > 0)
                        {
                            this.lbmsg.Text = "清理成功";
                        }
                        else
                        {
                            this.lbmsg.Text = "清理失败";
                        }
                    }
                    catch (Exception ex)
                    {
                        this.lbmsg.Text = ex.Message;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageFactory.CheckSecondPwd();
            if (!base.IsPostBack)
            {
                this.EtimeBox.Text = DateTime.Now.AddDays(0.0).ToString("yyyy-MM-dd");
                this.EtimeBox.Attributes.Add("onFocus", string.Format("WdatePicker({{maxDate:'{0}'}})", DateTime.Now.AddDays(0.0).ToString("yyyy-MM-dd 00:00:00")));
            }
        }
    }
}

