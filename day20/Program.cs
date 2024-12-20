using aoc;

var map = Aoc.ConsoleLines().ToGrid();
var (_, start) = map.Tiles.First(t => t.c == 'S');
var (_, end) = map.Tiles.First(t => t.c == 'E');

Dictionary<Vec2, int> dist = new();
Queue<Vec2> queue = new();
dist[start] = 0;
queue.Enqueue(start);

while (queue.TryDequeue(out var pos)) {
	var d = dist[pos];
	foreach (var npos in map.Neighbours4(pos)) {
		if (map[npos] != '#' && dist.TryAdd(npos, d + 1))
			queue.Enqueue(npos);
	}
}

// Assumes that each wall is used in at most one (small) cheat.
// (Works for both the example and my input.)
var saves =
	from t in map.Tiles
	where t.c == '#'
	let ds = (
		from p in map.Neighbours4(t.pos)
		where map[p] != '#'
		select dist[p]
	).ToArray()
	where ds.Any()
	select ds.Max() - ds.Min() - 2;

Console.WriteLine(saves.Count(s => s >= 100));

int MDist(Vec2 a, Vec2 b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

saves =
	from t1 in map.Tiles
	where t1.c != '#'
	from t2 in map.Tiles
	where t2.c != '#' && t1.pos != t2.pos
	let d = MDist(t1.pos, t2.pos)
	where d <= 20
	select dist[t2.pos] - dist[t1.pos] - d;
;

Console.WriteLine(saves.Count(s => s >= 100));
