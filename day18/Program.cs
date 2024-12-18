using aoc;

var bytes = Aoc.ConsoleLines().Select(line => {
	var nums = line.Split(',').Select(int.Parse).ToArray();
	return new Vec2(nums[0], nums[1]);
}).ToArray();

Vec2 goal = bytes.Length < 100 ? (6, 6) : (70, 70);
int fallen = bytes.Length < 100 ? 12 : 1024;
var (w, h) = goal + (1, 1);
var blocked = bytes.Take(fallen).ToHashSet();

var IsEmpty = (Vec2 p)
	=> 0 <= p.x && p.x < w && 0 <= p.y && p.y < h && !blocked.Contains(p);

int? ShortestPath() {
	Queue<Vec2> queue = new();
	Dictionary<Vec2, int> dist = new();
	queue.Enqueue((0, 0));
	dist[(0, 0)] = 0;

	while (queue.TryDequeue(out var pos)) {
		var d = dist[pos];
		foreach (var dir in Grid.dirs4) {
			var npos = pos + dir;
			if (npos == goal)
				return d + 1;

			if (IsEmpty(npos) && dist.TryAdd(npos, d + 1))
				queue.Enqueue(npos);
		}
	}

	return null;
}

Console.WriteLine(ShortestPath());

foreach (var next in bytes.Skip(fallen)) {
	blocked.Add(next);
	if (ShortestPath() is null) {
		Console.WriteLine("{0},{1}", next.x, next.y);
		break;
	}
}
