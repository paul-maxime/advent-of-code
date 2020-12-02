// Advent of Code 2018
// Day 12

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(x => x.length > 0);

const initialState = new Array(1024).fill(false);
[...lines[0].substring('initial state: '.length)].forEach((x, i) => {
    initialState[512 + i] = x === '#';
});

const combinations = new Array(32).fill(false);
lines.slice(1).map(line => line.split(' => ')).forEach(([combination, result]) => {
    const hash = [...combination].map((x, i) => x === '#' ? Math.pow(2, i) : 0).reduce((x, y) => x + y);
    combinations[hash] = result === '#';
});

function applyNextState(currentState, nextState) {
    for (let i = 0; i < currentState.length; ++i) {
        let hash = 0;
        for (let j = 0; j < 5; ++j) {
            const pos = i + j - 2;
            if (pos < 0 || pos >= currentState.length) continue;
            hash += currentState[pos] ? Math.pow(2, j) : 0;
        }
        nextState[i] = combinations[hash];
    }
}

function computeGeneration(generation) {
    let currentFlowers = initialState.slice(0);
    let nextFlowers = new Array(initialState.length);
    
    let currentIncrement = 0;
    let sameIncrementTimes = 0;
    let oldTotal = 0;
    
    for (let i = 0; i < generation; ++i) {
        applyNextState(currentFlowers, nextFlowers);
        [currentFlowers, nextFlowers] = [nextFlowers, currentFlowers];
        const total = currentFlowers.map((x, i) => x ? i - 512 : 0).reduce((x, y) => x + y);
        const delta = total - oldTotal;
        if (delta === currentIncrement) {
            sameIncrementTimes++;
            if (sameIncrementTimes > 5) {
                return total + delta * (generation - (i + 1));
            }
        } else {
            currentIncrement = delta;
            sameIncrementTimes = 0;
        }
        oldTotal = total;
    }
    return oldTotal;
}

console.log(computeGeneration(20));
console.log(computeGeneration(50000000000));
