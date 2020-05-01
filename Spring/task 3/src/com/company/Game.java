package com.company;

import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

public class Game {
    private final String[] deckOfCards = new String[4 * 8 * 13];
    private final Player[] players;
    private final Dealer dealer;

    public Dealer getDealer() {
        return dealer;
    }

    public Player[] getPlayers() {
        return players;
    }

    public String[] getDeckOfCards() {
        return deckOfCards;
    }

    private static void shuffleArray(String[] ar) { //Fisher–Yates shuffle
        Random rnd = ThreadLocalRandom.current();
        for (int i = ar.length - 1; i > 0; i--) {
            int index = rnd.nextInt(i + 1);
            String a = ar[index];
            ar[index] = ar[i];
            ar[i] = a;
        }
    }

    public Game (Player[] people) {
        players = people;
        dealer = new Dealer();
        for (int i = 0; i < 4 * 8; i++) {
            deckOfCards[i * 13] = "2";
            deckOfCards[i * 13 + 1] = "3";
            deckOfCards[i * 13 + 2] = "4";
            deckOfCards[i * 13 + 3] = "5";
            deckOfCards[i * 13 + 4] = "6";
            deckOfCards[i * 13 + 5] = "7";
            deckOfCards[i * 13 + 6] = "8";
            deckOfCards[i * 13 + 7] = "9";
            deckOfCards[i * 13 + 8] = "10";
            deckOfCards[i * 13 + 9] = "J";
            deckOfCards[i * 13 + 10] = "Q";
            deckOfCards[i * 13 + 11] = "K";
            deckOfCards[i * 13 + 12] = "A";
        }
        shuffleArray(deckOfCards);
    }

    private int dealCards(int i) {
        for (Player player: players) {
            if (player.inGame.equals("playing")) {
                if (player.makeMove(dealer.getCard()).equals("take")) {
                    player.addCard(deckOfCards[i]);
                    i++;
                } else {
                    player.inGame = "stoped";
                }
            }
        }
        dealer.addCard(deckOfCards[i]);
        return ++i;
    }

    public void play() {
        int i = 0;
        for (Player player: players) {
            player.addCard(deckOfCards[i]);
            i++;
        }
        i = dealCards(i);
        for (Player player: players) {
            if (player.sum() == 21) {
                String bj = player.ifBlackJack();
                if (bj.equals("take")) {
                    player.win(2);
                    player.inGame = "finished";
                } else {
                    player.inGame = "waiting";
                }
            }
        }

        boolean first = true;
        while (dealer.sum() < 17) {
            i = dealCards(i);
            if (first && dealer.sum() == 21) {
                for (Player player: players) {
                    if (player.inGame.equals("waiting")) {
                        player.win(3);
                    } else {
                        player.lose(2);
                    }
                }
                break;
            }
            first = false;
            for (Player player: players) {
                if (player.sum() > 21) {
                    player.lose(2);
                    player.inGame = "finished";
                }
            }
        }

        for (Player player: players) {
            if (player.inGame.equals("playing") || player.inGame.equals("stoped")) {
                if (player.sum() > dealer.sum() || dealer.sum() > 21) {
                    player.win(2);
                } else {
                    player.lose(2);
                }
            }
        }
    }
}
