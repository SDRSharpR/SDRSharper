using SDRSharp.Radio;

namespace SDRSharp.DNR
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
