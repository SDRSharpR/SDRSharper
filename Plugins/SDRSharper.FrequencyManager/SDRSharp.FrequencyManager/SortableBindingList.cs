using System.Collections.Generic;
using System.ComponentModel;

namespace SDRSharp.FrequencyManager
{
	public class SortableBindingList<T> : BindingList<T>
	{
		private bool _isSorted;

		private PropertyDescriptor _sortProperty;

		private ListSortDirection _sortDirection;

		protected override bool SupportsSortingCore => true;

		protected override ListSortDirection SortDirectionCore => this._sortDirection;

		protected override PropertyDescriptor SortPropertyCore => this._sortProperty;

		protected override bool IsSortedCore => this._isSorted;

		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
		{
			List<T> list = (List<T>)base.Items;
			if (list != null)
			{
				SortableBindingListComparer<T> comparer = new SortableBindingListComparer<T>(property.Name, direction);
				list.Sort(comparer);
				this._isSorted = true;
			}
			else
			{
				this._isSorted = false;
			}
			this._sortProperty = property;
			this._sortDirection = direction;
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
	}
}
