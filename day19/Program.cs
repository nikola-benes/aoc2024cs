using aoc;

var towels = Console.ReadLine()!.Split(", ");
Console.ReadLine();
var patterns = Aoc.ConsoleLines().ToArray();

bool Possible(string pat)
	=> pat.Length == 0
	|| towels.Any(t => pat.StartsWith(t) && Possible(pat[t.Length ..]));

Dictionary<string, long> cache = new();

long HowMany(string pat)
	=> cache.TryGetValue(pat, out var c) ? c : cache[pat] =
	   pat.Length == 0 ? 1 :
	   towels.Where(pat.StartsWith).Sum(t => HowMany(pat[t.Length ..]));

Console.WriteLine(patterns.Count(Possible));
Console.WriteLine(patterns.Sum(HowMany));
