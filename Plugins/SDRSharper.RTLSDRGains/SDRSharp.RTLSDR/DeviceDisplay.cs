namespace SDRSharp.RTLSDR
{
	public class DeviceDisplay
	{
		public uint Index
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			set;
		}

		public static DeviceDisplay[] GetActiveDevices()
		{
			uint num = NativeMethods.rtlsdr_get_device_count();
			DeviceDisplay[] array = new DeviceDisplay[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				string name = NativeMethods.rtlsdr_get_device_name(num2);
				array[num2] = new DeviceDisplay
				{
					Index = num2,
					Name = name
				};
			}
			return array;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
