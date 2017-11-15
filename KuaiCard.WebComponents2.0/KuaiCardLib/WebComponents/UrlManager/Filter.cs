namespace KuaiCardLib.WebComponents.UrlManager
{
    using KuaiCardLib.ExceptionHandling;
    using KuaiCardLib.IO;
    using System;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Filter : Stream
    {
        private byte[] _buffer;
        private int _count;
        private long _position;
        private Stream _sink;
        private MemoryStream _tempMemoryStream;
        private BinaryWriter _writer;
        private string filePath;

        public Filter(Stream sink, string file)
        {
            this._sink = sink;
            this.filePath = file;
        }

        public override void Close()
        {
            try
            {
                try
                {
                    if (this._sink != null)
                    {
                        this._sink.Close();
                    }
                    if (this._tempMemoryStream != null)
                    {
                        if (System.IO.File.Exists(this.filePath))
                        {
                            KuaiCardLib.IO.File.Delete(this.filePath);
                        }
                        using (FileStream stream = new FileStream(this.filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            this._tempMemoryStream.WriteTo(stream);
                            stream.Close();
                        }
                        this._tempMemoryStream.Close();
                    }
                    if (this._writer != null)
                    {
                        this._writer.Close();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                }
            }
            finally
            {
            }
        }

        public override void Flush()
        {
            this._sink.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._sink.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin direction)
        {
            return this._sink.Seek(offset, direction);
        }

        public override void SetLength(long length)
        {
            this._sink.SetLength(length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (((((HttpContext.Current.Response.ContentType == "text/html") || (HttpContext.Current.Response.ContentType == "text/javascript")) || ((HttpContext.Current.Response.ContentType == "text/vbscript") || (HttpContext.Current.Response.ContentType == "text/ecmascript"))) || (HttpContext.Current.Response.ContentType == "text/Jscript")) || (HttpContext.Current.Response.ContentType == "text/xml"))
            {
                Encoding encoding = Encoding.GetEncoding(HttpContext.Current.Response.Charset);
                string s = encoding.GetString(buffer, offset, count);
                this._buffer = encoding.GetBytes(s);
                this._count = encoding.GetByteCount(s);
                this._sink.Write(this._buffer, 0, this._count);
                this.BWriter.Write(this._buffer, 0, this._count);
            }
            else
            {
                this._sink.Write(buffer, offset, count);
            }
        }

        private BinaryWriter BWriter
        {
            get
            {
                if (this._writer == null)
                {
                    string directoryName = Path.GetDirectoryName(this.filePath);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    this._writer = new BinaryWriter(this.TempMemoryStream);
                }
                return this._writer;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return 0L;
            }
        }

        public override long Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        private MemoryStream TempMemoryStream
        {
            get
            {
                if (this._tempMemoryStream == null)
                {
                    this._tempMemoryStream = new MemoryStream();
                }
                return this._tempMemoryStream;
            }
        }
    }
}

