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

	private void GetTestDictionaries(out Dictionary<string, int> dictionary, out LazyDictionary<string, int> lazyDictionary, Func<int> getDefault = null!)
	{
		dictionary = Enumerable.Range(0, 10).ToDictionary(x => x.ToString(), x => x);
		lazyDictionary = new(dictionary)
		{
			GetDefault = getDefault
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
		var getDefault = () => 10;
		GetTestDictionaries(out _, out var lazyDictionary, getDefault);

		Assert.AreEqual(getDefault(), lazyDictionary[key]);
		Assert.IsTrue(lazyDictionary.ContainsKey(key));
		Assert.AreEqual(getDefault(), lazyDictionary[key]); // check again to make sure it was set correctly
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
	#endregion
}