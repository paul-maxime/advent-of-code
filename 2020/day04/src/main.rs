use regex::Regex;
use lazy_static::lazy_static;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let passports: Vec<&str> = input.split("\n\n").collect();

    println!(
        "{} valid passports (basic check)",
        passports.iter().filter(|x| is_valid_passport_basic(x)).count()
    );

    println!(
        "{} valid passports (full checks)",
        passports.iter().filter(|x| is_valid_passport_full(x)).count()
    );
}

fn parse_fields(passport: &str) -> Vec<(String, String)> {
    lazy_static! {
        static ref FIELD_REGEX: Regex = Regex::new(r"([a-z]{3}):([^\s]+)").unwrap();
    }

    FIELD_REGEX
        .captures_iter(passport)
        .map(|x| (String::from(&x[1]), String::from(&x[2])))
        .collect()
}

fn is_valid_passport_basic(passport: &str) -> bool {
    let fields = parse_fields(passport);
    fields.len() == 8 || (fields.len() == 7 && fields.iter().filter(|&x| x.0 == "cid").count() == 0)
}

fn is_valid_passport_full(passport: &str) -> bool {
    let fields = parse_fields(passport);
    is_valid_passport_basic(passport) && fields.iter().all(|x| is_valid_field(&x.0, &x.1))
}

fn is_valid_field(field: &str, value: &str) -> bool {
    lazy_static! {
        static ref HGT_REGEX: Regex = Regex::new(r"^([0-9]+)(in|cm)$").unwrap();
        static ref HCL_REGEX: Regex = Regex::new(r"^#[0-9a-f]{6}$").unwrap();
        static ref PID_REGEX: Regex = Regex::new(r"^[0-9]{9}$").unwrap();
    }

    match field {
        "byr" => {
            let year = value.parse().unwrap_or(0);
            year >= 1920 && year <= 2002
        },
        "iyr" => {
            let year = value.parse().unwrap_or(0);
            year >= 2010 && year <= 2020
        },
        "eyr" => {
            let year = value.parse().unwrap_or(0);
            year >= 2020 && year <= 2030
        },
        "ecl" => {
            ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"].iter().any(|&x| x == value)
        },
        "hgt" => {
            if let Some(captures) = HGT_REGEX.captures(value) {
                let height = captures[1].parse().unwrap_or(0);
                match &captures[2] {
                    "cm" => height >= 150 && height <= 193,
                    "in" => height >= 59 && height <= 76,
                    _ => false
                }
            } else {
                false
            }
        },
        "hcl" => {
            HCL_REGEX.is_match(value)
        },
        "pid" => {
            PID_REGEX.is_match(value)
        },
        "cid" => true,
        _ => false
    }
}
