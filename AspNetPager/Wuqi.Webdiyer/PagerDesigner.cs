using System;
using System.ComponentModel.Design;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;

namespace Wuqi.Webdiyer
{
	public class PagerDesigner : ControlDesigner
	{
		private AspNetPager wb;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				return new DesignerActionListCollection
				{
					new AspNetPagerActionList(base.Component)
				};
			}
		}

		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			region.Selectable = false;
			return null;
		}

		public override string GetDesignTimeHtml()
		{
			this.wb = (AspNetPager)base.Component;
			this.wb.RecordCount = 225;
			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
			this.wb.RenderControl(writer);
			return stringWriter.ToString();
		}

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			string instruction = "Error creating control：" + e.Message;
			return base.CreatePlaceHolderDesignTimeHtml(instruction);
		}
	}
}
