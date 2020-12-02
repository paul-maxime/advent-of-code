// Advent of Code 2018
// Day 08, Part 2

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const numbers = content.split(' ').filter(x => x.length > 0).map(x => parseInt(x));

let i = 0;

function parseNode() {
    let subNodesCount = numbers[i++];
    let metadataEntriesCount = numbers[i++];
    let subNodes = new Array(subNodesCount);
    for (let j = 0; j < subNodesCount; ++j) {
        subNodes[j] = parseNode();
    }
    let metaDataEntries = new Array(metadataEntriesCount);
    for (let j = 0; j < metadataEntriesCount; ++j) {
        metaDataEntries[j] = numbers[i++];
    }
    if (subNodesCount === 0) {
        return metaDataEntries.reduce((a, b) => a + b, 0);
    }
    return metaDataEntries.filter(x => x > 0 && x <= subNodes.length).map(x => subNodes[x - 1]).reduce((a, b) => a + b, 0);
}

console.log(parseNode());
