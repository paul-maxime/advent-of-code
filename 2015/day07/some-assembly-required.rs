use std::collections::HashMap;
use regex::Regex;

#[derive(Debug)]
struct Instruction {
    operation: String,
    left: String,
    right: String,
}

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let regex = Regex::new(r"([a-z0-9]+)? ?([A-Z]+)? ?([a-z0-9]+)? \-> ([a-z]+)").unwrap();

    let mut instructions: HashMap<String, Instruction> = HashMap::new();
    let mut cache: HashMap<String, u16> = HashMap::new();

    for cap in regex.captures_iter(&input) {
        let instruction = Instruction {
            left: String::from(if let Some(s) = cap.get(1) { s.as_str() } else { "" }),
            right: String::from(if let Some(s) = cap.get(3) { s.as_str() } else { "" }),
            operation: String::from(if let Some(s) = cap.get(2) { s.as_str() } else { "" }),
        };
        instructions.insert(String::from(&cap[4]), instruction);
    }

    let first_result = get_variable(&instructions, &mut cache, "a");
    println!("First iteration: {}", first_result);

    cache.clear();
    cache.insert(String::from("b"), first_result);
    let second_result = get_variable(&instructions, &mut cache, "a");
    println!("Second iteration: {}", second_result);
}

fn get_variable(program: &HashMap<String, Instruction>, cache: &mut HashMap<String, u16>, variable: &str) -> u16 {
    if let Some(&cached_value) = cache.get(variable) {
        return cached_value;
    }

    if let Ok(numeric_value) = variable.parse::<u16>() {
        return numeric_value;
    }

    let instruction = program.get(variable).unwrap();
    let result = match instruction.operation.as_str() {
        "AND" => get_variable(program, cache, &instruction.left) & get_variable(program, cache, &instruction.right),
        "OR" => get_variable(program, cache, &instruction.left) | get_variable(program, cache, &instruction.right),
        "NOT" => !get_variable(program, cache, &instruction.right),
        "LSHIFT" => get_variable(program, cache, &instruction.left) << get_variable(program, cache, &instruction.right),
        "RSHIFT" => get_variable(program, cache, &instruction.left) >> get_variable(program, cache, &instruction.right),
        _ => get_variable(program, cache, &instruction.left),
    };

    cache.insert(String::from(variable), result);
    result
}
