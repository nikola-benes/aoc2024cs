using aoc;
using PD = (aoc.Vec2 pos, aoc.Vec2 dir);

Vec2 Rot(Vec2 d, bool r) => r ? (-d.y, d.x) : (d.y, -d.x);

var map = Aoc.ConsoleLines().ToGrid();
var (_, s) = map.Tiles.First(t => t.c == 'S');
var start = (s, (1, 0));

Dictionary<PD, List<PD>> prev = new();
Dictionary<PD, int> dist = new();
HashSet<PD> closed = new();
PriorityQueue<PD, int> pq = new();

dist.Add(start, 0);
prev.Add(start, new());
pq.Enqueue(start, 0);

void Relax(PD src, PD dst, int w) {
	var relaxed = dist[src] + w;
	if (!dist.TryGetValue(dst, out var d_dst) || d_dst > relaxed) {
		dist[dst] = d_dst = relaxed;
		prev[dst] = new();
		pq.Enqueue(dst, relaxed);
	}
	if (d_dst == relaxed) {
		prev[dst].Add(src);
	}
}

int best = int.MaxValue;
List<PD> goals = new();

while (pq.TryDequeue(out var pd, out var d)) {
	if (!closed.Add(pd) || d > best)
		continue;

	var (pos, dir) = pd;

	if (map[pos] == 'E') {
		best = d;
		goals.Add(pd);
	}

	var next = pos + dir;
	if (map[next] != '#')
		Relax(pd, (next, dir), 1);

	Relax(pd, (pos, Rot(dir, true)), 1000);
	Relax(pd, (pos, Rot(dir, false)), 1000);
}

Console.WriteLine(best);

var seen = goals.ToHashSet();
var queue = goals.ToQueue();

while (queue.TryDequeue(out var pd)) {
	foreach (var npd in prev[pd]) {
		if (seen.Add(npd))
			queue.Enqueue(npd);
	}
}

Console.WriteLine(seen.Select(pd => pd.pos).Distinct().Count());
