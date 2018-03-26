using System;

namespace SDRSharp.Radio
{
	public class RdsDumpGroups
	{
		private char[] _radioTextSBA = new char[72];

		private char[] _radioTextSBB = new char[72];

		private char[] _programServiceSB = new char[12];

		private char[] _chars = new char[4];

		private string _radioText = string.Empty;

		private string _programService = string.Empty;

		private bool _radioTextABFlag;

		private ushort _piCode;

		public string RadioText => this._radioText;

		public string ProgramService => this._programService;

		public ushort PICode => this._piCode;

		public void Reset()
		{
			lock (this)
			{
				for (int i = 0; i < this._radioTextSBA.Length; i++)
				{
					this._radioTextSBA[i] = ' ';
				}
				for (int j = 0; j < this._radioTextSBB.Length; j++)
				{
					this._radioTextSBB[j] = ' ';
				}
				for (int k = 0; k < this._programServiceSB.Length; k++)
				{
					this._programServiceSB[k] = ' ';
				}
				this._radioText = string.Empty;
				this._programService = string.Empty;
				this._piCode = 0;
				this._radioTextABFlag = false;
			}
		}

		public bool AnalyseFrames(ushort groupA, ushort groupB, ushort groupC, ushort groupD)
		{
			bool result = false;
			if ((groupB & 0xF800) == 16384)
			{
				string value = RdsDumpGroups.Dump4A(groupB, groupC, groupD);
				Console.WriteLine(value);
			}
			if ((groupB & 0xF800) == 8192)
			{
				bool flag = (groupB & 0x10) > 0;
				int num = (groupB & 0xF) * 4;
				this._chars[0] = (char)(groupC >> 8);
				this._chars[1] = (char)(groupC & 0xFF);
				this._chars[2] = (char)(groupD >> 8);
				this._chars[3] = (char)(groupD & 0xFF);
				lock (this)
				{
					if (flag != this._radioTextABFlag)
					{
						this._radioTextABFlag = flag;
					}
					for (int i = 0; i < this._chars.Length; i++)
					{
						if (this._chars[i] >= ' ' && this._chars[i] <= '~')
						{
							if (!flag)
							{
								this._radioTextSBA[num + i] = this._chars[i];
							}
							else
							{
								this._radioTextSBB[num + i] = this._chars[i];
							}
						}
					}
					if (!flag)
					{
						this._radioText = new string(this._radioTextSBA).TrimEnd();
					}
					else
					{
						this._radioText = new string(this._radioTextSBB).TrimEnd();
					}
					this._piCode = groupA;
				}
				result = true;
			}
			if ((groupB & 0xF800) == 0)
			{
				int num2 = (groupB & 3) * 2;
				this._chars[0] = (char)(groupD >> 8);
				this._chars[1] = (char)(groupD & 0xFF);
				lock (this)
				{
					for (int j = 0; j < 2; j++)
					{
						if (this._chars[j] >= ' ' && this._chars[j] <= '~')
						{
							this._programServiceSB[num2 + j] = this._chars[j];
						}
					}
					this._programService = new string(this._programServiceSB).Substring(0, 8);
					this._piCode = groupA;
				}
				result = true;
			}
			return result;
		}

		private static string Dump4A(ushort blockB, ushort block3, ushort block4)
		{
			int num = block4 & 0x1F;
			if ((block4 & 0x20) != 0)
			{
				num *= -1;
			}
			int minute = block4 >> 6 & 0x3F;
			int hour = (block4 >> 12 & 0xF) | (block3 << 4 & 0x10);
			int num2 = block3 >> 1 | (blockB << 15 & 0x18000);
			int num3 = (int)(((double)num2 - 15078.2) / 365.25);
			int num4 = (int)(((double)num2 - 14956.1 - (double)(int)((double)num3 * 365.25)) / 30.6001);
			int day = num2 - 14956 - (int)((double)num3 * 365.25) - (int)((double)num4 * 30.6001);
			int num5 = 0;
			if (num4 == 14 || num4 == 15)
			{
				num5 = 1;
			}
			num3 = num3 + num5 + 1900;
			num4 = num4 - 1 - num5 * 12;
			try
			{
				DateTime d = new DateTime(num3, num4, day, hour, minute, 0);
				TimeSpan t = new TimeSpan(num / 2, num * 30 % 60, 0);
				d += t;
				return "4A " + d.ToLongDateString() + " " + d.ToLongTimeString();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
