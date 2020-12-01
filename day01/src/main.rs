use std::fs;

fn main() {
    let entries: Vec<i32> = fs::read_to_string("./input")
        .unwrap()
        .lines()
        .map(|x| x.parse().unwrap())
        .collect();

    find_two(&entries);
    find_three(&entries);
}

fn find_two(entries: &Vec<i32>) {
    for a in entries {
        for b in entries {
            if a + b == 2020 {
                println!("{} x {} = {}", a, b, a * b);
                return;
            }
        }
    }
}

fn find_three(entries: &Vec<i32>) {
    for a in entries {
        for b in entries {
            for c in entries {
                if a + b + c == 2020 {
                    println!("{} x {} x {} = {}", a, b, c, a * b * c);
                    return;
                }
            }
        }
    }
}
