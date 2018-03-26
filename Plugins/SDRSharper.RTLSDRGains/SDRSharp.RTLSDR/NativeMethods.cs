using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SDRSharp.RTLSDR
{
	public class NativeMethods
	{
		private const string LibRtlSdr = "librtlsdr";

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint rtlsdr_get_device_count();

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rtlsdr_get_device_name")]
		private static extern IntPtr rtlsdr_get_device_name_native(uint index);

		public static string rtlsdr_get_device_name(uint index)
		{
			IntPtr ptr = NativeMethods.rtlsdr_get_device_name_native(index);
			return Marshal.PtrToStringAnsi(ptr);
		}

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_get_device_usb_strings(uint index, StringBuilder manufact, StringBuilder product, StringBuilder serial);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_open(out IntPtr dev, uint index);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_close(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_xtal_freq(IntPtr dev, uint rtlFreq, uint tunerFreq);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_get_xtal_freq(IntPtr dev, out uint rtlFreq, out uint tunerFreq);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_get_usb_strings(IntPtr dev, StringBuilder manufact, StringBuilder product, StringBuilder serial);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_center_freq(IntPtr dev, uint freq);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint rtlsdr_get_center_freq(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_freq_correction(IntPtr dev, int ppm);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_get_freq_correction(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_get_tuner_gains(IntPtr dev, [In] [Out] int[] gains);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern RtlSdrTunerType rtlsdr_get_tuner_type(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_tuner_gain(IntPtr dev, int gain);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_tuner_gain_ext(IntPtr dev, int lna_gain, int mixer_gain, int vga_gain);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_get_tuner_gain(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_tuner_gain_mode(IntPtr dev, int manual);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_agc_mode(IntPtr dev, int on);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_direct_sampling(IntPtr dev, int on);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_offset_tuning(IntPtr dev, int on);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_sample_rate(IntPtr dev, uint rate);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint rtlsdr_get_sample_rate(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_set_testmode(IntPtr dev, int on);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_reset_buffer(IntPtr dev);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_read_sync(IntPtr dev, IntPtr buf, int len, out int nRead);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_wait_async(IntPtr dev, RtlSdrReadAsyncDelegate cb, IntPtr ctx);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_read_async(IntPtr dev, RtlSdrReadAsyncDelegate cb, IntPtr ctx, uint bufNum, uint bufLen);

		[DllImport("librtlsdr", CallingConvention = CallingConvention.Cdecl)]
		public static extern int rtlsdr_cancel_async(IntPtr dev);
	}
}
