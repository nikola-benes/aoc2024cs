using System.Text.RegularExpressions;

IEnumerable<string> ConsoleLines() {
	string? line;
	while ((line = Console.ReadLine()) != null) {
		yield return line;
	}
}

var mulRe = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
int part1 = 0;
int part2 = 0;
bool enabled = true;

foreach (var m in ConsoleLines().SelectMany(line => mulRe.Matches(line))) {
	if (m.Value == "do()") {
		enabled = true;
	} else if (m.Value == "don't()") {
		enabled = false;
	} else if (m is { Groups: [ _, { Value: var a }, { Value: var b } ] }) {
		var mul = int.Parse(a) * int.Parse(b);
		part1 += mul;
		if (enabled) part2 += mul;
	}
}

Console.WriteLine(part1);
Console.WriteLine(part2);
