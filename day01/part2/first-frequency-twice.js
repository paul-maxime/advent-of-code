// Advent of Code 2018
// Day 01, Step 02

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');

const numbers = content.split('\n').filter(x => x !== '').map(x => parseInt(x));
const alreadyUsed = new Set();

function getFirstFrequencyUsedTwice() {
  var currentFrequency = 0;

  while (true) {
  for (var number of numbers) {
      currentFrequency += number;
      if (alreadyUsed.has(currentFrequency)) {
        return currentFrequency;
      }
      alreadyUsed.add(currentFrequency);
    }
  }
}

console.log(getFirstFrequencyUsedTwice());

