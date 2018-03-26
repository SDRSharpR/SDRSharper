using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.FrequencyScanner
{
	public class DialogEditRange : Form
	{
		private List<MemoryEntryFrequencyRange> _memoryEntryFrequencyRange;

		private readonly SortableBindingList<MemoryEntryFrequencyRange> _displayedEntries = new SortableBindingList<MemoryEntryFrequencyRange>();

		private IContainer components;

		private DataGridView editRangeDataGridView;

		private Button cancelButton;

		private Button okButton;

		private Button deleteRangeButton;

		private BindingSource editRangeBindingSource;

		private DataGridViewTextBoxColumn rangeNameDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn startFrequencyDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn endFrequencyDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn rangeDetectorTypeDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn FilterBandwidth;

		private DataGridViewTextBoxColumn StepSize;

		public DialogEditRange()
		{
			this.InitializeComponent();
			this.editRangeBindingSource.DataSource = this._displayedEntries;
			this.editRangeDataGridView.CellEndEdit += this.ValidateForm;
			this.editRangeDataGridView.CellBeginEdit += this.FormBegiEdit;
		}

		private void FormBegiEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			this.okButton.Enabled = false;
		}

		public List<MemoryEntryFrequencyRange> EditRange(List<MemoryEntryFrequencyRange> memoryEntry)
		{
			this._memoryEntryFrequencyRange = memoryEntry;
			if (this._memoryEntryFrequencyRange != null)
			{
				foreach (MemoryEntryFrequencyRange item in this._memoryEntryFrequencyRange)
				{
					this._displayedEntries.Add(item);
				}
			}
			return this._memoryEntryFrequencyRange;
		}

		private void editRangeDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
		}

		private string GetFrequencyDisplay(long frequency)
		{
			long num = Math.Abs(frequency);
			if (num == 0L)
			{
				return "DC";
			}
			if (num > 1500000000)
			{
				return $"{(double)frequency / 1000000000.0:#,0.000 000} GHz";
			}
			if (num > 30000000)
			{
				return $"{(double)frequency / 1000000.0:0,0.000###} MHz";
			}
			if (num > 1000)
			{
				return $"{(double)frequency / 1000.0:#,#.###} kHz";
			}
			return frequency.ToString();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (this.editRangeBindingSource != null)
			{
				this._memoryEntryFrequencyRange.Clear();
				foreach (MemoryEntryFrequencyRange item in this.editRangeBindingSource)
				{
					this._memoryEntryFrequencyRange.Add(item);
				}
			}
			base.DialogResult = DialogResult.OK;
		}

		private void ValidateForm(object sender, DataGridViewCellEventArgs e)
		{
			bool flag = true;
			foreach (MemoryEntryFrequencyRange item in this.editRangeBindingSource)
			{
				int row = this.editRangeBindingSource.IndexOf(item);
				bool flag2 = item.RangeName != null && !"".Equals(item.RangeName.Trim());
				this.IndicateErrorCells(0, row, flag2);
				bool flag3 = item.StartFrequency < item.EndFrequency && item.StartFrequency != 0L && item.EndFrequency != 0;
				this.IndicateErrorCells(1, row, flag3);
				this.IndicateErrorCells(2, row, flag3);
				bool flag4 = item.FilterBandwidth != 0;
				this.IndicateErrorCells(4, row, flag4);
				bool flag5 = item.StepSize != 0;
				this.IndicateErrorCells(5, row, flag5);
				flag = (flag & flag2 & flag3 & flag4 & flag5);
			}
			this.okButton.Enabled = flag;
		}

		private void IndicateErrorCells(int collumn, int row, bool valid)
		{
			if (valid)
			{
				this.editRangeDataGridView[collumn, row].Style.BackColor = this.editRangeDataGridView.DefaultCellStyle.BackColor;
			}
			else
			{
				this.editRangeDataGridView[collumn, row].Style.BackColor = Color.LightPink;
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void deleteRangeButton_Click(object sender, EventArgs e)
		{
			if (this.editRangeBindingSource.IndexOf(this.editRangeBindingSource.Current) > -1)
			{
				this.editRangeBindingSource.RemoveCurrent();
				this.ValidateForm(null, null);
			}
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			this.editRangeDataGridView = new DataGridView();
			this.rangeNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.startFrequencyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.endFrequencyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.rangeDetectorTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.FilterBandwidth = new DataGridViewTextBoxColumn();
			this.StepSize = new DataGridViewTextBoxColumn();
			this.editRangeBindingSource = new BindingSource(this.components);
			this.cancelButton = new Button();
			this.okButton = new Button();
			this.deleteRangeButton = new Button();
			((ISupportInitialize)this.editRangeDataGridView).BeginInit();
			((ISupportInitialize)this.editRangeBindingSource).BeginInit();
			base.SuspendLayout();
			this.editRangeDataGridView.AutoGenerateColumns = false;
			this.editRangeDataGridView.Columns.AddRange(this.rangeNameDataGridViewTextBoxColumn, this.startFrequencyDataGridViewTextBoxColumn, this.endFrequencyDataGridViewTextBoxColumn, this.rangeDetectorTypeDataGridViewTextBoxColumn, this.FilterBandwidth, this.StepSize);
			this.editRangeDataGridView.DataSource = this.editRangeBindingSource;
			this.editRangeDataGridView.Location = new Point(15, 15);
			this.editRangeDataGridView.Margin = new Padding(4, 4, 4, 4);
			this.editRangeDataGridView.MultiSelect = false;
			this.editRangeDataGridView.Name = "editRangeDataGridView";
			this.editRangeDataGridView.RowHeadersVisible = false;
			this.editRangeDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.editRangeDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.editRangeDataGridView.ShowEditingIcon = false;
			this.editRangeDataGridView.ShowRowErrors = false;
			this.editRangeDataGridView.Size = new Size(912, 351);
			this.editRangeDataGridView.TabIndex = 0;
			this.rangeNameDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.rangeNameDataGridViewTextBoxColumn.DataPropertyName = "RangeName";
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			this.rangeNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle;
			this.rangeNameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.rangeNameDataGridViewTextBoxColumn.MinimumWidth = 110;
			this.rangeNameDataGridViewTextBoxColumn.Name = "rangeNameDataGridViewTextBoxColumn";
			this.startFrequencyDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.startFrequencyDataGridViewTextBoxColumn.DataPropertyName = "StartFrequency";
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "N0";
			dataGridViewCellStyle2.NullValue = null;
			this.startFrequencyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.startFrequencyDataGridViewTextBoxColumn.HeaderText = "Start";
			this.startFrequencyDataGridViewTextBoxColumn.MinimumWidth = 80;
			this.startFrequencyDataGridViewTextBoxColumn.Name = "startFrequencyDataGridViewTextBoxColumn";
			this.startFrequencyDataGridViewTextBoxColumn.Resizable = DataGridViewTriState.True;
			this.endFrequencyDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.endFrequencyDataGridViewTextBoxColumn.DataPropertyName = "EndFrequency";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "N0";
			dataGridViewCellStyle3.NullValue = null;
			this.endFrequencyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
			this.endFrequencyDataGridViewTextBoxColumn.HeaderText = "End";
			this.endFrequencyDataGridViewTextBoxColumn.MinimumWidth = 80;
			this.endFrequencyDataGridViewTextBoxColumn.Name = "endFrequencyDataGridViewTextBoxColumn";
			this.rangeDetectorTypeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.rangeDetectorTypeDataGridViewTextBoxColumn.DataPropertyName = "RangeDetectorType";
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.NullValue = null;
			this.rangeDetectorTypeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
			this.rangeDetectorTypeDataGridViewTextBoxColumn.FillWeight = 10f;
			this.rangeDetectorTypeDataGridViewTextBoxColumn.HeaderText = "Detector";
			this.rangeDetectorTypeDataGridViewTextBoxColumn.MinimumWidth = 60;
			this.rangeDetectorTypeDataGridViewTextBoxColumn.Name = "rangeDetectorTypeDataGridViewTextBoxColumn";
			this.rangeDetectorTypeDataGridViewTextBoxColumn.Resizable = DataGridViewTriState.False;
			this.rangeDetectorTypeDataGridViewTextBoxColumn.Width = 60;
			this.FilterBandwidth.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.FilterBandwidth.DataPropertyName = "FilterBandwidth";
			dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "N0";
			dataGridViewCellStyle5.NullValue = null;
			this.FilterBandwidth.DefaultCellStyle = dataGridViewCellStyle5;
			this.FilterBandwidth.HeaderText = "Bandwidth";
			this.FilterBandwidth.Name = "FilterBandwidth";
			this.FilterBandwidth.Width = 80;
			this.StepSize.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			this.StepSize.DataPropertyName = "StepSize";
			dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "N0";
			dataGridViewCellStyle6.NullValue = null;
			this.StepSize.DefaultCellStyle = dataGridViewCellStyle6;
			this.StepSize.HeaderText = "Step size";
			this.StepSize.Name = "StepSize";
			this.StepSize.Width = 80;
			this.editRangeBindingSource.DataSource = typeof(MemoryEntryFrequencyRange);
			this.cancelButton.DialogResult = DialogResult.Cancel;
			this.cancelButton.Location = new Point(833, 372);
			this.cancelButton.Margin = new Padding(4, 4, 4, 4);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new Size(100, 28);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += this.cancelButton_Click;
			this.okButton.Location = new Point(725, 372);
			this.okButton.Margin = new Padding(4, 4, 4, 4);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(100, 28);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += this.okButton_Click;
			this.deleteRangeButton.Location = new Point(15, 370);
			this.deleteRangeButton.Margin = new Padding(4, 4, 4, 4);
			this.deleteRangeButton.Name = "deleteRangeButton";
			this.deleteRangeButton.Size = new Size(100, 28);
			this.deleteRangeButton.TabIndex = 3;
			this.deleteRangeButton.Text = "Delete rows";
			this.deleteRangeButton.UseVisualStyleBackColor = true;
			this.deleteRangeButton.Click += this.deleteRangeButton_Click;
			base.AcceptButton = this.okButton;
			base.AutoScaleDimensions = new SizeF(8f, 16f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.BackColor = SystemColors.Control;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new Size(932, 407);
			base.ControlBox = false;
			base.Controls.Add(this.deleteRangeButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.editRangeDataGridView);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Margin = new Padding(4, 4, 4, 4);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DialogEditRange";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "Edit Range";
			((ISupportInitialize)this.editRangeDataGridView).EndInit();
			((ISupportInitialize)this.editRangeBindingSource).EndInit();
			base.ResumeLayout(false);
		}
	}
}
