fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let public_keys: Vec<u64> = input.lines().map(|x| x.parse().unwrap()).collect();

    let card_key = public_keys[0];
    let door_key = public_keys[1];

    println!("Public keys: {} {}", card_key, door_key);

    let card_loop_size = get_loop_size(card_key);
    let door_loop_size = get_loop_size(door_key);

    println!("Loop sizes: {} {}", card_loop_size, door_loop_size);

    println!("Encryption keys: {} {}",
        compute_encryption_key(card_key, door_loop_size),
        compute_encryption_key(door_key, card_loop_size)
    );
}

fn get_loop_size(public_key: u64) -> u64 {
    let mut loop_size = 0;
    let mut x = 1;
    loop {
        loop_size += 1;
        x *= 7;
        x %= 20201227;
        if x == public_key {
            break;
        }
    }
    loop_size
}

fn compute_encryption_key(public_key: u64, loop_size: u64) -> u64 {
    let mut x = 1;
    for _ in 0..loop_size {
        x *= public_key;
        x %= 20201227;
    }
    x
}
