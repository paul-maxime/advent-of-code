use std::collections::VecDeque;
use std::fs;

struct IntcodeProgram {
    memory: Vec<i128>,
    ip: usize,
    relative_base: i128,
    inputs: VecDeque<i128>,
    outputs: VecDeque<i128>,
}

impl IntcodeProgram {
    fn from(opcodes: &Vec<i128>, memory_size: usize) -> IntcodeProgram {
        let mut program = IntcodeProgram {
            memory: opcodes.clone(),
            ip: 0,
            relative_base: 0,
            inputs: VecDeque::new(),
            outputs: VecDeque::new(),
        };
        program.memory.resize(memory_size, 0);
        program
    }

    fn run_until_halt(&mut self) {
        while self.execute_opcode() {}
    }

    fn execute_opcode(&mut self) -> bool {
        let opcode = self.memory[self.ip] % 100;
        match opcode {
            1 => self.execute_addition(),
            2 => self.execute_multiplication(),
            3 => self.execute_input(),
            4 => self.execute_output(),
            5 => self.execute_jump_if_true(),
            6 => self.execute_jump_if_false(),
            7 => self.execute_less_than(),
            8 => self.execute_equals(),
            9 => self.execute_adjust_relative_base(),
            99 => (),
            _ => panic!("unexpected opcode")
        }
        opcode != 99
    }

    fn get_parameter(&mut self, n: usize) -> i128 {
        let address_type = (self.memory[self.ip] / 10_i32.pow(n as u32 + 1) as i128) % 10;
        let value = self.memory[self.ip + n];
        if address_type == 1 { // immediate
            value
        } else if address_type == 2 { // relative
            self.memory[(value + self.relative_base) as usize]
        } else { // absolute
            self.memory[value as usize]
        }
    }

    fn set_parameter(&mut self, n: usize, value: i128) {
        let address_type = (self.memory[self.ip] / 10_i32.pow(n as u32 + 1) as i128) % 10;
        let destination = self.memory[self.ip + n];
        if address_type == 1 { // immediate
            panic!("immediate address type in set_parameter");
        } else if address_type == 2 { // relative
            self.memory[(destination + self.relative_base) as usize] = value;
        } else { // absolute
            self.memory[destination as usize] = value;
        }
    }

    fn execute_addition(&mut self) {
        let p1 = self.get_parameter(1);
        let p2 = self.get_parameter(2);
        self.set_parameter(3, p1 + p2);
        self.ip += 4;
    }

    fn execute_multiplication(&mut self) {
        let p1 = self.get_parameter(1);
        let p2 = self.get_parameter(2);
        self.set_parameter(3, p1 * p2);
        self.ip += 4;
    }

    fn execute_input(&mut self) {
        let input = self.inputs.pop_front().unwrap();
        self.set_parameter(1, input);
        self.ip += 2;
    }

    fn execute_output(&mut self) {
        let p1 = self.get_parameter(1);
        self.outputs.push_back(p1);
        self.ip += 2;
    }

    fn execute_jump_if_true(&mut self) {
        let p1 = self.get_parameter(1);
        let p2 = self.get_parameter(2);
        if p1 != 0 {
            self.ip = p2 as usize;
        } else {
            self.ip += 3;
        }
    }

    fn execute_jump_if_false(&mut self) {
        let p1 = self.get_parameter(1);
        let p2 = self.get_parameter(2);
        if p1 == 0 {
            self.ip = p2 as usize;
        } else {
            self.ip += 3;
        }
    }

    fn execute_less_than(&mut self) {
        let p1 = self.get_parameter(1);
        let p2 = self.get_parameter(2);
        self.set_parameter(3, if p1 < p2 { 1 } else { 0 });
        self.ip += 4;
    }

    fn execute_equals(&mut self) {
        let p1 = self.get_parameter(1);
        let p2 = self.get_parameter(2);
        self.set_parameter(3, if p1 == p2 { 1 } else { 0 });
        self.ip += 4;
    }

    fn execute_adjust_relative_base(&mut self) {
        let p1 = self.get_parameter(1);
        self.relative_base += p1;
        self.ip += 2;
    }
}

fn main() {
    let opcodes: Vec<i128> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    println!("{}", run_boost_program(&opcodes, 1));
    println!("{}", run_boost_program(&opcodes, 2));
}

fn run_boost_program(opcodes: &Vec<i128>, input: i128) -> i128 {
    let mut program = IntcodeProgram::from(&opcodes, 1024 * 64);
    program.inputs.push_back(input);
    program.run_until_halt();
    program.outputs.pop_front().unwrap()
}
