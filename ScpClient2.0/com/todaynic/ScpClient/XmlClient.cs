namespace com.todaynic.ScpClient
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml;

    public class XmlClient
    {
        private MemoryStream m_MemoryStream;
        private string m_ReceiveXML;
        private string m_SendXML;
        private string m_ServerName;
        private int m_ServerPort;
        protected XmlWriter m_XMLWriter;

        public XmlClient(string ServerName, int ServerPort)
        {
            this.m_ServerName = ServerName;
            this.m_ServerPort = ServerPort;
            this.Initialize(System.Text.Encoding.Default);
        }

        public void Initialize()
        {
            XmlWriterSettings settings2 = new XmlWriterSettings();
            settings2.Encoding = System.Text.Encoding.GetEncoding("gbk");
            settings2.Indent = true;
            settings2.IndentChars = "  ";
            settings2.OmitXmlDeclaration = false;
            settings2.NewLineHandling = NewLineHandling.None;
            settings2.CheckCharacters = true;
            XmlWriterSettings settings = settings2;
            this.m_MemoryStream = new MemoryStream();
            this.m_XMLWriter = XmlWriter.Create(this.m_MemoryStream, settings);
        }

        public void Initialize(System.Text.Encoding Encoding)
        {
            XmlWriterSettings settings2 = new XmlWriterSettings();
            settings2.Encoding = Encoding;
            settings2.Indent = true;
            settings2.IndentChars = "  ";
            settings2.OmitXmlDeclaration = false;
            settings2.NewLineHandling = NewLineHandling.None;
            settings2.CheckCharacters = true;
            XmlWriterSettings settings = settings2;
            this.m_MemoryStream = new MemoryStream();
            this.m_XMLWriter = XmlWriter.Create(this.m_MemoryStream, settings);
        }

        protected SCPReply send()
        {
            SCPReply reply;
            IPEndPoint remoteEP = new IPEndPoint(Dns.GetHostAddresses(this.m_ServerName)[0], this.m_ServerPort);
            Socket socket = new Socket(remoteEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                this.m_XMLWriter.Flush();
                this.m_XMLWriter.Close();
                this.m_MemoryStream.Flush();
                byte[] bytes = this.m_MemoryStream.ToArray();
                this.m_MemoryStream.Close();
                this.m_SendXML = System.Text.Encoding.Default.GetString(bytes);
                socket.Connect(remoteEP);
                socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
                byte[] buffer = new byte[0x400];
                byte[] buffer3 = new byte[0];
                new StringBuilder();
                int num = 0;
                do
                {
                    num = socket.Receive(buffer);
                    byte[] array = new byte[buffer3.Length];
                    buffer3.CopyTo(array, 0);
                    buffer3 = new byte[buffer3.Length + num];
                    array.CopyTo(buffer3, 0);
                    for (int i = 0; i < num; i++)
                    {
                        buffer3[array.Length + i] = buffer[i];
                    }
                }
                while (!System.Text.Encoding.Default.GetString(buffer3, 0, buffer3.Length).ToString().Trim().Contains("</scp>"));
                string str = System.Text.Encoding.Default.GetString(buffer3, 0, buffer3.Length).ToString().Trim();
                int index = str.IndexOf("<?xml");
                if (index > 0)
                {
                    str = str.Substring(index);
                }
                index = str.IndexOf("</scp>");
                if (index > 0)
                {
                    str = str.Substring(0, index + "</scp>".Length);
                }
                this.m_ReceiveXML = str;
                Console.WriteLine(this.m_ReceiveXML);
                reply = new SCPReply(this.m_ReceiveXML);
            }
            finally
            {
                socket.Close();
            }
            return reply;
        }

        private void SetEncoding(string encoding)
        {
            Utility.Encoding = this.m_XMLWriter.Settings.Encoding;
            this.Initialize(Utility.Encoding);
        }

        protected void writeSecurityMessage(string ID, string Password, UserType userType)
        {
            this.m_XMLWriter.WriteStartElement("security");
            this.m_XMLWriter.WriteElementString(userType.ToString(), ID);
            string str = DateTime.Now.ToFileTime().ToString();
            this.m_XMLWriter.WriteElementString("cltrid", str);
            string str2 = Utility.getMd5Hash(str + "-" + Password);
            this.m_XMLWriter.WriteElementString("login", str2);
            this.m_XMLWriter.WriteEndElement();
        }

        private System.Text.Encoding Encoding
        {
            set
            {
                this.Initialize(value);
                Utility.Encoding = value;
            }
        }

        public string ReceiveXML
        {
            get
            {
                return this.m_ReceiveXML;
            }
        }

        public string SendXML
        {
            get
            {
                return this.m_SendXML;
            }
        }

        public string ServerName
        {
            get
            {
                return this.m_ServerName;
            }
            set
            {
                this.m_ServerName = value;
            }
        }

        public int ServerPort
        {
            get
            {
                return this.m_ServerPort;
            }
            set
            {
                this.m_ServerPort = value;
            }
        }
    }
}

