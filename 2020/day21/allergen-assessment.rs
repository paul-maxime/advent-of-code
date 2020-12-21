use std::collections::HashSet;
use std::collections::HashMap;
use std::collections::BTreeMap;

fn main() {
    let input = std::fs::read_to_string("./input")
        .unwrap()
        .replace(&['(', ')', ','][..], "");

    let foods = parse_foods(&input);
    let matches = compute_all_matches(&foods);

    println!("Useless ingredients used {} times", calc_useless_ingredient_usage(&foods, &matches));
    println!("Canonical ingredient list: {}", matches.iter().map(|x| *x.1).collect::<Vec<&str>>().join(","));
}

fn parse_foods(input: &str) -> Vec<(HashSet<&str>, HashSet<&str>)> {
    input.lines().map(|line| {
        let mut groups = line.split("contains")
            .map(|l| l.split(" ").filter(|x| !x.is_empty()).collect::<HashSet<&str>>())
            .collect::<Vec<HashSet<&str>>>();

        (groups.remove(0), groups.remove(0))
    }).collect()
}

fn compute_all_matches<'a>(foods: &'a Vec<(HashSet<&str>, HashSet<&str>)>) -> BTreeMap<&'a str, &'a str> {
    let mut matches: HashMap<&str, HashSet<&str>> = HashMap::new();

    for food in foods.iter() {
        let ingredients = &food.0;
        let allergens = &food.1;
        for allergen in allergens {
            if !matches.contains_key(allergen) {
                matches.insert(allergen, ingredients.clone());
            } else {
                let current_ingredients = matches.get(allergen).unwrap();
                let remaining: HashSet<&str> = current_ingredients.intersection(ingredients).copied().collect();
                matches.insert(allergen, remaining);
            }
        }
    }

    let mut final_matches: BTreeMap<&str, &str> = BTreeMap::new();
    let mut found_ingredients: HashSet<&str> = HashSet::new();

    while matches.iter().any(|x| x.1.len() > 1) {
        for (allergen, ingredients) in matches.clone() {
            let remaining: HashSet<&str> = ingredients.difference(&found_ingredients).cloned().collect();
            if remaining.len() == 1 {
                let matched_ingredient = remaining.iter().nth(0).unwrap();
                println!("{} contains {} ;", matched_ingredient, allergen);
                found_ingredients.insert(matched_ingredient);
                final_matches.insert(allergen, matched_ingredient);
                matches.insert(allergen, remaining);
            }
        }
    }

    final_matches
}

fn calc_useless_ingredient_usage(foods: &Vec<(HashSet<&str>, HashSet<&str>)>, matches: &BTreeMap<&str, &str>) -> usize {
    let all_ingredients: HashSet<&str> = foods.iter().map(|x| x.0.clone()).flatten().collect();
    let used_ingredients: HashSet<&str> = matches.iter().map(|x| x.1.clone()).collect();
    let useless_ingredients: HashSet<&str> = all_ingredients.difference(&used_ingredients).cloned().collect();

    useless_ingredients.iter().map(|ing| foods.iter().filter(|x| x.0.contains(ing)).count()).sum()
}
