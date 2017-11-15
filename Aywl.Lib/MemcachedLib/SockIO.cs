namespace MemcachedLib
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Resources;
    using System.Text;
    using System.Threading;

    public class SockIO
    {
        private DateTime _created;
        private string _host;
        private int _id;
        private Stream _networkStream;
        private SockIOPool _pool;
        private static ResourceManager _resourceManager = new ResourceManager("Discuz.Cache.MemCached.StringMessages", typeof(SockIO).Assembly);
        private Socket _socket;
        private static int IdGenerator;

        private SockIO()
        {
            this._id = Interlocked.Increment(ref IdGenerator);
            this._created = DateTime.Now;
        }

        public SockIO(SockIOPool pool, string host, int timeout, int connectTimeout, bool noDelay) : this()
        {
            if ((host == null) || (host.Length == 0))
            {
                throw new ArgumentNullException(GetLocalizedString("host"), GetLocalizedString("null host"));
            }
            this._pool = pool;
            string[] strArray = host.Split(new char[] { ':' });
            if (connectTimeout > 0)
            {
                this._socket = GetSocket(strArray[0], int.Parse(strArray[1], new NumberFormatInfo()), connectTimeout);
            }
            else
            {
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._socket.Connect(new IPEndPoint(IPAddress.Parse(strArray[0]), int.Parse(strArray[1], new NumberFormatInfo())));
            }
            this._networkStream = new BufferedStream(new NetworkStreamIgnoreSeek(this._socket));
            this._host = host;
        }

        public SockIO(SockIOPool pool, string host, int port, int timeout, int connectTimeout, bool noDelay) : this()
        {
            if ((host == null) || (host.Length == 0))
            {
                throw new ArgumentNullException(GetLocalizedString("host"), GetLocalizedString("null host"));
            }
            this._pool = pool;
            if (connectTimeout > 0)
            {
                this._socket = GetSocket(host, port, connectTimeout);
            }
            else
            {
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._socket.Connect(new IPEndPoint(IPAddress.Parse(host), port));
            }
            this._networkStream = new BufferedStream(new NetworkStreamIgnoreSeek(this._socket));
            this._host = host + ":" + port;
        }

        public void ClearEndOfLine()
        {
            if (!((this._socket != null) && this._socket.Connected))
            {
                throw new IOException(GetLocalizedString("read closed socket"));
            }
            byte[] buffer = new byte[1];
            bool flag = false;
            while (this._networkStream.Read(buffer, 0, 1) != -1)
            {
                if (buffer[0] == 13)
                {
                    flag = true;
                }
                else if (flag)
                {
                    if (buffer[0] == 10)
                    {
                        break;
                    }
                    flag = false;
                }
            }
        }

        public void Close()
        {
            this._pool.CheckIn(this);
        }

        public void Flush()
        {
            if (!((this._socket != null) && this._socket.Connected))
            {
                throw new IOException(GetLocalizedString("write closed socket"));
            }
            this._networkStream.Flush();
        }

        private static string GetLocalizedString(string key)
        {
            return _resourceManager.GetString(key);
        }

        protected static Socket GetSocket(string host, int port, int timeout)
        {
            ConnectThread thread = new ConnectThread(host, port);
            thread.Start();
            int num = 0;
            int millisecondsTimeout = 0x19;
            while (num < timeout)
            {
                if (thread.IsConnected)
                {
                    return thread.Socket;
                }
                if (thread.IsError)
                {
                    throw new IOException();
                }
                try
                {
                    Thread.Sleep(millisecondsTimeout);
                }
                catch (ThreadInterruptedException)
                {
                }
                num += millisecondsTimeout;
            }
            throw new IOException(GetLocalizedString("connect timeout").Replace("$$timeout$$", timeout.ToString(new NumberFormatInfo())));
        }

        public void Read(byte[] bytes)
        {
            if (!((this._socket != null) && this._socket.Connected))
            {
                throw new IOException(GetLocalizedString("read closed socket"));
            }
            if (bytes != null)
            {
                int num2;
                for (int i = 0; i < bytes.Length; i += num2)
                {
                    num2 = this._networkStream.Read(bytes, i, bytes.Length - i);
                }
            }
        }

        public string ReadLine()
        {
            if (!((this._socket != null) && this._socket.Connected))
            {
                throw new IOException(GetLocalizedString("read closed socket"));
            }
            byte[] buffer = new byte[1];
            MemoryStream stream = new MemoryStream();
            bool flag = false;
            while (this._networkStream.Read(buffer, 0, 1) != -1)
            {
                if (buffer[0] == 13)
                {
                    flag = true;
                }
                else if (flag)
                {
                    if (buffer[0] == 10)
                    {
                        break;
                    }
                    flag = false;
                }
                stream.Write(buffer, 0, 1);
            }
            if ((stream == null) || (stream.Length <= 0L))
            {
                throw new IOException(GetLocalizedString("closing dead stream"));
            }
            char[] trimChars = new char[3];
            trimChars[1] = '\r';
            trimChars[2] = '\n';
            return Encoding.UTF8.GetString(stream.GetBuffer()).TrimEnd(trimChars);
        }

        public override string ToString()
        {
            if (this._socket == null)
            {
                return "";
            }
            return this._id.ToString(new NumberFormatInfo());
        }

        public void TrueClose()
        {
            bool flag = false;
            StringBuilder builder = new StringBuilder();
            if ((this._socket == null) || (this._networkStream == null))
            {
                flag = true;
                builder.Append(GetLocalizedString("socket already closed"));
            }
            if (this._socket != null)
            {
                try
                {
                    this._socket.Close();
                }
                catch (IOException exception)
                {
                    builder.Append(GetLocalizedString("error closing socket").Replace("$$ToString$$", this.ToString()).Replace("$$Host$$", this.Host) + Environment.NewLine);
                    builder.Append(exception.ToString());
                    flag = true;
                }
                catch (SocketException exception2)
                {
                    builder.Append(GetLocalizedString("error closing socket").Replace("$$ToString$$", this.ToString()).Replace("$$Host$$", this.Host) + Environment.NewLine);
                    builder.Append(exception2.ToString());
                    flag = true;
                }
            }
            if (this._socket != null)
            {
                this._pool.CheckIn(this, false);
            }
            this._networkStream = null;
            this._socket = null;
            if (flag)
            {
                throw new IOException(builder.ToString());
            }
        }

        public void Write(byte[] bytes)
        {
            this.Write(bytes, 0, bytes.Length);
        }

        public void Write(byte[] bytes, int offset, int count)
        {
            if (!((this._socket != null) && this._socket.Connected))
            {
                throw new IOException(GetLocalizedString("write closed socket"));
            }
            if (bytes != null)
            {
                this._networkStream.Write(bytes, offset, count);
            }
        }

        public string Host
        {
            get
            {
                return this._host;
            }
        }

        public bool IsConnected
        {
            get
            {
                return ((this._socket != null) && this._socket.Connected);
            }
        }

        private class ConnectThread
        {
            private bool _error;
            private string _host;
            private int _port;
            private System.Net.Sockets.Socket _socket;
            private Thread _thread;

            public ConnectThread(string host, int port)
            {
                this._host = host;
                this._port = port;
                this._thread = new Thread(new ThreadStart(this.Connect));
                this._thread.IsBackground = true;
            }

            private void Connect()
            {
                try
                {
                    this._socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this._socket.Connect(new IPEndPoint(IPAddress.Parse(this._host), this._port));
                }
                catch (IOException)
                {
                    this._error = true;
                }
                catch
                {
                    this._error = true;
                }
            }

            public void Start()
            {
                this._thread.Start();
            }

            public bool IsConnected
            {
                get
                {
                    return ((this._socket != null) && this._socket.Connected);
                }
            }

            public bool IsError
            {
                get
                {
                    return this._error;
                }
            }

            public System.Net.Sockets.Socket Socket
            {
                get
                {
                    return this._socket;
                }
            }
        }
    }
}

