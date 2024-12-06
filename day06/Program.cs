using YX = (int y, int x);

// I would love to have ‹using PDSet = HashSet<(YX pos, YX dir)>› here…
using PDSet = System.Collections.Generic.HashSet
	<((int y, int x) pos, (int y, int x) dir)>;

IEnumerable<string> ConsoleLines() {
	while (Console.ReadLine() is {} line) {
		yield return line;
	}
}

YX Add(YX a, YX b) => (a.y + b.y, a.x + b.x);
YX RotR(YX d) => (d.x, -d.y);

var map = ConsoleLines().ToArray();
var (_, start) = map.SelectMany((row, y) =>
	row.Select((c, x) => (c: c, (y: y, x: x)))).First(cp => cp.c == '^');

var Get = (YX p, YX? block) =>
	p.y < 0 || p.y >= map.Length || p.x < 0 || p.x >= map[p.y].Length
		? '@' : p == block ? '#' : map[p.y][p.x];

PDSet? MoveGuard(YX pos, YX? block = null) {
	// returns null if the guard loops
	var seen = new PDSet();
	YX dir = (-1, 0);

	while (Get(pos, block) != '@') {
		if (!seen.Add((pos, dir)))
			return null;

		var npos = Add(pos, dir);

		if (Get(npos, block) == '#') {
			dir = RotR(dir);
		} else {
			pos = npos;
		}
	}
	return seen;
}

var visited = MoveGuard(start)!.Select(pd => pd.pos).Distinct().ToArray();

Console.WriteLine(visited.Length);
Console.WriteLine(visited.Where(p => p != start).AsParallel()
	.Count(p => MoveGuard(start, p) is null));
