IEnumerable<string> ConsoleLines() {
	string? line;
	while ((line = Console.ReadLine()) != null) {
		yield return line;
	}
}

var lists = (
	from n_i in ConsoleLines().SelectMany(
		line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			.Select((n, i) => (int.Parse(n), i))
	)
	group n_i by n_i.Item2 into g
	orderby g.Key
	select (
		from n_i in g
		orderby n_i.Item1
		select n_i.Item1
	).ToArray()
).ToArray();

// Part 1
Console.WriteLine(lists[0].Zip(lists[1], (a, b) => int.Abs(a - b)).Sum());

// Part 2
var freq = (
	from n in lists[1]
	group n by n into g
	select (g.Key, g.Count())
).ToDictionary();

Console.WriteLine((
	from n in lists[0]
	select n * freq.GetValueOrDefault(n, 0)
).Sum());
