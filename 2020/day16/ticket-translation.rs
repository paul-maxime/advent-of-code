use std::collections::HashMap;

#[derive(Debug)]
struct Field {
    name: String,
    range1: (i32, i32),
    range2: (i32, i32),
}

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    let fields = parse_fields(&input);
    let tickets = parse_tickets(&input);

    println!("Ticket scanning error rate: {}", compute_error_rate(&fields, &tickets));
    println!("Departure product: {}", compute_departure_product(&fields, &tickets));
}

// Parsing

fn parse_fields(input: &str) -> Vec<Field> {
    let field_regex = regex::Regex::new(r"([a-z ]+): (\d+)-(\d+) or (\d+)-(\d+)").unwrap();

    field_regex.captures_iter(input).map(|cap| Field {
        name: cap[1].to_string(),
        range1: (cap[2].parse().unwrap(), cap[3].parse().unwrap()),
        range2: (cap[4].parse().unwrap(), cap[5].parse().unwrap()),
    }).collect()
}

fn parse_tickets(input: &str) -> Vec<Vec<i32>> {
    input.lines()
        .filter(|x| x.contains(","))
        .map(|line| line.split(",").map(|x| x.parse().unwrap()).collect())
        .collect()
}

// Part 1

fn compute_error_rate(fields: &Vec<Field>, tickets: &Vec<Vec<i32>>) -> i32 {
    tickets[1..].iter().map(|ticket| ticket.iter().filter(|&&x| !is_valid_field(fields, x)).sum::<i32>()).sum()
}

fn is_valid_field(fields: &Vec<Field>, value: i32) -> bool {
    fields.iter().any(|field| is_in_field_range(field, value))
}

fn is_in_field_range(field: &Field, value: i32) -> bool {
    field.range1.0 <= value && value <= field.range1.1 || field.range2.0 <= value && value <= field.range2.1
}

// Part 2

fn compute_departure_product(fields: &Vec<Field>, tickets: &Vec<Vec<i32>>) -> i64 {
    let mapped_fields = map_index_to_fields(fields, tickets);
    mapped_fields.iter().filter(|x| x.1.starts_with("departure")).map(|x| tickets[0][*x.0]).fold(1, |cur, arr| cur * arr as i64)
}

fn map_index_to_fields<'a>(fields: &'a Vec<Field>, tickets: &Vec<Vec<i32>>) -> HashMap<usize, &'a str> {
    let valid_tickets: Vec<&Vec<i32>> = tickets.iter().filter(|&x| is_valid_ticket(fields, x)).collect();
    let mut mapped_fields: HashMap<usize, &str> = HashMap::new();

    while mapped_fields.len() != fields.len() {
        for i in 0..valid_tickets[0].len() {
            let field_values: Vec<i32> = valid_tickets.iter().map(|x| x[i]).collect();
            let matching_fields: Vec<&Field> = fields.iter().filter(|field|
                !mapped_fields.values().any(|&x| x == field.name) &&
                field_values.iter().all(|&x| is_in_field_range(field, x))
            ).collect();

            if matching_fields.len() == 1 {
                mapped_fields.insert(i, &matching_fields[0].name);
            }
        }
    }

    mapped_fields
}

fn is_valid_ticket(fields: &Vec<Field>, ticket: &Vec<i32>) -> bool {
    ticket.iter().all(|&x| is_valid_field(fields, x))
}
