// Advent of Code 2018
// Day 17

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(line => line.length > 0);
const regexp = /^([xy])=(\d+), ([xy])=(\d+)..(\d+)$/

const width = 1000;
let startHeight = 0;
let height = 0;
const clayField = new Set();
const stableWater = new Set();
const unstableWater = new Set();

lines.forEach(line => {
    const [_, leftAxis, leftValueStr, rightAxis, rightStartStr, rightEndStr] = line.match(regexp);
    const [ leftValue, rightStart, rightEnd ] = [leftValueStr, rightStartStr, rightEndStr].map(x => parseInt(x));
    if (leftAxis === 'x' && rightAxis === 'y') {
        for (let y = rightStart; y <= rightEnd; ++y) {
            clayField.add(y * width + leftValue);
        }
        if (startHeight == 0 || rightStart < startHeight) {
            startHeight = rightStart;
        }
        if (rightEnd > height) {
            height = rightEnd;
        }
    } else if (leftAxis === 'y' && rightAxis === 'x') {
        for (let x = rightStart; x <= rightEnd; ++x) {
            clayField.add(leftValue * width + x);
        }
        if (startHeight == 0 || leftValue < startHeight) {
            startHeight = leftValue;
        }
        if (leftValue > height) {
            height = leftValue;
        }
    }
});

const hashCell = (x, y) => y * width + x;

const isClay = (x, y) => clayField.has(hashCell(x, y));
const isWater = (x, y) => stableWater.has(hashCell(x, y));
const isEmpty = (x, y) => !isClay(x, y) && !isWater(x, y);

function propagateWater(x, y, unstable) {
    while (isEmpty(x, y + 1)) {
        if (unstable && y >= startHeight) {
            unstableWater.add(hashCell(x, y));
        }
        ++y;
        if (y > height) {
            return false;
        }
    }
    if (!isEmpty(x, y + 1)) {
        let isLeftStable = false;
        let startX = 0;
        let dx = 0;
        while (!isEmpty(x + dx, y + 1)) {
            if (unstable) {
                unstableWater.add(hashCell(x + dx, y));
            }
            startX = x + dx;
            if (!isEmpty(x + dx - 1, y)) {
                isLeftStable = true;
                break;
            }
            dx--;
        }
        let isRightStable = false;
        let endX = 0;
        dx = 0;
        while (!isEmpty(x + dx, y + 1)) {
            if (unstable) {
                unstableWater.add(hashCell(x + dx, y));
            }
            endX = x + dx;
            if (!isEmpty(x + dx + 1, y)) {
                isRightStable = true;
                break;
            }
            dx++;
        }
        if (isLeftStable && isRightStable) {
            for (let nx = startX; nx <= endX; ++nx) {
                stableWater.add(hashCell(nx, y));
            }
            return true;
        }
        if (isLeftStable) {
            return propagateWater(endX + 1, y, unstable);
        } else if (isRightStable) {
            return propagateWater(startX - 1, y, unstable);
        } else {
            return propagateWater(endX + 1, y, unstable) || propagateWater(startX - 1, y, unstable);
        }
    }
    return false;
}

// For debugging purposes.
function dumpField() {
    for (let y = 0; y <= height; ++y) {
        let line = '';
        for (let x = 494; x <= 507; ++x) {
            line += unstableWater.has(y * width + x) ? '|' : stableWater.has(y * width + x) ? '~' : clayField.has(y * width + x) ? '#' : '.';
        }
        console.log(line);
    }
}

while (propagateWater(500, 0, false)) {}
propagateWater(500, 0, true);

console.log(stableWater.size + unstableWater.size);
console.log(stableWater.size);
