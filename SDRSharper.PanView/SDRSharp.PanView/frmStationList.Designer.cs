namespace SDRSharp.PanView
{
	// Token: 0x02000014 RID: 20
	public partial class frmStationList : global::System.Windows.Forms.Form
	{
		// Token: 0x06000162 RID: 354 RVA: 0x0000E941 File Offset: 0x0000CB41
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000E960 File Offset: 0x0000CB60
		private void InitializeComponent()
		{
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.grdStations = new global::System.Windows.Forms.DataGridView();
			this.txtMaxStation = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.chkFreq = new global::System.Windows.Forms.CheckBox();
			this.chkDate = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.grdStations).BeginInit();
			base.SuspendLayout();
			this.grdStations.AllowUserToAddRows = false;
			this.grdStations.AllowUserToDeleteRows = false;
			this.grdStations.AllowUserToResizeRows = false;
			dataGridViewCellStyle.BackColor = global::System.Drawing.Color.FromArgb(240, 240, 240);
			this.grdStations.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.grdStations.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.grdStations.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = global::System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = global::System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 7.8f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = global::System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = global::System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = global::System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = global::System.Windows.Forms.DataGridViewTriState.False;
			this.grdStations.DefaultCellStyle = dataGridViewCellStyle2;
			this.grdStations.Location = new global::System.Drawing.Point(0, 3);
			this.grdStations.Margin = new global::System.Windows.Forms.Padding(2);
			this.grdStations.MultiSelect = false;
			this.grdStations.Name = "grdStations";
			this.grdStations.ReadOnly = true;
			this.grdStations.RowHeadersVisible = false;
			this.grdStations.RowTemplate.Height = 24;
			this.grdStations.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.grdStations.Size = new global::System.Drawing.Size(444, 184);
			this.grdStations.TabIndex = 0;
			this.grdStations.CellMouseDoubleClick += new global::System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdStations_CellMouseDoubleClick);
			this.txtMaxStation.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.txtMaxStation.Location = new global::System.Drawing.Point(136, 191);
			this.txtMaxStation.Name = "txtMaxStation";
			this.txtMaxStation.Size = new global::System.Drawing.Size(39, 20);
			this.txtMaxStation.TabIndex = 2;
			this.txtMaxStation.Validated += new global::System.EventHandler(this.txtMaxStation_Validated);
			this.label1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(2, 194);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(129, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Max Nr of station per freq:";
			this.chkFreq.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.chkFreq.AutoSize = true;
			this.chkFreq.CheckAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.chkFreq.Location = new global::System.Drawing.Point(194, 192);
			this.chkFreq.Name = "chkFreq";
			this.chkFreq.Size = new global::System.Drawing.Size(104, 17);
			this.chkFreq.TabIndex = 4;
			this.chkFreq.Text = "show Frequency";
			this.chkFreq.UseVisualStyleBackColor = true;
			this.chkDate.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.chkDate.AutoSize = true;
			this.chkDate.CheckAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.chkDate.Location = new global::System.Drawing.Point(302, 192);
			this.chkDate.Name = "chkDate";
			this.chkDate.Size = new global::System.Drawing.Size(105, 17);
			this.chkDate.TabIndex = 5;
			this.chkDate.Text = "show Date/Time";
			this.chkDate.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(444, 210);
			base.Controls.Add(this.chkDate);
			base.Controls.Add(this.chkFreq);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.txtMaxStation);
			base.Controls.Add(this.grdStations);
			base.Margin = new global::System.Windows.Forms.Padding(2);
			base.Name = "frmStationList";
			this.Text = "StationList";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.frmStationList_FormClosing);
			((global::System.ComponentModel.ISupportInitialize)this.grdStations).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000157 RID: 343
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000158 RID: 344
		private global::System.Windows.Forms.DataGridView grdStations;

		// Token: 0x04000159 RID: 345
		private global::System.Windows.Forms.TextBox txtMaxStation;

		// Token: 0x0400015A RID: 346
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400015B RID: 347
		private global::System.Windows.Forms.CheckBox chkFreq;

		// Token: 0x0400015C RID: 348
		private global::System.Windows.Forms.CheckBox chkDate;
	}
}
