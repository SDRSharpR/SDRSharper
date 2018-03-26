namespace SDRSharp.RTLTCP
{
	// Token: 0x02000002 RID: 2
	public partial class RTLTcpSettings : global::System.Windows.Forms.Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		private void InitializeComponent()
		{
			this.hostBox = new global::System.Windows.Forms.TextBox();
			this.portBox = new global::System.Windows.Forms.TextBox();
			this.srBox = new global::System.Windows.Forms.TextBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.autoRB = new global::System.Windows.Forms.RadioButton();
			this.manualRB = new global::System.Windows.Forms.RadioButton();
			this.label4 = new global::System.Windows.Forms.Label();
			this.gainBox = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.fcBox = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.hostBox.Location = new global::System.Drawing.Point(139, 11);
			this.hostBox.Name = "hostBox";
			this.hostBox.Size = new global::System.Drawing.Size(133, 20);
			this.hostBox.TabIndex = 0;
			this.portBox.Location = new global::System.Drawing.Point(139, 37);
			this.portBox.Name = "portBox";
			this.portBox.Size = new global::System.Drawing.Size(133, 20);
			this.portBox.TabIndex = 1;
			this.srBox.Location = new global::System.Drawing.Point(139, 88);
			this.srBox.Name = "srBox";
			this.srBox.Size = new global::System.Drawing.Size(133, 20);
			this.srBox.TabIndex = 2;
			this.button1.Location = new global::System.Drawing.Point(133, 213);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(139, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Update Settings";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(27, 11);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(55, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Hostname";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(27, 37);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(26, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Port";
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(27, 88);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(68, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Sample Rate";
			this.autoRB.AutoSize = true;
			this.autoRB.Location = new global::System.Drawing.Point(30, 123);
			this.autoRB.Name = "autoRB";
			this.autoRB.Size = new global::System.Drawing.Size(72, 17);
			this.autoRB.TabIndex = 7;
			this.autoRB.TabStop = true;
			this.autoRB.Text = "Auto Gain";
			this.autoRB.UseVisualStyleBackColor = true;
			this.autoRB.CheckedChanged += new global::System.EventHandler(this.autoRB_CheckedChanged);
			this.manualRB.AutoSize = true;
			this.manualRB.Location = new global::System.Drawing.Point(139, 123);
			this.manualRB.Name = "manualRB";
			this.manualRB.Size = new global::System.Drawing.Size(85, 17);
			this.manualRB.TabIndex = 8;
			this.manualRB.TabStop = true;
			this.manualRB.Text = "Manual Gain";
			this.manualRB.UseVisualStyleBackColor = true;
			this.manualRB.CheckedChanged += new global::System.EventHandler(this.manualRB_CheckedChanged);
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(27, 151);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(101, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Gain setting (dB*10)";
			this.gainBox.Location = new global::System.Drawing.Point(139, 148);
			this.gainBox.Name = "gainBox";
			this.gainBox.Size = new global::System.Drawing.Size(133, 20);
			this.gainBox.TabIndex = 10;
			this.label5.AutoSize = true;
			this.label5.Location = new global::System.Drawing.Point(27, 181);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(108, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Freq Correction (ppm)";
			this.fcBox.Location = new global::System.Drawing.Point(139, 181);
			this.fcBox.Name = "fcBox";
			this.fcBox.Size = new global::System.Drawing.Size(133, 20);
			this.fcBox.TabIndex = 12;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 248);
			base.Controls.Add(this.fcBox);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.gainBox);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.manualRB);
			base.Controls.Add(this.autoRB);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.srBox);
			base.Controls.Add(this.portBox);
			base.Controls.Add(this.hostBox);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RTLTcpSettings";
			base.ShowInTaskbar = false;
			this.Text = "RTLTcpSettings";
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.RTLTcpSettings_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000001 RID: 1
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000002 RID: 2
		private global::System.Windows.Forms.TextBox hostBox;

		// Token: 0x04000003 RID: 3
		private global::System.Windows.Forms.TextBox portBox;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.TextBox srBox;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.RadioButton autoRB;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.RadioButton manualRB;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.TextBox gainBox;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.TextBox fcBox;
	}
}
