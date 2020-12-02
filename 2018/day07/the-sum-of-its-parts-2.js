// Advent of Code 2018
// Day 07, Part 2

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
        remainingParts.set(x.part, { deps: new Set() });
    }
    if (!remainingParts.has(x.dependency)) {
        remainingParts.set(x.dependency, { deps: new Set() });
    }
    const partDeps = remainingParts.get(x.part).deps;
    partDeps.add(x.dependency);
});

remainingParts.forEach((part, c) => {
    part.work = 60 + c.charCodeAt(0) - 65 + 1;
    part.started = false;
});

let elapsedTime = 0;
while (remainingParts.size > 0) {
    const nextParts = Array.from(remainingParts.keys())
        .filter(x => remainingParts.get(x).deps.size === 0)
        .sort((a, b) => {
            const startedA = remainingParts.get(a).started;
            const startedB = remainingParts.get(b).started;
            return (startedA && !startedB) ? -1 : (startedB && !startedA) ? 1 : a.charCodeAt(0) - b.charCodeAt(0);
        }).slice(0, 5);
    for (const nextPart of nextParts) {
        remainingParts.get(nextPart).started = true;
        remainingParts.get(nextPart).work -= 1;
        if (remainingParts.get(nextPart).work === 0) {
            remainingParts.forEach(p => p.deps.delete(nextPart));
            remainingParts.delete(nextPart);
        }
    }
    elapsedTime += 1;
}

console.log(elapsedTime);
