namespace WingSharpExtensions;
using System.Collections.Generic;
using System.Linq;

public static class ArrayDestructuringExtension
{
	public static void Deconstruct<T>(this IList<T> list, out T item1, out T[] rest)
	{
		item1 = list[0];
		rest = list.Skip(1).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		rest = list.Skip(2).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T item3, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		item3 = list[2];
		rest = list.Skip(3).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T item3, out T item4, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		item3 = list[2];
		item4 = list[3];
		rest = list.Skip(4).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T item3, out T item4, out T item5, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		item3 = list[2];
		item4 = list[3];
		item5 = list[4];
		rest = list.Skip(5).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		item3 = list[2];
		item4 = list[3];
		item5 = list[4];
		item6 = list[5];
		rest = list.Skip(6).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		item3 = list[2];
		item4 = list[3];
		item5 = list[4];
		item6 = list[5];
		item7 = list[6];
		rest = list.Skip(7).ToArray();
	}

	public static void Deconstruct<T>(this IList<T> list, out T item1, out T item2, out T item3, out T item4, out T item5, out T item6, out T item7, out T item8, out T[] rest)
	{
		item1 = list[0];
		item2 = list[1];
		item3 = list[2];
		item4 = list[3];
		item5 = list[4];
		item6 = list[5];
		item7 = list[6];
		item8 = list[7];
		rest = list.Skip(8).ToArray();
	}
}
