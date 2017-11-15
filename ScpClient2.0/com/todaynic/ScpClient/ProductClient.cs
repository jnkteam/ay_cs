namespace com.todaynic.ScpClient
{
    using System;
    using System.Collections.Generic;

    public class ProductClient : XmlClient
    {
        private string m_VCPID;
        private string m_VCPPassword;

        public ProductClient(string HostName, int HostPort, string VcpID, string VcpPwd) : base(HostName, HostPort)
        {
            this.m_VCPID = VcpID;
            this.m_VCPPassword = VcpPwd;
        }

        public Dictionary<string, string> getProductList(int Category)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "product:getProductList");
            base.m_XMLWriter.WriteElementString("category", Category.ToString());
            base.m_XMLWriter.WriteEndElement();
            base.writeSecurityMessage(this.m_VCPID, this.m_VCPPassword, UserType.vcpuser);
            this.writeSCPEnd();
            SCPReply reply = base.send();
            if (reply.Status != ScpStatus.Successfully)
            {
                throw new SCPException(reply);
            }
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> list = reply.getTextListValue("/scp/response/resdata/product");
            for (int i = 0; i < list.Count; i++)
            {
                string[] strArray = list[i].Split(new char[] { ':' }, StringSplitOptions.None);
                dictionary.Add(strArray[0], strArray[strArray.Length - 1]);
            }
            return dictionary;
        }

        public Dictionary<string, string> getProductPrice(string IDProduct, string year, string quota)
        {
            this.writeSCPStart();
            base.m_XMLWriter.WriteStartElement("command");
            base.m_XMLWriter.WriteElementString("action", "product:getProductPrice");
            base.m_XMLWriter.WriteElementString("productid", IDProduct);
            base.m_XMLWriter.WriteElementString("year", year);
            base.m_XMLWriter.WriteElementString("quota", quota);
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

        private void writeSCPEnd()
        {
            base.m_XMLWriter.WriteEndDocument();
        }

        private void writeSCPStart()
        {
            base.m_XMLWriter.WriteStartElement("scp", "urn:scp:params:xml:ns:scp-3.0");
        }
    }
}

