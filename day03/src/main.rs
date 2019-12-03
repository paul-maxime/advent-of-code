use std::collections::HashMap;
use std::fs;

#[derive(Clone, Debug)]
struct Point {
    x: i32,
    y: i32,
    distance_from_start: i32,
}

impl Point {
    fn is_zero(&self) -> bool {
        self.x == 0 && self.y == 0
    }

    fn distance_from_origin(&self) -> i32 {
        self.x.abs() + self.y.abs()
    }

    fn hash_code(&self) -> i32 {
        return self.x * 10000 + self.y;
    }
}

#[derive(Debug)]
enum Direction {
    Up,
    Down,
    Left,
    Right,
}

impl Direction {
    fn from(c: char) -> Direction {
        match c {
            'U' => Direction::Up,
            'D' => Direction::Down,
            'L' => Direction::Left,
            'R' => Direction::Right,
            _ => panic!("unexpected direction")
        }
    }
}

#[derive(Debug)]
struct Movement {
    direction: Direction,
    distance: i32,
}

impl Movement {
    fn from(str: &str) -> Movement {
        Movement {
            direction: Direction::from(str.chars().nth(0).unwrap()),
            distance: str[1..].parse().unwrap(),
        }
    }
}

#[derive(Debug)]
struct Path {
    movements: Vec<Movement>,
}

impl Path {
    fn from(str: &str) -> Path {
        Path {
            movements: str.split(",")
                .map(|x| Movement::from(x))
                .collect()
        }
    }

    fn get_points(&self) -> HashMap<i32, Point> {
        let mut pos = Point { x: 0, y: 0, distance_from_start: 0 };
        let mut hashes = HashMap::new();
        hashes.insert(pos.hash_code(), pos.clone());
        for movement in &self.movements {
            for _ in 0..movement.distance {
                match movement.direction {
                    Direction::Up => pos.y -= 1,
                    Direction::Down => pos.y += 1,
                    Direction::Left => pos.x -= 1,
                    Direction::Right => pos.x += 1,
                }
                pos.distance_from_start += 1;
                if !hashes.contains_key(&(pos.hash_code())) {
                    hashes.insert(pos.hash_code(), pos.clone());
                }
            }
        }
        hashes
    }
}

fn main() {
    let paths: Vec<Path> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .lines()
        .map(|x| Path::from(x))
        .collect();

    let points_a = paths[0].get_points();
    let points_b = paths[1].get_points();
    let mut intersections = Vec::new();

    for p in points_a {
        if points_b.contains_key(&p.0) && !p.1.is_zero() {
            let p2 = points_b.get(&p.0).unwrap();
            let d = p.1.distance_from_start + p2.distance_from_start;
            intersections.push((p.1, d));
        }
    }

    print_nearest_from_origin(&intersections);
    print_nearest_from_start(&intersections);
}

fn print_nearest_from_origin(intersections: &Vec<(Point, i32)>) {
    let nearest_point = intersections.iter()
        .min_by_key(|x| (x.0).distance_from_origin());

    match nearest_point {
        Some(p) => println!("({}, {}) -> distance from origin = {}", (p.0).x, (p.0).y, (p.0).distance_from_origin()),
        None => panic!("no intersection point found")
    }

}

fn print_nearest_from_start(intersections: &Vec<(Point, i32)>) {
    let nearest_point = intersections.iter()
        .min_by_key(|x| x.1);

    match nearest_point {
        Some(p) => println!("({}, {}) -> distance from start = {}", (p.0).x, (p.0).y, p.1),
        None => panic!("no intersection point found")
    }
}
