using Block = (int offset, int len);

var pos = 0;
var blocks = Console.ReadLine()!
	.Select((d, i) => {
		int len = d - '0';
		int offset = pos;
		pos += len;
		return (offset, len);
	}).ToArray();

void AddToSum(ref long sum, int pos, int id, int len) {
	for (int i = pos; i < pos + len; ++i) {
		sum += id * i;
	}
}

long Solve1(Block[] blocks) {  // non-destructive
	long sum = 0;
	int lastFile = blocks.Length - 2 + blocks.Length % 2;
	int lastSize = blocks[lastFile].len;
	int pos = 0;

	for (int i = 0; i < lastFile; ++i) {
		if (i % 2 == 0) {
			AddToSum(ref sum, pos, i / 2, blocks[i].len);
			pos += blocks[i].len;
			continue;
		}

		int size = blocks[i].len;
		for (int j = 0; j < size && i < lastFile; ++j) {
			sum += lastFile / 2 * pos++;
			--lastSize;
			if (lastSize == 0) {
				lastFile -= 2;
				lastSize = blocks[lastFile].len;
			}
		}
	}

	if (lastSize < blocks[lastFile].len)
		AddToSum(ref sum, pos, lastFile / 2, lastSize);

	return sum;
}

long Solve2(Block[] blocks) {  // destructive
	long sum = 0;
	int lastFile = blocks.Length - 2 + blocks.Length % 2;

	List<(int id, int offset, int size)> newBlocks = new();

	for (; lastFile > 0; lastFile -= 2) {
		int size = blocks[lastFile].len;
		int freeBlock = 1;

		while (freeBlock < lastFile && blocks[freeBlock].len < size)
			freeBlock += 2;

		if (freeBlock >= lastFile)
			continue;

		newBlocks.Add((lastFile / 2, blocks[freeBlock].offset, size));
		blocks[lastFile].len = 0;
		blocks[freeBlock].len -= size;
		blocks[freeBlock].offset += size;
	}

	for (int i = 0; i < blocks.Length; i += 2) {
		AddToSum(ref sum, blocks[i].offset, i / 2, blocks[i].len);
	}

	foreach (var b in newBlocks) {
		var (id, offset, len) = b;
		AddToSum(ref sum, offset, id, len);
	}

	return sum;
}

Console.WriteLine(Solve1(blocks));
Console.WriteLine(Solve2(blocks));
