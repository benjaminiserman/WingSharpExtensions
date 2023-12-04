namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class MemoizeTests
{
	[TestMethod]
	public void Memoize_NoArgs_CorrectResult()
	{
		var memoizedFunction = MemoizeFactory.Memoize(() => new Random().NextDouble());

		var firstResult = memoizedFunction();
		var secondResult = memoizedFunction();

		Assert.AreEqual(firstResult, secondResult);
	}

	[TestMethod]
	public void Memoize_Unary_CorrectResult()
	{
		var i = 1;
		int BadFunction(int x)
		{
			i++;
			return i * x;
		}

		var memoizedBadFunction = MemoizeFactory.Memoize<int, int>(BadFunction);
		var firstResult = memoizedBadFunction(5);
		var secondResult = memoizedBadFunction(5);

		Assert.AreEqual(firstResult, secondResult);
	}

	[TestMethod]
	public void Memoize_Binary_CorrectResult()
	{
		Random random = new();
		int BadFunction(int a, int b)
		{
			return new List<int>()
			{
				a * random.Next(),
				b * random.Next()
			}.Aggregate((a, b) => a * b);
		}

		var memoizedBadFunction = MemoizeFactory.Memoize<int, int, int>(BadFunction);
		var firstResult = memoizedBadFunction(1, 2);
		var secondResult = memoizedBadFunction(1, 2);

		Assert.AreEqual(firstResult, secondResult);
	}

	[TestMethod]
	public void Memoize_Ternary_CorrectResult()
	{
		Random random = new();
		int BadFunction(int a, int b, int c)
		{
			return new List<int>()
			{
				a * random.Next(),
				b * random.Next(),
				c * random.Next()
			}.Aggregate((a, b) => a * b);
		}

		var memoizedBadFunction = MemoizeFactory.Memoize<int, int, int, int>(BadFunction);
		var firstResult = memoizedBadFunction(1, 2, 3);
		var secondResult = memoizedBadFunction(1, 2, 3);

		Assert.AreEqual(firstResult, secondResult);
	}

	[TestMethod]
	public void Memoize_Quaternary_CorrectResult()
	{
		Random random = new();
		int BadFunction(int a, int b, int c, int d)
		{
			return new List<int>()
			{
				a * random.Next(),
				b * random.Next(),
				c * random.Next(),
				d * random.Next()
			}.Aggregate((a, b) => a * b);
		}

		var myDictionary = new Dictionary<List<int>, int>();
		var test = MemoizeFactory.Memoize<List<int>, int>(a => 1);

		var memoizedBadFunction = MemoizeFactory.Memoize<int, int, int, int, int>(BadFunction);
		var firstResult = memoizedBadFunction(1, 2, 3, 4);
		var secondResult = memoizedBadFunction(1, 2, 3, 4);

		Assert.AreEqual(firstResult, secondResult);
	}

	[TestMethod, Timeout(1000)]
	public void Memoize_Fibonacci()
	{
		Func<BigInteger, BigInteger> fibonacci = x => x;
		fibonacci = (BigInteger x) =>
		{
			if (x == 0)
			{
				return 0;
			} 
			else if (x == 1)
			{
				return 1;
			}
			else
			{
				return fibonacci(x - 2) + fibonacci(x - 1);
			}
		};
		fibonacci = fibonacci.Memoize();

		Assert.AreEqual(fibonacci(50), new BigInteger(12586269025));
	}
}