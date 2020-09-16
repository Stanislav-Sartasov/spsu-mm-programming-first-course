package com.company.main;

import com.company.game.Game;
import com.company.player.*;

public class Main {

    public static void main(String[] args) {
		Player[] players = {new RandomPlayer(800), new MoreCleverPlayer(800), new TheMostCleverPlayer(800)};
		Game game = new Game(players);
		game.play();
    }
}

