using SDRSharp.Controls;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace SDRSharp.CollapsiblePanel
{
	[Category("Containers")]
	[DesignTimeVisible(true)]
	[Description("Visual Studio like Collapsible Panel")]
	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
	public class CollapsiblePanel : UserControl
	{
		public delegate void DelegateStateChanged();

		private IContainer components;

		private gButton titlePanel;

		private Label labTitle;

		private int _expandedHeight;

		private PanelStateOptions _panelState = PanelStateOptions.Expanded;

		private bool _isCollapsed;

		private bool _fitToParent;

		private CollapsiblePanel _nextPanel;

		[DefaultValue(0)]
		public int ExpandedHeight
		{
			get
			{
				return this._expandedHeight;
			}
			set
			{
				if (value > 0)
				{
					if (base.DesignMode)
					{
						if (this._panelState == PanelStateOptions.Expanded)
						{
							base.SetBounds(base.Location.X, base.Location.Y, base.Size.Width, this.titlePanel.Height + value);
						}
					}
					else
					{
						this._expandedHeight = value;
					}
				}
			}
		}

		public string PanelTitle
		{
			get
			{
				return this.titlePanel.Text;
			}
			set
			{
				this.titlePanel.Text = value;
			}
		}

		[DefaultValue(typeof(PanelStateOptions), "Expanded")]
		public PanelStateOptions PanelState
		{
			get
			{
				return this._panelState;
			}
			set
			{
				this._panelState = value;
				this._isCollapsed = (this._panelState != PanelStateOptions.Collapsed);
				this.ToggleState(null, null);
			}
		}

		[DefaultValue(false)]
		public bool FitToParent
		{
			get
			{
				return this._fitToParent;
			}
			set
			{
				this._fitToParent = value;
				if (this._fitToParent)
				{
					if (base.Parent != null)
					{
						base.Location = new Point(0, base.Location.Y);
						base.Size = new Size(base.Parent.Size.Width, base.Size.Height);
						this.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
					}
				}
				else
				{
					this.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
				}
			}
		}

		public CollapsiblePanel NextPanel
		{
			get
			{
				return this._nextPanel;
			}
			set
			{
				this._nextPanel = value;
				this.MoveNextPanel();
			}
		}

		public event EventHandler StateChanged;

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
			this.labTitle = new Label();
			this.titlePanel = new gButton();
			base.SuspendLayout();
			this.labTitle.BackColor = Color.DimGray;
			this.labTitle.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.labTitle.ForeColor = Color.White;
			this.labTitle.Location = new Point(0, -2);
			this.labTitle.Name = "labTitle";
			this.labTitle.Size = new Size(16, 16);
			this.labTitle.TabIndex = 93;
			this.labTitle.Text = "+";
			this.labTitle.TextAlign = ContentAlignment.TopCenter;
			this.labTitle.MouseDown += this.ToggleState;
			this.titlePanel.Arrow = 0;
			this.titlePanel.Checked = false;
			this.titlePanel.Dock = DockStyle.Top;
			this.titlePanel.Edge = 0.15f;
			this.titlePanel.EndColor = Color.DarkGray;
			this.titlePanel.EndFactor = 0.4f;
			this.titlePanel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.titlePanel.ForeColor = Color.White;
			this.titlePanel.Location = new Point(0, 0);
			this.titlePanel.Margin = new Padding(2, 2, 2, 2);
			this.titlePanel.Name = "titlePanel";
			this.titlePanel.NoBorder = true;
			this.titlePanel.NoLed = true;
			this.titlePanel.RadioButton = false;
			this.titlePanel.Radius = 1;
			this.titlePanel.RadiusB = 0;
			this.titlePanel.Size = new Size(150, 18);
			this.titlePanel.StartColor = Color.Black;
			this.titlePanel.StartFactor = 0.8f;
			this.titlePanel.TabIndex = 92;
			this.titlePanel.Text = "Caption";
			this.titlePanel.CheckedChanged += this.ToggleState;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.labTitle);
			base.Controls.Add(this.titlePanel);
			base.Name = "CollapsiblePanel";
			base.ResumeLayout(false);
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.titlePanel.ForeColor = this.ForeColor;
		}

		public CollapsiblePanel()
		{
			this.InitializeComponent();
			base.Load += this.CollapsiblePanel_Load;
			base.SizeChanged += this.CollapsiblePanel_SizeChanged;
			base.LocationChanged += this.CollapsiblePanel_LocationChanged;
		}

		private void CollapsiblePanel_Load(object sender, EventArgs e)
		{
			this._isCollapsed = (this._panelState == PanelStateOptions.Collapsed);
			if (this._isCollapsed)
			{
				this.labTitle.Text = "+";
			}
			else
			{
				this.labTitle.Text = "-";
			}
		}

		private void CollapsiblePanel_SizeChanged(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				if (this._panelState == PanelStateOptions.Expanded)
				{
					this._expandedHeight = base.Height;
				}
				else
				{
					base.SetBounds(base.Location.X, base.Location.Y, base.Size.Width, this.titlePanel.Height);
				}
				if (base.Parent != null && base.Parent.Size.Width != base.Size.Width)
				{
					this.FitToParent = false;
				}
			}
			this.MoveNextPanel();
		}

		private void CollapsiblePanel_LocationChanged(object sender, EventArgs e)
		{
			if (base.DesignMode && base.Location.X > 0)
			{
				this.FitToParent = false;
			}
			this.MoveNextPanel();
		}

		private void ToggleState(object sender, EventArgs e)
		{
			if (this._isCollapsed)
			{
				base.SetBounds(base.Location.X, base.Location.Y, base.Size.Width, this._expandedHeight);
			}
			else
			{
				base.SetBounds(base.Location.X, base.Location.Y, base.Size.Width, this.titlePanel.Height);
			}
			this._isCollapsed = !this._isCollapsed;
			if (this._isCollapsed)
			{
				this._panelState = PanelStateOptions.Collapsed;
				this.labTitle.Text = "+";
			}
			else
			{
				this._panelState = PanelStateOptions.Expanded;
				this.labTitle.Text = "-";
			}
			if (!base.DesignMode && this.StateChanged != null)
			{
				this.StateChanged(this, new EventArgs());
			}
		}

		private void MoveNextPanel()
		{
			if (this._nextPanel != null)
			{
				this._nextPanel.Location = new Point(this._nextPanel.Location.X, base.Location.Y + base.Size.Height);
			}
		}
	}
}
