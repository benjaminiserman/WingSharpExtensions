namespace WingSharpExtensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public struct NRange<T>
	where T : INumber<T>
{
	public ImmutableArray<(T start, T end)> ranges;
	public NRange(params (T start, T end)[] ranges)
	{
		this.ranges = ranges.ToImmutableArray();
	}

	public NRange(ImmutableArray<(T start, T end)> ranges)
	{
		this.ranges = ranges.ToImmutableArray();
	}

	public override string ToString()
	{
		return string.Join(", ", ranges
			.Select(x => $"({x.start}, {x.end})"));
	}

	public bool Intersects(NRange<T> otherRange)
	{
		if (ranges.Length != otherRange.ranges.Length)
		{
			throw new ArgumentException($"{this} and {otherRange} do not have same dimensionality");
		}

		var intersects = true;
		for (var i = 0; i < ranges.Length; i++)
		{
			intersects &= ranges[i].start < otherRange.ranges[i].end 
				&& otherRange.ranges[i].start < ranges[i].end;
		}

		return intersects;
	}

	public IEnumerable<NRange<T>> Split(NRange<T> otherRange) 
	{
		for (var i = 0; i < ranges.Length; i++)
		{
			if(ranges[i].start < otherRange.ranges[i].end
				&& otherRange.ranges[i].start < ranges[i].end)
			{

			}
		}

		yield break;
	}
}
