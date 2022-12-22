namespace WingSharpExtensions;

using System.Collections.Generic;
using System.Linq;

public static class ChunkByExtension
{
	public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> enumerable, int chunkBy)
	{
		foreach (var item in enumerable
			.Select((x, i) => (x, i))
			.GroupBy(x => x.i / chunkBy)
			.Select(g => g.Select(x => x.x)))
		{
			yield return item;
		}
	}
}
