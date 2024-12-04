IEnumerable<string> ConsoleLines() {
	while (Console.ReadLine() is {} line) {
		yield return line;
	}
}

(int y, int x)[] dirs = {
	(-1, -1), (-1, 0), (-1, 1),
	( 0, -1),          ( 0, 1),
	( 1, -1), ( 1, 0), ( 1, 1),
};

var ws = ConsoleLines().ToArray();
var positions = ws.SelectMany((row, y) => row.Select((_, x) => (y, x)));

var Get = (int y, int x) =>
	y < 0 || y >= ws.Length || x < 0 || x >= ws[y].Length ? '#' : ws[y][x];

var FindXmas = (int y, int x) => dirs.Count(d =>
	"XMAS".Select((c, i) => Get(y + i * d.y, x + i * d.x) == c).All(b => b)
);

var Find_X_Mas = (int y, int x) => Get(y, x) == 'A' &&
	(Get(y - 1, x - 1), Get(y + 1, x + 1)) is ('M', 'S') or ('S', 'M') &&
	(Get(y - 1, x + 1), Get(y + 1, x - 1)) is ('M', 'S') or ('S', 'M');

Console.WriteLine(positions.Sum(p => FindXmas(p.y, p.x)));
Console.WriteLine(positions.Count(p => Find_X_Mas(p.y, p.x)));
