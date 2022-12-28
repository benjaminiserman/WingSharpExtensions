namespace WingSharpExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ZipExtension
{
	public static IEnumerable<T[]> Zip<T>(this IEnumerable<T> first, params IEnumerable<T>[] list)
	{
		var array = new IEnumerable<T>[list.Length + 1];
		Array.Copy(list, array, list.Length);
		array[^1] = first;

		return Zip(array);
	}

	public static IEnumerable<T[]> Zip<T>(params IEnumerable<T>[] list)
	{
		var enumerators = list.Select(x => x.GetEnumerator());
		var halted = false;
		var size = list.Length;
		T[] GetRow()
		{
			var row = new T[size];
			var i = 0;
			foreach (var enumerator in enumerators)
			{
				if (enumerator.MoveNext())
				{
					row[i] = enumerator.Current;
				}
				else
				{
					halted = true;
					break;
				}

				i++;
			}

			return row;
		}

		while (true)
		{
			var row = GetRow();
			if (halted)
			{
				break;
			}
			else
			{
				yield return row;
			}
		}
	}

	public static IEnumerable<T2> Zip<T1, T2>(Func<T1[], T2> selector, params IEnumerable<T1>[] list) => Zip(list).Select(selector);

	public static IEnumerable<T2> Zip<T1, T2>(this IEnumerable<T1> first, Func<T1[], T2> selector, params IEnumerable<T1>[] list) => first.Zip(list).Select(selector);

	public static IEnumerable<T2> ZipInPlace<T1, T2>(Func<T1[], T2> selector, params IEnumerable<T1>[] list)
	{
		var enumerators = list.Select(x => x.GetEnumerator());
		var halted = false;
		var size = list.Length;
		var buffer = new T1[size];
		void GetRow()
		{
			var i = 0;
			foreach (var enumerator in enumerators)
			{
				if (enumerator.MoveNext())
				{
					buffer[i] = enumerator.Current;
				}
				else
				{
					halted = true;
					break;
				}

				i++;
			}
		}

		while (true)
		{
			GetRow();
			if (halted)
			{
				break;
			}
			else
			{
				yield return selector(buffer);
			}
		}
	}
}
