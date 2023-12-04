namespace WingSharpExtensions;

using System;
using System.Collections.Generic;

public static class MemoizeFactory
{
	public static Func<TResult> Memoize<TResult>(this Func<TResult> function)
	{
		TResult? result = default;
		var calculated = false;

		TResult WrappedFunction()
		{
			if (!calculated)
			{
				result = function();
				calculated = true;
			}

			return result!;
		}

		return WrappedFunction;
	}

	public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> function)
		where T1 : notnull
	{
		var memoizedResults = new Dictionary<T1, TResult>();
		
		TResult WrappedFunction(T1 arg1)
		{
			if (memoizedResults.TryGetValue(arg1, out var value))
			{
				return value;
			}
			else
			{
				var result = function(arg1);
				memoizedResults.Add(arg1, result);
				return result;
			}
		}

		return WrappedFunction;
	}

	public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> function)
	{
		var memoizedResults = new Dictionary<(T1, T2), TResult>();

		TResult WrappedFunction(T1 arg1, T2 arg2)
		{
			if (memoizedResults.TryGetValue((arg1, arg2), out var value))
			{
				return value;
			}
			else
			{
				var result = function(arg1, arg2);
				memoizedResults.Add((arg1, arg2), result);
				return result;
			}
		}

		return WrappedFunction;
	}

	public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> function)
	{
		var memoizedResults = new Dictionary<(T1, T2, T3), TResult>();

		TResult WrappedFunction(T1 arg1, T2 arg2, T3 arg3)
		{
			if (memoizedResults.TryGetValue((arg1, arg2, arg3), out var value))
			{
				return value;
			}
			else
			{
				var result = function(arg1, arg2, arg3);
				memoizedResults.Add((arg1, arg2, arg3), result);
				return result;
			}
		}

		return WrappedFunction;
	}

	public static Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> function)
	{
		var memoizedResults = new Dictionary<(T1, T2, T3, T4), TResult>();

		TResult WrappedFunction(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			if (memoizedResults.TryGetValue((arg1, arg2, arg3, arg4), out var value))
			{
				return value;
			}
			else
			{
				var result = function(arg1, arg2, arg3, arg4);
				memoizedResults.Add((arg1, arg2, arg3, arg4), result);
				return result;
			}
		}

		return WrappedFunction;
	}
}
