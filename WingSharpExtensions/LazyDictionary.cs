﻿namespace WingSharpExtensions;

using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class LazyDictionary<T1, T2> : IDictionary<T1, T2> 
	where T1: notnull
{
	private readonly Dictionary<T1, T2> _internalDictionary = new();

	public LazyDictionary(Dictionary<T1, T2> dictionary)
	{
		_internalDictionary = dictionary;
	}

	public T2 this[T1 key] 
	{
		get
		{
			if (_internalDictionary.TryGetValue(key, out var value))
			{
				return value;
			}
			else
			{
				return default;
			}
		}

		set
		{
			if (!_internalDictionary.TryAdd(key, value))
			{
				_internalDictionary[key] = value;
			}
		}
	}

	public ICollection<T1> Keys => _internalDictionary.Keys;
	public ICollection<T2> Values => _internalDictionary.Values;
	public int Count => _internalDictionary.Count;
	public bool IsReadOnly => false;

	public void Add(T1 key, T2 value) => this[key] = value;
	public void Add(KeyValuePair<T1, T2> item) => this[item.Key] = item.Value;
	public void Clear() => _internalDictionary.Clear();
	public bool Contains(KeyValuePair<T1, T2> item) =>_internalDictionary.Contains(item);
	public bool ContainsKey(T1 key) => _internalDictionary.ContainsKey(key);
	public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex) => throw new();
	IEnumerator<KeyValuePair<T1, T2>> IEnumerable<KeyValuePair<T1, T2>>.GetEnumerator() => _internalDictionary.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => _internalDictionary.GetEnumerator();
	public bool Remove(T1 key) => _internalDictionary.Remove(key);
	public bool Remove(T1 key, out T2 value) => _internalDictionary.Remove(key, out value);
	public bool Remove(KeyValuePair<T1, T2> item)
	{
		if (_internalDictionary.Contains(item))
		{
			_internalDictionary.Remove(item.Key);
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool TryGetValue(T1 key, out T2 value) => _internalDictionary.TryGetValue(key, out value);
}
