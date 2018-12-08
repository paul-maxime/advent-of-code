// Advent of Code 2018
// Day 08, Part 1

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const numbers = content.split(' ').filter(x => x.length > 0).map(x => parseInt(x));

let total = 0;
let i = 0;

function parseNode() {
    let subNodes = numbers[i++];
    let metadataEntries = numbers[i++];
    for (let j = 0; j < subNodes; ++j) {
        parseNode();
    }
    for (let j = 0; j < metadataEntries; ++j) {
        total += numbers[i++];
    }
}

parseNode();

console.log(total);
