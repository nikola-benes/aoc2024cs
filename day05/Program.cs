IEnumerable<string> ConsoleLines() {
	while (Console.ReadLine() is {} line) {
		yield return line;
	}
}

List<string> Postorder(ILookup<string, string> rules, string[] pages) {
	var postorder = new List<string>();
	var unseen = pages.ToHashSet();

	void Dfs(string page) {
		if (!unseen.Remove(page)) {
			return;  // wasn't there
		}
		foreach (var target in rules[page]) {
			Dfs(target);
		}
		postorder.Add(page);
	}

	foreach (var page in pages) {
		Dfs(page);
	}

	return postorder;
}

var rules = ConsoleLines().TakeWhile(line => line != "")
	.Select(line =>	line.Split('|')
		switch { var a => (src: a[0], dst: a[1]) })
	.ToArray();

var (good, bad) = ConsoleLines().Select(line => line.Split(','))
	.ToLookup(pages => {
		var order = pages.Select((p, i) => (p, i)).ToDictionary();
		return rules.All(
			r => !order.ContainsKey(r.src)
			  || !order.ContainsKey(r.dst)
			  || order[r.src] < order[r.dst]);
	}) switch { var g => (g[true], g[false]) };

var GetMid = (IList<string> p) => int.Parse(p[p.Count / 2]);

Console.WriteLine(good.Select(GetMid).Sum());

var rulesLookup = rules.ToLookup(r => r.src, r => r.dst);
Console.WriteLine(bad.Select(p => GetMid(Postorder(rulesLookup, p))).Sum());
