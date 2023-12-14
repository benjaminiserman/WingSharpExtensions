namespace WingSharpExtensions;

using System;
using System.Collections.Generic;

public class AStar<T>
	where T : notnull
{
	public PriorityQueue<T, double> OpenQueue;
	public LazyDictionary<T, double> OpenSet;
	public Dictionary<T, T> CameFrom;

	public LazyDictionary<T, double> BestPathTo;
	public LazyDictionary<T, double> DistanceFromGoalEstimate;

	public (List<T> nodes, double distance) BestPathThusFar;

	public int NodesExplored;

	public AStar()
	{
		Clear();
	}

	public void Clear()
	{
		OpenQueue = new PriorityQueue<T, double>();
		OpenSet = new LazyDictionary<T, double>();
		CameFrom = new Dictionary<T, T>();

		BestPathTo = new LazyDictionary<T, double>()
		{
			GetDefault = _ => double.PositiveInfinity
		};
		DistanceFromGoalEstimate = new LazyDictionary<T, double>()
		{
			GetDefault = _ => double.PositiveInfinity
		};

		BestPathThusFar = (new(), double.PositiveInfinity);
		NodesExplored = 0;
	}

	public (List<T> nodes, double distance) ReconstructPath(T current)
	{
		var totalDistance = BestPathTo[current];
		var path = new List<T>() { current };
		var set = new HashSet<T>() { current };
		while (CameFrom.ContainsKey(current))
		{
			current = CameFrom[current];
			if (set.Contains(current))
			{
				break;
			}

			path.Add(current);
			set.Add(current);
		}

		path.Reverse();

		return (path, totalDistance);
	}

	public bool SearchPathFor(T current, T target)
	{
		while (true)
		{
			if (current.Equals(target))
			{
				return true;
			}

			if (CameFrom.TryGetValue(current, out var next))
			{
				current = next;
			}
			else
			{
				return false;
			}
		}
	}

	public Func<T, bool> DefaultCutoff => current => BestPathTo[current] > BestPathThusFar.distance;

	public (List<T> nodes, double distance) Path(
		T start, 
		Func<T, bool> goal, 
		Func<T, double> heuristic, 
		Func<T, T, double> distance,
		Func<T, IEnumerable<T>> neighbors,
		bool searchExhaustively = false,
		Func<T, bool>? cutoff = null)
	{
		var heuristicResult = heuristic(start);
		OpenSet[start] = heuristicResult;
		OpenQueue.Enqueue(start, heuristicResult);
		BestPathTo[start] = 0;
		DistanceFromGoalEstimate[start] = heuristicResult;

		while (OpenQueue.TryDequeue(out var current, out var gotPriority))
		{
			if (gotPriority > DistanceFromGoalEstimate[current])
			{
				continue;
			}

			if (goal(current))
			{
				var path = ReconstructPath(current);

				if (!searchExhaustively)
				{
					return path;
				}

				if (path.distance < BestPathThusFar.distance)
				{
					BestPathThusFar = path;
				}
			}

			if (cutoff is not null && cutoff(current))
			{
				continue;
			}

			NodesExplored++;

			foreach (var neighbor in neighbors(current))
			{
				var distanceFromStart = BestPathTo[current] + distance(current, neighbor);
				if (distanceFromStart < BestPathTo[neighbor])
				{
					if (CameFrom.TryGetValue(neighbor, out var previous) 
						&& previous.Equals(current))
					{
						return (BestPathThusFar.nodes, double.NegativeInfinity);
					}

					//Console.WriteLine($"{current} => {neighbor}, {distanceFromStart}");
					CameFrom[neighbor] = current;
					BestPathTo[neighbor] = distanceFromStart;
					DistanceFromGoalEstimate[neighbor] = distanceFromStart + heuristic(neighbor);

					if (!OpenSet.TryGetValue(neighbor, out var neighborPriority) 
						|| neighborPriority > DistanceFromGoalEstimate[neighbor])
					{
						//Console.WriteLine($"{neighbor}: {distanceFromStart}, nodes: {NodesExplored}, {DistanceFromGoalEstimate[neighbor]}");
						OpenSet[neighbor] = DistanceFromGoalEstimate[neighbor];
						OpenQueue.Enqueue(neighbor, DistanceFromGoalEstimate[neighbor]);
					}
				}
			}
		}

		return BestPathThusFar;
	}
}
