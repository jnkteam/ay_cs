namespace KuaiCard.WebComponents
{
    using System;
    using System.Collections.Generic;

    public class RandomController
    {
        private int _Count;
        public List<char> datas = new List<char>((IEnumerable<char>) new char[] { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
         });
        public List<ushort> weights = new List<ushort>((IEnumerable<ushort>) new ushort[] { 
            1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1
         });

        public RandomController(ushort count)
        {
            if (count > 0x1a)
            {
                throw new Exception("抽取个数不能超过数据集合大小！！");
            }
            this._Count = count;
        }

        public char[] ControllerRandomExtract(Random rand)
        {
            List<char> list = new List<char>();
            if (rand != null)
            {
                Dictionary<char, int> dict = new Dictionary<char, int>(0x1a);
                for (int i = this.datas.Count - 1; i >= 0; i--)
                {
                    dict.Add(this.datas[i], rand.Next(100) * this.weights[i]);
                }
                foreach (KeyValuePair<char, int> pair in this.SortByValue(dict).GetRange(0, this.Count))
                {
                    list.Add(pair.Key);
                }
            }
            return list.ToArray();
        }

        public char[] RandomExtract(Random rand)
        {
            List<char> list = new List<char>();
            if (rand != null)
            {
                int count = this.Count;
                while (count > 0)
                {
                    char item = this.datas[rand.Next(0x19)];
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                        count--;
                    }
                }
            }
            return list.ToArray();
        }

        private List<KeyValuePair<char, int>> SortByValue(Dictionary<char, int> dict)
        {
            List<KeyValuePair<char, int>> list = new List<KeyValuePair<char, int>>();
            if (dict != null)
            {
                list.AddRange(dict);
                list.Sort(delegate (KeyValuePair<char, int> kvp1, KeyValuePair<char, int> kvp2) {
                    return kvp2.Value - kvp1.Value;
                });
            }
            return list;
        }

        public int Count
        {
            get
            {
                return this._Count;
            }
            set
            {
                this._Count = value;
            }
        }
    }
}

