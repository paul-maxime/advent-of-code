fn main() {
    let input = "ckczppom";

    println!("{}", find_hash(input, "00000"));
    println!("{}", find_hash(input, "000000"));
}

fn find_hash(input: &str, expected: &str) -> i32 {
    let mut iteration = 0;
    loop {
        let digest = md5::compute(format!("{}{}", input, iteration));
        let hex = format!("{:x}", digest);
        if hex.starts_with(expected) {
            break;
        }
        iteration += 1;
    }
    iteration
}
