namespace KuaiCard.SysConfig
{
    using KuaiCardLib.Configuration;
    using System;
    using System.Xml;

    public sealed class PaymentSetting
    {
        private static readonly string _group = "paymentSettings";

        private PaymentSetting()
        {
        }

        public static string alipay_body
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "alipay_body");
            }
        }

        public static string alipay_subject
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "alipay_subject");
            }
        }

        public static string Gopay_userType
        {
            get
            {
                string config = ConfigHelper.GetConfig(SettingGroup, "Gopay_userType");
                if (string.IsNullOrEmpty(config))
                {
                    config = "1";
                }
                return config;
            }
        }

        public static string jumpUrl
        {
            get
            {
                try
                {
                    return ConfigHelper.GetConfig(SettingGroup, "jumpUrl");
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public static string KuaiQian_prikey_path
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "KuaiQian_prikey_path");
            }
        }

        public static string KuaiQian_pubkey_path
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "KuaiQian_pubkey_path");
            }
        }

        public static string mengsmsarrCom
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "mengsmsarrCom");
            }
        }

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        public static string shenzhoufucertificate
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "shenzhoufucertificate");
            }
        }

        public static bool showjubao
        {
            get
            {
                try
                {
                    string config = ConfigHelper.GetConfig(SettingGroup, "showjubao");
                    return (!string.IsNullOrEmpty(config) && (Convert.ToInt32(config) > 0));
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static string switch_alipay_form_url
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "switch_alipay_form_url");
            }
        }

        public static string switch_ipspay_form_url
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "switch_ipspay_form_url");
            }
        }

        public static string switch_sdopay_form_url
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "switch_sdopay_form_url");
            }
        }

        public static string switch_tenpay_form_url
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "switch_tenpay_form_url");
            }
        }

        public static string switch_yeepay_form_url
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "switch_yeepay_form_url");
            }
        }

        public static string tftpay_MerBusType
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "tftpay_MerBusType");
            }
        }

        public static string tftpay_MerLicences
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "tftpay_MerLicences");
            }
        }

        public static string tftpay_PostAdd
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "tftpay_PostAdd");
            }
        }

        public static string tftpay_TBLicences
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "tftpay_TBLicences");
            }
        }

        public static string weixin_body
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "weixin_body");
            }
        }

        public static string weixin_subject
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "weixin_subject");
            }
        }

        public static string yeepay_pcat
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "yeepay_pcat");
            }
        }

        public static string yeepay_pdesc
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "yeepay_pdesc");
            }
        }

        public static string yeepay_pid
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "yeepay_pid");
            }
        }

        public static string yisheng_buyer_realname
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "yisheng_buyer_realname");
            }
        }


        /// <summary>
        /// 取随机商品名称
        /// </summary>
        public static string random_product 
        {
            get
            {
                try
                {
                    //取配置文件
                    string p_config = System.Web.Configuration.WebConfigurationManager.AppSettings["random_product"].ToString();
                    string p_filename = System.Web.HttpContext.Current.Server.MapPath(p_config);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(p_filename); //加载xml文件
                    XmlNodeList xns = xmlDoc.SelectSingleNode("Configuration").ChildNodes;
                    int totalCnt = xns.Count;
                    Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
                    int RandKey = ran.Next(0, totalCnt - 1);
                    string prodname = xns[RandKey].Attributes["Name"].Value.ToString();

                    return prodname;
                }
                catch (Exception err)
                {
                    KuaiCardLib.Logging.LogHelper.WriteErrMsg("random_product函数错误：" + err.Message.ToString());
                    return PaymentSetting.alipay_body;
                }
            }
        }

        /// <summary>
        /// 取随机阿里商铺名称
        /// </summary>
        public static KuaiCard.Model.RandomStore random_alipay_store
        {
            get
            {
                KuaiCard.Model.RandomStore rs = new Model.RandomStore();

                try
                {
                    //取配置文件
                    string p_config = System.Web.Configuration.WebConfigurationManager.AppSettings["random_alipay_store"].ToString();
                    string p_filename = System.Web.HttpContext.Current.Server.MapPath(p_config);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(p_filename); //加载xml文件
                    XmlNodeList xns = xmlDoc.SelectSingleNode("Configuration").ChildNodes;
                    int totalCnt = xns.Count;
                    Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
                    int RandKey = ran.Next(0, totalCnt - 1);

                    rs.StoreNo = xns[RandKey].Attributes["No"].Value.ToString();
                    rs.StoreName = xns[RandKey].Attributes["Name"].Value.ToString();
                    rs.StoreEngName = xns[RandKey].Attributes["EngName"].Value.ToString();

                    return rs;
                }
                catch (Exception err)
                {
                    KuaiCardLib.Logging.LogHelper.WriteErrMsg("random_alipay_store函数错误：" + err.Message.ToString());
                    return rs;
                }
            }
        }

        /// <summary>
        /// 取随机微信商铺名称
        /// </summary>
        public static KuaiCard.Model.RandomStore random_wx_store
        {
            get
            {
                KuaiCard.Model.RandomStore rs = new Model.RandomStore();

                try
                {
                    //取配置文件
                    string p_config = System.Web.Configuration.WebConfigurationManager.AppSettings["random_wx_store"].ToString();
                    string p_filename = System.Web.HttpContext.Current.Server.MapPath(p_config);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(p_filename); //加载xml文件
                    XmlNodeList xns = xmlDoc.SelectSingleNode("Configuration").ChildNodes;
                    int totalCnt = xns.Count;
                    Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
                    int RandKey = ran.Next(0, totalCnt - 1);

                    rs.StoreNo = xns[RandKey].Attributes["No"].Value.ToString();
                    rs.StoreName = xns[RandKey].Attributes["Name"].Value.ToString();
                    rs.StoreEngName = xns[RandKey].Attributes["EngName"].Value.ToString();

                    return rs;
                }
                catch (Exception err)
                {
                    KuaiCardLib.Logging.LogHelper.WriteErrMsg("random_alipay_store函数错误：" + err.Message.ToString());
                    return rs;
                }
            }
        }
    }

}

