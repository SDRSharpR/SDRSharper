using System;

namespace SDRSharp.Radio
{
	public sealed class Vfo
	{
		private const float TimeConst = 0.001f;

		public const int DefaultCwSideTone = 600;

		public const int DefaultSSBBandwidth = 2400;

		public const int DefaultWFMBandwidth = 180000;

		public const int MinSSBAudioFrequency = 400;

		public const int MinBCAudioFrequency = 20;

		public const int MaxBCAudioFrequency = 16000;

		public const int MaxNFMBandwidth = 15000;

		public const int MinNFMAudioFrequency = 300;

		public const int EnvelopeFrequency = 6000;

		private readonly AutomaticGainControl _agc = new AutomaticGainControl();

		private readonly AutomaticGainControl _agcX = new AutomaticGainControl();

		private readonly AutomaticGainControl _agcY = new AutomaticGainControl();

		private readonly AutomaticGainControl _agcEnv = new AutomaticGainControl();

		private readonly DownConverter _downConverter = new DownConverter();

		private readonly DownConverter _downConverter2 = new DownConverter();

		private readonly AmDetector _amDetector = new AmDetector();

		private readonly SamDetector _samDetector = new SamDetector();

		private readonly FmDetector _fmDetector = new FmDetector();

		private readonly PmDetector _pmDetector = new PmDetector();

		private readonly SsbDetector _lsbDetector = new SsbDetector(SsbDetector.Mode.LSB);

		private readonly SsbDetector _usbDetector = new SsbDetector(SsbDetector.Mode.USB);

		private readonly CwDetector _cwDetector = new CwDetector();

		private readonly DsbDetector _dsbDetector = new DsbDetector();

		private readonly StereoDecoder _stereoDecoder = new StereoDecoder();

		private readonly RdsDecoder _rdsDecoder = new RdsDecoder();

		private IQFirFilter _realIqFilter;

		private CpxFirFilter _cpxIqFilter;

		private readonly FirFilter _audioFilter = new FirFilter();

		private readonly FirFilter _rFilter = new FirFilter();

		private readonly FirFilter _lFilter = new FirFilter();

		private readonly VfoHookManager _hookManager;

		private CpxBandFilter _envFilter = new CpxBandFilter();

		private CpxBandFilter[] _notchFilter = new CpxBandFilter[4]
		{
			new CpxBandFilter(),
			new CpxBandFilter(),
			new CpxBandFilter(),
			new CpxBandFilter()
		};

		private int _notch = -1;

		private int _notchWidth;

		private int _notchFrequency;

		private DcRemover _dcRemover = new DcRemover(0.001f);

		private IQDecimator _baseBandDecimator;

		private IQDecimator _envelopeDecimator;

		private DetectorType _detectorType;

		private DetectorType _actualDetectorType;

		private WindowType _windowType;

		private double _sampleRate;

		private int _bandwidth;

		private int _frequency;

		private int _frequencyOffset;

		private int _filterOrder;

		private bool _needNewFilters;

		private int _decimationStageCount;

		private int _baseBandDecimationStageCount;

		private int _audioDecimationStageCount;

		private int _envelopeDecimationStageCount;

		private bool _needNewDecimators;

		private bool _decimationModeHasChanged;

		private int _cwToneShift;

		private bool _needConfigure;

		private bool _useAgc;

		private float _agcThreshold;

		private float _agcDecay;

		private float _agcSlope;

		private bool _agcUseHang;

		private int _squelchThreshold;

		private bool _stereo;

		private bool _filterAudio;

		private UnsafeBuffer _rawAudioBuffer;

		private unsafe float* _rawAudioPtr;

		private UnsafeBuffer _envBuf;

		private unsafe Complex* _envPtr;

		private UnsafeBuffer _rBuf;

		private UnsafeBuffer _lBuf;

		private unsafe float* _rPtr;

		private unsafe float* _lPtr;

		private DemodType _demodX;

		private DemodType _demodY;

		private float _rfGain = 0.05f;

		public DetectorType DetectorType
		{
			get
			{
				return this._detectorType;
			}
			set
			{
				if (value != this._detectorType)
				{
					this._decimationModeHasChanged = ((this._detectorType == DetectorType.WFM && value != DetectorType.WFM) || (this._detectorType != DetectorType.WFM && value == DetectorType.WFM));
					if (this._decimationModeHasChanged)
					{
						this._needNewDecimators = true;
					}
					this._detectorType = value;
					this._needConfigure = true;
				}
			}
		}

		public double RFgain
		{
			set
			{
				this._rfGain = (float)Math.Pow(10.0, value / 20.0);
			}
		}

		public float FreqErr => this._pmDetector.FreqErr;

		public DemodType DemodX
		{
			get
			{
				return this._demodX;
			}
			set
			{
				this._demodX = value;
			}
		}

		public DemodType DemodY
		{
			get
			{
				return this._demodY;
			}
			set
			{
				this._demodY = value;
			}
		}

		public int Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				if (this._frequency != value)
				{
					this._frequency = value;
					this._needConfigure = true;
				}
			}
		}

		public double DownConverterFrequency => this._downConverter.Frequency;

		public int FilterOrder
		{
			get
			{
				return this._filterOrder;
			}
			set
			{
				if (this._filterOrder != value)
				{
					this._filterOrder = value;
					this._needNewFilters = true;
					this._needConfigure = true;
				}
			}
		}

		public double SampleRate
		{
			get
			{
				return this._sampleRate;
			}
			set
			{
				if (this._sampleRate != value)
				{
					this._sampleRate = value;
					this._needNewDecimators = true;
					this._needConfigure = true;
				}
			}
		}

		public WindowType WindowType
		{
			get
			{
				return this._windowType;
			}
			set
			{
				if (this._windowType != value)
				{
					this._windowType = value;
					this._needNewFilters = true;
					this._needConfigure = true;
				}
			}
		}

		public int Bandwidth
		{
			get
			{
				return this._bandwidth;
			}
			set
			{
				if (this._bandwidth != value)
				{
					this._bandwidth = value;
					this._needNewFilters = true;
					this._needConfigure = true;
				}
			}
		}

		public int FrequencyOffset
		{
			get
			{
				return this._frequency;
			}
			set
			{
				if (this._frequencyOffset != value)
				{
					this._frequencyOffset = value;
					this._needConfigure = true;
				}
			}
		}

		public bool UseAGC
		{
			get
			{
				return this._useAgc;
			}
			set
			{
				this._useAgc = value;
			}
		}

		public float AgcThreshold
		{
			get
			{
				return this._agcThreshold;
			}
			set
			{
				if (this._agcThreshold != value)
				{
					this._agcThreshold = value;
					this._needConfigure = true;
				}
			}
		}

		public float AgcDecay
		{
			get
			{
				return this._agcDecay;
			}
			set
			{
				if (this._agcDecay != value)
				{
					this._agcDecay = value;
					this._needConfigure = true;
				}
			}
		}

		public float AgcSlope
		{
			get
			{
				return this._agcSlope;
			}
			set
			{
				if (this._agcSlope != value)
				{
					this._agcSlope = value;
					this._needConfigure = true;
				}
			}
		}

		public bool AgcHang
		{
			get
			{
				return this._agcUseHang;
			}
			set
			{
				if (this._agcUseHang != value)
				{
					this._agcUseHang = value;
					this._needConfigure = true;
				}
			}
		}

		public int SquelchThreshold
		{
			get
			{
				return this._squelchThreshold;
			}
			set
			{
				if (this._squelchThreshold != value)
				{
					this._squelchThreshold = value;
					this._needConfigure = true;
				}
			}
		}

		public bool IsSquelchOpen
		{
			get
			{
				if (this._actualDetectorType == DetectorType.NFM && this._fmDetector.IsSquelchOpen)
				{
					goto IL_0042;
				}
				if (this._actualDetectorType == DetectorType.AM && this._amDetector.IsSquelchOpen)
				{
					goto IL_0042;
				}
				if (this._actualDetectorType == DetectorType.SAM)
				{
					return this._amDetector.IsSquelchOpen;
				}
				return false;
				IL_0042:
				return true;
			}
		}

		public int DecimationStageCount
		{
			get
			{
				return this._decimationStageCount;
			}
			set
			{
				if (this._decimationStageCount != value)
				{
					this._decimationStageCount = Math.Abs(value);
					this._needNewDecimators = true;
					this._needConfigure = true;
					this.setDecimationCount();
				}
			}
		}

		public int BaseBandDecimationStageCount => this._baseBandDecimationStageCount;

		public int EnvelopeDecimationStageCount => this._envelopeDecimationStageCount;

		public int CWToneShift
		{
			get
			{
				return this._cwToneShift;
			}
			set
			{
				if (this._cwToneShift != value)
				{
					this._cwToneShift = value;
					this._needNewFilters = true;
					this._needConfigure = true;
				}
			}
		}

		public bool Stereo
		{
			get
			{
				return this._stereo;
			}
			set
			{
				if (this._stereo != value)
				{
					this._stereo = value;
					this._needConfigure = true;
				}
			}
		}

		public bool SignalIsStereo
		{
			get
			{
				if (this._actualDetectorType == DetectorType.WFM && this._stereo)
				{
					return this._stereoDecoder.IsPllLocked;
				}
				return false;
			}
		}

		public string RdsStationName => this._rdsDecoder.ProgramService;

		public string RdsStationText => this._rdsDecoder.RadioText;

		public bool FilterAudio
		{
			get
			{
				return this._filterAudio;
			}
			set
			{
				this._filterAudio = value;
				this._stereoDecoder.DoFiltering = value;
			}
		}

		public Vfo(VfoHookManager hookManager = null)
		{
			this._hookManager = hookManager;
			this._bandwidth = 2400;
			this._filterOrder = 500;
			this._needConfigure = true;
		}

		public void SetNotch(int notch, int offset, int width)
		{
			if (this._notchFilter[notch] != null && this._notch == notch && this._notchFrequency == this._frequency + offset && this._notchWidth == width)
			{
				return;
			}
			this._notch = notch;
			this._notchFrequency = offset;
			this._notchWidth = width;
			this._needConfigure = true;
			this._needNewFilters = true;
		}

		public void RdsReset()
		{
			this._rdsDecoder.Reset();
		}

		private void configure()
		{
			this._actualDetectorType = this._detectorType;
			this._downConverter.SampleRate = this._sampleRate;
			this._downConverter.Frequency = (double)this._frequency;
			this._downConverter2.SampleRate = this._sampleRate;
			this._downConverter2.Frequency = (double)(this._frequency - 6000);
			if (this._needNewDecimators)
			{
				this.setDecimationCount();
				this._baseBandDecimator = new IQDecimator(this._baseBandDecimationStageCount, this._sampleRate, false, Utils.ProcessorCount > 1);
				this._envelopeDecimator = new IQDecimator(this._envelopeDecimationStageCount, this._sampleRate, false, Utils.ProcessorCount > 1);
				if (this._hookManager != null)
				{
					this._hookManager.SetProcessorSampleRate(ProcessorType.RawIQ, this._sampleRate);
					this._hookManager.SetProcessorSampleRate(ProcessorType.FrequencyTranslatedIQ, this._sampleRate);
					this._hookManager.SetProcessorSampleRate(ProcessorType.DecimatedAndFilteredIQ, this._sampleRate / (double)(1 << this._baseBandDecimationStageCount));
					this._hookManager.SetProcessorSampleRate(ProcessorType.DemodulatorOutput, this._sampleRate / (double)(1 << this._baseBandDecimationStageCount));
					this._hookManager.SetProcessorSampleRate(ProcessorType.FilteredAudioOutput, this._sampleRate / (double)(1 << this._decimationStageCount));
				}
				this._needNewFilters = true;
			}
			if (this._needNewFilters)
			{
				this.initFilters();
				this._samDetector.Range = (float)(this._bandwidth / 2);
			}
			if (this._needNewDecimators)
			{
				double num = this._sampleRate / Math.Pow(2.0, (double)this._baseBandDecimationStageCount);
				this._usbDetector.SampleRate = num;
				this._lsbDetector.SampleRate = num;
				this._cwDetector.SampleRate = num;
				this._fmDetector.SampleRate = num;
				this._pmDetector.SampleRate = num;
				this._samDetector.SampleRate = (float)num;
				this._samDetector.BandWidth = 500f;
				this._samDetector.LockTime = 2f;
				this._samDetector.LockTreshold = 3f;
				this._stereoDecoder.Configure(this._fmDetector.SampleRate, this._audioDecimationStageCount);
				this._rdsDecoder.SampleRate = this._fmDetector.SampleRate;
			}
			this._fmDetector.SquelchThreshold = this._squelchThreshold;
			this._amDetector.SquelchThreshold = this._squelchThreshold;
			this._stereoDecoder.ForceMono = !this._stereo;
			switch (this._actualDetectorType)
			{
			case DetectorType.AM:
			case DetectorType.DSB:
			case DetectorType.SAM:
				this._downConverter.Frequency += (double)this._frequencyOffset;
				break;
			case DetectorType.USB:
				this._usbDetector.BfoFrequency = -this._bandwidth / 2;
				this._downConverter.Frequency -= (double)this._usbDetector.BfoFrequency;
				break;
			case DetectorType.LSB:
				this._lsbDetector.BfoFrequency = -this._bandwidth / 2;
				this._downConverter.Frequency += (double)this._lsbDetector.BfoFrequency;
				break;
			case DetectorType.CW:
				this._cwDetector.BfoFrequency = this._cwToneShift;
				this._downConverter.Frequency += (double)this._frequencyOffset;
				break;
			case DetectorType.NFM:
				this._fmDetector.Mode = FmMode.Narrow;
				this._downConverter.Frequency += (double)this._frequencyOffset;
				break;
			case DetectorType.WFM:
				this._fmDetector.Mode = FmMode.Wide;
				this._downConverter.Frequency += (double)this._frequencyOffset;
				break;
			}
			this._agc.SampleRate = this._sampleRate / Math.Pow(2.0, (double)this._decimationStageCount);
			this._agc.Decay = this._agcDecay;
			this._agc.Slope = this._agcSlope;
			this._agc.Threshold = this._agcThreshold;
			this._agc.UseHang = this._agcUseHang;
			this._agcX.SampleRate = this._sampleRate / Math.Pow(2.0, (double)this._decimationStageCount);
			this._agcX.Decay = this._agcDecay;
			this._agcX.Slope = this._agcSlope;
			this._agcX.Threshold = this._agcThreshold;
			this._agcX.UseHang = this._agcUseHang;
			this._agcY.SampleRate = this._sampleRate / Math.Pow(2.0, (double)this._decimationStageCount);
			this._agcY.Decay = this._agcDecay;
			this._agcY.Slope = this._agcSlope;
			this._agcY.Threshold = this._agcThreshold;
			this._agcY.UseHang = this._agcUseHang;
			this._agcEnv.SampleRate = this._sampleRate / Math.Pow(2.0, (double)this._decimationStageCount);
			this._agcEnv.Decay = this._agcDecay;
			this._agcEnv.Slope = this._agcSlope;
			this._agcEnv.Threshold = this._agcThreshold;
			this._agcEnv.UseHang = this._agcUseHang;
			this._decimationModeHasChanged = false;
			this._needNewDecimators = false;
			this._needNewFilters = false;
		}

		private void setDecimationCount()
		{
			if (this._actualDetectorType == DetectorType.WFM)
			{
				double num = this._sampleRate / Math.Pow(2.0, (double)this._decimationStageCount);
				this._audioDecimationStageCount = 0;
				while (num * Math.Pow(2.0, (double)this._audioDecimationStageCount) < 180000.0 && this._audioDecimationStageCount < this._decimationStageCount)
				{
					this._audioDecimationStageCount++;
				}
				this._baseBandDecimationStageCount = this._decimationStageCount - this._audioDecimationStageCount;
			}
			else
			{
				this._baseBandDecimationStageCount = this._decimationStageCount;
				this._audioDecimationStageCount = 0;
			}
			long num2 = (long)this._sampleRate;
			this._envelopeDecimationStageCount = 0;
			while (this._envelopeDecimationStageCount < 16)
			{
				num2 /= 2;
				if (num2 < 48000)
				{
					break;
				}
				this._envelopeDecimationStageCount++;
			}
			Console.WriteLine("decimation=" + this._decimationStageCount.ToString() + ", basebandDecimation=" + this._baseBandDecimationStageCount.ToString() + ", audioDecimation=" + this._audioDecimationStageCount.ToString());
		}

		private void initFilters()
		{
			int num = this._bandwidth / 2;
			int filterOrder = (this._actualDetectorType == DetectorType.WFM) ? 60 : this._filterOrder;
			float[] coefficients = FilterBuilder.MakeLowPassKernel(this._sampleRate / Math.Pow(2.0, (double)this._baseBandDecimationStageCount), filterOrder, (double)num, this._windowType);
			if (this._realIqFilter == null)
			{
				this._realIqFilter = new IQFirFilter(coefficients, this._actualDetectorType == DetectorType.WFM, 1);
			}
			else
			{
				this._realIqFilter.SetCoefficients(coefficients);
			}
			if (this._cpxIqFilter == null)
			{
				this._cpxIqFilter = new CpxFirFilter(coefficients);
			}
			else
			{
				this._cpxIqFilter.SetCoefficients(coefficients);
			}
			this._envFilter.MakeCoefficients(this._sampleRate / Math.Pow(2.0, (double)this._envelopeDecimationStageCount), 6000, num, this._windowType, false);
			if (this._notch >= 0)
			{
				this._notchFilter[this._notch].MakeCoefficients(this._sampleRate / Math.Pow(2.0, (double)this._baseBandDecimationStageCount), this._notchFrequency, this._notchWidth, this._windowType, true);
				this._notch = -1;
			}
			int num2 = 0;
			int num3 = 10000;
			switch (this._actualDetectorType)
			{
			case DetectorType.AM:
			case DetectorType.SAM:
				num2 = 20;
				num3 = Math.Min(this._bandwidth / 2, 16000);
				break;
			case DetectorType.CW:
				num2 = this._cwToneShift - this._bandwidth / 2;
				num3 = this._cwToneShift + this._bandwidth / 2;
				break;
			case DetectorType.LSB:
			case DetectorType.USB:
				num2 = 400;
				num3 = this._bandwidth;
				break;
			case DetectorType.DSB:
				num2 = 400;
				num3 = this._bandwidth / 2;
				break;
			case DetectorType.NFM:
				num2 = 300;
				num3 = this._bandwidth / 2;
				break;
			}
			coefficients = FilterBuilder.MakeBandPassKernel(this._sampleRate / Math.Pow(2.0, (double)(this._baseBandDecimationStageCount + this._audioDecimationStageCount)), this._filterOrder, (double)num2, (double)num3, this._windowType);
			this._audioFilter.SetCoefficients(coefficients);
			this._rFilter.SetCoefficients(coefficients);
			this._lFilter.SetCoefficients(coefficients);
		}

		public unsafe int ProcessBuffer(Complex* iqBuffer, float* audioBuffer, int iqLen, float* xBuf, float* yBuf, float* envelopBuf)
		{
			if (this._needConfigure)
			{
				this.configure();
				this._needConfigure = false;
			}
			if (this._demodX == DemodType.Envelope || this._demodY == DemodType.Envelope || envelopBuf != null)
			{
				if (this._envBuf == null || this._envBuf.Length != iqLen)
				{
					if (this._envBuf != null)
					{
						this._envBuf.Dispose();
					}
					this._envBuf = UnsafeBuffer.Create(iqLen, sizeof(Complex));
					this._envPtr = (Complex*)(void*)this._envBuf;
				}
				Utils.Memcpy(this._envPtr, iqBuffer, iqLen * sizeof(Complex));
				this._downConverter2.Process(this._envPtr, iqLen);
				int num = iqLen;
				if (this._envelopeDecimator.StageCount > 0)
				{
					this._envelopeDecimator.Process(this._envPtr, iqLen);
					num = iqLen >> this._envelopeDecimationStageCount;
				}
				this._envFilter.Process(this._envPtr, num);
				if (this._demodX == DemodType.Envelope)
				{
					for (int i = 0; i < num; i++)
					{
						xBuf[i] = this._envPtr[i].Real * 0.005f;
					}
					this.doAgcAndGain(this._agcX, xBuf, num);
					if (this._useAgc)
					{
						for (int j = 0; j < num; j++)
						{
							xBuf[j] *= 0.33f;
						}
					}
				}
				if (this._demodY == DemodType.Envelope)
				{
					for (int k = 0; k < num; k++)
					{
						yBuf[k] = this._envPtr[k].Imag * 0.005f;
					}
					this.doAgcAndGain(this._agcY, yBuf, num);
					if (this._useAgc)
					{
						for (int l = 0; l < num; l++)
						{
							yBuf[l] *= 0.33f;
						}
					}
				}
				if (envelopBuf != null)
				{
					for (int m = 0; m < num; m++)
					{
						envelopBuf[m] = this._envPtr[m].Imag * 0.005f;
					}
					this.doAgcAndGain(this._agcEnv, envelopBuf, num);
					if (this._useAgc)
					{
						for (int n = 0; n < num; n++)
						{
							envelopBuf[n] *= 0.33f;
						}
					}
				}
			}
			this._downConverter.Process(iqBuffer, iqLen);
			if (this._hookManager != null)
			{
				this._hookManager.ProcessFrequencyTranslatedIQ(iqBuffer, iqLen);
			}
			int num2 = iqLen;
			if (this._baseBandDecimator.StageCount > 0)
			{
				this._baseBandDecimator.Process(iqBuffer, iqLen);
				num2 = iqLen >> this._baseBandDecimationStageCount;
			}
			if (!Utils.FastConvolve)
			{
				this._realIqFilter.Process(iqBuffer, num2);
			}
			else
			{
				this._cpxIqFilter.Process(iqBuffer, num2);
			}
			if (this._hookManager != null)
			{
				this._hookManager.ProcessDecimatedAndFilteredIQ(iqBuffer, num2);
			}
			for (int num3 = 0; num3 <= 3; num3++)
			{
				if (this._notchFilter[num3] != null && this._notchFilter[num3].Width > 0)
				{
					this._notchFilter[num3].Process(iqBuffer, num2);
				}
			}
			if (this._actualDetectorType == DetectorType.RAW)
			{
				Utils.Memcpy(audioBuffer, iqBuffer, num2 * sizeof(Complex));
			}
			else
			{
				if (this._rawAudioBuffer == null || this._rawAudioBuffer.Length != num2)
				{
					if (this._rawAudioBuffer != null)
					{
						this._rawAudioBuffer.Dispose();
					}
					this._rawAudioBuffer = UnsafeBuffer.Create(num2, 4);
					this._rawAudioPtr = (float*)(void*)this._rawAudioBuffer;
				}
				if (this.DetectorType == DetectorType.SAM && (this._rBuf == null || this._rBuf.Length != num2))
				{
					if (this._rBuf != null)
					{
						this._rBuf.Dispose();
					}
					if (this._lBuf != null)
					{
						this._lBuf.Dispose();
					}
					this._rBuf = UnsafeBuffer.Create(num2, 4);
					this._lBuf = UnsafeBuffer.Create(num2, 4);
					this._rPtr = (float*)(void*)this._rBuf;
					this._lPtr = (float*)(void*)this._lBuf;
				}
				if (this._actualDetectorType != DetectorType.WFM)
				{
					Vfo.scaleIQ(iqBuffer, num2);
				}
			}
			int num4 = num2 * 2;
			this.demodulate(iqBuffer, xBuf, yBuf, num2);
			if (this._demodX == DemodType.AM)
			{
				this.doAgcAndGain(this._agcX, xBuf, num2);
			}
			if (this._demodY == DemodType.AM)
			{
				this.doAgcAndGain(this._agcY, yBuf, num2);
			}
			if (this._actualDetectorType == DetectorType.RAW)
			{
				return num2;
			}
			if (this._actualDetectorType == DetectorType.SAM && this._stereo)
			{
				if (Utils.Chk1)
				{
					if (this._filterAudio)
					{
						this._rFilter.Process(this._rPtr, num2);
					}
					this.doAgcAndGain(this._agcX, this._rPtr, num2);
					if (this._filterAudio)
					{
						this._lFilter.Process(this._lPtr, num2);
					}
					this.doAgcAndGain(this._agcY, this._lPtr, num2);
				}
				int num5 = 0;
				for (int num6 = 0; num6 < num2; num6++)
				{
					audioBuffer[num5++] = this._rPtr[num6];
					audioBuffer[num5++] = this._lPtr[num6];
				}
				if (!Utils.Chk1)
				{
					if (this._filterAudio)
					{
						this._audioFilter.Process(audioBuffer, num2 * 2);
					}
					this.doAgcAndGain(this._agc, audioBuffer, num2 * 2);
				}
				return num2;
			}
			if (this._hookManager != null)
			{
				this._hookManager.ProcessDemodulatorOutput(this._rawAudioPtr, num2);
			}
			if (this._actualDetectorType != DetectorType.WFM)
			{
				if (this._filterAudio)
				{
					this._audioFilter.Process(this._rawAudioPtr, num2);
				}
				if (this._actualDetectorType != 0)
				{
					this.doAgcOrGain(this._agc, this._rawAudioPtr, num2);
				}
			}
			if (this._actualDetectorType == DetectorType.AM)
			{
				this._dcRemover.Process(this._rawAudioPtr, num2);
			}
			if (this._actualDetectorType != DetectorType.WFM)
			{
				Vfo.monoToStereo(this._rawAudioPtr, audioBuffer, num2);
			}
			else
			{
				this._rdsDecoder.Process(this._rawAudioPtr, num2);
				this._stereoDecoder.Process(this._rawAudioPtr, audioBuffer, num2);
				num4 >>= this._audioDecimationStageCount;
			}
			if (this._hookManager != null)
			{
				this._hookManager.ProcessFilteredAudioOutput(audioBuffer, num4);
			}
			return num2;
		}

		private unsafe void doAgcOrGain(AutomaticGainControl agc, float* buffer, int length)
		{
			if (this._useAgc)
			{
				agc.Process(buffer, length);
			}
			else
			{
				this.DoRFgain(buffer, length);
			}
		}

		private unsafe void doAgcAndGain(AutomaticGainControl agc, float* buffer, int length)
		{
			if (this._useAgc)
			{
				agc.Process(buffer, length);
			}
			this.DoRFgain(buffer, length);
		}

		private unsafe void DoRFgain(float* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i] *= this._rfGain;
			}
		}

		private unsafe static void scaleIQ(Complex* buffer, int length)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i].Real *= 0.01f;
				buffer[i].Imag *= 0.01f;
			}
		}

		private unsafe static void monoToStereo(float* input, float* output, int inputLength)
		{
			for (int i = 0; i < inputLength; i++)
			{
				float* intPtr = output;
				output = intPtr + 1;
				*intPtr = *input;
				float* intPtr2 = output;
				output = intPtr2 + 1;
				*intPtr2 = *input;
				input++;
			}
		}

		private unsafe void demodulate(Complex* iq, float* xBuf, float* yBuf, int length)
		{
			switch (this._demodX)
			{
			case DemodType.AM:
				this._amDetector.Demodulate(iq, xBuf, length);
				break;
			case DemodType.FM:
				this._fmDetector.Demodulate(iq, xBuf, length);
				break;
			case DemodType.PM:
				this._pmDetector.Demodulate(iq, xBuf, length);
				break;
			case DemodType.IQ:
				for (int i = 0; i < length; i++)
				{
					xBuf[i] = iq[i].Real;
				}
				break;
			}
			if (this._demodY == this._demodX && this.DemodX != 0)
			{
				Utils.Memcpy(yBuf, xBuf, length * 4);
			}
			else
			{
				switch (this._demodY)
				{
				case DemodType.AM:
					this._amDetector.Demodulate(iq, yBuf, length);
					break;
				case DemodType.FM:
					this._fmDetector.Demodulate(iq, yBuf, length);
					break;
				case DemodType.PM:
					this._pmDetector.Demodulate(iq, yBuf, length);
					break;
				case DemodType.IQ:
					for (int j = 0; j < length; j++)
					{
						yBuf[j] = iq[j].Real;
					}
					break;
				}
			}
			switch (this._actualDetectorType)
			{
			case DetectorType.RAW:
				break;
			case DetectorType.NFM:
			case DetectorType.WFM:
				if (this._demodX == DemodType.FM)
				{
					Utils.Memcpy(this._rawAudioPtr, xBuf, length * 4);
				}
				else if (this._demodY == DemodType.FM)
				{
					Utils.Memcpy(this._rawAudioPtr, yBuf, length * 4);
				}
				else
				{
					this._fmDetector.Demodulate(iq, this._rawAudioPtr, length);
				}
				break;
			case DetectorType.AM:
				if (this._demodX == DemodType.AM)
				{
					Utils.Memcpy(this._rawAudioPtr, xBuf, length * 4);
				}
				else if (this._demodY == DemodType.AM)
				{
					Utils.Memcpy(this._rawAudioPtr, yBuf, length * 4);
				}
				else
				{
					this._amDetector.Demodulate(iq, this._rawAudioPtr, length);
				}
				break;
			case DetectorType.SAM:
				if (!this._stereo)
				{
					this._samDetector.Demodulate(iq, this._rawAudioPtr, length, false, false);
				}
				else
				{
					this._samDetector.Demodulate(iq, this._rPtr, this._lPtr, length);
				}
				break;
			case DetectorType.DSB:
				this._dsbDetector.Demodulate(iq, this._rawAudioPtr, length);
				break;
			case DetectorType.LSB:
				this._lsbDetector.Demodulate(iq, this._rawAudioPtr, length);
				break;
			case DetectorType.USB:
				this._usbDetector.Demodulate(iq, this._rawAudioPtr, length);
				break;
			case DetectorType.CW:
				this._cwDetector.Demodulate(iq, this._rawAudioPtr, length);
				break;
			}
		}
	}
}
