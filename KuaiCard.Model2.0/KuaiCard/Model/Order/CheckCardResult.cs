namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class CheckCardResult
    {
        public string cardpwd
        {
            get;
            set;
        }

        public byte isRepeat
        {
            get;
            set;
        }

        public byte makeup
        {
            get;
            set;
        }

        public int supplierid
        {
            get;
            set;
        }

        public decimal supprate
        {
            get;
            set;
        }

        public decimal withhold
        {
            get;
            set;
        }
    }
}

