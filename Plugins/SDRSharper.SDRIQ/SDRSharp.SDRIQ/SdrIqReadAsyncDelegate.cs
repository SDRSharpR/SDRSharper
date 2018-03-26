using System;
using System.Runtime.InteropServices;

namespace SDRSharp.SDRIQ
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void SdrIqReadAsyncDelegate(short* buf, uint len, IntPtr ctx);
}
