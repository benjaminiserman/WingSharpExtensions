namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class DictUnionTests
{
	[TestMethod]
	public void DictUnion_Binary_CorrectResult()
	{
		var dictionaryA = new Dictionary<string, int>()
		{
			{ "blue", 3 },
			{ "red", 4 },
		};

		var dictionaryB = new Dictionary<string, int>()
		{
			{ "red", 1 },
			{ "green", 2 },
			{ "blue", 6 },
		};

		var resultDictionary = dictionaryA.DictUnion((a, b) => int.Max(a, b), dictionaryB);

		Assert.AreEqual(resultDictionary["red"], 4);
		Assert.AreEqual(resultDictionary["green"], 2);
		Assert.AreEqual(resultDictionary["blue"], 6);
	}

	[TestMethod]
	public void DictUnion_N_Ary_CorrectResult()
	{
		var dicts = new List<Dictionary<string, int>>()
		{
			new()
			{
				{ "red", 8 },
				{ "green", 3 },
				{ "blue", 1 },
			},
			new()
			{
				{ "green", 5 },
				{ "blue", 6 },
			},
			new()
			{
				{ "red", 10 },
				{ "green", 4 },
				{ "blue", 4 },
			},
			new()
			{
				{ "red", 6 },
				{ "green", 2 },
				{ "blue", 4 },
			},
			new()
			{
				{ "red", 8 },
				{ "green", 4 },
				{ "blue", 11 },
			},
			new()
			{
				{ "red", 10 },
				{ "blue", 10 },
			},
		};

		var resultDictionary = dicts.DictUnion((a, b) => int.Max(a, b));

		Assert.AreEqual(resultDictionary["red"], 10);
		Assert.AreEqual(resultDictionary["green"], 5);
		Assert.AreEqual(resultDictionary["blue"], 11);
	}

	[TestMethod]
	public void DictUnion_N_Ary_Random_CorrectResult()
	{
		var dicts = new List<Dictionary<string, int>>();
		var random = new Random();

		foreach (var _ in Enumerable.Range(0, 10))
		{
			dicts.Add(new Dictionary<string, int>()
			{
				{ "red", random.Next(0, 11) },
				{ "green", random.Next(0, 11) },
				{ "blue", random.Next(0, 11) },
			});
		}

		var resultDictionary = dicts.DictUnion((a, b) => int.Max(a, b));

		Assert.AreEqual(resultDictionary["red"], dicts.Aggregate(0, (a, b) => int.Max(a, b.ToLazyDictionary()["red"])));
		Assert.AreEqual(resultDictionary["green"], dicts.Aggregate(0, (a, b) => int.Max(a, b.ToLazyDictionary()["green"])));
		Assert.AreEqual(resultDictionary["blue"], dicts.Aggregate(0, (a, b) => int.Max(a, b.ToLazyDictionary()["blue"])));
	}

	[TestMethod]
	public void DictUnion_EmptySequence_EmptySequence()
	{
		var dictionaryA = new Dictionary<string, int>()
		{
			{ "red", 1 },
			{ "green", 2 },
			{ "blue", 6 },
		};

		var resultDictionary = dictionaryA
			.DictUnion((a, b) => a + b, Array.Empty<IDictionary<string, int>>());

		Assert.AreEqual(resultDictionary["red"], 1);
		Assert.AreEqual(resultDictionary["green"], 2);
		Assert.AreEqual(resultDictionary["blue"], 6);
	}
}