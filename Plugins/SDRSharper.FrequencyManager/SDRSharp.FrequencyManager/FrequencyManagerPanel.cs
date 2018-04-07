using SDRSharp.Common;
using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.FrequencyManager
{
	[Category("SDRSharp")]
	[DesignTimeVisible(true)]
	[Description("RF Memory Management Panel")]
	public class FrequencyManagerPanel : UserControl
    {
		private const string AllGroups = "[All Groups]";

		private const string FavouriteGroup = "[Favourites]";

		private readonly SortableBindingList<MemoryEntry> _displayedEntries = new SortableBindingList<MemoryEntry>();

		private readonly List<MemoryEntry> _entries;

		private readonly SettingsPersister _settingsPersister;

		private readonly List<string> _groups = new List<string>();

		private ISharpControl _controlInterface;

		private IContainer components;

		private ToolStrip mainToolStrip;

		private ToolStripButton btnNewEntry;

		private ToolStripButton btnEdit;

		private ToolStripButton btnDelete;

		private Label label17;

		private DataGridView frequencyDataGridView;

        private CustomComboxBox comboGroups;

		private BindingSource memoryEntryBindingSource;

		private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn frequencyDataGridViewTextBoxColumn;

		public string SelectedGroup
		{
			get
			{
				return (string)this.comboGroups.SelectedItem;
			}
			set
			{
				if (value != null && this.comboGroups.Items.IndexOf(value) != -1)
				{
					this.comboGroups.SelectedIndex = this.comboGroups.Items.IndexOf(value);
				}
			}
		}

		public FrequencyManagerPanel(ISharpControl control)
		{
			this.InitializeComponent();
			this._controlInterface = control;
			if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
			{
				this._settingsPersister = new SettingsPersister();
				this._entries = this._settingsPersister.ReadStoredFrequencies();
				this._groups = this.GetGroupsFromEntries();
				this.ProcessGroups(null);
			}
			this.memoryEntryBindingSource.DataSource = this._displayedEntries;
		}

		private void btnNewEntry_Click(object sender, EventArgs e)
		{
			this.Bookmark();
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (this.memoryEntryBindingSource.Current != null)
			{
				this.DoEdit((MemoryEntry)this.memoryEntryBindingSource.Current, false);
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			MemoryEntry memoryEntry = (MemoryEntry)this.memoryEntryBindingSource.Current;
			if (memoryEntry != null && MessageBox.Show("Are you sure that you want to delete '" + memoryEntry.Name + "'?", "Delete Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this._entries.Remove(memoryEntry);
				this._settingsPersister.PersistStoredFrequencies(this._entries);
				this._displayedEntries.Remove(memoryEntry);
			}
		}

		private void DoEdit(MemoryEntry memoryEntry, bool isNew)
		{
			DialogEntryInfo dialogEntryInfo = new DialogEntryInfo(memoryEntry, this._groups);
			if (dialogEntryInfo.ShowDialog() == DialogResult.OK)
			{
				if (isNew)
				{
					this._entries.Add(memoryEntry);
					this._entries.Sort((MemoryEntry e1, MemoryEntry e2) => e1.Frequency.CompareTo(e2.Frequency));
				}
				this._settingsPersister.PersistStoredFrequencies(this._entries);
				if (!this._groups.Contains(memoryEntry.GroupName))
				{
					this._groups.Add(memoryEntry.GroupName);
					this.ProcessGroups(memoryEntry.GroupName);
				}
				else if ((string)this.comboGroups.SelectedItem == "[All Groups]" || (string)this.comboGroups.SelectedItem == memoryEntry.GroupName || ((string)this.comboGroups.SelectedItem == "[Favourites]" && memoryEntry.IsFavourite))
				{
					if (isNew)
					{
						this._displayedEntries.Add(memoryEntry);
					}
				}
				else
				{
					this.comboGroups.SelectedItem = memoryEntry.GroupName;
				}
			}
		}

		private List<string> GetGroupsFromEntries()
		{
			List<string> list = new List<string>();
			foreach (MemoryEntry entry in this._entries)
			{
				if (!list.Contains(entry.GroupName))
				{
					list.Add(entry.GroupName);
				}
			}
			return list;
		}

		private void frequencyDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (this.frequencyDataGridView.Columns[e.ColumnIndex].DataPropertyName == "Frequency" && e.Value != null)
			{
				long frequency = (long)e.Value;
				e.Value = FrequencyManagerPanel.GetFrequencyDisplay(frequency);
				e.FormattingApplied = true;
			}
		}

		private void frequencyDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			this.Navigate();
		}

		private void ProcessGroups(string selectedGroupName)
		{
			this._groups.Sort();
			this.comboGroups.Items.Clear();
			this.comboGroups.Items.Add("[All Groups]");
			this.comboGroups.Items.Add("[Favourites]");
			this.comboGroups.Items.AddRange(this._groups.ToArray());
			if (selectedGroupName != null)
			{
				this.comboGroups.SelectedItem = selectedGroupName;
			}
			else
			{
				this.comboGroups.SelectedIndex = 0;
			}
		}

		private void comboGroups_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.memoryEntryBindingSource.Clear();
			this._displayedEntries.Clear();
			if (this.comboGroups.SelectedIndex != -1)
			{
				string text = (string)this.comboGroups.SelectedItem;
				foreach (MemoryEntry entry in this._entries)
				{
					if (text == "[All Groups]" || entry.GroupName == text || (text == "[Favourites]" && entry.IsFavourite))
					{
						this._displayedEntries.Add(entry);
					}
				}
			}
		}

		private void frequencyDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			this.btnDelete.Enabled = (this.frequencyDataGridView.SelectedRows.Count > 0);
			this.btnEdit.Enabled = (this.frequencyDataGridView.SelectedRows.Count > 0);
		}

		public void Bookmark()
		{
			if (this._controlInterface.IsPlaying)
			{
				MemoryEntry memoryEntry = new MemoryEntry();
				memoryEntry.DetectorType = this._controlInterface.DetectorType;
				memoryEntry.CenterFrequency = this._controlInterface.CenterFrequency;
				memoryEntry.Frequency = this._controlInterface.Frequency;
				memoryEntry.FilterBandwidth = this._controlInterface.FilterBandwidth;
				memoryEntry.Shift = (this._controlInterface.FrequencyShiftEnabled ? this._controlInterface.FrequencyShift : 0);
				memoryEntry.GroupName = "Misc";
				if (this._controlInterface.DetectorType == DetectorType.WFM)
				{
					string text = this._controlInterface.RdsProgramService.Trim();
					memoryEntry.Name = string.Empty;
					if (!string.IsNullOrEmpty(text))
					{
						memoryEntry.Name = text;
					}
					else
					{
						memoryEntry.Name = FrequencyManagerPanel.GetFrequencyDisplay(this._controlInterface.Frequency) + " " + memoryEntry.DetectorType;
					}
				}
				else
				{
					memoryEntry.Name = FrequencyManagerPanel.GetFrequencyDisplay(this._controlInterface.Frequency) + " " + memoryEntry.DetectorType;
				}
				memoryEntry.IsFavourite = true;
				this.DoEdit(memoryEntry, true);
			}
		}

		public void Navigate()
		{
			if (this._controlInterface.IsPlaying)
			{
				int num = (this.frequencyDataGridView.SelectedCells.Count > 0) ? this.frequencyDataGridView.SelectedCells[0].RowIndex : (-1);
				if (num != -1)
				{
					try
					{
						MemoryEntry memoryEntry = (MemoryEntry)this.memoryEntryBindingSource.List[num];
						this._controlInterface.FrequencyShift = memoryEntry.Shift;
						this._controlInterface.FrequencyShiftEnabled = (memoryEntry.Shift != 0);
						if (Math.Abs(memoryEntry.Frequency - memoryEntry.CenterFrequency - this._controlInterface.FrequencyShift) < this._controlInterface.RFBandwidth / 2)
						{
							this._controlInterface.CenterFrequency = memoryEntry.CenterFrequency;
							this._controlInterface.Frequency = memoryEntry.Frequency;
						}
						else
						{
							long num2 = memoryEntry.Frequency - memoryEntry.Shift + this._controlInterface.FilterBandwidth / 2 + 5000;
							if (Math.Abs(memoryEntry.Frequency - num2 - memoryEntry.Shift) < this._controlInterface.RFBandwidth / 2)
							{
								this._controlInterface.CenterFrequency = num2;
								this._controlInterface.Frequency = memoryEntry.Frequency;
							}
							else
							{
								this._controlInterface.CenterFrequency = memoryEntry.Frequency - this._controlInterface.FrequencyShift;
								this._controlInterface.Frequency = memoryEntry.Frequency;
							}
						}
						this._controlInterface.DetectorType = memoryEntry.DetectorType;
						this._controlInterface.FilterBandwidth = (int)memoryEntry.FilterBandwidth;
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
			}
		}

		private static string GetFrequencyDisplay(long frequency)
		{
			long num = Math.Abs(frequency);
			if (num == 0)
			{
				return "DC";
			}
			if (num > 1500000000)
			{
				return $"{(double)frequency / 1000000000.0:#,0.000 000} GHz";
			}
			if (num > 30000000)
			{
				return $"{(double)frequency / 1000000.0:0,0.000#} MHz";
			}
			if (num > 1000)
			{
				return $"{(double)frequency / 1000.0:#,#.###} kHz";
			}
			return frequency.ToString();
		}

		private void frequencyDataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.Navigate();
				e.Handled = true;
			}
		}

		private void frequencyDataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			this._controlInterface.FFTSkips = -10;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrequencyManagerPanel));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.mainToolStrip = new ToolStrip();
			this.btnNewEntry = new ToolStripButton();
			this.btnEdit = new ToolStripButton();
			this.btnDelete = new ToolStripButton();
			this.label17 = new Label();
			this.frequencyDataGridView = new DataGridView();
			this.nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.frequencyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			this.memoryEntryBindingSource = new BindingSource(this.components);
			this.comboGroups = new CustomComboxBox();
			this.mainToolStrip.SuspendLayout();
			((ISupportInitialize)this.frequencyDataGridView).BeginInit();
			((ISupportInitialize)this.memoryEntryBindingSource).BeginInit();
			base.SuspendLayout();
            this.mainToolStrip.Renderer = new CustomRenderer(); //ADDED
			this.mainToolStrip.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.mainToolStrip.AutoSize = false;
			this.mainToolStrip.Dock = DockStyle.None;
            this.mainToolStrip.BackColor = Color.FromArgb(45, 45, 48);
            this.mainToolStrip.GripStyle = ToolStripGripStyle.Hidden;
			this.mainToolStrip.Items.AddRange(new ToolStripItem[3]
			{
				this.btnNewEntry,
				this.btnEdit,
				this.btnDelete
			});
			this.mainToolStrip.Location = new Point(1, 6);
			this.mainToolStrip.MinimumSize = new Size(205, 0);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new Size(235, 22);
			this.mainToolStrip.Stretch = true;
			this.mainToolStrip.TabIndex = 7;
			this.mainToolStrip.Text = "toolStrip1";
			//this.btnNewEntry.Image = (Image)componentResourceManager.GetObject("btnNewEntry.Image");
			this.btnNewEntry.ImageTransparentColor = Color.Magenta;
			this.btnNewEntry.Name = "btnNewEntry";
            this.btnNewEntry.ForeColor = Color.RoyalBlue;
			this.btnNewEntry.Size = new Size(48, 19);
			this.btnNewEntry.Text = "New";
			this.btnNewEntry.Click += this.btnNewEntry_Click;
			//this.btnEdit.Image = (Image)componentResourceManager.GetObject("btnEdit.Image");
			this.btnEdit.ImageTransparentColor = Color.Magenta;
			this.btnEdit.Name = "btnEdit";
            this.btnEdit.ForeColor = Color.RoyalBlue;
            this.btnEdit.Size = new Size(45, 19);
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += this.btnEdit_Click;
			//this.btnDelete.Image = (Image)componentResourceManager.GetObject("btnDelete.Image");
			this.btnDelete.ImageTransparentColor = Color.Magenta;
			this.btnDelete.Name = "btnDelete";
            this.btnDelete.ForeColor = Color.RoyalBlue;
            this.btnDelete.Size = new Size(58, 19);
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += this.btnDelete_Click;
            this.label17.ForeColor = Color.Lime;
            this.label17.BackColor = Color.FromArgb(45, 45, 48);
            this.label17.AutoSize = false;
			this.label17.Location = new Point(2, 55);
			this.label17.Margin = new Padding(2, 0, 0, 0);
            this.label17.TextAlign = ContentAlignment.MiddleLeft;
			this.label17.Name = "label17";
			this.label17.Size = new Size(39, 23);
			this.label17.TabIndex = 5;
			this.label17.Text = "Group:";

            //Added -----------------------------------------------------------------------------
            this.frequencyDataGridView.EnableHeadersVisualStyles = false;
            this.frequencyDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            this.frequencyDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Lime;
            this.frequencyDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            this.frequencyDataGridView.BackgroundColor = Color.FromArgb(64, 64, 64);
            this.frequencyDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            //-----------------------------------------------------------------------------------

            this.frequencyDataGridView.AllowUserToAddRows = false;
			this.frequencyDataGridView.AllowUserToDeleteRows = false;
			this.frequencyDataGridView.AllowUserToResizeRows = false;

            // Replaced - Added ------------------------------------------------------------------
			dataGridViewCellStyle.BackColor = Color.FromArgb(54, 54, 54);
            dataGridViewCellStyle.ForeColor = Color.Gray;
            dataGridViewCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle.SelectionForeColor = Color.RoyalBlue;
            //this.frequencyDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
            this.frequencyDataGridView.DefaultCellStyle = dataGridViewCellStyle;
            //------------------------------------------------------------------------------------

            this.frequencyDataGridView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.frequencyDataGridView.AutoGenerateColumns = false;
			this.frequencyDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.frequencyDataGridView.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.frequencyDataGridViewTextBoxColumn);
			this.frequencyDataGridView.DataSource = this.memoryEntryBindingSource;
			this.frequencyDataGridView.Location = new Point(0, 60);
			this.frequencyDataGridView.Margin = new Padding(2, 2, 2, 2);
			this.frequencyDataGridView.Name = "frequencyDataGridView";
            this.frequencyDataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.frequencyDataGridView.RowHeadersVisible = false;
			this.frequencyDataGridView.RowTemplate.Height = 18;
			this.frequencyDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.frequencyDataGridView.ShowCellErrors = false;
			this.frequencyDataGridView.ShowCellToolTips = false;
			this.frequencyDataGridView.ShowEditingIcon = false;
			this.frequencyDataGridView.ShowRowErrors = false;
			this.frequencyDataGridView.Size = new Size(234, 329);
			this.frequencyDataGridView.TabIndex = 6;
			this.frequencyDataGridView.CellDoubleClick += this.frequencyDataGridView_CellDoubleClick;
			this.frequencyDataGridView.CellFormatting += this.frequencyDataGridView_CellFormatting;
			this.frequencyDataGridView.Scroll += this.frequencyDataGridView_Scroll;
			this.frequencyDataGridView.SelectionChanged += this.frequencyDataGridView_SelectionChanged;
			this.frequencyDataGridView.KeyDown += this.frequencyDataGridView_KeyDown;
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.frequencyDataGridViewTextBoxColumn.DataPropertyName = "Frequency";
			this.frequencyDataGridViewTextBoxColumn.HeaderText = "Frequency";
			this.frequencyDataGridViewTextBoxColumn.Name = "frequencyDataGridViewTextBoxColumn";
			this.frequencyDataGridViewTextBoxColumn.ReadOnly = true;
			this.memoryEntryBindingSource.DataSource = typeof(MemoryEntry);

            //Added-----------------------------------------------------------------------
            this.comboGroups.FlatStyle = FlatStyle.Flat;
            this.comboGroups.BackColor = Color.FromArgb(45, 45, 48);
            this.comboGroups.ForeColor = Color.RoyalBlue;
            //----------------------------------------------------------------------------

			this.comboGroups.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.comboGroups.AutoCompleteMode = AutoCompleteMode.Suggest;
			this.comboGroups.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboGroups.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboGroups.FormattingEnabled = true;
			this.comboGroups.Location = new Point(39, 35);
			this.comboGroups.Margin = new Padding(2, 2, 2, 2);
			this.comboGroups.Name = "comboGroups";
			this.comboGroups.Size = new Size(195, 21);
			this.comboGroups.TabIndex = 4;
			this.comboGroups.SelectedIndexChanged += this.comboGroups_SelectedIndexChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.mainToolStrip);
			base.Controls.Add(this.label17);
			base.Controls.Add(this.frequencyDataGridView);
			base.Controls.Add(this.comboGroups);
			base.Margin = new Padding(2, 2, 2, 2);
			base.Name = "FrequencyManagerPanel";
			base.Size = new Size(236, 389);
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			((ISupportInitialize)this.frequencyDataGridView).EndInit();
			((ISupportInitialize)this.memoryEntryBindingSource).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
