namespace KuaiCard.ETAPI
{
    using KuaiCard.BLL;
    using KuaiCard.BLL.Sys;
    using KuaiCard.Model;
    using KuaiCard.SysConfig;
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.Security;
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

