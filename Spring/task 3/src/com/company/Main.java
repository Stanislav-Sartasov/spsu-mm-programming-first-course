package com.company;

public class Main {

    public static void main(String[] args) {
		Player[] players = {new RandomPlayer(800), new MoreCleverPlayer(800), new TheMostCleverPlayer(800)};
		Game game = new Game(players);
		game.play();
    }
}

