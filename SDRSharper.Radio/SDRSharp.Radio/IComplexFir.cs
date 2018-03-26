namespace SDRSharp.Radio
{
	public interface IComplexFir
	{
		unsafe void Process(Complex* buffer, int length);
	}
}
