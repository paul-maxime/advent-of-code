use std::collections::HashMap;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let mut adapters: Vec<u32> = input.lines().map(|x| x.parse().unwrap()).collect();

    adapters.push(0);
    adapters.push(*adapters.iter().max().unwrap() + 3);
    adapters.sort();

    link_all_adapters(&adapters);

    let now = std::time::Instant::now();
    println!("Arrangements (recursive way): {} | {:?}",
        count_arrangements_rec(&adapters, &mut HashMap::new(), 0),
        now.elapsed()
    );

    let now = std::time::Instant::now();
    println!("Arrangements (optimized way): {} | {:?}",
        count_arrangements(&adapters),
        now.elapsed()
    );
}

// Part 1

fn link_all_adapters(adapters: &Vec<u32>) {
    let differences: Vec<u32> = (1..adapters.len()).map(|i| adapters[i] - adapters[i - 1]).collect();

    let nb1 = differences.iter().filter(|&&x| x == 1).count();
    let nb3 = differences.iter().filter(|&&x| x == 3).count();

    println!("{} × {} = {}", nb1, nb3, nb1 * nb3);
}

// Part 2 (recursive way)

fn count_arrangements_rec(adapters: &Vec<u32>, cache: &mut HashMap<usize, u64>, index: usize) -> u64 {
    if let Some(result) = cache.get(&index) {
        return *result;
    }
    let mut total = 0u64;
    if index == adapters.len() - 1 {
        return 1;
    }
    let current = adapters[index];
    for i in 1..=3 {
        if let Some(next) = adapters.get(index + i) {
            if next - current <= 3 {
                total += count_arrangements_rec(adapters, cache, index + i)
            }
        }
    }

    cache.insert(index, total);
    total
}

// Part 2 (optimized using tribonacci)

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
