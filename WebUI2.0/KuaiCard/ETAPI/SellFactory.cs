namespace KuaiCard.ETAPI
{
    using OriginalStudio.BLL;
    using OriginalStudio.BLL.Sys;
    using OriginalStudio.Model;
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.ExceptionHandling;
    using OriginalStudio.Lib.Security;
    using System;
    using System.Web;

    public class SellFactory
    {

        /// <summary>
        /// 代付款
        /// </summary>
        /// <param name="info"></param>
        public static void ReqDistribution(KuaiCard.Model.distribution info)
        {
            KuaiCardLib.Logging.LogHelper.Write("代付通道：" + info.suppid.ToString());
        }
    }
}

