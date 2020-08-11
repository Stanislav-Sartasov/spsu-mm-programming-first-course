package com.company.game;

import com.company.player.*;
import org.junit.Test;

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
                if (players[i].getMoney() >= players[(i + 1) % 3].getMoney() && players[i].getMoney() >= players[(i + 2) % 3].getMoney())
                    win[i] += 1;
                middle[i] += players[i].getMoney();
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