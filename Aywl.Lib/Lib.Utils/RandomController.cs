using System;
using System.Collections.Generic;

namespace OriginalStudio.Lib.Utils
{
	public class RandomController
	{
		public List<int> datas = new List<int>((IEnumerable<int>)new int[0]);

		public List<ushort> weights = new List<ushort>((IEnumerable<ushort>)new ushort[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1
		});

		private int _Count;

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

		public RandomController(ushort count)
		{
			if (count > 26)
			{
				throw new Exception("抽取个数不能超过数据集合大小！！");
			}
			this._Count = (int)count;
		}

		private List<KeyValuePair<int, int>> SortByValue(Dictionary<int, int> dict)
		{
			List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
			if (dict != null)
			{
				list.AddRange(dict);
				list.Sort((KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2) => kvp2.Value - kvp1.Value);
			}
			return list;
		}

		public int[] ControllerRandomExtract(Random rand)
		{
			List<int> list = new List<int>();
			if (rand != null)
			{
				Dictionary<int, int> dict = new Dictionary<int, int>(26);
				for (int index = this.datas.Count - 1; index >= 0; index--)
				{
					dict.Add(this.datas[index], rand.Next(1000) * (int)this.weights[index]);
				}
				foreach (KeyValuePair<int, int> keyValuePair in this.SortByValue(dict).GetRange(0, this.Count))
				{
					list.Add(keyValuePair.Key);
				}
			}
			return list.ToArray();
		}

		public int[] RandomExtract(Random rand)
		{
			List<int> list = new List<int>();
			if (rand != null)
			{
				int count = this.Count;
				while (count > 0)
				{
					int num = this.datas[rand.Next(25)];
					if (!list.Contains(num))
					{
						list.Add(num);
						count--;
					}
				}
			}
			return list.ToArray();
		}
	}
}
