// Advent of Code 2018
// Day 18

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(line => line.length > 0);

const height = lines.length;
const width = lines[0].length;
const field = new Array(width * height);

let currentField;
let nextField;

lines.forEach((line, y) => {
    [...line].forEach((c, x) => {
        field[y * width + x] = c;
    });
});

// For debugging purposes.
function printField() {
    for (let y = 0; y < height; ++y) {
        console.log(currentField.slice(y * width, (y + 1) * width).join(''));
    }
}

const adjacentCellDeltas = [
    [-1, -1], [0, -1], [1, -1],
    [-1, 0], [1, 0],
    [-1, 1], [0, 1], [1, 1]
];

function evolveCell(x, y) {
    const currentCell = currentField[y * width + x];
    const adjacentCells = adjacentCellDeltas
        .map(([dx, dy]) => [dx + x, dy + y])
        .filter(([ax, ay]) => ax >= 0 && ax < width && ay >= 0 && ay < height)
        .map(([ax, ay]) => currentField[ay * width + ax]);
    if (currentCell === '.') {
        if (adjacentCells.filter(c => c === '|').length >= 3) {
            nextField[y * width + x] = '|';
        } else {
            nextField[y * width + x] = '.';
        }
    }
    if (currentCell === '|') {
        if (adjacentCells.filter(c => c === '#').length >= 3) {
            nextField[y * width + x] = '#';
        } else {
            nextField[y * width + x] = '|';
        }
    }
    if (currentCell === '#') {
        if (adjacentCells.some(c => c === '|') && adjacentCells.some(c => c === '#')) {
            nextField[y * width + x] = '#';
        } else {
            nextField[y * width + x] = '.';
        }
    }
}

function getTotalResourceValue() {
    return currentField.filter(c => c === '#').length * currentField.filter(c => c === '|').length;
}

function evolve() {
    for (let y = 0; y < height; ++y) {
        for (let x = 0; x < width; ++x) {
            evolveCell(x, y);
        }
    }
    [currentField, nextField] = [nextField, currentField];
}

function computeEvolution(minutes) {
    currentField = field.slice(0);
    nextField = field.slice(0);

    let oldResourceValues = [];
    let lastRepeatingIndex = 0;
    let lastRepeatingDistance = 0;
    let repeatingCount = 0;

    for (let i = 0; i < minutes; ++i) {
        evolve();

        let currentValue = getTotalResourceValue();
        let oldIdenticalValue = oldResourceValues.find(x => x.value === currentValue);
        if (oldIdenticalValue) {
            const distance = i - oldIdenticalValue.index;
            if (i === lastRepeatingIndex + 1 && lastRepeatingDistance === distance) {
                ++repeatingCount;
            } else {
                repeatingCount = 0;
            }
            lastRepeatingIndex = i;
            lastRepeatingDistance = distance;
        }
        oldResourceValues.push({ index: i, value: currentValue });
        if (oldResourceValues.length > 50) {
            oldResourceValues.shift();
        }
        if (repeatingCount == 5) {
            const repeatingValues = oldResourceValues.slice(-lastRepeatingDistance);    
            return repeatingValues[(minutes - 1 - repeatingValues[0].index) % lastRepeatingDistance].value;
        }
    }

    return getTotalResourceValue();
}

console.log(computeEvolution(10));
console.log(computeEvolution(10000));
