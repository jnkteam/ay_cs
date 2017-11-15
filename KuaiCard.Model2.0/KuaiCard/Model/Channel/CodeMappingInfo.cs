namespace KuaiCard.Model.Channel
{
    using System;

    [Serializable]
    public class CodeMappingInfo
    {
        private int _id;
        private string _pmodecode;
        private string _suppcode;
        private int _suppid;

        public int id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string pmodeCode
        {
            get
            {
                return this._pmodecode;
            }
            set
            {
                this._pmodecode = value;
            }
        }

        public string suppCode
        {
            get
            {
                return this._suppcode;
            }
            set
            {
                this._suppcode = value;
            }
        }

        public int suppId
        {
            get
            {
                return this._suppid;
            }
            set
            {
                this._suppid = value;
            }
        }
    }
}

