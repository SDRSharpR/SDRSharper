using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SDRSharp.Radio
{
	public static class ExtIO
	{
		public enum HWTypes
		{
			Sdr14 = 1,
			Aud16BInt = 3,
			Soundcard,
			Aud24BInt,
			Aud32BInt,
			Aud32BFloat
		}

		public enum StatusEvent
		{
			SrChange = 100,
			LOChange,
			ProhibLO,
			LOChangeOk,
			LoChangeNoTune,
			TuneChange,
			DemodChange,
			RsqStart,
			RsqStop,
			FiltChange
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Ansi)]
		public delegate void ExtIOManagedCallbackDelegate([MarshalAs(UnmanagedType.I4)] int a, [MarshalAs(UnmanagedType.I4)] int b, [MarshalAs(UnmanagedType.R4)] float c, IntPtr data);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool dInitHW([MarshalAs(UnmanagedType.LPStr)] StringBuilder name, [MarshalAs(UnmanagedType.LPStr)] StringBuilder model, [MarshalAs(UnmanagedType.I4)] ref int type);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool dOpenHW();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dStartHW([MarshalAs(UnmanagedType.I4)] int freq);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		private delegate void dStopHW();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		private delegate void dCloseHW();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dSetHWLO([MarshalAs(UnmanagedType.I4)] int freq);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dTuneChanged([MarshalAs(UnmanagedType.I4)] int freq);

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dGetHWLO();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dGetHWSR();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dGetTune();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.I4)]
		private delegate int dGetStatus();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		private delegate void dShowGUI();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		private delegate void dHideGUI();

		[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
		private delegate void dSetCallback(ExtIOManagedCallbackDelegate callbackAddr);

		public static SamplesAvailableDelegate SamplesAvailable;

		private static dInitHW _initHW;

		private static dOpenHW _openHW;

		private static dStartHW _startHW;

		private static dStopHW _stopHW;

		private static dCloseHW _closeHW;

		private static dSetHWLO _setHWLO;

		private static dTuneChanged _tuneChanged;

		private static dGetHWLO _getHWLO;

		private static dGetHWSR _getHWSR;

		private static dGetTune _getTune;

		private static dGetStatus _getStatus;

		private static dShowGUI _showGUI;

		private static dHideGUI _hideGUI;

		private static dSetCallback _setCallback;

		private static IntPtr _dllHandle;

		private static string _dllName;

		private static string _name;

		private static string _model;

		private static HWTypes _hwType;

		private static bool _isHWInit;

		private static bool _isHWStarted;

		private static UnsafeBuffer _iqBuffer;

		private unsafe static Complex* _iqPtr;

		private static int _sampleCount;

		private static ExtIOManagedCallbackDelegate _callbackInst;

		private static ListBox _listBox;

		public static string DllName => ExtIO._dllName;

		public static HWTypes HWType => ExtIO._hwType;

		public static bool IsHardwareStarted => ExtIO._isHWStarted;

		public static bool IsHardwareOpen => ExtIO._isHWInit;

		public static ListBox ListBox
		{
			set
			{
				ExtIO._listBox = value;
			}
		}

		public static string HWName
		{
			get
			{
				if (ExtIO._dllHandle != IntPtr.Zero)
				{
					return ExtIO._name;
				}
				return string.Empty;
			}
		}

		public static string HWModel
		{
			get
			{
				if (ExtIO._dllHandle != IntPtr.Zero)
				{
					return ExtIO._model;
				}
				return string.Empty;
			}
		}

		public static event dSampleRateChanged SampleRateChanged;

		public static event dLOFrequencyChanged LOFreqChanged;

		public static event dTuneFrequencyChanged TuneFreqChanged;

		public static event dLOFrequencyChangeAccepted LOFreqChangedAccepted;

		public static event dProhibitLOChanges ProhibitLOChanged;

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string dllToLoad);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeLibrary(IntPtr hModule);

		unsafe static ExtIO()
		{
			ExtIO._iqBuffer = UnsafeBuffer.Create(1024, sizeof(Complex));
			ExtIO._callbackInst = ExtIO.extIOCallback;
			GCHandle.Alloc(ExtIO._callbackInst);
		}

		public static void UseLibrary(string fileName)
		{
			if (ExtIO._dllHandle != IntPtr.Zero)
			{
				ExtIO.CloseLibrary();
			}
			ExtIO.logInfo("UseLibrary(), dll=" + fileName);
			try
			{
				ExtIO._dllHandle = ExtIO.LoadLibrary(fileName);
			}
			catch (Exception ex)
			{
				ExtIO.logInfo("LoadLibrary: " + ex.Message);
			}
			ExtIO.logResult("LoadLibrary:");
			if (ExtIO._dllHandle == IntPtr.Zero)
			{
				ExtIO.logInfo("LoadLibrary(), Unable to load DLL file: " + fileName);
				throw new Exception("Unable to load ExtIO library " + fileName);
			}
			ExtIO._dllName = fileName;
			ExtIO._initHW = null;
			ExtIO._openHW = null;
			ExtIO._startHW = null;
			ExtIO._stopHW = null;
			ExtIO._closeHW = null;
			ExtIO._setHWLO = null;
			ExtIO._tuneChanged = null;
			ExtIO._getHWLO = null;
			ExtIO._getHWSR = null;
			ExtIO._getStatus = null;
			ExtIO._showGUI = null;
			ExtIO._hideGUI = null;
			ExtIO._setCallback = null;
			IntPtr procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "InitHW");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._initHW = (dInitHW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dInitHW));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "OpenHW");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._openHW = (dOpenHW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dOpenHW));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "StartHW");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._startHW = (dStartHW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dStartHW));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "StopHW");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._stopHW = (dStopHW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dStopHW));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "CloseHW");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._closeHW = (dCloseHW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dCloseHW));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "SetCallback");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._setCallback = (dSetCallback)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dSetCallback));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "SetHWLO");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._setHWLO = (dSetHWLO)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dSetHWLO));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "TuneChange");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._tuneChanged = (dTuneChanged)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dTuneChanged));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "GetHWLO");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._getHWLO = (dGetHWLO)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dGetHWLO));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "GetHWSR");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._getHWSR = (dGetHWSR)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dGetHWSR));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "GetTune");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._getTune = (dGetTune)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dGetTune));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "GetStatus");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._getStatus = (dGetStatus)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dGetStatus));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "ShowGUI");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._showGUI = (dShowGUI)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dShowGUI));
			}
			procAddress = ExtIO.GetProcAddress(ExtIO._dllHandle, "HideGUI");
			if (procAddress != IntPtr.Zero)
			{
				ExtIO._hideGUI = (dHideGUI)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(dHideGUI));
			}
			if (ExtIO._initHW != null && ExtIO._openHW != null && ExtIO._startHW != null && ExtIO._setHWLO != null && ExtIO._getStatus != null && ExtIO._setCallback != null && ExtIO._stopHW != null && ExtIO._closeHW != null)
			{
				return;
			}
			ExtIO.FreeLibrary(ExtIO._dllHandle);
			ExtIO._dllHandle = IntPtr.Zero;
			ExtIO.logInfo("LoadLibrary(), ExtIO DLL is not valid, not all entries found.");
			throw new ApplicationException("ExtIO DLL is not valid, not all entries found.");
		}

		public static void CloseLibrary()
		{
			if (ExtIO._isHWStarted)
			{
				ExtIO.StopHW();
			}
			if (ExtIO._isHWInit)
			{
				ExtIO.CloseHW();
			}
			if (!(ExtIO._dllHandle == IntPtr.Zero))
			{
				ExtIO.logInfo("CloseLibrary()");
				try
				{
					ExtIO.FreeLibrary(ExtIO._dllHandle);
				}
				catch (Exception ex)
				{
					ExtIO.logInfo("FreeLibrary: " + ex.Message);
				}
				ExtIO.logResult("FreeLibrary: ");
				ExtIO._dllHandle = IntPtr.Zero;
			}
		}

		public static void HWInit(bool setCallback)
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && !ExtIO._isHWInit && !ExtIO._isHWStarted)
			{
				ExtIO.logInfo("HWInit()");
				StringBuilder stringBuilder = new StringBuilder(256);
				StringBuilder stringBuilder2 = new StringBuilder(256);
				int hwType = 0;
				try
				{
					ExtIO._isHWInit = ExtIO._initHW(stringBuilder, stringBuilder2, ref hwType);
				}
				catch (Exception ex)
				{
					ExtIO.logInfo("InitHW: " + ex.Message);
				}
				ExtIO.logResult("InitHW: ");
				ExtIO._name = stringBuilder.ToString();
				ExtIO._model = stringBuilder2.ToString();
				ExtIO._hwType = (HWTypes)hwType;
				if (!ExtIO._isHWInit)
				{
					ExtIO.logInfo("InitHW() returned " + ExtIO._isHWInit + ", ");
					ExtIO._isHWInit = true;
					ExtIO.CloseHW();
					throw new ApplicationException("InitHW() returned " + ExtIO._isHWInit);
				}
				ExtIO.logInfo("InitHW: " + ExtIO._name + ", " + ExtIO._model + ", type=" + hwType.ToString());
				if (setCallback)
				{
					ExtIO.logInfo("SetCallback: ");
					try
					{
						ExtIO._setCallback(ExtIO._callbackInst);
					}
					catch (Exception ex2)
					{
						ExtIO.logInfo("SetCallback: " + ex2.Message);
					}
				}
			}
		}

		public static bool OpenHW(bool setCallback = true)
		{
			if (!ExtIO._isHWInit)
			{
				ExtIO.HWInit(setCallback);
			}
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._isHWInit && !ExtIO._isHWStarted)
			{
				ExtIO.logInfo("OpenHW()");
				bool flag = false;
				try
				{
					flag = ExtIO._openHW();
				}
				catch (Exception ex)
				{
					ExtIO.logInfo("OpenHW: " + ex.Message);
				}
				if (flag)
				{
					ExtIO.ShowGUI();
				}
				else
				{
					ExtIO.logResult("OpenHW: ");
				}
				return flag;
			}
			return false;
		}

		public unsafe static void StartHW(int freq)
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._startHW != null)
			{
				ExtIO.logInfo("StartHW(), freq=" + freq.ToString());
				if (!ExtIO._isHWInit)
				{
					ExtIO.OpenHW(true);
				}
				if (ExtIO._iqBuffer != null)
				{
					ExtIO._iqBuffer.Dispose();
				}
				ExtIO._iqBuffer = null;
				ExtIO._iqPtr = null;
				int num = ExtIO._startHW(freq);
				ExtIO.logResult("StartHW: ");
				if (num <= 0)
				{
					ExtIO.logInfo("StartHW() returned " + num);
					throw new Exception("ExtIO StartHW() returned " + num);
				}
				ExtIO._isHWStarted = true;
				ExtIO._sampleCount = num;
				ExtIO._iqBuffer = UnsafeBuffer.Create(ExtIO._sampleCount, sizeof(Complex));
				ExtIO._iqPtr = (Complex*)(void*)ExtIO._iqBuffer;
				ExtIO.logInfo("StartHW succeeded, samplecount=" + ExtIO._sampleCount.ToString() + ", iqBuffer created.");
			}
		}

		public static void StopHW()
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._isHWInit)
			{
				ExtIO.logInfo("StopHW()");
				try
				{
					ExtIO._stopHW();
				}
				catch (Exception ex)
				{
					ExtIO.logInfo("StopHW: " + ex.Message);
				}
				ExtIO._isHWStarted = false;
			}
		}

		public static void CloseHW()
		{
			if (ExtIO._isHWStarted)
			{
				ExtIO.StopHW();
			}
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._isHWInit)
			{
				ExtIO.logInfo("CloseHW()");
				try
				{
					ExtIO._closeHW();
				}
				catch (Exception ex)
				{
					ExtIO.logInfo("CloseHW: " + ex.Message);
				}
				ExtIO._isHWInit = false;
			}
		}

		public static int GetHWSR()
		{
			int result = 0;
			if (ExtIO._dllHandle != IntPtr.Zero && ExtIO._getHWSR != null && ExtIO._isHWInit)
			{
				result = ExtIO._getHWSR();
			}
			ExtIO.logInfo("getHWSR, SR=" + result.ToString());
			return result;
		}

		public static int GetHWLO()
		{
			int result = 0;
			if (ExtIO._dllHandle != IntPtr.Zero && ExtIO._getHWLO != null && ExtIO._isHWInit)
			{
				result = ExtIO._getHWLO();
			}
			ExtIO.logInfo("getHWLO, freq=" + result.ToString());
			return result;
		}

		public static int GetTune()
		{
			int result = 0;
			if (ExtIO._dllHandle != IntPtr.Zero && ExtIO._getTune != null && ExtIO._isHWInit)
			{
				result = ExtIO._getTune();
			}
			ExtIO.logInfo("getTune, freq=" + result.ToString());
			return result;
		}

		public static void SetHWLO(int freq)
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._setHWLO != null && ExtIO._isHWInit)
			{
				try
				{
					ExtIO._setHWLO(freq);
				}
				catch (Exception)
				{
				}
			}
		}

		public static void TuneChanged(int freq)
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._tuneChanged != null && ExtIO._isHWInit)
			{
				ExtIO.logInfo("tuneChanged to " + freq.ToString());
				ExtIO._tuneChanged(freq);
			}
		}

		public static void ShowGUI()
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._showGUI != null && ExtIO._isHWInit)
			{
				ExtIO.logInfo("ShowGui()");
				ExtIO._showGUI();
			}
		}

		public static void HideGUI()
		{
			if (!(ExtIO._dllHandle == IntPtr.Zero) && ExtIO._hideGUI != null && ExtIO._isHWInit)
			{
				ExtIO.logInfo("HideGui()");
				ExtIO._hideGUI();
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private unsafe static void extIOCallback(int count, int status, float iqOffs, IntPtr dataPtr)
		{
			if (count >= 0 && ExtIO._isHWStarted)
			{
				if (ExtIO._iqPtr != null)
				{
					int length = ExtIO._iqBuffer.Length;
					if (ExtIO._hwType == HWTypes.Aud16BInt || ExtIO._hwType == HWTypes.Sdr14)
					{
						short* ptr = (short*)(void*)dataPtr;
						for (int i = 0; i < length; i++)
						{
							Complex* intPtr = ExtIO._iqPtr + i;
							short* intPtr2 = ptr;
							ptr = intPtr2 + 1;
							intPtr->Imag = (float)(*intPtr2) * 3.051851E-05f;
							Complex* intPtr3 = ExtIO._iqPtr + i;
							short* intPtr4 = ptr;
							ptr = intPtr4 + 1;
							intPtr3->Real = (float)(*intPtr4) * 3.051851E-05f;
						}
					}
					else if (ExtIO._hwType == HWTypes.Aud24BInt)
					{
						Int24* ptr2 = (Int24*)(void*)dataPtr;
						for (int j = 0; j < length; j++)
						{
							Complex* intPtr5 = ExtIO._iqPtr + j;
							Int24* intPtr6 = ptr2;
							ptr2 = intPtr6 + 1;
							intPtr5->Imag = (float)(*intPtr6) * 1.1920929E-07f;
							Complex* intPtr7 = ExtIO._iqPtr + j;
							Int24* intPtr8 = ptr2;
							ptr2 = intPtr8 + 1;
							intPtr7->Real = (float)(*intPtr8) * 1.1920929E-07f;
						}
					}
					else if (ExtIO._hwType == HWTypes.Aud32BInt)
					{
						int* ptr3 = (int*)(void*)dataPtr;
						for (int k = 0; k < length; k++)
						{
							Complex* intPtr9 = ExtIO._iqPtr + k;
							int* intPtr10 = ptr3;
							ptr3 = intPtr10 + 1;
							intPtr9->Imag = (float)(*intPtr10) * 4.656613E-10f;
							Complex* intPtr11 = ExtIO._iqPtr + k;
							int* intPtr12 = ptr3;
							ptr3 = intPtr12 + 1;
							intPtr11->Real = (float)(*intPtr12) * 4.656613E-10f;
						}
					}
					else if (ExtIO._hwType == HWTypes.Aud32BFloat)
					{
						float* ptr4 = (float*)(void*)dataPtr;
						for (int l = 0; l < length; l++)
						{
							Complex* intPtr13 = ExtIO._iqPtr + l;
							float* intPtr14 = ptr4;
							ptr4 = intPtr14 + 1;
							intPtr13->Imag = *intPtr14;
							Complex* intPtr15 = ExtIO._iqPtr + l;
							float* intPtr16 = ptr4;
							ptr4 = intPtr16 + 1;
							intPtr15->Real = *intPtr16;
						}
					}
					if (ExtIO.SamplesAvailable != null)
					{
						ExtIO.SamplesAvailable(null, ExtIO._iqPtr, length);
					}
				}
			}
			else if (status > 0)
			{
				int num = 0;
				switch (status)
				{
				case 100:
					ExtIO.logInfo("Status 100, SRChanged");
					num = ExtIO.GetHWSR();
					if (ExtIO.SampleRateChanged != null)
					{
						ExtIO.SampleRateChanged(num);
					}
					break;
				case 101:
					ExtIO.logInfo("Status 101, LOFreqChanged");
					num = ExtIO.GetHWLO();
					if (ExtIO.LOFreqChanged != null && num > 0)
					{
						ExtIO.LOFreqChanged(num);
					}
					break;
				case 102:
					ExtIO.logInfo("Status 102, ProhibitLO");
					if (ExtIO.ProhibitLOChanged != null)
					{
						ExtIO.ProhibitLOChanged();
					}
					break;
				case 103:
					ExtIO.logInfo("Status 103, LOChangeOK");
					if (ExtIO.LOFreqChangedAccepted != null)
					{
						ExtIO.LOFreqChangedAccepted();
					}
					break;
				case 104:
					ExtIO.logInfo("Status 104, LOChangedNoTune");
					num = ExtIO.GetHWLO();
					if (ExtIO.LOFreqChanged != null && num > 0)
					{
						ExtIO.LOFreqChanged(num);
					}
					break;
				case 105:
					ExtIO.logInfo("Status 105, TuneChanged");
					num = ExtIO.GetTune();
					if (ExtIO.TuneFreqChanged != null && num > 0)
					{
						ExtIO.TuneFreqChanged(num);
					}
					break;
				case 106:
					ExtIO.logInfo("Status 106, DemodChange");
					break;
				case 107:
					ExtIO.logInfo("Status 107, RsqStart");
					break;
				case 108:
					ExtIO.logInfo("Status 108, RsqStop");
					break;
				case 109:
					ExtIO.logInfo("FiltChange (109)");
					break;
				default:
					ExtIO.logInfo("Unknown status " + status.ToString() + " received from DLL.");
					break;
				}
			}
		}

		private static void logResult(string prefix)
		{
			Marshal.GetLastWin32Error();
			ExtIO.logInfo(prefix + new Win32Exception().Message);
		}

		private static void logInfo(string msg)
		{
			Console.WriteLine("ExtIO: " + msg);
		}
	}
}
