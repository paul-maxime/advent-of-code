// Advent of Code 2018
// Day 07, Part 1

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(x => x.length > 0);

const dependencies = lines.map(line => {
    const [ _, dependency, part ] = line.match(/Step ([A-Z]) .+ step ([A-Z])/);
    return { dependency, part };
});

const remainingParts = new Map();

dependencies.forEach(x => {
    if (!remainingParts.has(x.part)) {
        remainingParts.set(x.part, new Set());
    }
    if (!remainingParts.has(x.dependency)) {
        remainingParts.set(x.dependency, new Set());
    }
    const partDeps = remainingParts.get(x.part);
    partDeps.add(x.dependency);
});

let order = '';
while (remainingParts.size > 0) {
    const nextPart = Array.from(remainingParts.keys()).filter(x => remainingParts.get(x).size === 0).sort()[0];
    order += nextPart;
    remainingParts.forEach(deps => deps.delete(nextPart));
    remainingParts.delete(nextPart);
}

console.log(order);
