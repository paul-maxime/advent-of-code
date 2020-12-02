// Advent of Code 2018
// Day 11, Part 1

const SERIAL_NUMBER = 1955;

function powerLevel(x, y) {
    const rackId = x + 10;
    let powerLevel = rackId * y;
    powerLevel += SERIAL_NUMBER;
    powerLevel *= rackId;
    powerLevel = Math.floor((powerLevel % 1000) / 100);
    powerLevel -= 5;
    return powerLevel;
}

let maxX, maxY;
let max = 0;
for (let x = 0; x < 300 - 2; ++x) {
    for (let y = 0; y < 300 - 2; ++y) {
        const totalPower =
            powerLevel(x, y) + powerLevel(x + 1, y) + powerLevel(x + 2, y) +
            powerLevel(x, y + 1) + powerLevel(x + 1, y + 1) + powerLevel(x + 2, y + 1) +
            powerLevel(x, y + 2) + powerLevel(x + 1, y + 2) + powerLevel(x + 2, y + 2);
        if (totalPower > max) {
            max = totalPower;
            maxX = x;
            maxY = y;
        }
    }
}

console.log(`${maxX},${maxY}`);

