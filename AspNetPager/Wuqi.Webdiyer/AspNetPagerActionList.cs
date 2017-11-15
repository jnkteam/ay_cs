using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class AspNetPagerActionList : DesignerActionList
	{
		private AspNetPager pager;

		private DesignerActionUIService svc;

		public bool UrlPaging
		{
			get
			{
				return this.pager.UrlPaging;
			}
			set
			{
				this.SetProperty("UrlPaging", value);
			}
		}

		public bool ShowFirstLast
		{
			get
			{
				return this.pager.ShowFirstLast;
			}
			set
			{
				this.SetProperty("ShowFirstLast", value);
			}
		}

		public bool ShowPrevNext
		{
			get
			{
				return this.pager.ShowPrevNext;
			}
			set
			{
				this.SetProperty("ShowPrevNext", value);
			}
		}

		public bool CenterCurrentPageButton
		{
			get
			{
				return this.pager.CenterCurrentPageButton;
			}
			set
			{
				this.SetProperty("CenterCurrentPageButton", value);
			}
		}

		public AspNetPagerActionList(IComponent component) : base(component)
		{
			this.pager = (component as AspNetPager);
			this.svc = (base.GetService(typeof(DesignerActionUIService)) as DesignerActionUIService);
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			return new DesignerActionItemCollection
			{
				new DesignerActionHeaderItem("分页方式"),
				new DesignerActionPropertyItem("UrlPaging", "启用Url分页", "分页"),
				new DesignerActionHeaderItem("外观及文本"),
				new DesignerActionPropertyItem("ShowFirstLast", "显示首页和尾页按钮", "外观"),
				new DesignerActionPropertyItem("ShowPrevNext", "显示上一页和下一页按钮", "外观"),
				new DesignerActionPropertyItem("CenterCurrentPageButton", "居中显示当前页数字索引按钮", "外观"),
				new DesignerActionMethodItem(this, "SetPageIndexBox", "页索引文本或下拉框...", "外观", true),
				new DesignerActionMethodItem(this, "SetNavText", "导航按钮显示文本...", "外观", true),
				new DesignerActionMethodItem(this, "SetCustomInfoHTML", "自定义信息区显示方式及内容...", "外观", true),
				new DesignerActionMethodItem(this, "ShowSPWindow", "分页存储过程生成工具...", "工具及帮助", true),
				new DesignerActionMethodItem(this, "ShowAboutForm", "关于AspNetPager...", "工具及帮助", true)
			};
		}

		public void SetNavText()
		{
			NavTextForm navTextForm = new NavTextForm(this.pager.FirstPageText, this.pager.LastPageText, this.pager.PrevPageText, this.pager.NextPageText);
			if (navTextForm.ShowDialog() == DialogResult.OK)
			{
				this.SetProperty("FirstPageText", navTextForm.FirstPageText);
				this.SetProperty("LastPageText", navTextForm.LastPageText);
				this.SetProperty("PrevPageText", navTextForm.PrevPageText);
				this.SetProperty("NextPageText", navTextForm.NextPageText);
			}
		}

		public void SetCustomInfoHTML()
		{
			CustomInfoForm customInfoForm = new CustomInfoForm(this.pager.ShowCustomInfoSection, this.pager.CustomInfoHTML);
			if (customInfoForm.ShowDialog() == DialogResult.OK)
			{
				this.SetProperty("ShowCustomInfoSection", customInfoForm.ShowCustomSection);
				this.SetProperty("CustomInfoHTML", customInfoForm.CustomInfoHtml);
			}
		}

		public void SetPageIndexBox()
		{
			PageIndexBoxForm pageIndexBoxForm = new PageIndexBoxForm(this.pager.ShowPageIndexBox, this.pager.ShowBoxThreshold, this.pager.PageIndexBoxType, this.pager.TextBeforePageIndexBox, this.pager.TextAfterPageIndexBox, this.pager.SubmitButtonText);
			if (pageIndexBoxForm.ShowDialog() == DialogResult.OK)
			{
				this.SetProperty("ShowPageIndexBox", pageIndexBoxForm.ShowIndexBox);
				this.SetProperty("ShowBoxThreshold", pageIndexBoxForm.Threshold);
				this.SetProperty("PageIndexBoxType", pageIndexBoxForm.BoxType);
				this.SetProperty("TextBeforePageIndexBox", pageIndexBoxForm.TextBeforeBox);
				this.SetProperty("TextAfterPageIndexBox", pageIndexBoxForm.TextAfterBox);
				this.SetProperty("SubmitButtonText", pageIndexBoxForm.SubmitButtonText);
			}
		}

		public void ShowSPWindow()
		{
			StoredProcForm storedProcForm = new StoredProcForm();
			storedProcForm.ShowDialog();
		}

		public void ShowAboutForm()
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog();
		}

		private void SetProperty(string propertyName, object value)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.pager)[propertyName];
			propertyDescriptor.SetValue(this.pager, value);
		}
	}
}
