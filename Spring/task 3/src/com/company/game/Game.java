package com.company.game;

import com.company.cards.*;
import com.company.player.*;

public class Game {
    private final DeskOfCards deckOfCards;
    private final Player[] players;
    private final Dealer dealer;

    public Dealer getDealer() {
        return dealer;
    }

    public Player[] getPlayers() {
        return players;
    }

    public DeskOfCards getDeckOfCards() {
        return deckOfCards;
    }

    public Game (Player[] people) {
        players = people;
        dealer = new Dealer();
        deckOfCards = new DeskOfCards();
    }

    private int dealCards(int i) {
        for (Player player: players) {
            if (player.getInGame().equals("playing")) {
                if (player.makeMove(dealer.getCard()).equals("take")) {
                    player.addCard(deckOfCards.getCard(i));
                    i++;
                } else {
                    player.setInGame("stoped");
                }
            }
        }
        dealer.addCard(deckOfCards.getCard(i));
        return ++i;
    }

    public void play() {
        int i = 0;
        for (Player player: players) {
            player.addCard(deckOfCards.getCard(i));
            i++;
        }
        i = dealCards(i);
        for (Player player: players) {
            if (player.sum() == 21) {
                String bj = player.ifBlackJack();
                if (bj.equals("take")) {
                    player.win(2);
                    player.setInGame("finished");
                } else {
                    player.setInGame("waiting");
                }
            }
        }

        boolean first = true;
        while (dealer.sum() < 17) {
            i = dealCards(i);
            if (first && dealer.sum() == 21) {
                for (Player player: players) {
                    if (player.getInGame().equals("waiting")) {
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
                    player.setInGame("finished");
                }
            }
        }

        for (Player player: players) {
            if (player.getInGame().equals("playing") || player.getInGame().equals("stoped")) {
                if (player.sum() > dealer.sum() || dealer.sum() > 21) {
                    player.win(2);
                } else {
                    player.lose(2);
                }
            }
        }
    }
}

