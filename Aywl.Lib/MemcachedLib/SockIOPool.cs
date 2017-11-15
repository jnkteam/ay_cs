namespace MemcachedLib
{
    using System;
    using System.Collections;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SockIOPool
    {
        private Hashtable _availPool;
        private ArrayList _buckets;
        private Hashtable _busyPool;
        private Hashtable _createShift;
        private bool _failover = true;
        private MemcachedLib.HashingAlgorithm _hashingAlgorithm = MemcachedLib.HashingAlgorithm.Native;
        private Hashtable _hostDead;
        private Hashtable _hostDeadDuration;
        private int _initConns = 3;
        private bool _initialized;
        private MaintenanceThread _maintenanceThread;
        private long _maintThreadSleep = 0x1388L;
        private long _maxBusyTime = 0x493e0L;
        private int _maxConns = 10;
        private int _maxCreate = 1;
        private long _maxIdle = 0x2bf20L;
        private int _minConns = 3;
        private bool _nagle = true;
        private int _poolMultiplier = 4;
        private static ResourceManager _resourceManager = new ResourceManager("Discuz.Cache.MemCached.StringMessages", typeof(SockIOPool).Assembly);
        private ArrayList _servers;
        private int _socketConnectTimeout = 50;
        private int _socketTimeout = 0x2710;
        private ArrayList _weights;
        private static Hashtable Pools = new Hashtable();

        protected SockIOPool()
        {
        }

        protected static void AddSocketToPool(Hashtable pool, string host, SockIO socket)
        {
            if (pool != null)
            {
                Hashtable hashtable;
                if (((host != null) && (host.Length != 0)) && pool.ContainsKey(host))
                {
                    hashtable = (Hashtable) pool[host];
                    if (hashtable != null)
                    {
                        hashtable[socket] = DateTime.Now;
                        return;
                    }
                }
                hashtable = new Hashtable();
                hashtable[socket] = DateTime.Now;
                pool[host] = hashtable;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(SockIO socket)
        {
            this.CheckIn(socket, true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(SockIO socket, bool addToAvail)
        {
            if (socket != null)
            {
                string host = socket.Host;
                RemoveSocketFromPool(this._busyPool, host, socket);
                if (addToAvail && socket.IsConnected)
                {
                    AddSocketToPool(this._availPool, host, socket);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void ClearHostFromPool(Hashtable pool, string host)
        {
            if (((pool != null) && ((host == null) || (host.Length != 0))) && pool.ContainsKey(host))
            {
                Hashtable hashtable = (Hashtable) pool[host];
                if ((hashtable != null) && (hashtable.Count > 0))
                {
                    foreach (SockIO kio in new IteratorIsolateCollection(hashtable.Keys))
                    {
                        try
                        {
                            kio.TrueClose();
                        }
                        catch
                        {
                        }
                        hashtable.Remove(kio);
                    }
                }
            }
        }

        protected static void ClosePool(Hashtable pool)
        {
            if (pool != null)
            {
                foreach (string str in pool.Keys)
                {
                    Hashtable hashtable = (Hashtable) pool[str];
                    foreach (SockIO kio in new IteratorIsolateCollection(hashtable.Keys))
                    {
                        try
                        {
                            kio.TrueClose();
                        }
                        catch
                        {
                        }
                        hashtable.Remove(kio);
                    }
                }
            }
        }

        protected SockIO CreateSocket(string host)
        {
            SockIO kio = null;
            long num;
            if ((this._failover && this._hostDead.ContainsKey(host)) && this._hostDeadDuration.ContainsKey(host))
            {
                DateTime time = (DateTime) this._hostDead[host];
                num = (long) this._hostDeadDuration[host];
                if (time.AddMilliseconds((double) num) > DateTime.Now)
                {
                    return null;
                }
            }
            try
            {
                kio = new SockIO(this, host, this._socketTimeout, this._socketConnectTimeout, this._nagle);
                if (!kio.IsConnected)
                {
                    try
                    {
                        kio.TrueClose();
                    }
                    catch
                    {
                        kio = null;
                    }
                }
            }
            catch
            {
                kio = null;
            }
            if (kio == null)
            {
                DateTime now = DateTime.Now;
                this._hostDead[host] = now;
                num = this._hostDeadDuration.ContainsKey(host) ? (((long) this._hostDeadDuration[host]) * 2L) : 100L;
                this._hostDeadDuration[host] = num;
                ClearHostFromPool(this._availPool, host);
                if (this._buckets.BinarySearch(host) >= 0)
                {
                    this._buckets.Remove(host);
                }
                return kio;
            }
            this._hostDead.Remove(host);
            this._hostDeadDuration.Remove(host);
            if (this._buckets.BinarySearch(host) < 0)
            {
                this._buckets.Add(host);
            }
            return kio;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public SockIO GetConnection(string host)
        {
            if (this._initialized)
            {
                SockIO current;
                if (host == null)
                {
                    return null;
                }
                if ((this._availPool != null) && (this._availPool.Count != 0))
                {
                    Hashtable hashtable = (Hashtable) this._availPool[host];
                    if ((hashtable != null) && (hashtable.Count != 0))
                    {
                        IEnumerator enumerator = new IteratorIsolateCollection(hashtable.Keys).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            current = (SockIO)enumerator.Current;
                            if (current.IsConnected)
                            {
                                hashtable.Remove(current);
                                AddSocketToPool(this._busyPool, host, current);
                                return current;
                            }
                            hashtable.Remove(current);
                        }
                    }                    
                }
                object obj2 = this._createShift[host];
                int num = (obj2 != null) ? ((int) obj2) : 0;
                int num2 = ((int) 1) << num;
                if (num2 >= this._maxCreate)
                {
                    num2 = this._maxCreate;
                }
                else
                {
                    num++;
                }
                this._createShift[host] = num;
                for (int i = num2; i > 0; i--)
                {
                    current = this.CreateSocket(host);
                    if (current == null)
                    {
                        break;
                    }
                    if (i == 1)
                    {
                        AddSocketToPool(this._busyPool, host, current);
                        return current;
                    }
                    AddSocketToPool(this._availPool, host, current);
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static SockIOPool GetInstance()
        {
            return GetInstance(GetLocalizedString("default instance"));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static SockIOPool GetInstance(string poolName)
        {
            if (Pools.ContainsKey(poolName))
            {
                return (SockIOPool) Pools[poolName];
            }
            SockIOPool pool = new SockIOPool();
            Pools[poolName] = pool;
            return pool;
        }

        private static string GetLocalizedString(string key)
        {
            return _resourceManager.GetString(key);
        }

        public SockIO GetSock(string key)
        {
            return this.GetSock(key, null);
        }

        public SockIO GetSock(string key, object hashCode)
        {
            int num2;
            string str = "<null>";
            if (hashCode != null)
            {
                str = hashCode.ToString();
            }
            if ((key == null) || (key.Length == 0))
            {
                return null;
            }
            if (!this._initialized)
            {
                return null;
            }
            if (this._buckets.Count == 0)
            {
                return null;
            }
            if (this._buckets.Count == 1)
            {
                return this.GetConnection((string) this._buckets[0]);
            }
            int num = 0;
            if (hashCode != null)
            {
                num2 = (int) hashCode;
            }
            else
            {
                switch (this._hashingAlgorithm)
                {
                    case MemcachedLib.HashingAlgorithm.Native:
                        num2 = key.GetHashCode();
                        goto Label_01F0;

                    case MemcachedLib.HashingAlgorithm.OldCompatibleHash:
                        num2 = HashingAlgorithmHelper.OriginalHashingAlgorithm(key);
                        goto Label_01F0;

                    case MemcachedLib.HashingAlgorithm.NewCompatibleHash:
                        num2 = HashingAlgorithmHelper.NewHashingAlgorithm(key);
                        goto Label_01F0;
                }
                num2 = key.GetHashCode();
                this._hashingAlgorithm = MemcachedLib.HashingAlgorithm.Native;
            }
        Label_01F0:
            while (num++ <= this._buckets.Count)
            {
                int num3 = num2 % this._buckets.Count;
                if (num3 < 0)
                {
                    num3 += this._buckets.Count;
                }
                SockIO connection = this.GetConnection((string) this._buckets[num3]);
                if (connection != null)
                {
                    return connection;
                }
                if (!this._failover)
                {
                    return null;
                }
                switch (this._hashingAlgorithm)
                {
                    case MemcachedLib.HashingAlgorithm.Native:
                    {
                        num2 += (num + key).GetHashCode();
                        continue;
                    }
                    case MemcachedLib.HashingAlgorithm.OldCompatibleHash:
                    {
                        num2 += HashingAlgorithmHelper.OriginalHashingAlgorithm(num + key);
                        continue;
                    }
                    case MemcachedLib.HashingAlgorithm.NewCompatibleHash:
                    {
                        num2 += HashingAlgorithmHelper.NewHashingAlgorithm(num + key);
                        continue;
                    }
                }
                num2 += (num + key).GetHashCode();
                this._hashingAlgorithm = MemcachedLib.HashingAlgorithm.Native;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Initialize()
        {
            if (((!this._initialized || (this._buckets == null)) || (this._availPool == null)) || (this._busyPool == null))
            {
                this._buckets = new ArrayList();
                this._availPool = new Hashtable(this._servers.Count * this._initConns);
                this._busyPool = new Hashtable(this._servers.Count * this._initConns);
                this._hostDeadDuration = new Hashtable();
                this._hostDead = new Hashtable();
                this._createShift = new Hashtable();
                this._maxCreate = (this._poolMultiplier > this._minConns) ? this._minConns : (this._minConns / this._poolMultiplier);
                if ((this._servers == null) || (this._servers.Count <= 0))
                {
                    throw new ArgumentException(GetLocalizedString("initialize with no servers"));
                }
                for (int i = 0; i < this._servers.Count; i++)
                {
                    if ((this._weights != null) && (this._weights.Count > i))
                    {
                        for (int k = 0; k < ((int) this._weights[i]); k++)
                        {
                            this._buckets.Add(this._servers[i]);
                        }
                    }
                    else
                    {
                        this._buckets.Add(this._servers[i]);
                    }
                    for (int j = 0; j < this._initConns; j++)
                    {
                        SockIO socket = this.CreateSocket((string) this._servers[i]);
                        if (socket == null)
                        {
                            break;
                        }
                        AddSocketToPool(this._availPool, (string) this._servers[i], socket);
                    }
                }
                this._initialized = true;
                if (this._maintThreadSleep > 0L)
                {
                    this.StartMaintenanceThread();
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void RemoveSocketFromPool(Hashtable pool, string host, SockIO socket)
        {
            if ((((host == null) || (host.Length != 0)) && (pool != null)) && pool.ContainsKey(host))
            {
                Hashtable hashtable = (Hashtable) pool[host];
                if (hashtable != null)
                {
                    hashtable.Remove(socket);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void SelfMaintain()
        {
            Hashtable hashtable;
            foreach (string str in new IteratorIsolateCollection(this._availPool.Keys))
            {
                hashtable = (Hashtable) this._availPool[str];
                if (hashtable.Count < this._minConns)
                {
                    int num = this._minConns - hashtable.Count;
                    for (int i = 0; i < num; i++)
                    {
                        SockIO socket = this.CreateSocket(str);
                        if (socket == null)
                        {
                            break;
                        }
                        AddSocketToPool(this._availPool, str, socket);
                    }
                }
                else if (hashtable.Count > this._maxConns)
                {
                    int num3 = hashtable.Count - this._maxConns;
                    int num4 = (num3 <= this._poolMultiplier) ? num3 : (num3 / this._poolMultiplier);
                    foreach (SockIO kio in new IteratorIsolateCollection(hashtable.Keys))
                    {
                        if (num4 <= 0)
                        {
                            break;
                        }
                        DateTime time = (DateTime) hashtable[kio];
                        if (time.AddMilliseconds((double) this._maxIdle) < DateTime.Now)
                        {
                            try
                            {
                                kio.TrueClose();
                            }
                            catch
                            {
                            }
                            hashtable.Remove(kio);
                            num4--;
                        }
                    }
                }
                this._createShift[str] = 0;
            }
            foreach (string str in this._busyPool.Keys)
            {
                hashtable = (Hashtable) this._busyPool[str];
                foreach (SockIO kio in new IteratorIsolateCollection(hashtable.Keys))
                {
                    DateTime time2 = (DateTime) hashtable[kio];
                    if (time2.AddMilliseconds((double) this._maxBusyTime) < DateTime.Now)
                    {
                        try
                        {
                            kio.TrueClose();
                        }
                        catch
                        {
                        }
                        hashtable.Remove(kio);
                    }
                }
            }
        }

        public void SetServers(string[] servers)
        {
            this.SetServers(new ArrayList(servers));
        }

        public void SetServers(ArrayList servers)
        {
            this._servers = servers;
        }

        public void SetWeights(int[] weights)
        {
            this.SetWeights(new ArrayList(weights));
        }

        public void SetWeights(ArrayList weights)
        {
            this._weights = weights;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Shutdown()
        {
            if ((this._maintenanceThread != null) && this._maintenanceThread.IsRunning)
            {
                this.StopMaintenanceThread();
            }
            ClosePool(this._availPool);
            ClosePool(this._busyPool);
            this._availPool = null;
            this._busyPool = null;
            this._buckets = null;
            this._hostDeadDuration = null;
            this._hostDead = null;
            this._initialized = false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void StartMaintenanceThread()
        {
            if (this._maintenanceThread != null)
            {
                if (!this._maintenanceThread.IsRunning)
                {
                    this._maintenanceThread.Start();
                }
            }
            else
            {
                this._maintenanceThread = new MaintenanceThread(this);
                this._maintenanceThread.Interval = this._maintThreadSleep;
                this._maintenanceThread.Start();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void StopMaintenanceThread()
        {
            if ((this._maintenanceThread != null) && this._maintenanceThread.IsRunning)
            {
                this._maintenanceThread.StopThread();
            }
        }

        public bool Failover
        {
            get
            {
                return this._failover;
            }
            set
            {
                this._failover = value;
            }
        }

        public MemcachedLib.HashingAlgorithm HashingAlgorithm
        {
            get
            {
                return this._hashingAlgorithm;
            }
            set
            {
                this._hashingAlgorithm = value;
            }
        }

        public int InitConnections
        {
            get
            {
                return this._initConns;
            }
            set
            {
                this._initConns = value;
            }
        }

        public bool Initialized
        {
            get
            {
                return this._initialized;
            }
        }

        public long MaintenanceSleep
        {
            get
            {
                return this._maintThreadSleep;
            }
            set
            {
                this._maintThreadSleep = value;
            }
        }

        public long MaxBusy
        {
            get
            {
                return this._maxBusyTime;
            }
            set
            {
                this._maxBusyTime = value;
            }
        }

        public int MaxConnections
        {
            get
            {
                return this._maxConns;
            }
            set
            {
                this._maxConns = value;
            }
        }

        public long MaxIdle
        {
            get
            {
                return this._maxIdle;
            }
            set
            {
                this._maxIdle = value;
            }
        }

        public int MinConnections
        {
            get
            {
                return this._minConns;
            }
            set
            {
                this._minConns = value;
            }
        }

        public bool Nagle
        {
            get
            {
                return this._nagle;
            }
            set
            {
                this._nagle = value;
            }
        }

        public ArrayList Servers
        {
            get
            {
                return this._servers;
            }
        }

        public int SocketConnectTimeout
        {
            get
            {
                return this._socketConnectTimeout;
            }
            set
            {
                this._socketConnectTimeout = value;
            }
        }

        public int SocketTimeout
        {
            get
            {
                return this._socketTimeout;
            }
            set
            {
                this._socketTimeout = value;
            }
        }

        public ArrayList Weights
        {
            get
            {
                return this._weights;
            }
        }

        private class MaintenanceThread
        {
            private long _interval;
            private SockIOPool _pool;
            private bool _stopThread;
            private Thread _thread;

            private MaintenanceThread()
            {
                this._interval = 0xbb8L;
            }

            public MaintenanceThread(SockIOPool pool)
            {
                this._interval = 0xbb8L;
                this._thread = new Thread(new ThreadStart(this.Maintain));
                this._pool = pool;
            }

            private void Maintain()
            {
                while (!this._stopThread)
                {
                    try
                    {
                        Thread.Sleep((int) this._interval);
                        if (this._pool.Initialized)
                        {
                            this._pool.SelfMaintain();
                        }
                    }
                    catch (ThreadInterruptedException)
                    {
                    }
                    catch
                    {
                    }
                }
            }

            public void Start()
            {
                this._stopThread = false;
                this._thread.Start();
            }

            public void StopThread()
            {
                this._stopThread = true;
                this._thread.Interrupt();
            }

            public long Interval
            {
                get
                {
                    return this._interval;
                }
                set
                {
                    this._interval = value;
                }
            }

            public bool IsRunning
            {
                get
                {
                    return this._thread.IsAlive;
                }
            }
        }
    }
}

