using aoc;
using Box = (aoc.Vec2 left, aoc.Vec2 right);

var dirs = new Dictionary<char, Vec2>{
	['^'] = ( 0, -1),
	['v'] = ( 0,  1),
	['<'] = (-1,  0),
	['>'] = ( 1,  0),
};

var map = Aoc.ConsoleLines().TakeWhile(line => line != "").ToGrid();
var moves = string.Join("", Aoc.ConsoleLines());
var (_, start) = map.Tiles.First(t => t.c == '@');

// --- part 1 ---

var robot = start;
var boxes = map.Tiles.Where(t => t.c == 'O').Select(t => t.pos).ToHashSet();

foreach (var m in moves) {
	var dir = dirs[m];
	var pos = robot;
	do {
		pos += dir;
	} while (boxes.Contains(pos));

	if (map[pos] == '#')
		continue;

	robot += dir;
	boxes.Remove(robot);
	if (pos != robot)
		boxes.Add(pos);
}

Console.WriteLine(boxes.Sum(p => 100 * p.y + p.x));

// --- part 2 ---

robot = start;
robot.x *= 2;

Dictionary<Vec2, Box> bigBoxes = new();
var AddBox = (Vec2 left, Vec2 right)
	=> bigBoxes[left] = bigBoxes[right] = (left, right);

foreach (var t in map.Tiles.Where(t => t.c == 'O')) {
	var (x, y) = t.pos;
	AddBox((x * 2, y), (x * 2 + 1, y));
}

bool CanMoveTo(Vec2 src, Vec2 dir, HashSet<Box> toMove) {
	var pos = src + dir;

	if (map[(pos.x / 2, pos.y)] == '#')
		return false;

	if (!bigBoxes.TryGetValue(pos, out var box) || toMove.Contains(box))
		return true;

	Vec2[] next = dir.y == 0
		? new[] { dir.x == 1 ? box.right : box.left }
		: new[] { box.left, box.right };

	if (!next.All(p => CanMoveTo(p, dir, toMove)))
		return false;

	toMove.Add(box);
	return true;
}

foreach (var m in moves) {
	var dir = dirs[m];
	HashSet<Box> toMove = new();
	if (!CanMoveTo(robot, dir, toMove))
		continue;

	foreach (var box in toMove) {
		bigBoxes.Remove(box.left);
		bigBoxes.Remove(box.right);
	}
	foreach (var box in toMove) {
		AddBox(box.left + dir, box.right + dir);
	}
	robot += dir;
}

Console.WriteLine(bigBoxes.Where(kv => kv.Key == kv.Value.left)
		.Sum(kv => kv.Key.x + 100 * kv.Key.y));
