use std::collections::HashMap;
use std::collections::HashSet;
use regex::Regex;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let (distances, cities) = parse_cities(&input);

    let min_distance = pathfind_distance(&distances, cities.iter().collect(), "".to_string(), false);
    println!("Min distance: {}", min_distance);

    let max_distance = pathfind_distance(&distances, cities.iter().collect(), "".to_string(), true);
    println!("Max distance: {}", max_distance);
}

fn parse_cities(input: &str) -> (HashMap<(String, String), u32>, HashSet<String>) {
    let regex = Regex::new(r"(\w+) to (\w+) = (\d+)").unwrap();

    let mut distances: HashMap<(String, String), u32> = HashMap::new();
    let mut cities: HashSet<String> = HashSet::new();

    for cap in regex.captures_iter(&input) {
        let city1 = cap[1].to_string();
        let city2 = cap[2].to_string();
        let distance: u32 = cap[3].parse().unwrap();

        distances.insert((city1.clone(), city2.clone()), distance);
        distances.insert((city2.clone(), city1.clone()), distance);

        cities.insert(city1.clone());
        cities.insert(city2.clone());
    }

    (distances, cities)
}

fn pathfind_distance(
    distances: &HashMap<(String, String), u32>,
    remaining_cities: Vec<&String>,
    current_city: String,
    is_max: bool
) -> u32 {
    if remaining_cities.len() == 0 {
        return 0;
    }

    let mut best_distance = 0;
    for city in remaining_cities.clone() {
        let mut new_remaining_cities: Vec<&String> = remaining_cities.clone();
        new_remaining_cities.retain(|x| *x != city);

        let current_distance = if current_city == "" { 0 } else { *distances.get(&(city.clone(), current_city.clone())).unwrap() };
        let tail_distance = pathfind_distance(distances, new_remaining_cities, city.clone(), is_max);
        let full_distance = current_distance + tail_distance;

        if best_distance == 0 || (is_max && full_distance > best_distance) || (!is_max && full_distance < best_distance) {
            best_distance = full_distance;
        }
    }

    best_distance
}
