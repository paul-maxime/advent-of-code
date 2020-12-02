// Advent of Code 2018
// Day 03, Part 1

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');
const lines = content.split('\n').filter(x => x !== '');
const regexp = /^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$/;

const claims = lines.map(line => line.match(regexp)).map(match => {
    const [_, _id, spaceX, spaceY, width, height] = match;
    return {
        startX: parseInt(spaceX) + 1,
        startY: parseInt(spaceY) + 1,
        width: parseInt(width),
        height: parseInt(height),
    }
})

let totalOverlaps = 0;
for (let x = 0; x < 1000; ++x) {
    for (let y = 0; y < 1000; ++y) {
        let overlaps = 0;
        for (const claim of claims) {
            if (x >= claim.startX && x < claim.startX + claim.width &&
                y >= claim.startY && y < claim.startY + claim.height) {
                overlaps += 1;
                if (overlaps === 2) {
                    totalOverlaps += 1;
                    break;
                }
            }
        }
    }
}

console.log(totalOverlaps);
