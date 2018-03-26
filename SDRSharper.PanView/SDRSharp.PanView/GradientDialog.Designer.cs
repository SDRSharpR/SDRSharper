namespace SDRSharp.PanView
{
	// Token: 0x02000004 RID: 4
	public partial class GradientDialog : global::System.Windows.Forms.Form
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00004139 File Offset: 0x00002339
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004158 File Offset: 0x00002358
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.colorListBox = new global::System.Windows.Forms.ListBox();
			this.upButton = new global::System.Windows.Forms.Button();
			this.downButton = new global::System.Windows.Forms.Button();
			this.gradientPictureBox = new global::System.Windows.Forms.PictureBox();
			this.addButton = new global::System.Windows.Forms.Button();
			this.deleteButton = new global::System.Windows.Forms.Button();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.colorDialog = new global::System.Windows.Forms.ColorDialog();
			this.btn1 = new global::System.Windows.Forms.RadioButton();
			this.btn2 = new global::System.Windows.Forms.RadioButton();
			this.btn3 = new global::System.Windows.Forms.RadioButton();
			this.btn4 = new global::System.Windows.Forms.RadioButton();
			this.btn5 = new global::System.Windows.Forms.RadioButton();
			this.traceButton = new global::System.Windows.Forms.Button();
			this.backgroundButton = new global::System.Windows.Forms.Button();
			this.cmbFill = new global::System.Windows.Forms.ComboBox();
			this.labFill = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.picBox = new global::System.Windows.Forms.PictureBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.fastButton = new global::System.Windows.Forms.Button();
			this.saButton = new global::System.Windows.Forms.Button();
			this.wfButton = new global::System.Windows.Forms.Button();
			this.agButton = new global::System.Windows.Forms.Button();
			this.toolTip = new global::System.Windows.Forms.ToolTip(this.components);
			this.trackBar = new global::System.Windows.Forms.TrackBar();
			this.okButton = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.gradientPictureBox).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.picBox).BeginInit();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.trackBar).BeginInit();
			base.SuspendLayout();
			this.colorListBox.BackColor = global::System.Drawing.Color.FromArgb(224, 224, 224);
			this.colorListBox.DrawMode = global::System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.colorListBox.FormattingEnabled = true;
			this.colorListBox.ItemHeight = 15;
			this.colorListBox.Location = new global::System.Drawing.Point(7, 21);
			this.colorListBox.Name = "colorListBox";
			this.colorListBox.Size = new global::System.Drawing.Size(62, 210);
			this.colorListBox.TabIndex = 0;
			this.toolTip.SetToolTip(this.colorListBox, "Select. move or edit gradient color.");
			this.colorListBox.Click += new global::System.EventHandler(this.colorListBox_Click);
			this.colorListBox.DrawItem += new global::System.Windows.Forms.DrawItemEventHandler(this.colorListBox_DrawItem);
			this.upButton.Enabled = false;
			this.upButton.Location = new global::System.Drawing.Point(110, 161);
			this.upButton.Name = "upButton";
			this.upButton.Size = new global::System.Drawing.Size(55, 23);
			this.upButton.TabIndex = 1;
			this.upButton.Text = "Up";
			this.toolTip.SetToolTip(this.upButton, "Move color up");
			this.upButton.UseVisualStyleBackColor = true;
			this.upButton.Click += new global::System.EventHandler(this.upButton_Click);
			this.downButton.Enabled = false;
			this.downButton.Location = new global::System.Drawing.Point(111, 191);
			this.downButton.Name = "downButton";
			this.downButton.Size = new global::System.Drawing.Size(53, 23);
			this.downButton.TabIndex = 2;
			this.downButton.Text = "Down";
			this.toolTip.SetToolTip(this.downButton, "Move color down");
			this.downButton.UseVisualStyleBackColor = true;
			this.downButton.Click += new global::System.EventHandler(this.downButton_Click);
			this.gradientPictureBox.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.gradientPictureBox.Location = new global::System.Drawing.Point(71, 23);
			this.gradientPictureBox.Name = "gradientPictureBox";
			this.gradientPictureBox.Size = new global::System.Drawing.Size(29, 209);
			this.gradientPictureBox.TabIndex = 3;
			this.gradientPictureBox.TabStop = false;
			this.gradientPictureBox.Paint += new global::System.Windows.Forms.PaintEventHandler(this.gradientPictureBox_Paint);
			this.addButton.Location = new global::System.Drawing.Point(105, 76);
			this.addButton.Name = "addButton";
			this.addButton.Size = new global::System.Drawing.Size(63, 23);
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add color";
			this.toolTip.SetToolTip(this.addButton, "Add new color to gradient.");
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new global::System.EventHandler(this.addButton_Click);
			this.deleteButton.Enabled = false;
			this.deleteButton.Location = new global::System.Drawing.Point(105, 106);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new global::System.Drawing.Size(64, 23);
			this.deleteButton.TabIndex = 4;
			this.deleteButton.Text = "Delete";
			this.toolTip.SetToolTip(this.deleteButton, "Delete selected color");
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new global::System.EventHandler(this.deleteButton_Click);
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new global::System.Drawing.Point(332, 280);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(48, 20);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.toolTip.SetToolTip(this.cancelButton, "Cancel/undo changes");
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new global::System.EventHandler(this.closeButton_Click);
			this.colorDialog.AnyColor = true;
			this.colorDialog.FullOpen = true;
			this.btn1.AutoSize = true;
			this.btn1.BackColor = global::System.Drawing.SystemColors.Control;
			this.btn1.ForeColor = global::System.Drawing.SystemColors.ControlText;
			this.btn1.Location = new global::System.Drawing.Point(62, 10);
			this.btn1.Margin = new global::System.Windows.Forms.Padding(2);
			this.btn1.Name = "btn1";
			this.btn1.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.btn1.Size = new global::System.Drawing.Size(31, 17);
			this.btn1.TabIndex = 8;
			this.btn1.TabStop = true;
			this.btn1.Text = "1";
			this.toolTip.SetToolTip(this.btn1, "Select gradient to use");
			this.btn1.UseVisualStyleBackColor = false;
			this.btn1.CheckedChanged += new global::System.EventHandler(this.btnGradient_CheckedChanged);
			this.btn2.AutoSize = true;
			this.btn2.Location = new global::System.Drawing.Point(92, 10);
			this.btn2.Margin = new global::System.Windows.Forms.Padding(2);
			this.btn2.Name = "btn2";
			this.btn2.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.btn2.Size = new global::System.Drawing.Size(31, 17);
			this.btn2.TabIndex = 9;
			this.btn2.TabStop = true;
			this.btn2.Text = "2";
			this.btn2.UseVisualStyleBackColor = true;
			this.btn2.CheckedChanged += new global::System.EventHandler(this.btnGradient_CheckedChanged);
			this.btn3.AutoSize = true;
			this.btn3.Location = new global::System.Drawing.Point(122, 10);
			this.btn3.Margin = new global::System.Windows.Forms.Padding(2);
			this.btn3.Name = "btn3";
			this.btn3.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.btn3.Size = new global::System.Drawing.Size(31, 17);
			this.btn3.TabIndex = 10;
			this.btn3.TabStop = true;
			this.btn3.Text = "3";
			this.btn3.UseVisualStyleBackColor = true;
			this.btn3.CheckedChanged += new global::System.EventHandler(this.btnGradient_CheckedChanged);
			this.btn4.AutoSize = true;
			this.btn4.Location = new global::System.Drawing.Point(152, 10);
			this.btn4.Margin = new global::System.Windows.Forms.Padding(2);
			this.btn4.Name = "btn4";
			this.btn4.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.btn4.Size = new global::System.Drawing.Size(31, 17);
			this.btn4.TabIndex = 11;
			this.btn4.TabStop = true;
			this.btn4.Text = "4";
			this.btn4.UseVisualStyleBackColor = true;
			this.btn4.CheckedChanged += new global::System.EventHandler(this.btnGradient_CheckedChanged);
			this.btn5.AutoSize = true;
			this.btn5.Location = new global::System.Drawing.Point(182, 10);
			this.btn5.Margin = new global::System.Windows.Forms.Padding(2);
			this.btn5.Name = "btn5";
			this.btn5.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.btn5.Size = new global::System.Drawing.Size(31, 17);
			this.btn5.TabIndex = 12;
			this.btn5.TabStop = true;
			this.btn5.Text = "5";
			this.btn5.UseVisualStyleBackColor = true;
			this.btn5.CheckedChanged += new global::System.EventHandler(this.btnGradient_CheckedChanged);
			this.traceButton.Location = new global::System.Drawing.Point(183, 42);
			this.traceButton.Name = "traceButton";
			this.traceButton.Size = new global::System.Drawing.Size(54, 23);
			this.traceButton.TabIndex = 15;
			this.traceButton.Text = "Trace";
			this.toolTip.SetToolTip(this.traceButton, "Change spectrum trace color");
			this.traceButton.UseVisualStyleBackColor = true;
			this.traceButton.Click += new global::System.EventHandler(this.traceButton_Click);
			this.backgroundButton.ForeColor = global::System.Drawing.Color.White;
			this.backgroundButton.Location = new global::System.Drawing.Point(245, 42);
			this.backgroundButton.Name = "backgroundButton";
			this.backgroundButton.Size = new global::System.Drawing.Size(53, 23);
			this.backgroundButton.TabIndex = 17;
			this.backgroundButton.Text = "B-gnd";
			this.toolTip.SetToolTip(this.backgroundButton, "Change background color");
			this.backgroundButton.UseVisualStyleBackColor = true;
			this.backgroundButton.Click += new global::System.EventHandler(this.backgroundButton_Click);
			this.cmbFill.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFill.FormattingEnabled = true;
			this.cmbFill.Items.AddRange(new object[]
			{
				"none\t",
				"static",
				"dyn-1",
				"dyn-2"
			});
			this.cmbFill.Location = new global::System.Drawing.Point(330, 45);
			this.cmbFill.Name = "cmbFill";
			this.cmbFill.Size = new global::System.Drawing.Size(49, 21);
			this.cmbFill.TabIndex = 18;
			this.toolTip.SetToolTip(this.cmbFill, "Select SA fill type.");
			this.cmbFill.SelectedIndexChanged += new global::System.EventHandler(this.cmbFill_SelectedIndexChanged);
			this.labFill.AutoSize = true;
			this.labFill.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.labFill.Location = new global::System.Drawing.Point(305, 46);
			this.labFill.Name = "labFill";
			this.labFill.Size = new global::System.Drawing.Size(28, 16);
			this.labFill.TabIndex = 19;
			this.labFill.Text = "Fill:";
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label2.Location = new global::System.Drawing.Point(5, 10);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(46, 16);
			this.label2.TabIndex = 20;
			this.label2.Text = "Select";
			this.picBox.BackColor = global::System.Drawing.SystemColors.ButtonHighlight;
			this.picBox.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			this.picBox.Cursor = global::System.Windows.Forms.Cursors.Default;
			this.picBox.Enabled = false;
			this.picBox.InitialImage = null;
			this.picBox.Location = new global::System.Drawing.Point(181, 96);
			this.picBox.Name = "picBox";
			this.picBox.Size = new global::System.Drawing.Size(200, 177);
			this.picBox.TabIndex = 22;
			this.picBox.TabStop = false;
			this.toolTip.SetToolTip(this.picBox, "Pick/move color or gradient");
			this.picBox.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.picBox_MouseDown);
			this.picBox.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.picBox_MouseMove);
			this.picBox.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.picBox_MouseUp);
			this.groupBox1.Controls.Add(this.fastButton);
			this.groupBox1.Controls.Add(this.colorListBox);
			this.groupBox1.Controls.Add(this.gradientPictureBox);
			this.groupBox1.Controls.Add(this.upButton);
			this.groupBox1.Controls.Add(this.downButton);
			this.groupBox1.Controls.Add(this.addButton);
			this.groupBox1.Controls.Add(this.deleteButton);
			this.groupBox1.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox1.Location = new global::System.Drawing.Point(2, 35);
			this.groupBox1.Margin = new global::System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new global::System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new global::System.Drawing.Size(172, 240);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Edit color gradient";
			this.fastButton.Location = new global::System.Drawing.Point(105, 23);
			this.fastButton.Name = "fastButton";
			this.fastButton.Size = new global::System.Drawing.Size(63, 23);
			this.fastButton.TabIndex = 8;
			this.fastButton.Text = "Fast edit";
			this.toolTip.SetToolTip(this.fastButton, "Edit whole gradient");
			this.fastButton.UseVisualStyleBackColor = true;
			this.fastButton.Click += new global::System.EventHandler(this.fastButton_Click);
			this.saButton.Location = new global::System.Drawing.Point(7, 280);
			this.saButton.Name = "saButton";
			this.saButton.Size = new global::System.Drawing.Size(43, 20);
			this.saButton.TabIndex = 24;
			this.saButton.Text = "Spect";
			this.toolTip.SetToolTip(this.saButton, "Change SpectrumAnalyzer colors");
			this.saButton.UseVisualStyleBackColor = true;
			this.saButton.Click += new global::System.EventHandler(this.saButton_Click);
			this.wfButton.Location = new global::System.Drawing.Point(57, 280);
			this.wfButton.Name = "wfButton";
			this.wfButton.Size = new global::System.Drawing.Size(43, 20);
			this.wfButton.TabIndex = 25;
			this.wfButton.Text = "W-fall";
			this.toolTip.SetToolTip(this.wfButton, "Change Waterfall colors");
			this.wfButton.UseVisualStyleBackColor = true;
			this.wfButton.Click += new global::System.EventHandler(this.wfButton_Click);
			this.agButton.Location = new global::System.Drawing.Point(106, 280);
			this.agButton.Name = "agButton";
			this.agButton.Size = new global::System.Drawing.Size(43, 20);
			this.agButton.TabIndex = 25;
			this.agButton.Text = "Audio";
			this.toolTip.SetToolTip(this.agButton, "Change Audiogram colors");
			this.agButton.UseVisualStyleBackColor = true;
			this.agButton.Click += new global::System.EventHandler(this.agButton_Click);
			this.toolTip.AutoPopDelay = 2000;
			this.toolTip.InitialDelay = 500;
			this.toolTip.ReshowDelay = 100;
			this.trackBar.AutoSize = false;
			this.trackBar.Location = new global::System.Drawing.Point(181, 79);
			this.trackBar.Margin = new global::System.Windows.Forms.Padding(2);
			this.trackBar.Maximum = 100;
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new global::System.Drawing.Size(200, 20);
			this.trackBar.TabIndex = 39;
			this.trackBar.TickFrequency = 10;
			this.trackBar.TickStyle = global::System.Windows.Forms.TickStyle.None;
			this.trackBar.Value = 100;
			this.trackBar.ValueChanged += new global::System.EventHandler(this.trackBar_ValueChanged);
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okButton.Enabled = false;
			this.okButton.Location = new global::System.Drawing.Point(242, 280);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(54, 20);
			this.okButton.TabIndex = 40;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new global::System.EventHandler(this.saveButton_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new global::System.Drawing.Size(388, 306);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.trackBar);
			base.Controls.Add(this.agButton);
			base.Controls.Add(this.wfButton);
			base.Controls.Add(this.saButton);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.picBox);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cmbFill);
			base.Controls.Add(this.backgroundButton);
			base.Controls.Add(this.traceButton);
			base.Controls.Add(this.btn5);
			base.Controls.Add(this.btn4);
			base.Controls.Add(this.btn3);
			base.Controls.Add(this.btn2);
			base.Controls.Add(this.btn1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.labFill);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "GradientDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color Editor";
			this.toolTip.SetToolTip(this, "Accept/save changes");
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.GradientDialog_FormClosing);
			((global::System.ComponentModel.ISupportInitialize)this.gradientPictureBox).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.picBox).EndInit();
			this.groupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.trackBar).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000027 RID: 39
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000028 RID: 40
		private global::System.Windows.Forms.ListBox colorListBox;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.Button upButton;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.Button downButton;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.PictureBox gradientPictureBox;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.Button addButton;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.Button deleteButton;

		// Token: 0x0400002E RID: 46
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x0400002F RID: 47
		private global::System.Windows.Forms.ColorDialog colorDialog;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.RadioButton btn1;

		// Token: 0x04000031 RID: 49
		private global::System.Windows.Forms.RadioButton btn2;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.RadioButton btn3;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.RadioButton btn4;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.RadioButton btn5;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.Button traceButton;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.Button backgroundButton;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.ComboBox cmbFill;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.Label labFill;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.PictureBox picBox;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400003C RID: 60
		private global::System.Windows.Forms.Button saButton;

		// Token: 0x0400003D RID: 61
		private global::System.Windows.Forms.Button wfButton;

		// Token: 0x0400003E RID: 62
		private global::System.Windows.Forms.Button agButton;

		// Token: 0x0400003F RID: 63
		private global::System.Windows.Forms.ToolTip toolTip;

		// Token: 0x04000040 RID: 64
		private global::System.Windows.Forms.TrackBar trackBar;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.Button fastButton;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.Button okButton;
	}
}
