using aoc;

bool IsLock(Grid g) => Enumerable.Range(0, g.Width).All(x => g[(x, 0)] == '#');

bool Fit(Grid key, Grid lck)
	=> key.Tiles.Zip(lck.Tiles, (t1, t2) => t1.c == '.' || t2.c == '.')
		.All(b => b);

var (keys, locks) = Aoc.ConsoleLines().SplitBy(line => line == "")
	.Select(s => s.ToGrid())
	.ToLookup(IsLock) switch { var lk => (lk[true], lk[false]) };

Console.WriteLine((
	from key in keys
	from lck in locks
	where Fit(key, lck)
	select 1
).Count());
