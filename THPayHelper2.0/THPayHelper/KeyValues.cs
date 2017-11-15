namespace THPayHelper
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class KeyValues
    {
        private List<KeyValue> keyValues = new List<KeyValue>();

        public void add(KeyValue kv)
        {
            if ((kv != null) && !string.IsNullOrEmpty(kv.getVal()))
            {
                this.keyValues.Add(kv);
            }
        }

        public List<KeyValue> items()
        {
            return this.keyValues;
        }

        public string sign(string key, string inputCharset)
        {
            StringBuilder sb = new StringBuilder();
            this.keyValues.Sort(new KeyValueComparer());
            foreach (KeyValue value2 in this.keyValues)
            {
                URLUtils.appendParam(sb, value2.getKey(), value2.getVal());
            }
            URLUtils.appendParam(sb, AppConstants.KEY, key);
            string str = sb.ToString();
            return MD5Encoder.encode(str.Substring(1, str.Length - 1), inputCharset);
        }

        private class KeyValueComparer : IComparer<KeyValue>
        {
            public int Compare(KeyValue l, KeyValue r)
            {
                return l.compare(r);
            }
        }
    }
}

