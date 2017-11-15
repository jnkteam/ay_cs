using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class SPVarRegForm : Form
	{
		private string sqlVariables;

		private IContainer components;

		private Label label1;

		private Label label2;

		private Label label3;

		private Button btn_add;

		private TextBox tb_vname;

		private TextBox tb_size;

		private ComboBox cmb_dtype;

		private Button btn_rem;

		private ListBox listBox1;

		private Button btn_ok;

		private Button btn_cancel;

		private GroupBox groupBox1;

		public string SqlVariables
		{
			get
			{
				return this.sqlVariables;
			}
			set
			{
				this.sqlVariables = value;
			}
		}

		public SPVarRegForm(string sqlvars)
		{
			this.InitializeComponent();
			this.sqlVariables = sqlvars;
		}

		private void btn_add_Click(object sender, EventArgs e)
		{
			if (this.tb_vname.Text.Trim().Length == 0)
			{
				MessageBox.Show("变量名称不能为空");
				return;
			}
			string text = this.tb_vname.Text.Trim().Trim(new char[]
			{
				'@'
			});
			string text2 = this.cmb_dtype.SelectedItem.ToString();
			string text3 = this.tb_size.Text.Trim();
			if (text3.Length > 0)
			{
				text3 = "(" + text3 + ")";
			}
			this.listBox1.Items.Add(string.Concat(new string[]
			{
				"@",
				text,
				"  ",
				text2,
				text3
			}));
		}

		private void SPVarRegForm_Load(object sender, EventArgs e)
		{
			this.cmb_dtype.DataSource = Enum.GetNames(typeof(SqlDbType));
			this.cmb_dtype.SelectedItem = "Int";
			if (this.sqlVariables.Length > 0)
			{
				string[] array = this.sqlVariables.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string item = array2[i];
					this.listBox1.Items.Add(item);
				}
			}
		}

		private void btn_rem_Click(object sender, EventArgs e)
		{
			this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
		}

		private void btn_ok_Click(object sender, EventArgs e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.listBox1.Items)
			{
				stringBuilder.Append(value).Append(",");
			}
			this.sqlVariables = stringBuilder.ToString().Trim(new char[]
			{
				','
			});
		}

		private void cmb_dtype_SelectedIndexChanged(object sender, EventArgs e)
		{
			string a = this.cmb_dtype.SelectedItem.ToString().ToLower();
			if (a == "int" || a == "bit" || a == "tinyint" || a == "smallint" || a == "bitint" || a == "datetime" || a == "smalldatetime")
			{
				this.tb_size.Text = "";
				this.tb_size.Enabled = false;
				return;
			}
			this.tb_size.Enabled = true;
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
			this.btn_add = new Button();
			this.tb_vname = new TextBox();
			this.tb_size = new TextBox();
			this.cmb_dtype = new ComboBox();
			this.btn_rem = new Button();
			this.listBox1 = new ListBox();
			this.btn_ok = new Button();
			this.btn_cancel = new Button();
			this.groupBox1 = new GroupBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 26);
			this.label1.Name = "label1";
			this.label1.Size = new Size(67, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "变量名称：";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 59);
			this.label2.Name = "label2";
			this.label2.Size = new Size(67, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "数据类型：";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(15, 94);
			this.label3.Name = "label3";
			this.label3.Size = new Size(67, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "变量宽度：";
			this.btn_add.Location = new Point(212, 39);
			this.btn_add.Name = "btn_add";
			this.btn_add.Size = new Size(36, 23);
			this.btn_add.TabIndex = 3;
			this.btn_add.Text = ">>";
			this.btn_add.UseVisualStyleBackColor = true;
			this.btn_add.Click += new EventHandler(this.btn_add_Click);
			this.tb_vname.Location = new Point(86, 26);
			this.tb_vname.Name = "tb_vname";
			this.tb_vname.Size = new Size(114, 20);
			this.tb_vname.TabIndex = 4;
			this.tb_size.Location = new Point(86, 94);
			this.tb_size.Name = "tb_size";
			this.tb_size.Size = new Size(114, 20);
			this.tb_size.TabIndex = 5;
			this.cmb_dtype.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmb_dtype.FormattingEnabled = true;
			this.cmb_dtype.Location = new Point(86, 59);
			this.cmb_dtype.Name = "cmb_dtype";
			this.cmb_dtype.Size = new Size(114, 21);
			this.cmb_dtype.TabIndex = 6;
			this.cmb_dtype.SelectedIndexChanged += new EventHandler(this.cmb_dtype_SelectedIndexChanged);
			this.btn_rem.Location = new Point(212, 84);
			this.btn_rem.Name = "btn_rem";
			this.btn_rem.Size = new Size(36, 23);
			this.btn_rem.TabIndex = 7;
			this.btn_rem.Text = "<<";
			this.btn_rem.UseVisualStyleBackColor = true;
			this.btn_rem.Click += new EventHandler(this.btn_rem_Click);
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new Point(258, 26);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new Size(178, 95);
			this.listBox1.TabIndex = 8;
			this.btn_ok.DialogResult = DialogResult.OK;
			this.btn_ok.Location = new Point(137, 154);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new Size(75, 23);
			this.btn_ok.TabIndex = 9;
			this.btn_ok.Text = "确定(&O)";
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new EventHandler(this.btn_ok_Click);
			this.btn_cancel.DialogResult = DialogResult.Cancel;
			this.btn_cancel.Location = new Point(228, 154);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new Size(75, 23);
			this.btn_cancel.TabIndex = 10;
			this.btn_cancel.Text = "取消(&C)";
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.groupBox1.Controls.Add(this.tb_vname);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.listBox1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.btn_rem);
			this.groupBox1.Controls.Add(this.btn_add);
			this.groupBox1.Controls.Add(this.cmb_dtype);
			this.groupBox1.Controls.Add(this.tb_size);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(442, 136);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "注册变量";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(467, 186);
			base.ControlBox = false;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btn_cancel);
			base.Controls.Add(this.btn_ok);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "SPVarRegForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "注册存储过程变量";
			base.Load += new EventHandler(this.SPVarRegForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
