namespace SDRSharp.Radio
{
	public interface IFilter
	{
		unsafe void Process(float* buffer, int length);

		unsafe void ProcessInterleaved(float* buffer, int length);
	}
}
