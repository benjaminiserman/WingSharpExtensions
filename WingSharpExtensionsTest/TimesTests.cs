namespace WingSharpExtensionsTest;

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class TimesTests
{
	[DataTestMethod]
	[DataRow(-1)]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(5)]
	public void TimesIndex_Identity_CorrectResult(int n)
	{
		Func<int, int> function = x => x;
		var sequence = function.Times(n).ToList();

		Assert.AreEqual(Math.Max(0, n), sequence.Count);

		for (var i = 0; i < sequence.Count; i++)
		{
			Assert.AreEqual(i, sequence[i]);
		}
	}

	[DataTestMethod]
	[DataRow(-1)]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(5)]
	public void Times_Constant_CorrectResult(int n)
	{
		Func<int> function = () => 1;
		var sequence = function.Times(n).ToList();

		Assert.AreEqual(Math.Max(0, n), sequence.Count);

		for (var i = 0; i < sequence.Count; i++)
		{
			Assert.AreEqual(1, sequence[i]);
		}
	}

	[DataTestMethod]
	[DataRow(-1)]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(5)]
	public void TimesIndex_Action_CorrectResult(int n)
	{
		var count = 0;
		Action function = () => count++;
		function.Times(n);

		Assert.AreEqual(Math.Max(0, n), count);
	}

	[DataTestMethod]
	[DataRow(-1)]
	[DataRow(0)]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(5)]
	public void TimesIndex_Factorial_CorrectResult(int n)
	{
		var count = 1;
		Action<int> function = x => count *= x + 1;
		function.Times(n);

		var factorial = 1;
		for (var i = 2; i <= n; i++)
		{
			factorial *= i;
		}

		Assert.AreEqual(factorial, count);
	}
}