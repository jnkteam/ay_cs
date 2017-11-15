using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wuqi.Webdiyer
{
	public class StoredProcForm : Form
	{
		private IContainer components;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		private Label label6;

		private Label label7;

		private Label label9;

		private TextBox tb_spname;

		private TextBox tb_tblname;

		private TextBox tb_idname;

		private TextBox tb_ofldname;

		private RadioButton rb_asc;

		private RadioButton rb_desc;

		private TextBox tb_fields;

		private TextBox tb_vars;

		private TextBox tb_where;

		private CheckBox ckb_rconly;

		private Label label10;

		private ComboBox cmb_sqlver;

		private Button btn_gensp;

		private Button btn_close;

		private Button btn_regvar;

		private GroupBox groupBox1;

		private ErrorProvider errorProvider1;

		private Button btn_help;

		public StoredProcForm()
		{
			this.InitializeComponent();
		}

		private void StoredProcForm_Load(object sender, EventArgs e)
		{
			this.cmb_sqlver.SelectedIndex = 1;
		}

		private void btn_gensp_Click(object sender, EventArgs e)
		{
			if (this.ValidateChildren(ValidationConstraints.Enabled))
			{
				string newValue = this.tb_spname.Text.Trim();
				string text = this.tb_tblname.Text.Trim();
				string newValue2 = this.tb_idname.Text.Trim();
				string newValue3 = this.tb_ofldname.Text.Trim();
				string text2 = this.tb_fields.Text.Trim(new char[]
				{
					',',
					' '
				});
				if (text2.Length > 1)
				{
					StringBuilder stringBuilder = new StringBuilder();
					string[] array = text2.Split(new char[]
					{
						','
					});
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string value = array2[i];
						stringBuilder.Append("O.").Append(value).Append(",");
					}
					text2 = stringBuilder.ToString().Trim(new char[]
					{
						',',
						' '
					});
				}
				string text3 = this.tb_vars.Text.Trim();
				if (text3.Length > 0)
				{
					text3 = text3.Replace(",", ",\n");
				}
				string text4 = this.tb_where.Text.Trim().ToLower();
				if (text4.Length > 0)
				{
					if (!text4.StartsWith("where"))
					{
						text4 = " where " + text4;
					}
					else
					{
						text4 = " " + text4;
					}
				}
				string newValue4 = this.rb_asc.Checked ? "asc" : "desc";
				bool flag = !this.ckb_rconly.Checked;
				bool flag2 = this.cmb_sqlver.SelectedIndex == 1;
				string newValue5 = string.Empty;
				string newValue6 = string.Empty;
				if (flag)
				{
					newValue5 = ",\n@docount bit";
					newValue6 = "\nif(@docount=1)\nselect count(*) from " + text + text4 + "\nelse";
				}
				StringBuilder stringBuilder2;
				if (flag2)
				{
					stringBuilder2 = new StringBuilder("create procedure %spname% %newline%(%sqlvariables%@startIndex int,%newline%@endIndex int%docountdec%)%newline%as%newline%%docountclause%%newline%begin%newline% with temptbl as (%newline%SELECT ROW_NUMBER() OVER (ORDER BY %orderfld% %orderdir%)AS Row, %fieldlist% from %tblname% %where%)%newline% SELECT * FROM temptbl where Row between @startIndex and @endIndex%newline%end");
				}
				else
				{
					stringBuilder2 = new StringBuilder("create procedure %spname% %newline%(%sqlvariables%@startIndex int,%newline%@endIndex int%docountdec%)%newline%as%newline%set nocount on%docountclause%%newline%begin%newline%declare @indextable table(id int identity(1,1),nid int)%newline%set rowcount @endIndex%newline%insert into @indextable(nid) select %pkfield% from %tblname% %where% order by %orderfld% %orderdir%%newline%select %fieldlist% from %tblname% O,@indextable t where O.%pkfield%=t.nid%newline%and t.id between @startIndex and @endIndex order by t.id%newline%end%newline%set nocount off%newline%");
				}
				stringBuilder2.Replace("%spname%", newValue).Replace("%sqlvariables%", text3).Replace("%orderfld%", newValue3).Replace("%orderdir%", newValue4);
				stringBuilder2.Replace("%tblname%", text).Replace("%docountdec%", newValue5).Replace("%docountclause%", newValue6);
				stringBuilder2.Replace("%where%", text4).Replace("%fieldlist%", text2).Replace("%pkfield%", newValue2).Replace("%newline%", "\n");
				Clipboard.SetText(stringBuilder2.ToString());
				MessageBox.Show("已生成存储过程并复制到剪贴板");
			}
		}

		private void tb_spname_Validating(object sender, CancelEventArgs e)
		{
			if (this.tb_spname.Text.Trim().Length == 0)
			{
				this.errorProvider1.SetError(this.tb_spname, "存储过程名称不能为空");
				e.Cancel = true;
				return;
			}
			this.errorProvider1.SetError(this.tb_spname, "");
		}

		private void tb_tblname_Validating(object sender, CancelEventArgs e)
		{
			if (this.tb_tblname.Text.Trim().Length == 0)
			{
				this.errorProvider1.SetError(this.tb_tblname, "数据库表名不能为空");
				e.Cancel = true;
				return;
			}
			this.errorProvider1.SetError(this.tb_tblname, "");
		}

		private void tb_idname_Validating(object sender, CancelEventArgs e)
		{
			if (this.tb_idname.Text.Trim().Length == 0)
			{
				this.errorProvider1.SetError(this.tb_idname, "标识字段名不能为空");
				e.Cancel = true;
				return;
			}
			this.errorProvider1.SetError(this.tb_idname, "");
		}

		private void tb_ofldname_Validating(object sender, CancelEventArgs e)
		{
			if (this.tb_ofldname.Text.Trim().Length == 0)
			{
				this.errorProvider1.SetError(this.tb_ofldname, "排序字段名不能为空");
				e.Cancel = true;
				return;
			}
			this.errorProvider1.SetError(this.tb_ofldname, "");
		}

		private void btn_regvar_Click(object sender, EventArgs e)
		{
			SPVarRegForm sPVarRegForm = new SPVarRegForm(this.tb_vars.Text);
			if (sPVarRegForm.ShowDialog(this) == DialogResult.OK)
			{
				this.tb_vars.Text = sPVarRegForm.SqlVariables;
			}
		}

		private void btn_help_Click(object sender, EventArgs e)
		{
			SPHelpForm sPHelpForm = new SPHelpForm();
			sPHelpForm.ShowDialog(this);
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
			this.components = new Container();
			this.label1 = new Label();
			this.label2 = new Label();
			this.label3 = new Label();
			this.label4 = new Label();
			this.label5 = new Label();
			this.label6 = new Label();
			this.label7 = new Label();
			this.label9 = new Label();
			this.tb_spname = new TextBox();
			this.tb_tblname = new TextBox();
			this.tb_idname = new TextBox();
			this.tb_ofldname = new TextBox();
			this.rb_asc = new RadioButton();
			this.rb_desc = new RadioButton();
			this.tb_fields = new TextBox();
			this.tb_vars = new TextBox();
			this.tb_where = new TextBox();
			this.ckb_rconly = new CheckBox();
			this.label10 = new Label();
			this.cmb_sqlver = new ComboBox();
			this.btn_gensp = new Button();
			this.btn_close = new Button();
			this.btn_regvar = new Button();
			this.groupBox1 = new GroupBox();
			this.errorProvider1 = new ErrorProvider(this.components);
			this.btn_help = new Button();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.errorProvider1).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.ForeColor = Color.Red;
			this.label1.Location = new Point(15, 36);
			this.label1.Name = "label1";
			this.label1.Size = new Size(91, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "存储过程名称：";
			this.label2.AutoSize = true;
			this.label2.ForeColor = Color.Red;
			this.label2.Location = new Point(15, 60);
			this.label2.Name = "label2";
			this.label2.Size = new Size(79, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "数据库表名：";
			this.label3.AutoSize = true;
			this.label3.ForeColor = Color.Red;
			this.label3.Location = new Point(15, 84);
			this.label3.Name = "label3";
			this.label3.Size = new Size(79, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "标识字段名：";
			this.label4.AutoSize = true;
			this.label4.ForeColor = Color.Red;
			this.label4.Location = new Point(15, 108);
			this.label4.Name = "label4";
			this.label4.Size = new Size(79, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "排序字段名：";
			this.label5.AutoSize = true;
			this.label5.ForeColor = Color.Red;
			this.label5.Location = new Point(15, 132);
			this.label5.Name = "label5";
			this.label5.Size = new Size(490, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "字段列表：select语句中要选择的字段列表，字段名间用英文标点“,”分隔，用“*”表示所有字段";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(15, 187);
			this.label6.Name = "label6";
			this.label6.Size = new Size(480, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "注册变量（可选）：存储过程中需要额外添加的变量，可以用来在where子句中做为条件值";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(15, 246);
			this.label7.Name = "label7";
			this.label7.Size = new Size(389, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "Where子句（可选）：SQL语句中的where子句，用来过滤指定条件的数据";
			this.label9.AutoSize = true;
			this.label9.ForeColor = Color.Red;
			this.label9.Location = new Point(308, 108);
			this.label9.Name = "label9";
			this.label9.Size = new Size(67, 13);
			this.label9.TabIndex = 8;
			this.label9.Text = "排序方式：";
			this.tb_spname.Location = new Point(109, 32);
			this.tb_spname.Name = "tb_spname";
			this.tb_spname.Size = new Size(257, 20);
			this.tb_spname.TabIndex = 9;
			this.tb_spname.Validating += new CancelEventHandler(this.tb_spname_Validating);
			this.tb_tblname.Location = new Point(109, 56);
			this.tb_tblname.Name = "tb_tblname";
			this.tb_tblname.Size = new Size(257, 20);
			this.tb_tblname.TabIndex = 10;
			this.tb_tblname.Validating += new CancelEventHandler(this.tb_tblname_Validating);
			this.tb_idname.Location = new Point(109, 80);
			this.tb_idname.Name = "tb_idname";
			this.tb_idname.Size = new Size(191, 20);
			this.tb_idname.TabIndex = 11;
			this.tb_idname.Validating += new CancelEventHandler(this.tb_idname_Validating);
			this.tb_ofldname.Location = new Point(109, 104);
			this.tb_ofldname.Name = "tb_ofldname";
			this.tb_ofldname.Size = new Size(194, 20);
			this.tb_ofldname.TabIndex = 12;
			this.tb_ofldname.Validating += new CancelEventHandler(this.tb_ofldname_Validating);
			this.rb_asc.AutoSize = true;
			this.rb_asc.Location = new Point(372, 106);
			this.rb_asc.Name = "rb_asc";
			this.rb_asc.Size = new Size(49, 17);
			this.rb_asc.TabIndex = 13;
			this.rb_asc.Text = "升序";
			this.rb_asc.UseVisualStyleBackColor = true;
			this.rb_desc.AutoSize = true;
			this.rb_desc.Checked = true;
			this.rb_desc.Location = new Point(419, 106);
			this.rb_desc.Name = "rb_desc";
			this.rb_desc.Size = new Size(49, 17);
			this.rb_desc.TabIndex = 14;
			this.rb_desc.TabStop = true;
			this.rb_desc.Text = "降序";
			this.rb_desc.UseVisualStyleBackColor = true;
			this.tb_fields.Location = new Point(19, 155);
			this.tb_fields.Name = "tb_fields";
			this.tb_fields.Size = new Size(477, 20);
			this.tb_fields.TabIndex = 15;
			this.tb_fields.Text = "*";
			this.tb_vars.Location = new Point(19, 210);
			this.tb_vars.Name = "tb_vars";
			this.tb_vars.ReadOnly = true;
			this.tb_vars.Size = new Size(385, 20);
			this.tb_vars.TabIndex = 16;
			this.tb_where.Location = new Point(19, 270);
			this.tb_where.Multiline = true;
			this.tb_where.Name = "tb_where";
			this.tb_where.Size = new Size(477, 61);
			this.tb_where.TabIndex = 17;
			this.ckb_rconly.AutoSize = true;
			this.ckb_rconly.Location = new Point(21, 337);
			this.ckb_rconly.Name = "ckb_rconly";
			this.ckb_rconly.Size = new Size(221, 17);
			this.ckb_rconly.TabIndex = 18;
			this.ckb_rconly.Text = "不统计记录总数，仅获取分页的数据 ";
			this.ckb_rconly.UseVisualStyleBackColor = true;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(15, 365);
			this.label10.Name = "label10";
			this.label10.Size = new Size(137, 13);
			this.label10.TabIndex = 19;
			this.label10.Text = "SQL Server 数据库版本：";
			this.cmb_sqlver.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmb_sqlver.FormattingEnabled = true;
			this.cmb_sqlver.Items.AddRange(new object[]
			{
				"SQL Server 2000",
				"SQL Server 2005"
			});
			this.cmb_sqlver.Location = new Point(158, 360);
			this.cmb_sqlver.Name = "cmb_sqlver";
			this.cmb_sqlver.Size = new Size(198, 21);
			this.cmb_sqlver.TabIndex = 20;
			this.btn_gensp.Location = new Point(18, 398);
			this.btn_gensp.Name = "btn_gensp";
			this.btn_gensp.Size = new Size(219, 44);
			this.btn_gensp.TabIndex = 21;
			this.btn_gensp.Text = "生成存储过程并复制到剪贴板(&G)";
			this.btn_gensp.UseVisualStyleBackColor = true;
			this.btn_gensp.Click += new EventHandler(this.btn_gensp_Click);
			this.btn_close.CausesValidation = false;
			this.btn_close.DialogResult = DialogResult.Cancel;
			this.btn_close.Location = new Point(452, 501);
			this.btn_close.Name = "btn_close";
			this.btn_close.Size = new Size(75, 23);
			this.btn_close.TabIndex = 22;
			this.btn_close.Text = "关闭(&C)";
			this.btn_close.UseVisualStyleBackColor = true;
			this.btn_regvar.Location = new Point(411, 207);
			this.btn_regvar.Name = "btn_regvar";
			this.btn_regvar.Size = new Size(84, 23);
			this.btn_regvar.TabIndex = 23;
			this.btn_regvar.Text = "注册(&R)...";
			this.btn_regvar.UseVisualStyleBackColor = true;
			this.btn_regvar.Click += new EventHandler(this.btn_regvar_Click);
			this.groupBox1.Controls.Add(this.btn_help);
			this.groupBox1.Controls.Add(this.tb_where);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btn_gensp);
			this.groupBox1.Controls.Add(this.btn_regvar);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.rb_desc);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.rb_asc);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.cmb_sqlver);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.ckb_rconly);
			this.groupBox1.Controls.Add(this.tb_spname);
			this.groupBox1.Controls.Add(this.tb_tblname);
			this.groupBox1.Controls.Add(this.tb_vars);
			this.groupBox1.Controls.Add(this.tb_idname);
			this.groupBox1.Controls.Add(this.tb_fields);
			this.groupBox1.Controls.Add(this.tb_ofldname);
			this.groupBox1.Location = new Point(19, 25);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(508, 457);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "创建SQL Server分页存储过程";
			this.errorProvider1.ContainerControl = this;
			this.btn_help.CausesValidation = false;
			this.btn_help.Location = new Point(276, 398);
			this.btn_help.Name = "btn_help";
			this.btn_help.Size = new Size(219, 44);
			this.btn_help.TabIndex = 24;
			this.btn_help.Text = "存储过程使用说明(&H)...";
			this.btn_help.UseVisualStyleBackColor = true;
			this.btn_help.Click += new EventHandler(this.btn_help_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(544, 538);
			base.ControlBox = false;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.btn_close);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "StoredProcForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "分页存储过程生成工具";
			base.Load += new EventHandler(this.StoredProcForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.errorProvider1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
