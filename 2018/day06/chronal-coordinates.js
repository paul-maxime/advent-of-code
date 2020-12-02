// Advent of Code 2018
// Day 06

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(x => x !== '');

const hash = (x, y) => x * 1000 + y;
const distance = (a, b) => Math.abs(a.x - b.x) + Math.abs(a.y - b.y);

const points = lines.map(line => {
    const [_, x, y] = line.match(/^(\d+), (\d+)$/).map(n => parseInt(n));
    return { x, y };
});

let pointCount = new Map();
points.forEach(p => pointCount.set(hash(p.x, p.y), 0));

for (let x = 0; x < 1000; ++x) {
    for (let y = 0; y < 1000; ++y) {
        let bestDistance = -1;
        let bestPoint = null;
        for (const point of points) {
            const currentDistance = distance({ x, y }, point);
            if (bestDistance < 0 || currentDistance <= bestDistance) {
                bestPoint = currentDistance === bestDistance ? null : point;
                bestDistance = currentDistance;
            }
        }
        if (bestPoint) {
            const bestHash = hash(bestPoint.x, bestPoint.y);
            if (x === 0 || y === 0 || x === 999 || y === 999) {
                pointCount.delete(bestHash)
            } else {
                if (pointCount.has(bestHash)) {
                    pointCount.set(bestHash, pointCount.get(bestHash) + 1);
                }
            }
        }
    }    
}

let bestArea = Math.max(...pointCount.values());
console.log(bestArea);

let areaSize = 0;

for (let x = 0; x < 1000; ++x) {
    for (let y = 0; y < 1000; ++y) {
        let totalDistance = 0;
        for (const point of points) {
            const currentDistance = distance({ x, y }, point);
            totalDistance += currentDistance;
        }
        areaSize += totalDistance < 10000 ? 1 : 0;
    }    
}

console.log(areaSize);
