namespace WingSharpExtensionsTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using WingSharpExtensions;

[TestClass]
public class ChunkByTests
{
	[DataTestMethod]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(5)]
	public void ChunkBy_EmptySequence_EmptySequence(int n)
	{
		var sequence = new List<int>();

		var result = sequence.ChunkBy(n);

		Assert.AreEqual(result.Count(), 0);
	}

	[TestMethod]
	public void ChunkBy_ChunkByZero_ThrowsArgumentOutOfRangeException()
	{
		var sequence = Enumerable.Range(0, 10);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => sequence.ChunkBy(0).ToList());
	}

	[TestMethod]
	public void ChunkBy_ChunkByNegative_ThrowsArgumentOutOfRangeException()
	{
		var sequence = Enumerable.Range(0, 10);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => sequence.ChunkBy(-1).ToList());
	}

	[TestMethod]
	public void ChunkBy_ChunkByOne_SameCountEnumerableOfSingletons()
	{
		var sequence = Enumerable.Range(0, 10);

		var result = sequence.ChunkBy(1);

		Assert.AreEqual(result.Count(), sequence.Count());

		foreach (var (sequenceValue, resultValue) in sequence.Zip(result))
		{
			Assert.AreEqual(1, resultValue.Count());
			Assert.AreEqual(sequenceValue, resultValue.First());
		}
	}

	[TestMethod]
	public void ChunkBy_ChunkByValueOverSequenceCount_SequenceWrappedInSingleton()
	{
		var sequence = Enumerable.Range(0, 10);

		var result = sequence.ChunkBy(1000);

		foreach (var x in result)
		{
			Console.WriteLine($"a: {x}");
			foreach (var y in x)
			{
				Console.WriteLine(y);
			}
		}

		Assert.AreEqual(1, result.Count());
		Assert.AreEqual(result.First().Count(), sequence.Count());

		foreach (var (sequenceValue, resultValue) in sequence.Zip(result.First()))
		{
			Assert.AreEqual(sequenceValue, resultValue);
		}
	}

	[DataTestMethod]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(5)]
	public void ChunkBy_RegularUse_CorrectResult(int n)
	{
		var sequence = Enumerable.Range(0, 10).ToList();

		var result = sequence.ChunkBy(n);

		Assert.AreEqual(sequence.Count, result.Sum(x => x.Count()));

		foreach (var chunk in result.Select((x, i) => (x, i)))
		{
			foreach (var value in chunk.x.Select((x, i) => (x, i)))
			{
				Assert.AreEqual(sequence[chunk.i * n + value.i], value.x);
			}
		}
	}
}