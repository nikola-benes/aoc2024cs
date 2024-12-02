IEnumerable<string> ConsoleLines() {
	string? line;
	while ((line = Console.ReadLine()) != null) {
		yield return line;
	}
}

bool Safe(IEnumerable<int> report) {
	var diffs = report.Zip(report.Skip(1), (a, b) => b - a);
	return
		diffs.All(x => -3 <= x && x <= -1) ||
		diffs.All(x =>  1 <= x && x <=  3);
}

bool SafeWithDampener(int[] report) {
	return Enumerable.Range(0, report.Length + 1)
		.Any(i => Safe(report.Without(i)));
}

var reports = (
	from line in ConsoleLines()
	select line.Split(' ').Select(int.Parse).ToArray()
).ToArray();

Console.WriteLine(reports.Count(Safe));
Console.WriteLine(reports.Count(SafeWithDampener));

public static class Extensions {
	public static
	IEnumerable<T> Without<T>(this IEnumerable<T> e, int index) {
		int i = 0;
		foreach (var x in e) {
			if (i++ != index) {
				yield return x;
			}
		}
	}
}
