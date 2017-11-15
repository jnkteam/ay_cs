using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class PageIndexBoxForm : Form
	{
		private ShowPageIndexBox showIndexBox;

		private int threshold;

		private PageIndexBoxType boxType;

		private string textBeforeBox;

		private string textAfterBox;

		private string submitButtonText;

		private IContainer components;

		private Label label1;

		private RadioButton rb_auto;

		private RadioButton rb_never;

		private RadioButton rb_always;

		private Label label2;

		private NumericUpDown num_threshold;

		private Label label3;

		private Label label4;

		private ComboBox cmb_boxtype;

		private Button btn_cancel;

		private Button btn_ok;

		private Label label5;

		private TextBox tb_textbf;

		private Label label6;

		private TextBox tb_textaft;

		private Label label7;

		private TextBox tb_btntxt;

		public ShowPageIndexBox ShowIndexBox
		{
			get
			{
				return this.showIndexBox;
			}
			set
			{
				this.showIndexBox = value;
			}
		}

		public int Threshold
		{
			get
			{
				return this.threshold;
			}
			set
			{
				this.threshold = value;
			}
		}

		public PageIndexBoxType BoxType
		{
			get
			{
				return this.boxType;
			}
			set
			{
				this.boxType = value;
			}
		}

		public string TextBeforeBox
		{
			get
			{
				return this.textBeforeBox;
			}
			set
			{
				this.textBeforeBox = value;
			}
		}

		public string TextAfterBox
		{
			get
			{
				return this.textAfterBox;
			}
			set
			{
				this.textAfterBox = value;
			}
		}

		public string SubmitButtonText
		{
			get
			{
				return this.submitButtonText;
			}
			set
			{
				this.submitButtonText = value;
			}
		}

		public PageIndexBoxForm(ShowPageIndexBox showBox, int threshold, PageIndexBoxType boxType, string txtBefore, string txtAfter, string btnText)
		{
			this.InitializeComponent();
			this.showIndexBox = showBox;
			this.threshold = threshold;
			this.boxType = boxType;
			this.textBeforeBox = txtBefore;
			this.textAfterBox = txtAfter;
			this.submitButtonText = btnText;
		}

		private void PageIndexBoxForm_Load(object sender, EventArgs e)
		{
			this.num_threshold.Value = this.threshold;
			this.cmb_boxtype.SelectedIndex = ((this.boxType == PageIndexBoxType.DropDownList) ? 1 : 0);
			switch (this.showIndexBox)
			{
			case ShowPageIndexBox.Never:
				this.rb_never.Checked = true;
				this.tb_btntxt.Enabled = (this.tb_textaft.Enabled = (this.tb_textbf.Enabled = (this.num_threshold.Enabled = (this.cmb_boxtype.Enabled = false))));
				return;
			case ShowPageIndexBox.Always:
				this.rb_always.Checked = true;
				this.num_threshold.Enabled = false;
				return;
			}
			this.rb_auto.Checked = true;
		}

		private void RblCheckedChanged(object sender, EventArgs e)
		{
			if (this.rb_never.Checked)
			{
				this.tb_btntxt.Enabled = (this.tb_textaft.Enabled = (this.tb_textbf.Enabled = (this.num_threshold.Enabled = (this.cmb_boxtype.Enabled = false))));
				return;
			}
			if (this.rb_always.Checked)
			{
				this.num_threshold.Enabled = false;
				this.cmb_boxtype.Enabled = (this.tb_textaft.Enabled = (this.tb_textbf.Enabled = true));
				this.tb_btntxt.Enabled = (this.cmb_boxtype.SelectedIndex == 0);
				return;
			}
			this.tb_textaft.Enabled = (this.tb_textbf.Enabled = (this.num_threshold.Enabled = (this.cmb_boxtype.Enabled = true)));
			this.tb_btntxt.Enabled = (this.cmb_boxtype.SelectedIndex == 0);
		}

		private void btn_ok_Click(object sender, EventArgs e)
		{
			if (this.rb_never.Checked)
			{
				this.showIndexBox = ShowPageIndexBox.Never;
				return;
			}
			if (this.rb_always.Checked)
			{
				this.showIndexBox = ShowPageIndexBox.Always;
				this.boxType = (PageIndexBoxType)this.cmb_boxtype.SelectedIndex;
				this.textAfterBox = this.tb_textaft.Text;
				this.textBeforeBox = this.tb_textbf.Text;
				this.submitButtonText = this.tb_btntxt.Text;
				return;
			}
			this.showIndexBox = ShowPageIndexBox.Always;
			this.threshold = (int)this.num_threshold.Value;
			this.boxType = (PageIndexBoxType)this.cmb_boxtype.SelectedIndex;
			this.textAfterBox = this.tb_textaft.Text;
			this.textBeforeBox = this.tb_textbf.Text;
			this.submitButtonText = this.tb_btntxt.Text;
		}

		private void cmb_boxtype_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.tb_btntxt.Enabled = (this.cmb_boxtype.SelectedIndex == 0);
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
			this.label1 = new Label();
			this.rb_auto = new RadioButton();
			this.rb_never = new RadioButton();
			this.rb_always = new RadioButton();
			this.label2 = new Label();
			this.num_threshold = new NumericUpDown();
			this.label3 = new Label();
			this.label4 = new Label();
			this.cmb_boxtype = new ComboBox();
			this.btn_cancel = new Button();
			this.btn_ok = new Button();
			this.label5 = new Label();
			this.tb_textbf = new TextBox();
			this.label6 = new Label();
			this.tb_textaft = new TextBox();
			this.label7 = new Label();
			this.tb_btntxt = new TextBox();
			((ISupportInitialize)this.num_threshold).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(20, 25);
			this.label1.Name = "label1";
			this.label1.Size = new Size(175, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "页索引文本或下拉框显示方式：";
			this.rb_auto.AutoSize = true;
			this.rb_auto.Location = new Point(23, 53);
			this.rb_auto.Name = "rb_auto";
			this.rb_auto.Size = new Size(49, 17);
			this.rb_auto.TabIndex = 1;
			this.rb_auto.TabStop = true;
			this.rb_auto.Text = "自动";
			this.rb_auto.UseVisualStyleBackColor = true;
			this.rb_auto.CheckedChanged += new EventHandler(this.RblCheckedChanged);
			this.rb_never.AutoSize = true;
			this.rb_never.Location = new Point(78, 53);
			this.rb_never.Name = "rb_never";
			this.rb_never.Size = new Size(73, 17);
			this.rb_never.TabIndex = 2;
			this.rb_never.TabStop = true;
			this.rb_never.Text = "从不显示";
			this.rb_never.UseVisualStyleBackColor = true;
			this.rb_never.CheckedChanged += new EventHandler(this.RblCheckedChanged);
			this.rb_always.AutoSize = true;
			this.rb_always.Location = new Point(157, 53);
			this.rb_always.Name = "rb_always";
			this.rb_always.Size = new Size(73, 17);
			this.rb_always.TabIndex = 3;
			this.rb_always.TabStop = true;
			this.rb_always.Text = "总是显示";
			this.rb_always.UseVisualStyleBackColor = true;
			this.rb_always.CheckedChanged += new EventHandler(this.RblCheckedChanged);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(20, 88);
			this.label2.Name = "label2";
			this.label2.Size = new Size(67, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "总页数超过";
			this.num_threshold.Location = new Point(90, 86);
			this.num_threshold.Name = "num_threshold";
			this.num_threshold.Size = new Size(42, 20);
			this.num_threshold.TabIndex = 5;
			NumericUpDown arg_39B_0 = this.num_threshold;
			int[] array = new int[4];
			array[0] = 30;
			arg_39B_0.Value = new decimal(array);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(135, 88);
			this.label3.Name = "label3";
			this.label3.Size = new Size(127, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "时，自动显示页索引框";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(20, 122);
			this.label4.Name = "label4";
			this.label4.Size = new Size(115, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "页索引框显示类型：";
			this.cmb_boxtype.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmb_boxtype.FormattingEnabled = true;
			this.cmb_boxtype.Items.AddRange(new object[]
			{
				"文本输入框",
				"下拉列表框"
			});
			this.cmb_boxtype.Location = new Point(135, 119);
			this.cmb_boxtype.Name = "cmb_boxtype";
			this.cmb_boxtype.Size = new Size(126, 21);
			this.cmb_boxtype.TabIndex = 8;
			this.cmb_boxtype.SelectedIndexChanged += new EventHandler(this.cmb_boxtype_SelectedIndexChanged);
			this.btn_cancel.DialogResult = DialogResult.Cancel;
			this.btn_cancel.Location = new Point(208, 247);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new Size(75, 23);
			this.btn_cancel.TabIndex = 9;
			this.btn_cancel.Text = "取消(&C)";
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_ok.DialogResult = DialogResult.OK;
			this.btn_ok.Location = new Point(118, 247);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new Size(75, 23);
			this.btn_ok.TabIndex = 10;
			this.btn_ok.Text = "确定(&O)";
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new EventHandler(this.btn_ok_Click);
			this.label5.AutoSize = true;
			this.label5.Location = new Point(20, 152);
			this.label5.Name = "label5";
			this.label5.Size = new Size(115, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "页索引框前的文本：";
			this.tb_textbf.Location = new Point(135, 149);
			this.tb_textbf.Name = "tb_textbf";
			this.tb_textbf.Size = new Size(127, 20);
			this.tb_textbf.TabIndex = 12;
			this.tb_textbf.Text = "转到";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(20, 184);
			this.label6.Name = "label6";
			this.label6.Size = new Size(115, 13);
			this.label6.TabIndex = 13;
			this.label6.Text = "页索引框后的文本：";
			this.tb_textaft.Location = new Point(135, 181);
			this.tb_textaft.Name = "tb_textaft";
			this.tb_textaft.Size = new Size(127, 20);
			this.tb_textaft.TabIndex = 14;
			this.tb_textaft.Text = "页";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(23, 210);
			this.label7.Name = "label7";
			this.label7.Size = new Size(115, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "提交按钮上的文本：";
			this.tb_btntxt.Location = new Point(135, 210);
			this.tb_btntxt.Name = "tb_btntxt";
			this.tb_btntxt.Size = new Size(126, 20);
			this.tb_btntxt.TabIndex = 16;
			this.tb_btntxt.Text = "Go";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(308, 279);
			base.ControlBox = false;
			base.Controls.Add(this.tb_btntxt);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.tb_textaft);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.tb_textbf);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.btn_ok);
			base.Controls.Add(this.btn_cancel);
			base.Controls.Add(this.cmb_boxtype);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.num_threshold);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.rb_always);
			base.Controls.Add(this.rb_never);
			base.Controls.Add(this.rb_auto);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "PageIndexBoxForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "设置页索引文本或下拉框";
			base.Load += new EventHandler(this.PageIndexBoxForm_Load);
			((ISupportInitialize)this.num_threshold).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
