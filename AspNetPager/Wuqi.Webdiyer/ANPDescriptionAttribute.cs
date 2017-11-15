using System;
using System.ComponentModel;

namespace Wuqi.Webdiyer
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class ANPDescriptionAttribute : DescriptionAttribute
	{
		private bool replaced;

		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = SR.GetString(base.DescriptionValue);
				}
				return base.Description;
			}
		}

		public ANPDescriptionAttribute(string text) : base(text)
		{
			this.replaced = false;
		}
	}
}
