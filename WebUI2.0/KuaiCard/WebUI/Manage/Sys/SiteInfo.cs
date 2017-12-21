namespace OriginalStudio.WebUI.Manage.Sys
{
    using OriginalStudio.BLL;
    using OriginalStudio.Model;
    using OriginalStudio.WebComponents.Web;
    using OriginalStudio.Lib;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Data;
    using System.Text;
    using OriginalStudio.Lib.Web;
    using System.Xml;

    public class SiteInfo : ManagePageBase
    {
        private WebInfo _objectInfo = null;
        protected Button btn_Update;      
        protected HtmlForm form1;
        protected DataSet allOptionsSet;
        protected HiddenField hiddenNameValue;
        protected DataTable tableOptionsType;

        protected DataTable test1;
        protected DataTable test2;

        protected DataTable test5;


        protected void Bind()
        {
            DataSet setOptionsTypeList = SysConfig.GetOptionsTypeList(string.Empty);
            DataTable tableOptionsType = setOptionsTypeList.Tables[0];
            this.tableOptionsType = tableOptionsType;
            DataSet coSet = new DataSet();
            foreach (DataRow row in tableOptionsType.Rows)
            {
                DataTable a = SysConfig.GetSysOptionsByTypeId(Convert.ToInt32(row["type_id"])).Tables[0];
                a.TableName = row["type_id"].ToString();
                coSet.Tables.Add(a.Copy());
                
            }
   
            this.allOptionsSet = coSet;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            this.Bind();
            DataTable dtConfig = new DataTable("Table_New");
           
            dtConfig.Columns.Add("option_code", typeof(String));
            dtConfig.Columns.Add("new_value", typeof(String));
            dtConfig.Columns.Add("option_value", typeof(String));


           
            //string[] itemArr = this.hiddenNameValue.Value.Split('|'); //传递的字符串
   
            XmlDocument xmlDoc = new XmlDocument();          
            xmlDoc.LoadXml(this.hiddenNameValue.Value); 
            XmlNode rootNode = xmlDoc.SelectSingleNode("HH");
            foreach (XmlNode xxNode in rootNode.ChildNodes)
            {
               
                string code = xxNode.Name;
                string name = xxNode.InnerText;
                string oriOpVal = SysConfig.GetOptionValue(code); //为改变之前的值
                dtConfig.Rows.Add(code, name, oriOpVal);
                
            }
        
            if (SysConfig.UpdateSysOptions(dtConfig))
            {
                base.AlertAndRedirect("操作成功");
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.setPower();
            if (!base.IsPostBack)
            {
                this.Bind();
                if (this.cmd == "update") {

                    
                }

           
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
        public string cmd
        {
            get
            {
                return WebBase.GetQueryStringString("cmd", "");
            }
        }
        public WebInfo ObjectInfo
        {
            get
            {
                if (this._objectInfo == null)
                {
                    this._objectInfo = WebInfoFactory.GetWebInfoByDomain(XRequest.GetHost());
                }
                return this._objectInfo;
            }
        }
    }
}

