namespace MemcachedLib
{
    using OriginalStudio.Lib.SysConfig;
    using OriginalStudio.Lib.Text;
    using OriginalStudio.Lib.Utils;
    using System;
    using System.Collections;

    public sealed class MemCachedManager
    {
        private static MemcachedClient mc = null;
        private static SockIOPool pool = null;
        private static string[] serverList = null;

        static MemCachedManager()
        {
            CreateManager();
        }

        public static void CreateManager()
        {
            serverList = MemCachedConfig.ServerList.Split(new char[] { ',' });
            pool = SockIOPool.GetInstance(MemCachedConfig.PoolName);
            pool.SetServers(serverList);
            pool.SetWeights(TypeParse.StringToIntArray(MemCachedConfig.ServerList, 1));
            pool.InitConnections = MemCachedConfig.IntConnections;
            pool.MinConnections = MemCachedConfig.MinConnections;
            pool.MaxConnections = MemCachedConfig.MaxConnections;
            pool.SocketConnectTimeout = MemCachedConfig.SocketConnectTimeout;
            pool.SocketTimeout = MemCachedConfig.SocketTimeout;
            pool.MaintenanceSleep = MemCachedConfig.MaintenanceSleep;
            pool.Failover = MemCachedConfig.FailOver;
            pool.Nagle = MemCachedConfig.Nagle;
            pool.HashingAlgorithm = HashingAlgorithm.NewCompatibleHash;
            pool.Initialize();
            mc = new MemcachedClient();
            mc.PoolName = MemCachedConfig.PoolName;
            mc.EnableCompression = false;
        }

        public static void Dispose()
        {
            if (MemCachedConfig.ApplyMemCached && (pool != null))
            {
                pool.Shutdown();
            }
        }

        public static ArrayList GetCachedKeyList(ArrayList serverArrayList, string poolName)
        {
            Hashtable hashtable = GetMemcachedClient(poolName, serverArrayList).Stats(serverArrayList, "stats items", poolName);
            ArrayList list = new ArrayList();
            foreach (string str in hashtable.Keys)
            {
                foreach (string str2 in ((Hashtable) hashtable[str]).Keys)
                {
                    Hashtable hashtable2 = CacheClient.Stats(serverArrayList, "stats cachedump " + str2.Split(new char[] { ':' })[1] + " 0", poolName);
                    foreach (string str3 in hashtable2.Keys)
                    {
                        foreach (string str4 in ((Hashtable) hashtable2[str3]).Keys)
                        {
                            if (!list.Contains(str4))
                            {
                                list.Add(str4);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public static string[] GetConnectedSocketHost()
        {
            SockIO connection = null;
            string str = null;
            foreach (string str2 in serverList)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    try
                    {
                        connection = SockIOPool.GetInstance(MemCachedConfig.PoolName).GetConnection(str2);
                        if (connection != null)
                        {
                            str = Strings.MergeString(str2, str);
                        }
                    }
                    finally
                    {
                        if (connection != null)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            return str.Split(new char[] { ',' });
        }

        public static MemcachedClient GetMemcachedClient(string poolName, ArrayList serverArrayList)
        {
            SockIOPool instance = SockIOPool.GetInstance(poolName);
            instance.SetServers(serverArrayList);
            instance.InitConnections = MemCachedConfig.IntConnections;
            instance.MinConnections = MemCachedConfig.MinConnections;
            instance.MaxConnections = MemCachedConfig.MaxConnections;
            instance.SocketConnectTimeout = MemCachedConfig.SocketConnectTimeout;
            instance.SocketTimeout = MemCachedConfig.SocketTimeout;
            instance.MaintenanceSleep = MemCachedConfig.MaintenanceSleep;
            instance.Failover = MemCachedConfig.FailOver;
            instance.Nagle = MemCachedConfig.Nagle;
            instance.HashingAlgorithm = HashingAlgorithm.NewCompatibleHash;
            instance.Initialize();
            return new MemcachedClient { PoolName = poolName, EnableCompression = false };
        }

        public static string GetSocketHost(string key, object hashCode)
        {
            string host = "";
            SockIO sock = null;
            try
            {
                sock = SockIOPool.GetInstance(MemCachedConfig.PoolName).GetSock(key, hashCode);
                if (sock != null)
                {
                    host = sock.Host;
                }
            }
            finally
            {
                if (sock != null)
                {
                    sock.Close();
                }
            }
            return host;
        }

        public static ArrayList GetStats()
        {
            ArrayList serverArrayList = new ArrayList();
            foreach (string str in serverList)
            {
                serverArrayList.Add(str);
            }
            return GetStats(serverArrayList, Stats.Default, null);
        }

        public static ArrayList GetStats(ArrayList serverArrayList, Stats statsCommand, string param)
        {
            ArrayList list = new ArrayList();
            param = string.IsNullOrEmpty(param) ? "" : param.Trim().ToLower();
            string command = "stats";
            switch (statsCommand)
            {
                case Stats.Reset:
                    command = "stats reset";
                    break;

                case Stats.Malloc:
                    command = "stats malloc";
                    break;

                case Stats.Maps:
                    command = "stats maps";
                    break;

                case Stats.Sizes:
                    command = "stats sizes";
                    break;

                case Stats.Slabs:
                    command = "stats slabs";
                    break;

                case Stats.Items:
                    command = "stats items";
                    break;

                case Stats.CachedDump:
                {
                    string[] strArray = param.Split(new char[] { ' ' });
                    if ((strArray.Length >= 2) && Utils.IsNumericArray(strArray))
                    {
                        command = "stats cachedump " + param;
                    }
                    break;
                }
                case Stats.Detail:
                    if ((string.Equals(param, "on") || string.Equals(param, "off")) || string.Equals(param, "dump"))
                    {
                        command = "stats detail " + param.Trim();
                    }
                    break;

                default:
                    command = "stats";
                    break;
            }
            Hashtable hashtable = CacheClient.Stats(serverArrayList, command, null);
            foreach (string str2 in hashtable.Keys)
            {
                list.Add("================================================================================================");
                list.Add(str2);
                Hashtable hashtable2 = (Hashtable) hashtable[str2];
                foreach (string str3 in hashtable2.Keys)
                {
                    if (statsCommand == Stats.CachedDump)
                    {
                        list.Add(str3);
                    }
                    else
                    {
                        list.Add(str3 + ":" + hashtable2[str3]);
                    }
                }
            }
            return list;
        }

        public static MemcachedClient CacheClient
        {
            get
            {
                if (mc == null)
                {
                    CreateManager();
                }
                return mc;
            }
        }

        public static string[] ServerList
        {
            get
            {
                return serverList;
            }
            set
            {
                if (value != null)
                {
                    serverList = value;
                }
            }
        }

        public enum Stats
        {
            Default,
            Reset,
            Malloc,
            Maps,
            Sizes,
            Slabs,
            Items,
            CachedDump,
            Detail
        }
    }
}

