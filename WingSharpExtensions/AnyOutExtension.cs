namespace WingSharpExtensions;

using System;
using System.Collections.Generic;
using System.Linq;

public static class AnyOutExtension
{
	public static bool AnyOut<T>(this IEnumerable<T> enumerable, out T found)
	{
		if (enumerable.Any())
		{
			found = enumerable.First();
			return true;
		}
		else
		{
			found = default!;
			return false;
		}
	}

	public static bool AnyOut<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out T found)
	{
		foreach (var item in enumerable)
		{
			if (predicate(item))
			{
				found = item;
				return true;
			}
		}

		found = default!;
		return false;
	}
}
