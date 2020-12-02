use std::fs;
use regex::Regex;

fn main() {
    let input = fs::read_to_string("./input").unwrap();

    let regex = Regex::new(r"(\d+)-(\d+) ([a-z]): ([a-z]+)").unwrap();

    first_policy(&input, &regex);
    second_policy(&input, &regex);
}

fn first_policy(input: &String, regex: &Regex) {
    let mut valid_passwords = 0;
    for cap in regex.captures_iter(&input) {
        let min: usize = cap[1].parse().unwrap();
        let max: usize = cap[2].parse().unwrap();
        let letter = cap[3].chars().nth(0).unwrap();
        let password = &cap[4];
        let occurrences = password.chars().filter(|&c| c == letter).count();

        if min <= occurrences && occurrences <= max {
            valid_passwords += 1;
        }
    }

    println!("First policy: {} valid passwords.", valid_passwords);
}

fn second_policy(input: &String, regex: &Regex) {
    let mut valid_passwords = 0;
    for cap in regex.captures_iter(&input) {
        let pos_1: usize = cap[1].parse().unwrap();
        let pos_2: usize = cap[2].parse().unwrap();
        let letter = cap[3].chars().nth(0).unwrap();
        let password = &cap[4];

        let char_1 = password.chars().nth(pos_1 - 1).unwrap();
        let char_2 = password.chars().nth(pos_2 - 1).unwrap();

        if (char_1 == letter && char_2 != letter) || (char_1 != letter && char_2 == letter) {
            valid_passwords += 1;
        }
    }

    println!("Second policy: {} valid passwords.", valid_passwords);
}
