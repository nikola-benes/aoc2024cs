using YX = (int y, int x);

IEnumerable<string> ConsoleLines() {
	while (Console.ReadLine() is {} line) {
		yield return line;
	}
}

YX Add(YX a, YX b) => (a.y + b.y, a.x + b.x);
YX RotR(YX d) => (d.x, -d.y);

var map = ConsoleLines().ToArray();
var positions = map.SelectMany((row, y) => row.Select((_, x) => (y: y, x: x)));
var Get = (YX p, YX? block = null) =>
	p.y < 0 || p.y >= map.Length || p.x < 0 || p.x >= map[p.y].Length
		? '@' : p == block ? '#' : map[p.y][p.x];

YX start = positions.First(p => Get(p) == '^');

int MoveGuard(YX pos, YX? block = null) {
	// returns -1 if the guard loops
	var seen = new HashSet<(YX pos, YX dir)>();
	YX dir = (-1, 0);

	while (Get(pos, block) != '@') {
		if (!seen.Add((pos, dir)))
			return -1;  // loop

		if (Get(Add(pos, dir), block) == '#') {
			dir = RotR(dir);
		} else {
			pos = Add(pos, dir);
		}
	}
	return seen.DistinctBy(pd => pd.pos).Count();
}

Console.WriteLine(MoveGuard(start));
Console.WriteLine(positions.Where(p => Get(p) == '.')
	.Count(p => MoveGuard(start, p) == -1));
