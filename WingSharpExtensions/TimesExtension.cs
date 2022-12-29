namespace WingSharpExtensions;
using System;
using System.Collections.Generic;

public static class TimesExtension
{
	public static IEnumerable<T> Times<T>(this Func<T> function, int times)
	{
		for (var i = 0; i < times; i++)
		{
			yield return function();
		}
	}

	public static IEnumerable<T> Times<T>(this Func<int, T> function, int times)
	{
		for (var i = 0; i < times; i++)
		{
			yield return function(i);
		}
	}

	public static void Times(this Action function, int times)
	{
		for (var i = 0; i < times; i++)
		{
			function();
		}
	}

	public static void Times(this Action<int> function, int times)
	{
		for (var i = 0; i < times; i++)
		{
			function(i);
		}
	}
}
