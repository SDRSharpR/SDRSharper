using System.Collections.Generic;

namespace SDRSharp.Radio
{
	public class VfoHookManager
	{
		private readonly List<IRealProcessor> _filteredAudioProcessors = new List<IRealProcessor>();

		private readonly List<IRealProcessor> _demodulatorOutputProcessors = new List<IRealProcessor>();

		private readonly List<IIQProcessor> _rawIQProcessors = new List<IIQProcessor>();

		private readonly List<IIQProcessor> _frequencyTranslatedIQProcessors = new List<IIQProcessor>();

		private readonly List<IIQProcessor> _decimatedAndFilteredIQProcessors = new List<IIQProcessor>();

		public Vfo Vfo
		{
			get;
			set;
		}

		public void RegisterStreamHook(object hook, ProcessorType processorType)
		{
			switch (processorType)
			{
			case ProcessorType.RawIQ:
				lock (this._rawIQProcessors)
				{
					this._rawIQProcessors.Add((IIQProcessor)hook);
				}
				break;
			case ProcessorType.FrequencyTranslatedIQ:
				lock (this._frequencyTranslatedIQProcessors)
				{
					this._frequencyTranslatedIQProcessors.Add((IIQProcessor)hook);
				}
				break;
			case ProcessorType.DecimatedAndFilteredIQ:
				lock (this._decimatedAndFilteredIQProcessors)
				{
					this._decimatedAndFilteredIQProcessors.Add((IIQProcessor)hook);
				}
				break;
			case ProcessorType.DemodulatorOutput:
				lock (this._demodulatorOutputProcessors)
				{
					this._demodulatorOutputProcessors.Add((IRealProcessor)hook);
				}
				break;
			case ProcessorType.FilteredAudioOutput:
				lock (this._filteredAudioProcessors)
				{
					this._filteredAudioProcessors.Add((IRealProcessor)hook);
				}
				break;
			}
		}

		public void UnregisterStreamHook(object hook)
		{
			if (hook != null)
			{
				if (hook is IIQProcessor)
				{
					IIQProcessor item = (IIQProcessor)hook;
					lock (this._rawIQProcessors)
					{
						this._rawIQProcessors.Remove(item);
					}
					lock (this._frequencyTranslatedIQProcessors)
					{
						this._frequencyTranslatedIQProcessors.Remove(item);
					}
					lock (this._decimatedAndFilteredIQProcessors)
					{
						this._decimatedAndFilteredIQProcessors.Remove(item);
					}
				}
				else if (hook is IRealProcessor)
				{
					IRealProcessor item2 = (IRealProcessor)hook;
					lock (this._demodulatorOutputProcessors)
					{
						this._demodulatorOutputProcessors.Remove(item2);
					}
					lock (this._filteredAudioProcessors)
					{
						this._filteredAudioProcessors.Remove(item2);
					}
				}
			}
		}

		public void SetProcessorSampleRate(ProcessorType processorType, double sampleRate)
		{
			switch (processorType)
			{
			case ProcessorType.RawIQ:
				this.SetSampleRate(this._rawIQProcessors, sampleRate);
				break;
			case ProcessorType.FrequencyTranslatedIQ:
				this.SetSampleRate(this._frequencyTranslatedIQProcessors, sampleRate);
				break;
			case ProcessorType.DecimatedAndFilteredIQ:
				this.SetSampleRate(this._decimatedAndFilteredIQProcessors, sampleRate);
				break;
			case ProcessorType.DemodulatorOutput:
				this.SetSampleRate(this._demodulatorOutputProcessors, sampleRate);
				break;
			case ProcessorType.FilteredAudioOutput:
				this.SetSampleRate(this._filteredAudioProcessors, sampleRate);
				break;
			}
		}

		public unsafe void ProcessRawIQ(Complex* buffer, int length)
		{
			this.ProcessHooks(this._rawIQProcessors, buffer, length);
		}

		public unsafe void ProcessDecimatedAndFilteredIQ(Complex* buffer, int length)
		{
			this.ProcessHooks(this._decimatedAndFilteredIQProcessors, buffer, length);
		}

		public unsafe void ProcessFrequencyTranslatedIQ(Complex* buffer, int length)
		{
			this.ProcessHooks(this._frequencyTranslatedIQProcessors, buffer, length);
		}

		public unsafe void ProcessDemodulatorOutput(float* buffer, int length)
		{
			this.ProcessHooks(this._demodulatorOutputProcessors, buffer, length);
		}

		public unsafe void ProcessFilteredAudioOutput(float* buffer, int length)
		{
			this.ProcessHooks(this._filteredAudioProcessors, buffer, length);
		}

		private void SetSampleRate(List<IIQProcessor> processors, double sampleRate)
		{
			lock (processors)
			{
				for (int i = 0; i < processors.Count; i++)
				{
					processors[i].SampleRate = sampleRate;
				}
			}
		}

		private void SetSampleRate(List<IRealProcessor> processors, double sampleRate)
		{
			lock (processors)
			{
				for (int i = 0; i < processors.Count; i++)
				{
					processors[i].SampleRate = sampleRate;
				}
			}
		}

		private unsafe void ProcessHooks(List<IIQProcessor> processors, Complex* buffer, int length)
		{
			lock (processors)
			{
				for (int i = 0; i < processors.Count; i++)
				{
					if (processors[i].Enabled)
					{
						processors[i].Process(buffer, length);
					}
				}
			}
		}

		private unsafe void ProcessHooks(List<IRealProcessor> processors, float* buffer, int length)
		{
			lock (processors)
			{
				for (int i = 0; i < processors.Count; i++)
				{
					if (processors[i].Enabled)
					{
						processors[i].Process(buffer, length);
					}
				}
			}
		}
	}
}
