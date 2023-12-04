namespace WingSharpExtensions;

using System;
using System.Collections.Generic;

public class AStar<T>
	where T : notnull
{
	public PriorityQueue<T, double> OpenQueue;
	public HashSet<T> OpenSet;
	public Dictionary<T, T> CameFrom;

	public LazyDictionary<T, double> BestPathTo;
	public LazyDictionary<T, double> DistanceFromGoalEstimate;

	public (List<T> nodes, double distance) BestPathThusFar;

	public AStar()
	{
		Clear();
	}

	public void Clear()
	{
		OpenQueue = new PriorityQueue<T, double>();
		OpenSet = new HashSet<T>();
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
	}

	public (List<T> nodes, double distance) ReconstructPath(T current)
	{
		var totalDistance = BestPathTo[current];
		var path = new List<T>() { current };
		while (CameFrom.ContainsKey(current))
		{
			current = CameFrom[current];
			path.Add(current);
		}

		path.Reverse();

		return (path, totalDistance);
	}

	public (List<T> nodes, double distance) Path(
		T start, 
		Func<T, bool> goal, 
		Func<T, double> heuristic, 
		Func<T, T, double> distance,
		Func<T, IEnumerable<T>> neighbors,
		bool searchExhaustively = false)
	{
		OpenSet.Add(start);
		OpenQueue.Enqueue(start, heuristic(start));
		BestPathTo[start] = 0;
		DistanceFromGoalEstimate[start] = heuristic(start);

		while (OpenQueue.TryDequeue(out var current, out _))
		{
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

			if (BestPathTo[current] > BestPathThusFar.distance)
			{
				continue;
			}

			foreach (var neighbor in neighbors(current))
			{
				var distanceFromStart = BestPathTo[current] + distance(current, neighbor);
				if (distanceFromStart < BestPathTo[neighbor])
				{
					CameFrom[neighbor] = current;
					BestPathTo[neighbor] = distanceFromStart;
					DistanceFromGoalEstimate[neighbor] = distanceFromStart + heuristic(neighbor);
					if (!OpenSet.Contains(neighbor))
					{
						OpenSet.Add(neighbor);
						OpenQueue.Enqueue(neighbor, DistanceFromGoalEstimate[neighbor]);
					}
				}
			}
		}

		return BestPathThusFar;
	}
}
