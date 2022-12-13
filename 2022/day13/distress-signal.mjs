import fs from 'fs';

const input = fs.readFileSync("input").toString()
    .split("\n\n")
    .map(block => block.split("\n")
        .filter(x => x)
        .map(x => JSON.parse(x))
    );

function compare(a, b) {
    if (typeof a === 'number' && typeof b === 'number') {
        return Math.sign(a - b);
    }
    if (typeof a === 'number') a = [a];
    if (typeof b === 'number') b = [b];
    for (let i = 0; i < a.length && i < b.length; i++) {
        const r = compare(a[i], b[i]);
        if (r !== 0) return r;
    }
    return Math.sign(a.length - b.length);
}

console.log(
    input.map(([a, b], i) => [i, compare(a, b)]).reduce((v, [i, c]) => v + (c < 0 ? i + 1 : 0), 0)
);

const sorted = input.flatMap(x => x).concat([[[2]]], [[[6]]]).sort(compare);
console.log(
    (sorted.findIndex(x => JSON.stringify(x) === '[[2]]') + 1) *
    (sorted.findIndex(x => JSON.stringify(x) === '[[6]]') + 1)
);
