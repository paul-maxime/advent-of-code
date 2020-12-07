use std::collections::HashMap;
use std::collections::HashSet;
use regex::Regex;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let bags = parse_bags(&input);

    println!("Bags containing a shiny gold bag: {}", get_nb_containers(&bags, "shiny gold"));
    println!("Bags inside a shiny gold bag: {}", get_nb_inner_bags(&bags, "shiny gold") - 1);
}

fn parse_bags(input: &str) -> HashMap<String, Vec<(String, usize)>> {
    let mut bags: HashMap<String, Vec<(String, usize)>> = HashMap::new();
    let lines: Vec<&str> = input.lines().collect();
    let regex = Regex::new(r"(?:(\d+) )?([a-z]+ [a-z]+) bag").unwrap();

    for line in lines {
        let mut is_first = true;
        let mut name = "".to_string();
        let mut elements = vec![];
        for cap in regex.captures_iter(line) {
            if is_first {
                name = cap[2].to_string();
            } else if cap.get(1).is_some() {
                elements.push((cap[2].to_string(), cap[1].parse().unwrap()));
            }
            is_first = false;
        }
        bags.insert(name, elements);
    }

    bags
}

fn get_nb_containers(bags: &HashMap<String, Vec<(String, usize)>>, bag_type: &str) -> usize {
    let mut good_containers: HashSet<&str> = HashSet::new();

    loop {
        let length_before = good_containers.len();
        for bag in bags.iter() {
            if bag.1.iter().any(|x| x.0.as_str() == bag_type || good_containers.contains(x.0.as_str())) {
                good_containers.insert(bag.0.as_str());
            }
        }
        if length_before == good_containers.len() {
            break;
        }
    }

    good_containers.len()
}

fn get_nb_inner_bags(bags: &HashMap<String, Vec<(String, usize)>>, bag_type: &str) -> usize {
    bags.get(bag_type).unwrap().iter().map(|x| get_nb_inner_bags(&bags, x.0.as_str()) * x.1).sum::<usize>() + 1
}
