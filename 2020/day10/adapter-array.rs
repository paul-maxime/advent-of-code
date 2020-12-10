fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let mut adapters: Vec<u32> = input.lines().map(|x| x.parse().unwrap()).collect();

    adapters.push(0);
    adapters.push(*adapters.iter().max().unwrap() + 3);
    adapters.sort();

    link_all_adapters(&adapters);
    println!("{}", count_arrangements(&adapters));
}

fn link_all_adapters(adapters: &Vec<u32>) {
    let differences: Vec<u32> = (1..adapters.len()).map(|i| adapters[i] - adapters[i - 1]).collect();

    let nb1 = differences.iter().filter(|&&x| x == 1).count();
    let nb3 = differences.iter().filter(|&&x| x == 3).count();

    println!("{} × {} = {}", nb1, nb3, nb1 * nb3);
}

fn count_arrangements(adapters: &Vec<u32>) -> u64 {
    let mut ways = 1u64;
    let mut current_series = 0;
    for i in 1..adapters.len() {
        if adapters[i] - adapters[i - 1] == 1 {
            current_series += 1
        } else {
            ways *= tribonacci(current_series + 2);
            current_series = 0;
        }
    }
    ways
}

fn tribonacci(n: u64) -> u64 {
    match n {
        0 | 1 => 0,
        2 => 1,
        _ => tribonacci(n - 1) + tribonacci(n - 2) + tribonacci(n - 3)
    }
}
