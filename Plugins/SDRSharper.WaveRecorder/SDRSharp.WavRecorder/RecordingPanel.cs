using SDRSharp.Common;
using SDRSharp.Controls;
using SDRSharp.Radio;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SDRSharp.WavRecorder
{
	public class RecordingPanel : UserControl
	{
		private IContainer components;

		private Timer recDisplayTimer;

		private Label sampleFormatLbl;

		private Label label3;

		private Label label1;

		private gButton recBtn;

		private gButton audioCb;

		private gButton basebandCb;

		private gCombo sampleFormatCombo;

		private Panel panel1;

		private gButton butPath;

		private gTextBox txtPath;

		private gLabel skippedBufferCountLbl;

		private gLabel sizeLbl;

		private gLabel durationLbl;

		private gButton showBtn;

		private readonly ISharpControl _control;

		private readonly RecordingIQObserver _iqObserver = new RecordingIQObserver();

		private readonly RecordingAudioProcessor _audioProcessor = new RecordingAudioProcessor();

		private readonly SimpleRecorder _audioRecorder;

		private readonly SimpleRecorder _basebandRecorder;

		private WavSampleFormat _wavSampleFormat;

		private DateTime _startTime;

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
			this.recDisplayTimer = new Timer(this.components);
			this.sampleFormatLbl = new Label();
			this.label3 = new Label();
			this.label1 = new Label();
			this.panel1 = new Panel();
			this.skippedBufferCountLbl = new gLabel();
			this.sizeLbl = new gLabel();
			this.durationLbl = new gLabel();
			this.butPath = new gButton();
			this.txtPath = new gTextBox();
			this.recBtn = new gButton();
			this.sampleFormatCombo = new gCombo();
			this.audioCb = new gButton();
			this.basebandCb = new gButton();
			this.showBtn = new gButton();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.recDisplayTimer.Interval = 1000;
			this.recDisplayTimer.Tick += this.recDisplayTimer_Tick;
			this.sampleFormatLbl.ForeColor = Color.Orange;
			this.sampleFormatLbl.Location = new Point(0, 12);
			this.sampleFormatLbl.Name = "sampleFormatLbl";
			this.sampleFormatLbl.Size = new Size(72, 13);
			this.sampleFormatLbl.TabIndex = 5;
			this.sampleFormatLbl.Text = "Format";
			this.label3.ForeColor = Color.Orange;
			this.label3.Location = new Point(0, 36);
			this.label3.Name = "label3";
			this.label3.Size = new Size(72, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Length/size";
			this.label1.ForeColor = Color.Orange;
			this.label1.Location = new Point(1, 58);
			this.label1.Name = "label1";
			this.label1.Size = new Size(71, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Dropped bufs";
			this.panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.panel1.BackColor = Color.FromArgb(64, 64, 64);
			this.panel1.Controls.Add(this.showBtn);
			this.panel1.Controls.Add(this.skippedBufferCountLbl);
			this.panel1.Controls.Add(this.sizeLbl);
			this.panel1.Controls.Add(this.durationLbl);
			this.panel1.Controls.Add(this.butPath);
			this.panel1.Controls.Add(this.txtPath);
			this.panel1.Controls.Add(this.recBtn);
			this.panel1.Controls.Add(this.sampleFormatCombo);
			this.panel1.Controls.Add(this.audioCb);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.basebandCb);
			this.panel1.Controls.Add(this.sampleFormatLbl);
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(198, 166);
			this.panel1.TabIndex = 13;
			this.skippedBufferCountLbl.ForeColor = Color.Yellow;
			this.skippedBufferCountLbl.Location = new Point(78, 57);
			this.skippedBufferCountLbl.Name = "skippedBufferCountLbl";
			this.skippedBufferCountLbl.Size = new Size(52, 20);
			this.skippedBufferCountLbl.TabIndex = 17;
			this.skippedBufferCountLbl.Text = "0";
			this.sizeLbl.ForeColor = Color.Yellow;
			this.sizeLbl.Location = new Point(132, 33);
			this.sizeLbl.Name = "sizeLbl";
			this.sizeLbl.Size = new Size(59, 20);
			this.sizeLbl.TabIndex = 16;
			this.sizeLbl.Text = "2048.0 Mb";
			this.durationLbl.ForeColor = Color.Yellow;
			this.durationLbl.Location = new Point(78, 33);
			this.durationLbl.Name = "durationLbl";
			this.durationLbl.Size = new Size(52, 20);
			this.durationLbl.TabIndex = 15;
			this.durationLbl.Text = "00:00:00";
			this.butPath.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.butPath.Arrow = 0;
			this.butPath.Checked = false;
			this.butPath.Edge = 0.15f;
			this.butPath.EndColor = Color.White;
			this.butPath.EndFactor = 0.14f;
			this.butPath.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.butPath.ForeColor = Color.Orange;
			this.butPath.Location = new Point(174, 110);
			this.butPath.Name = "butPath";
			this.butPath.NoBorder = false;
			this.butPath.NoLed = true;
			this.butPath.RadioButton = false;
			this.butPath.Radius = 4;
			this.butPath.RadiusB = 0;
			this.butPath.Size = new Size(20, 20);
			this.butPath.StartColor = Color.Black;
			this.butPath.StartFactor = 0.3f;
			this.butPath.TabIndex = 14;
			this.butPath.Text = "...";
			this.butPath.CheckedChanged += this.butPath_CheckedChanged;
			this.txtPath.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtPath.Location = new Point(3, 110);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new Size(165, 20);
			this.txtPath.TabIndex = 13;
			this.txtPath.Text = "path";
			this.recBtn.Arrow = 0;
			this.recBtn.Checked = false;
			this.recBtn.Edge = 0.15f;
			this.recBtn.EndColor = Color.White;
			this.recBtn.EndFactor = 0.14f;
			this.recBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.recBtn.ForeColor = Color.FromArgb(255, 128, 128);
			this.recBtn.Location = new Point(143, 68);
			this.recBtn.Name = "recBtn";
			this.recBtn.NoBorder = false;
			this.recBtn.NoLed = false;
			this.recBtn.RadioButton = false;
			this.recBtn.Radius = 6;
			this.recBtn.RadiusB = 0;
			this.recBtn.Size = new Size(44, 30);
			this.recBtn.StartColor = Color.Black;
			this.recBtn.StartFactor = 0.3f;
			this.recBtn.TabIndex = 11;
			this.recBtn.Text = "Rec.";
			this.recBtn.CheckedChanged += this.recBtn_Click;
			this.sampleFormatCombo.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.sampleFormatCombo.BackColor = Color.FromArgb(64, 64, 64);
			this.sampleFormatCombo.ForeColor = Color.Yellow;
			this.sampleFormatCombo.Items.Add("8 Bit PCM");
			this.sampleFormatCombo.Items.Add("16 Bit PCM");
			this.sampleFormatCombo.Items.Add("32 Bit IEEE Float");
			this.sampleFormatCombo.Location = new Point(78, 9);
			this.sampleFormatCombo.Name = "sampleFormatCombo";
			this.sampleFormatCombo.SelectedIndex = -1;
			this.sampleFormatCombo.Size = new Size(117, 20);
			this.sampleFormatCombo.TabIndex = 12;
			this.sampleFormatCombo.Text = "gCombo1";
			this.sampleFormatCombo.ToolTip = null;
			this.sampleFormatCombo.SelectedIndexChanged += this.sampleFormatCombo_SelectedIndexChanged;
			this.audioCb.Arrow = 99;
			this.audioCb.Checked = false;
			this.audioCb.Edge = 0.15f;
			this.audioCb.EndColor = Color.White;
			this.audioCb.EndFactor = 0.14f;
			this.audioCb.ForeColor = Color.Orange;
			this.audioCb.Location = new Point(75, 84);
			this.audioCb.Name = "audioCb";
			this.audioCb.NoBorder = false;
			this.audioCb.NoLed = false;
			this.audioCb.RadioButton = false;
			this.audioCb.Radius = 6;
			this.audioCb.RadiusB = 0;
			this.audioCb.Size = new Size(54, 20);
			this.audioCb.StartColor = Color.Black;
			this.audioCb.StartFactor = 0.3f;
			this.audioCb.TabIndex = 10;
			this.audioCb.Text = "Audio";
			this.basebandCb.Arrow = 99;
			this.basebandCb.Checked = true;
			this.basebandCb.Edge = 0.15f;
			this.basebandCb.EndColor = Color.White;
			this.basebandCb.EndFactor = 0.14f;
			this.basebandCb.ForeColor = Color.Orange;
			this.basebandCb.Location = new Point(10, 84);
			this.basebandCb.Name = "basebandCb";
			this.basebandCb.NoBorder = false;
			this.basebandCb.NoLed = false;
			this.basebandCb.RadioButton = false;
			this.basebandCb.Radius = 6;
			this.basebandCb.RadiusB = 0;
			this.basebandCb.Size = new Size(56, 20);
			this.basebandCb.StartColor = Color.Black;
			this.basebandCb.StartFactor = 0.3f;
			this.basebandCb.TabIndex = 9;
			this.basebandCb.Text = "B-band";
			this.showBtn.Arrow = 0;
			this.showBtn.Checked = true;
			this.showBtn.Edge = 0.15f;
			this.showBtn.EndColor = Color.White;
			this.showBtn.EndFactor = 0.14f;
			this.showBtn.ForeColor = Color.Orange;
			this.showBtn.Location = new Point(2, 137);
			this.showBtn.Name = "showBtn";
			this.showBtn.NoBorder = false;
			this.showBtn.NoLed = true;
			this.showBtn.RadioButton = false;
			this.showBtn.Radius = 6;
			this.showBtn.RadiusB = 0;
			this.showBtn.Size = new Size(40, 20);
			this.showBtn.StartColor = Color.Black;
			this.showBtn.StartFactor = 0.3f;
			this.showBtn.TabIndex = 18;
			this.showBtn.Text = "Show";
			this.showBtn.CheckedChanged += this.showBtn_CheckedChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.panel1);
			base.Name = "RecordingPanel";
			base.Size = new Size(198, 186);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		public RecordingPanel(ISharpControl control)
		{
			this.InitializeComponent();
			this._control = control;
			this._audioProcessor.Enabled = false;
			this._iqObserver.Enabled = false;
			this._control.RegisterStreamHook(this._iqObserver, ProcessorType.RawIQ);
			this._control.RegisterStreamHook(this._audioProcessor, ProcessorType.FilteredAudioOutput);
			this._audioRecorder = new SimpleRecorder(this._audioProcessor);
			this._basebandRecorder = new SimpleRecorder(this._iqObserver);
			this._control.PropertyChanged += this.PropertyChangedHandler;
			this.InitializeGUI();
			this.ConfigureGUI();
		}

		private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			string propertyName;
			if ((propertyName = e.PropertyName) != null)
			{
				if (!(propertyName == "StartRadio"))
				{
					if (propertyName == "StopRadio")
					{
						if (this._audioRecorder.IsRecording)
						{
							this._audioRecorder.StopRecording();
						}
						if (this._basebandRecorder.IsRecording)
						{
							this._basebandRecorder.StopRecording();
						}
						this.ConfigureGUI();
					}
				}
				else
				{
					this.ConfigureGUI();
				}
			}
		}

		private void recBtn_Click(object sender, EventArgs e)
		{
			if (!this.recBtn.Checked)
			{
				if (this._audioRecorder.IsRecording)
				{
					this._audioRecorder.StopRecording();
				}
				if (this._basebandRecorder.IsRecording)
				{
					this._basebandRecorder.StopRecording();
				}
			}
			else if (!this._basebandRecorder.IsRecording && !this._audioRecorder.IsRecording)
			{
				if (!Directory.Exists(this.txtPath.Text.Trim()))
				{
					if (this.recBtn.Checked)
					{
						MessageBox.Show("Folder '" + this.txtPath.Text + "' does not exist on this computer,\n please select another destination folder.");
					}
				}
				else
				{
					this.PrepareRecorder();
					try
					{
						if (this.audioCb.Checked)
						{
							this._audioRecorder.StartRecording();
						}
						if (this.basebandCb.Checked)
						{
							this._basebandRecorder.StartRecording();
						}
						this._startTime = DateTime.Now;
					}
					catch (Exception ex)
					{
						MessageBox.Show("Unable to start recording", "Error=" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this._audioRecorder.StopRecording();
						this._basebandRecorder.StopRecording();
					}
				}
			}
			this.ConfigureGUI();
		}

		private void recDisplayTimer_Tick(object sender, EventArgs e)
		{
			TimeSpan timeSpan = DateTime.Now - this._startTime;
			float num = (float)this._audioRecorder.BytesWritten * 9.536743E-07f + (float)this._basebandRecorder.BytesWritten * 9.536743E-07f;
			this.durationLbl.Text = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
			this.sizeLbl.Text = $"{num:f1} Mb";
			this.skippedBufferCountLbl.Text = $"{this._basebandRecorder.SkippedBuffers + this._audioRecorder.SkippedBuffers}";
			bool flag = false;
			if (this._audioRecorder.IsStreamFull)
			{
				this._audioRecorder.StopRecording();
				int num2 = this._basebandRecorder.FileName.LastIndexOf("AF");
				string text = this._basebandRecorder.FileName.Substring(num2 + 2, 2);
				int num3 = (text == ".w") ? 1 : (int.Parse(text) + 1);
				if (num3 <= 99)
				{
					this._basebandRecorder.FileName = this._basebandRecorder.FileName.Substring(0, num2 + 2) + num3.ToString("00") + ".wav";
					this._basebandRecorder.StartRecording();
				}
				flag = true;
			}
			if (this._basebandRecorder.IsStreamFull)
			{
				this._basebandRecorder.StopRecording();
				int num4 = this._basebandRecorder.FileName.LastIndexOf("IQ");
				string text2 = this._basebandRecorder.FileName.Substring(num4 + 2, 2);
				int num5 = (text2 == ".w") ? 1 : (int.Parse(text2) + 1);
				if (num5 <= 99)
				{
					this._basebandRecorder.FileName = this._basebandRecorder.FileName.Substring(0, num4 + 2) + num5.ToString("00") + ".wav";
					this._basebandRecorder.StartRecording();
				}
				flag = true;
			}
			if (flag)
			{
				this.ConfigureGUI();
			}
		}

		private void sampleFormatCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._wavSampleFormat = (WavSampleFormat)this.sampleFormatCombo.SelectedIndex;
		}

		private void InitializeGUI()
		{
			this.sampleFormatCombo.SelectedIndex = 1;
			this.sampleFormatCombo_SelectedIndexChanged(null, null);
			this.txtPath.Text = Utils.GetStringSetting("RecordingPath", Application.ExecutablePath);
		}

		private void ConfigureGUI()
		{
			if (this._control.IsPlaying)
			{
				this.recBtn.Checked = (this._audioRecorder.IsRecording || this._basebandRecorder.IsRecording);
				this.recBtn.Text = ((this._audioRecorder.IsRecording || this._basebandRecorder.IsRecording) ? "Stop" : "Rec.");
				this.recDisplayTimer.Enabled = (this._audioRecorder.IsRecording || this._basebandRecorder.IsRecording);
			}
			else
			{
				this.recBtn.Checked = false;
				this.recDisplayTimer.Enabled = false;
				this.recBtn.Text = "Rec.";
				this.durationLbl.Text = "00:00:00";
				this.sizeLbl.Text = "0 MB";
				this.skippedBufferCountLbl.Text = "0";
			}
			this.sampleFormatCombo.Enabled = (!this._audioRecorder.IsRecording && !this._basebandRecorder.IsRecording);
			this.audioCb.Enabled = (!this._audioRecorder.IsRecording && !this._basebandRecorder.IsRecording);
			this.basebandCb.Enabled = (!this._audioRecorder.IsRecording && !this._basebandRecorder.IsRecording);
		}

		private string MakeFileName(RecordingMode mode, DateTime time)
		{
			long num = (mode == RecordingMode.Baseband) ? Math.Abs(this._control.CenterFrequency) : Math.Max(this._control.Frequency, 0L);
			long num2 = (num >= 1000) ? (num / 1000) : num;
			string text = (num >= 1000) ? "kHz" : "Hz";
			string text2 = (mode == RecordingMode.Baseband) ? "IQ" : "AF";
			string text3 = time.ToString("yyyyMMdd");
			string text4 = time.ToString("HHmmssZ");
			string text5 = this.txtPath.Text.Trim();
			if (text5.Length == 0)
			{
				text5 = Path.GetDirectoryName(Application.ExecutablePath);
			}
			return Path.Combine(text5 ?? "", $"SDRSharp_{text3}_{text4}_{num2}{text}_{text2}.wav");
		}

		private void PrepareRecorder()
		{
			DateTime utcNow = DateTime.UtcNow;
			if (this.audioCb.Checked)
			{
				this._audioRecorder.SampleRate = this._audioProcessor.SampleRate;
				this._audioRecorder.FileName = this.MakeFileName(RecordingMode.Audio, utcNow);
				this._audioRecorder.Format = this._wavSampleFormat;
			}
			if (this.basebandCb.Checked)
			{
				this._basebandRecorder.SampleRate = this._iqObserver.SampleRate;
				this._basebandRecorder.FileName = this.MakeFileName(RecordingMode.Baseband, utcNow);
				this._basebandRecorder.Format = this._wavSampleFormat;
			}
		}

		public void AbortRecording()
		{
			if (this._audioRecorder != null)
			{
				this._audioRecorder.StopRecording();
			}
			if (this._basebandRecorder != null)
			{
				this._basebandRecorder.StopRecording();
			}
		}

		private void butPath_CheckedChanged(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Select folder to save recordings";
			saveFileDialog.InitialDirectory = this.txtPath.Text;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.FileName = "Save Here";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.txtPath.Text = Path.GetDirectoryName(saveFileDialog.FileName);
				Utils.SaveSetting("RecordingPath", this.txtPath.Text);
			}
		}

		private void showBtn_CheckedChanged(object sender, EventArgs e)
		{
			Process.Start("explorer.exe", this.txtPath.Text);
		}
	}
}
