using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.PanView
{
	public class frmStationList : Form
	{
		private const int FREQ = 0;

		private const int TIME = 1;

		private const int DAYS = 2;

		private const int CTRY = 3;

		private const int NAME = 4;

		private const int LANG = 5;

		private const int DBUV = 6;

		private const int SITE = 7;

		private const int EMP = 8;

		private const int DIST = 9;

		private const int POWR = 10;

		private string _stations;

		private int _showDbm;

		private IContainer components;

		private DataGridView grdStations;

		private TextBox txtMaxStation;

		private Label label1;

		private CheckBox chkFreq;

		private CheckBox chkDate;

		public int ShowDbm
		{
			set
			{
				this._showDbm = value;
			}
		}

		public string Stations
		{
			get
			{
				return this._stations;
			}
			set
			{
				this._stations = value;
				this.ShowStations();
			}
		}

		public event EventHandler StationDataChanged;

		public frmStationList()
		{
			this.InitializeComponent();
			base.TopMost = true;
			int num = TextRenderer.MeasureText("abcdefghijklmnopqrstuvw", this.grdStations.Font).Width / 26;
			this.txtMaxStation.Text = Utils.GetStringSetting("StationlistMax", "10");
			this.chkFreq.Checked = Utils.GetBooleanSetting("StationlistFreq");
			this.chkDate.Checked = Utils.GetBooleanSetting("StationlistDate");
			bool numeric = true;
			bool visible = true;
			this.grdStations.Columns.Add(this.column("freq", "Freq", num * 12, false, this.chkFreq.Checked));
			this.grdStations.Columns.Add(this.column("ctry", "Ctry", num * 10, false, visible));
			this.grdStations.Columns.Add(this.column("lang", "Lang", num * 10, false, visible));
			this.grdStations.Columns.Add(this.column("name", "Name", num * 40, false, visible));
			this.grdStations.Columns.Add(this.column("site", "Site", num * 50, false, visible));
			this.grdStations.Columns.Add(this.column("dist", "Km", num * 10, numeric, visible));
			this.grdStations.Columns.Add(this.column("powr", "kW", num * 10, numeric, visible));
			this.grdStations.Columns.Add(this.column("dbm", "dBm", num * 10, numeric, false));
			this.grdStations.Columns.Add(this.column("sig", "Sig", num * 10, numeric, visible));
			this.grdStations.Columns.Add(this.column("time", "Time", num * 16, false, this.chkDate.Checked));
			this.grdStations.Columns.Add(this.column("days", "Days", num * 15, false, this.chkDate.Checked));
			int num2 = 0;
			for (int i = 0; i < this.grdStations.Columns.Count; i++)
			{
				num2 += this.grdStations.Columns[i].Width;
			}
			base.Width = Utils.GetIntSetting("StationlistWidth", num2);
			base.Height = Utils.GetIntSetting("StationlistHeight", 300);
		}

		private void frmStationList_FormClosing(object sender, FormClosingEventArgs e)
		{
			Utils.SaveSetting("StationlistFreq", this.chkFreq.Checked);
			Utils.SaveSetting("StationlistDate", this.chkDate.Checked);
			Utils.SaveSetting("StationlistWidth", base.Width);
			Utils.SaveSetting("StationlistHeight", base.Height);
		}

		private DataGridViewColumn column(string name, string header, int width, bool numeric, bool visible)
		{
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn.Name = name;
			dataGridViewTextBoxColumn.HeaderText = header;
			dataGridViewTextBoxColumn.Width = width;
			if (numeric)
			{
				dataGridViewTextBoxColumn.ValueType = typeof(int);
			}
			if (!visible)
			{
				dataGridViewTextBoxColumn.Visible = false;
			}
			return dataGridViewTextBoxColumn;
		}

		private void ShowStations()
		{
			if (this._stations != null && base.Visible)
			{
				this.grdStations.Rows.Clear();
				string[] array = this._stations.Split(';');
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				string headerText = "dBm";
				if (this._showDbm == 1)
				{
					headerText = "dBuV";
				}
				if (this._showDbm == 2)
				{
					headerText = "S-pts";
				}
				if (this._showDbm == 3)
				{
					headerText = "%";
				}
				this.grdStations.Columns["sig"].HeaderText = headerText;
				this.grdStations.Columns["sig"].ValueType = ((this._showDbm == 2) ? typeof(string) : typeof(int));
				for (int i = 0; i <= array.GetUpperBound(0); i++)
				{
					if (array[i].Length != 0)
					{
						string[] array2 = array[i].Split(',');
						int upperBound = array2.GetUpperBound(0);
						if (9 <= upperBound)
						{
							int.TryParse(array2[9], out num);
						}
						if (10 <= upperBound)
						{
							int.TryParse(array2[10], out num2);
						}
						if (upperBound >= 7)
						{
							num3 = Utils.Val(array2[6], 0) - 107;
							string text = Utils.Signal(num3, this._showDbm, false);
							if (this._showDbm == 2)
							{
								if (text.IndexOf('S') < 0)
								{
									text = "S9" + text;
								}
								this.grdStations.Rows.Add(array2[0], array2[3], array2[5], array2[4], array2[7], num, num2, num3, text, array2[1], array2[2]);
							}
							else
							{
								this.grdStations.Rows.Add(array2[0], array2[3], array2[5], array2[4], array2[7], num, num2, num3, Utils.Val(text, 0), array2[1], array2[2]);
							}
							this.grdStations[3, num4++].ToolTipText = "Double click to set station on top of list";
						}
					}
				}
			}
		}

		private void GetStations()
		{
			this._stations = "";
			string[] array = new string[11];
			for (int i = 0; i < this.grdStations.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = this.grdStations.Rows[i];
				array[0] = dataGridViewRow.Cells[0].Value.ToString();
				array[3] = dataGridViewRow.Cells[1].Value.ToString();
				array[5] = dataGridViewRow.Cells[2].Value.ToString();
				array[4] = dataGridViewRow.Cells[3].Value.ToString();
				array[7] = dataGridViewRow.Cells[4].Value.ToString();
				array[9] = dataGridViewRow.Cells[5].Value.ToString();
				array[10] = dataGridViewRow.Cells[6].Value.ToString();
				array[6] = ((int)dataGridViewRow.Cells[7].Value + 107).ToString();
				array[1] = dataGridViewRow.Cells[9].Value.ToString();
				array[2] = dataGridViewRow.Cells[10].Value.ToString();
				string str = string.Join(",", array);
				this._stations = this._stations + ((this._stations.Length <= 1) ? "" : ";") + str;
			}
		}

		private void grdStations_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int rowIndex = e.RowIndex;
			int index = this.grdStations.Columns["dbm"].Index;
			if (rowIndex >= 0 && index != 0)
			{
				int num = -999;
				for (int i = 0; i < this.grdStations.Rows.Count; i++)
				{
					int num2 = (int)this.grdStations.Rows[i].Cells[index].Value;
					if (num2 > num)
					{
						num = num2;
					}
				}
				this.grdStations.Rows[rowIndex].Cells[index].Value = num + 1;
				this.grdStations.Rows[rowIndex].Cells["sig"].Value = Utils.Signal(num + 1, this._showDbm, false);
				if (this.grdStations.Rows.Count > 1)
				{
					this.grdStations.Sort(this.grdStations.Columns[index], ListSortDirection.Descending);
				}
				this.GetStations();
				this.StationDataChanged(this, null);
			}
		}

		private void txtMaxStation_Validated(object sender, EventArgs e)
		{
			if (Utils.Val(this.txtMaxStation.Text, 0) > 0)
			{
				Utils.SaveSetting("StationlistMax", Utils.Val(this.txtMaxStation.Text, 0));
				MessageBox.Show("Please restart SDR.");
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.grdStations = new DataGridView();
			this.txtMaxStation = new TextBox();
			this.label1 = new Label();
			this.chkFreq = new CheckBox();
			this.chkDate = new CheckBox();
			((ISupportInitialize)this.grdStations).BeginInit();
			base.SuspendLayout();
			this.grdStations.AllowUserToAddRows = false;
			this.grdStations.AllowUserToDeleteRows = false;
			this.grdStations.AllowUserToResizeRows = false;
			dataGridViewCellStyle.BackColor = Color.FromArgb(240, 240, 240);
			this.grdStations.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			this.grdStations.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.grdStations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.grdStations.DefaultCellStyle = dataGridViewCellStyle2;
			this.grdStations.Location = new Point(0, 3);
			this.grdStations.Margin = new Padding(2);
			this.grdStations.MultiSelect = false;
			this.grdStations.Name = "grdStations";
			this.grdStations.ReadOnly = true;
			this.grdStations.RowHeadersVisible = false;
			this.grdStations.RowTemplate.Height = 24;
			this.grdStations.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.grdStations.Size = new Size(444, 184);
			this.grdStations.TabIndex = 0;
			this.grdStations.CellMouseDoubleClick += this.grdStations_CellMouseDoubleClick;
			this.txtMaxStation.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.txtMaxStation.Location = new Point(136, 191);
			this.txtMaxStation.Name = "txtMaxStation";
			this.txtMaxStation.Size = new Size(39, 20);
			this.txtMaxStation.TabIndex = 2;
			this.txtMaxStation.Validated += this.txtMaxStation_Validated;
			this.label1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(2, 194);
			this.label1.Name = "label1";
			this.label1.Size = new Size(129, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Max Nr of station per freq:";
			this.chkFreq.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.chkFreq.AutoSize = true;
			this.chkFreq.CheckAlign = ContentAlignment.MiddleRight;
			this.chkFreq.Location = new Point(194, 192);
			this.chkFreq.Name = "chkFreq";
			this.chkFreq.Size = new Size(104, 17);
			this.chkFreq.TabIndex = 4;
			this.chkFreq.Text = "show Frequency";
			this.chkFreq.UseVisualStyleBackColor = true;
			this.chkDate.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.chkDate.AutoSize = true;
			this.chkDate.CheckAlign = ContentAlignment.MiddleRight;
			this.chkDate.Location = new Point(302, 192);
			this.chkDate.Name = "chkDate";
			this.chkDate.Size = new Size(105, 17);
			this.chkDate.TabIndex = 5;
			this.chkDate.Text = "show Date/Time";
			this.chkDate.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(444, 210);
			base.Controls.Add(this.chkDate);
			base.Controls.Add(this.chkFreq);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.txtMaxStation);
			base.Controls.Add(this.grdStations);
			base.Margin = new Padding(2);
			base.Name = "frmStationList";
			this.Text = "StationList";
			base.FormClosing += this.frmStationList_FormClosing;
			((ISupportInitialize)this.grdStations).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
