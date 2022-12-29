namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class HashListTests
{
	private void AssertListsMatch<T>(List<T> list, HashList<T> hashlist)
	{
		Assert.AreEqual(list.Count, hashlist.Count);
		foreach (var (listValue, hashlistValue) in list.Zip(hashlist))
		{
			Assert.AreEqual(listValue, hashlistValue);
			Assert.AreEqual(1, hashlist.Count(x => x?.Equals(hashlistValue) ?? hashlist is null));
		}
	}

	private static List<int> GetRandomList(int n) => Enumerable.Repeat(0, n)
		.Select(x => Random.Shared.Next(0, n))
		.ToList();

	#region RegularCollectionMethods
	[TestMethod]
	public void Add_VerifyNoDuplicatesAndOrderingPreserved()
	{
		var hashlist = new HashList<int>();
		var list = new List<int>();
		var set = new HashSet<int>();

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var number = Random.Shared.Next(0, 100);
			hashlist.Add(number);
			if (set.Add(number))
			{
				list.Add(number);
			}
		}

		AssertListsMatch(list, hashlist);
	}

	[TestMethod]
	public void ICollectionAdd_VerifyNoDuplicatesAndOrderingPreserved()
	{
		var hashlist = new HashList<int>();
		var list = new List<int>();
		var set = new HashSet<int>();

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var number = Random.Shared.Next(0, 100);
			((ICollection<int>)hashlist).Add(number);
			if (set.Add(number))
			{
				((ICollection<int>)list).Add(number);
			}
		}

		AssertListsMatch(list, hashlist);
	}

	[TestMethod]
	public void Insert_VerifyNoDuplicatesAndOrderingPreserved()
	{
		var hashlist = new HashList<int>();
		var list = new List<int>();
		var set = new HashSet<int>();

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var number = Random.Shared.Next(0, 100);
			var index = Random.Shared.Next(0, list.Count);
			hashlist.Insert(index, number);
			if (set.Add(number))
			{
				list.Insert(index, number);
			}
		}

		AssertListsMatch(list, hashlist);
	}

	[TestMethod]
	public void TryInsert_VerifyNoDuplicatesAndOrderingPreserved()
	{
		var hashlist = new HashList<int>();
		var list = new List<int>();
		var set = new HashSet<int>();

		var listResults = new List<bool>();
		var hashlistResults = new List<bool>();

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var number = Random.Shared.Next(0, 100);
			var index = Random.Shared.Next(0, list.Count);
			hashlistResults.Add(hashlist.TryInsert(index, number));
			if (set.Add(number))
			{
				list.Insert(index, number);
				listResults.Add(true);
			}
			else
			{
				listResults.Add(false);
			}
		}

		AssertListsMatch(list, hashlist);
		foreach (var (listResult, hashlistResult) in listResults.Zip(hashlistResults))
		{
			Assert.AreEqual(listResult, hashlistResult);
		}
	}

	[TestMethod]
	public void TrySet_VerifyNoDuplicatesAndOrderingPreserved()
	{
		var n = 10;
		var set = GetRandomList(n).ToHashSet();
		var list = set.ToList();
		var hashlist = new HashList<int>(list);

		var listResults = new List<bool>();
		var hashlistResults = new List<bool>();

		Console.WriteLine($"list: {string.Join(", ", list)}");
		Console.WriteLine($"hashlist: {string.Join(", ", hashlist)}");

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var index = Random.Shared.Next(0, list.Count);
			var number = Random.Shared.Next(0, 100);

			hashlistResults.Add(hashlist.TrySet(index, number));

			if (set.Contains(number))
			{
				listResults.Add(false);
			}
			else
			{
				set.Remove(list[index]);
				list[index] = number;
				listResults.Add(true);
				set.Add(number);
			}

			Assert.AreEqual(list[index], hashlist[index], $"i: {index}, num: {number}, list: {listResults[^1]}, hashlist: {hashlistResults[^1]}\nlist: {string.Join(", ", list)}\nhashlist: {string.Join(", ", hashlist)}");
		}

		AssertListsMatch(list, hashlist);
		foreach (var (listResult, hashlistResult) in listResults.Zip(hashlistResults))
		{
			Assert.AreEqual(listResult, hashlistResult);
		}
	}

	[TestMethod]
	public void Remove_OrderingPreserved()
	{
		var set = GetRandomList(100).ToHashSet();
		var list = set.ToList();
		var hashlist = new HashList<int>(list);

		foreach (var _ in Enumerable.Range(0, 10))
		{
			var index = Random.Shared.Next(list.Count);
			set.Remove(list[index]);
			hashlist.Remove(list[index]);
			list.Remove(list[index]);
		}

		AssertListsMatch(list, hashlist);
	}

	[TestMethod]
	public void RemoveAt_OrderingPreserved()
	{
		var set = GetRandomList(100).ToHashSet();
		var list = set.ToList();
		var hashlist = new HashList<int>(list);

		foreach (var _ in Enumerable.Range(0, 10))
		{
			var index = Random.Shared.Next(list.Count);
			set.Remove(list[index]);
			hashlist.RemoveAt(index);
			list.RemoveAt(index);
		}

		AssertListsMatch(list, hashlist);
	}

	[TestMethod]
	public void IndexOf_SameAsList()
	{
		var set = GetRandomList(100).ToHashSet();
		var list = set.ToList();
		var hashlist = new HashList<int>(list);

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var value = Random.Shared.Next(list.Count);
			Assert.AreEqual(list.IndexOf(value), hashlist.IndexOf(value));
		}

		AssertListsMatch(list, hashlist);
	}

	[TestMethod]
	public void Clear_CorrectResult()
	{
		var hashlist = new HashList<int>(GetRandomList(100));
		hashlist.Clear();

		Assert.AreEqual(0, hashlist.Count);
	}

	[TestMethod]
	public void IsReadonly_False() => Assert.AreEqual(false, new HashList<int>().IsReadOnly);

	#endregion

	#region SetSpecific
	[TestMethod]
	public void ExceptWith_CorrectResult()
	{
		var setA = GetRandomList(5).ToHashSet();
		var setB = GetRandomList(3).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Console.WriteLine($"setA: {{ {string.Join(", ", setA)} }}");
		Console.WriteLine($"setB: {{ {string.Join(", ", setB)} }}");
		Console.WriteLine($"hashlist: {{ {string.Join(", ", hashlist)} }}");

		setA.ExceptWith(setB);
		hashlist.ExceptWith(setB);

		Console.WriteLine($"setA: {{ {string.Join(", ", setA)} }}");
		Console.WriteLine($"setB: {{ {string.Join(", ", setB)} }}");
		Console.WriteLine($"hashlist: {{ {string.Join(", ", hashlist)} }}");

		Assert.AreEqual(setA.Count, hashlist.Count);
		foreach (var x in setA)
		{
			Assert.IsTrue(hashlist.Contains(x));
		}
	}

	[TestMethod]
	public void IntersectWith_CorrectResult()
	{
		var setA = GetRandomList(3).ToHashSet();
		var setB = GetRandomList(3).ToHashSet();

		var hashlist = new HashList<int>(setA);

		setA.IntersectWith(setB);
		hashlist.IntersectWith(setB);

		Assert.AreEqual(setA.Count, hashlist.Count);
		foreach (var x in setA)
		{
			Assert.IsTrue(hashlist.Contains(x));
		}
	}

	[TestMethod]
	public void IsProperSubsetOf_CorrectResult()
	{
		var setA = GetRandomList(3).ToHashSet();
		var setB = GetRandomList(5).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Assert.AreEqual(setA.IsProperSubsetOf(setB), hashlist.IsProperSubsetOf(setB));
	}

	[TestMethod]
	public void IsProperSupersetOf_CorrectResult()
	{
		var setA = GetRandomList(5).ToHashSet();
		var setB = GetRandomList(3).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Assert.AreEqual(setA.IsProperSupersetOf(setB), hashlist.IsProperSupersetOf(setB));
	}

	[TestMethod]
	public void IsSubsetOf_CorrectResult()
	{
		var setA = GetRandomList(3).ToHashSet();
		var setB = GetRandomList(3).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Assert.AreEqual(setA.IsSubsetOf(setB), hashlist.IsSubsetOf(setB));
	}

	[TestMethod]
	public void IsSupersetOf_CorrectResult()
	{
		var setA = GetRandomList(3).ToHashSet();
		var setB = GetRandomList(3).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Assert.AreEqual(setA.IsSupersetOf(setB), hashlist.IsSupersetOf(setB));
	}

	[TestMethod]
	public void Overlaps_CorrectResult()
	{
		var setA = GetRandomList(10).ToHashSet();
		var setB = GetRandomList(10).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Assert.AreEqual(setA.Overlaps(setB), hashlist.Overlaps(setB));
	}

	[TestMethod]
	public void SetEquals_CorrectResult()
	{
		var setA = GetRandomList(3).ToHashSet();
		var setB = GetRandomList(3).ToHashSet();

		var hashlist = new HashList<int>(setA);

		Assert.AreEqual(setA.SetEquals(setB), hashlist.SetEquals(setB));
	}

	[TestMethod]
	public void SymmetricExceptWith_CorrectResult()
	{
		var setA = GetRandomList(5).ToHashSet();
		var setB = GetRandomList(5).ToHashSet();

		var hashlist = new HashList<int>(setA);

		setA.SymmetricExceptWith(setB);
		hashlist.SymmetricExceptWith(setB);

		Assert.AreEqual(setA.Count, hashlist.Count);
		foreach (var x in setA)
		{
			Assert.IsTrue(hashlist.Contains(x));
		}
	}

	[TestMethod]
	public void UnionWith_CorrectResult()
	{
		var setA = GetRandomList(5).ToHashSet();
		var setB = GetRandomList(5).ToHashSet();

		var hashlist = new HashList<int>(setA);

		setA.UnionWith(setB);
		hashlist.UnionWith(setB);

		Assert.AreEqual(setA.Count, hashlist.Count);
		foreach (var x in setA)
		{
			Assert.IsTrue(hashlist.Contains(x));
		}
	}
	#endregion

	[TestMethod]
	public void ToHashList_CorrectResult()
	{
		var list = GetRandomList(100);
		var hashlist = list.ToHashList();

		var newList = new List<int>();
		foreach (var x in list)
		{
			if (!newList.Contains(x))
			{
				newList.Add(x);
			}
		}

		AssertListsMatch(newList, hashlist);
	}

	[TestMethod]
	public void GetEnumerator_SameOrderAsList()
	{
		var list = GetRandomList(100).ToHashSet().ToList();
		var hashlist = new HashList<int>(list);

		var i = 0;
		foreach (var x in hashlist)
		{
			Assert.AreEqual(list[i], x);
			i++;
		}
	}

	#region Indexer
	[TestMethod]
	public void IndexerGet_SameAsList()
	{
		var list = GetRandomList(100).ToHashSet().ToList();
		var hashlist = new HashList<int>(list);

		foreach (var _ in Enumerable.Range(0, 100))
		{
			var index = Random.Shared.Next(list.Count);
			Assert.AreEqual(list[index], hashlist[index]);
		}
	}

	[TestMethod]
	public void IndexerSet_ValuesNotInList_SameAsList()
	{
		var n = 100;
		var list = GetRandomList(n).ToHashSet().ToList();
		var hashlist = new HashList<int>(list);

		foreach (var i in Enumerable.Range(0, n))
		{
			var index = Random.Shared.Next(list.Count);
			var value = n + i + 1;
			list[index] = value;
			hashlist[index] = value;
			Assert.AreEqual(list[index], hashlist[index]);
		}
	}

	[TestMethod]
	public void IndexerSet_ValueAlreadyInHashList_ThrowsInvalidOperationException()
	{
		var hashlist = new HashList<int>()
		{
			1, 2, 3, 4, 5
		};

		Assert.ThrowsException<InvalidOperationException>(() =>
		{
			hashlist[0] = 5;
		});
	}
	#endregion
}