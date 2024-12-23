using aoc;

var edges = (
	from line in Aoc.ConsoleLines()
	let p = line.Split('-')
	from i in Enumerable.Range(0, 2)
	select (src: p[i], dst: p[1 - i])
).ToHashSet();

var graph = edges.ToLookup(p => p.src, p => p.dst);

string Serialize(IEnumerable<string> s) => string.Join(',', s.Order());

Console.WriteLine((
	from succs in graph
	let v1 = succs.Key
	where v1[0] == 't'
	from v2 in succs
	from v3 in succs
	where edges.Contains((v2, v3))
	select Serialize([ v1, v2, v3 ])
).Distinct().Count());

var vertices = graph.Select(s => s.Key).ToArray();
List<string>? clique = null;

int MaxClique(List<string> current, int index, int best) {
	if (current.Count + vertices.Length - index < best)
		return best;

	if (index == vertices.Length) {
		clique = new(current);
		return current.Count;
	}

	var next = vertices[index++];
	if (current.All(v => edges.Contains((v, next)))) {
		current.Add(next);
		best = MaxClique(current, index, best);
		current.RemoveAt(current.Count - 1);
	}

	return MaxClique(current, index, best);
}

MaxClique(new(), 0, 0);
Console.WriteLine(Serialize(clique!));
