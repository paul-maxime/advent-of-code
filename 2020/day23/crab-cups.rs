use std::collections::VecDeque;

fn main() {
    let input = "398254716";
    let cups: VecDeque<u32> = input.chars().map(|x| x.to_digit(10).unwrap()).collect();

    play_game(cups.clone(), false);

    // Takes about 3 hours to give a result, but I'm too lazy to improve the code.
    play_game(cups.clone(), true);
}

fn play_game(mut cups: VecDeque<u32>, full_rules: bool) {
    let mut current: usize = 0;
    let mut max = *cups.iter().max().unwrap();

    while full_rules && cups.len() < 1000000 {
        max += 1;
        cups.push_back(max);
    }

    for _ in 0..(if full_rules { 10000000 } else { 100 }) {
        let cup = cups[current];
        let a = cups.remove((current + 1) % cups.len()).unwrap();
        current = cups.iter().position(|x| *x == cup).unwrap();
        let b = cups.remove((current + 1) % cups.len()).unwrap();
        current = cups.iter().position(|x| *x == cup).unwrap();
        let c = cups.remove((current + 1) % cups.len()).unwrap();

        let mut expected = cup;
        while expected == cup || expected == a || expected == b || expected == c {
            expected = expected - 1;
            if expected == 0 { expected = max };
        }

        let selected_cup = cups.iter().position(|x| *x == expected).unwrap();

        cups.insert(selected_cup + 1, c);
        cups.insert(selected_cup + 1, b);
        cups.insert(selected_cup + 1, a);

        current = (cups.iter().position(|x| *x == cup).unwrap() + 1) % cups.len();
    }

    current = cups.iter().position(|x| *x == 1).unwrap();

    if !full_rules {
        for i in 1..cups.len() {
            print!("{}", cups[(current + i) % cups.len()]);
        }
        println!();
    } else {
        let cup_1 = cups[(current + 1) % cups.len()];
        let cup_2 = cups[(current + 1) % cups.len()];
        println!("{} x {} = {}", cup_1, cup_2, cup_1 as u64 * cup_2 as u64);
    }
}
