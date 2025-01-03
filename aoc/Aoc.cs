namespace aoc;

public static class Aoc {
	public static
	IEnumerable<string> ConsoleLines() {
		while (Console.ReadLine() is {} line) {
			yield return line;
		}
	}

	// to avoid writing the type when cloning
	public static
	T Clone_<T>(this T obj) where T : ICloneable => (T)obj.Clone();

	public static
	Func<T, TResult> MemoizeRec<T, TResult>(
		Func<T, Func<T, TResult>, TResult> f
	) where T : notnull {
		Dictionary<T, TResult> cache = new();
		Func<T, TResult>? rec = null;
		return rec = x =>
			cache.TryGetValue(x, out var r) ? r :
			cache[x] = f(x, rec!);
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
	public static Vec2[] dirs4 = { (1, 0), (0, 1), (-1, 0), (0, -1) };

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

	public IEnumerable<Vec2> Neighbours4(Vec2 p)
		=> dirs4.Select(d => p + d).Where(InBounds);
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

	public static
	Queue<T> ToQueue<T>(this IEnumerable<T> e) => new Queue<T>(e);

	public static
	IEnumerable<T> Iterate<T>(this T start, Func<T, T> next) {
		while (true) {
			yield return start;
			start = next(start);
		}
	}

	public static
	IEnumerable<IEnumerable<T>>
	SplitBy<T>(this IEnumerable<T> e, Func<T, bool> pred) {
		var it = e.GetEnumerator();
		while (it.MoveNext()) {
			yield return SplitByAux(it, pred);
		}
	}

	private static
	IEnumerable<T> SplitByAux<T>(IEnumerator<T> it, Func<T, bool> pred) {
		while (!pred(it.Current)) {
			yield return it.Current;
			if (!it.MoveNext())
				break;
		}
	}
}
