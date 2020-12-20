use std::collections::HashSet;
use std::collections::HashMap;

const TILE_SIZE: usize = 10;

const SEA_MONSTER: [(usize, usize); 15] = [
    (0, 18),
    (1, 0), (1, 5), (1, 6), (1, 11), (1, 12), (1, 17), (1, 18), (1, 19),
    (2, 1), (2, 4), (2, 7), (2, 10), (2, 13), (2, 16),
];

const SEA_MONSTER_WIDTH: usize = 20;
const SEA_MONSTER_HEIGHT: usize = 3;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();

    // Part 1

    let mut tiles = parse_tiles(&input);
    let solved_jigsaw = solve_jigsaw(&mut tiles);

    // Part 2

    let final_image = build_final_image(tiles, solved_jigsaw);
    find_sea_monsters(&final_image);
}

pub fn transform(x: &mut usize, y: &mut usize, size: usize, flip_x: bool, flip_y: bool, swap_x_y: bool) {
    if flip_x {
        *x = size - 1 - *x;
    }
    if flip_y {
        *y = size - 1 - *y;
    }
    if swap_x_y {
        let tmp = *x;
        *x = *y;
        *y = tmp;
    }
}

// Part 1

#[derive(Debug)]
struct Tile {
    id: i64,
    content: Vec<Vec<char>>,

    swap_x_y: bool,
    flip_x: bool,
    flip_y: bool,
}

impl Tile {
    pub fn get(&self, mut x: usize, mut y: usize) -> char {
        transform(&mut x, &mut y, self.content.len(), self.flip_x, self.flip_y, self.swap_x_y);
        self.content[y][x]
    }

    pub fn can_fit_right(&self, tile: &Tile) -> bool {
        for i in 0..TILE_SIZE {
            if self.get(TILE_SIZE - 1, i) != tile.get(0, i) {
                return false;
            }
        }
        true
    }

    pub fn can_fit_bottom(&self, tile: &Tile) -> bool {
        for i in 0..TILE_SIZE {
            if self.get(i, TILE_SIZE - 1) != tile.get(i, 0) {
                return false;
            }
        }
        true
    }
}

fn parse_tiles(input: &str) -> Vec<Tile> {
    input.lines().filter(|x| !x.is_empty()).collect::<Vec<&str>>().chunks(TILE_SIZE + 1).map(|data| {
        let id: i64 = data[0]
            .trim_matches(|x: char| !x.is_digit(10))
            .parse().unwrap();

        let content: Vec<Vec<char>> = data[1..].iter()
            .map(|x| x.chars().collect())
            .collect();

        Tile { id, content, swap_x_y: false, flip_x: false, flip_y: false }
    }).collect()
}

fn solve_jigsaw(tiles: &mut Vec<Tile>) -> Vec<Vec<i64>> {
    for index in 0 .. tiles.len() {
        tiles[index].swap_x_y = false;
        tiles[index].flip_x = false;
        tiles[index].flip_y = false;

        if let Some(result) = solve_remaining_jigsaw(tiles, index) {
            println!("Solved jigsaw: {}",
                result[0][0] *
                result[result.len() - 1][0] *
                result[0][result.len() - 1] *
                result[result.len() - 1][result.len() - 1]
            );
            return result;
        }
    }

    panic!("The jigsaw doesn't seem to have any solution.");
}

fn solve_remaining_jigsaw(tiles: &mut Vec<Tile>, starting_index: usize) -> Option<Vec<Vec<i64>>> {
    let line_size = (tiles.len() as f64).sqrt() as usize;
    let mut found_indexes: Vec<usize> = vec![];
    let mut remaining_indexes: HashSet<usize> = (0..tiles.len()).collect();

    found_indexes.push(starting_index);
    remaining_indexes.remove(&starting_index);

    loop {
        let before = found_indexes.len();
        let is_bottom = found_indexes.len() % line_size == 0;

        let previous_index = if is_bottom {
            found_indexes[found_indexes.len() - line_size]
        } else {
            found_indexes[found_indexes.len() - 1]
        };

        'outer: for &index in remaining_indexes.iter() {
            for i in 0..8 {
                tiles[index].swap_x_y = i & 1 == 1;
                tiles[index].flip_x = i & 2 == 2;
                tiles[index].flip_y = i & 4 == 4;

                if is_bottom && tiles[previous_index].can_fit_bottom(&tiles[index]) || !is_bottom && tiles[previous_index].can_fit_right(&tiles[index]) {
                    found_indexes.push(index);
                    remaining_indexes.remove(&index);
                    break 'outer;
                }
            }
        }

        if found_indexes.len() == before {
            break;
        }
    }

    if remaining_indexes.len() == 0 {
        let found_tiles: Vec<i64> = found_indexes.iter().map(|&x| tiles[x].id).collect();
        let solved_tiles = found_tiles.chunks(line_size).map(|x| x.iter().map(|x| *x).collect::<Vec<i64>>()).collect();

        Some(solved_tiles)
    } else {
        None
    }
}

// Part 2

fn build_final_image(tiles: Vec<Tile>, solved_jigsaw: Vec<Vec<i64>>) -> Vec<Vec<char>> {
    let tiles: HashMap<i64, Tile> = tiles.into_iter().map(|x| (x.id, x)).collect();
    let picture_width = solved_jigsaw.len() * (TILE_SIZE - 2);

    let mut final_image: Vec<Vec<char>> = vec![vec!['.'; picture_width]; picture_width];

    for y in 0 .. picture_width {
        for x in 0 .. picture_width {

            let tile_x = x / (TILE_SIZE - 2);
            let tile_y = y / (TILE_SIZE - 2);

            let inner_x = x % (TILE_SIZE - 2) + 1;
            let inner_y = y % (TILE_SIZE - 2) + 1;

            final_image[y][x] = tiles[&solved_jigsaw[tile_y][tile_x]].get(inner_x, inner_y);
        }
    }

    final_image
}

fn find_sea_monsters(image: &Vec<Vec<char>>) {
    let sea_monsters = count_sea_monsters(image);

    let total_tiles: usize = image.iter().map(|line| line.iter().filter(|&&x| x == '#').count()).sum();
    println!("{} sea monsters, {} unused tiles", sea_monsters, total_tiles - sea_monsters * SEA_MONSTER.len());
}

fn count_sea_monsters(image: &Vec<Vec<char>>) -> usize {
    let mut count  = 0;

    for x in 0..=image.len() - SEA_MONSTER_WIDTH {
        for y in 0..=image.len() - SEA_MONSTER_HEIGHT {
            if is_sea_monster(image, x, y) {
                count += 1;
            }
        }
    }

    count
}

fn is_sea_monster(image: &Vec<Vec<char>>, x: usize, y: usize) -> bool {
    (0..8).any(|t| {
        SEA_MONSTER.iter().all(|tile| {
            let mut x = x + tile.1;
            let mut y = y + tile.0;
            transform(&mut x, &mut y, image.len(), t & 1 == 1, t & 2 == 2, t & 4 == 4);
            x < image.len() && y < image.len() && image[y][x] == '#'
        })
    })
}
