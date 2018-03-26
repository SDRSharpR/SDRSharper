using System;
using System.Collections.Generic;

namespace SDRSharp.FrequencyScanner
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue>
	{
		private List<TKey> _keys = new List<TKey>();

		private List<TValue> _values = new List<TValue>();

		public TValue this[TKey index]
		{
			get
			{
				int num = this._keys.IndexOf(index);
				if (num != -1)
				{
					return this._values[num];
				}
				throw new KeyNotFoundException();
			}
			set
			{
				int num = this._keys.IndexOf(index);
				if (num == -1)
				{
					this._keys.Add(index);
					this._values.Add(value);
				}
				else
				{
					this._values[num] = value;
				}
			}
		}

		public List<TKey> Keys
		{
			get
			{
				return this._keys;
			}
			set
			{
				this._keys = value;
			}
		}

		public List<TValue> Values
		{
			get
			{
				return this._values;
			}
			set
			{
				this._values = value;
			}
		}

		public bool ContainsKey(TKey key)
		{
			return this._keys.IndexOf(key) != -1;
		}

		public void Clear()
		{
			this._keys.Clear();
			this._values.Clear();
		}
	}
}
