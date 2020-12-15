use std::collections::HashMap;

fn main() {
    let input = [14, 3, 1, 0, 9, 5];
    let mut map: HashMap<i32, i32> = HashMap::new();

    let mut turn = 1;
    let mut spoken_number = 0;
    loop {
        let previous_number = spoken_number;
        if turn <= input.len() {
            spoken_number = input[turn - 1];
        } else {
            if let Some(&k) = map.get(&spoken_number) {
                spoken_number = turn as i32 - 1 - k;
            } else {
                spoken_number = 0;
            }
        }
        if turn == 2020 {
            println!("Turn 2020: {}", spoken_number);
        }
        if turn == 30000000 {
            println!("Turn 30000000: {}", spoken_number);
            break;
        }
        map.insert(previous_number, turn as i32 - 1);
        turn += 1;
    }
}
