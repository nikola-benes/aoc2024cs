IEnumerable<long> Blink(long stone) {
	if (stone == 0)
		yield return 1;
	else if (stone.ToString() is var s && s.Length % 2 == 0) {
		yield return long.Parse(s.Substring(0, s.Length / 2));
		yield return long.Parse(s.Substring(s.Length / 2));
	} else {
		yield return stone * 2024;
	}
}

var stones = Console.ReadLine()!.Split().Select(long.Parse).GroupBy(x => x)
	.Select(g => (val: g.Key, count: (long)g.Count()));

for (int i = 0; i < 75; ++i) {
	stones = (
		from s in stones
		from nval in Blink(s.val)
		group s.count by nval into g
		select (g.Key, g.Sum())
	);
	if (i == 24 || i == 74)
		Console.WriteLine(stones.Sum(s => s.count));
}
