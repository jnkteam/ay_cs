namespace OriginalStudio.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class OrderNotify
    {
        public Timer tmr;

        public OrderBankInfo orderInfo
        {
            get;
            set;
        }
    }
}

