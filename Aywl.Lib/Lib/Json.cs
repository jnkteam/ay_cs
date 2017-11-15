using System.Data;
using System.Text;

namespace OriginalStudio.Lib
{
    /// <summary>
    /// Json�����ࡣ
    /// </summary>
    public class Json
	{
		StringBuilder sbJson = new StringBuilder();
		
		public Json()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		public void AddToJson(string TableName, DataTable DtSource)
		{
			if(this.sbJson.Length > 0) this.sbJson.Append(',');

			this.sbJson.Append("'" + TableName + "'"); 
			this.sbJson.Append(":["); 
			try 
			{
                if (DtSource.Rows.Count > 0)
                {
                    foreach (DataRow dr in DtSource.Rows)
                    {
                        this.sbJson.Append("{");
                        foreach (DataColumn dc in DtSource.Columns)
                        {
                            this.sbJson.Append("'" + dc.ColumnName + "':");
                            if (dr[dc] != null)
                            {
                                this.sbJson.Append("'" + dr[dc] + "',");
                            }
                        }
                        this.sbJson.Remove(this.sbJson.ToString().LastIndexOf(','), 1);
                        this.sbJson.Append("},");
                    }
                    this.sbJson.Remove(this.sbJson.ToString().LastIndexOf(','), 1);
                }
			}
			catch
			{
				this.sbJson.Append("'errorCode':");
				this.sbJson.Append("'0'");
			}	
			this.sbJson.Append("]");
            this.sbJson.Replace("'", "\"");
		}

		public void AddToJson(string Key,string Value)
		{
			if(this.sbJson.Length > 0) this.sbJson.Append(',');

            this.sbJson.Append("\"" + Key + "\":");
            this.sbJson.Append("\"" + Value + "\""); 
		}

        public void AddToJson(string Key, bool Value)
        {
            if (this.sbJson.Length > 0) this.sbJson.Append(',');

            this.sbJson.Append("\"" + Key + "\":");
            this.sbJson.Append(Value == true ? "true" : "false");
        }

		public void AddToJson(string Key,int Value)
		{
			if(this.sbJson.Length > 0) this.sbJson.Append(',');

            this.sbJson.Append("\"" + Key + "\":");
            this.sbJson.Append("\"" + Value.ToString() + "\""); 
		}

		public void AddToJson(string Key, Json Value)
		{
			if (Key != string.Empty)
			{
                if (this.sbJson.Length > 0) this.sbJson.Append(',');

                this.sbJson.Append("\"" + Key + "\":");
                this.sbJson.Append("\"" + Value.ToString() + "\""); 
			}
		}

        public void AddToJson(string SourceName, JsonTable JsTable)
        {
            if (this.sbJson.Length > 0) this.sbJson.Append(',');
            DataTable DtSource = JsTable.DataTable;

            try
            {
                if (DtSource.Rows.Count != 0)
                {
                    this.sbJson.Append("\"" + SourceName + "\"");
                    this.sbJson.Append(":[");

                    //����DataList
                    this.sbJson.Append("{\"" + JsTable.TableName + "\":");
                    this.sbJson.Append("[");

                    foreach (DataRow dr in DtSource.Rows)
                    {
                        this.sbJson.Append("{");

                        foreach (DataColumn dc in DtSource.Columns)
                        {
                            this.sbJson.Append("\"" + dc.ColumnName + "\":");
                            if (dr[dc] != null)
                            {
                                this.sbJson.Append("\"" + dr[dc] + "\",");
                            }
                        }
                        this.sbJson.Remove(this.sbJson.ToString().LastIndexOf(','), 1);
                        this.sbJson.Append("},");
                    }

                    this.sbJson.Remove(this.sbJson.Length - 1, 1);
                    this.sbJson.Append("]}");

                    this.sbJson.Append(",{\"RecordCount\":\"" + JsTable.RecordCount + "\"}");
                    this.sbJson.Append(",{\"PageCount\":\"" + JsTable.PageCount + "\"}");
                    this.sbJson.Append(",{\"PageSize\":\"" + JsTable.PageSize + "\"}");
                    this.sbJson.Append(",{\"PageIndex\":\"" + JsTable.PageIndex + "\"}");
                    //this.sbJson.Remove(this.sbJson.ToString().LastIndexOf(','), 1);
                    this.sbJson.Append("]");
                }
            }
            catch
            {
                this.sbJson.Append("\"errorCode\":");
                this.sbJson.Append("\"0\"");
            }
        }

        public void AddToJson(string Key, StringBuilder Value)
        {
            if (Key.Length > 0)
            {
                if (this.sbJson.Length > 0) this.sbJson.Append(',');

                this.sbJson.Append("\"" + Key + "\":");
                this.sbJson.Append("[" + Value.ToString() + "]");
            }
        }

		public override string ToString()
		{
            string T = this.sbJson.ToString();
			return "{" + T + "}";

			/*��ֹ���ToString()����sbJson�仯
			//��ͷ�ӡ�{��
			this.sbJson.Insert(0,"{"); 
			//ȥ�����һ����,��
			//this.sbJson.Remove(this.sbJson.ToString().LastIndexOf(','), 1); 
			//ĩβ�ӡ�}��
			this.sbJson.Append("}"); 

			return this.sbJson.ToString();
			*/
		}
	}

    /// <summary>
    /// DataTableתJSON���ݸ�ʽ��
    /// </summary>
    public class JsonTable
    {
        private string _TableName = "DataList";	//Ĭ�ϱ���
        private int _PageCount = 0;
        private int _RecordCount = 0;
        private int _PageIndex = 0;
        private int _PageSize = 0;
        private DataTable _DataTable = new DataTable();

        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            get { return this._TableName; }
            set { this._TableName = value; }
        }

        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int RecordCount
        {
            get
            {
                return _RecordCount;
            }
            set
            {
                this._RecordCount = value;
            }
        }

        public int PageCount
        {
            get { return this._PageCount; }
            set { this._PageCount = value; }
        }

        public int PageIndex
        {
            get { return this._PageIndex; }
            set { this._PageIndex = value; }
        }

        public int PageSize
        {
            get { return this._PageSize; }
            set { this._PageSize = value; }
        }

        public DataTable DataTable
        {
            get { return this._DataTable; }
            set { this._DataTable = value; }
        }

        public override string ToString()
        {
            StringBuilder Sb = new StringBuilder();
            return Sb.ToString();
        }
    }
}

