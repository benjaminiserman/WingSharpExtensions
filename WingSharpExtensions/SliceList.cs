namespace WingSharpExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

public class SliceList<T> : List<T>
{
	public SliceList(IEnumerable<T> enumerable) : base(enumerable) { }
	public SliceList() : base() { }

	public SliceList<T> this[Range r]
	{
		get
		{
			var slice = new SliceList<T>();
			foreach (var index in r)
			{
				slice.Add(this[index]);
			}

			return slice;
		}

		set
		{
			var start = Math.Min(r.Start.Value, r.End.Value);
			var end = Math.Max(r.Start.Value, r.End.Value);

			if (start < 0 || start >= this.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(r), start, $"Range start is out of bounds.");
			}

			if (end < 0 || end >= this.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(r), end, $"Range end is out of bounds.");
			}

			var list = new SliceList<T>(this.Take(start));
			if (value is not null)
			{
				if (r.Start.Value <= r.End.Value)
				{
					foreach (var x in value)
					{
						list.Add(x);
					}
				}
				else
				{
					foreach (var x in ((IEnumerable<T>)value).Reverse())
					{
						list.Add(x);
					}
				}
			}

			list.AddRange(this.TakeLast(this.Count - end));

			this.Clear();
			this.AddRange(list);
		}
	}
}
