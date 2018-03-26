namespace SDRSharp.PanView
{
	// Token: 0x02000003 RID: 3
	public partial class FrequencyDialog : global::System.Windows.Forms.Form
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000025C5 File Offset: 0x000007C5
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025E4 File Offset: 0x000007E4
		private void InitializeComponent()
		{
			this.txtFrequency = new global::System.Windows.Forms.TextBox();
			this.butKhz = new global::System.Windows.Forms.Button();
			this.btnMhz = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.txtFrequency.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 10.2f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.txtFrequency.Location = new global::System.Drawing.Point(11, 24);
			this.txtFrequency.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			this.txtFrequency.Name = "txtFrequency";
			this.txtFrequency.Size = new global::System.Drawing.Size(91, 23);
			this.txtFrequency.TabIndex = 0;
			this.butKhz.DialogResult = global::System.Windows.Forms.DialogResult.Yes;
			this.butKhz.Location = new global::System.Drawing.Point(12, 55);
			this.butKhz.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			this.butKhz.Name = "butKhz";
			this.butKhz.Size = new global::System.Drawing.Size(38, 19);
			this.butKhz.TabIndex = 1;
			this.butKhz.Text = "kHz";
			this.butKhz.UseVisualStyleBackColor = true;
			this.btnMhz.DialogResult = global::System.Windows.Forms.DialogResult.No;
			this.btnMhz.Location = new global::System.Drawing.Point(64, 55);
			this.btnMhz.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnMhz.Name = "btnMhz";
			this.btnMhz.Size = new global::System.Drawing.Size(38, 19);
			this.btnMhz.TabIndex = 2;
			this.btnMhz.Text = "MHz";
			this.btnMhz.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(16, 6);
			this.label1.Margin = new global::System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(82, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Enter frequency";
			base.AcceptButton = this.butKhz;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(115, 80);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.btnMhz);
			base.Controls.Add(this.butKhz);
			base.Controls.Add(this.txtFrequency);
			base.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrequencyDialog";
			this.Text = "Frequency";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400000C RID: 12
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.TextBox txtFrequency;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.Button butKhz;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.Button btnMhz;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.Label label1;
	}
}
