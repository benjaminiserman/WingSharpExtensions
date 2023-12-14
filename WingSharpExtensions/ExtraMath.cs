namespace WingSharpExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class ExtraMath
{
	public static T GCD<T>(this IEnumerable<T> numbers) where T : IBinaryInteger<T>
	{
		return numbers.Aggregate(GCD);
	}

	public static T GCD<T>(params T[] numbers) where T : IBinaryInteger<T>
	{
		return GCD(numbers.ToList());
	}

	public static T GCD<T>(T a, T b) where T : IBinaryInteger<T>
	{
		return b.Equals(T.Zero) ? a : GCD(b, a % b);
	}

	public static T LCM<T>(this IEnumerable<T> numbers) where T : IBinaryInteger<T>
	{
		var lcm = T.MultiplicativeIdentity;

		foreach (var number in numbers)
		{
			if (number == T.Zero)
			{
				return T.Zero;
			}

			lcm *= number / GCD(lcm, number);
		}

		return T.Abs(lcm);
	}

	public static T LCM<T>(params T[] numbers) where T : IBinaryInteger<T>
	{
		return LCM(numbers.ToList());
	}

	public static T LCM<T>(T a, T b) where T : IBinaryInteger<T>
	{
		if (a == T.Zero && b == T.Zero)
		{
			return T.Zero;
		}

		return T.Abs(a * b / GCD(a, b));
	}

	public static T Product<T>(this IEnumerable<T> factors) where T : INumber<T>
	{
		return factors.Aggregate((a, b) => a * b);
	}

	public static T Factorial<T>(T n) where T : IBinaryInteger<T>
	{
		if (n < T.Zero)
		{
			throw new ArgumentException($"{nameof(n)} must be >= 0");
		}

		var product = T.MultiplicativeIdentity;
		for (var i = T.MultiplicativeIdentity; i <= n; i++)
		{
			product *= i;
		}

		return product;
	}

	public static T Comb<T>(T n, T k) where T : IBinaryInteger<T>
	{
		if (n < T.Zero)
		{
			throw new ArgumentException($"{nameof(n)} must be >= 0");
		}

		if (k < T.Zero)
		{
			throw new ArgumentException($"{nameof(k)} must be >= 0");
		}

		if (k > n)
		{
			return T.Zero;
		}

		var combinations = T.MultiplicativeIdentity;
		var a = T.Max(n - k, k);
		var b = T.Min(n - k, k);
		for (var i = a + T.One; i <= n; i++)
		{
			combinations *= i;
		}

		return combinations / Factorial(b);
	}

	public static T Perm<T>(T n, T k) where T : IBinaryInteger<T>
	{
		if (n < T.Zero)
		{
			throw new ArgumentException($"{nameof(n)} must be >= 0");
		}

		if (k < T.Zero)
		{
			throw new ArgumentException($"{nameof(k)} must be >= 0");
		}

		if (k > n)
		{
			return T.Zero;
		}

		var permutations = T.MultiplicativeIdentity;
		for (var i = n - k + T.One; i <= n; i++)
		{
			permutations *= i;
		}

		return permutations;
	}

	public static T Perm<T>(T n) where T : IBinaryInteger<T>
	{
		return Factorial(n);
	}
}