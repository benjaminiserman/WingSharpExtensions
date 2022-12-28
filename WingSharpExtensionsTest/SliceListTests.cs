namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class SliceListTests
{
	private SliceList<int> GetTestList() => new(Enumerable.Range(0, 10));

	#region IndexerGet
	[TestMethod]
	public void IndexerGet_CorrectResult()
	{
		var list = GetTestList();

		var start = Random.Shared.Next(0, list.Count);
		var end = start;

		while (end == start)
		{
			end = Random.Shared.Next(0, list.Count);
		}

		var slice = list[start..end];

		Console.WriteLine($"list: {{ {string.Join(", ", list)} }}");
		Console.WriteLine($"slice: [{start}..{end}] {{ {string.Join(", ", slice)} }}");

		Assert.AreEqual(Math.Abs(start - end), slice.Count);

		var j = 0;
		foreach (var i in start..end)
		{
			Assert.AreEqual(list[i], slice[j]);
			j++;
		}
	}

	[TestMethod]
	public void IndexerGet_EmptyRange_EmptySlice()
	{
		var list = GetTestList();

		var start = Random.Shared.Next(0, list.Count);
		var end = start;

		var slice = list[start..end];

		Assert.AreEqual(0, slice.Count);
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => slice[0]);
	}

	[TestMethod]
	public void IndexerGet_SliceEntireList_CorrectResult()
	{
		var list = GetTestList();

		var start = 0;
		var end = list.Count;

		var slice = list[start..end];

		Assert.AreEqual(list.Count, slice.Count);
		foreach (var (listValue, sliceValue) in list.Zip(slice))
		{
			Assert.AreEqual(listValue, sliceValue);
		}
	}

	[TestMethod]
	public void IndexerGet_SliceBeforeStart_ThrowsArgumentOutOfRangeException()
	{
		var list = GetTestList();

		var start = -1;
		var end = list.Count;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[start..end]);
	}

	[TestMethod]
	public void IndexerGet_SliceAfterEnd_ThrowsArgumentOutOfRangeException()
	{
		var list = GetTestList();

		var start = 0;
		var end = list.Count + 1;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[start..end]);
	}
	#endregion


	#region IndexerSet
	[TestMethod]
	public void IndexerSet_CorrectResult()
	{
		var list = GetTestList();

		var start = Random.Shared.Next(0, list.Count);
		var end = start;
		var replaceSlice = new SliceList<int>() { -1, -2, -3 };

		while (end == start)
		{
			end = Random.Shared.Next(0, list.Count);
		}

		var slice = new SliceList<int>(list);
		Console.WriteLine($"slice: [{start}..{end}]");
		Console.WriteLine($"before: {{ {string.Join(", ", slice)} }}");
		slice[start..end] = replaceSlice;
		Console.WriteLine($"after: {{ {string.Join(", ", slice)} }}");

		Assert.AreEqual(list.Count - (end - start) + replaceSlice.Count, slice.Count);

		foreach (var (listValue, sliceValue) in list
			.Take(start)
			.Zip(slice.Take(start)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}

		foreach (var (listValue, sliceValue) in slice
			.Skip(start)
			.Take(replaceSlice.Count)
			.Zip(replaceSlice))
		{
			Assert.AreEqual(listValue, sliceValue);
		}

		foreach (var (listValue, sliceValue) in list
			.TakeLast(list.Count - end)
			.Zip(slice.TakeLast(list.Count - end)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}
	}

	[TestMethod]
	public void IndexerSet_EmptySlice_Insert()
	{
		var list = GetTestList();

		var start = Random.Shared.Next(0, list.Count);
		var end = start;
		var replaceSlice = new SliceList<int>() { -1, -2, -3 };

		var slice = new SliceList<int>(list);
		Console.WriteLine($"slice: [{start}..{end}]");
		Console.WriteLine($"before: {{ {string.Join(", ", slice)} }}");
		slice[start..end] = replaceSlice;
		Console.WriteLine($"after: {{ {string.Join(", ", slice)} }}");

		Assert.AreEqual(list.Count - (end - start) + replaceSlice.Count, slice.Count);

		foreach (var (listValue, sliceValue) in list
			.Take(start)
			.Zip(slice.Take(start)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}

		foreach (var (listValue, sliceValue) in slice
			.Skip(start)
			.Take(replaceSlice.Count)
			.Zip(replaceSlice))
		{
			Assert.AreEqual(listValue, sliceValue);
		}

		foreach (var (listValue, sliceValue) in list
			.TakeLast(list.Count - end)
			.Zip(slice.TakeLast(list.Count - end)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}
	}

	[TestMethod]
	public void IndexerSet_Null_RemoveSlice()
	{
		var list = GetTestList();

		var start = Random.Shared.Next(0, list.Count);
		var end = start;
		SliceList<int> replaceSlice = null!;

		while (end == start)
		{
			end = Random.Shared.Next(0, list.Count);
		}

		var slice = new SliceList<int>(list);
		Console.WriteLine($"slice: [{start}..{end}]");
		Console.WriteLine($"before: {{ {string.Join(", ", slice)} }}");
		slice[start..end] = replaceSlice!;
		Console.WriteLine($"after: {{ {string.Join(", ", slice)} }}");

		Assert.AreEqual(list.Count - (end - start), slice.Count);

		foreach (var (listValue, sliceValue) in list
			.Take(start)
			.Zip(slice.Take(start)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}

		foreach (var (listValue, sliceValue) in list
			.TakeLast(list.Count - end)
			.Zip(slice.TakeLast(list.Count - end)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}
	}

	[TestMethod]
	public void IndexerSet_EmptyReplaceSlice_RemoveSlice()
	{
		var list = GetTestList();

		var start = Random.Shared.Next(0, list.Count);
		var end = start;
		SliceList<int> replaceSlice = new();

		while (end == start)
		{
			end = Random.Shared.Next(0, list.Count);
		}

		var slice = new SliceList<int>(list);
		Console.WriteLine($"slice: [{start}..{end}]");
		Console.WriteLine($"before: {{ {string.Join(", ", slice)} }}");
		slice[start..end] = replaceSlice;
		Console.WriteLine($"after: {{ {string.Join(", ", slice)} }}");

		Assert.AreEqual(list.Count - (end - start) + replaceSlice.Count, slice.Count);

		foreach (var (listValue, sliceValue) in list
			.Take(start)
			.Zip(slice.Take(start)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}

		foreach (var (listValue, sliceValue) in list
			.TakeLast(list.Count - end)
			.Zip(slice.TakeLast(list.Count - end)))
		{
			Assert.AreEqual(listValue, sliceValue);
		}
	}

	[TestMethod]
	public void IndexerSet_SliceBeforeStart_ThrowsArgumentOutOfRangeException()
	{
		var list = GetTestList();
		var start = -1;
		var end = list.Count + 1;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[start..end] = GetTestList());
	}

	[TestMethod]
	public void IndexerSet_SliceAfterStart_ThrowsArgumentOutOfRangeException()
	{
		var list = GetTestList();
		var start = -1;
		var end = list.Count + 1;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[start..end] = GetTestList());
	}
	#endregion

	[TestMethod]
	public void ToSliceList_CorrectResult()
	{
		var list = GetTestList().ToList();
		var sliceList = GetTestList().ToSliceList();

		Assert.AreEqual(list.Count, sliceList.Count);
		foreach (var (listValue, sliceValue) in list.Zip(sliceList))
		{
			Assert.AreEqual(listValue, sliceValue);
		}
	}
}