namespace aoc;

public static class Aoc {
	public static
	IEnumerable<string> ConsoleLines() {
		while (Console.ReadLine() is {} line) {
			yield return line;
		}
	}
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
