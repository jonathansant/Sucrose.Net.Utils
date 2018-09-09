using System;
using System.Collections.Generic;
using System.Linq;

namespace Sucrose.Net.Utils.Collections
{
	public class SortedBucket<TComparableKey, TItem> : SortedList<TComparableKey, TItem>
		where TComparableKey : IComparable
	{
		private readonly int _sizeLimit;
		private readonly SortMode _sortMode;

		public SortedBucket<TComparableKey, TItem> Next { get; private set; }

		public SortedBucket<TComparableKey, TItem> Previous { get; private set; }

		public SortedBucket(int sizeLimit, SortMode mode)
			: base(new KeyComparer<TComparableKey>(mode))
		{
			_sortMode = mode;
			_sizeLimit = sizeLimit;
		}

		public new void Add(TComparableKey key, TItem value)
		{
			// todo: remeber to apply the IsFit Optimization
			base.Add(key, value);

			if (Count <= _sizeLimit)
				return;

			if (Next == null)
				Next = new SortedBucket<TComparableKey, TItem>(_sizeLimit, _sortMode);

			var lastIndex = Count - 1;
			var lastItem = this.Last().Value;
			var lastKey = Keys.Last();

			RemoveAt(lastIndex);
			Next.Add(lastKey, lastItem);
			Next.Previous = this;
		}

		public new void Remove(TComparableKey key)
		{
			if (Next == null || ( Next == null && Count <= _sizeLimit ))
			{
				base.Remove(key);
				return;
			}

			// todo: simply remove at the begining
			base.Remove(key);

			const int firstSibblingIndex = 0;
			var firstSibblingItem = Next.First().Value;
			var firstSibblingKey = Next.Keys.First();

			Next.RemoveAt(firstSibblingIndex);
			Add(firstSibblingKey, firstSibblingItem);

			// todo: fix previous
		}

		private bool IsFit(TComparableKey key)
		{
			var firstKey = Keys.First();
			var lastKey = Keys.Last();

			if (_sortMode == SortMode.Asc)
			{
				return Comparer.Compare(firstKey, key) <= 0 && Comparer.Compare(lastKey, key) >= 0;
			}

			return Comparer.Compare(firstKey, key) >= 0 && Comparer.Compare(lastKey, key) <= 0;
		}
	}

	public class KeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
	{
		private readonly SortMode _sortMode;

		public KeyComparer(SortMode mode)
		{
			_sortMode = mode;
		}

		public int Compare(TKey x, TKey y)
		{
			var result = _sortMode == SortMode.Asc ? x.CompareTo(y) : y.CompareTo(x);
			return result == 0 ? 1 : result;
		}
	}

	public enum SortMode
	{
		Desc = 0,
		Asc
	}
}