namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class LazyDictionaryTests
{
	private void AssertDictionariesAreEqual<T1, T2>(Dictionary<T1, T2> dictionary, LazyDictionary<T1, T2> lazyDictionary)
		where T1 : notnull
	{
		Assert.AreEqual(dictionary.Count, lazyDictionary.Count);

		Assert.AreEqual(dictionary.Comparer, lazyDictionary.Comparer);

		foreach (var kvp in dictionary)
		{
			Assert.IsTrue(lazyDictionary.ContainsKey(kvp.Key));
			Assert.AreEqual(kvp.Value, lazyDictionary[kvp.Key]);
		}

		foreach (var kvp in lazyDictionary)
		{
			Assert.IsTrue(dictionary.ContainsKey(kvp.Key));
			Assert.AreEqual(kvp.Value, dictionary[kvp.Key]);
		}
	}

	private void GetTestDictionaries(out Dictionary<string, int> dictionary, out LazyDictionary<string, int> lazyDictionary, Func<string, int> getDefault = null!, bool addMissingKeys = false)
	{
		dictionary = Enumerable.Range(0, 10).ToDictionary(x => x.ToString(), x => x);
		lazyDictionary = new(dictionary)
		{
			GetDefault = getDefault,
			AddMissingKeys = addMissingKeys,
		};
	}

	#region LazyDictionaryDiffersFromDictionary
	[TestMethod]
	public void IndexerGet_KeyDoesNotExist_Default()
	{
		var key = "15";
		GetTestDictionaries(out _, out var lazyDictionary);

		Assert.AreEqual(default, lazyDictionary[key]);
	}

	[TestMethod]
	public void IndexerGet_KeyDoesNotExistWithGetDefaultSet_Default()
	{
		var key = "15";
		Func<string, int> getDefault = _ => 10;
		GetTestDictionaries(out _, out var lazyDictionary, getDefault);

		Assert.AreEqual(getDefault(key), lazyDictionary[key]);
		Assert.IsFalse(lazyDictionary.ContainsKey(key));
	}

	[TestMethod]
	public void IndexerGet_KeyDoesNotExistWithGetDefaultSetAndAddMissingKeys_Default()
	{
		var key = "15";
		Func<string, int> getDefault = int.Parse;
		GetTestDictionaries(out _, out var lazyDictionary, getDefault, true);

		Assert.AreEqual(getDefault(key), lazyDictionary[key]);
		Assert.IsTrue(lazyDictionary.ContainsKey(key));
		Assert.AreEqual(getDefault(key), lazyDictionary[key]); // check again to make sure it was set correctly
	}

	[TestMethod]
	public void IndexerGet_KeyExists_Value()
	{
		var key = "5";
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		Assert.AreEqual(dictionary[key], lazyDictionary[key]);
	}

	[TestMethod]
	public void IndexerSet_KeyExists_ResultMatchesRegularDictionary()
	{
		var key = "5";
		var value = 10;
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		dictionary[key] = value;
		lazyDictionary[key] = value;

		Assert.AreEqual(dictionary[key], lazyDictionary[key]);
	}

	[TestMethod]
	public void IndexerSet_KeyDoesNotExist_NoException()
	{
		var key = "5";
		var value = 10;
		GetTestDictionaries(out _, out var lazyDictionary);

		lazyDictionary[key] = value;

		Assert.AreEqual(value, lazyDictionary[key]);
	}

	[TestMethod]
	public void IndexerSet_WithGetDefaultAndAddMissingKeys_CorrectResult()
	{
		var lazyDictionary = new LazyDictionary<int, List<int>>()
		{
			GetDefault = _ => new(),
			AddMissingKeys = true,
		};

		lazyDictionary[0].Add(1);
		lazyDictionary[0].Add(2);
		lazyDictionary[0].Add(3);

		Assert.AreEqual(1, lazyDictionary.Count);
		Assert.AreEqual(3, lazyDictionary[0].Count);
		Assert.AreEqual(1, lazyDictionary[0][0]);
		Assert.AreEqual(2, lazyDictionary[0][1]);
		Assert.AreEqual(3, lazyDictionary[0][2]);
	}
	#endregion

	#region LazyDictionaryEqualsDictionary
	[TestMethod]
	public void Add_RandomKeyAndValue_SameAsDictionary()
	{
		var key = Random.Shared.Next(-1000, 1000).ToString();
		var value = Random.Shared.Next(-1000, 1000);

		var dictionaryErrored = false;
		var lazyDictionaryErrored = false;

		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		try
		{
			dictionary.Add(key, value);
		}
		catch
		{
			dictionaryErrored = true;
		}

		try
		{
			lazyDictionary.Add(key, value);
		}
		catch
		{
			lazyDictionaryErrored = true;
		}

		Assert.AreEqual(dictionaryErrored, lazyDictionaryErrored);
		if (!dictionaryErrored && !lazyDictionaryErrored)
		{
			Assert.AreEqual(dictionary[key], lazyDictionary[key]);
		}
	}

	[TestMethod]
	public void Clear_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		dictionary.Clear();
		lazyDictionary.Clear();

		Assert.AreEqual(dictionary.Count, lazyDictionary.Count);
	}

	[TestMethod]
	public void ContainsKey_RandomKeyAndValue_SameAsDictionary()
	{
		var key = Random.Shared.Next(-1000, 1000).ToString();

		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		Assert.AreEqual(dictionary.ContainsKey(key), lazyDictionary.ContainsKey(key));
	}

	[TestMethod]
	public void Enumerate_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void Remove_RandomKeyAndValue_SameAsDictionary()
	{
		var key = Random.Shared.Next(-1000, 1000).ToString();

		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		Assert.AreEqual(dictionary.Remove(key), lazyDictionary.Remove(key));

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void RemoveOut_RandomKeyAndValue_SameAsDictionary()
	{
		var key = Random.Shared.Next(-1000, 1000).ToString();

		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		Assert.AreEqual(dictionary.Remove(key, out var dictionaryValue), lazyDictionary.Remove(key, out var lazyDictionaryValue));
		Assert.AreEqual(dictionaryValue, lazyDictionaryValue);

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void TryGetValue_RandomKeyAndValue_SameAsDictionary()
	{
		var key = Random.Shared.Next(-1000, 1000).ToString();

		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		Assert.AreEqual(dictionary.TryGetValue(key, out var dictionaryValue), lazyDictionary.TryGetValue(key, out var lazyDictionaryValue));
		Assert.AreEqual(dictionaryValue, lazyDictionaryValue);
	}

	[TestMethod]
	public void Keys_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		var unmatchedKeys = dictionary.Keys.ToHashSet();
		unmatchedKeys.SymmetricExceptWith(lazyDictionary.Keys.ToHashSet());

		Assert.AreEqual(0, unmatchedKeys.Count);
	}

	[TestMethod]
	public void Values_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		var unmatchedValues = dictionary.Values.ToHashSet();
		unmatchedValues.SymmetricExceptWith(lazyDictionary.Values.ToHashSet());

		Assert.AreEqual(0, unmatchedValues.Count);
	}
	#endregion

	#region ExtensionMethods
	[TestMethod]
	public void ToLazyDictionary_CorrectResult()
	{
		GetTestDictionaries(out var dictionary, out _);
		var lazyDictionary = dictionary.ToLazyDictionary();

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void ToLazyDictionary_WithKeySelectorAndComparer_CorrectResult()
	{
		Func<int, string> keySelector = x => x.ToString();
		var comparer = EqualityComparer<string>.Default;

		var enumerable = Enumerable.Range(0, 10);

		var dictionary = enumerable.ToDictionary(keySelector, comparer);
		var lazyDictionary = enumerable.ToLazyDictionary(keySelector, comparer);

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void ToLazyDictionary_WithKeySelector_CorrectResult()
	{
		Func<int, string> keySelector = x => x.ToString();

		var enumerable = Enumerable.Range(0, 10);

		var dictionary = enumerable.ToDictionary(keySelector);
		var lazyDictionary = enumerable.ToLazyDictionary(keySelector);

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void ToLazyDictionary_WithKeyAndElementSelectors_CorrectResult()
	{
		Func<int, string> keySelector = x => x.ToString();
		Func<int, int> elementSelector = x => x * 2;

		var enumerable = Enumerable.Range(0, 10);

		var dictionary = enumerable.ToDictionary(keySelector, elementSelector);
		var lazyDictionary = enumerable.ToLazyDictionary(keySelector, elementSelector);

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}

	[TestMethod]
	public void ToLazyDictionary_WithKeyAndElementSelectorsAndComparer_CorrectResult()
	{
		Func<int, string> keySelector = x => x.ToString();
		Func<int, int> elementSelector = x => x * 2;
		var comparer = EqualityComparer<string>.Default;

		var enumerable = Enumerable.Range(0, 10);

		var dictionary = enumerable.ToDictionary(keySelector, elementSelector, comparer);
		var lazyDictionary = enumerable.ToLazyDictionary(keySelector, elementSelector, comparer);

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}
	#endregion

	#region InterfaceOnly
	[TestMethod]
	public void ICollectionContains_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);
		var exampleKey = dictionary.Keys.First();

		var trueKvp = KeyValuePair.Create(exampleKey, dictionary[exampleKey]);
		var falseKvp = KeyValuePair.Create("this is def not in the dictionary", -1);

		Assert.IsFalse(dictionary.ContainsKey(falseKvp.Key));

		Assert.AreEqual(((ICollection<KeyValuePair<string, int>>)dictionary).Contains(trueKvp),
			((ICollection<KeyValuePair<string, int>>)lazyDictionary).Contains(trueKvp));

		Assert.AreEqual(((ICollection<KeyValuePair<string, int>>)dictionary).Contains(falseKvp),
			((ICollection<KeyValuePair<string, int>>)lazyDictionary).Contains(falseKvp));
	}

	[TestMethod]
	public void ICollectionCopyTo_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);
		var dictionaryArray = new KeyValuePair<string, int>[dictionary.Count];
		var lazyDictionaryArray = new KeyValuePair<string, int>[lazyDictionary.Count];

		((ICollection<KeyValuePair<string, int>>)dictionary).CopyTo(dictionaryArray, 0);
		((ICollection<KeyValuePair<string, int>>)lazyDictionary).CopyTo(lazyDictionaryArray, 0);

		Assert.AreEqual(dictionaryArray.Length, lazyDictionaryArray.Length);
		foreach (var (dictionaryValue, lazyDictionaryValue) in dictionary.Zip(lazyDictionaryArray))
		{
			Assert.AreEqual(dictionaryValue, lazyDictionaryValue);
		}
	}

	[TestMethod]
	public void ICollectionRemove_SameAsDictionary()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);
		var exampleKey = dictionary.Keys.First();

		var trueKvp = KeyValuePair.Create(exampleKey, dictionary[exampleKey]);
		var falseKvp = KeyValuePair.Create("this is def not in the dictionary", -1);

		Assert.IsFalse(dictionary.ContainsKey(falseKvp.Key));

		Assert.AreEqual(((ICollection<KeyValuePair<string, int>>)dictionary).Remove(trueKvp),
			((ICollection<KeyValuePair<string, int>>)lazyDictionary).Remove(trueKvp));

		Assert.AreEqual(((ICollection<KeyValuePair<string, int>>)dictionary).Remove(falseKvp),
			((ICollection<KeyValuePair<string, int>>)lazyDictionary).Remove(falseKvp));

		AssertDictionariesAreEqual(dictionary, lazyDictionary);
	}
	#endregion
}