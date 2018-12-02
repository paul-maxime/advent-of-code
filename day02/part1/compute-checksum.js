// Advent of Code 2018
// Day 02, Part 1

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');
const words = content.split('\n').filter(x => x !== '');

function computeChecksum() {
  const occurences = new Map();
  let nbTwoIdenticalLetters = 0;
  let nbThreeIdenticalLetters = 0;
  for (const word of words) {
    occurences.clear();
    for (const letter of word) {
      if (!occurences.has(letter)) {
        occurences.set(letter, 1);
      } else {
        occurences.set(letter, occurences.get(letter) + 1);
      }
    }
    const hasTwoIdenticalLetters = [...occurences.values()].some(x => x === 2);
    if (hasTwoIdenticalLetters) {
      nbTwoIdenticalLetters += 1;
    }
    const hasThreeIdenticalLetters = [...occurences.values()].some(x => x === 3);
    if (hasThreeIdenticalLetters) {
      nbThreeIdenticalLetters += 1;
    }
  }
  return nbTwoIdenticalLetters * nbThreeIdenticalLetters;
}

console.log(computeChecksum());

