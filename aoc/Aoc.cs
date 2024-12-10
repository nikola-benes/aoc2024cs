namespace aoc;

public static class Aoc {
	public static
	IEnumerable<string> ConsoleLines() {
		while (Console.ReadLine() is {} line) {
			yield return line;
		}
	}
}

public record struct Vec2(int x, int y) {
	public static implicit operator Vec2((int x, int y) p)
		=> new Vec2(p.x, p.y);

	public static Vec2 operator +(Vec2 a, Vec2 b)
		=> new Vec2(a.x + b.x, a.y + b.y);

	public static Vec2 operator -(Vec2 a, Vec2 b)
		=> new Vec2(a.x - b.x, a.y - b.y);

	public static Vec2 operator *(int k, Vec2 v)
		=> new Vec2(k * v.x, k * v.y);

	public static Vec2 operator *(Vec2 v, int k)
		=> new Vec2(v.x * k, v.y * k);
}

public struct Grid {
	string[] grid;

	public Grid(IEnumerable<string> rows) { grid = rows.ToArray(); }

	public int Height { get => grid.Length; }
	public int Width { get => grid[0].Length; }

	public IEnumerable<(char c, Vec2 pos)> Tiles { get =>
		grid.SelectMany((row, y)
			=> row.Select((c, x) => (c, new Vec2(x, y)))); }

	public bool InBounds(Vec2 p)
		=> 0 <= p.y && p.y < Height
		&& 0 <= p.x && p.x < Width;

	public char this[Vec2 p] => grid[p.y][p.x];

	public char GetOr(Vec2 p, char d) => InBounds(p) ? this[p] : d;
}

public static class LinkedListExtensions {
	public static
	LinkedListNode<T>?
	FindIf<T>(this LinkedList<T> llist, Func<T, bool> pred) {
		var ptr = llist.First;
		while (ptr is {} node && !pred(node.Value))
			ptr = node.Next;
		return ptr;
	}
}

public static class EnumerableExtensions {
	public static
	Grid ToGrid(this IEnumerable<string> rows) => new Grid(rows);
}
