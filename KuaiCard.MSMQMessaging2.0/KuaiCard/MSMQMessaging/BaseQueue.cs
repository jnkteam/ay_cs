namespace KuaiCard.MSMQMessaging
{
    using System;
    using System.Messaging;

    public class BaseQueue : IDisposable
    {
        protected MessageQueue queue;
        protected TimeSpan timeout;
        protected MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;

        public BaseQueue(string queuePath, int timeoutSeconds)
        {
            try
            {
                if (!MessageQueue.Exists(queuePath))
                    MessageQueue.Create(queuePath);
            }
            catch(Exception err)
            {
                KuaiCardLib.Logging.LogHelper.Write("BaseQueue.BaseQueue错误：" + err.Message.ToString());
            }
            this.queue = new MessageQueue(queuePath);
            this.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeoutSeconds));
            this.queue.DefaultPropertiesToSend.AttachSenderId = false;
            this.queue.DefaultPropertiesToSend.UseAuthentication = false;
            this.queue.DefaultPropertiesToSend.UseEncryption = false;
            this.queue.DefaultPropertiesToSend.AcknowledgeType = AcknowledgeTypes.None;
            this.queue.DefaultPropertiesToSend.UseJournalQueue = false;
        }

        public void Dispose()
        {
            this.queue.Dispose();
        }

        public virtual object Receive()
        {
            object obj2;
            try
            {
                using (Message message = this.queue.Receive(this.timeout, this.transactionType))
                {
                    obj2 = message;
                }
            }
            catch (MessageQueueException exception)
            {
                if (exception.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    throw new TimeoutException();
                }
                throw;
            }
            return obj2;
        }

        public virtual void Send(object msg)
        {
            this.queue.Send(msg, this.transactionType);
        }
    }
}

