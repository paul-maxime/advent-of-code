// Advent of Code 2018
// Day 10, Part 1

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(x => x.length > 0);

const stars = lines.map(line => {
    const [_, x, y, vx, vy] = line.match(/^position=<(.+), (.+)> velocity=<(.+), (.+)>$/);
    return { x: parseInt(x), y: parseInt(y), vx: parseInt(vx), vy: parseInt(vy) };
});

let deltaY = 0;
let seconds = 0;
while (true) {
    let minY = null;
    let maxY = null;
    for (const star of stars) {
        star.x += star.vx;
        star.y += star.vy;
        if (minY === null || star.y < minY) {
            minY = star.y;
        }
        if (maxY === null || star.y > maxY) {
            maxY = star.y;
        }
    }
    seconds += 1;
    if (Math.abs(minY - maxY) < 10) {
        deltaY = minY;
        break;
    }
}

for (let y = 0; y < 10; ++y) {
    let str = "";
    for (let x = 0; x < 200; ++x) {
        if (stars.some(star => star.x === x && star.y === deltaY + y)) {
            str += "@";
        } else {
            str += " ";
        }
    }
    console.log(str);
}

console.log(seconds);
