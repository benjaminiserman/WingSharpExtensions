namespace WingSharpExtensions;

using System.Collections.Generic;
using System.Linq;

public static class WithIndexExtension
{
	public static IEnumerable<(T Value, int Index)> WithIndex<T>(this IEnumerable<T> enumerable)
	{
		foreach (var t in enumerable.Select((x, i) => (x, i)))
		{
			yield return t;
		}
	}
}
