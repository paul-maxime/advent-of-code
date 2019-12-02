use std::fs;

fn main() {
    let opcodes: Vec<usize> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    println!("1202 program alarm: {}", execute_program(&opcodes, 12, 2));

    let (noun, verb) = bruteforce_inputs(&opcodes, 19690720);
    println!("noun: {}, verb: {}, result: {}", noun, verb, 100 * noun + verb);
}

fn bruteforce_inputs(opcodes: &Vec<usize>, expected_output: usize) -> (usize, usize) {
    for noun in 0..99 {
        for verb in 0..99 {
            if execute_program(opcodes, noun, verb) == expected_output {
                return (noun, verb);
            }
        }
    }
    panic!("could not find matching inputs");
}

fn execute_program(program: &Vec<usize>, noun: usize, verb: usize) -> usize {
    let mut memory = program.clone();
    memory[1] = noun;
    memory[2] = verb;

    let mut ip = 0;
    while execute_opcode(&mut memory, &mut ip) {}

    memory[0]
}

fn execute_opcode(memory: &mut Vec<usize>, ip: &mut usize) -> bool {
    match memory[*ip] {
        1 => execute_addition(memory, ip),
        2 => execute_multiplication(memory, ip),
        99 => false,
        _ => panic!("unexpected opcode")
    }
}

fn execute_addition(memory: &mut Vec<usize>, ip: &mut usize) -> bool {
    let p1 = memory[*ip + 1];
    let p2 = memory[*ip + 2];
    let p3 = memory[*ip + 3];
    memory[p3] = memory[p1] + memory[p2];
    *ip += 4;
    true
}

fn execute_multiplication(memory: &mut Vec<usize>, ip: &mut usize) -> bool {
    let p1 = memory[*ip + 1];
    let p2 = memory[*ip + 2];
    let p3 = memory[*ip + 3];
    memory[p3] = memory[p1] * memory[p2];
    *ip += 4;
    true
}
