using SDRSharp.Radio;

namespace SDRSharp.NoiseReduction
{
	public interface INoiseProcessor : IBaseProcessor
	{
		int NoiseThreshold
		{
			get;
			set;
		}
	}
}
