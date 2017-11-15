namespace MemcachedLib
{
    using System;
    using System.IO;
    using System.Net.Sockets;

    public class NetworkStreamIgnoreSeek : NetworkStream
    {
        public NetworkStreamIgnoreSeek(Socket socket) : base(socket)
        {
        }

        public NetworkStreamIgnoreSeek(Socket socket, bool ownsSocket) : base(socket, ownsSocket)
        {
        }

        public NetworkStreamIgnoreSeek(Socket socket, FileAccess access) : base(socket, access)
        {
        }

        public NetworkStreamIgnoreSeek(Socket socket, FileAccess access, bool ownsSocket) : base(socket, access, ownsSocket)
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0L;
        }
    }
}

