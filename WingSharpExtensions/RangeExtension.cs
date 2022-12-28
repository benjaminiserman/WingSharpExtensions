namespace WingSharpExtensions;

using System;
using System.Collections.Generic;
using System.Text;

public static class RangeExtension
{
    private static IEnumerator<int> RangeEnumerator(int start, int end)
    {
		foreach (var x in Range(start, end))
		{
			yield return x;
		}
	}

	public static IEnumerable<int> Range(int start, int end)
	{
		if (start < end)
		{
			for (var i = start; i < end; i++)
			{
				yield return i;
			}
		}
		else if (start > end)
		{
			for (var i = start; i > end; i--)
			{
				yield return i;
			}
		}
	}

	public static IEnumerator<int> GetEnumerator(this Range range) => RangeEnumerator(range.Start.Value, range.End.Value);


	public static IEnumerable<int> Enumerate(this Range range)
    {
        foreach (var x in range)
        {
            yield return x;
        }
    }
}
