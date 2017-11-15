namespace Aywl.DBAccess
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading;

    public class CommandInfo
    {
        public string CommandText;
        public Aywl.DBAccess.EffentNextType EffentNextType;
        public object OriginalData;
        public DbParameter[] Parameters;
        public object ShareObject;

        private event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add
            {
                this._solicitationEvent += value;
            }
            remove
            {
                this._solicitationEvent -= value;
            }
        }

        public CommandInfo()
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = Aywl.DBAccess.EffentNextType.None;
        }

        public CommandInfo(string sqlText, SqlParameter[] para)
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = Aywl.DBAccess.EffentNextType.None;
            this.CommandText = sqlText;
            this.Parameters = (DbParameter[]) para;
        }

        public CommandInfo(string sqlText, SqlParameter[] para, Aywl.DBAccess.EffentNextType type)
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = Aywl.DBAccess.EffentNextType.None;
            this.CommandText = sqlText;
            this.Parameters = (DbParameter[]) para;
            this.EffentNextType = type;
        }

        public void OnSolicitationEvent()
        {
            if (this._solicitationEvent != null)
            {
                this._solicitationEvent(this, new EventArgs());
            }
        }
    }
}

