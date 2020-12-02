// Advent of Code 2018
// Day 25

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(line => line.length > 0);

const stars = lines.map((line, id) => {
    const [_, w, x, y, z] = line.match(/^(-?\d+),(-?\d+),(-?\d+),(-?\d+)$/);
    return { id, w: parseInt(w), x: parseInt(x), y: parseInt(y), z: parseInt(z) };
});

const manhattanDistance = (a, b) => Math.abs(a.w - b.w) + Math.abs(a.x - b.x) + Math.abs(a.y - b.y) + Math.abs(a.z - b.z);

for (const starA of stars) {
    starA.links = [];
    for (const starB of stars) {
        if (starA !== starB && manhattanDistance(starA, starB) <= 3) {
            starA.links.push(starB);
        }
    }
}

function computeConstellation(star) {
    let open = [ star ];
    let closed = new Set();
    closed.add(star.id);
    while (open.length > 0) {
        const currentStar = open.pop();
        currentStar.links.forEach(linkedStar => {
            if (!closed.has(linkedStar.id)) {
                open.push(linkedStar);
                closed.add(linkedStar.id);
            }
        });
    }
    return closed;
}

let remainingStars = stars.slice(0);
let nbConstellations = 0;
while (remainingStars.length > 0) {
    let constellation = computeConstellation(remainingStars[0]);
    remainingStars = remainingStars.filter(x => !constellation.has(x.id));
    nbConstellations++;
}

console.log(nbConstellations);
