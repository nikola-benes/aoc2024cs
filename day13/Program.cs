using aoc;
using System.Text.RegularExpressions;

var machines = Aoc.ConsoleLines().Chunk(4).Select(Machine.Parse).ToArray();

Console.WriteLine(machines.Select(m => m.Solve()).Sum());

long move = 10000000000000;
Console.WriteLine(machines.Select(m =>
	(m with { tx = m.tx + move, ty = m.ty + move }).Solve()).Sum());

record struct Machine(long ax, long ay, long bx, long by, long tx, long ty) {
	static Regex numRe = new(@"\d+");

	public static Machine Parse(IEnumerable<string> desc) {
		var nums = desc.SelectMany(line
			=> numRe.Matches(line).Select(m => long.Parse(m.Value))
		).ToArray();

		return new(nums[0], nums[1],
		           nums[2], nums[3],
		           nums[4], nums[5]);
	}

	public long Solve() {
		long d = ax * by - bx * ay;
		if (d == 0)
			// Assume that the input contains no such cases.
			throw new ArgumentException("More than one solution.");

		long da = tx * by - bx * ty;
		long db = ax * ty - tx * ay;
		return Math.DivRem(da, d) is (var a, 0) &&
		       Math.DivRem(db, d) is (var b, 0) &&
		       a >= 0 && b >= 0 ? 3 * a + b : 0;
	}
};
