fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let data: Vec<u64> = input.lines().map(|x| x.parse().unwrap()).collect();

    let anomaly = find_anomaly(&data).unwrap();
    println!("Anomaly: {}", anomaly);

    let weakness = find_weakness(&data, anomaly, 0, 0).unwrap();
    println!("Weakness: {}", weakness);
}

fn find_anomaly(data: &Vec<u64>) -> Option<u64> {
    const PREAMBLE: usize = 25;

    for i in 0..data.len() - PREAMBLE {
        let n = data[i + PREAMBLE];
        let slice = &data[i..i + PREAMBLE];

        if !slice.iter().any(|x| slice.iter().any(|y| x != y && x + y == n)) {
            return Some(n);
        }
    }

    None
}

fn find_weakness(data: &Vec<u64>, expected: u64, from: usize, to: usize) -> Option<u64> {
    let slice = &data[from..to+1];
    let total = slice.iter().sum::<u64>();

    if from != to && total == expected {
        return Some(slice.iter().min().unwrap() + slice.iter().max().unwrap());
    }

    if total < expected && to < data.len() {
        if let Some(result) = find_weakness(data, expected, from, to + 1) {
            return Some(result);
        }
    }

    if from == to && from < data.len() {
        if let Some(result) = find_weakness(data, expected, from + 1, to + 1) {
            return Some(result);
        }
    }

    None
}
