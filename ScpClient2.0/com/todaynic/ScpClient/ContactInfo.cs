namespace com.todaynic.ScpClient
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ContactInfo
    {
        public string name;
        public string cnname;
        public string org;
        public string cnorg;
        public string cc;
        public string sp;
        public string pc;
        public string city;
        public string street;
        public string street1;
        public string voice;
        public string fax;
        public string email;
        public string mobile;
    }
}

