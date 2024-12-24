using aoc;
using Gate = (string lhs, string op, string rhs);

var values = Aoc.ConsoleLines().TakeWhile(line => line != "")
	.Select(line => line.Split(": "))
	.ToDictionary(p => p[0], p => p[1] == "1");

var gates = Aoc.ConsoleLines()
	.Select(line => line.Split(' '))
	.ToDictionary(p => p[4], p => (lhs: p[0], op: p[1], rhs: p[2]));

bool Eval(string wire) {
	if (values.TryGetValue(wire, out var v))
		return v;

	var (lhs, op, rhs) = gates[wire];
	var (lval, rval) = (Eval(lhs), Eval(rhs));
	return values[wire] = op switch {
		"AND" => lval & rval,
		"OR"  => lval | rval,
		_     => lval ^ rval,
	};
}

if (args.Length > 0 && args[0] == "dot") {
	Console.WriteLine("digraph {");
	int i = 0;
	foreach (var (res, (lhs, op, rhs)) in gates) {
		Console.WriteLine($"\"{i}\" [label=\"{op}\"]");
		Console.WriteLine($"\"{lhs}\" -> \"{i}\"");
		Console.WriteLine($"\"{rhs}\" -> \"{i}\"");
		Console.WriteLine($"\"{i}\" -> \"{res}\"");
		++i;
	}
	Console.WriteLine("}");
	return;
}

var maxIn = values.Keys.Where(w => w[0] == 'x').Max(w => int.Parse(w[1..]));

Console.WriteLine(gates.Keys
	.Where(w => w[0] == 'z').Order()
	.Select((w, i) => (Eval(w) ? 1L : 0) << i)
	.Sum()
);

if (maxIn < 5) return;  // examples have no part 2

Gate OrderedGate(Gate g) => g.lhs.CompareTo(g.rhs) < 0
	? (g.lhs, g.op, g.rhs)
	: (g.rhs, g.op, g.lhs);

var gateOut = gates.ToDictionary(g => OrderedGate(g.Value), g => g.Key);
string Out(string lhs, string op, string rhs)
	=> gateOut.GetValueOrDefault(OrderedGate((lhs, op, rhs)), "");

var Check = (string lhs, string op, string rhs, string want)
	=> Out(lhs, op, rhs) switch {
		""                       => (err: 2, a: "",  b: ""),
		var res when res != want => (err: 1, a: res, b: want),
		_                        => (err: 0, a: "",  b: ""),
	};

List<string> swapped = new();

void SwapWires(string a, string b) {
	swapped.AddRange([a, b]);
	gateOut[OrderedGate(gates[a])] = b;
	gateOut[OrderedGate(gates[b])] = a;
}

if (Check("x00", "XOR", "y00", "z00") is (1, var a, var b))
	SwapWires(a, b);

string carry = Out("x00", "AND", "y00");

for (int i = 1; i < maxIn; ++i) {
	var (x, y, z) = ($"x{i:00}", $"y{i:00}", $"z{i:00}");

	var xy = Out(x, "XOR", y);
	var r = Check(xy, "XOR", carry, z);
	if (r.err == 1)
		SwapWires(r.a, r.b);

	var a1 = Out(x, "AND", y);
	if (r.err == 2) {
		SwapWires(xy, a1);
		(xy, a1) = (a1, xy);
	}

	var a2 = Out(xy, "AND", carry);
	carry = Out(a1, "OR", a2);
}

Console.WriteLine(string.Join(',', swapped.Order()));
