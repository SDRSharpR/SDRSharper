using System;
using System.Runtime.InteropServices;

namespace SDRSharp.SDRIQ
{
	public class NativeMethods
	{
		private const string LibSDRIQ = "sdriq";

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern void sdriq_initialise();

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern void sdriq_destroy();

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint sdriq_get_device_count();

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_open(uint devIndex, uint buffersCount, out IntPtr dev);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_close(IntPtr dev);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_async_read(IntPtr dev, IntPtr context, SdrIqReadAsyncDelegate callback, int readBlocks);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_async_cancel(IntPtr dev);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_set_center_frequency(IntPtr dev, uint frequency);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_set_out_samplerate(IntPtr dev, uint rate);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_set_if_gain(IntPtr dev, sbyte value);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl)]
		public static extern int sdriq_set_rf_gain(IntPtr dev, sbyte value);

		[DllImport("sdriq", CallingConvention = CallingConvention.Cdecl, EntryPoint = "sdriq_get_serial_number")]
		private static extern IntPtr sdriq_get_serial_number_native(uint devNo);

		public static string sdriq_get_serial_number(uint index)
		{
			IntPtr ptr = NativeMethods.sdriq_get_serial_number_native(index);
			return Marshal.PtrToStringAnsi(ptr);
		}
	}
}
