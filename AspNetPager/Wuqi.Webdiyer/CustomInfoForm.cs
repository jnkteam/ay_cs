using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class CustomInfoForm : Form
	{
		private string customInfoHtml;

		private ShowCustomInfoSection showCustomSection;

		private IContainer components;

		private ListBox listBox1;

		private Label label1;

		private TextBox tb_propvalue;

		private Label label2;

		private Label lbl_preview;

		private Label label3;

		private Button btn_cancel;

		private Button btn_ok;

		private Label label4;

		private RadioButton rb_right;

		private RadioButton rb_left;

		private RadioButton rb_never;

		public ShowCustomInfoSection ShowCustomSection
		{
			get
			{
				return this.showCustomSection;
			}
			set
			{
				this.showCustomSection = value;
			}
		}

		public string CustomInfoHtml
		{
			get
			{
				return this.customInfoHtml;
			}
			set
			{
				this.customInfoHtml = value;
			}
		}

		public CustomInfoForm(ShowCustomInfoSection showSection, string customInfo)
		{
			this.InitializeComponent();
			this.showCustomSection = showSection;
			this.CustomInfoHtml = customInfo;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.listBox1.SelectedIndex)
			{
			case 0:
				this.tb_propvalue.Text = "Page %CurrentPageIndex% of %PageCount%";
				this.lbl_preview.Text = "Page 1 of 23";
				return;
			case 1:
				this.tb_propvalue.Text = "共%PageCount%页，当前为第%CurrentPageIndex%页";
				this.lbl_preview.Text = "共23页，当前为第1页";
				return;
			case 2:
				this.tb_propvalue.Text = "共%PageCount%页，当前为第%CurrentPageIndex%页，每页%PageSize%条";
				this.lbl_preview.Text = "共23页，当前为第1页，每页10条";
				return;
			case 3:
				this.tb_propvalue.Text = "第%CurrentPageIndex%页，共%PageCount%页，每页%PageSize%条";
				this.lbl_preview.Text = "第1页，共23页，每页10条";
				return;
			default:
				return;
			}
		}

		private void btn_ok_Click(object sender, EventArgs e)
		{
			this.customInfoHtml = this.tb_propvalue.Text;
			if (this.rb_never.Checked)
			{
				this.showCustomSection = ShowCustomInfoSection.Never;
				return;
			}
			if (this.rb_left.Checked)
			{
				this.showCustomSection = ShowCustomInfoSection.Left;
				return;
			}
			this.showCustomSection = ShowCustomInfoSection.Right;
		}

		private void CustomInfoForm_Load(object sender, EventArgs e)
		{
			switch (this.showCustomSection)
			{
			case ShowCustomInfoSection.Never:
				this.listBox1.Enabled = (this.tb_propvalue.Enabled = false);
				this.rb_never.Checked = true;
				break;
			case ShowCustomInfoSection.Left:
				this.listBox1.Enabled = (this.tb_propvalue.Enabled = true);
				this.rb_left.Checked = true;
				break;
			default:
				this.listBox1.Enabled = (this.tb_propvalue.Enabled = true);
				this.rb_right.Checked = true;
				break;
			}
			this.tb_propvalue.Text = this.customInfoHtml;
		}

		private void CustomInfoChanged(object sender, EventArgs e)
		{
			this.listBox1.Enabled = (this.tb_propvalue.Enabled = !this.rb_never.Checked);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.listBox1 = new ListBox();
			this.label1 = new Label();
			this.tb_propvalue = new TextBox();
			this.label2 = new Label();
			this.lbl_preview = new Label();
			this.label3 = new Label();
			this.btn_cancel = new Button();
			this.btn_ok = new Button();
			this.label4 = new Label();
			this.rb_right = new RadioButton();
			this.rb_left = new RadioButton();
			this.rb_never = new RadioButton();
			base.SuspendLayout();
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Items.AddRange(new object[]
			{
				"默认设置（Page N of M）",
				"总页数及当前页",
				"总页数、当前页及每页记录数",
				"当前页、总页数及每页记录数"
			});
			this.listBox1.Location = new Point(25, 117);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new Size(299, 69);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(25, 196);
			this.label1.Name = "label1";
			this.label1.Size = new Size(55, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "属性值：";
			this.tb_propvalue.Location = new Point(25, 212);
			this.tb_propvalue.Multiline = true;
			this.tb_propvalue.Name = "tb_propvalue";
			this.tb_propvalue.Size = new Size(299, 61);
			this.tb_propvalue.TabIndex = 2;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(22, 279);
			this.label2.Name = "label2";
			this.label2.Size = new Size(91, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "显示效果预览：";
			this.lbl_preview.AutoSize = true;
			this.lbl_preview.ForeColor = Color.Blue;
			this.lbl_preview.Location = new Point(25, 301);
			this.lbl_preview.Name = "lbl_preview";
			this.lbl_preview.Size = new Size(0, 13);
			this.lbl_preview.TabIndex = 4;
			this.label3.Location = new Point(22, 72);
			this.label3.Name = "label3";
			this.label3.Size = new Size(302, 42);
			this.label3.TabIndex = 5;
			this.label3.Text = "请从下面预定义的内容模板中选择自定义信息，或者根据需要手工修改属性值文本框中的属性值：";
			this.btn_cancel.DialogResult = DialogResult.Cancel;
			this.btn_cancel.Location = new Point(270, 336);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new Size(75, 23);
			this.btn_cancel.TabIndex = 6;
			this.btn_cancel.Text = "取消(&C)";
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_ok.DialogResult = DialogResult.OK;
			this.btn_ok.Location = new Point(185, 336);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new Size(75, 23);
			this.btn_ok.TabIndex = 7;
			this.btn_ok.Text = "确定(&O)";
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new EventHandler(this.btn_ok_Click);
			this.label4.AutoSize = true;
			this.label4.Location = new Point(22, 13);
			this.label4.Name = "label4";
			this.label4.Size = new Size(199, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "自定义信息区显示在分页导航栏的：";
			this.rb_right.AutoSize = true;
			this.rb_right.Location = new Point(172, 42);
			this.rb_right.Name = "rb_right";
			this.rb_right.Size = new Size(49, 17);
			this.rb_right.TabIndex = 9;
			this.rb_right.TabStop = true;
			this.rb_right.Text = "右边";
			this.rb_right.UseVisualStyleBackColor = true;
			this.rb_right.CheckedChanged += new EventHandler(this.CustomInfoChanged);
			this.rb_left.AutoSize = true;
			this.rb_left.Location = new Point(107, 42);
			this.rb_left.Name = "rb_left";
			this.rb_left.Size = new Size(49, 17);
			this.rb_left.TabIndex = 10;
			this.rb_left.TabStop = true;
			this.rb_left.Text = "左边";
			this.rb_left.UseVisualStyleBackColor = true;
			this.rb_left.CheckedChanged += new EventHandler(this.CustomInfoChanged);
			this.rb_never.AutoSize = true;
			this.rb_never.Location = new Point(28, 42);
			this.rb_never.Name = "rb_never";
			this.rb_never.Size = new Size(73, 17);
			this.rb_never.TabIndex = 11;
			this.rb_never.TabStop = true;
			this.rb_never.Text = "从不显示";
			this.rb_never.UseVisualStyleBackColor = true;
			this.rb_never.CheckedChanged += new EventHandler(this.CustomInfoChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(358, 369);
			base.ControlBox = false;
			base.Controls.Add(this.rb_never);
			base.Controls.Add(this.rb_left);
			base.Controls.Add(this.rb_right);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.btn_ok);
			base.Controls.Add(this.btn_cancel);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.lbl_preview);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.tb_propvalue);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.listBox1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "CustomInfoForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "设置自定义信息区显示内容";
			base.Load += new EventHandler(this.CustomInfoForm_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
