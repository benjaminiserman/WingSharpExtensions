namespace WingSharpExtensionsTest;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class AStarTests
{
	private int?[][] GetMatrix() => new int?[][]
	{
		new int?[] { null, 5, 3, 5, null },
		new int?[] { 5, null, 5, -1, null },
		new int?[] { 4, null, 5, 6, null },
		new int?[] { -10, 5, 2, null, 1 },
		new int?[] { 7, null, 5, null, null },
	};

	[TestMethod, Timeout(1000)]
	public void AStar_DetectsNegativeCycle()
	{
		var matrix = GetMatrix();

		IEnumerable<int> Neighbors(int current)
		{
			foreach (var (distance, neighbor) in matrix![current].WithIndex())
			{
				if (distance is not null)
				{
					yield return neighbor;
				}
			}
		}

		var aStar = new AStar<int>();
		var result = aStar.Path(start: 0,
			  goal: x => x == 4,
			  heuristic: _ => -1000,
			  distance: (a, b) => (double)matrix[a][b]!,
			  neighbors: Neighbors,
			  searchExhaustively: true);

		Assert.AreEqual(double.NegativeInfinity, result.distance);
	}
}