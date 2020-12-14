use std::collections::HashMap;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap()
        .replace("mem[", "")
        .replace("]", "");

    let instructions: Vec<(&str, &str)> = input.lines()
        .map(|x| x.split(" = ").collect())
        .map(|x: Vec<&str>| (x[0], x[1]))
        .collect();

    execute_program_v1(&instructions);
    execute_program_v2(&instructions);
}

fn execute_program_v1(instructions: &Vec<(&str, &str)>) {
    let mut memory: HashMap<u64, u64> = HashMap::new();
    let mut mask = (0u64, 0u64);

    for instruction in instructions {
        if instruction.0 == "mask" {
            mask = parse_mask_v1(instruction.1);
        } else {
            let address: u64 = instruction.0.parse().unwrap();
            let value: u64 = instruction.1.parse().unwrap();
            memory.insert(address, (value | mask.0) & mask.1);
        }
    }

    println!("Final sum: {}", memory.iter().map(|x| x.1).sum::<u64>());
}

fn parse_mask_v1(mask: &str) -> (u64, u64) {
    let or_mask = u64::from_str_radix(&mask.replace("X", "0"), 2).unwrap();
    let and_mask = u64::from_str_radix(&mask.replace("X", "1"), 2).unwrap();
    (or_mask, and_mask)
}

fn execute_program_v2(instructions: &Vec<(&str, &str)>) {
    let mut memory: HashMap<u64, u64> = HashMap::new();
    let mut mask = (0u64, 0u64);

    for instruction in instructions {
        if instruction.0 == "mask" {
            mask = parse_mask_v2(instruction.1);
        } else {
            let address: u64 = instruction.0.parse::<u64>().unwrap() | mask.0;
            let value: u64 = instruction.1.parse().unwrap();

            insert_recursive(&mut memory, address, value, mask.1, 0);
        }
    }

    println!("Final sum: {}", memory.iter().map(|x| x.1).sum::<u64>());
}

fn parse_mask_v2(mask: &str) -> (u64, u64) {
    let or_mask = u64::from_str_radix(&mask.replace("X", "0"), 2).unwrap();
    let float_mask = u64::from_str_radix(&mask.replace("1", "0").replace("X", "1"), 2).unwrap();
    (or_mask, float_mask)
}

fn insert_recursive(memory: &mut HashMap<u64, u64>, address: u64, value: u64, mask: u64, index: u64) {
    if index < 36 {
        if (mask & (1 << index)) > 0 {
            insert_recursive(memory, address | (1 << index), value, mask, index + 1);
            insert_recursive(memory, address & !(1 << index), value, mask, index + 1);
        } else {
            insert_recursive(memory, address, value, mask, index + 1);
        }
    } else {
        memory.insert(address, value);
    }
}
