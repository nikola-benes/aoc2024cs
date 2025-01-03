using aoc;

Vec2[] dirs4 = { (1, 0), (0, 1), (-1, 0), (0, -1) };

var map = Aoc.ConsoleLines().ToGrid();

var heads = (from t in map.Tiles where t.c == '0' select t.pos).ToArray();
var tails = new List<Vec2>();

var sources = heads.ToDictionary(p => p, p => (new[] { p }).ToHashSet());
var paths = heads.ToDictionary(p => p, _ => 1);
var queue = heads.ToQueue();

// Compute everything using just one BFS, yay!
while (queue.TryDequeue(out var pos)) {
	var h = map[pos];
	if (h == '9') {
		tails.Add(pos);
		continue;
	}

	var s = sources[pos];
	var p = paths[pos];

	foreach (var d in dirs4) {
		var npos = pos + d;
		if (map.GetOr(npos, '.') != h + 1) continue;

		if (sources.ContainsKey(npos)) {
			sources[npos].UnionWith(s);
			paths[npos] += p;
		} else {
			// HashSet has no Clone :-/
			sources[npos] = s.ToHashSet();
			paths[npos] = p;
			queue.Enqueue(npos);
		}
	}
}

Console.WriteLine(tails.Sum(p => sources[p].Count));
Console.WriteLine(tails.Sum(p => paths[p]));
