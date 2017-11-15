using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wuqi.Webdiyer
{
	internal class AspNetPagerIDConverter : ControlIDConverter
	{
		protected override bool FilterControl(Control control)
		{
			return control is AspNetPager;
		}
	}
}
