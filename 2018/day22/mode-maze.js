// Advent of Code 2018
// Day 22

const CAVE_DEPTH = 8787;
const TARGET_X = 10;
const TARGET_Y = 725;

const TYPE_ROCKY = 0;
const TYPE_WET = 1;
const TYPE_NARROW = 2;

const TOOL_NEITHER = 0;
const TOOL_TORCH = 1;
const TOOL_CLIMBING_GEAR = 2;

const regionTypes = ['.', '=', '|'];

const hashCell = (x, y) => x * 1000 + y;
const geologicIndices = new Map();
const erosionLevels = new Map();

function computeGeologicIndex(x, y) {
    if (x === 0 && y === 0) {
        return 0;
    }
    if (x === TARGET_X && y === TARGET_Y) {
        return 0;
    }
    if (y === 0) {
        return x * 16807;
    }
    if (x === 0) {
        return y * 48271;
    }
    return getErosionLevel(x - 1, y) * getErosionLevel(x, y - 1);
}

function computeErosionLevel(x, y) {
    return (getGeologicIndex(x, y) + CAVE_DEPTH) % 20183;
}

function getGeologicIndex(x, y) {
    const hash = hashCell(x, y);
    let value = geologicIndices.get(hash);
    if (value === undefined) {
        value = computeGeologicIndex(x, y);
        geologicIndices.set(hash, value);
    }
    return value;
}

function getErosionLevel(x, y) {
    const hash = hashCell(x, y);
    let value = erosionLevels.get(hash);
    if (value === undefined) {
        value = computeErosionLevel(x, y);
        erosionLevels.set(hash, value);
    }
    return value;
}

function computeRegionType(x, y) {
    return getErosionLevel(x, y) % 3;
}

// For debugging purposes.
function printCave() {
    for (let y = 0; y <= TARGET_Y; ++y) {
        let line = "";
        for (let x = 0; x <= TARGET_X; ++x) {
            line += regionTypes[computeRegionType(x, y)];
        }
        console.log(line);
    }
}

function computeRiskLevel(fromX, fromY, toX, toY) {
    let riskLevel = 0;
    for (let y = fromY; y <= toY; ++y) {
        for (let x = fromX; x <= toX; ++x) {
            riskLevel += computeRegionType(x, y);
        }
    }
    return riskLevel;
}

const adjacentDeltas = [[0, -1], [0, 1], [-1, 0], [1, 0]];
const availableTools = [TOOL_NEITHER, TOOL_TORCH, TOOL_CLIMBING_GEAR];
const hashCellAndTool = (x, y, t) => x * 10000 + y * 10 + t;

function findPath(fromX, fromY, toX, toY) {
    const open = [];
    const closed = new Map();
    open.push({ x: fromX, y: fromY, time: 0, tool: TOOL_TORCH });
    closed.set(hashCellAndTool(fromX, fromY, TOOL_TORCH), 0);
    while (open.length > 0) {
        const node = open.sort((a, b) => b.time - a.time).pop();
        if (node.x === toX && node.y === toY && node.tool === TOOL_TORCH) {
            return node.time;
        }
        adjacentDeltas.map(([dx, dy]) => [node.x + dx, node.y + dy]).filter(([x, y]) => x >= 0 && y >= 0 && x < TARGET_X * 8).forEach(([x, y]) => {
            const nextRegionType = computeRegionType(x, y);
            if (nextRegionType !== node.tool) {
                let existingNode = closed.get(hashCellAndTool(x, y, node.tool));
                if (!existingNode || existingNode.time > node.time + 1) {
                    const newNode = { x, y, time: node.time + 1, tool: node.tool, parent: node };
                    open.push(newNode);
                    closed.set(hashCellAndTool(x, y, node.tool), newNode);
                }
            }
        });
        const currentRegionType = computeRegionType(node.x, node.y);
        availableTools.filter((tool) => tool !== node.tool && tool !== currentRegionType).forEach((tool) => {
            let existingNode = closed.get(hashCellAndTool(node.x, node.y, tool));
            if (!existingNode || existingNode.time > node.time + 7) {
                const newNode = { x: node.x, y: node.y, time: node.time + 7, tool, parent: node };
                open.push(newNode);
                closed.set(hashCellAndTool(node.x, node.y, tool), newNode);
            }
        });
    }
    throw new Error('No path available.');
}

console.log(computeRiskLevel(0, 0, TARGET_X, TARGET_Y));
console.log(findPath(0, 0, TARGET_X, TARGET_Y));
