namespace OriginalStudio.Model.BaseData
{
    using System;

    [Serializable]
    public class identitycard
    {
        private string _bm;
        private string _dq;
        private int _id;

        public string BM
        {
            get
            {
                return this._bm;
            }
            set
            {
                this._bm = value;
            }
        }

        public string DQ
        {
            get
            {
                return this._dq;
            }
            set
            {
                this._dq = value;
            }
        }

        public int ID
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
    }
}

