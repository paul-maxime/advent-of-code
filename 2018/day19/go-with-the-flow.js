// Advent of Code 2018
// Day 19

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const lines = content.split('\n').filter(line => line.length > 0);

const rip = parseInt(lines[0].substr('#ip '.length));

const program = lines.slice(1).map(line => {
    const [opcode, a, b, c] = line.split(' ').map((x, i) => i > 0 ? parseInt(x) : x);
    return { opcode, a, b, c };
});

const opcodes = {
    addr: (r, a, b) => r[a] + r[b],
    addi: (r, a, b) => r[a] + b,
    mulr: (r, a, b) => r[a] * r[b],
    muli: (r, a, b) => r[a] * b,
    banr: (r, a, b) => r[a] & r[b],
    bani: (r, a, b) => r[a] & b,
    borr: (r, a, b) => r[a] | r[b],
    bori: (r, a, b) => r[a] | b,
    setr: (r, a, _) => r[a],
    seti: (r, a, _) => a,
    gtir: (r, a, b) => a > r[b] ? 1 : 0,
    gtri: (r, a, b) => r[a] > b ? 1 : 0,
    gtrr: (r, a, b) => r[a] > r[b] ? 1 : 0,
    eqir: (r, a, b) => a === r[b] ? 1 : 0,
    eqri: (r, a, b) => r[a] === b ? 1 : 0,
    eqrr: (r, a, b) => r[a] === r[b] ? 1 : 0,
};

function executeProgram(registers) {
    let time = 0;
    while (registers[rip] < program.length) {
        if (registers[rip] === 2) console.log(registers);
        const instruction = program[registers[rip]];
        registers[instruction.c] = opcodes[instruction.opcode](registers, instruction.a, instruction.b);
        registers[rip]++;
        ++time;
    }
    console.log(time);
    return registers[0];
}

console.log(executeProgram([0, 0, 0, 0, 0, 0]));

// console.log(executeProgram([1, 0, 0, 0, 0, 0]));

// OK, this is taking way too much time to process.
// Take a look at program_v3.c for the optimized C implementation.
