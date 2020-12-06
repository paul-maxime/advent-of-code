use regex::Regex;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let regex = Regex::new(r"(turn on|turn off|toggle) ([\d]+),([\d]+) through ([\d]+),([\d]+)").unwrap();

    println!("{}", compute_lights(&input, &regex));
    println!("{}", compute_brightness(&input, &regex));
}

fn compute_lights(input: &str, regex: &Regex) -> usize {
    let mut lights = [[false; 1000] ; 1000];

    for cap in regex.captures_iter(input) {
        let (x1, y1, x2, y2) = parse_coordinates(&cap);
        for x in x1..x2+1 {
            for y in y1..y2+1 {
                match &cap[1] {
                    "turn on" => lights[x][y] = true,
                    "turn off" => lights[x][y] = false,
                    "toggle" => lights[x][y] = !lights[x][y],
                    _ => (),
                }
            }
        }
    }

    lights.iter().map(|x| x.iter().filter(|&y| *y == true).count()).sum()
}

fn compute_brightness(input: &str, regex: &Regex) -> i32 {
    let mut lights = [[0; 1000] ; 1000];

    for cap in regex.captures_iter(input) {
        let (x1, y1, x2, y2) = parse_coordinates(&cap);
        for x in x1..x2+1 {
            for y in y1..y2+1 {
                match &cap[1] {
                    "turn on" => lights[x][y] += 1,
                    "turn off" => if lights[x][y] > 0 { lights[x][y] -= 1 },
                    "toggle" => lights[x][y] += 2,
                    _ => (),
                }
            }
        }
    }

    lights.iter().map(|x| x.iter().sum::<i32>()).sum()
}

fn parse_coordinates(cap: &regex::Captures) -> (usize, usize, usize, usize) {
    let x1: usize = cap[2].parse().unwrap();
    let y1: usize = cap[3].parse().unwrap();
    let x2: usize = cap[4].parse().unwrap();
    let y2: usize = cap[5].parse().unwrap();
    (x1, y1, x2, y2)
}
