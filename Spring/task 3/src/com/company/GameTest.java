package com.company;

import org.junit.Test;

import static org.junit.Assert.*;

public class GameTest {

    @Test
    public void play() {
        int[] win = new int[3];
        int[] middle = new int[3];

        for (int j = 0; j < 100; j++) {
            Player[] players = {new RandomPlayer(800), new MoreCleverPlayer(800), new TheMostCleverPlayer(800)};
            Game game = new Game(players);
            for (int i = 0; i < 400; i++)
                game.play();
            for (int i = 0; i < 3; i++) {
                if (players[i].money >= players[(i + 1) % 3].money && players[i].money >= players[(i + 2) % 3].money)
                    win[i] += 1;
                middle[i] += players[i].money;
            }
        }

        System.out.println("Average value of remaining money from RandomPlayer:  " + (middle[0] / 100.0));
        System.out.println("Average value of remaining money from MoreCleverPlayer:  " + (middle[1] / 100.0));
        System.out.println("Average value of remaining money from TheMostCleverPlayer:  " + (middle[2] / 100.0));
        System.out.println();
        System.out.println("RandomPlayer collected more money than others:  " + win[0] + " times");
        System.out.println("MoreCleverPlayer collected more money than others:  " + win[1] + " times");
        System.out.println("TheMostCleverPlayer collected more money than others:  " + win[2] + " times");
    }
}