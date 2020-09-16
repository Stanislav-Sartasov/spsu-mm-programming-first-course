package com.company.player;

import com.company.cards.Card;

import static java.lang.Integer.max;

public class Dealer extends Player {

    public Dealer() {
        super(1000000);
    }

    public int getCard() {
        int ans = 2;
        for (Card card: cards) {
            ans = max(ans, card.getIntValue());
        }
        return ans;
    }

    @Override
    public Action makeMove(int dealersCard) {
        if (this.sum() < 17)
            return Action.TAKE;
        return Action.PASS;
    }

    @Override
    public Action ifBlackJack() {
        return null;
    }
}
