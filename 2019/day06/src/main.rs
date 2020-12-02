use std::fs;
use std::collections::HashSet;
use std::collections::HashMap;

fn main() {
    let orbits: HashMap<String, String> = fs::read_to_string("./input")
        .unwrap()
        .lines()
        .map(|x| x.split(")").collect())
        .map(|x: Vec<&str>| { (String::from(x[1]), String::from(x[0])) })
        .collect();

    let planet_links = compute_links(&orbits);

    println!("{}", count_orbits(&orbits));
    println!("{}", find_santa(&planet_links, &mut HashSet::new(), "YOU") - 2);
}

fn count_orbits(orbits: &HashMap<String, String>) -> i32 {
    orbits.iter().map(|x| count_orbits_of_planet(orbits, x.0)).sum()
}

fn count_orbits_of_planet(orbits: &HashMap<String, String>, planet: &String) -> i32 {
    match orbits.get(planet) {
        Some(parent) => count_orbits_of_planet(orbits, parent) + 1,
        None => 0
    }
}

fn compute_links(orbits: &HashMap<String, String>) -> HashMap<String, Vec<String>> {
    let all_planets: HashSet<String> = orbits
        .iter()
        .map(|x| vec![String::from(x.0), String::from(x.1)])
        .flatten()
        .collect();

    let mut planet_links: HashMap<String, Vec<String>> = all_planets
        .iter()
        .map(|x| (x.clone(), Vec::new()))
        .collect();

    for orbit in orbits {
        planet_links.get_mut(orbit.0).unwrap().push((orbit.1).clone());
        planet_links.get_mut(orbit.1).unwrap().push((orbit.0).clone());
    }
    planet_links
}

fn find_santa(planet_links: &HashMap<String, Vec<String>>, visited_planets: &mut HashSet<String>, current_planet: &str) -> i32 {
    if visited_planets.contains(current_planet) {
        return 0;
    }
    visited_planets.insert(String::from(current_planet));
    for link in &planet_links[current_planet] {
        if link == "SAN" {
            return 1;
        }
        let found = find_santa(planet_links, visited_planets, link);
        if found != 0 {
            return found + 1;
        }
    }
    0
}
