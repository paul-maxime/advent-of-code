fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let lines: Vec<&str> = input.lines().collect();

    let earliest_time: i64 = lines[0].parse().unwrap();
    let shuttles: Vec<i64> = lines[1].split(",").map(|x| x.parse().unwrap_or_default()).collect();

    compute_time_to_wait(earliest_time, &shuttles);
    compute_weird_challenge(&shuttles);
}

fn compute_time_to_wait(earliest_time: i64, shuttles: &Vec<i64>) {
    let shuttle_times: Vec<(i64, i64)> = shuttles
        .iter()
        .filter(|&&x| x > 0)
        .map(|&x| (x, x * (earliest_time as f32 / x as f32).ceil() as i64))
        .collect();

    let best_shuttle = shuttle_times.iter().min_by_key(|x| x.1).unwrap();
    let time_to_wait = best_shuttle.1 - earliest_time;

    println!("Bus: {} × Time to wait: {} = {}", best_shuttle.0, time_to_wait, best_shuttle.0 * time_to_wait);
}

fn compute_weird_challenge(shuttles: &Vec<i64>) {
    let best_group_of_shuttles: (i64, i64) = shuttles.iter().enumerate().map(|(index, &time)| {
        let product = shuttles.iter().enumerate()
            .filter(|(index2, &time2)| (index as i64 - *index2 as i64).abs() == time2)
            .fold(time, |acc, cur| acc * cur.1);
        (index as i64, product)
    }).filter(|x| x.1 > 0).max_by_key(|x| x.1).unwrap();

    let step = best_group_of_shuttles.1;
    let delta = -best_group_of_shuttles.0;

    let mut time = delta;
    loop {
        if shuttles.iter().enumerate().all(|(i, &x)| x == 0 || (time + i as i64) % x == 0) {
            break;
        }
        time += step;
    }

    println!("The shuttles are aligned at {}", time);
}
