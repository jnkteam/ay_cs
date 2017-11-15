using System;
using System.Data;

namespace OriginalStudio.Lib
{
	[Serializable]
	public class PageData
	{
		private DataSet _items = new DataSet();

		private int _recordCount = 0;

		public DataSet Items
		{
			get
			{
				return this._items;
			}
			set
			{
				this._items = value;
			}
		}

		public int RecordCount
		{
			get
			{
				return this._recordCount;
			}
			set
			{
				this._recordCount = value;
			}
		}
	}
}
