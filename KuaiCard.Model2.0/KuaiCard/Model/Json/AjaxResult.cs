namespace KuaiCard.Model.Json
{
    using System;
    using System.Runtime.CompilerServices;

    public class AjaxResult
    {
        public string msg
        {
            get;
            set;
        }

        public string result
        {
            get;
            set;
        }

        public AjaxResult()
        {
            this.result = "fail";
            this.msg = "未知错误";
        }
    }

}

