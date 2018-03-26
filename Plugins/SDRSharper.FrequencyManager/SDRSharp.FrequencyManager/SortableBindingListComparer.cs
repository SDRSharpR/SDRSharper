using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SDRSharp.FrequencyManager
{
	public class SortableBindingListComparer<T> : IComparer<T>
	{
		private PropertyInfo _sortProperty;

		private ListSortDirection _sortDirection;

		public SortableBindingListComparer(string sortProperty, ListSortDirection sortDirection)
		{
			this._sortProperty = typeof(T).GetProperty(sortProperty);
			this._sortDirection = sortDirection;
		}

		public int Compare(T x, T y)
		{
			IComparable comparable = (IComparable)this._sortProperty.GetValue(x, null);
			IComparable comparable2 = (IComparable)this._sortProperty.GetValue(y, null);
			if (this._sortDirection == ListSortDirection.Ascending)
			{
				return comparable.CompareTo(comparable2);
			}
			return comparable2.CompareTo(comparable);
		}
	}
}
