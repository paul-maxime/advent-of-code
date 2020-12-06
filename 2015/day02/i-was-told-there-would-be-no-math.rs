fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let presents: Vec<(u32, u32, u32)> = input.lines().map(|x| parse_present(x)).collect();

    println!("Wrapping paper: {} ft²", required_wrapping_paper(&presents));
    println!("Ribbon: {} ft", required_ribbon(&presents));
}

fn parse_present(present: &str) -> (u32, u32, u32) {
    let mut sizes: Vec<u32> = present.split("x").map(|x| x.parse().unwrap()).collect();
    sizes.sort();
    (sizes[0], sizes[1], sizes[2])
}

fn required_wrapping_paper(presents: &Vec<(u32, u32, u32)>) -> u32 {
    presents.iter().map(|x| 2 * x.0 * x.1 + 2 * x.0 * x.2 + 2 * x.1 * x.2 + x.0 * x.1).sum()
}

fn required_ribbon(presents: &Vec<(u32, u32, u32)>) -> u32 {
    presents.iter().map(|x| x.0 * 2 + x.1 * 2 + x.0 * x.1 * x.2).sum()
}
