using aoc;
using SalesDict
	= System.Collections.Generic.Dictionary<(int, int, int, int), int>;

long Step(long s) {
	long mod = 16777216;
	s = (s ^ (s * 64)) % mod;
	s = (s ^ (s / 32)) % mod;
	s = (s ^ (s * 2048)) % mod;
	return s;
}

var secrets = Aoc.ConsoleLines().Select(long.Parse).ToArray();

Console.WriteLine(secrets.Sum(init => init.Iterate(Step).ElementAt(2000)));

SalesDict Sales(long init) {
	var prices = init.Iterate(Step).Take(2000).Select(s => (int)s % 10)
		.ToArray();
	SalesDict sales = new();
	Queue<int> queue = new();

	foreach (var (lp, p) in prices.Zip(prices.Skip(1))) {
		queue.Enqueue(p - lp);
		if (queue.Count == 4) {
			var ds = queue.ToArray();
			sales.TryAdd((ds[0], ds[1], ds[2], ds[3]), p);
			queue.Dequeue();
		}
	}
	return sales;
}

SalesDict total = new();

foreach (var s in secrets) {
	foreach (var (key, sale) in Sales(s)) {
		total[key] = total.GetValueOrDefault(key) + sale;
	}
}

Console.WriteLine(total.Values.Max());
