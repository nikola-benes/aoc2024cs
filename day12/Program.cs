using aoc;

var garden = Aoc.ConsoleLines().ToGrid();
var (part1, part2) = (0, 0);

var visited = new HashSet<Vec2>();
var queue = new Queue<Vec2>();

foreach (var (flower, start) in garden.Tiles) {
	if (!visited.Add(start))
		continue;

	var (area, perimeter, sides) = (0, 0, 0);
	var fences = new Dictionary<Vec2, List<Vec2>>();

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

	part1 += area * perimeter;
	part2 += area * sides;
}

Console.WriteLine(part1);
Console.WriteLine(part2);
