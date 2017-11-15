namespace MemcachedLib
{
    using System;
    using System.Collections;

    public class IteratorIsolateCollection : IEnumerable
    {
        private IEnumerable _enumerable;

        public IteratorIsolateCollection(IEnumerable enumerable)
        {
            this._enumerable = enumerable;
        }

        public IEnumerator GetEnumerator()
        {
            return new IteratorIsolateEnumerator(this._enumerable.GetEnumerator());
        }

        internal class IteratorIsolateEnumerator : IEnumerator
        {
            private int currentItem;
            private ArrayList items = new ArrayList();

            internal IteratorIsolateEnumerator(IEnumerator enumerator)
            {
                while (enumerator.MoveNext())
                {
                    this.items.Add(enumerator.Current);
                }
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                this.currentItem = -1;
            }

            public bool MoveNext()
            {
                this.currentItem++;
                if (this.currentItem == this.items.Count)
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                this.currentItem = -1;
            }

            public object Current
            {
                get
                {
                    return this.items[this.currentItem];
                }
            }
        }
    }
}

