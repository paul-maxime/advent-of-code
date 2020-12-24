use std::collections::HashSet;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    let mut tiles: HashSet<(i32, i32, i32)> = HashSet::new();

    for line in input.lines() {
        let tile = get_tile(line);
        if tiles.contains(&tile) {
            tiles.remove(&tile);
        } else {
            tiles.insert(tile);
        }
    }

    println!("Initial tiles: {}", tiles.len());

    for _ in 0..100 {
        tiles = apply_day(&tiles);
    }

    println!("Tiles after 100 days: {}", tiles.len());
}

fn get_tile(movement: &str) -> (i32, i32, i32) {
    let mut pos = (0, 0, 0);

    let mut i = 0usize;
    while i < movement.chars().count() {
        let c = movement.chars().nth(i).unwrap();
        i += 1;
        match c {
            'e' => {
                pos.0 += 1;
                pos.1 += -1;
                pos.2 += 0;
            },
            'w' => {
                pos.0 += -1;
                pos.1 += 1;
                pos.2 += 0;
            },
            'n' => {
                let c = movement.chars().nth(i).unwrap();
                i += 1;
                match c {
                    'w' => {
                        pos.0 += 0;
                        pos.1 += 1;
                        pos.2 += -1;
                    },
                    'e' => {
                        pos.0 += 1;
                        pos.1 += 0;
                        pos.2 += -1;
                    },
                    _ => unreachable!()
                };
            },
            's' => {
                let c = movement.chars().nth(i).unwrap();
                i += 1;
                match c {
                    'w' => {
                        pos.0 += -1;
                        pos.1 += 0;
                        pos.2 += 1;
                    },
                    'e' => {
                        pos.0 += 0;
                        pos.1 += -1;
                        pos.2 += 1;
                    },
                    _ => unreachable!()
                };
            }
            _ => unreachable!(),
        }
    }

    pos
}

fn count_adjacent_tiles(tiles: &HashSet<(i32, i32, i32)>, tile: (i32, i32, i32)) -> u32 {
    let mut adjacent = 0;
    if tiles.contains(&(tile.0 + 1, tile.1 - 1, tile.2)) { adjacent += 1; }
    if tiles.contains(&(tile.0 - 1, tile.1 + 1, tile.2)) { adjacent += 1; }
    if tiles.contains(&(tile.0, tile.1 + 1, tile.2 - 1)) { adjacent += 1; }
    if tiles.contains(&(tile.0, tile.1 - 1, tile.2 + 1)) { adjacent += 1; }
    if tiles.contains(&(tile.0 + 1, tile.1, tile.2 - 1)) { adjacent += 1; }
    if tiles.contains(&(tile.0 - 1, tile.1, tile.2 + 1)) { adjacent += 1; }
    adjacent
}

fn apply_day(old_tiles: &HashSet<(i32, i32, i32)>) -> HashSet<(i32, i32, i32)> {
    let mut new_tiles: HashSet<(i32, i32, i32)> = HashSet::new();

    for tile in old_tiles {
        for x in (tile.0 - 1)..=(tile.0 + 1) {
            for y in (tile.1 - 1)..=(tile.1 + 1) {
                for z in (tile.2 - 1)..=(tile.2 + 1) {
                    let adjacent = count_adjacent_tiles(old_tiles, (x, y, z));
                    if old_tiles.contains(&(x, y, z)) {
                        if adjacent >= 1 && adjacent <= 2 {
                            new_tiles.insert((x, y, z));
                        }
                    } else {
                        if adjacent == 2 {
                            new_tiles.insert((x, y, z));
                        }
                    }

                }
            }
        }
    }

    new_tiles
}
