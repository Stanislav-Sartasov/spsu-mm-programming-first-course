package com.company.player;

import com.company.cards.Card;
import com.company.cards.Values;

import java.util.ArrayList;


public abstract class Player {
    protected int money;
    protected ArrayList<Card> cards = new ArrayList<>();
    protected State inGame = State.PLAYING;

    public State getInGame() {
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
            if (card.getValue() == Values.ACE) {
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

    public abstract Action makeMove(int dealersCard);

    public abstract Action ifBlackJack();

    public int sum() {
        int s = 0;
        int cnt = 0;
        for (Card card: cards) {
            if (card.getIntValue() != 11) {
                s += card.getIntValue();
            } else {
                s += 11;
                cnt++;
            }
        }
        while (s > 21 && cnt > 0) {
            s -= 10;
            cnt -= 1;
        }
        return s;
    }

    public void setInGame(State s) {
        inGame = s;
    }
}

