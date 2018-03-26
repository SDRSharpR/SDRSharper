namespace SDRSharp.Radio
{
	public interface IIQProcessor : IBaseProcessor
	{
		unsafe void Process(Complex* buffer, int length);
	}
}
