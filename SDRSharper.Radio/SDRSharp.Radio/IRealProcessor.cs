namespace SDRSharp.Radio
{
	public interface IRealProcessor : IBaseProcessor
	{
		unsafe void Process(float* buffer, int length);
	}
}
