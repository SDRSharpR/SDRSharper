using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using SDRSharp.Radio;

namespace SDRSharp
{
	public static class Program
	{
		[DllImport("gdi32.dll")]
		private static extern int AddFontResource(string lpszFilename);
		[STAThread]
		private static void Main()
		{
			Utils.Log("Program start", true);
			string fontFile = "LCD-BOLD.TTF";
			string fontDestination = Environment.GetEnvironmentVariable("SystemRoot");
			if (fontDestination == null)
			{
				fontDestination = Environment.GetEnvironmentVariable("Windir");
			}
			if (fontDestination != null)
			{
				fontDestination = Path.Combine(fontDestination, "Fonts");
			}
			if (fontDestination != null)
			{
				fontDestination = Path.Combine(fontDestination, fontFile);
				if (!File.Exists(fontDestination))
				{
					try
					{
						File.Copy(Path.Combine(Directory.GetCurrentDirectory(), fontFile), fontDestination);
						Utils.Log("LCD font " + fontFile + " installed", false);
						PrivateFontCollection fontCol = new PrivateFontCollection();
						fontCol.AddFontFile(fontDestination);
						string actualFontName = fontCol.Families[0].Name;
						Program.AddFontResource(fontDestination);
						Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", actualFontName, fontFile, RegistryValueKind.String);
						MessageBox.Show("LCD font installed, please restart again.");
						return;
					}
					catch (UnauthorizedAccessException)
					{
						MessageBox.Show("Trying to install LCD font.\nplease right-click on SDRSharper.exe\nand once use 'Run as administrator'.");
					}
					catch (FileNotFoundException)
					{
						MessageBox.Show("LCD Fontfile " + fontFile + " not available.");
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				Process process = Process.GetCurrentProcess();
				process.PriorityBoostEnabled = true;
				process.PriorityClass = ProcessPriorityClass.RealTime;
				Utils.TimeBeginPeriod(1u);
			}
			Utils.ProcessorCount = 1;
			DSPThreadPool.Initialize();
			Utils.Log("Threadpool initialized", false);
			Control.CheckForIllegalCrossThreadCalls = false;
			Utils.Log("Explicitely enabled VisualStyles", false);
			Application.EnableVisualStyles();
			Utils.Log("Start FormMain", false);
			Application.Run(new MainForm());
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				Utils.TimeEndPeriod(1u);
			}
			DSPThreadPool.Terminate();
			Utils.Log("Program exit", false);
			Application.Exit();
		}
	}
}
