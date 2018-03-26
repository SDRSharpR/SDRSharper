namespace SDRSharp.Radio
{
	public interface IBaseProcessor
	{
		double SampleRate
		{
			set;
		}

		bool Enabled
		{
			get;
			set;
		}
	}
}
