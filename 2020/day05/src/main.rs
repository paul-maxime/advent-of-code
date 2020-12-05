fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let seats: Vec<&str> = input.lines().collect();

    let mut seat_ids: Vec<i32> = seats.iter().map(|x| compute_seat_id(x)).collect();
    seat_ids.sort();

    println!("Highest seat ID: {}", seat_ids.last().unwrap());
    println!("Missing seat ID: {}", seat_ids.iter().fold(0, |a, b| if a == 0 || a == b - 1 { *b } else { a }) + 1);
}

fn compute_seat_id(seat: &str) -> i32 {
    let row = seat[..7].chars().fold(0, |i, c| i << 1 | if c == 'B' { 1 } else { 0 });
    let column = seat[7..].chars().fold(0, |i, c| i << 1 | if c == 'R' { 1 } else { 0 });
    row * 8 + column
}
