// Day 6: Custom Customs

use std::collections::HashSet;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let groups: Vec<&str> = input.split("\n\n").collect();

    println!("All unique answers: {}", groups.iter().map(|x| get_unique_answers(x).len()).sum::<usize>());
    println!("Unanimous answers: {}", groups.iter().map(|x| get_unanimous_answers(x)).sum::<usize>());
}

fn get_unique_answers(group: &str) -> HashSet<char> {
    group.chars().filter(|&c| c.is_alphabetic()).collect()
}

fn get_unanimous_answers(group: &str) -> usize {
    let people: Vec<&str> = group.lines().collect();
    get_unique_answers(group).iter().filter(|&&c| people.iter().all(|&p| p.contains(c))).count()
}
