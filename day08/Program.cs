using YX = (int y, int x);

IEnumerable<string> ConsoleLines() {
	while (Console.ReadLine() is {} line) {
		yield return line;
	}
}

YX LinearCombination(int a, YX u, int b, YX v)
	=> (a * u.y + b * v.y, a * u.x + b * v.x);

var map = ConsoleLines().ToArray();
var antennas = map.IndexedTiles()
	.Where(t => t.c != '.')
	.ToLookup(t => t.c, t => t.pos);

var pairs = from g in antennas
	from a in g
	from b in g
	where a != b
	select (a, b);

Console.WriteLine(pairs
	.Select(p => LinearCombination(2, p.a, -1, p.b))
	.Where(map.InBounds)
	.Distinct().Count());

Console.WriteLine(pairs.
	SelectMany(p =>
		Enumerable.Range(1, int.MaxValue)
		.Select(k => LinearCombination(k, p.a, -k + 1, p.b))
		.TakeWhile(map.InBounds))
	.Distinct().Count());

static class Extensions {
	public static
	IEnumerable<(char c, YX pos)> IndexedTiles(this IEnumerable<string> map)
		=> map.SelectMany((row, y)
			=> row.Select((c, x) => (c, (y, x))));

	public static
	bool InBounds(this IList<string> map, YX p)
		=> 0 <= p.y && p.y < map.Count
		&& 0 <= p.x && p.x < map[p.y].Length;
}
