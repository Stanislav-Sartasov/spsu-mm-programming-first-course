package com.company;

import static java.lang.Integer.max;

public class Dealer extends Player {

    public Dealer() {
        super(1000000);
    }

    public int getCard() {
        int ans = 2;
        for (Card card: cards) {
            int cur;
            switch (card.getValue()) {
                case "2" -> cur = 2;
                case "3" -> cur = 3;
                case "4" -> cur = 4;
                case "5" -> cur = 5;
                case "6" -> cur = 6;
                case "7" -> cur = 7;
                case "8" -> cur = 8;
                case "9" -> cur = 9;
                case "10" -> cur = 10;
                case "J", "Q", "K" -> cur = 10;
                case "A" -> cur = 11;
                default -> throw new IllegalStateException("Unexpected value: " + card);
            }
            ans = max(ans, cur);
        }
        return ans;
    }

    @Override
    public String makeMove(int dealersCard) {
        if (this.sum() < 17)
            return "take";
        return "pass";
    }

    @Override
    public String ifBlackJack() {
        return null;
    }
}
