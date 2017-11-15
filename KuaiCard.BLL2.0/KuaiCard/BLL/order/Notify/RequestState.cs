namespace KuaiCard.BLL.Order.Notify
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class RequestState
    {
        private const int BUFFER_SIZE = 0x400;
        public byte[] BufferRead = new byte[0x400];
        public object order;
        public int orderclass;
        public HttpWebRequest request = null;
        public StringBuilder requestData = new StringBuilder("");
        public HttpWebResponse response;
        public Stream streamResponse = null;
        public string url;
    }
}

