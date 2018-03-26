using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.FrequencyManager
{
	public class DialogEntryInfo : Form
	{
		private MemoryEntry _memoryEntry;

		private IContainer components;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		private Label lblMode;

		private ComboBox comboGroupName;

		private TextBox textBoxName;

		private NumericUpDown frequencyNumericUpDown;

		private Button btnOk;

		private Button btnCancel;

		private NumericUpDown shiftNumericUpDown;

		private Label label6;

		private NumericUpDown nudFilterBandwidth;

		private Label label7;

		private CheckBox favouriteCb;

		public DialogEntryInfo()
		{
			this.InitializeComponent();
			this.ValidateForm();
		}

		public DialogEntryInfo(MemoryEntry memoryEntry, List<string> groups)
		{
			this._memoryEntry = memoryEntry;
			this.InitializeComponent();
			this.textBoxName.Text = memoryEntry.Name;
			this.comboGroupName.Text = memoryEntry.GroupName;
			this.frequencyNumericUpDown.Value = memoryEntry.Frequency;
			this.shiftNumericUpDown.Value = memoryEntry.Shift;
			this.lblMode.Text = memoryEntry.DetectorType.ToString();
			this.comboGroupName.Items.AddRange(groups.ToArray());
			this.nudFilterBandwidth.Value = memoryEntry.FilterBandwidth;
			this.favouriteCb.Checked = memoryEntry.IsFavourite;
			this.ValidateForm();
		}

		private void Control_TextChanged(object sender, EventArgs e)
		{
			this.ValidateForm();
		}

		private void ValidateForm()
		{
			bool enabled = this.textBoxName.Text != null && !"".Equals(this.textBoxName.Text.Trim()) && this.comboGroupName.Text != null && !"".Equals(this.comboGroupName.Text.Trim()) && this.frequencyNumericUpDown.Value != 0m && this.nudFilterBandwidth.Value != 0m;
			this.btnOk.Enabled = enabled;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this._memoryEntry.Name = this.textBoxName.Text.Trim();
			this._memoryEntry.GroupName = this.comboGroupName.Text.Trim();
			this._memoryEntry.Frequency = (long)this.frequencyNumericUpDown.Value;
			this._memoryEntry.Shift = (long)this.shiftNumericUpDown.Value;
			this._memoryEntry.FilterBandwidth = (long)this.nudFilterBandwidth.Value;
			this._memoryEntry.IsFavourite = this.favouriteCb.Checked;
			base.DialogResult = DialogResult.OK;
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
			this.label5 = new Label();
			this.lblMode = new Label();
			this.comboGroupName = new ComboBox();
			this.textBoxName = new TextBox();
			this.frequencyNumericUpDown = new NumericUpDown();
			this.btnOk = new Button();
			this.btnCancel = new Button();
			this.shiftNumericUpDown = new NumericUpDown();
			this.label6 = new Label();
			this.nudFilterBandwidth = new NumericUpDown();
			this.label7 = new Label();
			this.favouriteCb = new CheckBox();
			((ISupportInitialize)this.frequencyNumericUpDown).BeginInit();
			((ISupportInitialize)this.shiftNumericUpDown).BeginInit();
			((ISupportInitialize)this.nudFilterBandwidth).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(10, 11);
			this.label1.Margin = new Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(250, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select an existing group or enter a new group name";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(10, 37);
			this.label2.Margin = new Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new Size(39, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Group:";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(10, 67);
			this.label3.Margin = new Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new Size(38, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Name:";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(10, 98);
			this.label4.Margin = new Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new Size(60, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Frequency:";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(11, 187);
			this.label5.Margin = new Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new Size(37, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Mode:";
			this.lblMode.Location = new Point(89, 187);
			this.lblMode.Margin = new Padding(2, 0, 2, 0);
			this.lblMode.Name = "lblMode";
			this.lblMode.Size = new Size(120, 17);
			this.lblMode.TabIndex = 5;
			this.comboGroupName.FormattingEnabled = true;
			this.comboGroupName.Location = new Point(85, 34);
			this.comboGroupName.Margin = new Padding(2);
			this.comboGroupName.Name = "comboGroupName";
			this.comboGroupName.Size = new Size(178, 21);
			this.comboGroupName.TabIndex = 0;
			this.comboGroupName.TextChanged += this.Control_TextChanged;
			this.textBoxName.Location = new Point(85, 64);
			this.textBoxName.Margin = new Padding(2);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new Size(178, 20);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.TextChanged += this.Control_TextChanged;
			this.frequencyNumericUpDown.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.frequencyNumericUpDown.Location = new Point(85, 96);
			this.frequencyNumericUpDown.Margin = new Padding(2);
			this.frequencyNumericUpDown.Maximum = new decimal(new int[4]
			{
				-727379969,
				232,
				0,
				0
			});
			this.frequencyNumericUpDown.Minimum = new decimal(new int[4]
			{
				-727379969,
				232,
				0,
				-2147483648
			});
			this.frequencyNumericUpDown.Name = "frequencyNumericUpDown";
			this.frequencyNumericUpDown.Size = new Size(124, 20);
			this.frequencyNumericUpDown.TabIndex = 2;
			this.frequencyNumericUpDown.TextAlign = HorizontalAlignment.Right;
			this.frequencyNumericUpDown.ThousandsSeparator = true;
			this.frequencyNumericUpDown.ValueChanged += this.Control_TextChanged;
			this.btnOk.DialogResult = DialogResult.OK;
			this.btnOk.Enabled = false;
			this.btnOk.Location = new Point(149, 245);
			this.btnOk.Margin = new Padding(2);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(56, 23);
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "O&K";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += this.btnOk_Click;
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(209, 245);
			this.btnCancel.Margin = new Padding(2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(56, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.shiftNumericUpDown.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.shiftNumericUpDown.Location = new Point(85, 126);
			this.shiftNumericUpDown.Margin = new Padding(2);
			this.shiftNumericUpDown.Maximum = new decimal(new int[4]
			{
				-727379969,
				232,
				0,
				0
			});
			this.shiftNumericUpDown.Minimum = new decimal(new int[4]
			{
				-727379969,
				232,
				0,
				-2147483648
			});
			this.shiftNumericUpDown.Name = "shiftNumericUpDown";
			this.shiftNumericUpDown.Size = new Size(124, 20);
			this.shiftNumericUpDown.TabIndex = 3;
			this.shiftNumericUpDown.TextAlign = HorizontalAlignment.Right;
			this.shiftNumericUpDown.ThousandsSeparator = true;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(10, 129);
			this.label6.Margin = new Padding(2, 0, 2, 0);
			this.label6.Name = "label6";
			this.label6.Size = new Size(31, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Shift:";
			this.nudFilterBandwidth.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			this.nudFilterBandwidth.Location = new Point(85, 156);
			this.nudFilterBandwidth.Margin = new Padding(2);
			this.nudFilterBandwidth.Maximum = new decimal(new int[4]
			{
				1410065407,
				2,
				0,
				0
			});
			this.nudFilterBandwidth.Name = "nudFilterBandwidth";
			this.nudFilterBandwidth.Size = new Size(124, 20);
			this.nudFilterBandwidth.TabIndex = 4;
			this.nudFilterBandwidth.TextAlign = HorizontalAlignment.Right;
			this.nudFilterBandwidth.ThousandsSeparator = true;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(10, 160);
			this.label7.Margin = new Padding(2, 0, 2, 0);
			this.label7.Name = "label7";
			this.label7.Size = new Size(53, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "Filter BW:";
			this.favouriteCb.AutoSize = true;
			this.favouriteCb.Location = new Point(85, 207);
			this.favouriteCb.Name = "favouriteCb";
			this.favouriteCb.Size = new Size(70, 17);
			this.favouriteCb.TabIndex = 16;
			this.favouriteCb.Text = "Favourite";
			this.favouriteCb.UseVisualStyleBackColor = true;
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(276, 279);
			base.Controls.Add(this.favouriteCb);
			base.Controls.Add(this.nudFilterBandwidth);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.shiftNumericUpDown);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.frequencyNumericUpDown);
			base.Controls.Add(this.textBoxName);
			base.Controls.Add(this.comboGroupName);
			base.Controls.Add(this.lblMode);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Margin = new Padding(2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DialogEntryInfo";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Edit Entry Information";
			((ISupportInitialize)this.frequencyNumericUpDown).EndInit();
			((ISupportInitialize)this.shiftNumericUpDown).EndInit();
			((ISupportInitialize)this.nudFilterBandwidth).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
