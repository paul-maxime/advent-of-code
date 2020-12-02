// Advent of Code 2018
// Day 20

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8').trim();

let cursor = 0;

function parsePath() {
    let path = [];
    while (cursor < content.length) {
        const c = content[cursor];
        switch (c) {
            case 'N':
            case 'S':
            case 'E':
            case 'W':
                cursor++;
                path.push({ direction: c });
                break;
            case '^':
            case '$':
                cursor++;
                break;
            case '(':
                cursor++;
                path.push({ children: parseSplit() });
                break;
            default:
                return path;
        }
    }
    return path;
}

function parseSplit() {
    const paths = [ ];
    while (cursor < content.length) {
        const c = content[cursor];
        switch (c) {
            case ')':
                cursor++;
                return paths;
            case '|':
                cursor++;
                break;
            default:
                paths.push(parsePath());
        }
    }
    return paths;
}

const hashCell = (x, y) => y * 10000 + x;
const maze = new Map();

function buildMapFromPath(currentPath, x, y) {
    for (const element of currentPath) {
        if (element.direction) {
            let room = maze.get(hashCell(x, y));
            if (!room) {
                room = new Set();
                maze.set(hashCell(x, y), room);
            }
            room.add(element.direction);
            switch (element.direction) {
                case 'N':
                    y -= 1;
                    break;
                case 'S':
                    y += 1;
                    break;
                case 'E':
                    x += 1;
                    break;
                case 'W':
                    x -= 1;
                    break;
            }
        } else if (element.children) {
            for (const child of element.children) {
                buildMapFromPath(child, x, y);
            }
        }
    }
}

function findFurthestRoom() {
    const open = [{ x: 0, y: 0, d: 0 }];
    const closed = new Set();
    closed.add(hashCell(0, 0));
    let lastDistance = 0;
    let nbRoomAtLeast1000 = 0;
    while (open.length > 0) {
        const cell = open.shift();
        const { x, y, d } = cell;
        lastDistance = d;
        if (d >= 1000) {
            nbRoomAtLeast1000++;
        }
        const room = maze.get(hashCell(x, y));
        if (!room) {
            continue;
        }
        if (room.has('N') && !closed.has(hashCell(x, y - 1))) {
            closed.add(hashCell(x, y - 1));
            open.push({ x: x, y: y - 1, d: d + 1 });
        }
        if (room.has('S') && !closed.has(hashCell(x, y + 1))) {
            closed.add(hashCell(x, y + 1));
            open.push({ x: x, y: y + 1, d: d + 1 });
        }
        if (room.has('E') && !closed.has(hashCell(x + 1, y))) {
            closed.add(hashCell(x + 1, y));
            open.push({ x: x + 1, y: y, d: d + 1 });
        }
        if (room.has('W') && !closed.has(hashCell(x - 1, y))) {
            closed.add(hashCell(x - 1, y));
            open.push({ x: x - 1, y: y, d: d + 1 });
        }
    }
    return [lastDistance, nbRoomAtLeast1000];
}

const root = parsePath();
buildMapFromPath(root, 0, 0);
console.log(findFurthestRoom());
