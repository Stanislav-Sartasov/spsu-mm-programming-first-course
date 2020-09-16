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
            if (player.getInGame() == State.PLAYING) {
                if (player.makeMove(dealer.getCard()) == Action.TAKE) {
                    player.addCard(deckOfCards.getCard(i));
                    i++;
                } else {
                    player.setInGame(State.STOPPED);
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
                Action bj = player.ifBlackJack();
                if (bj == Action.TAKE) {
                    player.win(2);
                    player.setInGame(State.FINISHED);
                } else {
                    player.setInGame(State.WAITING);
                }
            }
        }

        boolean first = true;
        while (dealer.sum() < 17) {
            i = dealCards(i);
            if (first && dealer.sum() == 21) {
                for (Player player: players) {
                    if (player.getInGame() == State.WAITING) {
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
                    player.setInGame(State.FINISHED);
                }
            }
        }

        for (Player player: players) {
            if (player.getInGame() == State.PLAYING || player.getInGame() == State.STOPPED) {
                if (player.sum() > dealer.sum() || dealer.sum() > 21) {
                    player.win(2);
                } else {
                    player.lose(2);
                }
            }
        }
    }
}

