namespace OriginalStudio.Lib.SysConfig
{
    using OriginalStudio.Lib.Configuration;
    using System;

    /// <summary>
    /// 消息队列配置
    /// </summary>
    public sealed class MSMQSetting
    {
        private static readonly string _group = "MSMQ";

        private MSMQSetting()
        {
        }

        #region 固定配置

        //==========Order =================
        public static string OrderMessagingAssembly
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderMessagingAssembly");
            }
        }

        public static string OrderMessagingClass
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "OrderMessagingClass");
            }
        }
        //==========Order =================

        #endregion


        public static string BankNotifyPath
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "BankNotifyPath");
            }
        }

        public static string BankOrderPath
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "BankOrderPath");
            }
        }

        public static int BatchSize
        {
            get
            {
                int result = 10;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "BatchSize"), out result))
                {
                    result = 10;
                }
                return result;
            }
        }

        public static int BatchSize_CardOrder
        {
            get
            {
                int result = 10;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "BatchSize_CardOrder"), out result))
                {
                    result = 10;
                }
                return result;
            }
        }

        public static string CardNotifyPath
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "CardNotifyPath");
            }
        }

        public static string CardOrderPath
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "CardOrderPath");
            }
        }

        public static int NotifyBatchSize
        {
            get
            {
                int result = 10;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyBatchSize"), out result))
                {
                    result = 10;
                }
                return result;
            }
        }

        public static int NotifyBatchSize_Card
        {
            get
            {
                int result = 10;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyBatchSize_Card"), out result))
                {
                    result = 10;
                }
                return result;
            }
        }

        public static int NotifyQueueTimeout
        {
            get
            {
                int result = 20;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyQueueTimeout"), out result))
                {
                    result = 20;
                }
                return result;
            }
        }

        public static int NotifyQueueTimeout_Card
        {
            get
            {
                int result = 20;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyQueueTimeout_Card"), out result))
                {
                    result = 20;
                }
                return result;
            }
        }

        public static int NotifyThreadCount
        {
            get
            {
                int result = 2;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyThreadCount"), out result))
                {
                    result = 2;
                }
                return result;
            }
        }

        public static int NotifyThreadCount_Card
        {
            get
            {
                int result = 2;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyThreadCount_Card"), out result))
                {
                    result = 2;
                }
                return result;
            }
        }

        public static int NotifyTransactionTimeout
        {
            get
            {
                int result = 30;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyTransactionTimeout"), out result))
                {
                    result = 30;
                }
                return result;
            }
        }

        public static int NotifyTransactionTimeout_Card
        {
            get
            {
                int result = 30;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "NotifyTransactionTimeout_Card"), out result))
                {
                    result = 30;
                }
                return result;
            }
        }

        public static int QueueTimeout
        {
            get
            {
                int result = 20;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "QueueTimeout"), out result))
                {
                    result = 20;
                }
                return result;
            }
        }

        public static int QueueTimeout_CardOrder
        {
            get
            {
                int result = 20;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "QueueTimeout_CardOrder"), out result))
                {
                    result = 20;
                }
                return result;
            }
        }

        public static string SettingGroup
        {
            get
            {
                return _group;
            }
        }

        public static string SmsNotifyPath
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SmsNotifyPath");
            }
        }

        public static string SmsOrderPath
        {
            get
            {
                return ConfigHelper.GetConfig(SettingGroup, "SmsOrderPath");
            }
        }

        public static int ThreadCount
        {
            get
            {
                int result = 2;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "ThreadCount"), out result))
                {
                    result = 2;
                }
                return result;
            }
        }

        public static int ThreadCount_CardOrder
        {
            get
            {
                int result = 2;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "ThreadCount_CardOrder"), out result))
                {
                    result = 2;
                }
                return result;
            }
        }

        public static int TransactionTimeout
        {
            get
            {
                int result = 30;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "TransactionTimeout"), out result))
                {
                    result = 30;
                }
                return result;
            }
        }

        public static int TransactionTimeout_CardOrder
        {
            get
            {
                int result = 30;
                if (!int.TryParse(ConfigHelper.GetConfig(SettingGroup, "TransactionTimeout_CardOrder"), out result))
                {
                    result = 30;
                }
                return result;
            }
        }
    }
}

