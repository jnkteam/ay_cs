using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class SPHelpForm : Form
	{
		private IContainer components;

		private TextBox textBox1;

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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SPHelpForm));
			this.textBox1 = new TextBox();
			base.SuspendLayout();
			this.textBox1.BackColor = Color.White;
			this.textBox1.Dock = DockStyle.Fill;
			this.textBox1.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBox1.Location = new Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = ScrollBars.Vertical;
			this.textBox1.Size = new Size(468, 268);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = componentResourceManager.GetString("textBox1.Text");
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.ClientSize = new Size(468, 268);
			base.Controls.Add(this.textBox1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SPHelpForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "分页存储过程使用说明";
			base.Load += new EventHandler(this.SPHelpForm_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public SPHelpForm()
		{
			this.InitializeComponent();
		}

		private void SPHelpForm_Load(object sender, EventArgs e)
		{
			this.textBox1.Select(0, 0);
		}
	}
}
