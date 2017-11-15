namespace com.todaynic.ScpClient
{
    using System;

    public class SCPException : Exception
    {
        private SCPReply m_Reply;

        public SCPException(SCPReply reply) : base(reply.getResultMessage())
        {
            this.m_Reply = reply;
        }

        public string ResultCode
        {
            get
            {
                return this.m_Reply.getResultCode();
            }
        }
    }
}

