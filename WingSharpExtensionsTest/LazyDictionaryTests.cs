namespace WingSharpExtensionsTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using WingSharpExtensions;

[TestClass]
public class LazyDictionaryTests
{
	void GetTestDictionaries(out Dictionary<string, int> dictionary, out LazyDictionary<string, int> lazyDictionary)
	{
		dictionary = Enumerable.Range(0, 10).ToDictionary(x => x.ToString(), x => x);
		lazyDictionary = new(dictionary);
	}

	#region LazyDictionaryDiffersFromDictionary
	[TestMethod]
	public void IndexerGet_KeyDoesNotExist_Default()
	{
		GetTestDictionaries(out var dictionary, out var lazyDictionary);

		Assert.AreEqual(default, lazyDictionary["15"]);
	}
	#endregion

	#region LazyDictionaryEqualsDictionary

	#endregion
}