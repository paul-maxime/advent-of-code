use std::collections::HashSet;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let cubes = parse_initial_state(&input);

    println!("3d boot process: {}", execute_boot_process(&cubes, false));
    println!("4d boot process: {}", execute_boot_process(&cubes, true));
}

fn parse_initial_state(input: &str) -> HashSet<(i32, i32, i32, i32)> {
    let mut cubes = HashSet::new();

    for (y, line) in input.lines().enumerate() {
        for (x, c) in line.chars().enumerate() {
            if c == '#' {
                cubes.insert((x as i32, y as i32, 0, 0));
            }
        }
    }

    cubes
}

fn execute_boot_process(initial_cubes: &HashSet<(i32, i32, i32, i32)>, is_4d: bool) -> usize {
    let mut cubes = initial_cubes.clone();

    for _ in 0..6 {
        cubes = execute_cycle(&cubes, is_4d);
    }

    cubes.len()
}

fn execute_cycle(old_cubes: &HashSet<(i32, i32, i32, i32)>, is_4d: bool) -> HashSet<(i32, i32, i32, i32)> {
    let mut new_cubes = HashSet::new();

    let all_adjacent_cubes: HashSet<(i32, i32, i32, i32)> = old_cubes.iter().map(|cube| get_all_nearby_cubes(cube, is_4d)).flatten().collect();

    for cube in all_adjacent_cubes {
        let is_activated = old_cubes.contains(&cube);
        let neighbors = count_activated_neighbors(old_cubes, &cube);
        if is_activated && (neighbors == 2 || neighbors == 3) {
            new_cubes.insert(cube);
        } else if !is_activated && neighbors == 3 {
            new_cubes.insert(cube);
        }
    }

    new_cubes
}

fn get_all_nearby_cubes(cube: &(i32, i32, i32, i32), is_4d: bool) -> HashSet<(i32, i32, i32, i32)> {
    let mut adjacent_cubes = HashSet::new();

    for x in cube.0 - 1 ..= cube.0 + 1 {
        for y in cube.1 - 1 ..= cube.1 + 1 {
            for z in cube.2 - 1 ..= cube.2 + 1 {
                if is_4d {
                    for w in cube.3 -1 ..= cube.3 + 1 {
                        adjacent_cubes.insert((x, y, z, w));
                    }
                } else {
                    adjacent_cubes.insert((x, y, z, 0));
                }
            }
        }
    }

    adjacent_cubes
}

fn count_activated_neighbors(cubes: &HashSet<(i32, i32, i32, i32)>, cube: &(i32, i32, i32, i32)) -> i32 {
    let mut count = 0;
    for x in cube.0 - 1 ..= cube.0 + 1 {
        for y in cube.1 - 1 ..= cube.1 + 1 {
            for z in cube.2 - 1 ..= cube.2 + 1 {
                for w in cube.3 -1 ..= cube.3 + 1 {
                    if (x, y, z, w) != *cube && cubes.contains(&(x, y, z, w)) {
                        count += 1;
                    }
                }
            }
        }
    }
    count
}
