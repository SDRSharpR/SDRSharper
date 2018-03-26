namespace SDRSharp.WavRecorder
{
	public struct WavFormatHeader
	{
		public ushort FormatTag;

		public ushort Channels;

		public uint SamplesPerSec;

		public uint AvgBytesPerSec;

		public ushort BlockAlign;

		public ushort BitsPerSample;

		public WavFormatHeader(WavSampleFormat format, ushort channels, uint sampleRate)
		{
			this.BitsPerSample = 0;
			this.FormatTag = 0;
			switch (format)
			{
			case WavSampleFormat.PCM8:
				this.FormatTag = 1;
				this.BitsPerSample = 8;
				break;
			case WavSampleFormat.PCM16:
				this.FormatTag = 1;
				this.BitsPerSample = 16;
				break;
			case WavSampleFormat.Float32:
				this.FormatTag = 3;
				this.BitsPerSample = 32;
				break;
			}
			this.BlockAlign = (ushort)(channels * ((int)this.BitsPerSample / 8));
			this.SamplesPerSec = sampleRate;
			this.Channels = channels;
			this.AvgBytesPerSec = sampleRate * this.BlockAlign;
		}
	}
}
