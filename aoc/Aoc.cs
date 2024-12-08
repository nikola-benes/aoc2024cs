namespace aoc;

public static class Aoc {
	public static
	IEnumerable<string> ConsoleLines() {
		while (Console.ReadLine() is {} line) {
			yield return line;
		}
	}
}
