namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class AnyOutTests
{
	[TestMethod]
	public void AnyOut_EmptySequence_FalseDefault()
	{
		var sequence = new List<int>();

		var result = sequence.AnyOut(out var found);

		Assert.IsFalse(result);
		Assert.AreEqual(default, found);
	}

	[TestMethod]
	public void AnyOutWithPredicate_EmptySequence_FalseDefault()
	{
		var sequence = new List<bool>();
		Func<bool, bool> predicate = x => x == default;

		var result = sequence.AnyOut(predicate, out var found);

		Assert.IsFalse(result);
		Assert.AreEqual(default, found);
	}

	[TestMethod]
	public void AnyOut_SingletonDoesNotMatch_FalseDefault()
	{
		var sequence = new List<double>() { 5.0 };
		Func<double, bool> predicate = x => x == default;

		var result = sequence.AnyOut(predicate, out var found);

		Assert.IsFalse(result);
		Assert.AreEqual(default, found);
	}

	[TestMethod]
	public void AnyOut_SingletonDoesMatch_TrueFirst()
	{
		var sequence = new List<int>() { 5 };
		Func<int, bool> predicate = x => x == 5;

		var result = sequence.AnyOut(predicate, out var found);

		Assert.IsTrue(result);
		Assert.AreEqual(sequence.First(predicate), found);
	}

	[TestMethod]
	public void AnyOut_Singleton_TrueFirst()
	{
		var sequence = new List<string>() { "5" };

		var result = sequence.AnyOut(out var found);

		Assert.IsTrue(result);
		Assert.AreEqual(sequence.First(), found);
	}

	[TestMethod]
	public void AnyOut_BigList_TrueFirst()
	{
		var sequence = Enumerable.Range(0, 100);

		var result = sequence.AnyOut(out var found);

		Assert.IsTrue(result);
		Assert.AreEqual(sequence.First(), found);
	}

	[TestMethod]
	public void AnyOutWithPredicate_BigList_TrueFirst()
	{
		var sequence = Enumerable.Range(0, 100);
		Func<int, bool> predicate = x => x == 5;

		var result = sequence.AnyOut(predicate, out var found);

		Assert.IsTrue(result);
		Assert.AreEqual(sequence.First(predicate), found);
	}

	[TestMethod]
	public void AnyOutWithPredicate_BigList_FalseDefault()
	{
		var sequence = Enumerable.Range(0, 100);
		Func<int, bool> predicate = x => x == -1;

		var result = sequence.AnyOut(predicate, out var found);

		Assert.IsFalse(result);
		Assert.AreEqual(default, found);
	}
}