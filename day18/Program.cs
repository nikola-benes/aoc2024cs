using aoc;

var bytes = Aoc.ConsoleLines().Select(line => {
	var nums = line.Split(',').Select(int.Parse).ToArray();
	return new Vec2(nums[0], nums[1]);
}).ToArray();

Vec2 start = (0, 0);
Vec2 goal = bytes.Length < 100 ? (6, 6) : (70, 70);
int fallen = bytes.Length < 100 ? 12 : 1024;
var (w, h) = goal + (1, 1);
var blocked = bytes.Take(fallen).ToHashSet();

var IsEmpty = (Vec2 p)
	=> 0 <= p.x && p.x < w && 0 <= p.y && p.y < h && !blocked.Contains(p);

int? ShortestPath() {
	Queue<Vec2> queue = new();
	Dictionary<Vec2, int> dist = new();
	queue.Enqueue(start);
	dist[start] = 0;

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

int? FirstBlocked() {
	PriorityQueue<Vec2, int> pq = new();
	HashSet<Vec2> seen = new();
	var bTime = bytes.Select((p, i) => (p, i)).ToDictionary();
	var max = bytes.Length;
	pq.Enqueue(start, -max);

	// this is a graph search with a priority queue that is *not* Dijkstra
	while (pq.TryDequeue(out var pos, out var first)) {
		first = -first;

		foreach (var dir in Grid.dirs4) {
			var npos = pos + dir;
			if (npos == goal)
				return first;

			if (IsEmpty(npos) && seen.Add(npos)) {
				var block = bTime.GetValueOrDefault(npos, max);
				pq.Enqueue(npos, -Math.Min(first, block));
			}
		}
	}

	return null;
}

Console.WriteLine(ShortestPath());

var b = bytes[FirstBlocked()!.Value];
Console.WriteLine("{0},{1}", b.x, b.y);
