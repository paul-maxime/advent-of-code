fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let seats: Vec<Vec<char>> = input.lines().map(|x| x.chars().collect()).collect();

    println!("Fake simulation: {:?}", simulate_people_seating(&seats, false));
    println!("Real simulation: {:?}", simulate_people_seating(&seats, true));
}

fn simulate_people_seating(original_seats: &Vec<Vec<char>>, is_real_simulation: bool) -> i32 {
    let mut seats = original_seats.clone();
    loop {
        let (output, occupied_seats, has_changed) = simulate_single_round(&seats, is_real_simulation);
        if !has_changed {
            return occupied_seats;
        }
        seats = output;
    }
}

fn simulate_single_round(seats: &Vec<Vec<char>>, is_real_simulation: bool) -> (Vec<Vec<char>>, i32, bool) {
    let mut output = seats.clone();
    let mut occupied_seats = 0;
    let mut has_changed = false;

    let tolerance = if is_real_simulation { 5 } else { 4 };

    for y in 0..seats.len() {
        for x in 0..seats[y].len() {
            let adjacent_seats = if is_real_simulation {
                count_visible_seats(seats, x as i32, y as i32, 0, 0)
            } else {
                count_adjacent_seats(seats, x as i32, y as i32)
            };
            if seats[y][x] == 'L' && adjacent_seats == 0 {
                output[y][x] = '#';
                has_changed = true;
            }
            if seats[y][x] == '#' && adjacent_seats >= tolerance {
                output[y][x] = 'L';
                has_changed = true;
            }
            if output[y][x] == '#' {
                occupied_seats += 1;
            }
       }
    }

    (output, occupied_seats, has_changed)
}

fn count_adjacent_seats(seats: &Vec<Vec<char>>, x: i32, y: i32) -> i32 {
    let mut adjacent_seats = 0;
    for dx in -1..=1 {
        for dy in -1..=1 {
            if dx != 0 || dy != 0 {
                if let Some(c) = get_cell(seats, x + dx, y + dy) {
                    if c == '#' {
                        adjacent_seats += 1;
                    }
                }
            }
        }
    }
    adjacent_seats
}

fn count_visible_seats(seats: &Vec<Vec<char>>, x: i32, y: i32, dx: i32, dy: i32) -> i32 {
    if dx == 0 && dy == 0 {
        count_visible_seats(seats, x, y, dx - 1, dy - 1) +
        count_visible_seats(seats, x, y, dx - 1, dy - 0) +
        count_visible_seats(seats, x, y, dx - 1, dy + 1) +
        count_visible_seats(seats, x, y, dx - 0, dy - 1) +
        count_visible_seats(seats, x, y, dx - 0, dy + 1) +
        count_visible_seats(seats, x, y, dx + 1, dy - 1) +
        count_visible_seats(seats, x, y, dx + 1, dy - 0) +
        count_visible_seats(seats, x, y, dx + 1, dy + 1)
    } else {
        if let Some(c) = get_cell(seats, x + dx, y + dy) {
            match c {
                'L' => 0,
                '#' => 1,
                _ => count_visible_seats(seats, x + dx, y + dy, dx, dy),
            }
        } else {
            0
        }
    }
}

fn get_cell(seats: &Vec<Vec<char>>, x: i32, y: i32) -> Option<char> {
    if x < 0 || y < 0 {
        return None;
    }
    if let Some(line) = seats.get(y as usize) {
        if let Some(c) = line.get(x as usize) {
            return Some(*c);
        }
    }
    None
}
