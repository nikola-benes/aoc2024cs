using aoc;
using Block = (int id, int offset, int len);

var pos = 0;
var (files, free) = Console.ReadLine()!
	.Select((d, i) => {
		int len = d - '0';
		int offset = pos;
		pos += len;
		return (id: i % 2 == 0 ? i / 2 : -1, offset, len);
	}).ToLookup(b => b.id >= 0)
		switch { var lk => (lk[true], lk[false]) };

long CheckSum(Block b)
	=> (long)b.id * (b.offset * b.len + b.len * (b.len - 1) / 2);

IEnumerable<Block> Defrag1(IEnumerable<Block> files, IEnumerable<Block> free) {
	var e = files.Reverse().GetEnumerator();
	Func<Block?> NextFile = () => e.MoveNext() ? e.Current : null;

	var last = NextFile()!.Value;

	foreach (var block in free) {
		var (_, offset, len) = block;
		while (len > 0 && last.offset > offset) {
			int chunk = Math.Min(len, last.len);
			yield return (last.id, offset, chunk);

			offset += chunk;
			len -= chunk;
			last.len -= chunk;
			if (last.len == 0)
				last = NextFile()!.Value;
		}
		if (last.offset <= offset) break;
	}

	yield return last;

	while (NextFile() is {} file)
		yield return file;
}

IEnumerable<Block> Defrag2(IEnumerable<Block> files, IEnumerable<Block> free) {
	var freeList = new LinkedList<Block>(free);

	foreach (var file in files.Reverse()) {
		while (freeList.Last is { Value: var b }
				&& b.offset > file.offset) {
			freeList.RemoveLast();
		}
		if (freeList.FindIf(b => file.len <= b.len)
				is not { Value: var block } node) {
			yield return file;
			continue;
		}

		yield return (file.id, block.offset, file.len);

		block.len -= file.len;
		block.offset += file.len;
		if (block.len == 0) {
			freeList.Remove(node);
		} else {
			node.ValueRef = block;
		}
	}
}

Console.WriteLine(Defrag1(files, free).Sum(CheckSum));
Console.WriteLine(Defrag2(files, free).Sum(CheckSum));
