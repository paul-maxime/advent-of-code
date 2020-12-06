use std::fs;

fn main() {
    let input = fs::read_to_string("./input").unwrap();
    let lines: Vec<&str> = input.lines().collect();

    println!("{} nice strings", lines.iter().filter(|x| is_nice(x)).count());
    println!("{} very nice strings", lines.iter().filter(|x| is_very_nice(x)).count());
}

fn is_nice(line: &str) -> bool {
    line.chars().filter(is_vowel).count() >= 3 &&
        has_duplicate_consecutive(line) &&
        !line.contains("ab") &&
        !line.contains("cd") &&
        !line.contains("pq") &&
        !line.contains("xy")
}

fn has_duplicate_consecutive(line: &str) -> bool {
    let mut previous = line.chars().nth(0).unwrap();
    for c in line.chars().skip(1) {
        if c == previous {
            return true;
        }
        previous = c;
    }
    false
}

fn is_vowel(c: &char) -> bool {
    match c {
        'a' | 'e' | 'i' | 'o' | 'u' => true,
        _ => false
    }
}

fn is_very_nice(line: &str) -> bool {
    has_pair_of_two_letters(line) && has_repeat_with_middle(line)
}

fn has_pair_of_two_letters(line: &str) -> bool {
    line[2..].contains(&line[0..2]) || if line.len() > 2 { has_pair_of_two_letters(&line[1..]) } else { false }
}

fn has_repeat_with_middle(line: &str) -> bool {
    if line.len() < 3 { false }
    else { line.chars().nth(0).unwrap() == line.chars().nth(2).unwrap() || has_repeat_with_middle(&line[1..]) }
}
