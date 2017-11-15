using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wuqi.Webdiyer
{
	[DefaultEvent("PageChanged"), DefaultProperty("PageSize"), Designer(typeof(PagerDesigner)), ParseChildren(false), PersistChildren(false), ToolboxData("<{0}:AspNetPager runat=server></{0}:AspNetPager>"), ANPDescription("desc_AspNetPager")]
	[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AspNetPager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
	{
		private const string ver = "7.0.2";

		private string cssClassName;

		private string inputPageIndex;

		private string currentUrl;

		private string queryString;

		private AspNetPager cloneFrom;

		private static readonly object EventPageChanging = new object();

		private static readonly object EventPageChanged = new object();

		public event PageChangingEventHandler PageChanging
		{
			add
			{
				base.Events.AddHandler(AspNetPager.EventPageChanging, value);
			}
			remove
			{
				base.Events.RemoveHandler(AspNetPager.EventPageChanging, value);
			}
		}

		public event EventHandler PageChanged
		{
			add
			{
				base.Events.AddHandler(AspNetPager.EventPageChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(AspNetPager.EventPageChanged, value);
			}
		}

		[Browsable(true), DefaultValue(false), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_ShowNavigationToolTip")]
		public bool ShowNavigationToolTip
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowNavigationToolTip;
				}
				object obj = this.ViewState["ShowNvToolTip"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ShowNvToolTip"] = value;
			}
		}

		[Browsable(true), Themeable(true), ANPCategory("cat_Navigation"), ANPDefaultValue("def_NavigationToolTipTextFormatString"), ANPDescription("desc_NavigationToolTipTextFormatString")]
		public string NavigationToolTipTextFormatString
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.NavigationToolTipTextFormatString;
				}
				object obj = this.ViewState["NvToolTipFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				if (this.ShowNavigationToolTip)
				{
					return SR.GetString("def_NavigationToolTipTextFormatString");
				}
				return null;
			}
			set
			{
				string text = value;
				if (text.Trim().Length < 1 && text.IndexOf("{0}") < 0)
				{
					text = "{0}";
				}
				this.ViewState["NvToolTipFormatString"] = text;
			}
		}

		[Browsable(true), DefaultValue(""), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_NBTFormatString")]
		public string NumericButtonTextFormatString
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.NumericButtonTextFormatString;
				}
				object obj = this.ViewState["NumericButtonTextFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["NumericButtonTextFormatString"] = value;
			}
		}

		[Browsable(true), DefaultValue(""), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_CPBTextFormatString")]
		public string CurrentPageButtonTextFormatString
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CurrentPageButtonTextFormatString;
				}
				object obj = this.ViewState["CurrentPageButtonTextFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return this.NumericButtonTextFormatString;
			}
			set
			{
				this.ViewState["CurrentPageButtonTextFormatString"] = value;
			}
		}

		[Browsable(true), DefaultValue(PagingButtonType.Text), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_PagingButtonType")]
		public PagingButtonType PagingButtonType
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PagingButtonType;
				}
				object obj = this.ViewState["PagingButtonType"];
				if (obj != null)
				{
					return (PagingButtonType)obj;
				}
				return PagingButtonType.Text;
			}
			set
			{
				this.ViewState["PagingButtonType"] = value;
			}
		}

		[Browsable(true), DefaultValue(PagingButtonType.Text), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_NumericButtonType")]
		public PagingButtonType NumericButtonType
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.NumericButtonType;
				}
				object obj = this.ViewState["NumericButtonType"];
				if (obj != null)
				{
					return (PagingButtonType)obj;
				}
				return this.PagingButtonType;
			}
			set
			{
				this.ViewState["NumericButtonType"] = value;
			}
		}

		[Browsable(true), DefaultValue(PagingButtonType.Text), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_NavigationButtonType")]
		public PagingButtonType NavigationButtonType
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.NavigationButtonType;
				}
				object obj = this.ViewState["NavigationButtonType"];
				if (obj != null)
				{
					return (PagingButtonType)obj;
				}
				return this.PagingButtonType;
			}
			set
			{
				this.ViewState["NavigationButtonType"] = value;
			}
		}

		[Browsable(true), DefaultValue(""), TypeConverter(typeof(TargetConverter)), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_UrlPagingTarget")]
		public string UrlPagingTarget
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.UrlPagingTarget;
				}
				return (string)this.ViewState["UrlPagingTarget"];
			}
			set
			{
				this.ViewState["UrlPagingTarget"] = value;
			}
		}

		[Browsable(true), DefaultValue(PagingButtonType.Text), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_MoreButtonType")]
		public PagingButtonType MoreButtonType
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.MoreButtonType;
				}
				object obj = this.ViewState["MoreButtonType"];
				if (obj != null)
				{
					return (PagingButtonType)obj;
				}
				return this.PagingButtonType;
			}
			set
			{
				this.ViewState["MoreButtonType"] = value;
			}
		}

		[Browsable(true), DefaultValue(typeof(Unit), "5px"), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_PagingButtonSpacing")]
		public Unit PagingButtonSpacing
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PagingButtonSpacing;
				}
				object obj = this.ViewState["PagingButtonSpacing"];
				if (obj != null)
				{
					return Unit.Parse(obj.ToString());
				}
				return Unit.Pixel(5);
			}
			set
			{
				this.ViewState["PagingButtonSpacing"] = value;
			}
		}

		[Browsable(true), DefaultValue(true), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_ShowFirstLast")]
		public bool ShowFirstLast
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowFirstLast;
				}
				object obj = this.ViewState["ShowFirstLast"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowFirstLast"] = value;
			}
		}

		[Browsable(true), DefaultValue(true), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_ShowPrevNext")]
		public bool ShowPrevNext
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowPrevNext;
				}
				object obj = this.ViewState["ShowPrevNext"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowPrevNext"] = value;
			}
		}

		[Browsable(true), DefaultValue(true), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_ShowPageIndex")]
		public bool ShowPageIndex
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowPageIndex;
				}
				object obj = this.ViewState["ShowPageIndex"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowPageIndex"] = value;
			}
		}

		[Browsable(true), DefaultValue("&lt;&lt;"), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_FirstPageText")]
		public string FirstPageText
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.FirstPageText;
				}
				object obj = this.ViewState["FirstPageText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&lt;&lt;";
			}
			set
			{
				this.ViewState["FirstPageText"] = value;
			}
		}

		[Browsable(true), DefaultValue("&lt;"), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_PrevPageText")]
		public string PrevPageText
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PrevPageText;
				}
				object obj = this.ViewState["PrevPageText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&lt;";
			}
			set
			{
				this.ViewState["PrevPageText"] = value;
			}
		}

		[Browsable(true), DefaultValue("&gt;"), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_NextPageText")]
		public string NextPageText
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.NextPageText;
				}
				object obj = this.ViewState["NextPageText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&gt;";
			}
			set
			{
				this.ViewState["NextPageText"] = value;
			}
		}

		[Browsable(true), DefaultValue("&gt;&gt;"), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_LastPageText")]
		public string LastPageText
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.LastPageText;
				}
				object obj = this.ViewState["LastPageText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "&gt;&gt;";
			}
			set
			{
				this.ViewState["LastPageText"] = value;
			}
		}

		[Browsable(true), DefaultValue(10), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_NumericButtonCount")]
		public int NumericButtonCount
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.NumericButtonCount;
				}
				object obj = this.ViewState["NumericButtonCount"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 10;
			}
			set
			{
				this.ViewState["NumericButtonCount"] = value;
			}
		}

		[Browsable(true), DefaultValue(true), Themeable(true), ANPCategory("cat_Navigation"), ANPDescription("desc_ShowDisabledButtons")]
		public bool ShowDisabledButtons
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowDisabledButtons;
				}
				object obj = this.ViewState["ShowDisabledButtons"];
				return obj == null || (bool)obj;
			}
			set
			{
				this.ViewState["ShowDisabledButtons"] = value;
			}
		}

		[Browsable(true), DefaultValue(false), Themeable(true), ANPCategory("Behavior"), ANPDescription("desc_CenterCurrentPageButton")]
		public bool CenterCurrentPageButton
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CenterCurrentPageButton;
				}
				object obj = this.ViewState["CenterCurrentPageButton"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["CenterCurrentPageButton"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_ImagePath")]
		public string ImagePath
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ImagePath;
				}
				string text = (string)this.ViewState["ImagePath"];
				if (text != null)
				{
					text = base.ResolveUrl(text);
				}
				return text;
			}
			set
			{
				string text = value.Trim().Replace("\\", "/");
				this.ViewState["ImagePath"] = (text.EndsWith("/") ? text : (text + "/"));
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(".gif"), Themeable(true), ANPDescription("desc_ButtonImageExtension")]
		public string ButtonImageExtension
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ButtonImageExtension;
				}
				object obj = this.ViewState["ButtonImageExtension"];
				if (obj != null)
				{
					return (string)obj;
				}
				return ".gif";
			}
			set
			{
				string text = value.Trim();
				this.ViewState["ButtonImageExtension"] = (text.StartsWith(".") ? text : ("." + text));
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), Themeable(true), ANPDescription("desc_ButtonImageNameExtension")]
		public string ButtonImageNameExtension
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ButtonImageNameExtension;
				}
				object obj = this.ViewState["ButtonImageNameExtension"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["ButtonImageNameExtension"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), Themeable(true), ANPDescription("desc_CpiButtonImageNameExtension")]
		public string CpiButtonImageNameExtension
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CpiButtonImageNameExtension;
				}
				object obj = this.ViewState["CpiButtonImageNameExtension"];
				if (obj != null)
				{
					return (string)obj;
				}
				return this.ButtonImageNameExtension;
			}
			set
			{
				this.ViewState["CpiButtonImageNameExtension"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), Themeable(true), ANPDescription("desc_DisabledButtonImageNameExtension")]
		public string DisabledButtonImageNameExtension
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.DisabledButtonImageNameExtension;
				}
				object obj = this.ViewState["DisabledButtonImageNameExtension"];
				if (obj != null)
				{
					return (string)obj;
				}
				return this.ButtonImageNameExtension;
			}
			set
			{
				this.ViewState["DisabledButtonImageNameExtension"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(ImageAlign.NotSet), ANPDescription("desc_ButtonImageAlign")]
		public ImageAlign ButtonImageAlign
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ButtonImageAlign;
				}
				object obj = this.ViewState["ButtonImageAlign"];
				if (obj != null)
				{
					return (ImageAlign)obj;
				}
				return ImageAlign.NotSet;
			}
			set
			{
				if (value != ImageAlign.Right && value != ImageAlign.Left)
				{
					this.ViewState["ButtonImageAlign"] = value;
				}
			}
		}

		[Browsable(true), DefaultValue(false), ANPCategory("cat_Paging"), ANPDescription("desc_UrlPaging")]
		public bool UrlPaging
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.UrlPaging;
				}
				object obj = this.ViewState["UrlPaging"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["UrlPaging"] = value;
			}
		}

		[Browsable(true), DefaultValue("page"), ANPCategory("cat_Paging"), ANPDescription("desc_UrlPageIndexName")]
		public string UrlPageIndexName
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.UrlPageIndexName;
				}
				object obj = this.ViewState["UrlPageIndexName"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "page";
			}
			set
			{
				this.ViewState["UrlPageIndexName"] = value;
			}
		}

		[Browsable(true), DefaultValue(false), ANPCategory("cat_Paging"), ANPDescription("desc_ReverseUrlPageIndex")]
		public bool ReverseUrlPageIndex
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ReverseUrlPageIndex;
				}
				object obj = this.ViewState["ReverseUrlPageIndex"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["ReverseUrlPageIndex"] = value;
			}
		}

		[Browsable(false), DefaultValue(1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), ReadOnly(true), ANPCategory("cat_Paging"), ANPDescription("desc_CurrentPageIndex")]
		public int CurrentPageIndex
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CurrentPageIndex;
				}
				object obj = this.ViewState["CurrentPageIndex"];
				int num = (obj == null) ? 1 : ((int)obj);
				if (num > this.PageCount && this.PageCount > 0)
				{
					return this.PageCount;
				}
				if (num < 1)
				{
					return 1;
				}
				return num;
			}
			set
			{
				int num = value;
				if (num < 1)
				{
					num = 1;
				}
				else if (num > this.PageCount)
				{
					num = this.PageCount;
				}
				this.ViewState["CurrentPageIndex"] = num;
			}
		}

		[Browsable(false), Category("Data"), DefaultValue(0), ANPDescription("desc_RecordCount")]
		public int RecordCount
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.RecordCount;
				}
				object obj = this.ViewState["Recordcount"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				this.ViewState["Recordcount"] = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PagesRemain
		{
			get
			{
				return this.PageCount - this.CurrentPageIndex;
			}
		}

		[Browsable(true), DefaultValue(10), ANPCategory("cat_Paging"), ANPDescription("desc_PageSize")]
		public int PageSize
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PageSize;
				}
				object obj = this.ViewState["PageSize"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 10;
			}
			set
			{
				this.ViewState["PageSize"] = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int RecordsRemain
		{
			get
			{
				if (this.CurrentPageIndex < this.PageCount)
				{
					return this.RecordCount - this.CurrentPageIndex * this.PageSize;
				}
				return 0;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int StartRecordIndex
		{
			get
			{
				return (this.CurrentPageIndex - 1) * this.PageSize + 1;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int EndRecordIndex
		{
			get
			{
				return this.RecordCount - this.RecordsRemain;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PageCount
		{
			get
			{
				if (this.RecordCount == 0)
				{
					return 1;
				}
				return (int)Math.Ceiling((double)this.RecordCount / (double)this.PageSize);
			}
		}

		[Browsable(false), DefaultValue(ShowInputBox.Auto), Obsolete("该属性已废弃，请使用 ShowPageIndexBox 属性"), Themeable(false), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_ShowInputBox")]
		public ShowInputBox ShowInputBox
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowInputBox;
				}
				object obj = this.ViewState["ShowInputBox"];
				if (obj != null)
				{
					return (ShowInputBox)obj;
				}
				return ShowInputBox.Auto;
			}
			set
			{
				this.ViewState["ShowInputBox"] = value;
			}
		}

		[Browsable(true), DefaultValue(null), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_ShowPageIndexBox")]
		public ShowPageIndexBox ShowPageIndexBox
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowPageIndexBox;
				}
				object obj = this.ViewState["ShowPageIndexBox"];
				if (obj != null)
				{
					return (ShowPageIndexBox)obj;
				}
				return ShowPageIndexBox.Auto;
			}
			set
			{
				this.ViewState["ShowPageIndexBox"] = value;
			}
		}

		[Browsable(true), DefaultValue(null), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_PageIndexBoxType")]
		public PageIndexBoxType PageIndexBoxType
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PageIndexBoxType;
				}
				object obj = this.ViewState["PageIndexBoxType"];
				if (obj != null)
				{
					return (PageIndexBoxType)obj;
				}
				return PageIndexBoxType.TextBox;
			}
			set
			{
				this.ViewState["PageIndexBoxType"] = value;
			}
		}

		[Browsable(false), DefaultValue(null), Obsolete("该属性已废弃，请使用 PageIndexBoxClass 属性"), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_InputBoxClass")]
		public string InputBoxClass
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.InputBoxClass;
				}
				object obj = this.ViewState["PageIndexBoxClass"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				if (value.Trim().Length > 0)
				{
					this.ViewState["PageIndexBoxClass"] = value;
				}
			}
		}

		[Browsable(true), DefaultValue(null), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_PageIndexBoxClasss")]
		public string PageIndexBoxClass
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PageIndexBoxClass;
				}
				object obj = this.ViewState["PageIndexBoxClass"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				if (value.Trim().Length > 0)
				{
					this.ViewState["PageIndexBoxClass"] = value;
				}
			}
		}

		[Browsable(false), DefaultValue(null), Obsolete("该属性已废弃，请使用 PageIndexBoxStyle 属性"), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_InputBoxStyle")]
		public string InputBoxStyle
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.InputBoxStyle;
				}
				object obj = this.ViewState["PageIndexBoxStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				if (value.Trim().Length > 0)
				{
					this.ViewState["PageIndexBoxStyle"] = value;
				}
			}
		}

		[Browsable(true), DefaultValue(null), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_PageIndexBoxStyle")]
		public string PageIndexBoxStyle
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PageIndexBoxStyle;
				}
				object obj = this.ViewState["PageIndexBoxStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				if (value.Trim().Length > 0)
				{
					this.ViewState["PageIndexBoxStyle"] = value;
				}
			}
		}

		[Browsable(false), DefaultValue(null), Obsolete("该属性已废弃，请使用 TextBeforePageIndexBox 属性"), Themeable(false), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_TextBeforeInputBox")]
		public string TextBeforeInputBox
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.TextBeforeInputBox;
				}
				object obj = this.ViewState["TextBeforePageIndexBox"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["TextBeforePageIndexBox"] = value;
			}
		}

		[Browsable(true), DefaultValue(null), Themeable(true), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_TextBeforePageIndexBox")]
		public string TextBeforePageIndexBox
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.TextBeforePageIndexBox;
				}
				object obj = this.ViewState["TextBeforePageIndexBox"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["TextBeforePageIndexBox"] = value;
			}
		}

		[Browsable(false), DefaultValue(null), Obsolete("该属性已废弃，请使用 TextAfterPageIndexBox 属性"), Themeable(false), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_TextAfterInputBox")]
		public string TextAfterInputBox
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.TextAfterInputBox;
				}
				object obj = this.ViewState["TextAfterPageIndexBox"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["TextAfterPageIndexBox"] = value;
			}
		}

		[Browsable(true), DefaultValue(null), Themeable(true), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_TextAfterPageIndexBox")]
		public string TextAfterPageIndexBox
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.TextAfterPageIndexBox;
				}
				object obj = this.ViewState["TextAfterPageIndexBox"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["TextAfterPageIndexBox"] = value;
			}
		}

		[Browsable(true), DefaultValue("go"), Themeable(true), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_SubmitButtonText")]
		public string SubmitButtonText
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.SubmitButtonText;
				}
				object obj = this.ViewState["SubmitButtonText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "go";
			}
			set
			{
				if (value == null)
				{
					value = "go";
				}
				this.ViewState["SubmitButtonText"] = value;
			}
		}

		[Browsable(true), DefaultValue(null), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_SubmitButtonClass")]
		public string SubmitButtonClass
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.SubmitButtonClass;
				}
				object obj = this.ViewState["SubmitButtonClass"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["SubmitButtonClass"] = value;
			}
		}

		[Browsable(true), DefaultValue(null), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_SubmitButtonStyle")]
		public string SubmitButtonStyle
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.SubmitButtonStyle;
				}
				object obj = this.ViewState["SubmitButtonStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["SubmitButtonStyle"] = value;
			}
		}

		[Browsable(true), DefaultValue(30), Themeable(true), ANPCategory("cat_PageIndexBox"), ANPDescription("desc_ShowBoxThreshold")]
		public int ShowBoxThreshold
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowBoxThreshold;
				}
				object obj = this.ViewState["ShowBoxThreshold"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 30;
			}
			set
			{
				this.ViewState["ShowBoxThreshold"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(ShowCustomInfoSection.Never), Themeable(true), ANPDescription("desc_ShowCustomInfoSection")]
		public ShowCustomInfoSection ShowCustomInfoSection
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.ShowCustomInfoSection;
				}
				object obj = this.ViewState["ShowCustomInfoSection"];
				if (obj != null)
				{
					return (ShowCustomInfoSection)obj;
				}
				return ShowCustomInfoSection.Never;
			}
			set
			{
				this.ViewState["ShowCustomInfoSection"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(HorizontalAlign.NotSet), ANPDescription("desc_CustomInfoTextAlign")]
		public HorizontalAlign CustomInfoTextAlign
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CustomInfoTextAlign;
				}
				object obj = this.ViewState["CustomInfoTextAlign"];
				if (obj != null)
				{
					return (HorizontalAlign)obj;
				}
				return HorizontalAlign.NotSet;
			}
			set
			{
				this.ViewState["CustomInfoTextAlign"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(typeof(Unit), "40%"), ANPDescription("desc_CustomInfoSectionWidth")]
		public Unit CustomInfoSectionWidth
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CustomInfoSectionWidth;
				}
				object obj = this.ViewState["CustomInfoSectionWidth"];
				if (obj != null)
				{
					return (Unit)obj;
				}
				return Unit.Percentage(40.0);
			}
			set
			{
				this.ViewState["CustomInfoSectionWidth"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CustomInfoClass")]
		public string CustomInfoClass
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CustomInfoClass;
				}
				object obj = this.ViewState["CustomInfoClass"];
				if (obj != null)
				{
					return (string)obj;
				}
				return this.CssClass;
			}
			set
			{
				this.ViewState["CustomInfoClass"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CustomInfoStyle")]
		public string CustomInfoStyle
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CustomInfoStyle;
				}
				object obj = this.ViewState["CustomInfoStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return base.Style.Value;
			}
			set
			{
				this.ViewState["CustomInfoStyle"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue("Page %CurrentPageIndex% of %PageCount%"), Themeable(true), ANPDescription("desc_CustomInfoHTML")]
		public string CustomInfoHTML
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CustomInfoHTML;
				}
				object obj = this.ViewState["CustomInfoText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "Page %CurrentPageIndex% of %PageCount%";
			}
			set
			{
				this.ViewState["CustomInfoText"] = value;
			}
		}

		[Browsable(true), Category("Behavior"), DefaultValue(false), TypeConverter(typeof(AspNetPagerIDConverter)), Themeable(false), ANPDescription("desc_CloneFrom")]
		public string CloneFrom
		{
			get
			{
				return (string)this.ViewState["CloneFrom"];
			}
			set
			{
				if (value != null && string.Empty == value.Trim())
				{
					throw new ArgumentNullException("CloneFrom", "The Value of property CloneFrom can not be empty string!");
				}
				if (this.ID.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException("The property value of CloneFrom can not be set to control itself!", "CloneFrom");
				}
				this.ViewState["CloneFrom"] = value;
			}
		}

		public override bool EnableTheming
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.EnableTheming;
				}
				return base.EnableTheming;
			}
			set
			{
				base.EnableTheming = value;
			}
		}

		public override string SkinID
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.SkinID;
				}
				return base.SkinID;
			}
			set
			{
				base.SkinID = value;
			}
		}

		[Browsable(true), Category("Behavior"), DefaultValue(false), Themeable(true), ANPDescription("desc_EnableUrlWriting")]
		public bool EnableUrlRewriting
		{
			get
			{
				object obj = this.ViewState["UrlRewriting"];
				if (obj == null)
				{
					return this.cloneFrom != null && this.cloneFrom.EnableUrlRewriting;
				}
				return (bool)obj;
			}
			set
			{
				this.ViewState["UrlRewriting"] = value;
				if (value)
				{
					this.UrlPaging = true;
				}
			}
		}

		[Browsable(true), Category("Behavior"), DefaultValue(null), Themeable(true), ANPDescription("desc_UrlRewritePattern")]
		public string UrlRewritePattern
		{
			get
			{
				object obj = this.ViewState["URPattern"];
				if (obj != null)
				{
					return (string)obj;
				}
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.UrlRewritePattern;
				}
				if (this.EnableUrlRewriting)
				{
					string filePath = this.Page.Request.FilePath;
					return Path.GetFileNameWithoutExtension(filePath) + "_{0}" + Path.GetExtension(filePath);
				}
				return null;
			}
			set
			{
				this.ViewState["URPattern"] = value;
			}
		}

		[Browsable(true), Category("Behavior"), DefaultValue(false), Themeable(true), ANPDescription("desc_AlwaysShow")]
		public bool AlwaysShow
		{
			get
			{
				object obj = this.ViewState["AlwaysShow"];
				if (obj == null)
				{
					return this.cloneFrom != null && this.cloneFrom.AlwaysShow;
				}
				return (bool)obj;
			}
			set
			{
				this.ViewState["AlwaysShow"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CssClass")]
		public override string CssClass
		{
			get
			{
				return base.CssClass;
			}
			set
			{
				base.CssClass = value;
				this.cssClassName = value;
			}
		}

		public override bool Wrap
		{
			get
			{
				return base.Wrap;
			}
			set
			{
				base.Wrap = false;
			}
		}

		[Browsable(true), Category("Data"), Themeable(true), ANPDefaultValue("def_PIOutOfRangerMsg"), ANPDescription("desc_PIOutOfRangeMsg")]
		public string PageIndexOutOfRangeErrorMessage
		{
			get
			{
				object obj = this.ViewState["PIOutOfRangeErrorMsg"];
				if (obj != null)
				{
					return (string)obj;
				}
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.PageIndexOutOfRangeErrorMessage;
				}
				return SR.GetString("def_PIOutOfRangerMsg");
			}
			set
			{
				this.ViewState["PIOutOfRangeErrorMsg"] = value;
			}
		}

		[Browsable(true), Category("Data"), Themeable(true), ANPDefaultValue("def_InvalidPIErrorMsg"), ANPDescription("desc_InvalidPIErrorMsg")]
		public string InvalidPageIndexErrorMessage
		{
			get
			{
				object obj = this.ViewState["InvalidPIErrorMsg"];
				if (obj != null)
				{
					return (string)obj;
				}
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.InvalidPageIndexErrorMessage;
				}
				return SR.GetString("def_InvalidPIErrorMsg");
			}
			set
			{
				this.ViewState["InvalidPIErrorMsg"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CurrentPageButtonStyle")]
		public string CurrentPageButtonStyle
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CurrentPageButtonStyle;
				}
				object obj = this.ViewState["CPBStyle"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["CPBStyle"] = value;
			}
		}

		[Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CurrentPageButtonClass")]
		public string CurrentPageButtonClass
		{
			get
			{
				if (this.cloneFrom != null)
				{
					return this.cloneFrom.CurrentPageButtonClass;
				}
				object obj = this.ViewState["CPBClass"];
				if (obj != null)
				{
					return (string)obj;
				}
				return null;
			}
			set
			{
				this.ViewState["CPBClass"] = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this.CloneFrom != null && string.Empty != this.CloneFrom.Trim())
			{
				AspNetPager aspNetPager = this.Parent.FindControl(this.CloneFrom) as AspNetPager;
				if (aspNetPager == null)
				{
					string text = SR.GetString("clonefromexeption");
					if (text == null)
					{
						text = "The control \" %controlID% \" does not exist or is not of type Wuqi.Webdiyer.AspNetPager!";
					}
					throw new ArgumentException(text.Replace("%controlID%", this.CloneFrom), "CloneFrom");
				}
				if (aspNetPager.cloneFrom != null && this == aspNetPager.cloneFrom)
				{
					string text2 = SR.GetString("recusiveclonefrom");
					if (text2 == null)
					{
						text2 = "Invalid value for the CloneFrom property, AspNetPager controls can not to be cloned recursively!";
					}
					throw new ArgumentException(text2, "CloneFrom");
				}
				this.cloneFrom = aspNetPager;
				this.CssClass = this.cloneFrom.CssClass;
				this.Width = this.cloneFrom.Width;
				this.Height = this.cloneFrom.Height;
				this.HorizontalAlign = this.cloneFrom.HorizontalAlign;
				this.BackColor = this.cloneFrom.BackColor;
				this.BackImageUrl = this.cloneFrom.BackImageUrl;
				this.BorderColor = this.cloneFrom.BorderColor;
				this.BorderStyle = this.cloneFrom.BorderStyle;
				this.BorderWidth = this.cloneFrom.BorderWidth;
				this.Font.CopyFrom(this.cloneFrom.Font);
				this.ForeColor = this.cloneFrom.ForeColor;
				this.EnableViewState = this.cloneFrom.EnableViewState;
				this.Enabled = this.cloneFrom.Enabled;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			if (this.UrlPaging)
			{
				this.currentUrl = this.Page.Request.Path;
				this.queryString = this.Page.Request.ServerVariables["Query_String"];
				if (!this.Page.IsPostBack && this.cloneFrom == null)
				{
					int num;
					int.TryParse(this.Page.Request.QueryString[this.UrlPageIndexName], out num);
					if (num <= 0)
					{
						num = 1;
					}
					else if (this.ReverseUrlPageIndex)
					{
						num = this.PageCount - num + 1;
					}
					PageChangingEventArgs e2 = new PageChangingEventArgs(num);
					this.OnPageChanging(e2);
				}
			}
			else
			{
				this.inputPageIndex = this.Page.Request.Form[this.UniqueID + "_input"];
			}
			base.OnLoad(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (this.PageCount > 1 && (this.ShowPageIndexBox == ShowPageIndexBox.Always || (this.ShowPageIndexBox == ShowPageIndexBox.Auto && this.PageCount >= this.ShowBoxThreshold)))
			{
				StringBuilder stringBuilder = new StringBuilder("<script language=\"Javascript\"><!--\n");
				if (this.UrlPaging)
				{
					stringBuilder.Append("function ANP_goToPage(boxEl){if(boxEl!=null){var pi;if(boxEl.tagName==\"SELECT\")");
					stringBuilder.Append("{pi=boxEl.options[boxEl.selectedIndex].value;}else{pi=boxEl.value;}");
					if (string.IsNullOrEmpty(this.UrlPagingTarget))
					{
						stringBuilder.Append("location.href=\"").Append(this.GetHrefString(-1)).Append("\"");
					}
					else
					{
						stringBuilder.Append("window.open(\"").Append(this.GetHrefString(-1)).Append("\",\"").Append(this.UrlPagingTarget).Append("\")");
					}
					stringBuilder.Append(";}}\n");
				}
				if (this.PageIndexBoxType == PageIndexBoxType.TextBox)
				{
					string text = SR.GetString("checkinputscript");
					if (text != null)
					{
						text = text.Replace("%PageIndexOutOfRangeErrorMessage%", this.PageIndexOutOfRangeErrorMessage);
						text = text.Replace("%InvalidPageIndexErrorMessage%", this.InvalidPageIndexErrorMessage);
					}
					stringBuilder.Append(text).Append("\n");
					string @string = SR.GetString("keydownscript");
					stringBuilder.Append(@string);
				}
				stringBuilder.Append("\n--></script>");
				Type type = base.GetType();
				ClientScriptManager clientScript = this.Page.ClientScript;
				if (!clientScript.IsClientScriptBlockRegistered(type, "anp_script"))
				{
					clientScript.RegisterClientScriptBlock(type, "anp_script", stringBuilder.ToString());
				}
			}
			base.OnPreRender(e);
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (this.Page != null && !this.UrlPaging)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			base.AddAttributesToRender(writer);
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			bool flag = this.PageCount > 1 || (this.PageCount <= 1 && this.AlwaysShow);
			writer.WriteLine();
			writer.Write("<!-- ");
			writer.Write("AspNetPager V7.0.2 for VS2005 & VS2008  Copyright:2003-2007 Webdiyer (www.webdiyer.com)");
			writer.WriteLine(" -->");
			if (!flag)
			{
				writer.Write("<!--");
				writer.Write(SR.GetString("autohideinfo"));
				writer.Write("-->");
				return;
			}
			base.RenderBeginTag(writer);
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			if (this.PageCount > 1 || (this.PageCount <= 1 && this.AlwaysShow))
			{
				base.RenderEndTag(writer);
			}
			writer.WriteLine();
			writer.Write("<!-- ");
			writer.Write("AspNetPager V7.0.2 for VS2005 & VS2008 End");
			writer.WriteLine(" -->");
			writer.WriteLine();
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (this.PageCount <= 1 && !this.AlwaysShow)
			{
				return;
			}
			writer.Indent = 0;
			if (this.ShowCustomInfoSection == ShowCustomInfoSection.Left)
			{
				this.RenderCustomInfoSection(writer);
			}
			if (this.ShowCustomInfoSection != ShowCustomInfoSection.Never)
			{
				if (this.CustomInfoSectionWidth.Type == UnitType.Percentage)
				{
					writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Unit.Percentage(100.0 - this.CustomInfoSectionWidth.Value).ToString());
				}
				if (this.HorizontalAlign != HorizontalAlign.NotSet)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Align, this.HorizontalAlign.ToString().ToLower());
				}
				if (!string.IsNullOrEmpty(this.CssClass))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
				}
				writer.AddStyleAttribute("float", "left");
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
			}
			int num = (this.CurrentPageIndex - 1) / this.NumericButtonCount * this.NumericButtonCount;
			if (this.PageCount > this.NumericButtonCount)
			{
				int num2 = this.CurrentPageIndex - (int)Math.Ceiling((double)this.NumericButtonCount / 2.0);
				if (this.CenterCurrentPageButton && num2 > 0)
				{
					num = num2;
					if (num > this.PageCount - this.NumericButtonCount)
					{
						num = this.PageCount - this.NumericButtonCount;
					}
				}
			}
			int num3 = (num + this.NumericButtonCount > this.PageCount) ? this.PageCount : (num + this.NumericButtonCount);
			this.CreateNavigationButton(writer, "first");
			this.CreateNavigationButton(writer, "prev");
			if (this.ShowPageIndex)
			{
				if (num > 0)
				{
					this.CreateMoreButton(writer, num);
				}
				for (int i = num + 1; i <= num3; i++)
				{
					this.CreateNumericButton(writer, i);
				}
				if (this.PageCount > this.NumericButtonCount && num3 < this.PageCount)
				{
					this.CreateMoreButton(writer, num3 + 1);
				}
			}
			this.CreateNavigationButton(writer, "next");
			this.CreateNavigationButton(writer, "last");
			if (this.ShowPageIndexBox == ShowPageIndexBox.Always || (this.ShowPageIndexBox == ShowPageIndexBox.Auto && this.PageCount >= this.ShowBoxThreshold))
			{
				string text = this.UniqueID + "_input";
				writer.Write("&nbsp;&nbsp;");
				if (!string.IsNullOrEmpty(this.TextBeforePageIndexBox))
				{
					writer.Write(this.TextBeforePageIndexBox);
				}
				if (this.PageIndexBoxType == PageIndexBoxType.TextBox)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
					writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
					writer.AddAttribute(HtmlTextWriterAttribute.Value, this.CurrentPageIndex.ToString());
					if (!string.IsNullOrEmpty(this.PageIndexBoxStyle))
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Style, this.PageIndexBoxStyle);
					}
					if (!string.IsNullOrEmpty(this.PageIndexBoxClass))
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Class, this.PageIndexBoxClass);
					}
					if (!this.Enabled || (this.PageCount <= 1 && this.AlwaysShow))
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Name, text);
					writer.AddAttribute(HtmlTextWriterAttribute.Id, text);
					string text2 = string.Concat(new object[]
					{
						"ANP_checkInput('",
						text,
						"',",
						this.PageCount,
						")"
					});
					string value = "ANP_keydown(event,'" + this.UniqueID + "_btn');";
					string text3 = string.Concat(new string[]
					{
						"if(",
						text2,
						"){ANP_goToPage(document.getElementById('",
						text,
						"'));}"
					});
					writer.AddAttribute("onkeydown", value);
					writer.RenderBeginTag(HtmlTextWriterTag.Input);
					writer.RenderEndTag();
					if (!string.IsNullOrEmpty(this.TextAfterPageIndexBox))
					{
						writer.Write(this.TextAfterPageIndexBox);
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Type, this.UrlPaging ? "Button" : "Submit");
					writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
					writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "_btn");
					writer.AddAttribute(HtmlTextWriterAttribute.Value, this.SubmitButtonText);
					if (!string.IsNullOrEmpty(this.SubmitButtonClass))
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Class, this.SubmitButtonClass);
					}
					if (!string.IsNullOrEmpty(this.SubmitButtonStyle))
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Style, this.SubmitButtonStyle);
					}
					if (!this.Enabled || (this.PageCount <= 1 && this.AlwaysShow))
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Onclick, this.UrlPaging ? text3 : ("return " + text2));
					writer.RenderBeginTag(HtmlTextWriterTag.Input);
					writer.RenderEndTag();
				}
				else
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Name, text);
					writer.AddAttribute(HtmlTextWriterAttribute.Id, text);
					writer.AddAttribute(HtmlTextWriterAttribute.Onchange, this.UrlPaging ? "ANP_goToPage(this)" : this.Page.ClientScript.GetPostBackEventReference(this, null));
					writer.RenderBeginTag(HtmlTextWriterTag.Select);
					if (this.PageCount > 80)
					{
						if (this.CurrentPageIndex <= 15)
						{
							this.listPageIndexes(writer, 1, 15);
							this.addMoreListItem(writer, 16);
							this.listPageIndexes(writer, this.PageCount - 4, this.PageCount);
						}
						else if (this.CurrentPageIndex >= this.PageCount - 14)
						{
							this.listPageIndexes(writer, 1, 5);
							this.addMoreListItem(writer, this.PageCount - 15);
							this.listPageIndexes(writer, this.PageCount - 14, this.PageCount);
						}
						else
						{
							this.listPageIndexes(writer, 1, 5);
							this.addMoreListItem(writer, this.CurrentPageIndex - 6);
							this.listPageIndexes(writer, this.CurrentPageIndex - 5, this.CurrentPageIndex + 5);
							this.addMoreListItem(writer, this.CurrentPageIndex + 6);
							this.listPageIndexes(writer, this.PageCount - 4, this.PageCount);
						}
					}
					else
					{
						this.listPageIndexes(writer, 1, this.PageCount);
					}
					writer.RenderEndTag();
					if (!string.IsNullOrEmpty(this.TextAfterPageIndexBox))
					{
						writer.Write(this.TextAfterPageIndexBox);
					}
				}
			}
			if (this.ShowCustomInfoSection != ShowCustomInfoSection.Never)
			{
				writer.RenderEndTag();
			}
			if (this.ShowCustomInfoSection == ShowCustomInfoSection.Right)
			{
				this.RenderCustomInfoSection(writer);
			}
		}

		private void addMoreListItem(HtmlTextWriter writer, int pageIndex)
		{
			writer.Write("<option value=\"");
			writer.Write(pageIndex);
			writer.Write("\">......</option>");
		}

		private void listPageIndexes(HtmlTextWriter writer, int startIndex, int endIndex)
		{
			for (int i = startIndex; i <= endIndex; i++)
			{
				writer.Write("<option value=\"");
				writer.Write(i);
				writer.Write("\"");
				if (i == this.CurrentPageIndex)
				{
					writer.Write(" selected=\"true\"");
				}
				writer.Write(">");
				writer.Write(i);
				writer.Write("</option>");
			}
		}

		private void RenderCustomInfoSection(HtmlTextWriter writer)
		{
			if (this.Height != Unit.Empty)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.ToString());
			}
			writer.AddStyleAttribute("float", "left");
			string value = this.CustomInfoSectionWidth.ToString();
			if (this.CustomInfoClass != null && this.CustomInfoClass.Trim().Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CustomInfoClass);
			}
			if (this.CustomInfoStyle != null && this.CustomInfoStyle.Trim().Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Style, this.CustomInfoStyle);
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, value);
			if (this.CustomInfoTextAlign != HorizontalAlign.NotSet)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Align, this.CustomInfoTextAlign.ToString().ToLower());
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Write(this.GetCustomInfoHTML(this.CustomInfoHTML));
			writer.RenderEndTag();
		}

		private string GetHrefString(int pageIndex)
		{
			if (!this.UrlPaging)
			{
				return this.Page.ClientScript.GetPostBackClientHyperlink(this, pageIndex.ToString());
			}
			int num = pageIndex;
			string str = "pi";
			if (this.ReverseUrlPageIndex)
			{
				str = "(" + this.PageCount + "-pi+1)";
				num = ((pageIndex == -1) ? -1 : (this.PageCount - pageIndex + 1));
			}
			if (this.EnableUrlRewriting)
			{
				Regex regex = new Regex("(?<p>%(?<m>[^%]+)%)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				MatchCollection matchCollection = regex.Matches(this.UrlRewritePattern);
				NameValueCollection nameValueCollection = AspNetPager.ConvertQueryStringToCollection(this.queryString);
				string text = this.UrlRewritePattern;
				foreach (Match match in matchCollection)
				{
					string newValue = nameValueCollection[match.Groups["m"].Value];
					text = text.Replace(match.Groups["p"].Value, newValue);
				}
				return base.ResolveUrl(string.Format(text, (num == -1) ? ("\"+" + str + "+\"") : num.ToString()));
			}
			return this.BuildUrlString(this.UrlPageIndexName, (num == -1) ? ("\"+" + str + "+\"") : num.ToString());
		}

		private string GetCustomInfoHTML(string origText)
		{
			if (!string.IsNullOrEmpty(origText) && origText.IndexOf('%') >= 0)
			{
				string[] array = new string[]
				{
					"recordcount",
					"pagecount",
					"currentpageindex",
					"startrecordindex",
					"endrecordindex",
					"pagesize",
					"pagesremain",
					"recordsremain"
				};
				StringBuilder stringBuilder = new StringBuilder(origText);
				Regex regex = new Regex("(?<ph>%(?<pname>\\w{8,})%)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				MatchCollection matchCollection = regex.Matches(origText);
				foreach (Match match in matchCollection)
				{
					string text = match.Groups["pname"].Value.ToLower();
					if (Array.IndexOf<string>(array, text) >= 0)
					{
						string text2 = null;
						string key;
						switch (key = text)
						{
						case "recordcount":
							text2 = this.RecordCount.ToString();
							break;
						case "pagecount":
							text2 = this.PageCount.ToString();
							break;
						case "currentpageindex":
							text2 = this.CurrentPageIndex.ToString();
							break;
						case "startrecordindex":
							text2 = this.StartRecordIndex.ToString();
							break;
						case "endrecordindex":
							text2 = this.EndRecordIndex.ToString();
							break;
						case "pagesize":
							text2 = this.PageSize.ToString();
							break;
						case "pagesremain":
							text2 = this.PagesRemain.ToString();
							break;
						case "recordsremain":
							text2 = this.RecordsRemain.ToString();
							break;
						}
						if (text2 != null)
						{
							stringBuilder.Replace(match.Groups["ph"].Value, text2);
						}
					}
				}
				return stringBuilder.ToString();
			}
			return origText;
		}

		private static NameValueCollection ConvertQueryStringToCollection(string s)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			int num = (s != null) ? s.Length : 0;
			for (int i = 0; i < num; i++)
			{
				int num2 = i;
				int num3 = -1;
				while (i < num)
				{
					char c = s[i];
					if (c == '=')
					{
						if (num3 < 0)
						{
							num3 = i;
						}
					}
					else if (c == '&')
					{
						break;
					}
					i++;
				}
				string name = null;
				string value;
				if (num3 >= 0)
				{
					name = s.Substring(num2, num3 - num2);
					value = s.Substring(num3 + 1, i - num3 - 1);
				}
				else
				{
					value = s.Substring(num2, i - num2);
				}
				nameValueCollection.Add(name, value);
				if (i == num - 1 && s[i] == '&')
				{
					nameValueCollection.Add(null, string.Empty);
				}
			}
			return nameValueCollection;
		}

		private string BuildUrlString(string sk, string sv)
		{
			StringBuilder stringBuilder = new StringBuilder(80);
			bool flag = false;
			int num = (this.queryString != null) ? this.queryString.Length : 0;
			for (int i = 0; i < num; i++)
			{
				int num2 = i;
				int num3 = -1;
				while (i < num)
				{
					char c = this.queryString[i];
					if (c == '=')
					{
						if (num3 < 0)
						{
							num3 = i;
						}
					}
					else if (c == '&')
					{
						break;
					}
					i++;
				}
				string text = null;
				string value;
				if (num3 >= 0)
				{
					text = this.queryString.Substring(num2, num3 - num2);
					value = this.queryString.Substring(num3 + 1, i - num3 - 1);
				}
				else
				{
					value = this.queryString.Substring(num2, i - num2);
				}
				stringBuilder.Append(text).Append("=");
				if (text == sk)
				{
					flag = true;
					stringBuilder.Append(sv);
				}
				else
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Append("&");
			}
			if (!flag)
			{
				stringBuilder.Append(sk).Append("=").Append(sv);
			}
			stringBuilder.Insert(0, "?").Insert(0, Path.GetFileName(this.currentUrl));
			return stringBuilder.ToString().Trim(new char[]
			{
				'&'
			});
		}

		private void CreateNavigationButton(HtmlTextWriter writer, string btnname)
		{
			if (!this.ShowFirstLast && (btnname == "first" || btnname == "last"))
			{
				return;
			}
			if (!this.ShowPrevNext && (btnname == "prev" || btnname == "next"))
			{
				return;
			}
			bool flag = this.PagingButtonType == PagingButtonType.Image && this.NavigationButtonType == PagingButtonType.Image;
			if (btnname == "prev" || btnname == "first")
			{
				bool flag2 = this.CurrentPageIndex <= 1 | !this.Enabled;
				if (!this.ShowDisabledButtons && flag2)
				{
					return;
				}
				int pageIndex = (btnname == "first") ? 1 : (this.CurrentPageIndex - 1);
				this.writeSpacingStyle(writer);
				if (!flag)
				{
					string value = (btnname == "prev") ? this.PrevPageText : this.FirstPageText;
					if (flag2)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
					}
					else
					{
						this.WriteCssClass(writer);
						this.AddToolTip(writer, pageIndex);
						this.AddHyperlinkTarget(writer);
						writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(pageIndex));
					}
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(value);
					writer.RenderEndTag();
					return;
				}
				if (!flag2)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(pageIndex));
					this.AddToolTip(writer, pageIndex);
					this.AddHyperlinkTarget(writer);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImagePath + btnname + this.ButtonImageNameExtension + this.ButtonImageExtension);
					writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
					if (this.ButtonImageAlign != ImageAlign.NotSet)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ButtonImageAlign.ToString());
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Img);
					writer.RenderEndTag();
					writer.RenderEndTag();
					return;
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImagePath + btnname + this.DisabledButtonImageNameExtension + this.ButtonImageExtension);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				if (this.ButtonImageAlign != ImageAlign.NotSet)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ButtonImageAlign.ToString());
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				return;
			}
			else
			{
				bool flag2 = this.CurrentPageIndex >= this.PageCount | !this.Enabled;
				if (!this.ShowDisabledButtons && flag2)
				{
					return;
				}
				int pageIndex = (btnname == "last") ? this.PageCount : (this.CurrentPageIndex + 1);
				this.writeSpacingStyle(writer);
				if (!flag)
				{
					string value = (btnname == "next") ? this.NextPageText : this.LastPageText;
					if (flag2)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
					}
					else
					{
						this.WriteCssClass(writer);
						this.AddToolTip(writer, pageIndex);
						writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(pageIndex));
						this.AddHyperlinkTarget(writer);
					}
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(value);
					writer.RenderEndTag();
					return;
				}
				if (!flag2)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(pageIndex));
					this.AddToolTip(writer, pageIndex);
					this.AddHyperlinkTarget(writer);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImagePath + btnname + this.ButtonImageNameExtension + this.ButtonImageExtension);
					writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
					if (this.ButtonImageAlign != ImageAlign.NotSet)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ButtonImageAlign.ToString());
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Img);
					writer.RenderEndTag();
					writer.RenderEndTag();
					return;
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImagePath + btnname + this.DisabledButtonImageNameExtension + this.ButtonImageExtension);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				if (this.ButtonImageAlign != ImageAlign.NotSet)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ButtonImageAlign.ToString());
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				return;
			}
		}

		private void WriteCssClass(HtmlTextWriter writer)
		{
			if (this.cssClassName != null && this.cssClassName.Trim().Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, this.cssClassName);
			}
		}

		private void AddToolTip(HtmlTextWriter writer, int pageIndex)
		{
			if (this.ShowNavigationToolTip)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Title, string.Format(this.NavigationToolTipTextFormatString, pageIndex));
			}
		}

		private void CreateNumericButton(HtmlTextWriter writer, int index)
		{
			bool flag = index == this.CurrentPageIndex;
			if (this.PagingButtonType == PagingButtonType.Image && this.NumericButtonType == PagingButtonType.Image)
			{
				this.writeSpacingStyle(writer);
				if (!flag)
				{
					if (this.Enabled)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(index));
					}
					this.AddToolTip(writer, index);
					this.AddHyperlinkTarget(writer);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					this.CreateNumericImages(writer, index, flag);
					writer.RenderEndTag();
					return;
				}
				if (!string.IsNullOrEmpty(this.CurrentPageButtonClass))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CurrentPageButtonClass);
				}
				if (!string.IsNullOrEmpty(this.CurrentPageButtonStyle))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Style, this.CurrentPageButtonStyle);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
				this.CreateNumericImages(writer, index, flag);
				writer.RenderEndTag();
				return;
			}
			else
			{
				this.writeSpacingStyle(writer);
				if (flag)
				{
					if (string.IsNullOrEmpty(this.CurrentPageButtonClass) && string.IsNullOrEmpty(this.CurrentPageButtonStyle))
					{
						writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold");
						writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
					}
					else
					{
						if (!string.IsNullOrEmpty(this.CurrentPageButtonClass))
						{
							writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CurrentPageButtonClass);
						}
						if (!string.IsNullOrEmpty(this.CurrentPageButtonStyle))
						{
							writer.AddAttribute(HtmlTextWriterAttribute.Style, this.CurrentPageButtonStyle);
						}
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					if (!string.IsNullOrEmpty(this.CurrentPageButtonTextFormatString))
					{
						writer.Write(string.Format(this.CurrentPageButtonTextFormatString, index));
					}
					else
					{
						writer.Write(index);
					}
					writer.RenderEndTag();
					return;
				}
				if (this.Enabled)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(index));
				}
				this.WriteCssClass(writer);
				this.AddToolTip(writer, index);
				this.AddHyperlinkTarget(writer);
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				if (!string.IsNullOrEmpty(this.NumericButtonTextFormatString))
				{
					writer.Write(string.Format(this.NumericButtonTextFormatString, index));
				}
				else
				{
					writer.Write(index);
				}
				writer.RenderEndTag();
				return;
			}
		}

		private void CreateNumericImages(HtmlTextWriter writer, int index, bool isCurrent)
		{
			string text = index.ToString();
			for (int i = 0; i < text.Length; i++)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, string.Concat(new object[]
				{
					this.ImagePath,
					text[i],
					isCurrent ? this.CpiButtonImageNameExtension : this.ButtonImageNameExtension,
					this.ButtonImageExtension
				}));
				if (this.ButtonImageAlign != ImageAlign.NotSet)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ButtonImageAlign.ToString());
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}
		}

		private void CreateMoreButton(HtmlTextWriter writer, int pageIndex)
		{
			this.writeSpacingStyle(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			this.WriteCssClass(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Href, this.GetHrefString(pageIndex));
			this.AddToolTip(writer, pageIndex);
			this.AddHyperlinkTarget(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			if (this.PagingButtonType == PagingButtonType.Image && this.MoreButtonType == PagingButtonType.Image)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.ImagePath + "more" + this.ButtonImageNameExtension + this.ButtonImageExtension);
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				if (this.ButtonImageAlign != ImageAlign.NotSet)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Align, this.ButtonImageAlign.ToString());
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}
			else
			{
				writer.Write("...");
			}
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		private void writeSpacingStyle(HtmlTextWriter writer)
		{
			if (this.PagingButtonSpacing.Value != 0.0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.MarginRight, this.PagingButtonSpacing.ToString());
			}
		}

		private void AddHyperlinkTarget(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.UrlPagingTarget))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Target, this.UrlPagingTarget);
			}
		}

		public void RaisePostBackEvent(string args)
		{
			int newPageIndex = this.CurrentPageIndex;
			try
			{
				if (args == null || args == "")
				{
					args = this.inputPageIndex;
				}
				newPageIndex = int.Parse(args);
			}
			catch
			{
			}
			PageChangingEventArgs e = new PageChangingEventArgs(newPageIndex);
			if (this.cloneFrom != null)
			{
				this.cloneFrom.OnPageChanging(e);
				return;
			}
			this.OnPageChanging(e);
		}

		public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
		{
			string text = pcol[this.UniqueID + "_input"];
			if (text != null && text.Trim().Length > 0)
			{
				try
				{
					int num = int.Parse(text);
					if (num > 0 && num <= this.PageCount)
					{
						this.inputPageIndex = text;
						this.Page.RegisterRequiresRaiseEvent(this);
					}
				}
				catch
				{
				}
			}
			return false;
		}

		public virtual void RaisePostDataChangedEvent()
		{
		}

		protected virtual void OnPageChanging(PageChangingEventArgs e)
		{
			PageChangingEventHandler pageChangingEventHandler = (PageChangingEventHandler)base.Events[AspNetPager.EventPageChanging];
			if (pageChangingEventHandler != null)
			{
				pageChangingEventHandler(this, e);
				if (!e.Cancel || this.UrlPaging)
				{
					this.CurrentPageIndex = e.NewPageIndex;
					this.OnPageChanged(EventArgs.Empty);
					return;
				}
			}
			else
			{
				this.CurrentPageIndex = e.NewPageIndex;
				this.OnPageChanged(EventArgs.Empty);
			}
		}

		protected virtual void OnPageChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[AspNetPager.EventPageChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}
	}
}
