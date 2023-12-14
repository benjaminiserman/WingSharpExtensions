namespace WingSharpExtensionsTest;

using System;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WingSharpExtensions.ExtraMath;

[TestClass]
public class ExtraMathTests
{
	[TestMethod]
	public void GCD_CorrectResults()
	{
		Assert.AreEqual(-2, GCD(-2, -6));
		Assert.AreEqual(0, GCD(0, 0));
		Assert.AreEqual(1, GCD(1, 0));
		Assert.AreEqual(1, GCD(0, 1));
		Assert.AreEqual(7, GCD(7, 0));
		Assert.AreEqual(7, GCD(0, 7));
		Assert.AreEqual(5, GCD(-5, 5));
		Assert.AreEqual(4, GCD(32, 4));
		Assert.AreEqual(1, GCD(53, 27));
		Assert.AreEqual(2, GCD(6, 8));
		Assert.AreEqual(10, GCD(10, 100));
		Assert.AreEqual(2, GCD(48, 74, 28, 54, 32));
	}

	[TestMethod]
	public void GCD_ParamsSameAsEnumerable()
	{
		var random = new Random();
		foreach (var trial in Enumerable.Range(1, 10))
		{
			var randomList = Enumerable.Repeat(1, trial).Select(_ => random.Next()).ToList();
			Assert.AreEqual(randomList.GCD(), GCD(randomList.ToArray()));
		}
	}

	[TestMethod]
	public void LCM_CorrectResults()
	{
		Assert.AreEqual(6, LCM(-2, -6));
		Assert.AreEqual(0, LCM(0, 0));
		Assert.AreEqual(0, LCM(1, 0));
		Assert.AreEqual(0, LCM(0, 1));
		Assert.AreEqual(0, LCM(7, 0));
		Assert.AreEqual(0, LCM(0, 7));
		Assert.AreEqual(5, LCM(-5, 5));
		Assert.AreEqual(32, LCM(32, 4));
		Assert.AreEqual(1431, LCM(53, 27));
		Assert.AreEqual(24, LCM(6, 8));
		Assert.AreEqual(100, LCM(10, 100));
		Assert.AreEqual(223776, LCM(48, 74, 28, 54, 32));
	}

	[TestMethod]
	public void LCM_ParamsSameAsEnumerable()
	{
		var random = new Random();
		foreach (var trial in Enumerable.Range(1, 10))
		{
			var randomList = Enumerable.Repeat(1, trial).Select(_ => random.Next()).ToList();
			Assert.AreEqual(randomList.LCM(), LCM(randomList.ToArray()));
		}
	}

	[TestMethod]
	public void Factorial_CorrectResult()
	{
		var correctAnswers = new int[] { 1, 1, 2, 6, 24, 120, 720 };
		foreach (var trial in Enumerable.Range(0, correctAnswers.Length))
		{
			Assert.AreEqual(correctAnswers[trial], Factorial(trial));
		}
	}

	[TestMethod]
	public void Perm_CorrectResults()
	{
		Assert.ThrowsException<ArgumentException>(() => Perm(-2, -6));
		Assert.AreEqual(863040, Perm(32, 4));
		Assert.AreEqual(1, Perm(1, 0));
		Assert.AreEqual(0, Perm(0, 1));
		Assert.AreEqual(1, Perm(7, 0));
		Assert.AreEqual(0, Perm(0, 7));
		Assert.AreEqual(120, Perm(5, 5));
		Assert.AreEqual(120, Perm(5));
		Assert.ThrowsException<ArgumentException>(() => Perm(-5, 5));
		Assert.AreEqual(BigInteger.Parse("10599984616877389761291348267009835008000000"), Perm((BigInteger)53, 27));
		Assert.AreEqual(0, Perm(6, 8));
		Assert.AreEqual(0, Perm(10, 100));
		Assert.AreEqual(0, Perm(48, 74));
	}

	[TestMethod]
	public void Comb_CorrectResults()
	{
		Assert.ThrowsException<ArgumentException>(() => Comb(-2, -6));
		Assert.AreEqual(35960, Comb(32, 4));
		Assert.AreEqual(1, Comb(1, 0));
		Assert.AreEqual(0, Comb(0, 1));
		Assert.AreEqual(1, Comb(7, 0));
		Assert.AreEqual(0, Comb(0, 7));
		Assert.AreEqual(1, Comb(5, 5));
		Assert.ThrowsException<ArgumentException>(() => Comb(-5, 5));
		Assert.AreEqual(BigInteger.Parse("973469712824056"), Comb((BigInteger)53, 27));
		Assert.AreEqual(0, Comb(6, 8));
		Assert.AreEqual(0, Comb(10, 100));
		Assert.AreEqual(0, Comb(48, 74));
	}

	[TestMethod]
	public void Product_CorrectResults()
	{
		Assert.AreEqual(720, new int[] { 1, 2, 3, 4, 5, 6 }.Product());
		var random = new Random();
		var testList = Enumerable
			.Repeat(0, 10)
			.Select(_ => (BigInteger)random.Next())
			.ToList();

		Assert.AreEqual(testList.Aggregate((a, b) => a * b), testList.Product());
	}
}