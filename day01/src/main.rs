use std::fs;

fn required_fuel(mass: i32, is_recursive: bool) -> i32 {
    let fuel = mass / 3 - 2;
    if fuel > 0 {
        fuel + if is_recursive { required_fuel(fuel, true) } else { 0 }
    } else {
        0
    }
}

fn total_fuel(mass_per_module: &Vec<i32>, is_recursive: bool) -> i32 {
    let fuel_per_module = mass_per_module
        .iter()
        .map(|&x| required_fuel(x, is_recursive));

    fuel_per_module.sum()
}

fn main() {
    let mass_per_module: Vec<i32> = fs::read_to_string("./input")
        .unwrap()
        .lines()
        .map(|x| x.parse().unwrap())
        .collect();

    println!("Sum of the fuel requirements: {}",
        total_fuel(&mass_per_module, false)
    );
    println!("Sum of the fuel requirements (including fuel): {}",
        total_fuel(&mass_per_module, true)
    );
}
