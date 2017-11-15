namespace OriginalStudio.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class CallBackInfo
    {
        public int error
        {
            get;
            set;
        }

        public string message
        {
            get;
            set;
        }

        public CallBackInfo()
        {
            this.error = 1;
            this.message = "error";
        }
    }
}

