namespace SDRSharp.Radio
{
	public unsafe delegate int ProcessBufferDelegate(Complex* iqBuffer, float* audioBuffer, int iqlen, int audioLen);
}
