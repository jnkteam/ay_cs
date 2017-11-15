namespace KuaiCard.Model.Order
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CardItem
    {
        public int seq
        {
            get;
            set;
        }

        public string sysorderid
        {
            get;
            set;
        }
    }
}

