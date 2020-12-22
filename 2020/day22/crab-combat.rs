use std::collections::VecDeque;

fn main() {
    let input = std::fs::read_to_string("./input").unwrap();
    let (player_1, player_2) = parse_decks(&input);

    play_game(player_1.clone(), player_2.clone(), -1);
    play_game(player_1.clone(), player_2.clone(), 0);
}

fn parse_decks(input: &str) -> (VecDeque<usize>, VecDeque<usize>) {
    let mut player_1: VecDeque<usize> = VecDeque::new();
    let mut player_2: VecDeque<usize> = VecDeque::new();
    let mut current_player: i32 = 0;

    for line in input.lines().filter(|x| !x.is_empty()) {
        match line {
            "Player 1:" => current_player = 1,
            "Player 2:" => current_player = 2,
            _ => if current_player == 1 { &mut player_1 } else { &mut player_2 }.push_back(line.parse().unwrap())
        }
    }

    (player_1, player_2)
}

fn play_game(mut player_1: VecDeque<usize>, mut player_2: VecDeque<usize>, depth: i32) -> i32 {
    let mut current_round = 0;

    while player_1.len() > 0 && player_2.len() > 0 {
        let card_1 = player_1.pop_front().unwrap();
        let card_2 = player_2.pop_front().unwrap();

        let winner = if depth >= 0 && player_1.len() >= card_1 && player_2.len() >= card_2 {
            let slice_1: VecDeque<usize> = player_1.iter().take(card_1).map(|x| *x).collect();
            let slice_2: VecDeque<usize> = player_2.iter().take(card_2).map(|x| *x).collect();
            play_game(slice_1, slice_2, depth + 1)
        } else {
            if card_1 > card_2 { 1 } else { 2 }
        };

        if winner == 1 {
            player_1.push_back(card_1);
            player_1.push_back(card_2);
        } else {
            player_2.push_back(card_2);
            player_2.push_back(card_1);
        }

        current_round += 1;
        if current_round == 1000 {
            // Eh, it's probably an infinite loop.
            return 1;
        }
    }

    let winner = if player_1.len() > 0 { 1 } else { 2 };
    let winner_deck: &VecDeque<usize> = if winner == 1 { &player_1 } else { &player_2 };

    if depth <= 0 {
        let score: usize = winner_deck.iter().enumerate().map(|(x, i)| (winner_deck.len() - x) * i).sum();
        println!("Winner score: {}", score);
    }

    winner
}
