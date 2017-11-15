namespace com.todaynic.ScpClient
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    public class XMLParser
    {
        private string m_XMLData;
        private XmlDocument m_XMLDocumet;
        private XmlNamespaceManager m_XMLNamespaceManager;

        public XMLParser(string XMLData)
        {
            this.m_XMLData = XMLData;
            this.m_XMLDocumet = new XmlDocument();
            this.m_XMLDocumet.LoadXml(XMLData);
            this.m_XMLNamespaceManager = new XmlNamespaceManager(this.m_XMLDocumet.NameTable);
            XmlElement documentElement = this.m_XMLDocumet.DocumentElement;
            if (documentElement.HasAttributes)
            {
                foreach (XmlAttribute attribute in documentElement.Attributes)
                {
                    if (!(attribute.LocalName == "xmlns"))
                    {
                        this.m_XMLNamespaceManager.AddNamespace(attribute.LocalName, attribute.Value);
                    }
                }
            }
        }

        public string getAttributeTextOfNode(string xpath, string attributeName)
        {
            XmlNode node = this.m_XMLDocumet.DocumentElement.SelectSingleNode(xpath, this.m_XMLNamespaceManager);
            if (node != null)
            {
                return node.Attributes[attributeName].InnerText;
            }
            return string.Empty;
        }

        public Dictionary<string, string> getDictionaryOfANode(string xpath)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (XmlNode node in this.m_XMLDocumet.DocumentElement.SelectSingleNode(xpath, this.m_XMLNamespaceManager).ChildNodes)
            {
                if (!dictionary.ContainsKey(node.Name))
                {
                    dictionary.Add(node.Name, node.InnerXml);
                }
            }
            return dictionary;
        }

        public List<string> getListOfANode(string xpath)
        {
            List<string> list = new List<string>();
            string[] strArray = this.getNodesInnerText(xpath).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(strArray[i]))
                {
                    list.Add(strArray[i]);
                }
            }
            return list;
        }

        public string getNodesInnerText(string xpath)
        {
            XmlNodeList list = this.m_XMLDocumet.DocumentElement.SelectNodes(xpath, this.m_XMLNamespaceManager);
            StringBuilder builder = new StringBuilder();
            foreach (XmlNode node in list)
            {
                builder.Append(node.InnerText + Environment.NewLine);
            }
            return builder.ToString();
        }

        public string getNodesInnerXML(string xpath)
        {
            XmlNodeList list = this.m_XMLDocumet.DocumentElement.SelectNodes(xpath, this.m_XMLNamespaceManager);
            StringBuilder builder = new StringBuilder();
            foreach (XmlNode node in list)
            {
                builder.Append(node.InnerXml + Environment.NewLine);
            }
            return builder.ToString();
        }

        public string getSingleNodeInnerText(string xpath)
        {
            XmlNode node = this.m_XMLDocumet.DocumentElement.SelectSingleNode(xpath, this.m_XMLNamespaceManager);
            if (node != null)
            {
                return node.InnerText;
            }
            return string.Empty;
        }

        public string getSingleNodeInnerXML(string xpath)
        {
            XmlNode node = this.m_XMLDocumet.DocumentElement.SelectSingleNode(xpath, this.m_XMLNamespaceManager);
            if (node != null)
            {
                return node.InnerXml;
            }
            return string.Empty;
        }

        public string XMLData
        {
            get
            {
                return this.m_XMLData;
            }
        }
    }
}

