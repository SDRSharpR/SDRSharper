using System;
using System.Collections.Generic;

namespace SDRSharp.Trunker
{
	public class LogParser
	{
		public static string ParseLogStyle(TrunkerLogger _trunkLoggerData, string logStyle = null, string parkedString = null, string unkString = null, bool ignoreParked = false)
		{
			if (logStyle == null || parkedString == null || unkString == null || !ignoreParked)
			{
				string[] settings = LogParser.getSettings();
				if (logStyle == null)
				{
					logStyle = settings[0];
				}
				if (parkedString == null)
				{
					parkedString = settings[1];
				}
				if (unkString == null)
				{
					unkString = settings[2];
				}
				if (!ignoreParked)
				{
					ignoreParked = Convert.ToBoolean(settings[3]);
				}
			}
			_trunkLoggerData = LogParser.PrepareLogData(_trunkLoggerData, parkedString, unkString);
			if (_trunkLoggerData.currentAction == "Park" && ignoreParked)
			{
				return null;
			}
			string text = logStyle;
			int num = -1;
			int num2 = num;
			while (true)
			{
				if (num + 1 > text.Length)
				{
					break;
				}
				num = text.IndexOf('[', num + 1);
				if (num < 0)
				{
					break;
				}
				int num3 = text.IndexOf(']', num);
				if (num3 >= 0)
				{
					string text2 = text.Substring(num, num3 - num + 1);
					string text3 = text2;
					int num4 = text2.IndexOf('%');
					if (num4 > 0)
					{
						string text4 = (text3.Length < num4 + 4) ? text2.Substring(num4, 2) : text2.Substring(num4, 4);
						switch (text4)
						{
						case "%tid":
							text3 = LogParser.filterData(text2, text4, _trunkLoggerData.currentTrunkgroup);
							break;
						case "%sid":
							text3 = LogParser.filterData(text2, text4, _trunkLoggerData.currentSourcegroup);
							break;
						default:
						{
							text4 = text4.Substring(0, 2);
							string a;
							if ((a = text4) != null && a == "%s")
							{
								text3 = LogParser.filterData(text2, text4, _trunkLoggerData.currentSourcelabel);
							}
							break;
						}
						}
						if (text2 != text3)
						{
							text = text.Replace(text2, text3);
						}
					}
					num2 = num3;
				}
				if (num2 <= num)
				{
					break;
				}
				num = num2;
			}
			text = text.Replace("%fk", (_trunkLoggerData.currentFrequency / 1000m).ToString());
			text = text.Replace("%fm", (_trunkLoggerData.currentFrequency / 1000000m).ToString());
			text = text.Replace("%f", _trunkLoggerData.currentFrequency.ToString());
			text = text.Replace("%tid", _trunkLoggerData.currentTrunkgroup);
			text = text.Replace("%t", _trunkLoggerData.currentTrunklabel);
			text = text.Replace("%sid", _trunkLoggerData.currentSourcegroup);
			text = text.Replace("%s", _trunkLoggerData.currentSourcelabel);
			text = text.Replace("%r", _trunkLoggerData.currentReceiver);
			return text.Replace("%a", _trunkLoggerData.currentAction);
		}

		private static string filterData(string source, string var, object loggerData)
		{
			if (loggerData != null)
			{
				source = source.Replace(var, loggerData.ToString());
				source = source.Remove(source.IndexOf('[', 0), 1);
				source = source.Remove(source.IndexOf(']', 0), 1);
			}
			else
			{
				source = null;
			}
			return source;
		}

		private static TrunkerLogger PrepareLogData(TrunkerLogger _trunkLoggerData, string parkedStr, string unkString)
		{
			if (_trunkLoggerData.currentAction == "Park")
			{
				_trunkLoggerData.currentTrunklabel = parkedStr;
			}
			if (_trunkLoggerData.currentAction == "Listen")
			{
				if (_trunkLoggerData.currentTrunklabel == null || _trunkLoggerData.currentTrunklabel.Length == 0)
				{
					_trunkLoggerData.currentTrunklabel = _trunkLoggerData.currentTrunkgroup;
				}
				if (_trunkLoggerData.currentSourcelabel == null || _trunkLoggerData.currentSourcelabel.Length == 0)
				{
					_trunkLoggerData.currentSourcelabel = _trunkLoggerData.currentSourcegroup;
				}
				if (_trunkLoggerData.currentSourcelabel == null || _trunkLoggerData.currentSourcelabel.Length == 0)
				{
					_trunkLoggerData.currentSourcelabel = unkString;
				}
			}
			return _trunkLoggerData;
		}

		private static string[] getSettings()
		{
			SettingsPersisterTrunker settingsPersisterTrunker = new SettingsPersisterTrunker();
			List<TrunkerSettings> list = settingsPersisterTrunker.readConfig();
			string text = list[0].logStyle ?? "%t %fm MHz";
			string text2 = list[0].parkedStr ?? "Parked";
			string text3 = list[0].unknownSrcStr ?? "Unknown";
			bool ignoreParked = list[0].ignoreParked;
			return new string[4]
			{
				text,
				text2,
				text3,
				ignoreParked.ToString()
			};
		}
	}
}
