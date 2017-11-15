namespace com.todaynic.ScpClient
{
    using System;

    public class DomainClient : XmlClient
    {
        private string m_VCPID;
        private string m_VCPPassword;

        public DomainClient(string HostName, int HostPort, string VcpID, string VcpPwd) : base(HostName, HostPort)
        {
            this.m_VCPID = VcpID;
            this.m_VCPPassword = VcpPwd;
        }

        public string createContact(string Domain, ContactInfo Contact)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":createContact");
            this.writeContactMessage(ContactType.None, Contact);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            return reply.getTextValue("/scp/response/resdata/contact:id");
        }

        public DomainInfo createDomain(DomainInfo domainInfo)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            string str = this.getTopDomain(domainInfo.Name);
            if (str.Trim().Length <= 0)
            {
                throw new Exception("domain name error");
            }
            base.m_XMLWriter.WriteElementString("action", "dot" + str + ":createDomain");
            base.m_XMLWriter.WriteElementString("domain", "name", null, domainInfo.Name);
            base.m_XMLWriter.WriteElementString("domain", "period", null, domainInfo.Period);
            base.m_XMLWriter.WriteElementString("domain", "rloginpassword", null, domainInfo.RLoginPassword);
            this.writeContactMessage(ContactType.registrant, domainInfo.RegistrantContactInfo);
            this.writeContactMessage(ContactType.admin, domainInfo.AdminContactInfo);
            this.writeContactMessage(ContactType.tech, domainInfo.TechContactInfo);
            this.writeContactMessage(ContactType.billing, domainInfo.BillingContactInfo);
            base.m_XMLWriter.WriteElementString("domain", "ns", null, domainInfo.NS1);
            base.m_XMLWriter.WriteElementString("domain", "ns", null, domainInfo.NS2);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            DomainInfo info2 = new DomainInfo();
            info2.Name = reply.getTextValue("/scp/response/resdata/domain:name");
            info2.ROID = reply.getTextValue("/scp/response/resdata/domain:roid");
            info2.RLoginPassword = reply.getTextValue("/scp/response/resdata/domain:RLoginPassword");
            info2.RegistrantContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"registrant\"]");
            info2.AdminContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"admin\"]");
            info2.TechContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"tech\"]");
            info2.BillingContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"billing\"]");
            info2.NS1 = reply.getTextValue("/scp/response/resdata/domain:ns1");
            info2.NS2 = reply.getTextValue("/scp/response/resdata/domain:ns2");
            info2.DtCreate = reply.getTextValue("/scp/response/resdata/domain:dtCreate");
            info2.DtUpdate = reply.getTextValue("/scp/response/resdata/domain:dtUpdate");
            info2.DtExpired = reply.getTextValue("/scp/response/resdata/domain:dtExpired");
            DomainInfo info = info2;
            this.resoveDomain(info.Name);
            return info;
        }

        public ContactInfo getContactInfo(string Domain, string ContactID)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":infoContact");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteElementString("contact", "id", null, ContactID);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            ContactInfo info = new ContactInfo();
            info.street = reply.getTextValue("/scp/response/resdata/Street");
            info.street1 = reply.getTextValue("/scp/response/resdata/Street1");
            info.mobile = reply.getTextValue("/scp/response/resdata/Mobile");
            info.org = reply.getTextValue("/scp/response/resdata/Organization");
            info.cnorg = reply.getTextValue("/scp/response/resdata/Organization_GB");
            info.pc = reply.getTextValue("/scp/response/resdata/PC");
            info.sp = reply.getTextValue("/scp/response/resdata/SP");
            info.voice = reply.getTextValue("/scp/response/resdata/Voice");
            info.cc = reply.getTextValue("/scp/response/resdata/CC");
            info.city = reply.getTextValue("/scp/response/resdata/City");
            info.fax = reply.getTextValue("/scp/response/resdata/Fax");
            info.name = reply.getTextValue("/scp/response/resdata/Name");
            info.cnname = reply.getTextValue("/scp/response/resdata/Name_GB");
            info.email = reply.getTextValue("/scp/response/resdata/Email");
            return info;
        }

        public DomainInfo getDomainInfo(string Domain)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":infoDomain");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            DomainInfo info = new DomainInfo();
            info.Status = reply.getTextValue("/scp/response/resdata/domain:status");
            info.Name = reply.getTextValue("/scp/response/resdata/domain:name");
            info.ROID = reply.getTextValue("/scp/response/resdata/domain:roid");
            info.RLoginPassword = reply.getTextValue("/scp/response/resdata/domain:RLoginPassword");
            info.RegistrantContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"registrant\"]");
            info.AdminContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"admin\"]");
            info.TechContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"tech\"]");
            info.BillingContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"billing\"]");
            info.NS1 = reply.getTextValue("/scp/response/resdata/domain:ns1");
            info.NS2 = reply.getTextValue("/scp/response/resdata/domain:ns2");
            info.DtCreate = reply.getTextValue("/scp/response/resdata/domain:dtCreate");
            info.DtUpdate = reply.getTextValue("/scp/response/resdata/domain:dtUpdate");
            info.DtExpired = reply.getTextValue("/scp/response/resdata/domain:dtExpired");
            return info;
        }

        private string getTopDomain(string Domain)
        {
            return Domain.Substring(Domain.LastIndexOf('.') + 1);
        }

        public bool isDomainCanBeRegisted(string Domain)
        {
            bool flag;
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            string str = this.getTopDomain(Domain);
            if (str.StartsWith("xn-"))
            {
                str = "ch";
            }
            base.m_XMLWriter.WriteElementString("action", "dot" + str + ":checkDomain");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            if (!bool.TryParse(reply.getTextValue("/scp/response/resdata/domain:name").Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1], out flag))
            {
                throw new SCPException(reply);
            }
            return flag;
        }

        public bool modifyDomainNS(string Domain, string newNS1, string newNS2)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":modifyDomain");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteElementString("domain", "newns", null, newNS1);
            base.m_XMLWriter.WriteElementString("domain", "newns", null, newNS2);
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

        public bool registNSServer(string topDomain, string ServerName)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + topDomain + ":createNS");
            base.m_XMLWriter.WriteElementString("host", "name", null, ServerName);
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

        public DomainInfo renewDomain(string Domain, string Period, string dtExpired)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":renewDomain");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteElementString("domain", "exDate", null, dtExpired);
            base.m_XMLWriter.WriteElementString("domain", "period", null, Period);
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            DomainInfo info = new DomainInfo();
            info.Name = reply.getTextValue("/scp/response/resdata/domain:name");
            info.ROID = reply.getTextValue("/scp/response/resdata/domain:roid");
            info.BillingContactID = reply.getTextValue("/scp/response/resdata/domain:contact[attribute::type=\"billing\"]");
            info.NS1 = reply.getTextValue("/scp/response/resdata/domain:ns1");
            info.NS2 = reply.getTextValue("/scp/response/resdata/domain:ns2");
            info.DtCreate = reply.getTextValue("/scp/response/resdata/domain:dtCreate");
            info.DtUpdate = reply.getTextValue("/scp/response/resdata/domain:dtUpdate");
            info.DtExpired = reply.getTextValue("/scp/response/resdata/domain:dtExpired");
            return info;
        }

        public bool resoveDomain(string DomainName)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(DomainName) + ":resoveDomain");
            base.m_XMLWriter.WriteElementString("domain", "name", null, DomainName);
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

        public bool setLoginPassword(string Domain, string loginpassword)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":setLoginPassword");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteElementString("domain", "loginpassword", null, loginpassword);
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

        public bool updateContact(string Domain, string ContactID, ContactType type, ContactInfo Contact)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "dot" + this.getTopDomain(Domain) + ":updateContact");
            base.m_XMLWriter.WriteElementString("domain", "name", null, Domain);
            base.m_XMLWriter.WriteElementString("contact", "id", null, ContactID);
            base.m_XMLWriter.WriteElementString("contact", "type", null, type.ToString());
            this.writeContactMessage(ContactType.None, Contact);
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

        private void writeContactMessage(ContactType type, ContactInfo contact)
        {
            string prefix = "contact";
            if (type == ContactType.registrant)
            {
                base.m_XMLWriter.WriteStartElement("domain", "registrant", null);
                prefix = "registrant";
            }
            else
            {
                base.m_XMLWriter.WriteStartElement("domain", "contact", null);
                if (type != ContactType.None)
                {
                    base.m_XMLWriter.WriteAttributeString("type", type.ToString());
                }
            }
            base.m_XMLWriter.WriteElementString(prefix, "name", null, contact.name);
            base.m_XMLWriter.WriteElementString(prefix, "cnname", null, contact.cnname);
            base.m_XMLWriter.WriteElementString(prefix, "org", null, contact.org);
            base.m_XMLWriter.WriteElementString(prefix, "cnorg", null, contact.cnorg);
            base.m_XMLWriter.WriteElementString(prefix, "cc", null, contact.cc);
            base.m_XMLWriter.WriteElementString(prefix, "sp", null, contact.sp);
            base.m_XMLWriter.WriteElementString(prefix, "pc", null, contact.pc);
            base.m_XMLWriter.WriteElementString(prefix, "city", null, contact.city);
            base.m_XMLWriter.WriteElementString(prefix, "street", null, contact.street);
            base.m_XMLWriter.WriteElementString(prefix, "street1", null, contact.street1);
            base.m_XMLWriter.WriteElementString(prefix, "voice", null, contact.voice);
            base.m_XMLWriter.WriteElementString(prefix, "fax", null, contact.fax);
            base.m_XMLWriter.WriteElementString(prefix, "email", null, contact.email);
            base.m_XMLWriter.WriteElementString(prefix, "mobile", null, contact.mobile);
            base.m_XMLWriter.WriteEndElement();
        }

        private void writeSCPEnd()
        {
            base.m_XMLWriter.WriteEndDocument();
        }

        private void writeSCPStart()
        {
            base.Initialize();
            base.m_XMLWriter.WriteStartElement("scp", "urn:scp:params:xml:ns:scp-3.0");
            base.m_XMLWriter.WriteAttributeString("xmlns", "domain", null, "urn:todaynic.com:domain");
            base.m_XMLWriter.WriteAttributeString("xmlns", "contact", null, "urn:todaynic.com:contact");
            base.m_XMLWriter.WriteAttributeString("xmlns", "registrant", null, "urn:todayisp.com:registrant");
            base.m_XMLWriter.WriteAttributeString("xmlns", "host", null, "urn:todaynic.com:host");
        }
    }
}

