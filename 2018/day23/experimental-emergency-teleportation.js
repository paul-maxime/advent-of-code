// Advent of Code 2018
// Day 23

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(line => line.length > 0);

const nanobots = lines.map((line, id) => {
    const [_, x, y, z, radius] = line.match(/^pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)$/);
    return {
        id,
        x: parseInt(x),
        y: parseInt(y),
        z: parseInt(z),
        radius: parseInt(radius),
    };
});

const manhattanDistance = (a, b) => Math.abs(a.x - b.x) + Math.abs(a.y - b.y) + Math.abs(a.z - b.z);

const botsInRange = new Array(nanobots.length * nanobots.length);

for (const botA of nanobots) {
    for (const botB of nanobots) {
        const isInRange = manhattanDistance(botA, botB) <= (botA.radius + botB.radius);
        botsInRange[botA.id * nanobots.length + botB.id] = isInRange;
    }
}

const strongestNanobot = nanobots.reduce((a, b) => a.radius > b.radius ? a : b);
const nbNanobotsInRange = nanobots.filter(bot => manhattanDistance(strongestNanobot, bot) <= strongestNanobot.radius).length;
console.log(nbNanobotsInRange);

function isGroupValid(group) {
    for (const botA of group) {
        for (const botB of group) {
            const isInRange = botsInRange[botA.id * nanobots.length + botB.id];
            if (!isInRange) {
                return false;
            }
        }
    }
    return true;
}

function incrementFilter(filter) {
    let hasRemainer = true;
    for (let i = 0; i < filter.length && hasRemainer; ++i) {
        filter[i]++;
        hasRemainer = (filter[i] === nanobots.length);
        if (hasRemainer) {
            filter[i] = 0;
        }
    }
    if (hasRemainer) {
        filter.push(0);
    }
}

function findFilteredBots(bots) {
    let filter = [];
    while (true) {
        const currentbots = bots.filter(x => filter.indexOf(x.id) === -1);
        if (isGroupValid(currentbots)) {
            return filter;
        }
        incrementFilter(filter);
    }
}

const filteredIndexes = new Set();

const STEP = 10;

for (let i = 0; i <= Math.floor(nanobots.length / STEP); ++i) {
    const start = i * STEP;
    const end = (i + 1) * STEP;
    const filteredStep = findFilteredBots(nanobots.slice(start, end));
    filteredStep.forEach(x => filteredIndexes.add(x));
}

const filteredNanobots = nanobots.filter(x => !filteredIndexes.has(x.id));

let min = 0;
let max = 1000000000;

filteredNanobots.forEach(bot => {
    let minDistance = bot.x + bot.y + bot.z - bot.radius;
    let maxDistance = bot.x + bot.y + bot.z + bot.radius;
    if (minDistance > min) {
        min = minDistance;
    }
    if (maxDistance < max) {
        max = maxDistance;
    }
});

console.log(min, max);
