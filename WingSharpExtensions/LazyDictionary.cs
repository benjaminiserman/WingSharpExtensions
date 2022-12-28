namespace WingSharpExtensions;

using System;
using System.Collections;
using System.Collections.Generic;

public class LazyDictionary<T1, T2> : IDictionary<T1, T2>
	where T1 : notnull
{
	private readonly Dictionary<T1, T2> _internalDictionary;

	public Func<T2>? GetDefault { get; init; } = null;
	public bool AddMissingKeys { get; init; } = false;

	public LazyDictionary() 
	{
		_internalDictionary = new();
	}

	public LazyDictionary(Dictionary<T1, T2> dictionary)
	{
		_internalDictionary = new(dictionary);
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
				if (GetDefault is null)
				{
					return default!;
				}
				else
				{
					var newValue = GetDefault();
					if (AddMissingKeys)
					{
						_internalDictionary.Add(key, newValue);
					}

					return newValue;
				}
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
	void ICollection<KeyValuePair<T1, T2>>.Add(KeyValuePair<T1, T2> item) => ((ICollection<KeyValuePair<T1, T2>>)_internalDictionary).Add(item);
	public void Clear() => _internalDictionary.Clear();
	public bool ContainsKey(T1 key) => _internalDictionary.ContainsKey(key);
	IEnumerator<KeyValuePair<T1, T2>> IEnumerable<KeyValuePair<T1, T2>>.GetEnumerator() => _internalDictionary.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => _internalDictionary.GetEnumerator();
	public bool Remove(T1 key) => _internalDictionary.Remove(key);
	public bool Remove(T1 key, out T2 value) => _internalDictionary.Remove(key, out value!);

	public bool TryGetValue(T1 key, out T2 value) => _internalDictionary.TryGetValue(key, out value!);
	public bool Contains(KeyValuePair<T1, T2> item) => ((ICollection<KeyValuePair<T1, T2>>)_internalDictionary).Contains(item);
	public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex) => ((ICollection<KeyValuePair<T1, T2>>)_internalDictionary).CopyTo(array, arrayIndex);
	public bool Remove(KeyValuePair<T1, T2> item) => ((ICollection<KeyValuePair<T1, T2>>)_internalDictionary).Remove(item);
}
