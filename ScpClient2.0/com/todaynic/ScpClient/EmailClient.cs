namespace com.todaynic.ScpClient
{
    using System;
    using System.Collections.Generic;

    public class EmailClient : XmlClient
    {
        private string m_VCPID;
        private string m_VCPPassword;

        public EmailClient(string HostName, int HostPort, string VcpID, string VcpPwd) : base(HostName, HostPort)
        {
            this.m_VCPID = VcpID;
            this.m_VCPPassword = VcpPwd;
        }

        public Dictionary<string, string> createEmail(EmailInfo emailInfo)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "email:createEmail");
            base.m_XMLWriter.WriteElementString("email", "productid", null, emailInfo.ProductID);
            base.m_XMLWriter.WriteElementString("email", "domain", null, emailInfo.Domain);
            base.m_XMLWriter.WriteElementString("email", "period", null, emailInfo.Period);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata");
        }

        public EmailInfo getEmailInfo(string EmailID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "email:infoEmail");
            base.m_XMLWriter.WriteElementString("email", "id", null, EmailID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            EmailInfo info = new EmailInfo();
            info.Status = reply.getTextValue("/scp/response/resdata/active");
            info.AttachVHostID = reply.getTextValue("/scp/response/resdata/vhost");
            info.Space = reply.getTextValue("/scp/response/resdata/space");
            info.MailServer = reply.getTextValue("/scp/response/resdata/mailserver");
            info.Password = reply.getTextValue("/scp/response/resdata/pwd");
            info.Domain = reply.getTextValue("/scp/response/resdata/domain");
            info.DtCreate = reply.getTextValue("/scp/response/resdata/datecreate");
            info.DtExpired = reply.getTextValue("/scp/response/resdata/dateexpired");
            return info;
        }

        public bool renewEmail(string EmailID, string Period, string dtExpired)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "email:renewEmail");
            base.m_XMLWriter.WriteElementString("email", "id", null, EmailID);
            base.m_XMLWriter.WriteElementString("email", "dateexpired", null, dtExpired);
            base.m_XMLWriter.WriteElementString("email", "period", null, Period);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return true;
        }

        private void writeSCPEnd()
        {
            base.m_XMLWriter.WriteEndDocument();
        }

        private void writeSCPStart()
        {
            base.m_XMLWriter.WriteStartElement("scp", "urn:scp:params:xml:ns:scp-3.0");
            base.m_XMLWriter.WriteAttributeString("xmlns", "email", null, "urn:todaynic.com:email");
        }
    }
}

