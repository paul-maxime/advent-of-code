use std::collections::VecDeque;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    let sum: i64 = input.lines().map(|x| shunting_yard(&x.replace(" ", ""), false)).sum();
    println!("Sum without precedence: {}", sum);

    let sum: i64 = input.lines().map(|x| shunting_yard(&x.replace(" ", ""), true)).sum();
    println!("Ultimate sum with precedence: {}", sum);
}

fn shunting_yard(input: &str, has_precedence: bool) -> i64 {
    let mut output_stack: VecDeque<i64> = VecDeque::new();
    let mut operator_stack: VecDeque<char> = VecDeque::new();

    for c in input.chars() {
        if c.is_digit(10) {
            output_stack.push_back(c.to_digit(10).unwrap() as i64);
        } else {
            push_operator(&mut output_stack, &mut operator_stack, c, has_precedence);
        }
    }

    while operator_stack.len() > 0 {
        pop_operator(&mut output_stack, &mut operator_stack);
    }

    output_stack.pop_back().unwrap()
}

fn push_operator(output_stack: &mut VecDeque<i64>, operator_stack: &mut VecDeque<char>, operator: char, has_precedence: bool) {
    match operator {
        '+' | '*' => {
            while let Some(&top_operator) = operator_stack.back() {
                if top_operator == '(' {
                    break;
                }
                if has_precedence && top_operator == '*' && operator == '+' {
                    break;
                }
                pop_operator(output_stack, operator_stack);
            }
            operator_stack.push_back(operator);
        },
        '(' => {
            operator_stack.push_back(operator);
        },
        ')' => {
            while let Some(&top_operator) = operator_stack.back() {
                if top_operator == '(' {
                    operator_stack.pop_back();
                    break;
                }
                pop_operator(output_stack, operator_stack);
            }
        }
        _ => unreachable!(),
    };
}

fn pop_operator(output_stack: &mut VecDeque<i64>, operator_stack: &mut VecDeque<char>) {
    let operator = operator_stack.pop_back().unwrap();
    let left = output_stack.pop_back().unwrap();
    let right = output_stack.pop_back().unwrap();
    match operator {
        '+' => output_stack.push_back(left + right),
        '*' => output_stack.push_back(left * right),
        _ => unreachable!(),
    }
}
