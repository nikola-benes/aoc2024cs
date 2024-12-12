using aoc;

(int, int) RegionPrice(Grid garden, Vec2 start, HashSet<Vec2> visited) {
	if (!visited.Add(start))
		return (0, 0);

	int flower = garden[start];
	var (area, perimeter, sides) = (0, 0, 0);
	var fences = new Dictionary<Vec2, List<Vec2>>();

	var queue = new Queue<Vec2>();
	queue.Enqueue(start);

	while (queue.TryDequeue(out var pos)) {
		++area;
		var fencesHere = fences[pos] = Grid.dirs4.ToList();
		var next = garden.Neighbours4(pos)
			.Where(p => garden[p] == flower);

		foreach (var npos in next) {
			fencesHere.Remove(npos - pos);
			if (visited.Add(npos))
				queue.Enqueue(npos);
		}

		perimeter += fencesHere.Count;
		sides     += fencesHere.Count;

		foreach (var npos in next) {
			if (fences.TryGetValue(npos, out var fs))
				sides -= fs.Intersect(fencesHere).Count();
		}
	}

	return (area * perimeter, area * sides);
}

var garden = Aoc.ConsoleLines().ToGrid();
var visited = new HashSet<Vec2>();
var prices = garden.Tiles
	.Select(t => RegionPrice(garden, t.pos, visited)).ToArray();

Console.WriteLine(prices.Sum(p => p.Item1));
Console.WriteLine(prices.Sum(p => p.Item2));
