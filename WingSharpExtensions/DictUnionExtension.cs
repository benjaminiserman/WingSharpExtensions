namespace WingSharpExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

public static class DictUnionExtension
{
	public static Dictionary<TKey, TValue> DictUnion<TKey, TValue>(
		this IDictionary<TKey, TValue> first, 
		Func<TValue, TValue, TValue> mergeFunction,
		IEqualityComparer<TKey>? equalityComparer,
		params IDictionary<TKey, TValue>[] rest)
		where TKey : notnull
	{
		var resultDict = new Dictionary<TKey, TValue>(first, equalityComparer);

		foreach (var dict in rest)
		{
			foreach (var kvp in dict)
			{
				if (resultDict.TryGetValue(kvp.Key, out var gotValue))
				{
					resultDict[kvp.Key] = mergeFunction(gotValue, kvp.Value);
				}
				else
				{
					resultDict.Add(kvp.Key, kvp.Value);
				}
			}
		}

		return resultDict;
	}

	public static Dictionary<TKey, TValue> DictUnion<TKey, TValue>(
		this IDictionary<TKey, TValue> first,
		Func<TValue, TValue, TValue> mergeFunction,
		params IDictionary<TKey, TValue>[] rest)
		where TKey : notnull
		=> DictUnion(first, mergeFunction, equalityComparer: null, rest);

	public static Dictionary<TKey, TValue> DictUnion<TKey, TValue>(
		this IDictionary<TKey, TValue> first,
		Func<TValue, TValue, TValue> mergeFunction,
		IEnumerable<IDictionary<TKey, TValue>> rest)
		where TKey : notnull
		=> DictUnion(first, mergeFunction, equalityComparer: null, rest.ToArray());

	public static Dictionary<TKey, TValue> DictUnion<TKey, TValue>(
		this IDictionary<TKey, TValue> first,
		Func<TValue, TValue, TValue> mergeFunction,
		IEqualityComparer<TKey> equalityComparer,
		IEnumerable<IDictionary<TKey, TValue>> rest)
		where TKey : notnull
		=> DictUnion(first, mergeFunction, equalityComparer, rest.ToArray());

	public static Dictionary<TKey, TValue> DictUnion<TKey, TValue>(
		this IEnumerable<IDictionary<TKey, TValue>> dicts,
		Func<TValue, TValue, TValue> mergeFunction)
		where TKey : notnull
		=> DictUnion(dicts.First(), mergeFunction, equalityComparer: null, dicts.Skip(1).ToArray());

	public static Dictionary<TKey, TValue> DictUnion<TKey, TValue>(
		this IEnumerable<IDictionary<TKey, TValue>> dicts,
		Func<TValue, TValue, TValue> mergeFunction,
		IEqualityComparer<TKey> equalityComparer)
		where TKey : notnull
		=> DictUnion(dicts.First(), mergeFunction, equalityComparer, dicts.Skip(1).ToArray());
}
