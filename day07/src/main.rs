use std::cmp;
use std::fs;

struct RunningProgram {
    memory: Vec<i32>,
    ip: usize
}

fn main() {
    let opcodes: Vec<i32> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    println!("{}", find_best_permutation(&opcodes, &mut vec![0, 1, 2, 3, 4], 0, &execute_amplification_circuit));
    println!("{}", find_best_permutation(&opcodes, &mut vec![0, 1, 2, 3, 4], 0, &execute_feedback_loop));
}

fn find_best_permutation<F: Fn(&Vec<i32>, &mut Vec<i32>) -> i32>(program: &Vec<i32>, execution_order: &mut Vec<i32>, index: usize, amplication_fn: &F) -> i32 {
    if index == execution_order.len() {
        amplication_fn(program, execution_order)
    } else {
        let mut best_output = 0;
        for i in index..execution_order.len() {
            execution_order.swap(index, i);
            best_output = cmp::max(find_best_permutation(program, execution_order, index + 1, amplication_fn), best_output);
            execution_order.swap(index, i);
        }
        best_output
    }
}

fn execute_amplification_circuit(program: &Vec<i32>, execution_order: &mut Vec<i32>) -> i32 {
    let mut outputs = Vec::new();
        let mut current_output = 0;
        for n in execution_order.clone() {
            execute_program(&program, &mut vec![current_output, n], &mut outputs);
            current_output = outputs.pop().unwrap();
        }
        current_output
}

fn execute_feedback_loop(program: &Vec<i32>, execution_order: &mut Vec<i32>) -> i32 {
    let mut programs: Vec<RunningProgram> = (0..5).map(|_| {
        RunningProgram {
            memory: program.clone(),
            ip: 0
        }
    }).collect();
    let mut current_output = 0;
    for i in 0..5 {
        current_output = execute_until_output(
            programs.get_mut(i).unwrap(),
            &mut vec![current_output, *execution_order.get(i).unwrap() + 5]
        ).unwrap();
    }
    let mut current_program = 0;
    loop {
        match execute_until_output(programs.get_mut(current_program).unwrap(), &mut vec![current_output]) {
            Some(value) => { current_output = value; }
            None => { break; }
        }
        current_program = (current_program + 1) % 5;
    }
    current_output
}

fn execute_program(program: &Vec<i32>, inputs: &mut Vec<i32>, outputs: &mut Vec<i32>) {
    let mut memory = program.clone();
    let mut ip = 0;
    while execute_opcode(&mut memory, &mut ip, inputs, outputs) {}
}

fn execute_until_output(program: &mut RunningProgram, inputs: &mut Vec<i32>) -> Option<i32> {
    let mut outputs = Vec::new();
    while execute_opcode(&mut program.memory, &mut program.ip, inputs, &mut outputs) {
        if let Some(output) = outputs.pop() {
            return Some(output);
        }
    }
    None
}

fn execute_opcode(memory: &mut Vec<i32>, ip: &mut usize, inputs: &mut Vec<i32>, outputs: &mut Vec<i32>) -> bool {
    let opcode = memory[*ip] % 100;
    match opcode {
        1 => execute_addition(memory, ip),
        2 => execute_multiplication(memory, ip),
        3 => execute_input(memory, ip, inputs),
        4 => execute_output(memory, ip, outputs),
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

fn execute_input(memory: &mut Vec<i32>, ip: &mut usize, inputs: &mut Vec<i32>) {
    set_parameter(memory, ip, 1, inputs.pop().unwrap());
    *ip += 2;
}

fn execute_output(memory: &mut Vec<i32>, ip: &mut usize, outputs: &mut Vec<i32>) {
    let p1 = get_parameter(memory, ip, 1);
    outputs.push(p1);
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
