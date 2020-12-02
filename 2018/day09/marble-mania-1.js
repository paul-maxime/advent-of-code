// Advent of Code 2018
// Day 09, Part 1

const PLAYERS = 400;
const LAST_MARBLE = 71864;

let marbles = [0];
let currentPlayer = 0;
let currentMarble = 1;
let currentMarblePosition = 0;
let scores = new Array(PLAYERS).fill(0);

while (currentMarble !== LAST_MARBLE + 1) {
    if (currentMarble % 23 === 0) {
        currentMarblePosition = (marbles.length + currentMarblePosition - 7) % marbles.length;
        const removedMarble = marbles.splice(currentMarblePosition, 1);
        scores[currentPlayer] += currentMarble + removedMarble[0];
    } else {
        currentMarblePosition = (currentMarblePosition + 2) % marbles.length;
        marbles.splice(currentMarblePosition, 0, currentMarble);
    }
    currentMarble++;
    currentPlayer = (currentMarble + 1) % PLAYERS;
}

const highest = Math.max(...scores);
console.log(highest);