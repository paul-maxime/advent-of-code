// Advent of Code 2018
// Day 05

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8').trim();

function reducePolymer(polymer) {
    let hasChanged = true;
    while (hasChanged) {
        hasChanged = false;
        for (let i = 0; i < polymer.length - 1; ++i) {
            if (polymer[i] !== polymer[i + 1] && polymer[i].toUpperCase() === polymer[i + 1].toUpperCase()) {
                polymer = polymer.slice(0, i) + polymer.slice(i + 2);
                hasChanged = true;
                break;
            }
        }
    }
    return polymer;
}

let polymer = reducePolymer(content);
console.log(polymer.length);

const alphabet = "abcdefghijklmnopqrstuvwxzy";
let bestLength = 0;

for (const c of alphabet) {
    polymer = content.replace(new RegExp(c, 'g'), '');
    polymer = polymer.replace(new RegExp(c.toUpperCase(), 'g'), '');
    polymer = reducePolymer(polymer);
    if (!bestLength || polymer.length < bestLength) {
        bestLength = polymer.length;
    }
}

console.log(bestLength);
