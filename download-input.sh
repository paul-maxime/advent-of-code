#!/bin/bash
set -e

cd "$(dirname "$0")"

year=$1
day=${2/day/}
day=${day#0}

if [ -z "$year" ] || [ -z "$day" ] || [ -n "$3" ]; then
  echo "Usage: $0 <year> <day>" 1>&2
  exit 2
fi

if [ -z "$ADVENT_OF_CODE_SESSION" ]; then
  echo "Missing env variable: ADVENT_OF_CODE_SESSION" 1>&2
  exit 2
fi

work_dir=$(printf "%d/day%02d" $year $day)
input_dir=inputs/"$work_dir"
shortcut_file="$work_dir"/input
input_file="$input_dir"/input

if [ -f "$shortcut_file" ]; then
  echo "$shortcut_file already exists" 1>&2
  exit 1
fi

echo "Downloading $shortcut_file..."

mkdir -vp "$work_dir"
mkdir -vp "$input_dir"

curl --fail --cookie session="$ADVENT_OF_CODE_SESSION" https://adventofcode.com/$year/day/$day/input -o "$input_file"
ln -vs "../../$input_file" "$shortcut_file"
(cd inputs && git add "$shortcut_file" && git commit -m "Add $work_dir")
