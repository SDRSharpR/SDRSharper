namespace SDRSharp.Radio
{
	public class Hsl
	{
		public float H;

		public float S;

		public float L;

		public Hsl(float h, float s, float l)
		{
			this.H = h;
			this.S = s;
			l = this.L;
		}
	}
}
