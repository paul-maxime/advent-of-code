use std::collections::HashSet;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let instructions: Vec<(&str, i32)> = input.lines().map(|x| {
        let elements: Vec<&str> = x.split(" ").collect();
        (elements[0], elements[1].parse().unwrap())
    }).collect();

    let (_, acc) = execute_program(&instructions, -1);
    println!("Corrupted boot code: {}", acc);

    for i in 0..instructions.len()+1 {
        let (ir, acc) = execute_program(&instructions, i as i32);
        if ir == instructions.len() as i32 {
            println!("Fixed boot code: {}", acc);
        }
    }
}

fn execute_program(instructions: &Vec<(&str, i32)>, swap_at: i32) -> (i32, i32) {
    let mut already_executed: HashSet<i32> = HashSet::new();
    let mut ir: i32 = 0;
    let mut acc: i32 = 0;

    loop {
        if already_executed.contains(&ir) || ir < 0 || ir >= instructions.len() as i32 {
            break;
        }
        already_executed.insert(ir);
        let current_instruction = instructions[ir as usize];
        match current_instruction.0 {
            "acc" => {
                acc += current_instruction.1;
                ir += 1;
            },
            "jmp" => {
                ir += if swap_at == ir { 1 } else { current_instruction.1 };
            },
            "nop" => {
                ir += if swap_at != ir { 1 } else { current_instruction.1 };
            }
            _ => ()
        }
    }

    (ir, acc)
}
