using aoc;

var map = Aoc.ConsoleLines().ToGrid();
var (_, start) = map.Tiles.First(t => t.c == 'S');

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

IEnumerable<Vec2> EmptyInDist(Vec2 start, int dist) =>
	from dx in Enumerable.Range(-dist, 2 * dist + 1)
	let distY = dist - Math.Abs(dx)
	from dy in Enumerable.Range(-distY, 2 * distY + 1)
	let pos = new Vec2(start.x + dx, start.y + dy)
	where pos != start && map.GetOr(pos, '#') != '#'
	select pos;

int MDist(Vec2 a, Vec2 b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

int Solve(int maxDist) => (
	from t in map.Tiles
	where t.c != '#'
	let start = t.pos
	from end in EmptyInDist(start, maxDist)
	select dist[end] - dist[start] - MDist(start, end)
).Count(s => s >= 100);

Console.WriteLine(Solve(2));
Console.WriteLine(Solve(20));
