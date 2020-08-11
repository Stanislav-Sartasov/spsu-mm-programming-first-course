package com.company.player;

import com.company.cards.Card;

import java.util.ArrayList;

public abstract class Player {
    protected int money;
    protected ArrayList<Card> cards = new ArrayList<>();
    protected String inGame = "playing";

    public String getInGame() {
        return inGame;
    }

    public Player(int x) {
        money = x;
    }

    public ArrayList<Card> getCards() {
        return cards;
    }

    public int getMoney() {
        return money;
    }

    protected boolean haveA() {
        boolean ans = false;
        for (Card card: cards) {
            if (card.getValue().equals("A")) {
                ans = true;
                break;
            }
        }
        return ans;
    }

    public void addCard(Card c) {
        cards.add(c);
    }

    public void win(int x) {
        money += x;
    }

    public void lose(int x) {
        money -= x;
    }

    public abstract String makeMove(int dealersCard);

    public abstract String ifBlackJack();

    public int sum() {
        int s = 0;
        int cnt = 0;
        for (Card card: cards) {
            switch (card.getValue()) {
                case "2" -> s += 2;
                case "3" -> s += 3;
                case "4" -> s += 4;
                case "5" -> s += 5;
                case "6" -> s += 6;
                case "7" -> s += 7;
                case "8" -> s += 8;
                case "9" -> s += 9;
                case "10" -> s += 10;
                case "J", "Q", "K" -> s += 10;
                case "A" -> {
                    s += 11;
                    cnt += 1;
                }
                default -> throw new IllegalStateException("Unexpected value: " + card);
            }
        }
        while (s > 21 && cnt > 0) {
            s -= 10;
            cnt -= 1;
        }
        return s;
    }

    public void setInGame(String s) {
        inGame = s;
    }
}

