// Advent of Code 2018
// Day 01, Step 02

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');

const numbers = content.split('\n').filter(x => x !== '').map(x => parseInt(x));
const sum = numbers.reduce((a, b) => a + b, 0);

console.log(sum);
