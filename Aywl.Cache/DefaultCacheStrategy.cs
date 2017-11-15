namespace OriginalStudio.Cache
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Caching;
    using System.Web;


    /// <summary>
    /// 系统默认缓存服务
    /// </summary>
    public class DefaultCacheStrategy : ICacheStrategy
    {
        protected int _timeOut = 3600;
        private static readonly DefaultCacheStrategy instance = new DefaultCacheStrategy();
        private static object syncObj = new object();
        protected static volatile Cache webCache = HttpRuntime.Cache;

        /// <summary>
        /// 缓存超时时间。单位秒
        /// </summary>
        public virtual int TimeOut
        {
            get
            {
                return ((this._timeOut > 0) ? this._timeOut : 3600);
            }
            set
            {
                this._timeOut = (value > 0) ? value : 3600;
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public static Cache GetWebCacheObj
        {
            get
            {
                return webCache;
            }
        }

        #region 添加缓存对象

        public virtual void AddObject(string objId, object o)
        {
            if (((objId != null) && (objId.Length != 0)) && (o != null))
            {
                CacheItemRemovedCallback onRemoveCallback = new CacheItemRemovedCallback(this.onRemove);
                if ((this.TimeOut == 7200) || (this.TimeOut == 0))
                {
                    webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.High, onRemoveCallback);
                }
                else
                {
                    //Cache.NoSlidingExpiration 绝对过期时间，当超过设定时间，立即移除。
                    webCache.Insert(objId, o, null, DateTime.Now.AddSeconds((double) this.TimeOut), Cache.NoSlidingExpiration, CacheItemPriority.High, onRemoveCallback);
                }
            }
        }

        public virtual void AddObject(int hashCode, string objId, object o)
        {
            this.AddObject(objId, o);
        }

        public virtual void AddObject(string objId, object o, bool saved)
        {
            this.AddObject(objId, o);
        }

        public virtual void AddObject(string objId, object o, int expires)
        {
            CacheItemRemovedCallback onRemoveCallback = new CacheItemRemovedCallback(this.onRemove);
            webCache.Insert(objId, o, null, DateTime.Now.AddSeconds((double) expires), Cache.NoSlidingExpiration, CacheItemPriority.High, onRemoveCallback);
        }

        public virtual void AddObject(int hashCode, string objId, object o, bool saved)
        {
            this.AddObject(objId, o);
        }

        public virtual void AddObject(string objId, object o, int expires, bool saved)
        {
            this.AddObject(objId, o, expires);
        }

        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
        }

        #endregion

        #region 移除缓存对象

        public virtual void RemoveObject(string objId)
        {
            if ((objId != null) && (objId.Length != 0))
            {
                webCache.Remove(objId);
            }
        }

        public virtual void RemoveObject(int hashCode, string objId)
        {
            this.RemoveObject(objId);
        }

        public virtual void RemoveObject(string objId, bool saved)
        {
            this.RemoveObject(objId);
        }

        #endregion

        #region 恢复缓存对象

        public virtual object RetrieveObject(string objId)
        {
            if ((objId == null) || (objId.Length == 0))
            {
                return null;
            }
            return webCache.Get(objId);
        }

        public virtual object RetrieveObject(int hashCode, string objId)
        {
            if ((objId == null) || (objId.Length == 0))
            {
                return null;
            }
            return webCache.Get(objId);
        }

        public virtual object RetrieveObject(string objId, Type type, bool saved)
        {
            return this.RetrieveObject(objId);
        }

        public virtual object RetrieveObject(int hashCode, string objId, Type type, bool saved)
        {
            return this.RetrieveObject(objId);
        }

        #endregion
    }
}

