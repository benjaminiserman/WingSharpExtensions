namespace WingSharpExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

public class SliceList<T> : List<T>, IEnumerable<T>
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
			var start = r.Start.Value;
			var end = r.End.Value;

			if (Math.Max(start, end) < 0
				|| Math.Min(start, end) >= this.Count
				|| start <= -1
				|| end <= -1
				|| start > this.Count
				|| end > this.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(r), start, $"Range start or end is out of bounds.");
			}

			SliceList<T> list;
			if (start <= end)
			{
				list = new SliceList<T>(this.Take(start));
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
			}
			else
			{
				list = new SliceList<T>(this.Take(end + 1));
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

				list.AddRange(this.TakeLast(this.Count - start - 1));
			}



			this.Clear();
			this.AddRange(list);
		}
	}
}
