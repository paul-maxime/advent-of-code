// Advent of Code 2018
// Day 09, Part 2

const PLAYERS = 400;
const LAST_MARBLE = 7186400;

class Node {
    constructor(value, next, prev) {
        this.value = value;
        this.next = next || this;
        this.prev = prev || this;
    }
    insertAfter(value) {
        const oldNext = this.next;
        this.next = new Node(value, oldNext, this);
        oldNext.prev = this.next;
        return this.next;
    }
    remove() {
        this.prev.next = this.next;
        this.next.prev = this.prev;
        return this.next;
    }
    get(delta) {
        let node = this;
        for (let i = 0; i < Math.abs(delta); ++i) {
            node = delta > 0 ? node.next : node.prev;
        }
        return node;
    }
    dump() {
        let startingNode = this;
        while (startingNode.value !== 0) {
            startingNode = startingNode.next;
        }
        let node = startingNode;
        let values = [];
        do {
            values.push(this === node ? `(${node.value})` : node.value.toString());
            node = node.next;
        } while (node !== startingNode);
        return values;
    }
}

let marbles = new Node(0);

let currentPlayer = 0;
let currentMarble = 1;
let scores = new Array(PLAYERS).fill(0);

while (currentMarble !== LAST_MARBLE + 1) {
    if (currentMarble % 23 === 0) {
        const removedMarble = marbles.get(-7);
        marbles = removedMarble.remove();
        scores[currentPlayer] += currentMarble + removedMarble.value;
    } else {
        marbles = marbles.get(1);
        marbles = marbles.insertAfter(currentMarble);
    }
    // DEBUG: console.log(`[${currentPlayer + 1}] ${marbles.dump().join(' ')}`);
    currentMarble++;
    currentPlayer = (currentPlayer + 1) % PLAYERS;
}

const highest = Math.max(...scores);
console.log(highest);
