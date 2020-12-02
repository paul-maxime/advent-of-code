use std::fs;

fn main() {
    let pixels: Vec<u32> = fs::read_to_string("./input")
        .unwrap()
        .trim()
        .chars()
        .map(|x| x.to_digit(10).unwrap())
        .collect();

    let mut final_pixels = [2; 25 * 6];
    let mut layer_stats: Vec<(i32, i32, i32)> = Vec::new();

    for layer in 0..100 {
        let mut numbers = (0, 0, 0);
        for y in 0..6 {
            for x in 0..25 {
                let n = pixels.get(x + y * 25 + layer * 6 * 25).unwrap();
                if *n == 0 { numbers.0 += 1 }
                if *n == 1 { numbers.1 += 1 }
                if *n == 2 { numbers.2 += 1 }
                if final_pixels[x + y * 25] == 2 {
                    final_pixels[x + y * 25] = *n;
                }
            }
        }
        layer_stats.push((layer as i32, numbers.0, numbers.1 * numbers.2));
    }

    if let Some(layer) = layer_stats.iter().min_by(|a, b| a.1.cmp(&b.1)) {
        println!("Layer: {} ({} zeros), hash: {}", layer.0, layer.1, layer.2);
    }

    for y in 0..6 {
        for x in 0..25 {
            print!("{}", if final_pixels[x + y * 25] == 0 { "." } else { "@" });
        }
        println!();
    }
}
