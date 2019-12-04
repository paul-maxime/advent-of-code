const FROM_CODE: i32 = 256310;
const TO_CODE: i32 = 732736;

fn is_valid_code(code: i32, last_digit: i32, found_adjacent: bool) -> bool {
    let current_digit = code % 10;
    if code == 0 {
        found_adjacent
    } else if last_digit < current_digit {
        false
    } else {
        is_valid_code(code / 10, current_digit, found_adjacent || last_digit == current_digit)
    }
}

fn is_valid_code_2(code: i32, last_digit: i32, found_adjacent: bool, adjacent_count: i32) -> bool {
    let current_digit = code % 10;
    if code == 0 {
        found_adjacent || adjacent_count == 2
    } else if last_digit < current_digit {
        false
    } else if last_digit == current_digit {
        is_valid_code_2(code / 10, current_digit, found_adjacent, adjacent_count + 1)
    } else {
        is_valid_code_2(code / 10, current_digit, found_adjacent || adjacent_count == 2, 1)
    }
}

fn main() {
    println!("{}", (FROM_CODE..TO_CODE+1).filter(|&x| is_valid_code(x, 10, false)).count());
    println!("{}", (FROM_CODE..TO_CODE+1).filter(|&x| is_valid_code_2(x, 10, false, 1)).count());
}
