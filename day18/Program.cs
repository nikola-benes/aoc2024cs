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

HashSet<Vec2> onPath = new();

int? ShortestPath() {
	Queue<Vec2> queue = new();
	Dictionary<Vec2, (int, Vec2)> distPrev = new();
	queue.Enqueue(start);
	distPrev[start] = (0, start);
	onPath.Clear();

	while (queue.TryDequeue(out var pos)) {
		var (d, _) = distPrev[pos];
		foreach (var dir in Grid.dirs4) {
			var npos = pos + dir;
			if (npos == goal) {
				while (pos != start) {
					onPath.Add(pos);
					(_, pos) = distPrev[pos];
				}
				return d + 1;
			}

			if (IsEmpty(npos)
					&& distPrev.TryAdd(npos, (d + 1, pos)))
				queue.Enqueue(npos);
		}
	}

	return null;
}

Console.WriteLine(ShortestPath());

foreach (var next in bytes.Skip(fallen)) {
	blocked.Add(next);
	if (onPath.Contains(next) && ShortestPath() is null) {
		Console.WriteLine("{0},{1}", next.x, next.y);
		break;
	}
}
