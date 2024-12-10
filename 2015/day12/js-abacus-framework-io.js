import fs from "fs";

const input = fs.readFileSync("input");
const jsonData = JSON.parse(input);

function sumNumbersDeep(data, ignoreRed) {
    if (Number.isInteger(data)) return data;
    if (typeof data === "string") return 0;

    if (ignoreRed && !Array.isArray(data) && Object.values(data).some(x => x === "red")) return 0;

    return Object.values(data)
        .map(inner => sumNumbersDeep(inner, ignoreRed))
        .reduce((a, b) => a + b);
}

console.log(sumNumbersDeep(jsonData, false));
console.log(sumNumbersDeep(jsonData, true));
