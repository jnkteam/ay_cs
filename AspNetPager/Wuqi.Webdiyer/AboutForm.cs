using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Wuqi.Webdiyer.Properties;

namespace Wuqi.Webdiyer
{
	public class AboutForm : Form
	{
		private IContainer components;

		private Label label1;

		private Label label2;

		private LinkLabel linkLabel1;

		private LinkLabel linkLabel2;

		private Button button1;

		private Label label3;

		private PictureBox pictureBox1;

		public AboutForm()
		{
			this.InitializeComponent();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.webdiyer.com");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.dotneturls.com/gb2312");
		}

		private void AboutForm_Load(object sender, EventArgs e)
		{
			this.label1.Text = "控件版本：" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
			this.linkLabel1 = new LinkLabel();
			this.linkLabel2 = new LinkLabel();
			this.button1 = new Button();
			this.label3 = new Label();
			this.pictureBox1 = new PictureBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(17, 61);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0, 13);
			this.label1.TabIndex = 0;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(14, 91);
			this.label2.Name = "label2";
			this.label2.Size = new Size(142, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "© 陕西省吴起县 Webdiyer ";
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new Point(16, 144);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new Size(118, 13);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "访问AspNetPager主页";
			this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new Point(149, 144);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new Size(102, 13);
			this.linkLabel2.TabIndex = 3;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "更多.Net 学习资源";
			this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			this.button1.DialogResult = DialogResult.OK;
			this.button1.Location = new Point(207, 176);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "确定(&O)";
			this.button1.UseVisualStyleBackColor = true;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(14, 114);
			this.label3.Name = "label3";
			this.label3.Size = new Size(79, 13);
			this.label3.TabIndex = 5;
			this.label3.Tag = "";
			this.label3.Text = "保留所有权利";
			this.pictureBox1.Dock = DockStyle.Top;
			this.pictureBox1.Image = Resources.aspnetpager;
			this.pictureBox1.Location = new Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(291, 45);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(291, 214);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.linkLabel2);
			base.Controls.Add(this.linkLabel1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "关于AspNetPager";
			base.Load += new EventHandler(this.AboutForm_Load);
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
