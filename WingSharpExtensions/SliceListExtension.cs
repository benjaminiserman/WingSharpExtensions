namespace WingSharpExtensions;

using System.Collections.Generic;

public static class SliceListExtension
{
	public static SliceList<T> ToSliceList<T>(this IEnumerable<T> enumerable) => new(enumerable);
}
