using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class NavTextForm : Form
	{
		private string firstPageText;

		private string lastPageText;

		private string prevPageText;

		private string nextPageText;

		private IContainer components;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private TextBox tb_first;

		private TextBox tb_prev;

		private TextBox tb_last;

		private TextBox tb_next;

		private Label label5;

		private ListBox listBox1;

		private Button btn_ok;

		private Button btn_cancel;

		public string FirstPageText
		{
			get
			{
				return this.firstPageText;
			}
			set
			{
				this.firstPageText = value;
			}
		}

		public string LastPageText
		{
			get
			{
				return this.lastPageText;
			}
			set
			{
				this.lastPageText = value;
			}
		}

		public string PrevPageText
		{
			get
			{
				return this.prevPageText;
			}
			set
			{
				this.prevPageText = value;
			}
		}

		public string NextPageText
		{
			get
			{
				return this.nextPageText;
			}
			set
			{
				this.nextPageText = value;
			}
		}

		public NavTextForm(string firstTest, string lastText, string prevText, string nextText)
		{
			this.InitializeComponent();
			this.firstPageText = firstTest;
			this.lastPageText = lastText;
			this.prevPageText = prevText;
			this.nextPageText = nextText;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.listBox1.SelectedIndex)
			{
			case 1:
				this.tb_first.Text = "首页";
				this.tb_last.Text = "尾页";
				this.tb_prev.Text = "前页";
				this.tb_next.Text = "后页";
				return;
			case 2:
				this.tb_first.Text = "首页";
				this.tb_last.Text = "尾页";
				this.tb_prev.Text = "上一页";
				this.tb_next.Text = "下一页";
				return;
			case 3:
				this.tb_first.Text = "First";
				this.tb_last.Text = "Last";
				this.tb_prev.Text = "Prev";
				this.tb_next.Text = "Next";
				return;
			default:
				this.tb_first.Text = "&lt;&lt;";
				this.tb_last.Text = "&gt;&gt;";
				this.tb_prev.Text = "&lt;";
				this.tb_next.Text = "&gt;";
				return;
			}
		}

		private void btn_ok_Click(object sender, EventArgs e)
		{
			this.firstPageText = this.tb_first.Text;
			this.lastPageText = this.tb_last.Text;
			this.prevPageText = this.tb_prev.Text;
			this.nextPageText = this.tb_next.Text;
		}

		private void NavTextForm_Load(object sender, EventArgs e)
		{
			this.tb_first.Text = this.firstPageText;
			this.tb_last.Text = this.lastPageText;
			this.tb_prev.Text = this.prevPageText;
			this.tb_next.Text = this.nextPageText;
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
			this.label2 = new Label();
			this.label3 = new Label();
			this.label4 = new Label();
			this.tb_first = new TextBox();
			this.tb_prev = new TextBox();
			this.tb_last = new TextBox();
			this.tb_next = new TextBox();
			this.label5 = new Label();
			this.listBox1 = new ListBox();
			this.btn_ok = new Button();
			this.btn_cancel = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(22, 184);
			this.label1.Name = "label1";
			this.label1.Size = new Size(91, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "首页按钮文本：";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(198, 187);
			this.label2.Name = "label2";
			this.label2.Size = new Size(91, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "尾页按钮文本：";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(22, 218);
			this.label3.Name = "label3";
			this.label3.Size = new Size(91, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "上页按钮文本：";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(198, 221);
			this.label4.Name = "label4";
			this.label4.Size = new Size(91, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "下页按钮文本：";
			this.tb_first.Location = new Point(110, 184);
			this.tb_first.Name = "tb_first";
			this.tb_first.Size = new Size(80, 20);
			this.tb_first.TabIndex = 4;
			this.tb_prev.Location = new Point(110, 215);
			this.tb_prev.Name = "tb_prev";
			this.tb_prev.Size = new Size(80, 20);
			this.tb_prev.TabIndex = 5;
			this.tb_last.Location = new Point(284, 187);
			this.tb_last.Name = "tb_last";
			this.tb_last.Size = new Size(82, 20);
			this.tb_last.TabIndex = 6;
			this.tb_next.Location = new Point(284, 218);
			this.tb_next.Name = "tb_next";
			this.tb_next.Size = new Size(82, 20);
			this.tb_next.TabIndex = 7;
			this.label5.Location = new Point(22, 13);
			this.label5.Name = "label5";
			this.label5.Size = new Size(341, 32);
			this.label5.TabIndex = 8;
			this.label5.Text = "请从下面的列表中选择预定义的导航按钮文本样式，或根据需要手工输入或修改文本框中相应的属性值：";
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Items.AddRange(new object[]
			{
				"默认（<<  < ... >  >>）",
				"首页  前页 ... 后页  尾页",
				"首页  上一页 ... 下一页  尾页",
				"First  Prev ...  Next  Last"
			});
			this.listBox1.Location = new Point(25, 48);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new Size(338, 121);
			this.listBox1.TabIndex = 9;
			this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
			this.btn_ok.DialogResult = DialogResult.OK;
			this.btn_ok.Location = new Point(201, 251);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new Size(75, 23);
			this.btn_ok.TabIndex = 10;
			this.btn_ok.Text = "确定(&O)";
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new EventHandler(this.btn_ok_Click);
			this.btn_cancel.DialogResult = DialogResult.Cancel;
			this.btn_cancel.Location = new Point(291, 251);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new Size(75, 23);
			this.btn_cancel.TabIndex = 11;
			this.btn_cancel.Text = "取消(&C)";
			this.btn_cancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(396, 286);
			base.ControlBox = false;
			base.Controls.Add(this.btn_cancel);
			base.Controls.Add(this.btn_ok);
			base.Controls.Add(this.listBox1);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.tb_next);
			base.Controls.Add(this.tb_last);
			base.Controls.Add(this.tb_prev);
			base.Controls.Add(this.tb_first);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "NavTextForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "设置导航按钮文本";
			base.Load += new EventHandler(this.NavTextForm_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
