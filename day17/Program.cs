using aoc;
using System.Text.RegularExpressions;

Regex numRe = new(@"\d+");

var reg = Aoc.ConsoleLines().TakeWhile(line => line != "")
	.Select(line => int.Parse(numRe.Match(line).Value)).ToArray();
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
	for (long i = 0; i < prog.Length; i += 2) {
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
	n => reg[A] = reg[A] >> Combo(n),
	// bxl
	n => reg[B] = reg[B] ^ n,
	// bst
	n => reg[B] = Combo(n) % 8,
	// jnz
	n => ip = reg[A] == 0 ? ip : n,
	// bxc
	_ => reg[B] = reg[B] ^ reg[C],
	// out
	n => output.Add(Combo(n) % 8),
	// bdv
	n => reg[B] = reg[A] >> Combo(n),
	// cdv
	n => reg[C] = reg[A] >> Combo(n),
];

while (ip < prog.Length) {
	var (cmd, n) = (prog[ip++], prog[ip++]);
	instr[cmd](n);
}

Console.WriteLine(string.Join(',', output));

bool Bit(long value, int i) => (value & (1 << i)) != 0;

bool Fix(bool?[] bits, int index, long value) {
	for (int i = 0; i < 3; ++i) {
		var fix = Bit(value, i);
		var bit = index + i < bits.Length ? bits[index + i] : false;
		if (bit is {} b && b != fix)
			return false;
		if (bit is null)
			bits[index + i] = fix;
	}
	return true;
}

long? Solve(int index = 0, bool?[]? bits = null) {
	bits ??= new bool?[prog.Length * 3];

	if (index == prog.Length * 3) {
		return bits.Select((b, i) => b!.Value ? 1L << i : 0L).Sum();
	}

	for (int b = 0; b < 8; ++b) {
		var newBits = bits.Clone_();
		// magic numbers from decompiled program
		var farIndex = index + (b ^ 5);
		var farValue = b ^ 3 ^ prog[index / 3];
		if (Fix(newBits, index, b) && Fix(newBits, farIndex, farValue)
				&& Solve(index + 3, newBits) is {} r)
			return r;
	}
	return null;
}

Console.WriteLine(Solve());
