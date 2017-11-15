namespace com.todaynic.ScpClient
{
    using System;
    using System.Collections.Generic;

    public class VHostClient : XmlClient
    {
        private string m_VCPID;
        private string m_VCPPassword;

        public VHostClient(string HostName, int HostPort, string VcpID, string VcpPwd) : base(HostName, HostPort)
        {
            this.m_VCPID = VcpID;
            this.m_VCPPassword = VcpPwd;
        }

        public bool addBindDomain(string VHostID, string Domain)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:addBindDomain");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "domain", null, Domain);
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

        public bool bindDomain(string VHostID, string Domain, string TomcatBase)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:bindDomain");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "domain", null, Domain);
            base.m_XMLWriter.WriteElementString("vhost", "tomcatbase", null, TomcatBase);
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

        public bool changeFTPPassword(string VHostID, string OldPassword, string NewPassword)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:changeFTPPassword");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "oldpwd", null, OldPassword);
            base.m_XMLWriter.WriteElementString("vhost", "newpwd", null, NewPassword);
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

        public bool changeVHostConfig(string VHostID, string NewProductID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:changeVHostConfig");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "productid", null, NewProductID);
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

        public bool createVDir(string VHostID, string VDirName)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:createVDir");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "vdirname", null, VDirName);
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

        public Dictionary<string, string> createVHost(VHostInfo vhostInfo)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:createVHost");
            base.m_XMLWriter.WriteElementString("vhost", "productid", null, vhostInfo.ProductID);
            base.m_XMLWriter.WriteElementString("vhost", "domain", null, vhostInfo.Domain);
            base.m_XMLWriter.WriteElementString("vhost", "username", null, vhostInfo.UserName);
            base.m_XMLWriter.WriteElementString("vhost", "password", null, vhostInfo.Password);
            base.m_XMLWriter.WriteElementString("vhost", "period", null, vhostInfo.Period);
            base.m_XMLWriter.WriteElementString("vhost", "vhostquota", null, vhostInfo.Quota);
            base.m_XMLWriter.WriteElementString("vhost", "dbquota", null, vhostInfo.DBQuota);
            base.m_XMLWriter.WriteElementString("vhost", "emailquota", null, vhostInfo.EmailQuota);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata/vhostinfo");
        }

        public bool deleteVDir(string VHostID, string VDirName)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:deleteVDir");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "vdirname", null, VDirName);
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

        public bool fixVHost(string VHostID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:fixVHost");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
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

        public Dictionary<string, string> getDefaultDoc(string VHostID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:getDefaultDocList");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata/DefaultDoc");
        }

        public Dictionary<string, string> getIISLogList(string VHostID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:getIISLogList");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata/loglist");
        }

        public Dictionary<string, string> getRemoteVHostInfo(string VHostID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:getRemoteVHostInfo");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata/vhostinfo");
        }

        public Dictionary<string, string> getVDirList(string VHostID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:getVDirList");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata/vdirlist");
        }

        public Dictionary<string, string> getVHostInfo(string VHostID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:getVHostInfo");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getDictionaryValue("/scp/response/resdata/vhostinfo");
        }

        public bool initializeFTPPassword(string VHostID, string Password)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:initializeFTPPassword");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "password", null, Password);
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

        public bool moveIISLogToFTPDir(string VHostID, string LogFileName)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:moveIISLogToFtpDir");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "iislogfilename", null, LogFileName);
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

        public bool removeBindDomain(string VHostID, string Domain)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:removeBindDomain");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "domain", null, Domain);
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

        public bool renewVHost(string VHostID, string Period, string dtExpired)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:renewVHost");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "period", null, Period);
            base.m_XMLWriter.WriteElementString("vhost", "curexpired", null, dtExpired);
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

        public bool setDefaultDoc(string VHostID, string DefDoc)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "vhost:setDefaultDoc");
            base.m_XMLWriter.WriteElementString("vhost", "vhostid", null, VHostID);
            base.m_XMLWriter.WriteElementString("vhost", "defaultdoc", null, DefDoc);
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
            base.m_XMLWriter.WriteAttributeString("xmlns", "vhost", null, "urn:todaynic.com:vhost");
        }
    }
}

