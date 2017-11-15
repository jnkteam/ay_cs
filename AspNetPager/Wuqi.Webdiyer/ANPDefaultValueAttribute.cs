using System;
using System.ComponentModel;

namespace Wuqi.Webdiyer
{
	[AttributeUsage(AttributeTargets.All)]
	internal class ANPDefaultValueAttribute : DefaultValueAttribute
	{
		private bool localized;

		public override object Value
		{
			get
			{
				if (!this.localized)
				{
					this.localized = true;
					string text = (string)base.Value;
					if (!string.IsNullOrEmpty(text))
					{
						return SR.GetString(text);
					}
				}
				return base.Value;
			}
		}

		public ANPDefaultValueAttribute(string name) : base(name)
		{
			this.localized = false;
		}
	}
}
