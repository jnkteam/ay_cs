namespace com.todaynic.ScpClient
{
    using System;
    using System.Collections.Generic;

    public class SMSClient : XmlClient
    {
        private string m_SMSPassword;
        private string m_SMSUser;

        public SMSClient(string HostName, int HostProt, string SMSUser, string SMSPassword) : base(HostName, HostProt)
        {
            this.m_SMSUser = SMSUser;
            this.m_SMSPassword = SMSPassword;
        }

        public string getBalance()
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "SMS:infoSMSAccount");
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_SMSUser, this.m_SMSPassword, UserType.smsuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getTextValue("/scp/response/resdata/smsaccount");
        }

        public Dictionary<string, string> receiveSMS()
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "SMS:readSMS");
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_SMSUser, this.m_SMSPassword, UserType.smsuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            dictionary.Add("mess", reply.getTextValue("/scp/response/resdata/msg"));
            dictionary.Add("id", reply.getTextValue("/scp/response/resdata/id"));
            dictionary.Add("src", reply.getTextValue("/scp/response/resdata/src"));
            dictionary.Add("time", reply.getTextValue("/scp/response/resdata/time"));
            dictionary.Add("message", Utility.getBase64ToString(reply.getTextValue("/scp/response/resdata/message")));
            dictionary.Add("err", reply.getTextValue("/scp/response/resdata/err"));
            return dictionary;
        }

        public bool sendSMS(string MobilePhone, string Msg, string type)
        {
            this.writeSCPStart();
            string str = string.Empty;
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "SMS:sendSMS");
            base.m_XMLWriter.WriteElementString("sms", "mobile", null, MobilePhone);
            base.m_XMLWriter.WriteElementString("sms", "message", null, Utility.getStringToBase64(Msg));
            base.m_XMLWriter.WriteElementString("sms", "datetime", null, "");
            base.m_XMLWriter.WriteElementString("sms", "smstype", null, "");
            base.m_XMLWriter.WriteElementString("sms", "smsabout", null, "");
            base.m_XMLWriter.WriteElementString("sms", "sender", null, str);
            base.m_XMLWriter.WriteElementString("sms", "apitype", null, type);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_SMSUser, this.m_SMSPassword, UserType.smsuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return true;
        }

        public bool sendSMS(string MobilePhone, string Msg, string sendTime, string type)
        {
            this.writeSCPStart();
            string str = string.Empty;
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "SMS:sendSMS");
            base.m_XMLWriter.WriteElementString("sms", "mobile", null, MobilePhone);
            base.m_XMLWriter.WriteElementString("sms", "message", null, Utility.getBase64ToString(Msg));
            base.m_XMLWriter.WriteElementString("sms", "datetime", null, sendTime);
            base.m_XMLWriter.WriteElementString("sms", "smstype", null, "");
            base.m_XMLWriter.WriteElementString("sms", "smsabout", null, "");
            base.m_XMLWriter.WriteElementString("sms", "sender", null, str);
            base.m_XMLWriter.WriteElementString("sms", "apitype", null, type);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_SMSUser, this.m_SMSPassword, UserType.smsuser);
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
            base.m_XMLWriter.WriteAttributeString("xmlns", "user", null, "urn:mobile:user");
            base.m_XMLWriter.WriteAttributeString("xmlns", "sms", null, "urn:mobile:sms");
        }

        public string SMSUser
        {
            get
            {
                return this.m_SMSUser;
            }
            set
            {
                this.m_SMSUser = value;
            }
        }
    }
}

