use std::fs;

fn main() {
    let opcodes: Vec<i32> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    execute_program(&opcodes, 1);
    execute_program(&opcodes, 5);
}

fn execute_program(program: &Vec<i32>, input: i32) {
    let mut memory = program.clone();
    let mut ip = 0;
    while execute_opcode(&mut memory, &mut ip, input) {}
}

fn execute_opcode(memory: &mut Vec<i32>, ip: &mut usize, input: i32) -> bool {
    let opcode = memory[*ip] % 100;
    match opcode {
        1 => execute_addition(memory, ip),
        2 => execute_multiplication(memory, ip),
        3 => execute_input(memory, ip, input),
        4 => execute_output(memory, ip),
        5 => execute_jump_if_true(memory, ip),
        6 => execute_jump_if_false(memory, ip),
        7 => execute_less_than(memory, ip),
        8 => execute_equals(memory, ip),
        99 => (),
        _ => panic!("unexpected opcode")
    }
    opcode != 99
}

fn get_parameter(memory: &mut Vec<i32>, ip: &mut usize, n: usize) -> i32 {
    let is_immediate = (memory[*ip] / 10_i32.pow(n as u32 + 1)) % 10 == 1;
    let value = memory[*ip + n];
    if is_immediate {
        value
    } else {
        memory[value as usize]
    }
}

fn set_parameter(memory: &mut Vec<i32>, ip: &mut usize, n: usize, value: i32) {
    let addr = memory[*ip + n];
    memory[addr as usize] = value;
}

fn execute_addition(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    let p2 = get_parameter(memory, ip, 2);
    set_parameter(memory, ip, 3, p1 + p2);
    *ip += 4;
}

fn execute_multiplication(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    let p2 = get_parameter(memory, ip, 2);
    set_parameter(memory, ip, 3, p1 * p2);
    *ip += 4;
}

fn execute_input(memory: &mut Vec<i32>, ip: &mut usize, input: i32) {
    set_parameter(memory, ip, 1, input);
    *ip += 2;
}

fn execute_output(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    println!("{}", p1);
    *ip += 2;
}

fn execute_jump_if_true(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    let p2 = get_parameter(memory, ip, 2);
    if p1 != 0 {
        *ip = p2 as usize;
    } else {
        *ip += 3;
    }
}

fn execute_jump_if_false(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    let p2 = get_parameter(memory, ip, 2);
    if p1 == 0 {
        *ip = p2 as usize;
    } else {
        *ip += 3;
    }
}

fn execute_less_than(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    let p2 = get_parameter(memory, ip, 2);
    set_parameter(memory, ip, 3, if p1 < p2 { 1 } else { 0 });
    *ip += 4;
}

fn execute_equals(memory: &mut Vec<i32>, ip: &mut usize) {
    let p1 = get_parameter(memory, ip, 1);
    let p2 = get_parameter(memory, ip, 2);
    set_parameter(memory, ip, 3, if p1 == p2 { 1 } else { 0 });
    *ip += 4;
}
