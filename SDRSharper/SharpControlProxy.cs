using System;
using System.ComponentModel;
using System.Windows.Forms;
using SDRSharp.Common;
using SDRSharp.Radio;

namespace SDRSharp
{
    public class SharpControlProxy : ISharpControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SharpControlProxy(MainForm owner)
        {
            this._owner = owner;
            this._owner.PropertyChanged += this.PropertyChangedEventHandler;
        }
        public DetectorType DetectorType
        {
            get
            {
                return this._owner.DetectorType;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.DetectorType = value;
                    }));
                    return;
                }
                this._owner.DetectorType = value;
            }
        }
        public WindowType FilterType
        {
            get
            {
                return this._owner.FilterType;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FilterType = value;
                    }));
                    return;
                }
                this._owner.FilterType = value;
            }
        }
        public int AudioGain
        {
            get
            {
                return this._owner.AudioGain;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.AudioGain = value;
                    }));
                    return;
                }
                this._owner.AudioGain = value;
            }
        }
        public long CenterFrequency
        {
            get
            {
                return this._owner.CenterFrequency;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.CenterFrequency = value;
                    }));
                    return;
                }
                this._owner.CenterFrequency = value;
            }
        }
        public int CWShift
        {
            get
            {
                return this._owner.CWShift;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.CWShift = value;
                    }));
                    return;
                }
                this._owner.CWShift = value;
            }
        }
        public bool FilterAudio
        {
            get
            {
                return this._owner.FilterAudio;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FilterAudio = value;
                    }));
                    return;
                }
                this._owner.FilterAudio = value;
            }
        }
        public int FilterBandwidth
        {
            get
            {
                return this._owner.FilterBandwidth;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FilterBandwidth = value;
                    }));
                    return;
                }
                this._owner.FilterBandwidth = value;
            }
        }
        public int FilterOrder
        {
            get
            {
                return this._owner.FilterOrder;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FilterOrder = value;
                    }));
                    return;
                }
                this._owner.FilterOrder = value;
            }
        }
        public bool FmStereo
        {
            get
            {
                return this._owner.FmStereo;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FmStereo = value;
                    }));
                    return;
                }
                this._owner.FmStereo = value;
            }
        }
        public long Frequency
        {
            get
            {
                return this._owner.Frequency;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.Frequency = value;
                    }));
                    return;
                }
                this._owner.Frequency = value;
            }
        }
        public long FrequencyShift
        {
            get
            {
                return this._owner.FrequencyShift;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FrequencyShift = value;
                    }));
                    return;
                }
                this._owner.FrequencyShift = value;
            }
        }
        public bool FrequencyShiftEnabled
        {
            get
            {
                return this._owner.FrequencyShiftEnabled;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.FrequencyShiftEnabled = value;
                    }));
                    return;
                }
                this._owner.FrequencyShiftEnabled = value;
            }
        }
        public bool UseAgc
        {
            get
            {
                return this._owner.UseAgc;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.UseAgc = value;
                    }));
                    return;
                }
                this._owner.UseAgc = value;
            }
        }
        public bool AgcHang
        {
            get
            {
                return this._owner.AgcHang;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.AgcHang = value;
                    }));
                    return;
                }
                this._owner.AgcHang = value;
            }
        }
        public int AgcThreshold
        {
            get
            {
                return this._owner.AgcThreshold;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.AgcThreshold = value;
                    }));
                    return;
                }
                this._owner.AgcThreshold = value;
            }
        }
        public int AgcDecay
        {
            get
            {
                return this._owner.AgcDecay;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.AgcDecay = value;
                    }));
                    return;
                }
                this._owner.AgcDecay = value;
            }
        }
        public int AgcSlope
        {
            get
            {
                return this._owner.AgcSlope;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.AgcSlope = value;
                    }));
                    return;
                }
                this._owner.AgcSlope = value;
            }
        }
        public bool MarkPeaks
        {
            get
            {
                return this._owner.MarkPeaks;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.MarkPeaks = value;
                    }));
                    return;
                }
                this._owner.MarkPeaks = value;
            }
        }
        public bool SnapToGrid
        {
            get
            {
                return this._owner.SnapToGrid;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.SnapToGrid = value;
                    }));
                    return;
                }
                this._owner.SnapToGrid = value;
            }
        }
        public bool SquelchEnabled
        {
            get
            {
                return this._owner.SquelchEnabled;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.SquelchEnabled = value;
                    }));
                    return;
                }
                this._owner.SquelchEnabled = value;
            }
        }
        public int SquelchThreshold
        {
            get
            {
                return this._owner.SquelchThreshold;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.SquelchThreshold = value;
                    }));
                    return;
                }
                this._owner.SquelchThreshold = value;
            }
        }
        public bool SwapIq
        {
            get
            {
                return this._owner.SwapIq;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.SwapIq = value;
                    }));
                    return;
                }
                this._owner.SwapIq = value;
            }
        }
        public bool IsPlaying
        {
            get
            {
                return this._owner.IsPlaying;
            }
        }
        public int WAttack
        {
            get
            {
                return this._owner.WAttack;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.WAttack = value;
                    }));
                    return;
                }
                this._owner.WAttack = value;
            }
        }
        public int WDecay
        {
            get
            {
                return this._owner.WDecay;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.WDecay = value;
                    }));
                    return;
                }
                this._owner.WDecay = value;
            }
        }
        public int SAttack
        {
            get
            {
                return this._owner.SAttack;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.SAttack = value;
                    }));
                    return;
                }
                this._owner.SAttack = value;
            }
        }
        public int SDecay
        {
            get
            {
                return this._owner.SDecay;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.SDecay = value;
                    }));
                    return;
                }
                this._owner.SDecay = value;
            }
        }
        public bool UseTimeMarkers
        {
            get
            {
                return this._owner.UseTimeMarkers;
            }
            set
            {
                if (this._owner.InvokeRequired)
                {
                    this._owner.Invoke(new MethodInvoker(delegate
                    {
                        this._owner.UseTimeMarkers = value;
                    }));
                    return;
                }
                this._owner.UseTimeMarkers = value;
            }
        }
        public string RdsProgramService
        {
            get
            {
                return this._owner.RdsProgramService;
            }
        }
        public string RdsRadioText
        {
            get
            {
                return this._owner.RdsRadioText;
            }
        }
        public bool IsSquelchOpen
        {
            get
            {
                return this._owner.IsSquelchOpen;
            }
        }
        public int RFBandwidth
        {
            get
            {
                return this._owner.RFBandwidth;
            }
        }
        public int FFTResolution
        {
            get
            {
                return this._owner.FFTResolution;
            }
        }
        public int FFTSkips
        {
            set
            {
                this._owner.FFTSkips = value;
            }
        }
        public void GetSpectrumSnapshot(byte[] destArray)
        {
            this._owner.GetSpectrumSnapshot(destArray);
        }
        public void StartRadio()
        {
            if (this._owner.InvokeRequired)
            {
                this._owner.Invoke(new MethodInvoker(delegate
                {
                    this._owner.StartRadio();
                }));
                return;
            }
            this._owner.StartRadio();
        }
        public void StopRadio()
        {
            if (this._owner.InvokeRequired)
            {
                this._owner.Invoke(new MethodInvoker(delegate
                {
                    this._owner.StopRadio();
                }));
                return;
            }
            this._owner.StopRadio();
        }
        public void RegisterStreamHook(object streamHook, ProcessorType processorType)
        {
            if (this._owner.InvokeRequired)
            {
                this._owner.Invoke(new MethodInvoker(delegate
                {
                    this._owner.RegisterStreamHook(streamHook, processorType);
                }));
                return;
            }
            this._owner.RegisterStreamHook(streamHook, processorType);
        }
        public void UnregisterStreamHook(object streamHook)
        {
            if (this._owner.InvokeRequired)
            {
                this._owner.Invoke(new MethodInvoker(delegate
                {
                    this._owner.UnregisterStreamHook(streamHook);
                }));
                return;
            }
            this._owner.UnregisterStreamHook(streamHook);
        }
        private void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            string propertyName;
            switch (propertyName = e.PropertyName)
            {
                case "AudioGain":
                case "FilterAudio":
                case "Frequency":
                case "CenterFrequency":
                case "FilterBandwidth":
                case "FilterOrder":
                case "FilterType":
                case "CorrectIq":
                case "FrequencyShiftEnabled":
                case "FrequencyShift":
                case "DetectorType":
                case "FmStereo":
                case "CWShift":
                case "SquelchThreshold":
                case "SquelchEnabled":
                case "SnapToGrid":
                case "StepSize":
                case "UseAgc":
                case "UseHang":
                case "AgcDecay":
                case "AgcThreshold":
                case "AgcSlope":
                case "SwapIq":
                case "SAttack":
                case "SDecay":
                case "WAttack":
                case "WDecay":
                case "MarkPeaks":
                case "UseTimeMarkers":
                case "StartRadio":
                case "StopRadio":
                case "FFTResolution":
                case "BFTResolution":
                    {
                        PropertyChangedEventHandler handler = this.PropertyChanged;
                        if (handler != null)
                        {
                            this.PropertyChanged(sender, new PropertyChangedEventArgs(e.PropertyName));
                        }
                        break;
                    }
            }
        }
        private readonly MainForm _owner;
    }
}
