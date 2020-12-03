fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let map: Vec<&str> = input.lines().collect();

    println!("First slope: {} trees", check_slope(&map, 3, 1));
    println!("All slopes: {}",
        check_slope(&map, 1, 1) *
        check_slope(&map, 3, 1) *
        check_slope(&map, 5, 1) *
        check_slope(&map, 7, 1) *
        check_slope(&map, 1, 2)
    );
}

fn check_slope(map: &Vec<&str>, dx: usize, dy: usize) -> usize {
    let map_height = map.len();
    let map_width = map[0].len();

    let mut x = 0;
    let mut y = 0;
    let mut trees = 0;

    loop {
        x += dx;
        y += dy;

        if y >= map_height {
            break;
        }

        if map[y].chars().nth(x % map_width).unwrap() == '#' {
            trees += 1;
        }
    }

    trees
}
