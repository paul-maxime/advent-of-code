fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    let mut floor = 0;
    let mut position = 0;
    let mut basement = 0;
    for c in input.chars() {
        position += 1;
        floor += if c == '(' { 1 } else { -1 };
        if floor == -1 && basement == 0 {
            basement = position
        }
    }

    std::println!("Final floor: {}", floor);
    std::println!("Basement position: {}", basement);
}
