using aoc;

Dictionary<char, Vec2> dirs = new() {
	['^'] = ( 0, -1),
	['v'] = ( 0,  1),
	['<'] = (-1,  0),
	['>'] = ( 1,  0),
};

var keypad = (new[] { "789", "456", "123", ".0A" }).ToGrid();
var arrows = (new[] { ".^A", "<v>" }).ToGrid();

Vec2 Find(Grid pad, char button)
	=> pad.Tiles.First(t => t.c == button).pos;

bool Possible(Grid pad, Vec2 pos, string moves) {
	foreach (char m in moves) {
		pos += dirs[m];
		if (pad.GetOr(pos, '.') == '.')
			return false;
	}
	return true;
}

Dictionary<(char, char, int), long> cache = new();

long MoveFromTo(Grid pad, char from, char to, int layers) {
	var key = (from, to, layers);
	if (cache.TryGetValue(key, out var r))
		return r;

	var src = Find(pad, from);
	var dst = Find(pad, to);
	var (dx, dy) = dst - src;
	List<string> moves = new();
	if (dx < 0) {
		moves.Add(new string('<', -dx));
	} else if (dx > 0) {
		moves.Add(new string('>', dx));
	}
	if (dy < 0) {
		moves.Add(new string('^', -dy));
	} else if (dy > 0) {
		moves.Add(new string('v', dy));
	}

	return cache[key] =
		(moves.Count == 0 ? new[] { "" } : moves.Count == 1 ?
			moves.ToArray() :
			new[] { moves[0] + moves[1], moves[1] + moves[0] })
		.Where(o => Possible(pad, src, o))
		.Select(o => Move(arrows, o, layers - 1))
		.Min();
}

long Move(Grid pad, string moves, int layers)
	=> layers == 0 ? moves.Length + 1
	 : ("A" + moves).Zip(moves + "A")
		.Sum(p => MoveFromTo(pad, p.First, p.Second, layers));

var ps = Aoc.ConsoleLines().Select(p => p[.. ^1]).ToArray();

Console.WriteLine(ps.Sum(p => int.Parse(p) * Move(keypad, p, 3)));
Console.WriteLine(ps.Sum(p => int.Parse(p) * Move(keypad, p, 26)));
