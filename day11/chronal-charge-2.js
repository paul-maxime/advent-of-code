// Advent of Code 2018
// Day 11, Part 2

const SERIAL_NUMBER = 1955;

const fuelCells = new Array(300 * 300);

function powerLevel(x, y) {
    const rackId = x + 10;
    let powerLevel = rackId * y;
    powerLevel += SERIAL_NUMBER;
    powerLevel *= rackId;
    powerLevel = Math.floor((powerLevel % 1000) / 100);
    powerLevel -= 5;
    return powerLevel;
}

for (let x = 0; x < 300; ++x) {
    for (let y = 0; y < 300; ++y) {
        fuelCells[x * 300 + y] = powerLevel(x, y);
    }
}

function squarePowerLevel(startX, startY, size, oldPowerLevel) {
    let totalPower = 0;
    for (let i = 0; i < size; ++i) {
        totalPower += fuelCells[(startX + size - 1) * 300 + (startY + i)];
        totalPower += fuelCells[(startX + i) * 300 + (startY + size - 1)];
    }
    return totalPower + oldPowerLevel;
}

let maxX, maxY, bestSize;
let max = 0;
for (let x = 0; x < 300; ++x) {
    for (let y = 0; y < 300; ++y) {
        let totalPower = 0;
        for (let size = 1; size < (300 - x) && size < (300 - y); ++size) {
            totalPower = squarePowerLevel(x, y, size, totalPower);
            if (totalPower > max) {
                max = totalPower;
                maxX = x;
                maxY = y;
                bestSize = size;
            }
        }
    }
}

console.log(`${maxX},${maxY},${bestSize}`);
