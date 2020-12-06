fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let lines: Vec<&str> = input.lines().collect();

    let original_length = lines.iter().map(|x| x.len()).sum::<usize>();
    let unescaped_length = lines.iter().map(|x| unescaped_size(x)).sum::<usize>();
    let escaped_length = lines.iter().map(|x| escaped_size(x)).sum::<usize>();

    println!("{} - {} = {}", original_length, unescaped_length, original_length - unescaped_length);
    println!("{} - {} = {}", escaped_length, original_length, escaped_length - original_length);
}

fn unescaped_size(text: &str) -> usize {
    unescape::unescape(&text[1..text.len() - 1]).unwrap().chars().count()
}

fn escaped_size(text: &str) -> usize {
    text.len() + text.matches("\\").count() + text.matches("\"").count() + 2
}
