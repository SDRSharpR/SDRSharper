namespace SDRSharp.PanView
{
	public class Notch
	{
		private int _offset;

		private int _width;

		private bool _active;

		private float _xPos;

		private float _xHalf;

		public int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		public int Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
			}
		}

		public bool Active
		{
			get
			{
				return this._active;
			}
			set
			{
				this._active = value;
			}
		}

		public float Xpos
		{
			get
			{
				return this._xPos;
			}
			set
			{
				this._xPos = value;
			}
		}

		public float Xhalf
		{
			get
			{
				return this._xHalf;
			}
			set
			{
				this._xHalf = value;
			}
		}
	}
}
