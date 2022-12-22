namespace WingSharpExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

internal class SliceList<T> : List<T>
{
	public IList<T> this[Range r]
	{
		get
		{
			var slice = new List<T>();
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

			var list = this.Take(start).ToList();
			if (r.Start.Value <= r.End.Value)
			{
				foreach (var x in value)
				{
					list.Add(x);
				}
			}
			else
			{
				foreach (var x in value.Reverse())
				{
					list.Add(x);
				}
			}

			list.AddRange(this.TakeLast(this.Count - end));

			this.Clear();
			this.AddRange(list);
		}
	}
}
