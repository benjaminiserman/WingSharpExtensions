namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class ArrayDestructuringTests
{
	private List<int> GetTestList() => new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

	[TestMethod]
	public void Destructure_One_CorrectResult()
	{
		var list = GetTestList();

		var (a, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.IsTrue(rest is [2, 3, 4, 5, 6, 7, 8, 9]);
	}

	[TestMethod]
	public void Destructure_Two_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.IsTrue(rest is [3, 4, 5, 6, 7, 8, 9]);
	}

	[TestMethod]
	public void Destructure_Three_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, c, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.AreEqual(c, list[2]);
		Assert.IsTrue(rest is [4, 5, 6, 7, 8, 9]);
	}

	[TestMethod]
	public void Destructure_Four_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, c, d, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.AreEqual(c, list[2]);
		Assert.AreEqual(d, list[3]);
		Assert.IsTrue(rest is [5, 6, 7, 8, 9]);
	}

	[TestMethod]
	public void Destructure_Five_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, c, d, e, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.AreEqual(c, list[2]);
		Assert.AreEqual(d, list[3]);
		Assert.AreEqual(e, list[4]);
		Assert.IsTrue(rest is [6, 7, 8, 9]);
	}

	[TestMethod]
	public void Destructure_Six_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, c, d, e, f, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.AreEqual(c, list[2]);
		Assert.AreEqual(d, list[3]);
		Assert.AreEqual(e, list[4]);
		Assert.AreEqual(f, list[5]);
		Assert.IsTrue(rest is [7, 8, 9]);
	}

	[TestMethod]
	public void Destructure_Seven_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, c, d, e, f, g, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.AreEqual(c, list[2]);
		Assert.AreEqual(d, list[3]);
		Assert.AreEqual(e, list[4]);
		Assert.AreEqual(f, list[5]);
		Assert.AreEqual(g, list[6]);
		Assert.IsTrue(rest is [8, 9]);
	}

	[TestMethod]
	public void Destructure_Eight_CorrectResult()
	{
		var list = GetTestList();

		var (a, b, c, d, e, f, g, h, rest) = list;

		Assert.AreEqual(a, list[0]);
		Assert.AreEqual(b, list[1]);
		Assert.AreEqual(c, list[2]);
		Assert.AreEqual(d, list[3]);
		Assert.AreEqual(e, list[4]);
		Assert.AreEqual(f, list[5]);
		Assert.AreEqual(g, list[6]);
		Assert.AreEqual(h, list[7]);
		Assert.IsTrue(rest is [9]);
	}

	[TestMethod]
	public void Destructure_EmptySequence_Throws()
	{
		var list = new List<int>();

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
		{
			var (a, b, c, _) = list;
			Console.WriteLine(a + b + c);
		});
	}
}