namespace OriginalStudio.MSMQMessaging
{
    using System;
    using System.Messaging;

    public class BaseQueue : IDisposable
    {
        protected MessageQueue queue;
        protected TimeSpan timeout;
        protected MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queuePath"></param>
        /// <param name="timeoutSeconds">毫秒时间差</param>
        public BaseQueue(string queuePath, int timeoutSeconds)
        {
            try
            {
                //OriginalStudio.Lib.Logging.LogHelper.Write("queuePath：" + queuePath);
                if (!MessageQueue.Exists(queuePath))
                    MessageQueue.Create(queuePath);
            }
            catch(Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("BaseQueue.BaseQueue错误：" + err.Message.ToString());
            }
            this.queue = new MessageQueue(queuePath);
            this.queue.Formatter = new BinaryMessageFormatter();

            this.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeoutSeconds));
            this.queue.DefaultPropertiesToSend.AttachSenderId = false;
            this.queue.DefaultPropertiesToSend.UseAuthentication = false;
            this.queue.DefaultPropertiesToSend.UseEncryption = false;
            this.queue.DefaultPropertiesToSend.AcknowledgeType = AcknowledgeTypes.None;
            this.queue.DefaultPropertiesToSend.UseJournalQueue = false;

            // 设置是否经过身份验证：默认为否
            this.queue.Authenticate = true;
            // 是否启用日志队列
            this.queue.UseJournalQueue = true;

            //设置权限
            this.queue.SetPermissions("Everyone",
                MessageQueueAccessRights.FullControl | MessageQueueAccessRights.GenericRead | MessageQueueAccessRights.DeleteMessage |
                MessageQueueAccessRights.DeleteQueue | MessageQueueAccessRights.DeleteJournalMessage);
            this.queue.SetPermissions("ANONYMOUS LOGON",
                MessageQueueAccessRights.FullControl | MessageQueueAccessRights.GenericRead | MessageQueueAccessRights.DeleteMessage |
                MessageQueueAccessRights.DeleteQueue | MessageQueueAccessRights.DeleteJournalMessage);
        }

        public void Dispose()
        {
            this.queue.Dispose();
        }

        public virtual object Receive()
        {
            //object obj2;
            //try
            //{
            //    using (Message message = this.queue.Receive(this.timeout, this.transactionType))
            //    {
            //        obj2 = message;
            //    }
            //}
            //catch (MessageQueueException exception)
            //{
            //    if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
            //    {
            //        throw new TimeoutException();
            //    }
            //    throw;
            //}
            //return obj2;
            return null;
        }

        public virtual void Send(object msg)
        {
            OriginalStudio.Model.Order.OrderBankInfo order = (OriginalStudio.Model.Order.OrderBankInfo)msg;
            OriginalStudio.Lib.Logging.LogHelper.Write("异步通知失败！发送消息Send：" + order.orderid);

            try
            {
                this.queue.Send("11", MessageQueueTransactionType.Automatic);
                //this.queue.Send("111111", this.transactionType);
                OriginalStudio.Lib.Logging.LogHelper.Write("异步发送消息。");
            }
            catch (Exception err)
            {
                OriginalStudio.Lib.Logging.LogHelper.Write("异步发送消息错误：" + err.Message);
            }
        }
    }
}

