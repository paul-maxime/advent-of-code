// Advent of Code 2018
// Day 03, Part 2

const fs = require('fs');
const content = fs.readFileSync('../input', 'utf8');
const lines = content.split('\n').filter(x => x !== '');
const regexp = /^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$/;

const claims = lines.map(line => line.match(regexp)).map(match => {
    const [_, id, spaceX, spaceY, width, height] = match;
    const startX = parseInt(spaceX) + 1;
    const startY = parseInt(spaceY) + 1;
    const endX = startX + parseInt(width);
    const endY = startY + parseInt(height);
    return { id, startX, startY, endX, endY }
})

const overlap = (claimA, claimB) => !(
    claimA.startX > claimB.endX ||
    claimB.startX > claimA.endX ||
    claimA.startY > claimB.endY ||
    claimB.startY > claimA.endY
);

for (const claimA of claims) {
    let doesOverlap = false;
    for (const claimB of claims) {
        if (claimA.id !== claimB.id && overlap(claimA, claimB)) {
            doesOverlap = true;
            break;
        }
    }
    if (!doesOverlap) {
        console.log(claimA.id);
    }
}
