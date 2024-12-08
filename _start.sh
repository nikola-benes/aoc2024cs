#!/bin/sh

Y=2024
C=  # Insert session cookie here
A="nikola.benes@gmail.com via curl"

if [ "$C" = "" ]; then
	echo "Missing session cookie."
	exit
fi

DAY="$1"
DIR="day"$(printf '%02d' "$1")

if [ "$DAY" = "" ]; then
	echo "Usage: $0 <day_number>"
	exit
fi

dotnet new console -o "$DIR"
dotnet add "$DIR" reference aoc

cat <<END > "$DIR/Program.cs"
using aoc;

var lines = Aoc.ConsoleLines();
END

curl "https://adventofcode.com/$Y/day/$DAY/input" \
	--cookie "session=$C" --user-agent "$A" > "$DIR/input"
