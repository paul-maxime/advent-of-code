use std::collections::VecDeque;
use std::fs;

struct IntcodeProgram {
    memory: Vec<i64>,
    ip: usize,
    relative_base: i64,
    inputs: VecDeque<i64>,
    outputs: VecDeque<i64>,
}

impl IntcodeProgram {
    fn from(opcodes: &Vec<i64>, memory_size: usize) -> IntcodeProgram {
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
        while self.execute_next_opcode() {}
    }

    fn run_until_input(&mut self) -> bool {
        loop {
            if self.next_opcode() == 3 && self.inputs.len() == 0 {
                return true;
            }
            if !self.execute_next_opcode() {
                return false;
            }
        }
    }

    fn next_opcode(&mut self) -> i64 {
        self.memory[self.ip] % 100
    }

    fn execute_next_opcode(&mut self) -> bool {
        let opcode = self.next_opcode();
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

    fn get_parameter(&mut self, n: usize) -> i64 {
        let address_type = (self.memory[self.ip] / 10_i32.pow(n as u32 + 1) as i64) % 10;
        let value = self.memory[self.ip + n];
        if address_type == 1 { // immediate
            value
        } else if address_type == 2 { // relative
            self.memory[(value + self.relative_base) as usize]
        } else { // absolute
            self.memory[value as usize]
        }
    }

    fn set_parameter(&mut self, n: usize, value: i64) {
        let address_type = (self.memory[self.ip] / 10_i32.pow(n as u32 + 1) as i64) % 10;
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
    let opcodes: Vec<i64> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    run_without_money(&opcodes);
    play_the_game(&opcodes);
}

fn run_without_money(opcodes: &Vec<i64>) {
    let mut program = IntcodeProgram::from(&opcodes, 1024 * 64);
    program.run_until_halt();
    println!("Block tiles: {}", count_block_tiles(&program.outputs, 0));
}

fn count_block_tiles(actions: &VecDeque<i64>, index: usize) -> i64 {
    if let Some(tile) = actions.get(index + 2) {
        let current_tile = if *tile == 2 { 1 } else { 0 };
        count_block_tiles(actions, index + 3) + current_tile
    } else {
        0
    }
}

fn play_the_game(opcodes: &Vec<i64>) {
    let mut opcodes = opcodes.clone();
    opcodes[0] = 2;

    let mut program = IntcodeProgram::from(&opcodes, 1024 * 64);

    let mut paddle_x = 0;
    let mut ball_x = 0;
    while program.run_until_input() {
        if let Some(new_paddle_pos) = get_element_position(&program.outputs, 3) {
            paddle_x = new_paddle_pos.0;
        }
        if let Some(new_ball_pos) = get_element_position(&program.outputs, 4) {
            ball_x = new_ball_pos.0;
        }
        program.outputs.clear();
        program.inputs.push_back(if paddle_x == ball_x { 0 } else if paddle_x < ball_x { 1 } else { -1 });
    }

    if let Some(score) = get_score(&program.outputs) {
        println!("Final score: {}", score);
    }
}

fn get_element_position(program_outputs: &VecDeque<i64>, element_type: i64) -> Option<(i64, i64)> {
    for i in (0..program_outputs.len()).step_by(3) {
        if program_outputs[i + 2] == element_type {
            return Some((program_outputs[i], program_outputs[i + 1]));
        }
    }
    None
}

fn get_score(program_outputs: &VecDeque<i64>) -> Option<i64> {
    for i in (0..program_outputs.len()).step_by(3) {
        if program_outputs[i] == -1 {
            return Some(program_outputs[i + 2]);
        }
    }
    None
}
