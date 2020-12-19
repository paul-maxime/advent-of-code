use std::collections::HashMap;
use rand::seq::SliceRandom;

#[derive(Debug)]
enum Rule {
    Character(char),
    Single(Vec<u32>),
    Multiple(Vec<Vec<u32>>),
}

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    let mut rules = parse_rules(&input);
    let words: Vec<&str> = input.lines().filter(|x| x.len() > 0 && !x.contains(':')).collect();

    println!("Valid messages before fix: {}", words.iter().filter(|x| match_root_rule(&rules, x)).count());

    rules.insert(8, Rule::Multiple(vec![vec![42], vec![42, 8]]));
    rules.insert(11, Rule::Multiple(vec![vec![42, 31], vec![42, 11, 31]]));

    println!("Valid messages after fix: {}", words.iter().filter(|x| try_to_match_a_thousand_times(&rules, x)).count());
}

fn parse_rules(input: &str) -> HashMap<u32, Rule> {
    let mut rules: HashMap<u32, Rule> = HashMap::new();

    for line in input.lines().filter(|x| x.contains(':')) {
        let parts: Vec<&str> = line.split(':').collect();
        let id = parts[0].parse().unwrap();

        if parts[1].contains('"') {
            rules.insert(id, Rule::Character(parts[1].chars().nth(2).unwrap()));
        } else if parts[1].contains('|') {
            let sub_rules: Vec<Vec<u32>> = parts[1]
                .split('|')
                .map(|part| part.trim().split(' ').map(|x| x.parse().unwrap()).collect())
                .collect();
            rules.insert(id, Rule::Multiple(sub_rules));
        } else {
            let sub_rules = parts[1].trim().split(' ').map(|x| x.parse().unwrap()).collect();
            rules.insert(id, Rule::Single(sub_rules));
        }
    }

    rules
}

fn try_to_match_a_thousand_times(rules: &HashMap<u32, Rule>, word: &str) -> bool {
    for _ in 0..1000 {
        if match_root_rule(rules, word) {
            return true;
        }
    }
    false
}

fn match_root_rule(rules: &HashMap<u32, Rule>, word: &str) -> bool {
    let mut index = 0;
    match_rule(rules, 0, word, &mut index, 0) && index == word.len()
}

fn match_rule(rules: &HashMap<u32, Rule>, rule_id: u32, word: &str, index: &mut usize, depth: u32) -> bool {
    if depth > 100 {
        return false;
    }
    match rules.get(&rule_id).unwrap() {
        Rule::Character(c) => match_character(*c, word, index),
        Rule::Single(sub_rules) => match_single(rules, sub_rules, word, index, depth + 1),
        Rule::Multiple(sub_rules) => match_multiple(rules, sub_rules, word, index, depth + 1),
    }
}

fn match_character(character: char, word: &str, index: &mut usize) -> bool {
    if let Some(c) = word.chars().nth(*index) {
        if c == character {
            *index += 1;
            return true;
        }
    }
    false
}

fn match_single(rules: &HashMap<u32, Rule>, rule_ids: &Vec<u32>, word: &str, index: &mut usize, depth: u32) -> bool {
    let saved_index = *index;
    for rule_id in rule_ids {
        if !match_rule(rules, *rule_id, word, index, depth) {
            *index = saved_index;
            return false;
        }
    }
    true
}

fn match_multiple(rules: &HashMap<u32, Rule>, sub_rules: &Vec<Vec<u32>>, word: &str, index: &mut usize, depth: u32) -> bool {
    let saved_index = *index;
    let mut possible_indexes: Vec<usize> = vec![];
    for rule_ids in sub_rules {
        if match_single(rules, rule_ids, word, index, depth) {
            possible_indexes.push(*index);
            *index = saved_index;
        }
    }
    if possible_indexes.len() > 0 {
        *index = *possible_indexes.choose(&mut rand::thread_rng()).unwrap();
        true
    } else {
        false
    }
}
