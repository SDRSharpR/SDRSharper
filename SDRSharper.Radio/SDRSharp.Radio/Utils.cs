using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SDRSharp.Radio
{
	public static class Utils
	{
        public static Assembly UnmanagedDLLAssemblyVer = Assembly.GetExecutingAssembly();

        private const string Libc = "msvcrt.dll";

		private static SortedDictionary<string, string> _settings = new SortedDictionary<string, string>();

		public static int ProcessorCount;

		public static bool Chk1;

		public static float Value1 = 1f;

		public static bool ChkAver;

		public static bool FastFFT;

		public static bool FastConvolve;

		public static int PhaseAverage;

		public static int PhaseGain;

		private static Pen _outlinePen = new Pen(Color.Black);

		private static FontFamily _fontFamily = new FontFamily("Arial");

		public static SortedDictionary<string, string> Settings
		{
			get
			{
				return Utils._settings;
			}
			set
			{
				Utils._settings = value;
			}
		}

		public static void Log(string msg, bool create = false)
		{
			string path = "Debug_" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
			if (create)
			{
				File.Delete(path);
			}
			using (StreamWriter streamWriter = new StreamWriter(path, true))
			{
				streamWriter.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff") + " - " + msg);
			}
		}

		public static Color ChangeColor(Color col, int delta)
		{
			int val = col.R + delta;
			int val2 = col.G + delta;
			int val3 = col.B + delta;
			return Color.FromArgb(Math.Min(255, Math.Max(0, val)), Math.Min(255, Math.Max(0, val2)), Math.Min(255, Math.Max(0, val3)));
		}

		public static Color ChangeColor(Color col, float factor)
		{
			int val = (int)((float)(int)col.R * factor);
			int val2 = (int)((float)(int)col.G * factor);
			int val3 = (int)((float)(int)col.B * factor);
			return Color.FromArgb(Math.Min(255, Math.Max(0, val)), Math.Min(255, Math.Max(0, val2)), Math.Min(255, Math.Max(0, val3)));
		}

		public static ColorBlend BackGroundBlend(Color color, int height, int margin)
		{
			ColorBlend colorBlend = new ColorBlend(3);
			colorBlend.Colors = new Color[3]
			{
				Color.Black,
				color,
				Color.Black
			};
			colorBlend.Positions = new float[3]
			{
				0f,
				(float)(height - margin) / (float)(height + margin),
				1f
			};
			return colorBlend;
		}

		public static PathGradientBrush BackgroundBrush(string name, Color color, Rectangle r, bool spectrum, bool border)
		{
			return Utils.BackgroundBrush(name, color, r.Width, r.Height, spectrum, border);
		}

		public static PathGradientBrush BackgroundBrush(string Name, Color color, int width, int height, bool spectrum, bool border)
		{
			if (width != 0 && height != 0)
			{
				using (GraphicsPath graphicsPath = new GraphicsPath())
				{
					if (Name.Contains("spectrum"))
					{
						graphicsPath.AddRectangle(new Rectangle(-1 * width, (int)(-0.1 * (double)height), 3 * width, (int)(1.2 * (double)height)));
					}
					else if (Name.Contains("Analyzer"))
					{
						graphicsPath.AddRectangle(new Rectangle((int)(-0.05 * (double)width), (int)(-0.1 * (double)height), (int)(1.1 * (double)width), (int)(1.2 * (double)height)));
					}
					else if (Name.Contains("cope"))
					{
						graphicsPath.AddRectangle(new Rectangle(0, 0, width, height));
					}
					else
					{
						graphicsPath.AddRectangle(new Rectangle((int)(-0.1 * (double)width), (int)(-0.1 * (double)height), (int)(1.2 * (double)width), (int)(1.2 * (double)height)));
					}
					int num = 100;
					PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
					if (Name.Contains("spectrum"))
					{
						pathGradientBrush.SurroundColors = new Color[1]
						{
							Utils.ChangeColor(color, 0.1f)
						};
						pathGradientBrush.CenterColor = Utils.ChangeColor(color, 0.9f);
						pathGradientBrush.CenterPoint = new Point(width / 2, height * 2);
					}
					else if (Name.Contains("Analyzer"))
					{
						pathGradientBrush.SurroundColors = new Color[1]
						{
							Utils.ChangeColor(color, 0.1f)
						};
						pathGradientBrush.CenterColor = Utils.ChangeColor(color, 0.7f);
						pathGradientBrush.CenterPoint = new Point(width / 2, (int)((double)height * 1.5));
					}
					else if (Name.Contains("cope"))
					{
						pathGradientBrush.SurroundColors = new Color[1]
						{
							Utils.ChangeColor(color, 0.1f)
						};
						pathGradientBrush.CenterColor = Utils.ChangeColor(color, (Name == "scope") ? 0.6f : 0.5f);
						pathGradientBrush.CenterPoint = new Point(width / 2, height / 2);
						num = 40;
					}
					else
					{
						pathGradientBrush.SurroundColors = new Color[1]
						{
							Utils.ChangeColor(color, 0.2f)
						};
						pathGradientBrush.CenterColor = Utils.ChangeColor(color, 0.5f);
						pathGradientBrush.CenterPoint = new Point(width / 2, (int)((double)height * 0.66));
					}
					Blend blend = new Blend(6);
					blend.Positions = new float[6]
					{
						0f,
						0.2f,
						0.35f,
						0.43f,
						0.5f,
						1f
					};
					blend.Factors = new float[6]
					{
						0f,
						0.6f,
						0.8f,
						0.9f,
						0.95f,
						1f
					};
					float num2 = (float)num / (float)Math.Min(width, height);
					for (int i = 0; i < blend.Positions.Length - 1; i++)
					{
						blend.Positions[i] *= num2 / 0.5f;
					}
					pathGradientBrush.Blend = blend;
					return pathGradientBrush;
				}
			}
			return null;
		}

		public unsafe static void ManagedMemcpy(void* dest, void* src, int len)
		{
			byte* ptr = (byte*)dest;
			byte* ptr2 = (byte*)src;
			if (len >= 16)
			{
				do
				{
					*(int*)ptr = *(int*)ptr2;
					*(int*)(ptr + 4) = *(int*)(ptr2 + 4);
					*(int*)(ptr + 8) = *(int*)(ptr2 + 8);
					*(int*)(ptr + 12) = *(int*)(ptr2 + 12);
					ptr += 16;
					ptr2 += 16;
				}
				while ((len -= 16) >= 16);
			}
			if (len > 0)
			{
				if ((len & 8) != 0)
				{
					*(int*)ptr = *(int*)ptr2;
					*(int*)(ptr + 4) = *(int*)(ptr2 + 4);
					ptr += 8;
					ptr2 += 8;
				}
				if ((len & 4) != 0)
				{
					*(int*)ptr = *(int*)ptr2;
					ptr += 4;
					ptr2 += 4;
				}
				if ((len & 2) != 0)
				{
					*(short*)ptr = *(short*)ptr2;
					ptr += 2;
					ptr2 += 2;
				}
				if ((len & 1) != 0)
				{
					*ptr = *ptr2;
				}
			}
		}

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "memcpy")]
		public unsafe static extern void* Memcpy(void* dest, void* src, int len);

		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
		public static extern uint TimeBeginPeriod(uint uMilliseconds);

		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
		public static extern uint TimeEndPeriod(uint uMilliseconds);

		public static string GetSetting(string name)
		{
			if (Utils.Settings.ContainsKey(name))
			{
				return Utils.Settings[name];
			}
			Console.WriteLine("XML Setting '" + name + "' not found.");
			try
			{
				return ConfigurationManager.AppSettings[name];
			}
			catch
			{
				return "App Setting '" + name + "' not found.";
			}
		}

		public static double GetDoubleSetting(string name, double defaultValue)
		{
			string setting = Utils.GetSetting(name);
			if (double.TryParse(setting, NumberStyles.Number, (IFormatProvider)CultureInfo.InvariantCulture, out double result))
			{
				return result;
			}
			return defaultValue;
		}

		public static bool GetBooleanSetting(string name)
		{
			string str;
			try
			{
				str = (Utils.GetSetting(name) ?? string.Empty);
			}
			catch
			{
				return false;
			}
			str += " ";
			return "YyTt".IndexOf(str[0]) >= 0;
		}

		public static Color GetColorSetting(string name, Color defaultColor)
		{
			try
			{
				string setting = Utils.GetSetting(name);
				int red = int.Parse(setting.Substring(0, 2), NumberStyles.HexNumber);
				int green = int.Parse(setting.Substring(2, 2), NumberStyles.HexNumber);
				int blue = int.Parse(setting.Substring(4, 2), NumberStyles.HexNumber);
				return Color.FromArgb(red, green, blue);
			}
			catch
			{
				return defaultColor;
			}
		}

		public static ColorBlend GetGradientBlend(int alpha, string settingName)
		{
			ColorBlend colorBlend = new ColorBlend();
			string text;
			try
			{
				text = (Utils.GetSetting(settingName) ?? string.Empty);
			}
			catch
			{
				text = string.Empty;
			}
			string[] array = text.Split(',');
			if (array.Length < 2)
			{
				colorBlend.Colors = new Color[6]
				{
					Color.White,
					Color.LightBlue,
					Color.DodgerBlue,
					Color.FromArgb(0, 0, 80),
					Color.Black,
					Color.Black
				};
				for (int i = 0; i < colorBlend.Colors.Length; i++)
				{
					colorBlend.Colors[i] = Color.FromArgb(alpha, colorBlend.Colors[i]);
				}
			}
			else
			{
				colorBlend.Colors = new Color[array.Length];
				for (int j = 0; j < array.Length; j++)
				{
					string text2 = array[j];
					int red = int.Parse(text2.Substring(0, 2), NumberStyles.HexNumber);
					int green = int.Parse(text2.Substring(2, 2), NumberStyles.HexNumber);
					int blue = int.Parse(text2.Substring(4, 2), NumberStyles.HexNumber);
					colorBlend.Colors[j] = Color.FromArgb(red, green, blue);
				}
			}
			float[] array2 = new float[colorBlend.Colors.Length];
			float num = 1f / (float)(array2.Length - 1);
			for (int k = 0; k < array2.Length; k++)
			{
				byte r = colorBlend.Colors[k].R;
				byte g = colorBlend.Colors[k].G;
				byte b = colorBlend.Colors[k].B;
				colorBlend.Colors[k] = Color.FromArgb(alpha, r, g, b);
				array2[k] = (float)k * num;
			}
			colorBlend.Positions = array2;
			return colorBlend;
		}

		public static void AddString(GraphicsPath path, string text, float fontSize, int x, int y)
		{
			path.AddString(text, Utils._fontFamily, 0, fontSize, new Point(x, y), StringFormat.GenericTypographic);
		}

		public static void DrawPath(Graphics graphics, GraphicsPath path, Brush brush, int outlineWidth)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			InterpolationMode interpolationMode = graphics.InterpolationMode;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Utils._outlinePen.Width = (float)outlineWidth;
			graphics.DrawPath(Utils._outlinePen, path);
			graphics.FillPath(brush, path);
			graphics.SmoothingMode = smoothingMode;
			graphics.InterpolationMode = interpolationMode;
		}

		public static int GetIntSetting(string name, int defaultValue)
		{
			string setting = Utils.GetSetting(name);
			if (int.TryParse(setting, out int result))
			{
				return result;
			}
			return defaultValue;
		}

		public static long GetLongSetting(string name, long defaultValue)
		{
			string setting = Utils.GetSetting(name);
			if (long.TryParse(setting, out long result))
			{
				return result;
			}
			return defaultValue;
		}

		public static string IntArrayToString(params int[] values)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int value in values)
			{
				stringBuilder.Append(value);
				stringBuilder.Append(',');
			}
			return stringBuilder.ToString().TrimEnd(',');
		}

		public static int[] GetIntArraySetting(string name, int[] defaultValue)
		{
			try
			{
				string setting = Utils.GetSetting(name);
				string[] array = setting.Split(',');
				int num = array.Length;
				if (defaultValue != null && defaultValue.Length > num)
				{
					num = defaultValue.Length;
				}
				int[] array2 = new int[num];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = int.Parse(array[i]);
				}
				if (defaultValue != null)
				{
					for (int j = array.Length; j < defaultValue.Length; j++)
					{
						array2[j] = defaultValue[j];
					}
				}
				return array2;
			}
			catch
			{
				return defaultValue;
			}
		}

		public static string GetStringSetting(string name, string defaultValue)
		{
			string setting = Utils.GetSetting(name);
			if (string.IsNullOrEmpty(setting))
			{
				return defaultValue;
			}
			return setting;
		}

		public static void SaveSetting(string key, object value)
		{
			string value2 = Convert.ToString(value, CultureInfo.InvariantCulture);
			Utils.SaveSetting(key, value2);
		}

		public static void SaveSetting(string key, string value)
		{
			Utils.Settings[key] = value;
		}

		public static int Val(string str, int def = 0)
		{
			if (!int.TryParse(str, out int result))
			{
				result = def;
				return result;
			}
			return result;
		}

		public static double ValD(string str, double def = 0.0)
		{
			if (!double.TryParse(str, NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture, out double result))
			{
				result = def;
				return result;
			}
			return result;
		}

		public static float ValF(string str, float def = 0f)
		{
			if (!float.TryParse(str, NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture, out float result))
			{
				result = def;
				return result;
			}
			return result;
		}

		public static string Signal(int dBm, int showDbm, bool unit)
		{
			switch (showDbm)
			{
			case 0:
				return string.Format(unit ? "{0:##0.0}dBm" : "{0:##0}", dBm);
			case 1:
				return string.Format(unit ? "{0:##0.0}dBuv" : "{0:##0}", dBm + 107);
			case 3:
				return string.Format(unit ? "{0:##0} %" : "{0:##0}", Math.Max(0, dBm + 133));
			default:
				if (dBm > -73)
				{
					return string.Format(unit ? "S9+{0:#0}" : "+{0:#0}", dBm + 73);
				}
				if (dBm + 127 < 0)
				{
					return "";
				}
				return string.Format(unit ? "S{0:0.#}" : "S{0:0}", (float)(dBm + 127) / 6f);
			}
		}

		public unsafe static void ClrBuf(float* acc, int length)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i] = 0f;
			}
		}

		public unsafe static void ClrBuf(Complex* acc, int length)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i].Real = 0f;
				acc[i].Imag = 0f;
			}
		}

		public unsafe static void AddBufs(float* acc, float* buf, int length)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i] += buf[i];
			}
		}

		public unsafe static void AddBufs(Complex* acc, Complex* buf, int length)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i].Real += buf[i].Real;
				acc[i].Imag += buf[i].Imag;
			}
		}

		public unsafe static void RemBufs(float* acc, float* buf, int length)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i] -= buf[i];
			}
		}

		public unsafe static void RemBufs(Complex* acc, Complex* buf, int length)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i].Real -= buf[i].Real;
				acc[i].Imag -= buf[i].Imag;
			}
		}

		public unsafe static void DivBuf(float* acc, int length, int cnt)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i] /= (float)cnt;
			}
		}

		public unsafe static void DivBuf(Complex* acc, int length, int cnt)
		{
			for (int i = 0; i < length; i++)
			{
				acc[i].Real /= (float)cnt;
				acc[i].Imag /= (float)cnt;
			}
		}

		public static long niceVal(float value, int n = 0)
		{
			int num = (int)Math.Log10((double)value) - n;
			long num2 = (long)Math.Pow(10.0, (double)num);
			return (long)value / num2 * num2;
		}

		public static Color HslToRgb(float H, float S, float L)
		{
			float num3;
			float num2;
			float num;
			if (S == 0f)
			{
				num3 = (num2 = (num = L));
			}
			else
			{
				float num4 = ((double)L < 0.5) ? (L * (1f + S)) : (L + S - L * S);
				float p = 2f * L - num4;
				num3 = Utils.hue2rgb(p, num4, H + 0.333333343f);
				num2 = Utils.hue2rgb(p, num4, H);
				num = Utils.hue2rgb(p, num4, H - 0.333333343f);
			}
			return Color.FromArgb((int)Math.Round((double)(num3 * 255f)), (int)Math.Round((double)(num2 * 255f)), (int)Math.Round((double)(num * 255f)));
		}

		private static float hue2rgb(float p, float q, float t)
		{
			if (t < 0f)
			{
				t += 1f;
			}
			if (t > 1f)
			{
				t -= 1f;
			}
			if (6f * t < 1f)
			{
				return p + (q - p) * 6f * t;
			}
			if (2f * t < 1f)
			{
				return q;
			}
			if (3f * t < 2f)
			{
				return p + (q - p) * (0.6666667f - t) * 6f;
			}
			return p;
		}

		public static Hsl RgbToHsl(Color color)
		{
			return new Hsl(color.GetHue() / 360f, color.GetSaturation(), color.GetBrightness());
		}
	}
}
