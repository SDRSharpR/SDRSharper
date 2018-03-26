using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.Trunker
{
	public class LogOptions : Form
	{
		private readonly SettingsPersisterTrunker _settingsPersister;

		private TrunkerLogger _trunkLoggerData;

		private List<TrunkerSettings> _trunkerConfig;

		private IContainer components;

		private ComboBox logStylePreset;

		private Label label2;

		private Label label1;

		private Label label3;

		private Label exampleOut;

		private TextBox logStyle;

		private Button logCustomizeSave;

		private Label guideLabel;

		private Button button1;

		private Label label4;

		private ComboBox logSimulation;

		private Label guideLabel2;

		private Label label5;

		private Label label6;

		private TextBox parkedString;

		private TextBox unkString;

		private CheckBox ignoreParked;

		public LogOptions()
		{
			this.InitializeComponent();
			this._settingsPersister = new SettingsPersisterTrunker();
			this._trunkerConfig = this._settingsPersister.readConfig();
			this._trunkLoggerData = new TrunkerLogger();
			this.guideLabel.Text = "Guide:" + Environment.NewLine + "%t = targetlabel" + Environment.NewLine + "%tid = targetid" + Environment.NewLine + "%s = sourcelabel" + Environment.NewLine + "%sid = sourceid" + Environment.NewLine + "%f = frequency (Hz)" + Environment.NewLine + "%fk  = frequency (kHz)" + Environment.NewLine + "%fm  = frequency (MHz)" + Environment.NewLine + "%a = action" + Environment.NewLine + "%r = receiver";
			this.guideLabel2.Text = "To filter strings based on variable availabity, use brackets. Please try some presets from the dropdown list above, as well as the simulator for examples on how this works." + Environment.NewLine + "Nested filters are not currently supported." + Environment.NewLine + "Only works on %tid, %s, and %sid.";
			this.logSimulation.SelectedIndex = 0;
			this.logStyle.Text = (this._trunkerConfig[0].logStyle ?? "%t %fm MHz");
			this.parkedString.Text = (this._trunkerConfig[0].parkedStr ?? "Parked");
			this.unkString.Text = (this._trunkerConfig[0].unknownSrcStr ?? "Unknown");
			this.ignoreParked.Checked = this._trunkerConfig[0].ignoreParked;
		}

		private void PrepareLog()
		{
			this._trunkLoggerData = null;
			this._trunkLoggerData = new TrunkerLogger();
		}

		private void PopulateFakeLog()
		{
			this.PrepareLog();
			this._trunkLoggerData.currentFrequency = 860987725m;
			this._trunkLoggerData.currentReceiver = "Debug";
			this._trunkLoggerData.currentTrunklabel = "Test Target";
			this._trunkLoggerData.currentTrunkgroup = "1234";
			switch (this.logSimulation.SelectedIndex + 1)
			{
			case 1:
				this._trunkLoggerData.currentAction = "Park";
				this._trunkLoggerData.currentTrunklabel = null;
				this._trunkLoggerData.currentTrunkgroup = null;
				break;
			case 2:
				this._trunkLoggerData.currentAction = "Listen";
				this._trunkLoggerData.currentSourcegroup = "420";
				this._trunkLoggerData.currentSourcelabel = "Test Source";
				break;
			}
		}

		private void DoNothing(Exception ex)
		{
		}

		private void logCustomizeSave_Click(object sender, EventArgs e)
		{
			try
			{
				this._trunkerConfig[0].settingsExist();
			}
			catch (Exception ex)
			{
				this.DoNothing(ex);
				this._trunkerConfig.Insert(0, new TrunkerSettings());
			}
			this._trunkerConfig[0].logStyle = this.logStyle.Text;
			this._trunkerConfig[0].parkedStr = this.parkedString.Text;
			this._trunkerConfig[0].unknownSrcStr = this.unkString.Text;
			this._trunkerConfig[0].ignoreParked = this.ignoreParked.Checked;
			this._settingsPersister.writeConfig(this._trunkerConfig);
			base.Close();
		}

		private void logStyle_TextChanged(object sender, EventArgs e)
		{
			string text = this.doLogParse();
			if (text != null)
			{
				this.exampleOut.Text = text;
			}
		}

		private void logStylePreset_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.logStylePreset.SelectedIndex + 1)
			{
			case 2:
				this.logStyle.Text = "%t (%fm MHz)";
				break;
			case 3:
				this.logStyle.Text = "%t[ %s]";
				break;
			case 4:
				this.logStyle.Text = "%t[ %s] %fm MHz";
				break;
			case 5:
				this.logStyle.Text = "[%s to ]%t";
				break;
			case 6:
				this.logStyle.Text = "[%s to ]%t (%fm MHz)";
				break;
			case 7:
				this.logStyle.Text = "%s";
				break;
			case 8:
				this.logStyle.Text = "%t";
				break;
			case 9:
				this.logStyle.Text = "%fm MHz";
				break;
			default:
				this.logStyle.Text = "%t %fm MHz";
				break;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void logSimulation_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.PopulateFakeLog();
			string text = this.doLogParse();
			if (text != null)
			{
				this.exampleOut.Text = text;
			}
		}

		private string doLogParse()
		{
			return LogParser.ParseLogStyle(this._trunkLoggerData, this.logStyle.Text, this.parkedString.Text, this.unkString.Text, this.ignoreParked.Checked);
		}

		private void parkedString_TextChanged(object sender, EventArgs e)
		{
			string text = this.doLogParse();
			if (text != null)
			{
				this.exampleOut.Text = text;
			}
		}

		private void unkString_TextChanged(object sender, EventArgs e)
		{
			string text = this.doLogParse();
			if (text != null)
			{
				this.exampleOut.Text = text;
			}
		}

		private void ignoreParked_CheckedChanged(object sender, EventArgs e)
		{
			if (this.logSimulation.SelectedIndex == 0)
			{
				this.logSimulation.SelectedIndex = 1;
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
			this.logStylePreset = new ComboBox();
			this.label2 = new Label();
			this.label1 = new Label();
			this.label3 = new Label();
			this.exampleOut = new Label();
			this.logStyle = new TextBox();
			this.logCustomizeSave = new Button();
			this.guideLabel = new Label();
			this.button1 = new Button();
			this.label4 = new Label();
			this.logSimulation = new ComboBox();
			this.guideLabel2 = new Label();
			this.label5 = new Label();
			this.label6 = new Label();
			this.parkedString = new TextBox();
			this.unkString = new TextBox();
			this.ignoreParked = new CheckBox();
			base.SuspendLayout();
			this.logStylePreset.DropDownStyle = ComboBoxStyle.DropDownList;
			this.logStylePreset.FormattingEnabled = true;
			this.logStylePreset.Items.AddRange(new object[9]
			{
				"target frequency",
				"target (frequency)",
				"target source",
				"target source frequency",
				"source to target",
				"source to target (frequency)",
				"source",
				"target",
				"frequency"
			});
			this.logStylePreset.Location = new Point(72, 12);
			this.logStylePreset.Name = "logStylePreset";
			this.logStylePreset.Size = new Size(330, 21);
			this.logStylePreset.TabIndex = 56;
			this.logStylePreset.SelectedIndexChanged += this.logStylePreset_SelectedIndexChanged;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(3, 15);
			this.label2.Name = "label2";
			this.label2.Size = new Size(45, 13);
			this.label2.TabIndex = 57;
			this.label2.Text = "Presets:";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(3, 48);
			this.label1.Name = "label1";
			this.label1.Size = new Size(54, 13);
			this.label1.TabIndex = 58;
			this.label1.Text = "Log Style:";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(3, 127);
			this.label3.Name = "label3";
			this.label3.Size = new Size(85, 13);
			this.label3.TabIndex = 58;
			this.label3.Text = "Example Output:";
			this.exampleOut.Location = new Point(12, 150);
			this.exampleOut.Name = "exampleOut";
			this.exampleOut.Size = new Size(390, 33);
			this.exampleOut.TabIndex = 58;
			this.logStyle.Location = new Point(72, 45);
			this.logStyle.Name = "logStyle";
			this.logStyle.Size = new Size(330, 20);
			this.logStyle.TabIndex = 59;
			this.logStyle.TextChanged += this.logStyle_TextChanged;
			this.logCustomizeSave.Location = new Point(294, 193);
			this.logCustomizeSave.Name = "logCustomizeSave";
			this.logCustomizeSave.Size = new Size(42, 22);
			this.logCustomizeSave.TabIndex = 60;
			this.logCustomizeSave.Text = "Save";
			this.logCustomizeSave.UseVisualStyleBackColor = true;
			this.logCustomizeSave.Click += this.logCustomizeSave_Click;
			this.guideLabel.Location = new Point(12, 198);
			this.guideLabel.Name = "guideLabel";
			this.guideLabel.Size = new Size(144, 129);
			this.guideLabel.TabIndex = 61;
			this.button1.Location = new Point(342, 193);
			this.button1.Name = "button1";
			this.button1.Size = new Size(60, 22);
			this.button1.TabIndex = 62;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += this.button1_Click;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(191, 105);
			this.label4.Name = "label4";
			this.label4.Size = new Size(93, 13);
			this.label4.TabIndex = 63;
			this.label4.Text = "Output Simulation:";
			this.logSimulation.DropDownStyle = ComboBoxStyle.DropDownList;
			this.logSimulation.FormattingEnabled = true;
			this.logSimulation.Items.AddRange(new object[3]
			{
				"Parked",
				"Full w/ Source",
				"Unknown Source"
			});
			this.logSimulation.Location = new Point(290, 102);
			this.logSimulation.Name = "logSimulation";
			this.logSimulation.Size = new Size(112, 21);
			this.logSimulation.TabIndex = 64;
			this.logSimulation.SelectedIndexChanged += this.logSimulation_SelectedIndexChanged;
			this.guideLabel2.Location = new Point(162, 223);
			this.guideLabel2.Name = "guideLabel2";
			this.guideLabel2.Size = new Size(240, 104);
			this.guideLabel2.TabIndex = 65;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(3, 79);
			this.label5.Name = "label5";
			this.label5.Size = new Size(60, 13);
			this.label5.TabIndex = 66;
			this.label5.Text = "Parked Str:";
			this.label6.AutoSize = true;
			this.label6.Location = new Point(181, 79);
			this.label6.Name = "label6";
			this.label6.Size = new Size(105, 13);
			this.label6.TabIndex = 67;
			this.label6.Text = "Unknown Src String:";
			this.parkedString.Location = new Point(72, 76);
			this.parkedString.Name = "parkedString";
			this.parkedString.Size = new Size(103, 20);
			this.parkedString.TabIndex = 59;
			this.parkedString.TextChanged += this.parkedString_TextChanged;
			this.unkString.Location = new Point(290, 76);
			this.unkString.Name = "unkString";
			this.unkString.Size = new Size(112, 20);
			this.unkString.TabIndex = 59;
			this.unkString.TextChanged += this.unkString_TextChanged;
			this.ignoreParked.AutoSize = true;
			this.ignoreParked.Location = new Point(6, 104);
			this.ignoreParked.Name = "ignoreParked";
			this.ignoreParked.Size = new Size(176, 17);
			this.ignoreParked.TabIndex = 68;
			this.ignoreParked.Text = "Ignore Parked (Retain Last Call)";
			this.ignoreParked.UseVisualStyleBackColor = true;
			this.ignoreParked.CheckedChanged += this.ignoreParked_CheckedChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(414, 334);
			base.Controls.Add(this.ignoreParked);
			base.Controls.Add(this.parkedString);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.guideLabel2);
			base.Controls.Add(this.logSimulation);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.guideLabel);
			base.Controls.Add(this.logCustomizeSave);
			base.Controls.Add(this.unkString);
			base.Controls.Add(this.logStyle);
			base.Controls.Add(this.exampleOut);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.logStylePreset);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "LogOptions";
			this.Text = "Logging Customization";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
