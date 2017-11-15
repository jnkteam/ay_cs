using System;
using System.ComponentModel;

namespace Wuqi.Webdiyer
{
	[AttributeUsage(AttributeTargets.All)]
	internal class ANPCategoryAttribute : CategoryAttribute
	{
		internal ANPCategoryAttribute(string name) : base(name)
		{
		}

		protected override string GetLocalizedString(string value)
		{
			string text = base.GetLocalizedString(value);
			if (text == null)
			{
				text = SR.GetString(value);
			}
			return text;
		}
	}
}
