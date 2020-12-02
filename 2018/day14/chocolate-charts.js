// Advent of Code 2018
// Day 14

const INPUT = 760221;

const recipes = [ 3, 7 ];
const elves = [ 0, 1 ];

function improve() {
    let total = recipes[elves[0]] + recipes[elves[1]];
    if (total >= 10) {
        recipes.push(Math.floor(total / 10) % 10);
        if (parseInt(recipes.slice(-6).join('')) == INPUT) {
            return true;
        }
    }
    recipes.push(total % 10);
    if (parseInt(recipes.slice(-6).join('')) == INPUT) {
        return true;
    }
    elves[0] = (elves[0] + 1 + recipes[elves[0]]) % recipes.length;
    elves[1] = (elves[1] + 1 + recipes[elves[1]]) % recipes.length;
}

while (recipes.length < INPUT + 10) {
    improve();
}

console.log(recipes.slice(INPUT, INPUT + 10).join(''));

while (true) {
    if (improve()) {
        console.log(recipes.length - INPUT.toString().length);
        break;
    }
}
