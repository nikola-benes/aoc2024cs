IEnumerable<string> ConsoleLines() {
	while (Console.ReadLine() is {} line) {
		yield return line;
	}
}

bool HasSolution(long goal, long[] values, bool concat) {
	bool Rec(long goal, int i) {
		long current = values[i];
		if (i == 0)
			return current == goal;

		if (goal % current == 0 && Rec(goal / current, i - 1) ||
		    goal > current      && Rec(goal - current, i - 1))
			return true;

		if (!concat)
			return false;

		long pow = 1;
		long rem = current;
		while (rem > 0) {
			rem /= 10;
			pow *= 10;
		}
		return goal % pow == current && Rec(goal / pow, i - 1);
	}

	return Rec(goal, values.Length - 1);
}

var input = ConsoleLines().Select(line => {
	var lr = line.Split(": ");
	return (goal: long.Parse(lr[0]),
	        values: lr[1].Split(' ').Select(long.Parse).ToArray());
}).ToArray();

var Solve = (bool c) =>
	input.Sum(eq => HasSolution(eq.goal, eq.values, c) ? eq.goal : 0);

Console.WriteLine(Solve(false));
Console.WriteLine(Solve(true));
