using System;
using System.ComponentModel;

namespace Wuqi.Webdiyer
{
	public sealed class PageChangingEventArgs : CancelEventArgs
	{
		private readonly int _newpageindex;

		public int NewPageIndex
		{
			get
			{
				return this._newpageindex;
			}
		}

		public PageChangingEventArgs(int newPageIndex)
		{
			this._newpageindex = newPageIndex;
		}
	}
}
