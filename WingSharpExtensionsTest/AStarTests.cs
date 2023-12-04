namespace WingSharpExtensionsTest;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WingSharpExtensions;

[TestClass]
public class AStarTests
{
	enum Direction
	{
		Up, Down, Left, Right
	}

	record Blizzard(Direction Direction)
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Blizzard Move()
		{
			return Direction switch
			{
				Direction.Up => new(Direction)
				{
					X = X,
					Y = Y - 1
				},
				Direction.Left => new(Direction)
				{
					X = X - 1,
					Y = Y
				},
				Direction.Right => new(Direction)
				{
					X = X + 1,
					Y = Y
				},
				Direction.Down => new(Direction)
				{
					X = X,
					Y = Y + 1
				},
			};
		}
	}

	record BoardState(List<Blizzard> Blizzards, HashSet<(int x, int y)> BlizzardPositions, int Time);

	record Node((int x, int y) pos, BoardState boardState);

	[TestMethod]
	public void AStar_CorrectResult()
	{
		var input = File.ReadAllLines("blizzard_1.txt");

		var walls = new HashSet<(int x, int y)>();
		var openTiles = new HashSet<(int x, int y)>();
		var blizzards = new List<Blizzard>();

		for (var i = 0; i < input.Length; i++)
		{
			for (var j = 0; j < input[i].Length; j++)
			{
				switch (input[i][j])
				{
					case '#':
						walls.Add((j, i));
						break;
					case '.':
						openTiles.Add((j, i));
						break;
					case '^':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Up)
						{
							X = j,
							Y = i,
						});
						break;
					case 'v':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Down)
						{
							X = j,
							Y = i,
						});
						break;
					case '>':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Right)
						{
							X = j,
							Y = i,
						});
						break;
					case '<':
						openTiles.Add((j, i));
						blizzards.Add(new(Direction.Left)
						{
							X = j,
							Y = i,
						});
						break;
				}
			}
		}

		int xMin, xMax, yMin, yMax;
		xMin = walls.Min(x => x.x);
		xMax = walls.Max(x => x.x);
		yMin = walls.Min(x => x.y);
		yMax = walls.Max(x => x.y);

		List<BoardState> boardStates = new()
		{
			new(blizzards, blizzards.Select(x => (x.X, x.Y)).ToHashSet(), 0)
		};

		BoardState GetBoardState(int i)
		{
			if (i < boardStates.Count)
			{
				return boardStates[i];
			}
			else
			{
				return SimulateBoardState(i);
			}
		}

		BoardState SimulateBoardState(int target)
		{
			var lastState = boardStates.Last();
			var blizzards = new List<Blizzard>();
			for (var i = boardStates.Count; i <= target; i++)
			{
				foreach (var blizzard in lastState.Blizzards)
				{
					var newBlizzard = blizzard.Move();
					if (newBlizzard.X >= xMax)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = xMin + 1,
							Y = newBlizzard.Y
						};
					}
					else if (newBlizzard.Y >= yMax)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = newBlizzard.X,
							Y = yMin + 1
						};
					}
					else if (newBlizzard.X <= xMin)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = xMax - 1,
							Y = newBlizzard.Y
						};
					}
					else if (newBlizzard.Y <= yMin)
					{
						newBlizzard = new(newBlizzard.Direction)
						{
							X = newBlizzard.X,
							Y = yMax - 1
						};
					}

					blizzards.Add(newBlizzard);
				}

				lastState = new(blizzards, blizzards.Select(x => (x.X, x.Y)).ToHashSet(), i);
				boardStates.Add(lastState);
				blizzards = new();
			}

			return boardStates.Last();
		}

		var aStar = new AStar<Node>();
		var start = openTiles.Where(t => t.y == yMin).MinBy(t => t.x);
		var target = openTiles.Where(t => t.y == yMax).MaxBy(t => t.x);

		IEnumerable<Node> Neighbors(Node node)
		{
			var x = node.pos.x;
			var y = node.pos.y;
			var currentState = GetBoardState(node.boardState.Time + 1);

			if (openTiles.Contains((x + 1, y))
				&& !currentState.BlizzardPositions.Contains((x + 1, y)))
			{
				yield return new Node((x + 1, y), currentState);
			}

			if (openTiles.Contains((x, y + 1))
				&& !currentState.BlizzardPositions.Contains((x, y + 1)))
			{
				yield return new Node((x, y + 1), currentState);
			}

			if (openTiles.Contains((x - 1, y))
				&& !currentState.BlizzardPositions.Contains((x - 1, y)))
			{
				yield return new Node((x - 1, y), currentState);
			}

			if (openTiles.Contains((x, y - 1))
				&& !currentState.BlizzardPositions.Contains((x, y - 1)))
			{
				yield return new Node((x, y - 1), currentState);
			}

			if (openTiles.Contains((x, y))
				&& !currentState.BlizzardPositions.Contains((x, y)))
			{
				yield return new Node((x, y), currentState);
			}
		}

		var result = aStar.Path(new(start, boardStates[0]), 
			goal: node => node.pos == target,
			heuristic: node => node.boardState.Time + Math.Abs(target.x - node.pos.x) + Math.Abs(target.y - node.pos.y),
			distance: (_, _) => 1,
			neighbors: Neighbors,
			searchExhaustively: false);

		Console.WriteLine($"Explored: {aStar.NodesExplored}");

		Assert.AreEqual(299, result.distance);
	}
}