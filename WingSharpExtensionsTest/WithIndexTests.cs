namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class WithIndexTests
{
	private List<int> GetTestList() => Enumerable.Range(0, 10).ToList();

	[TestMethod]
	public void WithIndex_CorrectResult()
	{
		var list = GetTestList();

		var withIndexList = list.WithIndex().ToList();

		Assert.AreEqual(list.Count, withIndexList.Count);

		for (var i = 0; i < list.Count; i++)
		{
			Assert.AreEqual(list[i], withIndexList[i].Value);
			Assert.AreEqual(i, withIndexList[i].Index);
		}
	}

	[TestMethod]
	public void WithIndex_EmptySequence_EmptySequence()
	{
		var list = new List<int>();
		var withIndex = list.WithIndex();

		Assert.AreEqual(0, withIndex.Count());
	}
}