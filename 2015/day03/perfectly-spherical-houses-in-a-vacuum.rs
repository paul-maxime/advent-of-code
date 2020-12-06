use std::collections::HashSet;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    println!("{}", distribute_santa(&input));
    println!("{}", distribute_santa_and_bot(&input));
}

fn distribute_santa(input: &str) -> usize {
    let mut houses: HashSet<(i32, i32)> = HashSet::new();
    let mut position = (0, 0);

    houses.insert(position);
    for direction in input.chars() {
        move_character(&mut position, direction);
        houses.insert(position);
    }

    houses.len()
}

fn distribute_santa_and_bot(input: &str) -> usize {
    let mut houses: HashSet<(i32, i32)> = HashSet::new();
    let mut santa_position = (0, 0);
    let mut bot_position = (0, 0);
    let mut santa_moving = true;

    houses.insert(santa_position);
    for direction in input.chars() {
        let position: &mut (i32, i32) = if santa_moving { &mut santa_position } else { &mut bot_position };
        move_character(position, direction);
        houses.insert(*position);
        santa_moving = !santa_moving;
    }

    houses.len()
}

fn move_character(position: &mut (i32, i32), direction: char) {
    match direction {
        '>' => position.0 += 1,
        '<' => position.0 -= 1,
        'v' => position.1 += 1,
        '^' => position.1 -= 1,
        _ => (),
    }
}
