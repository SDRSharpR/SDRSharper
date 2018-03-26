using System;
using System.Runtime.InteropServices;

namespace SDRSharp.RTLSDR
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void RtlSdrReadAsyncDelegate(byte* buf, uint len, IntPtr ctx);
}
