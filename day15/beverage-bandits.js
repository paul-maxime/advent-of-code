// Advent of Code 2018
// Day 15

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(x => x.length > 0);

const height = lines.length;
const width = lines[0].length;
const map = new Array(width * height);
let entities = [];

function loadMap(elfAttack) {
    entities = [];
    lines.forEach((line, y) => {
        [...line].forEach((c, x) => {
            if (c === 'E' || c === 'G') {
                entities.push({ x, y, type: c, health: 200, attack: c === 'E' ? elfAttack : 3 });
            }
            map[y * width + x] = c;
        });
    });
}

function isCellFree(x, y) {
    return x >= 0 && y >= 0 && x < width && y < height && map[y * width + x] === '.';
}

function tryToAddCell(open, closed, x, y, parent) {
    if (isCellFree(x, y) && !closed.has(y * width + x)) {
        open.push({ x, y, parent, distance: parent.distance + 1 });
        closed.add(y * width + x);
    }
}

function addNeighbors(open, closed, parent) {
    const { x, y } = parent;
    tryToAddCell(open, closed, x - 1, y, parent);
    tryToAddCell(open, closed, x + 1, y, parent);
    tryToAddCell(open, closed, x, y - 1, parent);
    tryToAddCell(open, closed, x, y + 1, parent);
}

function isOpponent(x, y, opponentType) {
    return map[y * width + x] === opponentType;
}

function isNextToOpponent(cell, opponentType) {
    return isOpponent(cell.x - 1, cell.y, opponentType) ||
        isOpponent(cell.x + 1, cell.y, opponentType) ||
        isOpponent(cell.x, cell.y - 1, opponentType) ||
        isOpponent(cell.x, cell.y + 1, opponentType);
}

function compareScore(a, b) {
    if (a.distance !== b.distance) return b.distance - a.distance;
    if (a.y !== b.y) return b.y - a.y;
    return b.x - a.x;
}

function findPathToNearestOpponent(entity) {
    const opponentType = entity.type === 'E' ? 'G' : 'E';
    let open = [{ x: entity.x, y: entity.y, distance: 0 }];
    let closed = new Set();
    closed.add(entity.y * width + entity.x);
    let found = [];
    while (open.length > 0) {
        const current = open.sort(compareScore).pop();
        if (isNextToOpponent(current, opponentType)) {
            found.push(current);
            open = open.filter(node => node.distance == current.distance);
        }
        if (found.length == 0) {
            addNeighbors(open, closed, current);
        }
    }
    found.sort(compareScore);
    return found.length > 0 ? found[found.length - 1] : null;
}
 
function compareOpponents(a, b) {
    if (a.health !== b.health) return a.health - b.health;
    if (a.y !== b.y) return a.y - b.y;
    return a.x - b.x;
}

function getTargetOpponent(entity) {
    const opponentType = entity.type === 'E' ? 'G' : 'E';
    const neighbors = [[0, -1], [-1, 0], [0, 1], [1, 0]];

    return neighbors.map(n => {
        if (isOpponent(entity.x + n[0], entity.y + n[1], opponentType)) {
            return entities.find(op => op.x == entity.x + n[0] && op.y == entity.y + n[1]);
        }
    }).filter(x => !!x).sort(compareOpponents)[0] || null;
}

function playEntity(entity) {
    if (entity.health <= 0) return;
    const path = findPathToNearestOpponent(entity);
    if (path && path.parent) {
        let node = path;
        while (node.parent && node.parent.parent) {
            node = node.parent;
        }
        map[entity.y * width + entity.x] = '.';
        entity.x = node.x;
        entity.y = node.y;
        map[entity.y * width + entity.x] = entity.type;
    }
    const target = getTargetOpponent(entity);
    if (target) {
        target.health -= entity.attack;
        if (target.health <= 0) {
            entities = entities.filter(x => x !== target);
            map[target.y * width + target.x] = '.';
        }
    }
}

function getWinner() {
    const goblin = entities.find(x => x.type == 'G');
    const elf = entities.find(x => x.type == 'E');
    return !goblin ? 'E' : !elf ? 'G' : null;
}


function playRound() {
    const sortedEntities = entities.sort((a, b) => a.y === b.y ? a.x - b.x : a.y - b.y);
    let hasEveryonePlayed = true;
    for (const entity of sortedEntities) {
        if (getWinner() === null) {
            playEntity(entity);
        } else {
            hasEveryonePlayed = false;
        }
    }
    return hasEveryonePlayed;
}

function computeVanillaBattle() {
    loadMap(3);
    let turn = 0;
    while (getWinner() === null) {
        if (playRound()) {
            turn++;
        }
    }
    return turn * entities.reduce((a, b) => a + b.health, 0);
}

function computeAdvancedBattle(level) {
    loadMap(level);
    let turn = 0;
    const expectedSurvivors = entities.filter(x => x.type === 'E').length;
    while (getWinner() === null) {
        if (playRound()) {
            turn++;
        }
        const survivors = entities.filter(x => x.type === 'E').length;
        if (survivors < expectedSurvivors) {
            return -1;
        }
    }
    return turn * entities.reduce((a, b) => a + b.health, 0);
}

console.log(computeVanillaBattle());

let level = 4;
while (true) {
    const score = computeAdvancedBattle(level++);
    if (score > 0) {
        console.log(score);
        break;
    }
}
