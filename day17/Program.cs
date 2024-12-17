using aoc;
using System.Text.RegularExpressions;

Regex numRe = new(@"\d+");

var reg = Aoc.ConsoleLines().TakeWhile(line => line != "")
	.Select(line => int.Parse(numRe.Match(line).Value)).ToArray();
var prog = numRe.Matches(Console.ReadLine()!)
	.Select(m => int.Parse(m.Value)).ToArray();

var ComboDesc = (int n) => n < 4 ? (char)('0' + n) : "ABC"[n - 4];

Func<int, string>[] desc = [
	n => "A = A / (1 << " + ComboDesc(n) + ")",
	n => "B = B ^ " + n,
	n => "B = " + ComboDesc(n) + " % 8",
	n => "if (A == 0) jmp " + n,
	_ => "B = B ^ C",
	n => "out(" + ComboDesc(n) + " % 8)",
	n => "B = A / (1 << " + ComboDesc(n) + ")",
	n => "C = A / (1 << " + ComboDesc(n) + ")",
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
	n => reg[A] = reg[A] / (1 << Combo(n)),
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
	n => reg[B] = reg[A] / (1 << Combo(n)),
	// cdv
	n => reg[C] = reg[A] / (1 << Combo(n)),
];

while (ip < prog.Length) {
	var (cmd, n) = (prog[ip], prog[ip + 1]);
	ip += 2;
	instr[cmd](n);
}

Console.WriteLine(string.Join(',', output));
