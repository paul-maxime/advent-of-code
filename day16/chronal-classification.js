// Advent of Code 2018
// Day 16

const fs = require('fs');
const content = fs.readFileSync('./input', 'utf8');
const [ sampleLines, programLines ] = content.split('\n\n\n\n').map(block => block.split('\n').filter(line => line.length > 0));

const samplesData = sampleLines.map(line => {
    const [_, n1, n2, n3, n4] = line.match(/(\d+).+(\d+).+(\d+).+(\d+)/);
    return [n1, n2, n3, n4].map(x => parseInt(x));
});

const samples = [];
for (let i = 0; i < samplesData.length; i += 3) {
    const [ before, operation, after ] = [ samplesData[i + 0], samplesData[i + 1], samplesData[i + 2] ];
    samples.push({ before, operation, after });
}

const program = programLines.map(line => {
    const [_, n1, n2, n3, n4] = line.match(/(\d+).+(\d+).+(\d+).+(\d+)/);
    return [n1, n2, n3, n4].map(x => parseInt(x));
});

const opcodes = [{
    name: 'addr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] + registers[b];
    }
}, {
    name: 'addi',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] + b;
    }
}, {
    name: 'mulr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] * registers[b];
    }
}, {
    name: 'muli',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] * b;
    }
}, {
    name: 'banr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] & registers[b];
    }
}, {
    name: 'bani',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] & b;
    }
}, {
    name: 'borr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] | registers[b];
    }
}, {
    name: 'bori',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] | b;
    }
}, {
    name: 'setr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a];
    }
}, {
    name: 'seti',
    fct: (registers, a, b, c) => {
        registers[c] = a;
    }
}, {
    name: 'gtir',
    fct: (registers, a, b, c) => {
        registers[c] = a > registers[b] ? 1 : 0;
    }
}, {
    name: 'gtri',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] > b ? 1 : 0;
    }
}, {
    name: 'gtrr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] > registers[b] ? 1 : 0;
    }
}, {
    name: 'eqir',
    fct: (registers, a, b, c) => {
        registers[c] = a === registers[b] ? 1 : 0;
    }
}, {
    name: 'eqri',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] === b ? 1 : 0;
    }
}, {
    name: 'eqrr',
    fct: (registers, a, b, c) => {
        registers[c] = registers[a] === registers[b] ? 1 : 0;
    }
}];

const mappedOpcodes = new Map();

function computeSamplesWithMoreThan3PossibleOpcodes() {
    return samples.map(sample => {
        const [_, a, b, c] = sample.operation;
        let possibleOpcodes = 0;
        for (const opcode of opcodes) {
            const registers = sample.before.slice();
            opcode.fct(registers, a, b, c);
            if (registers[0] === sample.after[0] && registers[1] === sample.after[1] && registers[2] === sample.after[2] && registers[3] === sample.after[3]) {
                possibleOpcodes++;
            }
        }
        return possibleOpcodes;
    }).map(x => x >= 3 ? 1 : 0).reduce((a, b) => a + b);
}

function computeOpcodeIds() {
    for (const sample of samples) {
        const [id, a, b, c] = sample.operation;
        if (mappedOpcodes.has(id)) {
            continue;
        }
        let possibleOpcodes = 0;
        let latestOpcode = null;
        for (const opcode of opcodes) {
            if (mappedOpcodes.has(opcode.id)) {
                continue;
            }
            const registers = sample.before.slice();
            opcode.fct(registers, a, b, c);
            if (registers[0] === sample.after[0] && registers[1] === sample.after[1] && registers[2] === sample.after[2] && registers[3] === sample.after[3]) {
                possibleOpcodes++;
                latestOpcode = opcode;
            }
        }
        if (possibleOpcodes === 1) {
            latestOpcode.id = id;
            mappedOpcodes.set(id, latestOpcode);
        }
    };
}

function executeProgram() {
    const registers = [0, 0, 0, 0];
    console.log(registers);
    for (const instruction of program) {
        const [opcode, a, b, c] = instruction;
        mappedOpcodes.get(opcode).fct(registers, a, b, c);
    }
    return registers;
}

console.log(computeSamplesWithMoreThan3PossibleOpcodes());

computeOpcodeIds();

console.log(executeProgram()[0]);
