namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class RangeTests
{
	[TestMethod]
	public void Enumerate_EmptyRange_EmptySequence()
	{
		var range = 10..10;

		Assert.AreEqual(0, range.Enumerate().Count());
	}

	[TestMethod]
	public void Enumerate_EquivalentToForeach()
	{
		var range = 0..10;

		var enumerable = range.Enumerate();

		var list = new List<int>();
		foreach (var x in range)
		{
			list.Add(x);
		}

		Assert.AreEqual(list.Count, enumerable.Count());
		foreach (var (listValue, enumerableValue) in list.Zip(enumerable))
		{
			Assert.AreEqual(listValue, enumerableValue);
		}
	}

	[TestMethod]
	public void Enumerate_MatchesEquivalentEnumerableRange()
	{
		var start = Random.Shared.Next(0, 100);
		var end = start + 10;

		var correctSequence = Enumerable.Range(start, end - start);
		var testSequence = (start..end).Enumerate();

		Assert.AreEqual(correctSequence.Count(), testSequence.Count());
		foreach (var (correctValue, testValue) in correctSequence.Zip(testSequence))
		{
			Assert.AreEqual(correctValue, testValue);
		}
	}

	[TestMethod]
	public void Enumerate_Downwards_CorrectResult()
	{
		var start = Random.Shared.Next(10, 100);
		var end = start - 10;

		Assert.AreEqual(start - end, (start..end).Enumerate().Count());

		var i = start;
		foreach (var value in start..end)
		{
			Assert.AreEqual(i, value);
			i--;
		}
	}

	[TestMethod]
	public void Range_EmptyRange_EmptySequence()
	{
		var start = 10;
		var end = start;

		Assert.AreEqual(0, RangeExtension.Range(start, end).Count());
	}

	[TestMethod]
	public void Range_MatchesEquivalentEnumerableRange()
	{
		var start = Random.Shared.Next(-100, 100);
		var end = start + 10;

		var correctSequence = Enumerable.Range(start, end - start);
		var testSequence = RangeExtension.Range(start, end);

		Assert.AreEqual(correctSequence.Count(), testSequence.Count());
		foreach (var (correctValue, testValue) in correctSequence.Zip(testSequence))
		{
			Assert.AreEqual(correctValue, testValue);
		}
	}

	[TestMethod]
	public void Range_Downwards_CorrectResult()
	{
		var start = Random.Shared.Next(-100, 100);
		var end = start - 10;

		var range = RangeExtension.Range(start, end);

		Assert.AreEqual(start - end, range.Count());

		var i = start;
		foreach (var value in range)
		{
			Assert.AreEqual(i, value);
			i--;
		}
	}
}