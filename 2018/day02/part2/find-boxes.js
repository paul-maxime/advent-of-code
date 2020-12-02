// Advent of Code 2018
// Day 02, Part 2

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');
const words = content.split('\n').filter(x => x !== '');

function findBoxes() {
  for (const wordA of words) {
    for (const wordB of words) {
      let differences = 0;
      for (let i = 0; i < wordA.length; ++i) {
        if (wordA[i] !== wordB[i]) {
          differences++;
        }
      }
      if (differences === 1) {
        return [wordA, wordB];
      }
    }
  }
  return null;
}

console.log(findBoxes());

