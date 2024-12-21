using aoc;

var dirs = new Dictionary<char, Vec2>{
	['^'] = ( 0, -1),
	['v'] = ( 0,  1),
	['<'] = (-1,  0),
	['>'] = ( 1,  0),
};

var keypad = (new[] { "789", "456", "123", ".0A" }).ToGrid();
var arrows = (new[] { ".^A", "<v>" }).ToGrid();
Vec2 kStart = (2, 3);
Vec2 aStart = (2, 0);

State? nextState(State state, char press, string pass, int which = 0) {
	var pos =
		which == 0 ? state.r1Pos :
		which == 1 ? state.r2Pos :
		             state.r3Pos;

	bool last = which == 2;
	var pad = last ? keypad : arrows;
	if (press != 'A') {
		pos += dirs![press];
		return pad.GetOr(pos, '.') == '.' ? null :
			which == 0 ? state with { r1Pos = pos } :
			which == 1 ? state with { r2Pos = pos } :
			             state with { r3Pos = pos };
	}

	if (last) {
		return pass[state.pIndex] == keypad[pos]
			? state with { pIndex = state.pIndex + 1 }
			: null;
	}

	return nextState(state, arrows[pos], pass, which + 1);
};

int? Solve(string pass) {
	State start = new ( aStart, aStart, kStart, 0);
	Dictionary<State, int> dist = new() { [start] = 0 };
	Queue<State> queue = new();
	queue.Enqueue(start);

	while (queue.TryDequeue(out var state)) {
		var d = dist[state] + 1;
		foreach (var press in "<v>^A") {
			if (nextState(state, press, pass) is {} next
					&& dist.TryAdd(next, d)) {
				if (next.pIndex == pass.Length)
					return d;

				queue.Enqueue(next);
			}
		}
	}
	return null;
}

Console.WriteLine(Aoc.ConsoleLines().Sum(p => int.Parse(p[.. ^1]) * Solve(p)));

record struct State (Vec2 r1Pos, Vec2 r2Pos, Vec2 r3Pos, int pIndex) {};
