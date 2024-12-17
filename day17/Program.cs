using aoc;
using System.Text.RegularExpressions;

Regex numRe = new(@"\d+");

var reg = Aoc.ConsoleLines().TakeWhile(line => line != "")
	.Select(line => long.Parse(numRe.Match(line).Value)).ToArray();
var prog = numRe.Matches(Console.ReadLine()!)
	.Select(m => int.Parse(m.Value)).ToArray();

var ComboDesc = (int n) => n < 4 ? (char)('0' + n) : "ABC"[n - 4];

Func<int, string>[] desc = [
	n => "A = A >> " + ComboDesc(n),
	n => "B = B ^ " + n,
	n => "B = " + ComboDesc(n) + " % 8",
	n => "if (A == 0) jmp " + n,
	_ => "B = B ^ C",
	n => "out(" + ComboDesc(n) + " % 8)",
	n => "B = A >> " + ComboDesc(n),
	n => "C = A >> " + ComboDesc(n),
];

if (args.Length > 0 && args[0] == "decompile") {
	for (int i = 0; i < prog.Length; i += 2) {
		var (cmd, n) = (prog[i], prog[i + 1]);
		Console.WriteLine(desc[cmd](n));
	}
	return;
}

const int A = 0;
const int B = 1;
const int C = 2;
var Combo = (int n) => n < 4 ? n : reg[n - 4];
var ip = 0;
List<int> output = new();

Action<int>[] instr = [
	// adv
	n => reg[A] = reg[A] >> (int)Combo(n),
	// bxl
	n => reg[B] = reg[B] ^ n,
	// bst
	n => reg[B] = Combo(n) % 8,
	// jnz
	n => ip = reg[A] == 0 ? ip : n,
	// bxc
	_ => reg[B] = reg[B] ^ reg[C],
	// out
	n => output.Add((int)(Combo(n) % 8)),
	// bdv
	n => reg[B] = reg[A] >> (int)Combo(n),
	// cdv
	n => reg[C] = reg[A] >> (int)Combo(n),
];

while (ip < prog.Length) {
	var (cmd, n) = (prog[ip++], prog[ip++]);
	instr[cmd](n);
}

Console.WriteLine(string.Join(',', output));

int FirstOutput(long start) {
	reg[A] = start;
	ip = 0;
	while (true) {
		// need to use ! to silence the null reference warnings
		// the compiler emits here for some reason
		var (cmd, n) = (prog![ip++], prog![ip++]);
		if (cmd == 5)
			return (int)(Combo!(n) % 8);
		instr[cmd](n);
	}
}

long? Solve(int? ix = null, long start = 0) {
	var index = ix ?? prog.Length;
	if (index-- == 0)
		return start;

	for (int d = 0; d < 8; ++d) {
		var a = start * 8 + d;
		if (FirstOutput(a) == prog[index] && Solve(index, a) is {} r)
			return r;
	}
	return null;
}

Console.WriteLine(Solve());
