fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let movements: Vec<(char, i32)> = input.lines().map(|x|
        (x.chars().nth(0).unwrap(), x[1..].parse().unwrap())
    ).collect();

    compute_ship_movement(&movements);
    compute_ship_and_waypoint_movements(&movements);
}

fn compute_ship_movement(movements: &Vec<(char, i32)>) {
    let mut ship_position = (0, 0);
    let mut ship_direction = 'E';

    for movement in movements {
        match movement.0 {
            'N' | 'S' | 'E' | 'W' => apply_movement(movement, &mut ship_position),
            'L' => apply_rotation_to_direction(360 - movement.1, &mut ship_direction),
            'R' => apply_rotation_to_direction(movement.1, &mut ship_direction),
            'F' => apply_movement(&(ship_direction, movement.1), &mut ship_position),
            _ => unreachable!(),
        }
    }

    println!("Final position: {:?} | Distance: {}", ship_position, ship_position.0.abs() + ship_position.1.abs());
}

fn compute_ship_and_waypoint_movements(movements: &Vec<(char, i32)>) {
    let mut ship_position = (0, 0);
    let mut waypoint_position = (10, -1);

    for movement in movements {
        match movement.0 {
            'N' | 'S' | 'E' | 'W' => apply_movement(movement, &mut waypoint_position),
            'L' => apply_rotation_to_position(360 - movement.1, &mut waypoint_position),
            'R' => apply_rotation_to_position(movement.1, &mut waypoint_position),
            'F' => {
                ship_position.0 += waypoint_position.0 * movement.1;
                ship_position.1 += waypoint_position.1 * movement.1;
            },
            _ => unreachable!(),
        }
    }

    println!("Final position: {:?} | Distance: {}", ship_position, ship_position.0.abs() + ship_position.1.abs());
}

fn apply_movement(movement: &(char, i32), position: &mut (i32, i32)) {
    match movement.0 {
        'N' => position.1 -= movement.1,
        'S' => position.1 += movement.1,
        'E' => position.0 += movement.1,
        'W' => position.0 -= movement.1,
        _ => unreachable!(),
    }
}

fn apply_rotation_to_direction(angle: i32, direction: &mut char) {
    let new_angle = (angle_from_direction(*direction) + angle) % 360;
    *direction = direction_from_angle(new_angle);
}

fn angle_from_direction(direction: char) -> i32 {
    match direction {
        'E' => 0,
        'S' => 90,
        'W' => 180,
        'N' => 270,
        _ => unreachable!(),
    }
}

fn direction_from_angle(angle: i32) -> char {
    match angle {
        0 => 'E',
        90 => 'S',
        180 => 'W',
        270 => 'N',
        _ => unreachable!(),
    }
}

fn apply_rotation_to_position(angle: i32, position: &mut (i32, i32)) {
    let x = position.0 * get_cos(angle) - position.1 * get_sin(angle);
    let y = position.1 * get_cos(angle) + position.0 * get_sin(angle);

    position.0 = x;
    position.1 = y;
}

fn get_sin(degrees: i32) -> i32 {
    match degrees {
        0 => 0,
        90 => 1,
        180 => 0,
        270 => -1,
        _ => unimplemented!(),
    }
}

fn get_cos(degrees: i32) -> i32 {
    get_sin((degrees + 90) % 360)
}
