using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PortAudioSharp
{
	internal static class PortAudioAPI
	{
		public const int PaFormatIsSupported = 0;

		public const int PaFramesPerBufferUnspecified = 0;

#if BUILD64
        public const string PortAudioLibrary = "portaudio_x64";
#endif

#if BUILD32
        public const string PortAudioLibrary = "portaudio_x32";
#endif

        [DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetVersion();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Pa_GetVersionText")]
		private static extern IntPtr IntPtr_Pa_GetVersionText();

		public static string Pa_GetVersionText()
		{
			IntPtr ptr = PortAudioAPI.IntPtr_Pa_GetVersionText();
			return Marshal.PtrToStringAnsi(ptr);
		}

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Pa_GetErrorText")]
		public static extern IntPtr IntPtr_Pa_GetErrorText(PaError errorCode);

		public static string Pa_GetErrorText(PaError errorCode)
		{
			IntPtr ptr = PortAudioAPI.IntPtr_Pa_GetErrorText(errorCode);
			return Marshal.PtrToStringAnsi(ptr);
		}

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_Initialize();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_Terminate();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetHostApiCount();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetDefaultHostApi();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Pa_GetHostApiInfo")]
		public static extern IntPtr IntPtr_Pa_GetHostApiInfo(int hostApi);

		public static PaHostApiInfo Pa_GetHostApiInfo(int hostApi)
		{
			IntPtr ptr = PortAudioAPI.IntPtr_Pa_GetHostApiInfo(hostApi);
			return (PaHostApiInfo)Marshal.PtrToStructure(ptr, typeof(PaHostApiInfo));
		}

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_HostApiTypeIdToHostApiIndex(PaHostApiTypeId type);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_HostApiDeviceIndexToDeviceIndex(int hostApi, int hostApiDeviceIndex);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Pa_GetLastHostErrorInfo")]
		public static extern IntPtr IntPtr_Pa_GetLastHostErrorInfo();

		public static PaHostErrorInfo Pa_GetLastHostErrorInfo()
		{
			IntPtr ptr = PortAudioAPI.IntPtr_Pa_GetLastHostErrorInfo();
			return (PaHostErrorInfo)Marshal.PtrToStructure(ptr, typeof(PaHostErrorInfo));
		}

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetDeviceCount();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetDefaultInputDevice();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetDefaultOutputDevice();

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Pa_GetDeviceInfo")]
		public static extern IntPtr IntPtr_Pa_GetDeviceInfo(int device);

		public static PaDeviceInfo Pa_GetDeviceInfo(int device)
		{
			IntPtr ptr = PortAudioAPI.IntPtr_Pa_GetDeviceInfo(device);
			return (PaDeviceInfo)Marshal.PtrToStructure(ptr, typeof(PaDeviceInfo));
		}

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_IsFormatSupported(ref PaStreamParameters inputParameters, ref PaStreamParameters outputParameters, double sampleRate);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_IsFormatSupported(IntPtr inputParameters, ref PaStreamParameters outputParameters, double sampleRate);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_IsFormatSupported(ref PaStreamParameters inputParameters, IntPtr outputParameters, double sampleRate);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_OpenStream(out IntPtr stream, ref PaStreamParameters inputParameters, ref PaStreamParameters outputParameters, double sampleRate, uint framesPerBuffer, PaStreamFlags streamFlags, PaStreamCallbackDelegate streamCallback, IntPtr userData);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_OpenStream(out IntPtr stream, IntPtr inputParameters, ref PaStreamParameters outputParameters, double sampleRate, uint framesPerBuffer, PaStreamFlags streamFlags, PaStreamCallbackDelegate streamCallback, IntPtr userData);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_OpenStream(out IntPtr stream, ref PaStreamParameters inputParameters, IntPtr outputParameters, double sampleRate, uint framesPerBuffer, PaStreamFlags streamFlags, PaStreamCallbackDelegate streamCallback, IntPtr userData);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_OpenDefaultStream(out IntPtr stream, int numInputChannels, int numOutputChannels, uint sampleFormat, double sampleRate, uint framesPerBuffer, PaStreamCallbackDelegate streamCallback, IntPtr userData);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_CloseStream(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_SetStreamFinishedCallback(ref IntPtr stream, [MarshalAs(UnmanagedType.FunctionPtr)] PaStreamFinishedCallbackDelegate streamFinishedCallback);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_StartStream(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_StopStream(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_AbortStream(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_IsStreamStopped(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_IsStreamActive(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Pa_GetStreamInfo")]
		public static extern IntPtr IntPtr_Pa_GetStreamInfo(IntPtr stream);

		public static PaStreamInfo Pa_GetStreamInfo(IntPtr stream)
		{
			IntPtr ptr = PortAudioAPI.IntPtr_Pa_GetStreamInfo(stream);
			return (PaStreamInfo)Marshal.PtrToStructure(ptr, typeof(PaStreamInfo));
		}

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern double Pa_GetStreamTime(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern double Pa_GetStreamCpuLoad(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] float[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] byte[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] sbyte[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] ushort[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] short[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] uint[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, [Out] int[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_ReadStream(IntPtr stream, IntPtr buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] float[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] byte[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] sbyte[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] ushort[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] short[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] uint[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_WriteStream(IntPtr stream, [In] int[] buffer, uint frames);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetStreamReadAvailable(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pa_GetStreamWriteAvailable(IntPtr stream);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern PaError Pa_GetSampleSize(PaSampleFormat format);

		[DllImport(PortAudioLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Pa_Sleep(int msec);

		static PortAudioAPI()
		{
            PortAudioAPI.Pa_Initialize();
		}

    }
}
