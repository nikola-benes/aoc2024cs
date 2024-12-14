using aoc;
using System.Text.RegularExpressions;

Regex numRe = new(@"-?\d+");
var robots = Aoc.ConsoleLines().Select(line => {
	var nums = numRe.Matches(line)
		.Select(m => int.Parse(m.Value)).ToArray();
	return (pos: new Vec2(nums[0], nums[1]),
	        vel: new Vec2(nums[2], nums[3]));
}).ToArray();

int Mod(int p, int q) => p % q is var r && r < 0 ? r + q : r;

var (w, h) = robots.Length > 100 ? (101, 103) : (11, 7);
var Wrap = (Vec2 v) => new Vec2(Mod(v.x, w), Mod(v.y, h));

Console.WriteLine(robots
	.Select(pv => Wrap(pv.pos + pv.vel * 100))
	.Where(p => p.x != w / 2 && p.y != h / 2)
	.ToLookup(p => (p.x < w / 2, p.y < h / 2))
	.Select(g => g.Count())
	.Aggregate((a, b) => a * b)
);

// run mirror 75
// notice the horizontal band starting at line 26
// run line 26 30
// look for the tree
var solution = 7892;
Console.WriteLine(solution);

if (args.Length == 0)
	return;

void Draw(HashSet<Vec2> ps, bool withNL = false) {
	for (int y = 0; y < h; ++y) {
		if (withNL) {
			Console.Write("{0,3}: ", y);
		}
		for (int x = 0; x < w; ++x) {
			Console.Write(ps.Contains((x, y)) ? "██" : "  ");
		}
		Console.WriteLine();
	}
}

Console.Write("\x1b[25l\x1b[2J\x1b[H");

if (args[0] == "final") {
	Draw(robots
		.Select(pv => Wrap(pv.pos + solution * pv.vel))
		.ToHashSet()
	);
	return;
}

var ShouldShow = (HashSet<Vec2> _) => true;

if (args[0] == "mirror" && args.Length >= 2
		&& int.TryParse(args[1], out var mt)) {
	ShouldShow = ps => {
		var mirror = ps.Select(p => p with { x = w - 1 - p.x });
		return ps.Intersect(mirror).Count() >= mt;
	};
} else if (args[0] == "line" && args.Length >= 3
		&& int.TryParse(args[1], out var line)
		&& int.TryParse(args[2], out var lt)) {
	ShouldShow = ps => ps.Where(p => p.y == line).Count() >= lt;
}

for (int i = 0; true; ++i) {
	var positions = robots.Select(pv => pv.pos).ToHashSet();
	if (ShouldShow(positions)) {
		Draw(positions, true);
		Console.WriteLine(i);
		Thread.Sleep(200);
		Console.Write("\x1b[H");
	}
	foreach (ref var r in robots.AsSpan()) {
		r.pos = Wrap(r.pos + r.vel);
	}
}
