namespace com.todaynic.ScpClient
{
    using System;
    using System.Collections.Generic;

    public class SCPReply
    {
        private XMLParser m_XMLParser;

        public SCPReply(string ReceiveXML)
        {
            this.m_XMLParser = new XMLParser(ReceiveXML);
        }

        public Dictionary<string, string> getDictionaryValue(string xPath)
        {
            return this.m_XMLParser.getDictionaryOfANode(xPath);
        }

        public string getResultCode()
        {
            return this.m_XMLParser.getAttributeTextOfNode("/scp/response/result", "code");
        }

        public string getResultMessage()
        {
            return this.m_XMLParser.getSingleNodeInnerText("/scp/response/result/msg");
        }

        public List<string> getTextListValue(string xPath)
        {
            return this.m_XMLParser.getListOfANode(xPath);
        }

        public string getTextValue(string xPath)
        {
            return this.m_XMLParser.getSingleNodeInnerText(xPath);
        }

        public ScpStatus Status
        {
            get
            {
                if (this.getResultCode() == "2000")
                {
                    return ScpStatus.Successfully;
                }
                return ScpStatus.Error;
            }
        }
    }
}

